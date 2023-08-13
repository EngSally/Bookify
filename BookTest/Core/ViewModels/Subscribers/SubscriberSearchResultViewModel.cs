namespace BookTest.Core.ViewModels.Subscribers
{
    public class SubscriberSearchResultViewModel
    {
        public  int Id { get; set; }
        public string FullName { set; get; } = null!;
        public string? ImageUrlThumbnail { get; set; }
    }
}
