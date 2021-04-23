using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Models.Interfaces;
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
using BlockchainDemonstratorApi.Models.Classes;

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
            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<BeerGameContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BeerGameContext")));

            services.AddCors(options =>
            {
                options.AddPolicy(name: BlockchainDemonstratorWebApp,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44313").AllowAnyHeader().AllowAnyMethod(); //TODO: use static URL
                    });
            });
            //services.AddSingleton<IGameRepository>()
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

            SeedData.Initialize(beerGameContext);

            app.UseCors(BlockchainDemonstratorWebApp);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
