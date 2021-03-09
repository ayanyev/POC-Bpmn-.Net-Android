using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POCPicking.Hubs;
using POCPicking.Processes.Extensions;
using POCPicking.Processes.Rest;
using POCPicking.Repositories;
using POCPicking.Repositories.Extensions;
using VueCliMiddleware;

namespace POCPicking
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
            services.AddControllers();  
            services.AddSignalR();
            services.AddRepositories();
            services.AddSingleton(new ProcessesHttpClient());
            services.AddExternalTaskHandlers();
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
  
            app.UseEndpoints(endpoints =>  
            {  
                endpoints.MapControllers();
                endpoints.MapHub<DashboardHub>("/dashboardhub");
                endpoints.MapHub<PickersHub>("/pickershub");
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