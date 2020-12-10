using System;
using App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace App
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddTransient<HttpService>();
            services.AddMemoryCache();
            services.AddSession();
        }
        
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Users}/{action=ActivateUser}/{userId?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Condition}/{action=DeleteConditions}/{userId?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Treatment}/{action=DeleteTreatments}/{userId?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Reports}/{action=DeleteReport}/{rptId?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Reports}/{action=UpdateReportPost}/{rptId?}");
            });
        }
    }
}
