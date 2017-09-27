using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TomHouben.AspNetCore.MongoDb;
using TomHouben.KULUurroosterfeed.ICalService;
using TomHouben.KULUurroosterfeed.HTMLParserServices;
using TomHouben.KULUurroosterfeed.Repositories;
using TomHouben.KULUurroosterfeed.Services;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed
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
            services.AddMongoConnection(Configuration.GetConnectionString("DefaultConnection"));
            services.RegisterRepositories();
            services.RegisterServices();
            services.RegisterICalService();
            services.RegisterHtmlParserServices();

            services.AddOptions();
            services.Configure<CalendarServiceOptions>(Configuration.GetSection("CalendarServiceOptions"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
