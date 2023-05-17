using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookTest.Core.ViewModels.Categories
{
    public class CategoriesFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLength)]
        [Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        [DisplayName ( "Category")]
        public string Name { get; set; } = null!;

    }
}
