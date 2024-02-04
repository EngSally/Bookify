using Bookify.Web.Core.ViewModels.Categories;

namespace Bookify.Web.Validators
{
    public class CategoriesFormValidator: AbstractValidator<CategoriesFormViewModel>
    {
        public CategoriesFormValidator()
        {
            RuleFor(e => e.Name).MaximumLength(100).WithMessage(Errors.MaxLength)
                .Matches(RegexPatterns.CharactersOnly_Eng).WithMessage(Errors.OnlyEnglishLetters);
        }
    }
}
