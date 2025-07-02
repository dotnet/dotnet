using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Channel;
using FunctionalTests.Utils;

namespace FunctionalTests.WebApi.Tests
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
            var endpointAddress = new EndpointAddress();
            services.AddSingleton<EndpointAddress>(endpointAddress);
            services.AddSingleton(typeof(ITelemetryChannel), new InMemoryChannel() { EndpointAddress = endpointAddress.ConnectionString, DeveloperMode = true });
            services.AddApplicationInsightsTelemetry(InProcessServer.IKey);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                // Add the following route for porting Web API 2 controllers.
                routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
