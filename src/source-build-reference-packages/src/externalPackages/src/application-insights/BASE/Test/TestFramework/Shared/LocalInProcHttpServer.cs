#if NETCOREAPP
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

namespace Microsoft.ApplicationInsights.TestFramework
{
    public class LocalInProcHttpServer : IDisposable
    {
        private readonly IWebHost host;
        private readonly CancellationTokenSource cts;
        
        public int RequestCounter = 0;

        public Func<HttpContext, Task> ServerLogic;

        public Action<HttpContext> ServerSideAsserts;

        public LocalInProcHttpServer(string url)
        {
            this.cts = new CancellationTokenSource();
            this.host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(url)
                .Configure((app) =>
                {
                    app.Run(Server);
                })
                .Build();

            Task.Run(() => this.host.RunAsync(this.cts.Token));
        }

        private Task Server(HttpContext context)
        {
            Interlocked.Increment(ref this.RequestCounter);
            
            this.ServerSideAsserts?.Invoke(context);

            return this.ServerLogic(context);
        }

        public void Dispose()
        {
            this.cts.Cancel(false);
            try
            {
                this.host.Dispose();
            }
            catch (Exception)
            {
            }
        }

        public static LocalInProcHttpServer MakeRedirectServer(string url, string redirectUrl, TimeSpan cacheExpirationDuration)
        {
            return new LocalInProcHttpServer(url)
            {
                ServerLogic = async (httpContext) =>
                {
                    httpContext.Response.StatusCode = StatusCodes.Status308PermanentRedirect;
                    httpContext.Response.Headers.Add("Location", redirectUrl);

                    // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/middleware?view=aspnetcore-5.0
                    // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.headers.cachecontrolheadervalue?view=net-5.0
                    httpContext.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = cacheExpirationDuration,
                    };

                    await httpContext.Response.WriteAsync("redirect");
                },
            };
        }

        public static LocalInProcHttpServer MakeTargetServer(string url, string response)
        {
            return new LocalInProcHttpServer(url)
            {
                ServerLogic = async (httpContext) => await httpContext.Response.WriteAsync(response)
            };
        }
    }
}
#endif
