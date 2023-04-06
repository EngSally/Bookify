namespace BookTest.Core.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public String Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime CretedOn { get; set; } 
        public DateTime? LastUpdate { get; set; }
    }
}
