namespace AspNetCoreWebApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            services.AddControllersWithViews();

            // Add custom TelemetryInitializer
            services.AddSingleton<ITelemetryInitializer, MyCustomTelemetryInitializer>();

            // Add custom TelemetryProcessor
            services.AddApplicationInsightsTelemetryProcessor<MyCustomTelemetryProcessor>();

            // Use this for any other configurations not covered by extension methods.
            services.Configure<TelemetryConfiguration>(config =>
            {
                
            });

            // Add and initialize the Application Insights SDK.
            services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000"
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        internal class MyCustomTelemetryInitializer : ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
                // Replace with actual properties.
                (telemetry as ISupportProperties).Properties["MyCustomKey"] = "MyCustomValue";
            }
        }

        internal class MyCustomTelemetryProcessor : ITelemetryProcessor
        {
            ITelemetryProcessor next;

            public MyCustomTelemetryProcessor(ITelemetryProcessor next)
            {
                this.next = next;
            }

            public void Process(ITelemetry item)
            {
                // Example processor - not filtering out anything.
                // This should be replaced with actual logic.
                this.next.Process(item);
            }
        }
    }
}
