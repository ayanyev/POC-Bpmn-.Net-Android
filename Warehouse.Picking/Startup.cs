using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VueCliMiddleware;
using warehouse.picking.api.Hubs;
using Warehouse.Picking.Api.Hubs.Extensions;
using warehouse.picking.api.Processes.Extensions;
using Warehouse.Picking.Api.Repositories.Extensions;
using Warehouse.Picking.Api.Services.Extensions;
using Warehouse.Picking.Api.Utilities;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace Warehouse.Picking.Api
{
    
    public enum AppName { PickingApp, IntakeApp }
    
    public class Startup
    {

        private readonly AppName _appName;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)  
        {  
            Configuration = configuration;
            _appName = (AppName) Enum.Parse(typeof(AppName), Configuration["APP_NAME"], true);
        }  
  
        public IConfiguration Configuration { get; }  
  
        // This method gets called by the runtime. Use this method to add services to the container. 
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();  
            services.AddSignalR();
            
            services.AddScoped<AuthenticationEvents>();

            services
                .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication(
                    options =>
                    {
                        options.Realm = "My Application";
                        options.EventsType = typeof(AuthenticationEvents);
                    });
            
            services.AddRepositories(_appName);
            services.AddServices(_appName);
            services.AddProcessServices(_appName);
            services.AddHubServices(_appName);
            // connect vue app - middleware  
            services.AddSpaStaticFiles(options => 
                options.RootPath = "dashboard-app"
            );  
        }  
  
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)  
        {  
            if (env.IsDevelopment())  
            {  
                app.UseDeveloperExceptionPage();  
            }  
            else  
            {  
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
                app.UseHsts();  
            }  
  
            app.UseHttpsRedirection();  
  
            app.UseRouting();  
  
            app.UseAuthorization();

            app.UseDefaultFiles();
            
            app.UseAuthentication();
  
            app.UseEndpoints(endpoints =>  
            {  
                endpoints.MapControllers();
                switch (_appName)
                {
                    case AppName.PickingApp:
                        endpoints.MapHub<PickingDashboardHub>("/dashboardhub");
                        endpoints.MapHub<PickersHub>("/pickershub");
                        break;
                    case AppName.IntakeApp:
                        endpoints.MapHub<IntakeDashboardHub>("/intakedashboardhub");
                        endpoints.MapHub<DeviceHub>("/intakedevicehub");
                        break;
                }
            });  
  
            // use middleware and launch server for Vue  
            app.UseSpaStaticFiles();  
            app.UseSpa(spa =>  
            {  
                spa.Options.SourcePath = "dashboard-app";  
                if (env.IsDevelopment())  
                {
                    spa.UseVueCli(npmScript: "serve");
                }  
            });  
        }  
    }  
}