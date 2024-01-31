using Bookify.Web.Core.ViewModels.Books;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Validators;

public class BooksFormValidation:AbstractValidator<BooksFormViewModel>
{
    public BooksFormValidation()
    {
        RuleFor(e => e.Title).MaximumLength(500).WithMessage(Errors.MaxLength);
        RuleFor(e => e.Publisher).MaximumLength(200).WithMessage(Errors.MaxLength);
        RuleFor(e => e.Hall).MaximumLength(100).WithMessage(Errors.MaxLength);
        
    }
}
