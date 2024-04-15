

namespace Bookify.Domain;

public class CopyRentalHistoryDto
{

       public   int BookCopyId {  get; set; }
    public string? SubscriberName { get; set; }
    public string? SubscriberNum {  get; set; }

    public DateTime RentalDate {  get; set; }
    public DateTime? EndDate {  get; set; }
    public DateTime? ReturnDate {  get; set; }
    public DateTime? ExtendedOn {  get; set; }

    }
