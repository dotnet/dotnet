// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Grpc.Core;
using Grpc.Core.Interceptors;
using OpenTelemetry.Instrumentation.GrpcCore;
using OpenTelemetry.Trace;

namespace Examples.GrpcCore.AspNetCore;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Wire in otel
        services.AddOpenTelemetry().WithTracing(
            builder => builder
                .AddAspNetCoreInstrumentation()
                .AddGrpcCoreInstrumentation()
                .AddConsoleExporter());

        // We are running an in-process gRPC Core service.
        services.AddHostedService<EchoGrpcHostedService>();

        // Add a singleton for the gRPC client to our local service.
        services.AddSingleton(provider =>
        {
            var channel = new Channel($"dns:localhost:{Program.GrpcServicePort}", ChannelCredentials.Insecure);

            var callInvoker = channel.CreateCallInvoker()
                .Intercept(new ClientTracingInterceptor(new ClientTracingInterceptorOptions()));

            return new Echo.EchoClient(callInvoker);
        });
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

    /// <summary>
    /// A hosted service wrapper for an in-process gRPC Core service.
    /// This gRPC service is instrumented using the server interceptor.
    /// </summary>
    private sealed class EchoGrpcHostedService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceDefinition = Echo.BindService(new EchoService())
                .Intercept(new ServerTracingInterceptor(new ServerTracingInterceptorOptions()));

            var server = new Server
            {
                Ports = { new ServerPort("localhost", Program.GrpcServicePort, ServerCredentials.Insecure) },
                Services = { serviceDefinition },
            };

            server.Start();

            var tcs = new TaskCompletionSource<bool>();

            var tokenRegistration = stoppingToken.Register(
                async () =>
                {
                    await server.ShutdownAsync().ConfigureAwait(false);
                    tcs.SetResult(true);
                });

            return tcs.Task.ContinueWith(antecedent => tokenRegistration.Dispose(), stoppingToken);
        }
    }
}
