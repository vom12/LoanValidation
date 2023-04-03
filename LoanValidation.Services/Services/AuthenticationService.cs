using LoanValidation.Domain.Models;
using LoanValidation.Services.Config;
using LoanValidation.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoanValidation.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAppConfig _config;
        private object login;

        public AuthenticationService(IAppConfig config)
        {
            _config = config;
        }

        public async Task<AuthenticationResult> Authenticate(Authentication auth)
        {
            // Authenticate user credentials (e.g. validate username and password in database)
            if(string.IsNullOrEmpty(auth.Username) && string.IsNullOrEmpty(auth.Password))
            {
                return new AuthenticationResult
                {
                    IsSuccessful = false,
                    ErrorMsg = "Invalid username or password"
                };
            }

            // If authentication succeeds, generate JWT token
            var jwtSettings = _config.JwtSettings;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, auth.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new AuthenticationResult
                {
                    IsSuccessful = true,
                    AccessToken = tokenHandler.WriteToken(token)
                };
            }
            catch(Exception ex)
            {

            }


            return new AuthenticationResult
            {
                IsSuccessful = false
            };
        }
    }
}
