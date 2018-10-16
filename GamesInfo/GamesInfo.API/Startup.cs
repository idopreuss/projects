using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesInfo.API.Entities;
using GamesInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace GamesInfo.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().
                SetBasePath(env.ContentRootPath).
                AddJsonFile("appSettings.json", optional:false, reloadOnChange:true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().
                AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));

            //Adding a context (currently DB)
            services.AddSingleton<IGameInfoContext, GameInfoContext>();

            //Adding a Repository to handle all CRUD actions
            services.AddSingleton<IGameInfoRepository, GameInfoDBRepository>();

            //The fetcher that will wake up each n seconds and fetch the games.

            services.AddHostedService<GamesFetcher>();

            // The algorithm that fetcher will use (parse site, use API etc)
            services.AddTransient<IFetcherAlgorithm, FetcherKnownAPIAlgorithm>();


            //    AddJsonOptions(o =>
            //      {
            //       if (o.SerializerSettings.ContractResolver != null)
            //         {
            // In order not to make the property names lower case (default). instead it will take it from the actual names in the code.
            //            var castedResolver = o.SerializerSettings.ContractResolver
            //               as DefaultContractResolver;
            ////           castedResolver.NamingStrategy = null;
            //        }
            //   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            //Use NLOG 
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseMvc();


          //  app.Run ((context) =>
          //  {
          //      throw new Exception("Example Exception");
          //   });
           //  app.Run(async (context) =>
           // {
           //      await context.Response.WriteAsync("Hello World!");
           //   });
        }
    }
}
