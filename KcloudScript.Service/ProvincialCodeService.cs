using KcloudScript.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KcloudScript.Service
{
    public interface IProvincialCodeService
    {
        Task<ProvinceCodeEntity?> GetProvinceDetaisByCodeAsync(string provinceCode);
    }

    public class ProvincialCodeService : IProvincialCodeService
    {
        private readonly IHttpClientFactory clientFactory;

        public ProvincialCodeService(IHttpClientFactory _clientFactory)
        {
            clientFactory = _clientFactory;
        }

        /// <summary>
        /// Function Name: GetProvinceDetaisByCodeAsync
        /// Purpose      : This method is used to get the details of the province (List of the Cities, State etc.) based on the                   provided province code. We use the http request to the third party service of usps to get these                        details.
        /// </summary>
        /// <param name="provinceCode">Provide Province code / zipcode to get the details of the Province</param>
        /// <returns></returns>
        public async Task<ProvinceCodeEntity?> GetProvinceDetaisByCodeAsync(string provinceCode)
        {
            var client = clientFactory.CreateClient("uspsClient");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("zip", provinceCode);

            var request = new HttpRequestMessage(HttpMethod.Post, "tools/app/ziplookup/cityByZip");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "zip", provinceCode }, });

            using (var response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ProvinceCodeEntity>(responseBody);
                }
            }

            return null;
        }
    }
}
