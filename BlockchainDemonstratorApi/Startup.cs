using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Hubs;
using BlockchainDemonstratorApi.Models.Classes;
using Newtonsoft.Json.Serialization;

namespace BlockchainDemonstratorApi
{
    public class Startup
    {
        readonly string BlockchainDemonstratorWebApp = "_blockchainDemonstratorWebApp";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddDbContext<BeerGameContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BeerGameContext")));

            services.AddCors(options =>
            {
                options.AddPolicy(name: BlockchainDemonstratorWebApp,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44313").AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials();
                        builder.WithOrigins("https://142.93.130.201:5003").AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials();
                    });
            });
            
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BeerGameContext beerGameContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(BlockchainDemonstratorWebApp);

            SeedData.Initialize(beerGameContext);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/GameHub");
            });
        }
    }
}
