﻿namespace BookTest.Core.Models
{
    public class BaseModel
    {
        public bool Deleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdate { get; set; }
    }
}
