using FluentValidation;
using Rampage.BLL.DTO_s.AuthenticationDTO_s;

namespace Rampage.BLL.Validators.AuthenticationValidators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("User firstName is required.")
                .MinimumLength(3)
                .WithMessage("User firstName must be at least 3 characters long.");

            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("User lastname is required.")
                .MinimumLength(3)
                .WithMessage("User lastname must be at least 3 characters long.");

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one numeric digit.");

            RuleFor(x => x)
                .Must(x => x.Password == x.ConfirmPassword).WithMessage("Passwords must match.");
        }
    }
}
