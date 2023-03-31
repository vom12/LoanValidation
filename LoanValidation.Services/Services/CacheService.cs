using LoanValidation.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace LoanValidation.Services.Services
{
    public class CacheService : ICacheService
    {
        private readonly MemoryCache _cache;

        public CacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Add(string key, object value, TimeSpan absoluteExpiration)
        {
            _cache.Set(key, value, absoluteExpiration);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }
    }
}
