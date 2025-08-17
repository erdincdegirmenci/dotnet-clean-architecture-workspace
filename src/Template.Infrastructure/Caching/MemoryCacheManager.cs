using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.Enums;

namespace Template.Infrastructure.Caching
{
    /// <summary>
    ///  List<ParameterCacheModel> cacheResult = _cacheManager.GetCache<List<ParameterCacheModel>>(_cacheManager.GenerateCacheKey(CacheKeys.PARAMETER));
    ///    _cacheManager.SetCache(_cacheManager.GenerateCacheKey(CacheKeys.PARAMETER), cacheData, _cacheManager.SetCacheEntryOptions(30, CacheTimeEnum.Minute));
    /// </summary>

    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private static object _localScopeKey;

        public static object LocalScopeKey
        {
            set { _localScopeKey = value; }
        }

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Get<T>(object key)
        {
            var cacheKey = GenerateCacheKey(key);
            return _memoryCache.TryGetValue(cacheKey, out T value) ? value : default;
        }

        public Task<T?> GetAsync<T>(object key)
        {
            // Async için simule edilmiş Task
            return Task.FromResult(Get<T>(key));
        }

        public void Set<T>(object key, T value, int durationCount, CacheTimeEnum durationType, bool useLocalScope = false)
        {
            var cacheKey = GenerateCacheKey(key, useLocalScope);

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = durationType switch
                {
                    CacheTimeEnum.Second => TimeSpan.FromSeconds(durationCount),
                    CacheTimeEnum.Minute => TimeSpan.FromMinutes(durationCount),
                    CacheTimeEnum.Hour => TimeSpan.FromHours(durationCount),
                    CacheTimeEnum.Day => TimeSpan.FromDays(durationCount),
                    _ => TimeSpan.FromMinutes(durationCount)
                }
            };

            _memoryCache.Set(cacheKey, value, options);
        }


        public void Remove(object key)
        {
            var cacheKey = GenerateCacheKey(key);
            _memoryCache.Remove(cacheKey);
        }

        public object GenerateCacheKey(object key, bool useLocalScope = false)
        {
            return string.Format("{0}{1}", ((useLocalScope) ? _localScopeKey + "_" : ""), key);
        }
    }

}