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
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Abstractions for dependency injection.\r\n\r\nCommonly Used Types:\r\nMicrosoft.Extensions.DependencyInjection.IServiceCollection")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ActivatorUtilities
    {
        public static ObjectFactory CreateFactory(System.Type instanceType, System.Type[] argumentTypes) { throw null; }

        public static ObjectFactory<T> CreateFactory<T>(System.Type[] argumentTypes) { throw null; }

        public static object CreateInstance(System.IServiceProvider provider, System.Type instanceType, params object[] parameters) { throw null; }

        public static T CreateInstance<T>(System.IServiceProvider provider, params object[] parameters) { throw null; }

        public static object GetServiceOrCreateInstance(System.IServiceProvider provider, System.Type type) { throw null; }

        public static T GetServiceOrCreateInstance<T>(System.IServiceProvider provider) { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.All)]
    public partial class ActivatorUtilitiesConstructorAttribute : System.Attribute
    {
    }

    public readonly partial struct AsyncServiceScope : IServiceScope, System.IDisposable, System.IAsyncDisposable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public AsyncServiceScope(IServiceScope serviceScope) { }

        public System.IServiceProvider ServiceProvider { get { throw null; } }

        public readonly void Dispose() { }

        public readonly System.Threading.Tasks.ValueTask DisposeAsync() { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public partial class FromKeyedServicesAttribute : System.Attribute
    {
        public FromKeyedServicesAttribute(object key) { }

        public object Key { get { throw null; } }
    }

    public partial interface IKeyedServiceProvider : System.IServiceProvider
    {
        object? GetKeyedService(System.Type serviceType, object? serviceKey);
        object GetRequiredKeyedService(System.Type serviceType, object? serviceKey);
    }

    public partial interface IServiceCollection : System.Collections.Generic.IList<ServiceDescriptor>, System.Collections.Generic.ICollection<ServiceDescriptor>, System.Collections.Generic.IEnumerable<ServiceDescriptor>, System.Collections.IEnumerable
    {
    }

    public partial interface IServiceProviderFactory<TContainerBuilder>
    {
        TContainerBuilder CreateBuilder(IServiceCollection services);
        System.IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder);
    }

    public partial interface IServiceProviderIsKeyedService : IServiceProviderIsService
    {
        bool IsKeyedService(System.Type serviceType, object? serviceKey);
    }

    public partial interface IServiceProviderIsService
    {
        bool IsService(System.Type serviceType);
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

    public static partial class KeyedService
    {
        public static object AnyKey { get { throw null; } }
    }

    public delegate object ObjectFactory(System.IServiceProvider serviceProvider, object?[]? arguments);
    public delegate T ObjectFactory<T>(System.IServiceProvider serviceProvider, object?[]? arguments);
    public partial class ServiceCollection : IServiceCollection, System.Collections.Generic.IList<ServiceDescriptor>, System.Collections.Generic.ICollection<ServiceDescriptor>, System.Collections.Generic.IEnumerable<ServiceDescriptor>, System.Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public ServiceDescriptor this[int index] { get { throw null; } set { } }

        public void Clear() { }

        public bool Contains(ServiceDescriptor item) { throw null; }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) { }

        public System.Collections.Generic.IEnumerator<ServiceDescriptor> GetEnumerator() { throw null; }

        public int IndexOf(ServiceDescriptor item) { throw null; }

        public void Insert(int index, ServiceDescriptor item) { }

        public void MakeReadOnly() { }

        public bool Remove(ServiceDescriptor item) { throw null; }

        public void RemoveAt(int index) { }

        void System.Collections.Generic.ICollection<ServiceDescriptor>.Add(ServiceDescriptor item) { }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public static partial class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddKeyedScoped(this IServiceCollection services, System.Type serviceType, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { throw null; }

        public static IServiceCollection AddKeyedScoped(this IServiceCollection services, System.Type serviceType, object? serviceKey, System.Type implementationType) { throw null; }

        public static IServiceCollection AddKeyedScoped(this IServiceCollection services, System.Type serviceType, object? serviceKey) { throw null; }

        public static IServiceCollection AddKeyedScoped<TService>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedScoped<TService>(this IServiceCollection services, object? serviceKey)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedScoped<TService, TImplementation>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddKeyedScoped<TService, TImplementation>(this IServiceCollection services, object? serviceKey)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, System.Type serviceType, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { throw null; }

        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, System.Type serviceType, object? serviceKey, object implementationInstance) { throw null; }

        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, System.Type serviceType, object? serviceKey, System.Type implementationType) { throw null; }

        public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, System.Type serviceType, object? serviceKey) { throw null; }

        public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object? serviceKey, TService implementationInstance)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object? serviceKey)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedSingleton<TService, TImplementation>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddKeyedSingleton<TService, TImplementation>(this IServiceCollection services, object? serviceKey)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddKeyedTransient(this IServiceCollection services, System.Type serviceType, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { throw null; }

        public static IServiceCollection AddKeyedTransient(this IServiceCollection services, System.Type serviceType, object? serviceKey, System.Type implementationType) { throw null; }

        public static IServiceCollection AddKeyedTransient(this IServiceCollection services, System.Type serviceType, object? serviceKey) { throw null; }

        public static IServiceCollection AddKeyedTransient<TService>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedTransient<TService>(this IServiceCollection services, object? serviceKey)
            where TService : class { throw null; }

        public static IServiceCollection AddKeyedTransient<TService, TImplementation>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static IServiceCollection AddKeyedTransient<TService, TImplementation>(this IServiceCollection services, object? serviceKey)
            where TService : class where TImplementation : class, TService { throw null; }

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

        public ServiceDescriptor(System.Type serviceType, object? serviceKey, System.Func<System.IServiceProvider, object?, object> factory, ServiceLifetime lifetime) { }

        public ServiceDescriptor(System.Type serviceType, object? serviceKey, object instance) { }

        public ServiceDescriptor(System.Type serviceType, object? serviceKey, System.Type implementationType, ServiceLifetime lifetime) { }

        public ServiceDescriptor(System.Type serviceType, object instance) { }

        public ServiceDescriptor(System.Type serviceType, System.Type implementationType, ServiceLifetime lifetime) { }

        public System.Func<System.IServiceProvider, object>? ImplementationFactory { get { throw null; } }

        public object? ImplementationInstance { get { throw null; } }

        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public System.Type? ImplementationType { get { throw null; } }

        public bool IsKeyedService { get { throw null; } }

        public System.Func<System.IServiceProvider, object?, object>? KeyedImplementationFactory { get { throw null; } }

        public object? KeyedImplementationInstance { get { throw null; } }

        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public System.Type? KeyedImplementationType { get { throw null; } }

        public ServiceLifetime Lifetime { get { throw null; } }

        public object? ServiceKey { get { throw null; } }

        public System.Type ServiceType { get { throw null; } }

        public static ServiceDescriptor Describe(System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory, ServiceLifetime lifetime) { throw null; }

        public static ServiceDescriptor Describe(System.Type serviceType, System.Type implementationType, ServiceLifetime lifetime) { throw null; }

        public static ServiceDescriptor DescribeKeyed(System.Type serviceType, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory, ServiceLifetime lifetime) { throw null; }

        public static ServiceDescriptor DescribeKeyed(System.Type serviceType, object? serviceKey, System.Type implementationType, ServiceLifetime lifetime) { throw null; }

        public static ServiceDescriptor KeyedScoped(System.Type service, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { throw null; }

        public static ServiceDescriptor KeyedScoped(System.Type service, object? serviceKey, System.Type implementationType) { throw null; }

        public static ServiceDescriptor KeyedScoped<TService>(object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor KeyedScoped<TService, TImplementation>(object? serviceKey, System.Func<System.IServiceProvider, object?, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor KeyedScoped<TService, TImplementation>(object? serviceKey)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor KeyedSingleton(System.Type serviceType, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { throw null; }

        public static ServiceDescriptor KeyedSingleton(System.Type serviceType, object? serviceKey, object implementationInstance) { throw null; }

        public static ServiceDescriptor KeyedSingleton(System.Type service, object? serviceKey, System.Type implementationType) { throw null; }

        public static ServiceDescriptor KeyedSingleton<TService>(object? serviceKey, TService implementationInstance)
            where TService : class { throw null; }

        public static ServiceDescriptor KeyedSingleton<TService>(object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor KeyedSingleton<TService, TImplementation>(object? serviceKey, System.Func<System.IServiceProvider, object?, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor KeyedSingleton<TService, TImplementation>(object? serviceKey)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor KeyedTransient(System.Type service, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { throw null; }

        public static ServiceDescriptor KeyedTransient(System.Type service, object? serviceKey, System.Type implementationType) { throw null; }

        public static ServiceDescriptor KeyedTransient<TService>(object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor KeyedTransient<TService, TImplementation>(object? serviceKey, System.Func<System.IServiceProvider, object?, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor KeyedTransient<TService, TImplementation>(object? serviceKey)
            where TService : class where TImplementation : class, TService { throw null; }

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

        public override string ToString() { throw null; }

        public static ServiceDescriptor Transient(System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }

        public static ServiceDescriptor Transient(System.Type service, System.Type implementationType) { throw null; }

        public static ServiceDescriptor Transient<TService>(System.Func<System.IServiceProvider, TService> implementationFactory)
            where TService : class { throw null; }

        public static ServiceDescriptor Transient<TService, TImplementation>()
            where TService : class where TImplementation : class, TService { throw null; }

        public static ServiceDescriptor Transient<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public partial class ServiceKeyAttribute : System.Attribute
    {
    }

    public enum ServiceLifetime
    {
        Singleton = 0,
        Scoped = 1,
        Transient = 2
    }

    public static partial class ServiceProviderKeyedServiceExtensions
    {
        public static T? GetKeyedService<T>(this System.IServiceProvider provider, object? serviceKey) { throw null; }

        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for an IEnumerable<serviceType> might not be available at runtime.")]
        public static System.Collections.Generic.IEnumerable<object?> GetKeyedServices(this System.IServiceProvider provider, System.Type serviceType, object? serviceKey) { throw null; }

        public static System.Collections.Generic.IEnumerable<T> GetKeyedServices<T>(this System.IServiceProvider provider, object? serviceKey) { throw null; }

        public static object GetRequiredKeyedService(this System.IServiceProvider provider, System.Type serviceType, object? serviceKey) { throw null; }

        public static T GetRequiredKeyedService<T>(this System.IServiceProvider provider, object? serviceKey) { throw null; }
    }

    public static partial class ServiceProviderServiceExtensions
    {
        public static AsyncServiceScope CreateAsyncScope(this IServiceScopeFactory serviceScopeFactory) { throw null; }

        public static AsyncServiceScope CreateAsyncScope(this System.IServiceProvider provider) { throw null; }

        public static IServiceScope CreateScope(this System.IServiceProvider provider) { throw null; }

        public static object GetRequiredService(this System.IServiceProvider provider, System.Type serviceType) { throw null; }

        public static T GetRequiredService<T>(this System.IServiceProvider provider) { throw null; }

        public static T? GetService<T>(this System.IServiceProvider provider) { throw null; }

        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for an IEnumerable<serviceType> might not be available at runtime.")]
        public static System.Collections.Generic.IEnumerable<object?> GetServices(this System.IServiceProvider provider, System.Type serviceType) { throw null; }

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

        public static IServiceCollection RemoveAllKeyed(this IServiceCollection collection, System.Type serviceType, object? serviceKey) { throw null; }

        public static IServiceCollection RemoveAllKeyed<T>(this IServiceCollection collection, object? serviceKey) { throw null; }

        public static IServiceCollection Replace(this IServiceCollection collection, ServiceDescriptor descriptor) { throw null; }

        public static void TryAdd(this IServiceCollection collection, ServiceDescriptor descriptor) { }

        public static void TryAdd(this IServiceCollection collection, System.Collections.Generic.IEnumerable<ServiceDescriptor> descriptors) { }

        public static void TryAddEnumerable(this IServiceCollection services, ServiceDescriptor descriptor) { }

        public static void TryAddEnumerable(this IServiceCollection services, System.Collections.Generic.IEnumerable<ServiceDescriptor> descriptors) { }

        public static void TryAddKeyedScoped(this IServiceCollection collection, System.Type service, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { }

        public static void TryAddKeyedScoped(this IServiceCollection collection, System.Type service, object? serviceKey, System.Type implementationType) { }

        public static void TryAddKeyedScoped(this IServiceCollection collection, System.Type service, object? serviceKey) { }

        public static void TryAddKeyedScoped<TService>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { }

        public static void TryAddKeyedScoped<TService>(this IServiceCollection collection, object? serviceKey)
            where TService : class { }

        public static void TryAddKeyedScoped<TService, TImplementation>(this IServiceCollection collection, object? serviceKey)
            where TService : class where TImplementation : class, TService { }

        public static void TryAddKeyedSingleton(this IServiceCollection collection, System.Type service, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { }

        public static void TryAddKeyedSingleton(this IServiceCollection collection, System.Type service, object? serviceKey, System.Type implementationType) { }

        public static void TryAddKeyedSingleton(this IServiceCollection collection, System.Type service, object? serviceKey) { }

        public static void TryAddKeyedSingleton<TService>(this IServiceCollection collection, object? serviceKey, TService instance)
            where TService : class { }

        public static void TryAddKeyedSingleton<TService>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { }

        public static void TryAddKeyedSingleton<TService>(this IServiceCollection collection, object? serviceKey)
            where TService : class { }

        public static void TryAddKeyedSingleton<TService, TImplementation>(this IServiceCollection collection, object? serviceKey)
            where TService : class where TImplementation : class, TService { }

        public static void TryAddKeyedTransient(this IServiceCollection collection, System.Type service, object? serviceKey, System.Func<System.IServiceProvider, object?, object> implementationFactory) { }

        public static void TryAddKeyedTransient(this IServiceCollection collection, System.Type service, object? serviceKey, System.Type implementationType) { }

        public static void TryAddKeyedTransient(this IServiceCollection collection, System.Type service, object? serviceKey) { }

        public static void TryAddKeyedTransient<TService>(this IServiceCollection services, object? serviceKey, System.Func<System.IServiceProvider, object?, TService> implementationFactory)
            where TService : class { }

        public static void TryAddKeyedTransient<TService>(this IServiceCollection collection, object? serviceKey)
            where TService : class { }

        public static void TryAddKeyedTransient<TService, TImplementation>(this IServiceCollection collection, object? serviceKey)
            where TService : class where TImplementation : class, TService { }

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