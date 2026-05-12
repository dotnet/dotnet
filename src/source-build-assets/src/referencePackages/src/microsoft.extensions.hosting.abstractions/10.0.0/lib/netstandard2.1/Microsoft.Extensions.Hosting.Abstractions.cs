// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = ".NET Standard 2.1")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Hosting.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Hosting and startup abstractions for applications.")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.25.52411")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.0+b0f34d51fccc69fd334253924abd8d6853fad7aa")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Hosting.Abstractions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionHostedServiceExtensions
    {
        public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services, System.Func<System.IServiceProvider, THostedService> implementationFactory) where THostedService : class, Hosting.IHostedService { throw null; }
        public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services) where THostedService : class, Hosting.IHostedService { throw null; }
    }
}

namespace Microsoft.Extensions.Hosting
{
    public abstract partial class BackgroundService : IHostedService, System.IDisposable
    {
        public virtual System.Threading.Tasks.Task? ExecuteTask { get { throw null; } }

        public virtual void Dispose() { }
        protected abstract System.Threading.Tasks.Task ExecuteAsync(System.Threading.CancellationToken stoppingToken);
        public virtual System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public virtual System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
    }

    [System.Obsolete("EnvironmentName has been deprecated. Use Microsoft.Extensions.Hosting.Environments instead.")]
    public static partial class EnvironmentName
    {
        public static readonly string Development;
        public static readonly string Production;
        public static readonly string Staging;
    }
    public static partial class Environments
    {
        public static readonly string Development;
        public static readonly string Production;
        public static readonly string Staging;
    }
    public sealed partial class HostAbortedException : System.Exception
    {
        public HostAbortedException() { }
        public HostAbortedException(string? message, System.Exception? innerException) { }
        public HostAbortedException(string? message) { }
    }

    public partial class HostBuilderContext
    {
        public HostBuilderContext(System.Collections.Generic.IDictionary<object, object> properties) { }
        public Configuration.IConfiguration Configuration { get { throw null; } set { } }
        public IHostEnvironment HostingEnvironment { get { throw null; } set { } }
        public System.Collections.Generic.IDictionary<object, object> Properties { get { throw null; } }
    }
    public static partial class HostDefaults
    {
        public static readonly string ApplicationKey;
        public static readonly string ContentRootKey;
        public static readonly string EnvironmentKey;
    }
    public static partial class HostEnvironmentEnvExtensions
    {
        public static bool IsDevelopment(this IHostEnvironment hostEnvironment) { throw null; }
        public static bool IsEnvironment(this IHostEnvironment hostEnvironment, string environmentName) { throw null; }
        public static bool IsProduction(this IHostEnvironment hostEnvironment) { throw null; }
        public static bool IsStaging(this IHostEnvironment hostEnvironment) { throw null; }
    }
    public static partial class HostingAbstractionsHostBuilderExtensions
    {
        public static IHost Start(this IHostBuilder hostBuilder) { throw null; }
        public static System.Threading.Tasks.Task<IHost> StartAsync(this IHostBuilder hostBuilder, System.Threading.CancellationToken cancellationToken = default) { throw null; }
    }
    public static partial class HostingAbstractionsHostExtensions
    {
        public static void Run(this IHost host) { }
        public static System.Threading.Tasks.Task RunAsync(this IHost host, System.Threading.CancellationToken token = default) { throw null; }
        public static void Start(this IHost host) { }
        public static System.Threading.Tasks.Task StopAsync(this IHost host, System.TimeSpan timeout) { throw null; }
        public static void WaitForShutdown(this IHost host) { }
        public static System.Threading.Tasks.Task WaitForShutdownAsync(this IHost host, System.Threading.CancellationToken token = default) { throw null; }
    }
    public static partial class HostingEnvironmentExtensions
    {
        public static bool IsDevelopment(this IHostingEnvironment hostingEnvironment) { throw null; }
        public static bool IsEnvironment(this IHostingEnvironment hostingEnvironment, string environmentName) { throw null; }
        public static bool IsProduction(this IHostingEnvironment hostingEnvironment) { throw null; }
        public static bool IsStaging(this IHostingEnvironment hostingEnvironment) { throw null; }
    }
    [System.Obsolete("IApplicationLifetime has been deprecated. Use Microsoft.Extensions.Hosting.IHostApplicationLifetime instead.")]
    public partial interface IApplicationLifetime
    {
        System.Threading.CancellationToken ApplicationStarted { get; }

        System.Threading.CancellationToken ApplicationStopped { get; }

        System.Threading.CancellationToken ApplicationStopping { get; }

        void StopApplication();
    }

    public partial interface IHost : System.IDisposable
    {
        System.IServiceProvider Services { get; }

        System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken = default);
    }

    public partial interface IHostApplicationBuilder
    {
        Configuration.IConfigurationManager Configuration { get; }

        IHostEnvironment Environment { get; }

        Logging.ILoggingBuilder Logging { get; }

        Diagnostics.Metrics.IMetricsBuilder Metrics { get; }

        System.Collections.Generic.IDictionary<object, object> Properties { get; }

        DependencyInjection.IServiceCollection Services { get; }

        void ConfigureContainer<TContainerBuilder>(DependencyInjection.IServiceProviderFactory<TContainerBuilder> factory, System.Action<TContainerBuilder>? configure = null);
    }

    public partial interface IHostApplicationLifetime
    {
        System.Threading.CancellationToken ApplicationStarted { get; }

        System.Threading.CancellationToken ApplicationStopped { get; }

        System.Threading.CancellationToken ApplicationStopping { get; }

        void StopApplication();
    }

    public partial interface IHostBuilder
    {
        System.Collections.Generic.IDictionary<object, object> Properties { get; }

        IHost Build();
        IHostBuilder ConfigureAppConfiguration(System.Action<HostBuilderContext, Configuration.IConfigurationBuilder> configureDelegate);
        IHostBuilder ConfigureContainer<TContainerBuilder>(System.Action<HostBuilderContext, TContainerBuilder> configureDelegate);
        IHostBuilder ConfigureHostConfiguration(System.Action<Configuration.IConfigurationBuilder> configureDelegate);
        IHostBuilder ConfigureServices(System.Action<HostBuilderContext, DependencyInjection.IServiceCollection> configureDelegate);
        IHostBuilder UseServiceProviderFactory<TContainerBuilder>(DependencyInjection.IServiceProviderFactory<TContainerBuilder> factory);
        IHostBuilder UseServiceProviderFactory<TContainerBuilder>(System.Func<HostBuilderContext, DependencyInjection.IServiceProviderFactory<TContainerBuilder>> factory);
    }

    public partial interface IHostedLifecycleService : IHostedService
    {
        System.Threading.Tasks.Task StartedAsync(System.Threading.CancellationToken cancellationToken);
        System.Threading.Tasks.Task StartingAsync(System.Threading.CancellationToken cancellationToken);
        System.Threading.Tasks.Task StoppedAsync(System.Threading.CancellationToken cancellationToken);
        System.Threading.Tasks.Task StoppingAsync(System.Threading.CancellationToken cancellationToken);
    }

    public partial interface IHostedService
    {
        System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken);
        System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken);
    }

    public partial interface IHostEnvironment
    {
        string ApplicationName { get; set; }

        FileProviders.IFileProvider ContentRootFileProvider { get; set; }

        string ContentRootPath { get; set; }

        string EnvironmentName { get; set; }
    }

    [System.Obsolete("IHostingEnvironment has been deprecated. Use Microsoft.Extensions.Hosting.IHostEnvironment instead.")]
    public partial interface IHostingEnvironment
    {
        string ApplicationName { get; set; }

        FileProviders.IFileProvider ContentRootFileProvider { get; set; }

        string ContentRootPath { get; set; }

        string EnvironmentName { get; set; }
    }

    public partial interface IHostLifetime
    {
        System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken);
        System.Threading.Tasks.Task WaitForStartAsync(System.Threading.CancellationToken cancellationToken);
    }
}