using LoanValidation.Domain.Models;
using LoanValidation.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LoanValidation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanValidationController : ControllerBase
    {
        private readonly ILoanValidationService _loanValidationService;
        private readonly IErrorHandlingService _errorHandlingService;

        public LoanValidationController(ILoanValidationService loanValidationService, IErrorHandlingService errorHandlingService)
        {
            _loanValidationService = loanValidationService;
            _errorHandlingService = errorHandlingService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lead lead)
        {
            if (lead == null)
            {
                return BadRequest("Lead object is null");
            }

            ResultResponse validationResult;
            try
            {
                validationResult = await _loanValidationService.ValidateLeadAsync(lead);
            }
            catch (Exception ex)
            {
                _errorHandlingService.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the loan validation");
            }

            return Ok(validationResult);
        }
    }
}
