using FluentValidation;
using Rampage.BLL.DTO_s.BlogDTO_s;

namespace Rampage.BLL.Validators.BlogVaidators
{
    public class UpdateBlogTranslationDTOValidator : AbstractValidator<UpdateBlogTranslationDTO>
    {
        public UpdateBlogTranslationDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Category author is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Category image is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Category language is required.");
        }
    }
}
