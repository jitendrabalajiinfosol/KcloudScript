using KcloudScript.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KcloudScript.Service
{
    public interface IShortUrlService
    {
        Task<string> GenerateShortUrl(string url, int slideExpiry, int absExpiry);
        Task<string> RedirectToUrl(string shortUrl);
    }

    public class ShortUrlService: IShortUrlService
    {
        private const string urlCacheKey = "Urls";

        IMemeoryConfigService memeoryConfig;
        public ShortUrlService(IMemeoryConfigService _memeoryConfig)
        {
            memeoryConfig = _memeoryConfig;
        }

        /// <summary>
        /// Purpose : This method will be used to generate shorten url. The url will be stored in to the memory.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="slideExpiry"></param>
        /// <param name="absExpiry"></param>
        /// <returns></returns>
        public async Task<string> GenerateShortUrl(string? url, int slideExpiry, int absExpiry)
        {
            string shortenUrl = string.Empty;
            shortenUrl = UrlOperations.GenerateUrl();
            Dictionary<string, string>? urls = await memeoryConfig.GetObjectFromMemory(urlCacheKey) as Dictionary<string, string>;
            if (urls == null)
            {
                urls = new Dictionary<string, string>();
            }
            else
            {
                string? urlData;
                if (urls.TryGetValue(url, out urlData))
                {
                    urls.Remove(url);
                }
            }
            urls.Add(url, shortenUrl);
            await memeoryConfig.SetObjectInMemroy(urlCacheKey, urls, slideExpiry, absExpiry);
            return shortenUrl;
        }

        /// <summary>
        /// Purpose : This method will retrive actual url from the cached memory.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        public async Task<string> RedirectToUrl(string shortUrl)
        {
            string returnUrl = string.Empty;
            Dictionary<string, string>? urls = await memeoryConfig.GetObjectFromMemory(urlCacheKey) as Dictionary<string, string>;
            if (urls != null)
            {
                var urlKey = urls.Where(x => x.Value == shortUrl).FirstOrDefault();
                returnUrl = urlKey.Key;
            }
            else
            {
                returnUrl = "Url not found";
            }
            return returnUrl;
        }
    }
}
