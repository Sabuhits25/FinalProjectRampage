using Microsoft.Extensions.Configuration;
using Rampage.BLL.Services.Interfaces;
using Rampage.Core.Entities.Identity;
using Rampage.Core.Models;
using System.Net;
using System.Net.Mail;

namespace Rampage.BLL.Services.Abstraction
{
    public class MailService : IMailService
    {
        protected readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailMessageAsync(int? confirmationNumber, string toUser,
            User? currentUser, string cases, Product? product)
        {
            var lang = "AZ"; //new LanguageCatcher(_http).GetLanguage();

            var bodyHtmlScript = string.Empty;

            switch (cases)
            {
                case "Auth":
                    bodyHtmlScript = BodyMessageGenerator(lang, currentUser, confirmationNumber);
                    break;
                case "Subs":
                    bodyHtmlScript = NewProductBodyMessage(lang, toUser, currentUser, product);
                    break;
            }

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("bendiyevf@gmail.com", "gbmb biwz ehim aspn");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("bendiyevf@gmail.com"),
                    Subject = lang switch
                    {
                        "AZ" => "Rapmpage-ə xoş gəlmisiniz",
                        "EN" or _ => "Welcome to Rampage"
                    },
                    Body = bodyHtmlScript,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toUser);
                await client.SendMailAsync(mailMessage);
            }
        }

        public string BodyMessageGenerator(string lang, User user, int? confNum)
        {
            switch (lang)
            {
                case "AZ":
                    return $@"<!DOCTYPE html>
                            <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: 'Arial', sans-serif;
                                        background-color: #f4f4f4;
                                        margin: 0;
                                        padding: 20px;
                                    }}
                                    .container {{
                                        max-width: 600px;
                                        margin: auto;
                                        background-color: #ffffff;
                                        border-radius: 5px;
                                        box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                                        padding: 20px;
                                        text-align: center;
                                    }}
                                    h1 {{
                                        color: #333;
                                    }}
                                    p {{
                                        color: #666;
                                        font-size: 16px;
                                        line-height: 1.6;
                                    }}
                                    .qr-code {{
                                        margin: 20px auto;
                                        padding: 15px;
                                        background-color: #f9f9f9;
                                        border-radius: 5px;
                                        display: inline-block;
                                    }}
                                    .qr-code img {{
                                        width: 140px;
                                        height: 140px;
                                    }}
                                    .hospital-info {{
                                        margin-top: 30px;
                                        border-top: 1px solid #ccc;
                                        padding-top: 20px;
                                        text-align: left;
                                    }}
                                    .hospital-info h5 {{
                                        color: #333;
                                        margin-bottom: 10px;
                                    }}
                                    .hospital-info h6 {{
                                        color: #666;
                                        margin: 5px 0;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <h1>Rampage xoş gəlmisiniz, {user.FirstName} {user.LastName}!</h1>
                                    <div style='font-family: Space Grotesk, Arial, sans-serif; font-weight: 700; font-size: 32px; letter-spacing: 4px; margin: 40px 0; color: rgba(0, 0, 0, 0.87); background-color: #f7f7f7; padding: 15px 30px; border-radius: 8px; display: inline-block; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);'>
                                    {confNum}
                                    </div>
                                    <p>Hər hansı bir sualınız varsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p>                                    
                                    <div class='hospital-info'>
                                        <h5>Şirkət Məlumatı:</h5>
                                        <h6><strong>Ünvan:</strong> Address</h6>
                                        <h6><strong>E-poçt:</strong> example@gmail.com</h6>
                                        <h6><strong>Telefon:</strong> +994 50 500 00 00</h6>
                                    </div>
                                </div>
                            </body>
                            </html>";

                case "EN":
                    return $@"<!DOCTYPE html>
                            <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: 'Arial', sans-serif;
                                        background-color: #f4f4f4;
                                        margin: 0;
                                        padding: 20px;
                                    }}
                                    .container {{
                                        max-width: 600px;
                                        margin: auto;
                                        background-color: #ffffff;
                                        border-radius: 5px;
                                        box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                                        padding: 20px;
                                        text-align: center;
                                    }}
                                    h1 {{
                                        color: #333;
                                    }}
                                    p {{
                                        color: #666;
                                        font-size: 16px;
                                        line-height: 1.6;
                                    }}
                                    .qr-code {{
                                        margin: 20px auto;
                                        padding: 15px;
                                        background-color: #f9f9f9;
                                        border-radius: 5px;
                                        display: inline-block;
                                    }}
                                    .qr-code img {{
                                        width: 140px;
                                        height: 140px;
                                    }}
                                    .hospital-info {{
                                        margin-top: 30px;
                                        border-top: 1px solid #ccc;
                                        padding-top: 20px;
                                        text-align: left;
                                    }}
                                    .hospital-info h5 {{
                                        color: #333;
                                        margin-bottom: 10px;
                                    }}
                                    .hospital-info h6 {{
                                        color: #666;
                                        margin: 5px 0;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <h1>Welcome to Rampage, {user.FirstName} {user.LastName}!</h1>
                                    <div style='font-family: Space Grotesk, Arial, sans-serif; font-weight: 700; font-size: 32px; letter-spacing: 4px; margin: 40px 0; color: rgba(0, 0, 0, 0.87); background-color: #f7f7f7; padding: 15px 30px; border-radius: 8px; display: inline-block; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);'>
                                    {confNum}
                                    </div>
                                    <p>If you have any questions, don't hesitate to contact us.</p>                                    
                                    <div class='hospital-info'>
                                        <h5>Company Information:</h5>
                                        <h6><strong>Location:</strong> Address</h6>
                                        <h6><strong>Email:</strong> example@gmail.com</h6>
                                        <h6><strong>Phone:</strong> +994 50 500 00 00</h6>
                                    </div>
                                </div>
                            </body>
                            </html>";

                default:
                    return string.Empty;
            }
        }

        public string NewProductBodyMessage(string lang, string email, User? user, Product product)
        {
            string greeting = user != null
                ? $"Welcome, {user.FirstName} {user.LastName}!"
                : $"Hello, valued subscriber!";

            string message = lang switch
            {
                "AZ" => $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Arial', sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 20px;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: auto;
                                background-color: #ffffff;
                                border-radius: 5px;
                                box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                                padding: 20px;
                                text-align: center;
                            }}
                            h1 {{ color: #333; }}
                            p {{ color: #666; font-size: 16px; line-height: 1.6; }}
                            .product-info {{ margin: 20px auto; padding: 15px; background-color: #f9f9f9; border-radius: 5px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1>{greeting}</h1>
                            <p>Bizim yeni məhsulumuz artıq mövcuddur! Sizin üçün əla bir fürsətdir.</p>
                            <div class='product-info'>
                                <h3>{product.Code} - {product.Star} ★</h3>
                                <p><strong>Qiymət:</strong> {product.Price} AZN</p>
                                <p><strong>Kod:</strong> {product.BarCode}</p>
                                <p><strong>Kategoriya:</strong> ... </p>
                                <a href='{_configuration["FrontUrl"]}{lang}/Shop/Component/{product.Id}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 10px 20px; border-radius: 5px;'>Məhsula bax</a>
                            </div>
                            <p>Əlavə suallar üçün bizimlə əlaqə saxlayın.</p>
                        </div>
                    </body>
                    </html>",

                "EN" => $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Arial', sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 20px;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: auto;
                                background-color: #ffffff;
                                border-radius: 5px;
                                box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                                padding: 20px;
                                text-align: center;
                            }}
                            h1 {{ color: #333; }}
                            p {{ color: #666; font-size: 16px; line-height: 1.6; }}
                            .product-info {{ margin: 20px auto; padding: 15px; background-color: #f9f9f9; border-radius: 5px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1>{greeting}</h1>
                            <p>Our new product is now available! This is a great opportunity for you.</p>
                            <div class='product-info'>
                                <h3>{product.Code} - {product.Star} ★</h3>
                                <p><strong>Price:</strong> {product.Price} USD</p>
                                <p><strong>Code:</strong> {product.BarCode}</p>
                                <p><strong>Category:</strong> ... </p>
                                <a href='{_configuration["FrontUrl"]}{lang}/Shop/Component/{product.Id}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 10px 20px; border-radius: 5px;'>View Product</a>
                            </div>
                            <p>Feel free to contact us for any questions.</p>
                        </div>
                    </body>
                    </html>",

                _ => string.Empty
            };

            return message;
        }


        public async Task SendConfirmationCodeMessageAsync(int number, string toUser, User user)
        {
            await SendMailMessageAsync(
                    confirmationNumber: number,
                    toUser: toUser,
                    currentUser: user,
                    cases: "Auth",
                    product: null
                );
        }

        public async Task SendMessageToSubscribersAsync(string toUser, User? user, Product newProduct)
        {
            await SendMailMessageAsync(
                   confirmationNumber: null,
                   toUser: toUser,
                   currentUser: user,
                   cases: "Subs",
                   product: newProduct
               );
        }
    }
}
