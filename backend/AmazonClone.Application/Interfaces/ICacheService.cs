using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key) where T : class;

        Task SetAsync<T>(string Key, T value, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration = null);
        Task RemoveAsync(string Key);
    }
}
