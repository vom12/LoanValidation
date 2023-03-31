using System;

namespace LoanValidation.Services.Interfaces
{
    public interface IErrorHandlingService
    {
        public void LogError(Exception ex);
    }
}
