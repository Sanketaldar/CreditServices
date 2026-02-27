using AutoMapper;
using CreditReporting.Application.DTOs;
using CreditReporting.Application.Interfaces;
using CreditReporting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CreditReporting.Application.Services
{
    public class CibilService(IApplicationDbContext context, IMapper mapper, ILogger<CibilService> logger, IUserClient userClient) : ICibilService
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CibilService> _logger = logger;
        private readonly IUserClient _userClient = userClient;
        private readonly Random _random = new Random();

        public async Task<CibilReportDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching CIBIL report by ID: {Id}", id);
            var report = await _context.CibilReports
                .FirstOrDefaultAsync(r => r.CibilId == id && !r.IsDeleted);
            return _mapper.Map<CibilReportDto>(report);
        }

        public async Task<CibilReportDto?> GetByPanNoAsync(string panNo)
        {
            _logger.LogInformation("Fetching CIBIL report by PAN: {PanNo}", panNo);
            var report = await _context.CibilReports
                .FirstOrDefaultAsync(r => r.PanNo == panNo && !r.IsDeleted);
            return _mapper.Map<CibilReportDto>(report);
        }

        public async Task<CibilReportDto?> GetByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Fetching CIBIL report by CustomerId: {CustomerId}", customerId);
            var report = await _context.CibilReports
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && !r.IsDeleted);
            return _mapper.Map<CibilReportDto>(report);
        }

        public async Task<IEnumerable<CibilReportDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all CIBIL reports");
            var reports = await _context.CibilReports
                .Where(r => !r.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CibilReportDto>>(reports);
        }

        public async Task<PaginatedList<CibilReportDto>> GetAllPagedAsync(int pageIndex, int pageSize)
        {
            _logger.LogInformation("Fetching paged CIBIL reports. Page: {Page}, Size: {Size}", pageIndex, pageSize);
            
            var query = _context.CibilReports.Where(r => !r.IsDeleted);
            var count = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<CibilReportDto>>(items);
            return new PaginatedList<CibilReportDto>(dtos, count, pageIndex, pageSize);
        }

        public async Task<CibilReportDto> CreateAsync(CibilCheckRequest request)
        {
            // PROACTIVE FETCH LOGIC: If PAN is missing, try to fetch from User Service
            if (string.IsNullOrEmpty(request.PanNo))
            {
                _logger.LogInformation("PAN missing. Attempting to fetch from User Service for CustomerId: {CustomerId} / UserId: {UserId}", request.CustomerId, request.UserId);

                //Logic is commented until UserService is ready

                UserDetailsDto ? userDetails = null;
                //if (request.UserId.HasValue)
                //    userDetails = await _userClient.GetCustomerByUserIdAsync(request.UserId.Value);
                //else
                    userDetails = await _userClient.GetCustomerByCustomerIdAsync(request.CustomerId);

                if (userDetails != null)
                {
                    request.PanNo = userDetails.Pan;
                    request.CustomerId = userDetails.CustomerId; // Ensure we have the correct CustomerId
                }

            }

            if (string.IsNullOrEmpty(request.PanNo))
            {
                _logger.LogWarning("Could not proceed with CIBIL generation. PAN is still missing.");
              
            }

            _logger.LogInformation("Generating CIBIL report for Customer: {CustomerId}, PAN: {PanNo}", request.CustomerId, request.PanNo);
            
            var existingReport = await _context.CibilReports
                .FirstOrDefaultAsync(r => r.PanNo == request.PanNo && !r.IsDeleted);
                
            if (existingReport != null)
            {
                _logger.LogInformation("Found existing report for PAN: {PanNo}. Skipping generation.", request.PanNo);
                return _mapper.Map<CibilReportDto>(existingReport);
            }

            var report = _mapper.Map<CibilReport>(request);
            
            report.CibilScore = _random.Next(300, 900);
            report.CreditHistory = $"Auto-generated for PAN: {report.PanNo}";
           
            report.CheckDate = DateTime.UtcNow;
            report.Status = "success";
            
            await _context.CibilReports.AddAsync(report);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Successfully generated CIBIL report for Customer: {CustomerId}", report.CustomerId);
            
            return _mapper.Map<CibilReportDto>(report);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogWarning("Deleting CIBIL report ID: {Id}", id);
            var report = await _context.CibilReports
                .FirstOrDefaultAsync(r => r.CibilId == id);
            if (report != null)
            {
                report.IsDeleted = true;
                report.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}

