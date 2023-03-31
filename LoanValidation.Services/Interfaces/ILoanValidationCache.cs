using LoanValidation.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanValidation.Services.Interfaces
{
    public interface ILoanValidationCache
    {
        Task<(bool, List<ValidationResult>)> TryGetValidationResultAsync(Lead lead);
        Task AddValidationResultAsync(Lead lead, List<ValidationResult> validationResult, TimeSpan expirationTime);
    }
}
