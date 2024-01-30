using Bookify.Web.Core.ViewModels.BookCopy;

namespace Bookify.Web.Validators
{
    public class BookCopyFormValidatores:AbstractValidator<BookCopyFormViewModel>
    {
        public BookCopyFormValidatores()
        {
            RuleFor(e => e.EditionNumber).ExclusiveBetween(1, 1000).WithMessage(Errors.RangNotBetween);
        }
    }
}
