/******************************************************************************
 * Class Name : ShortUrlController.cs
 * Author     : Jitendra Patel
 *______________________________________________________________________________
 * Date         Author	              Change description
 *______________________________________________________________________________
 * 09/26/2022   Jitendra Patel        Shorten Url Operations
 *******************************************************************************/
using KcloudScript.Model;
using KcloudScript.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KcloudScript.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : BaseController
    {
        private readonly IShortUrlService shortUrlService;
        private readonly ILogger<ShortUrlController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private IOptions<AppSettingsEntity> appSettings;
        private object? nullObject = null;
        public ShortUrlController(IShortUrlService _shortUrlService, ILogger<ShortUrlController> _logger, IOptions<AppSettingsEntity> _appSettings, IHttpContextAccessor _httpContextAccessor)
        {
            shortUrlService = _shortUrlService;
            logger = _logger;
            appSettings = _appSettings;
            httpContextAccessor = _httpContextAccessor;
        }

        /// <summary>
        /// Purpose      : This operation will generate shorten url.
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        [HttpPost("api/[controller]/GenerateUrl")]
        public async Task<IActionResult> CreateShortenUrl(UrlRequestEntity originalUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = await shortUrlService.GenerateShortUrl(originalUrl.RequestUrl, appSettings.Value.SlidingExpiry, appSettings.Value.AbsExpiry);
                    string host = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}/rdata/{result}";
                    return SetResponse(HttpStatusCode.OK, true, host, CommonMessage.Success);
                }
                else
                {
                    string errorMessage = string.Empty;
                    ModelState.TryGetValue("RequestUrl", out ModelStateEntry? entityMessage);
                    if (entityMessage != null && entityMessage.Errors != null && entityMessage.Errors.Count > 0)
                    {
                        errorMessage = entityMessage.Errors[0].ErrorMessage;
                    }
                    return SetResponse(HttpStatusCode.BadRequest, false, nullObject, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommonMessage.ExceptionMsg);
            }
        }

        /// <summary>
        /// Purpose      : This operation will provide actual url based on the shorten url and will redirect to the actual url.
        /// </summary>
        /// <param name="shortanUrl"></param>
        /// <returns></returns>
        [HttpGet("rdata/{shortanUrl}")]
        public async Task<IActionResult> GetOriginalUrl(string shortanUrl)
        {
            try
            {
                var url = await shortUrlService.RedirectToUrl(shortanUrl);
                if (string.IsNullOrEmpty(url) == false)
                {
                    RedirectResult redirectUrl = Redirect(url);
                    return Redirect(redirectUrl.Url);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommonMessage.UrlNotFound);
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
