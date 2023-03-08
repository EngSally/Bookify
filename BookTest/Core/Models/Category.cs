

namespace BookTest.Core.Models
{
    public class Category
    {

        public int Id { get; set; }

        [MaxLength(100)]
        public String Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime CretedOn { get; set; }
        public DateTime? LastUpdate { get; set; }
        
    }
}
