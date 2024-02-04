using Bookify.Web.Core.ViewModels.Subscribers;

namespace Bookify.Web.Validators
{
    public class SubscriberValidator : AbstractValidator<SubscriberFormViewModel>
    {
        public SubscriberValidator()
        {
            RuleFor(x => x.FristName)
                .MaximumLength(100).WithMessage(Errors.MaxLength)
                .Matches(RegexPatterns.CharactersOnly_Eng).WithMessage(Errors.OnlyEnglishLetters);

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage(Errors.MaxLength)
                .Matches(RegexPatterns.CharactersOnly_Eng).WithMessage(Errors.OnlyEnglishLetters);

            RuleFor(x => x.NationalId)
                .MaximumLength(14).WithMessage(Errors.MaxLength)
                .Matches(RegexPatterns.NationalId).WithMessage(Errors.InvalidNatialId);

            RuleFor(x => x.MobilNum)
                .MaximumLength(11).WithMessage(Errors.MaxLength)
                .Matches(RegexPatterns.MobileNumber).WithMessage(Errors.InvalidMobileNumber);

            RuleFor(x => x.Email)
                .MaximumLength(150).WithMessage(Errors.MaxLength)
                .EmailAddress();

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage(Errors.MaxLength);
        }
    }
}
