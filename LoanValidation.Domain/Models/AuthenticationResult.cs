using System.Collections.Generic;

namespace LoanValidation.Domain.Models
{
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMsg { get; set; }
    }
}
