using NUnit.Framework;
using KcloudScript.Model;
using KcloudScript.Service;
using KcloudScript.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using FluentAssertions;

namespace KcloudScript.Api.Tests.Integration
{
    public class CachingCapabilityControllerTest
    {
        private Mock<ILogger<CachingCapabilityController>> _logger;
        private Mock<IMemeoryConfigService> _memoryConfigService;
        private Mock<IMemoryCache> _memoryCache;
        private IOptions<AppSettingsEntity> _appSettings;

        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .Build();

            _appSettings = Options.Create(configuration.GetSection("AppSettings").Get<AppSettingsEntity>());
        }

        [SetUp]
        public void Setup()
        {
            _memoryCache = new Mock<IMemoryCache>();
            _logger = new Mock<ILogger<CachingCapabilityController>>();
            _memoryConfigService = new Mock<IMemeoryConfigService>();
        }

        [Test]
        public async Task SetKeyValue_Ok()
        {
            var controller = new CachingCapabilityController(_memoryConfigService.Object, _logger.Object, _appSettings);

            CachingEntity cachingEntity = new CachingEntity()
            {
                CachingKey = "Memory_Test",
                ExpirationTime = 10,
                CachingValue = "Memory Test"
            };

            var response = await controller.SetKeyValue(cachingEntity);
            var viewResult = ((ObjectResult)response).Value;

            var expected = new ResponseEntity() { data = null, result = false, message = CommonMessage.Success, statusCode = HttpStatusCode.OK };

            viewResult.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetValueByKey_Ok()
        {
            var controller = new CachingCapabilityController(_memoryConfigService.Object, _logger.Object, _appSettings);
            var CachingKey = "Memory_Test";
            var response = await controller.GetValueByKey(CachingKey);
            var viewResult = ((ObjectResult)response).Value;

            var expected = new ResponseEntity() { data = null, result = false, message = CommonMessage.NoValueFound, statusCode = HttpStatusCode.NotFound };

            viewResult.Should().BeEquivalentTo(expected);
        }
    }
}