using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace BookTest.Core.ViewModels.Rental
{
    public class ReturnViewModel
    {
        public    int Id { get; set; }
        public IList<RentalCopyViewModel> Copies { get; set; } = new List<RentalCopyViewModel>();

        public List<ReturnCopyViewModel> SelectedCopies { get; set; } = new();
        public  bool AllowExtend { get; set; }
        public  int TotalDelayinDays {
            get {
                return Copies.Sum(c => c.DelayInDays);
                    } 
        }
        [Display(Name = "Penalty Paid?")]
        [AssertThat("(TotalDelayInDays == 0 && PenaltyPaid == false) || PenaltyPaid == true", ErrorMessage = Errors.PenaltyShouldBePaid)]
        public bool PenaltyPaid { get; set; }

    }
}
