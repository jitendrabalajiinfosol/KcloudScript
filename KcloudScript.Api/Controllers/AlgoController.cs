using KcloudScript.Model;
using KcloudScript.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KcloudScript.Api.Controllers
{
    public class AlgoController : BaseController
    {
        private ILogger<AlgoController> logger;
        private string currentDirectory;
        private ICsvParserService csvFileParser;
        private IAlgoService algoService;
        object? nullObject = null;

        public AlgoController(ILogger<AlgoController> _logger, ICsvParserService _csvParserService, IAlgoService _algoService)
        {
            logger = _logger;
            currentDirectory = Directory.GetCurrentDirectory();
            csvFileParser = _csvParserService;
            algoService = _algoService;
        }

        [HttpGet("SortPhoneNo")]
        public async Task<IActionResult> SortPhoneNo()
        {
            string url = string.Empty;
            string copyPath = currentDirectory + @"\resources\PhoneNumbers-8-digits_sorted.csv";
            try
            {
                string absolutePath = currentDirectory + @"\resources\PhoneNumbers-8-digits.csv";

                List<int> data = await csvFileParser.ReadCsvFile<int>(absolutePath);

                algoService.SortNo(data, 0, data.Count - 1);

                bool IsSuccess = await csvFileParser.WriteCsvFile<int>(data, copyPath);

                if (IsSuccess)
                {
                    return SetResponse(HttpStatusCode.OK, true, copyPath, CommonMessage.FileWriteMsg);
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
