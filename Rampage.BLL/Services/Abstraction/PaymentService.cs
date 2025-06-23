using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Rampage.BLL.DTO_s.TransactionDTO_s;
using Rampage.BLL.Helpers;
using Rampage.BLL.Services.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


namespace Rampage.BLL.Services.Abstraction
{
    public class PaymentService : IPaymentService
    {
        protected readonly IConfiguration _config;
        protected readonly ITransactionRepository _transactionRepository;
        protected readonly IHttpContextAccessor _http;
        private readonly BankClient _client;

        public PaymentService(IConfiguration config,
            ITransactionRepository transactionRepository,
            IHttpContextAccessor http, BankClient client)
        {
            _config = config;
            _transactionRepository = transactionRepository;
            _http = http;
            _client = client;
        }

        public async Task<HttpResponseMessage> CreateOrderAsync(decimal amount,
            string description, string lang, string checkToken)
        {
            var frontUrl = _config["FrontUrl"];

            var order = new
            {
                order = new
                {
                    typeRid = "Order_SMS",
                    amount = amount.ToString("F2"),
                    currency = "AZN",
                    language = lang,
                    description = description,
                    hppRedirectUrl = $"{frontUrl}{lang}/Basket/HandlePayment?token={checkToken}",
                    hppCofCapturePurposes = new[] { "Cit" }
                }
            };

            var json = JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("order/", content);

            return response;
        }

        public async Task<string> GetOrderInformationAsync(string orderId)
        {
            var response = await _client.GetAsync($"https://txpgtst.kapitalbank.az/api/order/{orderId}");

            return response;
        }


        public async Task<ResponseTransactionDTO> PurchaseAsync(CreateTransactionDTO dto)
        {
            var checkToken = GeneratePaymentCheckToken();
            var orderDescription = "BuyDescription";
            var userId = dto.UserId;
            var price = dto.Amount;

            var response = await CreateOrderAsync(price, orderDescription, dto.Lang, checkToken);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseBody);

            var orderIdFromResponse = responseJson["order"]?["id"]?.ToString();
            var sessionIdFromResponse = responseJson["order"]?["password"]?.ToString();

            var newTransaction = new Transaction
            {
                UserId = userId,
                CheckToken = checkToken,
                OrderId = orderIdFromResponse,
                SessionId = sessionIdFromResponse,
                Status = EOrderStatus.ONPAYMENT,
                ResponseBody = responseBody,
                Amount = price,
                FullName = dto.FullName,
                Address = dto.Address,
                City = dto.City,
                Phone = dto.Phone,
                Email = dto.Email,
            };

            await _transactionRepository.AddAsync(newTransaction);

            return new ResponseTransactionDTO
            {
                Url = $"{responseJson["order"]?["hppUrl"]?.ToString()}?id={orderIdFromResponse}&password={sessionIdFromResponse}",
            };
        }

        public async Task<EOrderStatus> PaymentHandlerAsync(string checkToken)
        {
            var oldTransaction = await _transactionRepository.GetByIdAsync(x => x.CheckToken == checkToken);

            var data = await GetOrderInformationAsync(oldTransaction.OrderId.ToString());
            var responseData = JObject.Parse(data);

            EOrderStatus orderStatus = Enum.Parse<EOrderStatus>(
                responseData["order"]?["status"]?.ToString(), true);

            oldTransaction.Status = orderStatus;

            await _transactionRepository.UpdateAsync(oldTransaction);

            return orderStatus;
        }

        public string GeneratePaymentCheckToken()
        {
            byte[] numberBytes = BitConverter.GetBytes(DateTime.Now.Ticks);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(numberBytes);

                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }
        }
    }
}
