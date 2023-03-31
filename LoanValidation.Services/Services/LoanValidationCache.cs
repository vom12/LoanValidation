using LoanValidation.Domain.Models;
using LoanValidation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanValidation.Services.Services
{
    public class LoanValidationCache : ILoanValidationCache
    {
        private readonly ICacheService _cacheService;

        public LoanValidationCache(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<(bool, List<ValidationResult>)> TryGetValidationResultAsync(Lead lead)
        {
            var cacheKey = GetCacheKey(lead);
            var cachedResult = (List<ValidationResult>)_cacheService.Get(cacheKey);

            if (cachedResult != null)
            {
                return (true, cachedResult);
            }

            return (false, null);
        }

        public async Task AddValidationResultAsync(Lead lead, List<ValidationResult> validationResult, TimeSpan expirationTime)
        {
            var cacheKey = GetCacheKey(lead);
            _cacheService.Add(cacheKey, validationResult, expirationTime);
        }

        private static string GetCacheKey(Lead lead)
        {
            return $"ValidationResult:{lead.BusinessNumber}";
        }
    }
}
