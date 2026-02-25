using System;

namespace CreditReporting.Domain.Entities
{
    public class CibilReport : BaseEntity
    {
        public int CibilId { get; set; }
        public int CustomerId { get; set; }
        public string PanNo { get; set; } = string.Empty;
        public int CibilScore { get; set; }
        public string CreditHistory { get; set; } = string.Empty;
        public DateTime CheckDate { get; set; }
     
        public string Status { get; set; } = "success"; // success, failed
    }
}
