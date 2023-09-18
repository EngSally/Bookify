namespace BookTest.Core.ViewModels.Rental
{
    public class RentalFormViewModel
    {
        public string SubscriberKey { get; set; } = null!;
        public  IList<int> BookCopiesForRental { get; set; }=new  List<int>();  

    }
}
