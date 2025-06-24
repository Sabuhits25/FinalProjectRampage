using FluentValidation;
using Rampage.BLL.DTO_s.ColorDTO_s;

namespace Rampage.BLL.Validators.ColorValidators
{
    public class CreateColorTranslationDTOValidator : AbstractValidator<CreateColorTranslationDTO>
    {
        public CreateColorTranslationDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Category language is required.");
        }
    }
}
