using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookTest.Core.ViewModels.Categories
{
    public class CategoriesFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = Error.MaxLength)]
        [Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = Error.Duplicate)]
        [DisplayName ( "Category")]
        public string Name { get; set; } = null!;

    }
}
