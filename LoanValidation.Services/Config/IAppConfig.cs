using System;
using System.Collections.Generic;
using System.Text;

namespace LoanValidation.Services.Config
{
    public interface IAppConfig
    {
        public string AllowedCitizenshipStatuses { get; }
        public string AllowedCountryCodes { get; }
        public string AllowedIndustries { get; }
        public string BannedIndustries { get; }
        public int MinLoanAmount { get; }
        public int MaxLoanAmount { get; }
        public int MinTimeTrading { get; }
        public int MaxTimeTrading { get; }
        public int CacheExpirationMinutes { get; }
        public string LogFilePath { get; }
    }
}
