using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDE.Themes.Data;
using IDE.Themes.Models;
using IDE.Themes.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IDE.Themes {

    public class Startup {

        //stores configuration
        public IConfiguration Configuration { get; }

        //Configuration holds the basic settings: URLs, content root, application name, environment
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        //Injects controllers with views scheme to the server
        public void ConfigureServices(IServiceCollection services) {


           //create a table if it's not contructed yet
           using (var context = new ApplicationDbContext(Configuration)) {

                context.Database.EnsureCreated();
           }

            services.AddControllersWithViews();
            services.AddSingleton<UserColorDataModel>();
            services.AddSingleton<ThemeConverter>();
            services.AddSingleton<ThemeDictionary>();
            services.AddSingleton<ColorStringConverter>();
            services.AddSingleton<HelperModel>();
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment) {

            //ForwardHeadersMiddleware https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-3.1#use-a-reverse-proxy-server
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseDeveloperExceptionPage();

            /*
            //if crash in development, show the HTML page with crash dump
            if (environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            //else crash in production, so redirect to error page
            else {
                app.UseExceptionHandler("/Home/Error");

                //Adds HSTS header. The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            */

            //redirects HTTP to HTTPS
            app.UseHttpsRedirection();

            //service of wwwroot files to the client
            app.UseStaticFiles();

            //matches HTTP requests and dispatches them to app's endpoints
            app.UseRouting();

            //shows the order of middleware authorization
            app.UseAuthorization();

            //specifies the route layout plus the default route
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
