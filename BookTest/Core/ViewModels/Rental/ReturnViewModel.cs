namespace BookTest.Core.ViewModels.Rental
{
    public class ReturnViewModel
    {
        public    int Id { get; set; }   
        public IList<RentalCopyViewModel> Copies=new  List<RentalCopyViewModel>();
        public  List <ReturnCopyViewModel> SelectedCopies=new List<ReturnCopyViewModel> ();
        public  bool AllowExtend { get; set; }
        public  int TotalDelayinDays { get; }
    }
}
