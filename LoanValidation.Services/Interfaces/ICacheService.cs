using System;

namespace LoanValidation.Services.Interfaces
{
    public interface ICacheService
    {
        void Add(string key, object value, TimeSpan absoluteExpiration);
        object Get(string key);
    }
}
