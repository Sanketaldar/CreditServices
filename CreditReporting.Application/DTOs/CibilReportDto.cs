using System;

namespace CreditReporting.Application.DTOs
{
    public class CibilReportDto
    {
        public int CibilId { get; set; }
        public int CustomerId { get; set; }
        public string PanNo { get; set; } = string.Empty;
        public int CibilScore { get; set; }
        public string CreditHistory { get; set; } = string.Empty;
        public DateTime CheckDate { get; set; }
       
        public string Status { get; set; } = "success";
    }

    public class CibilCheckRequest
    {
        public int CustomerId { get; set; }
        public string? PanNo { get; set; }
        public int? UserId { get; set; }
        
    }

    public class CibilCreationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CibilReportDto? Data { get; set; }
    }
}
