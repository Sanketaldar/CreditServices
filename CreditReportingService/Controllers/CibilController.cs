using CreditReporting.Application.DTOs;
using CreditReporting.Application.Interfaces;
using CreditReporting.Application.Services;
using CreditReportingService.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditReportingService.Controllers
{
    [ApiController]
    [Route("api/v1/cibil")]
    public class CibilController(ICibilService cibilService) : ControllerBase
    {
        private readonly ICibilService _cibilService = cibilService;



        [HttpPost("Generate")]
        public async Task<IActionResult> Create([FromBody] CibilCheckRequest reportDto)
        {
            var result = await _cibilService.CreateAsync(reportDto);
            
            if (!result.Success)
            {
                return BadRequest(ApiResponse<CibilReportDto>.FailureResponse(result.Message));
            }

            return CreatedAtAction(nameof(GetByCustomerId), new { customerId = result.Data!.CustomerId }, ApiResponse<CibilReportDto>.SuccessResponse(result.Data!, result.Message));
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            var result = await _cibilService.GetByCustomerIdAsync(customerId);
            if (result == null)
                return NotFound(ApiResponse<CibilReportDto>.FailureResponse("CIBIL report not found for the given Customer ID."));

            return Ok(ApiResponse<CibilReportDto>.SuccessResponse(result));
        }

        [HttpGet("pan/{panNo}")]
        public async Task<IActionResult> GetByPanNo(string panNo)
        {
            var result = await _cibilService.GetByPanNoAsync(panNo);
            if (result == null)
                return NotFound(ApiResponse<CibilReportDto>.FailureResponse("CIBIL report not found for the given PAN."));

            return Ok(ApiResponse<CibilReportDto>.SuccessResponse(result));
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cibilService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CibilReportDto>>.SuccessResponse(result));
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("reports/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cibilService.DeleteAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Report deleted successfully."));
        }
    }
}
