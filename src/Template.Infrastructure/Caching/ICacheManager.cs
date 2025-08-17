using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.Enums;

namespace Template.Infrastructure.Caching
{
    public interface ICacheManager
    {
        T? Get<T>(object key);
        Task<T?> GetAsync<T>(object key);
        void Set<T>(object key, T value, int durationCount, CacheTimeEnum durationType, bool useLocalScope = false);
        void Remove(object key);
        object GenerateCacheKey(object key, bool useLocalScope = false);
    }

}
