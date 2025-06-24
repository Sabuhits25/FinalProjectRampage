using FluentValidation;
using Rampage.BLL.DTO_s.CategoryDTO_s;

namespace Rampage.BLL.Validators.CategoryValidators
{
    public class CreateCategoryTranslationDTOValidator : AbstractValidator<CreateCategoryTranslationDTO>
    {
        public CreateCategoryTranslationDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Category language is required.");
        }
    }
}
