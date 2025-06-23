using CloudinaryDotNet;
using Microsoft.Extensions.DependencyInjection;
using Rampage.BLL.Helpers;
using Rampage.BLL.Services.Abstraction;
using Rampage.BLL.Services.Interfaces;
using Rampage.BLL.Services.InternalServices.Abstraction;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.DAL.Services;
using Rampage.DAL.Services.Interface;

namespace Rampage.BLL
{
    public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddServices();
            services.RegisterAutoMapper();
            services.AddSingleton(new Cloudinary(new Account("dglztqnyd", "227273655514776", "g2-cpYCqwyaj-VSwN_K3sRoKDwI")));


            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();

            // External Services 

            services.AddScoped<IFileManagerService, FileManagerService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<HttpClient>();
            services.AddScoped<BankClient>();

            // Internal Services

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        private static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BusinessDependencyInjection));
        }
    }
}
