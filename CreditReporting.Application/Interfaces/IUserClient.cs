using System.Threading.Tasks;

namespace CreditReporting.Application.Interfaces
{
    public interface IUserClient
    {
        Task<UserDetailsDto?> GetCustomerByUserIdAsync(int userId);
        Task<UserDetailsDto?> GetCustomerByCustomerIdAsync(int customerId);
    }

    public class UserDetailsDto
    {
        public int CustomerId { get; set; }
        public string Pan { get; set; } = string.Empty;
      
        public int UserId { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Aadhar { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public decimal MonthlyIncome { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
