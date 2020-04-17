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
using ProtoActorWebApiDemo.DataAccess;
using ProtoActorWebApiDemo.DataAccess.CustomSqlMapper;
using ProtoActorWebApiDemo.Domain.ActorManager;

namespace ProtoActorWebApiDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAllActors();
            services.AddSingleton<ICustomSqlMapper, CustomSqlMapper>();
            services.AddSingleton<IDataAccessService, DataAccessService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Register all custom mapper
            var mapper = app.ApplicationServices.GetRequiredService<ICustomSqlMapper>();
            mapper.RegisterDapperDtoMap();
            var dataService = app.ApplicationServices.GetRequiredService<IDataAccessService>();
            dataService.InitData();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
