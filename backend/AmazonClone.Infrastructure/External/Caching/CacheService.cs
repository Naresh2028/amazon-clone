using AmazonClone.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Infrastructure.External.Caching
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _distributedCahce;
        private ILogger<CacheService> _logger;

        public CacheService(
            IDistributedCache distributedCahce,
            ILogger<CacheService> logger
            ) 
        {
            _distributedCahce = distributedCahce;
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                string? json = await _distributedCahce.GetStringAsync(key);

                if (string.IsNullOrEmpty(json)) 
                {
                    return null;
                }

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex) 
            {
                _logger.LogInformation(ex, "Cache read failed for key {key}", key);
                return null;
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _distributedCahce.RemoveAsync(key);
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, "Cache failed to remove {key}", key);
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration)
        {
            try
            {
                DistributedCacheEntryOptions option = new DistributedCacheEntryOptions();
                option.AbsoluteExpirationRelativeToNow = absoluteExpiration;

                if (slidingExpiration.HasValue) 
                {
                    option.SlidingExpiration = slidingExpiration.Value;
                }

                string json = JsonSerializer.Serialize(value);

                await _distributedCahce.SetStringAsync(key, json, option);
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex , "Cache failed to set {key}", key);
            }
        }
    }
}
