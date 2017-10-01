using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using WebApisample1.Entities;

namespace WebApisample1
{
    public class Startup
    {

        #region [Core 1]
        //public static IConfigurationRoot Configuration { get; private set; }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

        //    Configuration = builder.Build();
        //}
        #endregion

        #region [Core 2]
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion 


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc( o=> o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            /*
                .AddJsonOptions( o => {
                    if(o.SerializerSettings.ContractResolver != null)
                    {
                        var castResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                        castResolver.NamingStrategy = null;
                    }
                });
            */ 
#if DEBUG
            services.AddTransient<Services.IMailService, Services.LocalMailService>();
#else
            services.AddTransient<Services.IMailService, Services.CloudMailService>();
#endif
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];
             services.AddDbContext<Entities.CityInfoContext>(  o => o.UseSqlServer(connectionString) );

            services.AddScoped<Services.ICityInfoRepository, Services.CityInfoRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory
            , Entities.CityInfoContext cityInfoContext)
        {
            //loggerFactory.AddConsole();

            //loggerFactory.AddDebug();

            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());

            loggerFactory.AddNLog();

            app.AddNLogWeb(); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

           AutoMapper.Mapper.Initialize(cfg =>
           {
               cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
               cfg.CreateMap<Entities.City, Models.CityDto>();
               cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
               cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
               cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
               cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
           });

            app.UseMvc();
        }
    }
}
