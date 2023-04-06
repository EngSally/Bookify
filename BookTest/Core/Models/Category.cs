

using Microsoft.EntityFrameworkCore;

namespace BookTest.Core.Models
{
    [Index(nameof(Name),IsUnique =true)]
    public class Category
    {

        public int Id { get; set; }

        [MaxLength(100)]
        public String Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime CretedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdate { get; set; }
        
    }
}
