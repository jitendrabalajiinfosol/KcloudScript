using KcloudScript.Model;
using KcloudScript.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KcloudScript.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static AppSettingsEntity AppSettings { get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsEntity>(appSettingsSection);
            AppSettings = appSettingsSection.Get<AppSettingsEntity>();

            services.AddControllers();
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "KcloudScript.Api", Version = "v1" });
                swagger.EnableAnnotations();
            });

            var serilogLogger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Code).WriteTo.File("./Logs/KcloudScriptLog-.txt", rollingInterval: RollingInterval.Day).MinimumLevel.Debug().CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
            services.AddMemoryCache();

            services.AddHttpClient("uspsClient", c =>
           {
               c.BaseAddress = new Uri("https://tools.usps.com/");
               c.DefaultRequestHeaders.Add("Connection", "Keep-alive");
               c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           });

            RegisterServices(ref services);
        }

        private void RegisterServices(ref IServiceCollection services)
        {
            services.AddSingleton<IMemeoryConfigService, MemeoryConfigService>();
            services.AddSingleton<IShortUrlService, ShortUrlService>();
            services.AddTransient<IProvincialCodeService, ProvincialCodeService>();
            services.AddTransient<ICsvParserService, CsvParserService>();
            services.AddTransient<IAlgoService, AloService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KcloudScript Api v1"));
            }

            app.UseCors(builder => builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
            app.UseCors("ApiCorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
        }
    }
}
