namespace BookTest.Core.ViewModels.Subscribers
{
    public class RenewalSubscribtionViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string Status
        {
            get
            {
                return EndDate < DateTime.Today ? SubscribtionStatus.Expired :
                         StartDate > DateTime.Today ? string.Empty : SubscribtionStatus.Active;
            }
        }
    }
}
