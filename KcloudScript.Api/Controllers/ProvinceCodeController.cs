/******************************************************************************
 * Class Name : ProvinceCodeController.cs
 * Author     : Jitendra Patel
 *______________________________________________________________________________
 * Date         Author	              Change description
 *______________________________________________________________________________
 * 09/26/2022   Jitendra Patel        Retrive Province Details from usps
 *******************************************************************************/
using KcloudScript.Model;
using KcloudScript.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KcloudScript.Api.Controllers
{
    public class ProvinceCodeController : BaseController
    {
        IProvincialCodeService provinceCodeService;
        ILogger<ProvinceCodeController> logger;
        object? nullObject = null;

        public ProvinceCodeController(IProvincialCodeService _stateByZipService, ILogger<ProvinceCodeController> _logger)
        {
            provinceCodeService = _stateByZipService;
            logger = _logger;
        }

        /// <summary>
        /// Purpose      : This method is used to get the details of the province (List of the Cities, State etc.) based on the                    provided province code. We use the http request to the third party service of usps to get these                         details.
        /// </summary>
        /// <param name="provinceCode">Provide Province code / zipcode to get the details of the Province</param>
        /// <returns></returns>
        [HttpGet("GetProviceDetailsByCode")]
        public async Task<IActionResult> GetProviceDetailsByCode(string provinceCode)
        {
            try
            {
                if (string.IsNullOrEmpty(provinceCode) == false)
                {
                    object? result = await provinceCodeService.GetProvinceDetaisByCodeAsync(provinceCode);
                    return SetResponse(HttpStatusCode.OK, true, result, CommonMessage.Success);
                }
                else
                {
                    return SetResponse(HttpStatusCode.BadRequest, false, nullObject, CommonMessage.InputValueBlank);
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
