using FluentValidation;
using Rampage.BLL.DTO_s.BlogDTO_s;

namespace Rampage.BLL.Validators.BlogVaidators
{
    public class UpdateBlogDTOValidator : AbstractValidator<UpdateBlogDTO>
    {
        public UpdateBlogDTOValidator()
        {
            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Category author is required.");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Category image is required.");
        }
    }
}
