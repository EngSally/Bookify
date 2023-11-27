namespace BookTest.Core.ViewModels.Reports
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set;}
        public int TotalPages { get; set;}
        public bool HasPreviousPage {  get; set;}
        public bool HasNextPage {  get; set;}
        public  int Start{ get; set;}
        public  int End { get; set;}

    }
}
