using Rampage.BLL.DTO_s.TransactionDTO_s;
using Rampage.Core.Enums;

namespace Rampage.BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        // Bank Operations
        Task<HttpResponseMessage> CreateOrderAsync(decimal amount,
            string description, string lang, string checkToken);
        Task<string> GetOrderInformationAsync(string orderId);

        // Internal Payment Operations
        Task<ResponseTransactionDTO> PurchaseAsync(CreateTransactionDTO dto);
        Task<EOrderStatus> PaymentHandlerAsync(string checkToken);
    }
}
