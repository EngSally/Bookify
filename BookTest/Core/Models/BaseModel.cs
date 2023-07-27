namespace BookTest.Core.Models
{
    public class BaseModel
    {
        public bool Deleted { get; set; }
     
        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
     
        public string? LastUpdateById { get; set; }
        public ApplicationUser? LastUpdateBy { get; set; }
        public DateTime? LastUpdateOn { get; set; }
    }
}
