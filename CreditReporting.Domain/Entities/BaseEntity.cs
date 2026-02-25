using System;

namespace CreditReporting.Domain.Entities
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public string? DeletedBy { get; set; }
    }
}
