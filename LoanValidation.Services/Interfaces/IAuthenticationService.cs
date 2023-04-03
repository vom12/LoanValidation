using LoanValidation.Domain.Models;
using System.Threading.Tasks;

namespace LoanValidation.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResult> Authenticate(Authentication auth);
    }
}
