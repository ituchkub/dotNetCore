using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UsersApi.Business.Managers.Implementations;
using UsersApi.Business.Managers.Interfaces;
using UsersApi.DataAccess.DataAccess.Implementations;
using UsersApi.DataAccess.DataAccess.Interfaces;
using UsersApi.DataAccess.Repositories;
using UsersApi.DataAccess.Repositories.Implementations;
using UsersApi.DataAccess.Repositories.Interfaces;
using UsersApi.DataAccess.Settings;

namespace UsersApi
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
            services.Configure<MongoDbSettings>(Configuration.GetSection(nameof(MongoDbSettings)));
            services.AddSingleton<IMongoDbSettings>(x => x.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddControllers();

            UsersApi.Business.Binder.Setup(services);
            UsersApi.DataAccess.Binder.Setup(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
