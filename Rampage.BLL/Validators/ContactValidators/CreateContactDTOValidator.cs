using FluentValidation;
using Rampage.BLL.DTO_s.ContactDTO_s;

namespace Rampage.BLL.Validators.ContactValidators
{
    public class CreateContactDTOValidator : AbstractValidator<CreateContactDTO>
    {
        public CreateContactDTOValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Contact full name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Contact email is required.")
                .EmailAddress().WithMessage("Email address should be valid.");

            RuleFor(x => x.Message)
               .NotEmpty().WithMessage("Contact message is required.");
        }
    }
}
