// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.DependencyInjection.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("CommitHash", "930037a4f8b74a9c1e30d881507a05bea0a7c2e0")]
[assembly: System.Reflection.AssemblyMetadata("BuildNumber", "30846")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation.")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Abstractions for dependency injection.\nCommonly used types:\nMicrosoft.Extensions.DependencyInjection.IServiceCollection")]
[assembly: System.Reflection.AssemblyFileVersion("2.1.1.18157")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.1.1-rtm-30846")]
[assembly: System.Reflection.AssemblyProduct("Microsoft .NET Extensions")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.1.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ActivatorUtilities
    {
        public static ObjectFactory CreateFactory(System.Type instanceType, System.Type[] argumentTypes) { throw null; }

        public static object CreateInstance(System.IServiceProvider provider, System.Type instanceType, params object[] parameters) { throw null; }

        public static T CreateInstance<T>(System.IServiceProvider provider, params object[] parameters) { throw null; }

        public static object GetServiceOrCreateInstance(System.IServiceProvider provider, System.Type type) { throw null; }

        public static T GetServiceOrCreateInstance<T>(System.IServiceProvider provider) { throw null; }
    }

    public partial class ActivatorUtilitiesConstructorAttribute : System.Attribute
    {
    }

    public partial interface IServiceCollection : System.Collections.Generic.IList<ServiceDescriptor>, System.Collections.Generic.ICollection<ServiceDescriptor>, System.Collections.Generic.IEnumerable<ServiceDescriptor>, System.Collections.IEnumerable
    {
    }

    public partial interface IServiceProviderFactory<TContainerBuilder>
    {
        TContainerBuilder CreateBuilder(IServiceCollection services);
        System.IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder);
    }

    public partial interface IServiceScope : System.IDisposable
    {
        System.IServiceProvider ServiceProvider { get; }
    }

    public partial interface IServiceScopeFactory
    {
        IServiceScope CreateScope();
    }

    public partial interface ISupportRequiredService
    {
        object GetRequiredService(System.Type serviceType);
    }

    public delegate object ObjectFactory(System.IServiceProvider serviceProvider, object[] arguments);
    public static partial class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddScoped(this IServiceCollection services, System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static IServiceCollection AddScoped(this IServiceCollection services, System.Type serviceType, System.Type implementationType) { throw null; }

        public static IServiceCollection AddScoped(this IServiceCollection services, System.Type serviceType) { throw null; }

        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static IServiceCollection AddScoped<TService>(this IServiceCollection services)
            where TService : class { throw null; }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddSingleton(this IServiceCollection services, System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static IServiceCollection AddSingleton(this IServiceCollection services, System.Type serviceType, object implementationInstance) { throw null; }

        public static IServiceCollection AddSingleton(this IServiceCollection services, System.Type serviceType, System.Type implementationType) { throw null; }

        public static IServiceCollection AddSingleton(this IServiceCollection services, System.Type serviceType) { throw null; }

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService implementationInstance)
            where TService : class { throw null; }

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services)
            where TService : class { throw null; }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddTransient(this IServiceCollection services, System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static IServiceCollection AddTransient(this IServiceCollection services, System.Type serviceType, System.Type implementationType) { throw null; }

        public static IServiceCollection AddTransient(this IServiceCollection services, System.Type serviceType) { throw null; }

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services)
            where TService : class { throw null; }

        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class where TImplementation : class, TService { throw null; }
    }

    public partial class ServiceDescriptor
    {
        public ServiceDescriptor(System.Type serviceType, System.Func<System.IServiceProvider, object> factory, ServiceLifetime lifetime) { }

        public ServiceDescriptor(System.Type serviceType, object instance) { }

        public ServiceDescriptor(System.Type serviceType, System.Type implementationType, ServiceLifetime lifetime) { }

        public System.Func<System.IServiceProvider, object> ImplementationFactory { get { throw null; } }

        public object ImplementationInstance { get { throw null; } }

        public System.Type ImplementationType { get { throw null; } }

        public ServiceLifetime Lifetime { get { throw null; } }

        public System.Type ServiceType { get { throw null; } }

        public static ServiceDescriptor Describe(System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory, ServiceLifetime lifetime) { throw null; }

        public static ServiceDescriptor Describe(System.Type serviceType, System.Type implementationType, ServiceLifetime lifetime) { throw null; }

        public static ServiceDescriptor Scoped(System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static ServiceDescriptor Scoped(System.Type service, System.Type implementationType) { throw null; }

        public static ServiceDescriptor Scoped<TService>(System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor Scoped<TService, TImplementation>()
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor Scoped<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor Singleton(System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static ServiceDescriptor Singleton(System.Type serviceType, object implementationInstance) { throw null; }

        public static ServiceDescriptor Singleton(System.Type service, System.Type implementationType) { throw null; }

        public static ServiceDescriptor Singleton<TService>(TService implementationInstance)
            where TService : class { throw null; }

        public static ServiceDescriptor Singleton<TService>(System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor Singleton<TService, TImplementation>()
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor Singleton<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor Transient(System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static ServiceDescriptor Transient(System.Type service, System.Type implementationType) { throw null; }

        public static ServiceDescriptor Transient<TService>(System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor Transient<TService, TImplementation>()
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor Transient<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }
    }

    public enum ServiceLifetime
    {
        Singleton = 0,
        Scoped = 1,
        Transient = 2
    }

    public static partial class ServiceProviderServiceExtensions
    {
        public static IServiceScope CreateScope(this System.IServiceProvider provider) { throw null; }

        public static object GetRequiredService(this System.IServiceProvider provider, System.Type serviceType) { throw null; }

        public static T GetRequiredService<T>(this System.IServiceProvider provider) { throw null; }

        public static T GetService<T>(this System.IServiceProvider provider) { throw null; }

        public static System.Collections.Generic.IEnumerable<object> GetServices(this System.IServiceProvider provider, System.Type serviceType) { throw null; }

        public static System.Collections.Generic.IEnumerable<T> GetServices<T>(this System.IServiceProvider provider) { throw null; }
    }
}

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static partial class ServiceCollectionDescriptorExtensions
    {
        public static IServiceCollection Add(this IServiceCollection collection, ServiceDescriptor descriptor) { throw null; }

        public static IServiceCollection Add(this IServiceCollection collection, System.Collections.Generic.IEnumerable<ServiceDescriptor> descriptors) { throw null; }

        public static IServiceCollection RemoveAll(this IServiceCollection collection, System.Type serviceType) { throw null; }

        public static IServiceCollection RemoveAll<T>(this IServiceCollection collection) { throw null; }

        public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor) { throw null; }

        public static void TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor) { }

        public static void TryAdd(this IServiceCollection collection, System.Collections.Generic.IEnumerable<ServiceDescriptor> descriptors) { }

        public static void TryAddEnumerable(this IServiceCollection services, ServiceDescriptor descriptor) { }

        public static void TryAddEnumerable(this IServiceCollection services, System.Collections.Generic.IEnumerable<ServiceDescriptor> descriptors) { }

        public static void TryAddScoped(this IServiceCollection collection, System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { }

        public static void TryAddScoped(this IServiceCollection collection, System.Type service, System.Type implementationType) { }

        public static void TryAddScoped(this IServiceCollection collection, System.Type service) { }

        public static void TryAddScoped<TService>(this IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { }

        public static void TryAddScoped<TService>(this IServiceCollection collection)
            where TService : class { }

        public static void TryAddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TService : class where TImplementation : class, TService { }

        public static void TryAddSingleton(this IServiceCollection collection, System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { }

        public static void TryAddSingleton(this IServiceCollection collection, System.Type service, System.Type implementationType) { }

        public static void TryAddSingleton(this IServiceCollection collection, System.Type service) { }

        public static void TryAddSingleton<TService>(this IServiceCollection collection, TService instance)
            where TService : class { }

        public static void TryAddSingleton<TService>(this IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { }

        public static void TryAddSingleton<TService>(this IServiceCollection collection)
            where TService : class { }

        public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class where TImplementation : class, TService { }

        public static void TryAddTransient(this IServiceCollection collection, System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { }

        public static void TryAddTransient(this IServiceCollection collection, System.Type service, System.Type implementationType) { }

        public static void TryAddTransient(this IServiceCollection collection, System.Type service) { }

        public static void TryAddTransient<TService>(this IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { }

        public static void TryAddTransient<TService>(this IServiceCollection collection)
            where TService : class { }

        public static void TryAddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TService : class where TImplementation : class, TService { }
    }
}