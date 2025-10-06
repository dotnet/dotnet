#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation
{
    using System;
    using System.Globalization;
    using System.Runtime.Caching;
    using Microsoft.ApplicationInsights.DataContracts;    

    internal sealed class CacheBasedOperationHolder : IDisposable
    {
        /// <summary>
        /// The memory cache instance used to hold items. MemoryCache.Default is not used as it is shared 
        /// across application and can potentially collide with customer application.
        /// </summary>
        private readonly MemoryCache memoryCache;

        /// <summary>
        /// The cache item policy which identifies the expiration time.
        /// </summary>
        private readonly CacheItemPolicy cacheItemPolicy;
        
        public CacheBasedOperationHolder(string cacheName, long expirationInMilliSecs)
        {
            this.cacheItemPolicy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMilliseconds(expirationInMilliSecs) };
            this.memoryCache = new MemoryCache(cacheName);
        }

        public Tuple<DependencyTelemetry, bool> Get(long id)
        {            
            Tuple<DependencyTelemetry, bool> result = null;
            var cacheItem = this.memoryCache.GetCacheItem(id.ToString(CultureInfo.InvariantCulture));
            if (cacheItem != null)
            {
                result = (Tuple<DependencyTelemetry, bool>)cacheItem.Value;
            }

            return result;
        }

        public bool Remove(long id)
        {            
            return this.memoryCache.Remove(id.ToString(CultureInfo.InvariantCulture)) != null;
        }

        /// <summary>
        /// Adds telemetry tuple to MemoryCache. DO NOT call it for the id that already exists in the cache.
        /// This is a known Memory Cache race-condition issue when items with same id are added concurrently
        /// and MemoryCache leaks memory. It should be fixed sometime AFTER .NET 4.7.1.
        /// </summary>
        public void Store(long id, Tuple<DependencyTelemetry, bool> telemetryTuple)
        {
            if (telemetryTuple == null)
            {
                throw new ArgumentNullException(nameof(telemetryTuple));
            }

            // it might be possible to optimize by preventing the long to string conversion
            this.memoryCache.Set(id.ToString(CultureInfo.InvariantCulture), telemetryTuple, this.cacheItemPolicy);
        }

        public void Dispose()
        {
            this.memoryCache.Dispose();
        }
    }
}
#endif