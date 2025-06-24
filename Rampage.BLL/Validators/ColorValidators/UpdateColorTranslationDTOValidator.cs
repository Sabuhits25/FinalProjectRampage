using FluentValidation;
using Rampage.BLL.DTO_s.ColorDTO_s;
using Rampage.BLL.Validators.Commons;

namespace Rampage.BLL.Validators.ColorValidators
{
    public class UpdateColorTranslationDTOValidator : BaseEntityValidator<UpdateColorTranslationDTO>
    {
        public UpdateColorTranslationDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Category language is required.");
        }
    }
}
