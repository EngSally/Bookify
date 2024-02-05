using Bookify.Web.Core.ViewModels.Users;

namespace Bookify.Web.Validators
{
    public class UserFormValidation:AbstractValidator<UserFormViewModel>
    {
        public UserFormValidation()
        {
            RuleFor(e => e.FullName).MaximumLength(100).WithMessage(Errors.MaxLength)
            .Matches(RegexPatterns.CharactersOnly_Eng).WithMessage(Errors.OnlyEnglishLetters);
          
            RuleFor(e => e.UserName).MaximumLength(100).WithMessage(Errors.MaxLength)
            .Matches(RegexPatterns.Username).WithMessage(Errors.InvalidUsername);
          
            RuleFor(e => e.Email).EmailAddress().MaximumLength(100).WithMessage(Errors.MaxLength);
          
            RuleFor(e => e.Password).Length(8,100).WithMessage(Errors.MaxMinLength)
                                .Matches(RegexPatterns.Password).WithMessage(Errors.WeakPassword);

            RuleFor(e => e.ConfirmPassword).Equal(e => e.Password).WithMessage(Errors.ConfirmPasswordNotMatch);
        }
    }
}
