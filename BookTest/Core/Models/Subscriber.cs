namespace BookTest.Core.Models
{
    [Index(nameof(NationalId),IsUnique=true)]
    [Index(nameof(MobilNum),IsUnique =true)]
    [Index(nameof(Email),IsUnique =true)]
    public class Subscriber:BaseModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string FristName { get; set; } = null!;
        [MaxLength(100)]    
        public string LastName { get; set; } = null!;   
        public DateTime DateOfBirth { get; set; }
        [MaxLength(15)]
        public string NationalId { get; set; } = null!;
        [MaxLength(20)]
        public string MobilNum { get; set; } = null!;
        public bool HasWhatsApp { get; set; }
        [MaxLength(150)]
        public  string Email { get; set; } = null!;
        [MaxLength(500)]
        public string? ImageUrl { get; set; }
        [MaxLength(500)]
        public string? ImageUrlThumbnail { get; set; }
        public  string Address { get; set; } = null!;
        public  bool IsBlackListed { get; set; }  
        public int AreaId { get; set; } 
        public Area? Area { get; set; }
        public int  GovernorateId { get; set; }
        public Governorate? Governorate { get; set; }
        
    }
}
