using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rampage.BLL.DTO_s.ProductDTO_s;
using Rampage.BLL.Services.Interfaces;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Entities.Identity;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class ProductService : IProductService
    {
        protected readonly IProductTranslationRepository _productTranslationRepository;
        protected readonly IProductSettingRepository _productSettingRepository;
        protected readonly ISubscriptionRepository _subscriptionRepository;
        protected readonly IProductColorRepository _productColorRepository;
        protected readonly IProductImageRepository _productImageRepository;
        protected readonly IProductRepository _productRepository;
        protected readonly UserManager<User> _userManager;
        protected readonly IMailService _mailService;
        protected readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository,
            IProductColorRepository productColorRepository, IProductImageRepository productImageRepository,
            IMapper mapper, IProductTranslationRepository productTranslationRepository,
            IProductSettingRepository productSettingRepository, ISubscriptionRepository subscriptionRepository,
            IMailService mailService, UserManager<User> userManager)
        {
            _productTranslationRepository = productTranslationRepository;
            _productSettingRepository = productSettingRepository;
            _productColorRepository = productColorRepository;
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _mailService = mailService;
            _userManager = userManager;
        }

        public async Task<Product> CreateAsync(CreateProductDTO dto)
        {
            var createdEntity = await _productRepository.AddAsync(new Product
            {
                Code = dto.Code,
                BarCode = dto.BarCode,
                Star = dto.Star,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
            });

            foreach (var translation in dto.Translations)
            {
                var newProductTranslation = new ProductTranslation
                {
                    ProductId = createdEntity.Id,
                    Name = translation.Name,
                    Description = translation.Description,
                    Language = translation.Language,
                };

                await _productTranslationRepository.AddAsync(newProductTranslation);
            }

            foreach (var item in dto.Images)
            {
                var imageDTO = new CreateProductImageDTO
                {
                    ProductId = createdEntity.Id,
                    Image = item,
                };
                var entityImage = _mapper.Map<ProductImage>(imageDTO);

                await _productImageRepository.AddAsync(entityImage);
            }

            foreach (var item in dto.Settings)
            {
                var newProductSetting = new ProductSetting
                {
                    ProductId = createdEntity.Id,
                    Language = item.Language,
                    Key = item.Key,
                    Value = item.Value
                };

                var createdProductSetting = await _productSettingRepository.AddAsync(newProductSetting);
            }

            foreach (var ColorId in dto.ColorIds)
            {
                var entityColor = new ProductColor
                {
                    ProductId = createdEntity.Id,
                    ColorId = ColorId,
                };

                await _productColorRepository.AddAsync(entityColor);
            }

            await MessageForCreationOfNewProductAsync(createdEntity);

            return createdEntity;
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(
                await _productRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Product>> GetAllAsync(bool statement, int pageIndex, int pageSize)
        {
            var entities = await _productRepository.GetAllAsync(x => statement ? true : !x.IsDeleted,
                pi => pi.Images,
                pc => pc.Colors,
                pt => pt.Translations,
                c => c.Category.Translations);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Product>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(x => x.Id == id,
                pi => pi.Images,
                pc => pc.Colors,
                pt => pt.Translations,
                c => c.Category.Translations,
                s => s.Settings);
        }

        public async Task RecoverAsync(int id)
        {
            await _productRepository.RecoverAsync(
                await _productRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _productRepository.RemoveAsync(
                await _productRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<Product> UpdateAsync(UpdateProductDTO dto)
        {
            var oldEntity = await _productRepository.GetByIdAsync(x => x.Id == dto.Id,
                i => i.Images,
                c => c.Colors);

            oldEntity.Code = dto.Code;
            oldEntity.BarCode = dto.BarCode;
            oldEntity.Star = dto.Star;
            oldEntity.Price = dto.Price;
            oldEntity.CategoryId = dto.CategoryId;

            var updatedEntity = await _productRepository.UpdateAsync(oldEntity);

            foreach (var translation in dto.Translations)
            {
                var oldProductTranslation = await _productTranslationRepository.GetByIdAsync(x => x.Id == translation.Id);

                oldProductTranslation.Name = translation.Name;
                oldProductTranslation.Description = translation.Description;

                await _productTranslationRepository.UpdateAsync(oldProductTranslation);
            }

            if (dto.Images != null)
            {
                foreach (var item in dto.Images)
                {
                    var imageDTO = new UpdateProductImageDTO
                    {
                        ProductId = updatedEntity.Id,
                        Image = item,
                    };
                    var entityImage = _mapper.Map<ProductImage>(imageDTO);

                    await _productImageRepository.AddAsync(entityImage);
                }
            }

            if (!string.IsNullOrEmpty(dto.RemovedProductImageIds))
            {
                var idsToRemove = dto.RemovedProductImageIds.Split(',').Select(int.Parse).ToList();
                var imagesToRemove = oldEntity?.Images?.Where(img => idsToRemove.Contains(img.Id))?.ToList();

                foreach (var image in imagesToRemove)
                {
                    var oldImageUrl = image.ImageUrl;

                    oldEntity?.Images?.Remove(image);
                    await _productImageRepository.RemoveAsync(image);
                }
            }

            foreach (var item in dto.Settings)
            {
                var oldProductSetting = await _productSettingRepository.GetByIdAsync(x => x.Id == item.Id);

                oldProductSetting.Value = item.Value;

                var createdProductSetting = await _productSettingRepository.UpdateAsync(oldProductSetting);
            }

            var existingColors = oldEntity.Colors.Select(c => c.ColorId).ToList();

            var colorsToRemove = existingColors.Except(dto.ColorIds).ToList();
            foreach (var colorId in colorsToRemove)
            {
                var colorToRemove = oldEntity.Colors.FirstOrDefault(c => c.ColorId == colorId);
                if (colorToRemove != null)
                {
                    oldEntity.Colors.Remove(colorToRemove);
                    await _productColorRepository.RemoveAsync(colorToRemove);
                }
            }

            var colorsToAdd = dto.ColorIds.Except(existingColors).ToList();
            foreach (var colorId in colorsToAdd)
            {
                var newColor = new ProductColor
                {
                    ProductId = oldEntity.Id,
                    ColorId = colorId
                };
                await _productColorRepository.AddAsync(newColor);
            }

            var resultEntity = await _productRepository.UpdateAsync(oldEntity);

            return resultEntity;
        }

        // Support Methods

        public async Task<ICollection<Product>> GetProductsByTheirIdsAsync(List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                return new List<Product>();
            }

            return await _productRepository.GetAllAsync(
                p => productIds.Contains(p.Id) && !p.IsDeleted,
                t => t.Translations,
                i => i.Images);
        }

        public async Task MessageForCreationOfNewProductAsync(Product createdProduct)
        {
            var allSubs = await _subscriptionRepository.GetAllAsync(x => !x.IsDeleted);
            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                await _mailService.SendMessageToSubscribersAsync(user.Email, user, createdProduct);
            }

            foreach (var sub in allSubs)
            {
                await _mailService.SendMessageToSubscribersAsync(sub.Email, null, createdProduct);
            }
        }

        public async Task<ICollection<Product>> GetNotDeletedProductsAsync()
        {
            return await _productRepository.GetAllAsync(x => !x.IsDeleted,
                t => t.Translations,
                pi => pi.Images,
                c => c.Category,
                cl => cl.Colors);
        }

        public async Task<ICollection<Product>> GetAllProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetAllAsync(x =>
                !x.IsDeleted && x.CategoryId == categoryId,
                t => t.Translations,
                pi => pi.Images,
                c => c.Category);
        }

        public async Task<ICollection<ProductDTO>> GetAllProductsByTranslationAsync(string lang)
        {
            var products = await _productRepository.GetAllAsync(x => !x.IsDeleted, c => c.Translations);

            var localizedProducts = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                       p.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name,
                Description = p.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Description ??
                       p.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Description,
                ImageUrl = p.Images.FirstOrDefault().ImageUrl,
                Star = p.Star,
                Price = p.Price,
            }).ToList();

            return localizedProducts;
        }

        public async Task<int> GetProductIdBySearchAsync(string code)
        {
            var product = await _productRepository.GetByIdAsync(x => x.Code == code);

            return product is not null ? product.Id : 0;
        }
    }
}
