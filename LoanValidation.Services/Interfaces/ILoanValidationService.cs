using LoanValidation.Domain.Models;
using System.Threading.Tasks;

namespace LoanValidation.Services.Interfaces
{
    public interface ILoanValidationService
    {
        public Task<ResultResponse> ValidateLeadAsync(Lead lead);
    }
}
