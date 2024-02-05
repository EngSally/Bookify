using Bookify.Web.Core.ViewModels.Users;

namespace Bookify.Web.Validators
{
    public class RestPasswordValidator:AbstractValidator<RestPasswordViewModel>
    {
        public RestPasswordValidator()
        {
            RuleFor(e => e.Password).Length(8, 100).WithMessage(Errors.MaxMinLength)
                               .Matches(RegexPatterns.Password).WithMessage(Errors.WeakPassword);

            RuleFor(e => e.ConfirmPassword).Equal(e => e.Password).WithMessage(Errors.ConfirmPasswordNotMatch);
        }
    }
}
