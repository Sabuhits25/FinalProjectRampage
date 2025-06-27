using Rampage.Core.Entities;
using Rampage.Core.Entities.Identity;

namespace Rampage.BLL.Services.Interfaces
{
    public interface IMailService
    {
        Task SendConfirmationCodeMessageAsync(int number, string toUser, User user);
        Task SendMessageToSubscribersAsync(string toUser, User? user, Product newProduct);
    }
}
