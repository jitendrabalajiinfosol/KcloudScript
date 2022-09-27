using KcloudScript.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace KcloudScript.Service
{
    public interface IMemeoryConfigService
    {
        Task<bool> SetObjectInMemroy(string cacheKey, object dataObject, int slidingExpiry, int absExpiry);
        Task<object?> GetObjectFromMemory(string cacheKey);
    }
    public class MemeoryConfigService : IMemeoryConfigService
    {
        private readonly IMemoryCache memoryCache;
        public MemeoryConfigService(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }

        /// <summary>
        /// Purpose : This method will retrive the information from the cached memory based on the key.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<object?> GetObjectFromMemory(string cacheKey)
        {
            object? data = null;
            memoryCache.TryGetValue(cacheKey.ToLower(),out data);
            return data;
        }

        /// <summary>
        /// Purpose : This method will store data into the cached memory.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dataObject"></param>
        /// <param name="slidingExpiry"></param>
        /// <param name="absExpiry"></param>
        /// <returns></returns>
        public async Task<bool> SetObjectInMemroy(string cacheKey, object dataObject, int slidingExpiry, int absExpiry)
        {
            bool isSuccess = true;
            object? data = null;
            if (memoryCache.TryGetValue(cacheKey, out data) == false)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiry))
                                                                      .SetAbsoluteExpiration(TimeSpan.FromSeconds(absExpiry))
                                                                      .SetPriority(CacheItemPriority.Normal);

                memoryCache.Set(cacheKey.ToLower(), dataObject, cacheEntryOptions);
            }
            return isSuccess;
        }
    }
}
