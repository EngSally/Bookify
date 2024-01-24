

namespace Bookify.Web.Validators
{
    public class AuthorsFormValidator: AbstractValidator<AuthorsFormViewModel>
    {

        public AuthorsFormValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).WithMessage(Errors.MaxLength)
                .Matches(RegexPatterns.CharactersOnly_Eng).WithMessage(Errors.OnlyEnglishLetters);

        }
    }
}
