using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rampage.BLL.DTO_s.BasketDTO_s;
using Rampage.BLL.DTO_s.TransactionDTO_s;
using Rampage.BLL.Services.Interfaces;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Identity;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;
using Rampage.DAL.Services.Interface;

namespace Rampage.Controllers
{
    public class BasketController : Controller
    {
        protected readonly IProductService _productService;
        protected readonly IBasketRepository _basketRepository;
        protected readonly IPaymentService _paymentService;
        protected readonly IClaimService _claimService;
        protected readonly UserManager<User> _userManager;

        public BasketController(IProductService productService, IBasketRepository basketRepository, UserManager<User> userManager, IClaimService claimService, IPaymentService paymentService)
        {
            _productService = productService;
            _basketRepository = basketRepository;
            _userManager = userManager;
            _claimService = claimService;
            _paymentService = paymentService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            var basketDto = new BasketDTO
            {
                CreatedOn = DateTime.UtcNow,
                IsGuest = !User.Identity.IsAuthenticated
            };

            if (!User.Identity.IsAuthenticated)
            {
                var basketProductsCookie = Request.Cookies["BasketProducts"];

                if (!string.IsNullOrEmpty(basketProductsCookie))
                {
                    var productIds = basketProductsCookie.Split(',').Select(int.Parse).ToList();

                    basketDto.BasketItems = (await _productService.GetProductsByTheirIdsAsync(productIds))
                    .Select(p => new BasketItemDTO
                    {
                        ProductId = p.Id,
                        Name = p.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                               p.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name,
                        Price = p.Price,
                        Quantity = productIds.Count(b => b == p.Id),
                        ImageUrl = p.Images.FirstOrDefault()?.ImageUrl
                    }).ToList();

                    basketDto.TotalPrice = CalculateTotalPrice(basketDto.BasketItems.ToList());
                }
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                basketDto.UserId = user.Id;

                var basketItems = await _basketRepository.GetAllAsync(b => b.UserId == user.Id && !b.IsDeleted);
                var productIds = basketItems.Select(b => b.ProductId).ToList();

                basketDto.BasketItems = (await _productService.GetProductsByTheirIdsAsync(productIds))
                    .Select(p => new BasketItemDTO
                    {
                        ProductId = p.Id,
                        Name = p.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                               p.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name,
                        Price = p.Price,
                        Quantity = basketItems.Count(b => b.ProductId == p.Id),
                        ImageUrl = p.Images.FirstOrDefault()?.ImageUrl
                    }).ToList();

                basketDto.TotalPrice = CalculateTotalPrice(basketDto.BasketItems.ToList());
            }

            return View(basketDto);
        }

        [HttpPost]
        public IActionResult Index(BasketDTO dto)
        {
            if (dto.BasketItems == null || !dto.BasketItems.Any())
            {
                ModelState.AddModelError("", "Səbətiniz boşdur!");
                return View(dto);
            }

            var basketJson = JsonConvert.SerializeObject(dto);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1), // Cookie müddəti 1 gün
                HttpOnly = true,
                Secure = true
            };

            HttpContext.Response.Cookies.Append("BasketData", basketJson, cookieOptions);

            return RedirectToAction(nameof(Transformation));
        }

        [HttpGet]
        public async Task<IActionResult> Transformation()
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            if (!HttpContext.Request.Cookies.TryGetValue("BasketData", out var basketJson)
                || string.IsNullOrEmpty(basketJson))
            {
                return RedirectToAction("Index");
            }

            var basketDTO = JsonConvert.DeserializeObject<BasketDTO>(basketJson);
            var productsPrice = basketDTO.ProductsPrice;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var createTransactionDTO = new CreateTransactionDTO
                {
                    UserId = user.Id,
                    Address = user.Address,
                    Amount = basketDTO.TotalPrice,
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    City = user.City,
                    FullName = $"{user.FirstName} {user.LastName}",
                    ProductsPrice = productsPrice,
                    Lang = lang
                };

                return View(createTransactionDTO);
            }
            else
            {
                return View(new CreateTransactionDTO
                {
                    Amount = basketDTO.TotalPrice,
                    ProductsPrice = productsPrice,
                    Lang = lang
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transformation(CreateTransactionDTO dto)
        {
            var responseDTO = await _paymentService.PurchaseAsync(dto);

            if (!User.Identity.IsAuthenticated)
            {
                Response.Cookies.Delete("BasketProducts");
                Response.Cookies.Delete("BasketData");
            }
            else
            {
                var user = await _userManager.Users
                    .Include(x => x.Baskets).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                foreach (var item in user.Baskets)
                {
                    await _basketRepository.RemoveAsync(item);
                }
            }

            return Redirect(responseDTO.Url);
        }

        [HttpGet]
        public async Task<IActionResult> HandlePayment(string token)
        {
            if (token is not null)
            {
                var status = await _paymentService.PaymentHandlerAsync(token);

                if (status == EOrderStatus.FULLYPAID) TempData["Status"] = "Success";
                else TempData["Status"] = "Fail";

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddProduct(int productId, string returnUrl)
        {
            var product = await _productService.GetByIdAsync(productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (!User.Identity.IsAuthenticated)
            {
                AddProductToCookie(productId);
            }
            else
            {
                await AddProductToDatabase(productId);
            }

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnUrl);
        }



        [HttpGet]
        public async Task<IActionResult> GetBasketCount()
        {
            var currentUserId = _claimService.GetUserId();
            int basketItemsCount = 0;

            if (currentUserId is not null)
            {
                basketItemsCount = (await _basketRepository.GetAllAsync(x => x.UserId == currentUserId && !x.IsDeleted)).Count();
            }
            else
            {
                var basketProductsCookie = HttpContext.Request.Cookies["BasketProducts"];
                var basketProducts = string.IsNullOrEmpty(basketProductsCookie)
                    ? new List<int>()
                    : basketProductsCookie.Split(',').Select(int.Parse).ToList();

                basketItemsCount = basketProducts.Count;
            }

            return Json(new { count = basketItemsCount });
        }


        [HttpGet]
        public async Task<IActionResult> GetBasketProducts()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var basketProductsCookie = Request.Cookies["BasketProducts"];
                if (!string.IsNullOrEmpty(basketProductsCookie))
                {
                    var productIds = basketProductsCookie.Split(',').Select(int.Parse).ToList();
                    var products = await _productService.GetProductsByTheirIdsAsync(productIds);
                    return View(products);
                }
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                var basketProducts = await _basketRepository.GetAllAsync(
                    b => b.UserId == user.Id && !b.IsDeleted);

                return View(basketProducts);
            }

            return View(new List<Product>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] BasketItemDTO basketItem)
        {
            if (basketItem == null || basketItem.ProductId <= 0 || basketItem.Quantity <= 0)
            {
                return Json(new { success = false, message = "Invalid product or quantity" });
            }

            if (!User.Identity.IsAuthenticated)
            {
                var basketProductsCookie = Request.Cookies["BasketProducts"];
                var productIds = string.IsNullOrEmpty(basketProductsCookie)
                    ? new List<int>()
                    : basketProductsCookie.Split(',').Select(int.Parse).ToList();

                if (basketItem.IncreaseOrDecrease == true)
                {
                    productIds.Add(basketItem.ProductId);
                }
                else
                {
                    var existingItem = productIds.FirstOrDefault(id => id == basketItem.ProductId);
                    if (existingItem > 0)
                    {
                        productIds.Remove(existingItem);
                    }
                }

                Response.Cookies.Append("BasketProducts", string.Join(",", productIds), new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                });
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                if (basketItem.IncreaseOrDecrease == true)
                {
                    var newBasketItem = new Basket
                    {
                        UserId = user.Id,
                        ProductId = basketItem.ProductId,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _basketRepository.AddAsync(newBasketItem);
                }
                else
                {
                    var existingItem = await _basketRepository.GetAllAsync(x => x.UserId == user.Id && x.ProductId == basketItem.ProductId && !x.IsDeleted);
                    if (existingItem.Any())
                    {
                        var itemToRemove = existingItem.First();
                        await _basketRepository.DeleteAsync(itemToRemove);
                    }
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProduct([FromBody] BasketItemDTO basketItem)
        {
            var productId = basketItem.ProductId;

            if (!User.Identity.IsAuthenticated)
            {
                var basketProductsCookie = Request.Cookies["BasketProducts"];
                if (!string.IsNullOrEmpty(basketProductsCookie))
                {
                    var productIds = basketProductsCookie.Split(',').Select(int.Parse).ToList();
                    productIds.RemoveAll(p => p == productId);

                    Response.Cookies.Append("BasketProducts", string.Join(",", productIds), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(30),
                        HttpOnly = true,
                        Secure = true
                    });
                }
            }
            else
            {
                var baskets = await _basketRepository.GetAllAsync(x =>
                x.ProductId == productId && !x.IsDeleted);

                foreach (var item in baskets)
                {
                    await _basketRepository.DeleteAsync(item);
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ClearBasket()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Cookies.Delete("BasketProducts");
            }
            else
            {
                var user = await _userManager.Users
                    .Include(x => x.Baskets).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                foreach (var item in user.Baskets)
                {
                    await _basketRepository.DeleteAsync(item);
                }
            }

            return Json(new { success = true });
        }

        // Support Methods
        private decimal CalculateTotalPrice(List<BasketItemDTO> basketItems)
        {
            return basketItems.Sum(item => item.Price * item.Quantity);
        }

        private void AddProductToCookie(int productId)
        {
            var basketProductsCookie = Request.Cookies["BasketProducts"];
            var basketProducts = string.IsNullOrEmpty(basketProductsCookie)
                ? new List<int>()
                : basketProductsCookie.Split(',').Select(int.Parse).ToList();

            if (!basketProducts.Contains(productId))
            {
                basketProducts.Add(productId);

                var cookieValue = string.Join(",", basketProducts);

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };

                Response.Cookies.Append("BasketProducts", cookieValue, cookieOptions);
            }
        }

        private async Task AddProductToDatabase(int productId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var basketItem = new Basket
            {
                UserId = user.Id,
                ProductId = productId,
            };

            await _basketRepository.AddAsync(basketItem);
        }

        private async Task<List<BasketItemDTO>> GetProductsFromCookies(List<int> productIds, string lang)
        {
            var products = await _productService.GetProductsByTheirIdsAsync(productIds);

            var productQuantities = productIds.GroupBy(id => id)
                                              .ToDictionary(g => g.Key, g => g.Count());

            return products.Select(p => new BasketItemDTO
            {
                ProductId = p.Id,
                Name = p.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                       p.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name,
                Price = p.Price,
                Quantity = productQuantities.ContainsKey(p.Id) ? productQuantities[p.Id] : 1, // Get quantity from cookie count
                ImageUrl = p.Images.FirstOrDefault()?.ImageUrl
            }).ToList();
        }
    }
}
