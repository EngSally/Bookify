using BookTest.Core.ViewModels.Subscribers;

namespace BookTest.Core.ViewModels.Rental
{
    public class RentalViewModel
    {
        public int Id { get; set; }
        public SubscriberViewModel? Subscriber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool PenaltyPaid { set; get; }
        public IEnumerable<RentalCopyViewModel> RentalCopies { get; set; } = new List<RentalCopyViewModel>();
        public int TotalDelayInDays
            {
                get
                {
                    return RentalCopies.Sum(c => c.DelayInDays);
                }
            }
        public int NumberOfCopies
        {
            get
            {
                return RentalCopies.Count();
            }
        }

    
    }
}
