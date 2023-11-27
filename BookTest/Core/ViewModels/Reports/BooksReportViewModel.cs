using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BookTest.Core.ViewModels.Reports
{
    public class BooksReportViewModel
    {
        [DisplayName("Author")]
        public List<int>? SelectedAuthors { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> Authors {  get; set; }=new  List<SelectListItem>();
        [DisplayName("Categories ")]
        public List<int>? SelectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
         public PaginatedList<Book> Books { get; set; }
    }
}
