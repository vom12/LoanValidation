using LoanValidation.Domain;
using LoanValidation.Domain.Models;
using LoanValidation.Services.Config;
using LoanValidation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoanValidation.Services.Services
{
    public class LoanValidationService : ILoanValidationService
    {
        private readonly ILoanValidationCache _validationCache;
        private readonly IBusinessNumberService _businessNumberService;
        private readonly IAppConfig _config;

        public LoanValidationService(ILoanValidationCache validationCache, IAppConfig config, IBusinessNumberService businessNumberService)
        {
            _validationCache = validationCache;
            _config = config;
            _businessNumberService = businessNumberService;
        }

        public async Task<ResultResponse> ValidateLeadAsync(Lead lead)
        {
            var (foundInCache, cachedResult) = await _validationCache.TryGetValidationResultAsync(lead);
            if (foundInCache)
            {
                if (cachedResult.Any(vr => vr.Decision != Enums.Decision.Qualified.ToString()))
                {
                    return new ResultResponse
                    {
                        Decision = Enums.Decision.Unqualified.ToString(),
                        ValidationResult = cachedResult.Where(r => r.Decision == Enums.Decision.Unqualified.ToString() || r.Decision == Enums.Decision.Unknown.ToString()).ToList()
                    };
                }
                else
                {
                    return new ResultResponse
                    {
                        Decision = Enums.Decision.Qualified.ToString()
                    };
                }
            }

            var validationResult = new List<ValidationResult>();

            ValidateFirstNameOrLastName(lead, validationResult);
            ValidatePhoneNumber(lead, validationResult);
            ValidateEmailAddress(lead, validationResult);
            await ValidateBusinessNumberAsync(lead, validationResult);
            ValidateLoanAmount(lead, validationResult);
            ValidateCitizenshipStatus(lead, validationResult);
            ValidateTimeTrading(lead, validationResult);
            ValidateCountryCode(lead, validationResult);
            ValidateIndustry(lead, validationResult);


            // Cache the validation result
            await _validationCache.AddValidationResultAsync(lead, validationResult, TimeSpan.FromMinutes(_config.CacheExpirationMinutes));

            // Return the result
            if (validationResult.Any(vr => vr.Decision != Enums.Decision.Qualified.ToString()))
            {
                return new ResultResponse
                {
                    Decision = Enums.Decision.Unqualified.ToString(),
                    ValidationResult = validationResult.Where(r => r.Decision == Enums.Decision.Unqualified.ToString() || r.Decision == Enums.Decision.Unknown.ToString()).ToList()
                };
            }
            else
            {
                return new ResultResponse
                {
                    Decision = Enums.Decision.Qualified.ToString()
                };
            }
        }

        private void ValidateFirstNameOrLastName(Lead lead, List<ValidationResult> validationResult)
        {
            // Check if the first name is provided
            if (string.IsNullOrEmpty(lead.FirstName) || string.IsNullOrEmpty(lead.LastName))
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "nameRule",
                    Message = "Either first name or last name must be provided",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private void ValidatePhoneNumber(Lead lead, List<ValidationResult> validationResult)
        {
            if (string.IsNullOrWhiteSpace(lead.PhoneNumber) || !Regex.IsMatch(lead.PhoneNumber, @"^(04|\+614)[0-9]{8}$") && !Regex.IsMatch(lead.PhoneNumber, @"^(02|03|07|08)[0-9]{8}$"))
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "phoneRule",
                    Message = "Phone number is invalid",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private void ValidateEmailAddress(Lead lead, List<ValidationResult> validationResult)
        {
            if (!string.IsNullOrEmpty(lead.EmailAddress))
            {
                try
                {
                    var email = new System.Net.Mail.MailAddress(lead.EmailAddress);
                    validationResult.Add(new ValidationResult
                    {
                        Rule = "emailRule",
                        Message = "Email is valid",
                        Decision = Enums.Decision.Qualified.ToString()
                    });
                }
                catch
                {
                    validationResult.Add(new ValidationResult
                    {
                        Rule = "emailRule",
                        Message = "Email is invalid",
                        Decision = Enums.Decision.Unqualified.ToString()
                    });
                }
            }
            else
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "emailRule",
                    Message = "Email is not provided",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private async Task ValidateBusinessNumberAsync(Lead lead, List<ValidationResult> validationResult)
        {


            if (string.IsNullOrWhiteSpace(lead.BusinessNumber))
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "businessNumberRule",
                    Message = "Business number must be provided",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
                return;
            }

            var isValid = await _businessNumberService.ValidateBusinessNumber(lead.BusinessNumber);

            if(!isValid)
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "businessNumberRule",
                    Message = "Business number not valid",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private void ValidateLoanAmount(Lead lead, List<ValidationResult> validationResult)
        {
            if (lead.LoanAmount < _config.MinLoanAmount || lead.LoanAmount > _config.MaxLoanAmount)
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "loanAmountRule",
                    Message = "Loan amount must be between $1,000 and $10,000",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private void ValidateCitizenshipStatus(Lead lead, List<ValidationResult> validationResult)
        {
            var allowedCitizenshipStatuses = _config.AllowedCitizenshipStatuses.Split(",");

            if (!allowedCitizenshipStatuses.Contains(lead.CitizenshipStatus))
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "citizenshipStatusRule",
                    Message = "The provided citizenship status is not allowed.",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private void ValidateTimeTrading(Lead lead, List<ValidationResult> validationResult)
        {
            if (lead.TimeTrading < _config.MinTimeTrading || lead.TimeTrading > _config.MaxTimeTrading)
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "timeTradingRule",
                    Message = $"Time trading must be between {_config.MinTimeTrading} and {_config.MaxTimeTrading} months",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        public void ValidateCountryCode(Lead lead, List<ValidationResult> validationResult)
        {
            var allowedCountryCodes = _config.AllowedCountryCodes.Split(",");
            if (!allowedCountryCodes.Contains(lead.CountryCode))
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "countryCodeRule",
                    Message = "Invalid country code",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
        }

        private void ValidateIndustry(Lead lead, List<ValidationResult> validationResult)
        {
            var allowedIndustries = _config.AllowedIndustries?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();
            var bannedIndustries = _config.BannedIndustries?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();

            if (allowedIndustries.Contains(lead.Industry))
            {
                validationResult.Add(new ValidationResult
                {
                    Decision = Enums.Decision.Qualified.ToString()
                });
            }
            else if (bannedIndustries.Contains(lead.Industry))
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "industryBannedRule",
                    Message = "Industry is banned",
                    Decision = Enums.Decision.Unqualified.ToString()
                });
            }
            else
            {
                validationResult.Add(new ValidationResult
                {
                    Rule = "industryUnkownRule",
                    Message = "Industry unknown",
                    Decision = Enums.Decision.Unknown.ToString()
                });
            }
        }
    }
}
