/******************************************************************************
 * Class Name : CachingCapabilityController.cs
 * Author     : Jitendra Patel
 *______________________________________________________________________________
 * Date         Author	              Change description
 *______________________________________________________________________________
 * 09/26/2022   Jitendra Patel        Memory Caching Operations
 *******************************************************************************/
using KcloudScript.Model;
using KcloudScript.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KcloudScript.Api.Controllers
{
    public class CachingCapabilityController : BaseController
    {
        private readonly IMemeoryConfigService memeoryConfigService;
        private readonly ILogger<CachingCapabilityController> logger;
        private readonly IOptions<AppSettingsEntity> appSettings;
        object? nullObject = null;

        public CachingCapabilityController(IMemeoryConfigService _memeoryConfigService, ILogger<CachingCapabilityController> _logger, IOptions<AppSettingsEntity> _appSettings)
        {
            memeoryConfigService = _memeoryConfigService;
            logger = _logger;
            appSettings = _appSettings;
        }

        /// <summary>
        /// Purpose: Store the key and value in to the memory for a period of time as a expiry time.
        /// </summary>
        /// <param name="cachingEntity"></param>
        /// <returns></returns>
        [HttpPost("SetKeyValue")]
        public async Task<IActionResult> SetKeyValue(CachingEntity? cachingEntity)
        {
            try
            {
                if (cachingEntity == null)
                    return SetResponse(HttpStatusCode.BadRequest, false, nullObject, CommonMessage.InputValueBlank);
                else
                {
                    if (ModelState.IsValid)
                    {
                        bool result = await memeoryConfigService.SetObjectInMemroy(cachingEntity.CachingKey, cachingEntity.CachingValue, appSettings.Value.SlidingExpiry, cachingEntity.ExpirationTime);
                        return SetResponse(HttpStatusCode.OK, result, nullObject, CommonMessage.Success);
                    }
                    else
                    {
                        return SetResponse(HttpStatusCode.BadRequest, false, nullObject, CommonMessage.InputValueBlank);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommonMessage.ExceptionMsg);
            }
        }

        /// <summary>
        /// Purpose: Retrive the actual information from the memory based on the provided key. It will throguh an error if the key           is not found or it gets expired.
        /// </summary>
        /// <param name="cachingKey"></param>
        /// <returns></returns>
        [HttpGet("GetValueByKey")]
        public async Task<IActionResult> GetValueByKey(string cachingKey)
        {
            string url = string.Empty;
            try
            {
                object data = await memeoryConfigService.GetObjectFromMemory(cachingKey);
                if (data != null)
                {
                    return SetResponse(HttpStatusCode.OK, true, data, CommonMessage.Success);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommonMessage.NoValueFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommonMessage.ExceptionMsg);
            }

        }
    }
}
