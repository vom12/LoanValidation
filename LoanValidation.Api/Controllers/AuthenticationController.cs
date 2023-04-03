using LoanValidation.Domain.Models;
using LoanValidation.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanValidation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Authentication request)
        {
            var result = await _authService.Authenticate(request);
            if (!result.IsSuccessful)
            {
                return BadRequest(new { error = result.ErrorMsg });
            }
            else
            {
                return Ok(new { token = result.AccessToken });
            }
        }
    }
}
