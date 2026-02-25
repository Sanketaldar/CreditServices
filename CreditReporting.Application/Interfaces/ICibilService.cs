using CreditReporting.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditReporting.Application.Interfaces
{
    public interface ICibilService
    {
        Task<CibilReportDto?> GetByIdAsync(int id);
        Task<CibilReportDto?> GetByPanNoAsync(string panNo);
        Task<CibilReportDto?> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<CibilReportDto>> GetAllAsync();
        Task<PaginatedList<CibilReportDto>> GetAllPagedAsync(int pageIndex, int pageSize);
        Task<CibilReportDto> CreateAsync(CibilCheckRequest request);
        Task DeleteAsync(int id);
    }
}
