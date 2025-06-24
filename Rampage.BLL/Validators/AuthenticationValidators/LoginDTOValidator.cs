using FluentValidation;
using Rampage.BLL.DTO_s.AuthenticationDTO_s;

namespace Rampage.BLL.Validators.AuthenticationValidators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Please enter username or email address!");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Please enter your password!");
        }
    }
}
