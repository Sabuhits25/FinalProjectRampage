using FluentValidation;
using Rampage.BLL.DTO_s.CategoryDTO_s;
using Rampage.BLL.Validators.Commons;

namespace Rampage.BLL.Validators.CategoryValidators
{
    public class UpdateCategoryTranslationDTOValidator : BaseEntityValidator<UpdateCategoryTranslationDTO>
    {
        public UpdateCategoryTranslationDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Category language is required.");
        }
    }
}
