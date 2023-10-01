namespace BookTest.Core.ViewModels.Rental
{
    public class RentalFormViewModel
    {
        public string SubscriberKey { get; set; } = null!;
        public  IList<int> SelectedCopies { get; set; }=new  List<int>();  
        public  int? CountAvailableForRental { get; set; }
    }
}
