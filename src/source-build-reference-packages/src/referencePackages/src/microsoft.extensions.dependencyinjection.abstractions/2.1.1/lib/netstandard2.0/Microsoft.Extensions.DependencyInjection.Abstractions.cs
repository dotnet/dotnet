// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: AssemblyDescription("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: AssemblyDefaultAlias("Microsoft.Extensions.DependencyInjection.Abstractions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("2.1.1.18157")]
[assembly: AssemblyInformationalVersion("2.1.1.18157 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("2.1.1.0")]




namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ActivatorUtilities
    {
        public static Microsoft.Extensions.DependencyInjection.ObjectFactory CreateFactory(System.Type instanceType, System.Type[] argumentTypes) { throw null; }
        public static object CreateInstance(System.IServiceProvider provider, System.Type instanceType, params object[] parameters) { throw null; }
        public static T CreateInstance<T>(System.IServiceProvider provider, params object[] parameters) { throw null; }
        public static object GetServiceOrCreateInstance(System.IServiceProvider provider, System.Type type) { throw null; }
        public static T GetServiceOrCreateInstance<T>(System.IServiceProvider provider) { throw null; }
    }
    public partial class ActivatorUtilitiesConstructorAttribute : System.Attribute
    {
        public ActivatorUtilitiesConstructorAttribute() { }
    }
    public partial interface IServiceCollection : System.Collections.Generic.ICollection<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>, System.Collections.Generic.IEnumerable<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>, System.Collections.Generic.IList<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>, System.Collections.IEnumerable
    {
    }
    public partial interface IServiceProviderFactory<TContainerBuilder>
    {
        TContainerBuilder CreateBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection services);
        System.IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder);
    }
    public partial interface IServiceScope : System.IDisposable
    {
        System.IServiceProvider ServiceProvider { get; }
    }
    public partial interface IServiceScopeFactory
    {
        Microsoft.Extensions.DependencyInjection.IServiceScope CreateScope();
    }
    public partial interface ISupportRequiredService
    {
        object GetRequiredService(System.Type serviceType);
    }
    public delegate object ObjectFactory(System.IServiceProvider serviceProvider, object[] arguments);
    public static partial class ServiceCollectionServiceExtensions
    {
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, System.Type implementationType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScoped<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, object implementationInstance) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, System.Type implementationType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, TService implementationInstance) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSingleton<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Type serviceType, System.Type implementationType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddTransient<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService { throw null; }
    }
    public partial class ServiceDescriptor
    {
        public ServiceDescriptor(System.Type serviceType, System.Func<System.IServiceProvider, object> factory, Microsoft.Extensions.DependencyInjection.ServiceLifetime lifetime) { }
        public ServiceDescriptor(System.Type serviceType, object instance) { }
        public ServiceDescriptor(System.Type serviceType, System.Type implementationType, Microsoft.Extensions.DependencyInjection.ServiceLifetime lifetime) { }
        public System.Func<System.IServiceProvider, object> ImplementationFactory { get { throw null; } }
        public object ImplementationInstance { get { throw null; } }
        public System.Type ImplementationType { get { throw null; } }
        public Microsoft.Extensions.DependencyInjection.ServiceLifetime Lifetime { get { throw null; } }
        public System.Type ServiceType { get { throw null; } }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Describe(System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory, Microsoft.Extensions.DependencyInjection.ServiceLifetime lifetime) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Describe(System.Type serviceType, System.Type implementationType, Microsoft.Extensions.DependencyInjection.ServiceLifetime lifetime) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Scoped(System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Scoped(System.Type service, System.Type implementationType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Scoped<TService>(System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Scoped<TService, TImplementation>() where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Scoped<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton(System.Type serviceType, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton(System.Type serviceType, object implementationInstance) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton(System.Type service, System.Type implementationType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton<TService>(System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton<TService>(TService implementationInstance) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton<TService, TImplementation>() where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Singleton<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Transient(System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Transient(System.Type service, System.Type implementationType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Transient<TService>(System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Transient<TService, TImplementation>() where TService : class where TImplementation : class, TService { throw null; }
        public static Microsoft.Extensions.DependencyInjection.ServiceDescriptor Transient<TService, TImplementation>(System.Func<System.IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService { throw null; }
    }
    public enum ServiceLifetime
    {
        Singleton = 0,
        Scoped = 1,
        Transient = 2,
    }
    public static partial class ServiceProviderServiceExtensions
    {
        public static Microsoft.Extensions.DependencyInjection.IServiceScope CreateScope(this System.IServiceProvider provider) { throw null; }
        public static object GetRequiredService(this System.IServiceProvider provider, System.Type serviceType) { throw null; }
        public static T GetRequiredService<T>(this System.IServiceProvider provider) { throw null; }
        public static System.Collections.Generic.IEnumerable<object> GetServices(this System.IServiceProvider provider, System.Type serviceType) { throw null; }
        public static System.Collections.Generic.IEnumerable<T> GetServices<T>(this System.IServiceProvider provider) { throw null; }
        public static T GetService<T>(this System.IServiceProvider provider) { throw null; }
    }
}
namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static partial class ServiceCollectionDescriptorExtensions
    {
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection Add(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor descriptor) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection Add(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Collections.Generic.IEnumerable<Microsoft.Extensions.DependencyInjection.ServiceDescriptor> descriptors) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection RemoveAll(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type serviceType) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection RemoveAll<T>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) { throw null; }
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection Replace(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor descriptor) { throw null; }
        public static void TryAdd(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor descriptor) { }
        public static void TryAdd(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Collections.Generic.IEnumerable<Microsoft.Extensions.DependencyInjection.ServiceDescriptor> descriptors) { }
        public static void TryAddEnumerable(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.DependencyInjection.ServiceDescriptor descriptor) { }
        public static void TryAddEnumerable(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Collections.Generic.IEnumerable<Microsoft.Extensions.DependencyInjection.ServiceDescriptor> descriptors) { }
        public static void TryAddScoped(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service) { }
        public static void TryAddScoped(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { }
        public static void TryAddScoped(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service, System.Type implementationType) { }
        public static void TryAddScoped<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) where TService : class { }
        public static void TryAddScoped<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { }
        public static void TryAddScoped<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) where TService : class where TImplementation : class, TService { }
        public static void TryAddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service) { }
        public static void TryAddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { }
        public static void TryAddSingleton(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service, System.Type implementationType) { }
        public static void TryAddSingleton<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) where TService : class { }
        public static void TryAddSingleton<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { }
        public static void TryAddSingleton<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, TService instance) where TService : class { }
        public static void TryAddSingleton<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) where TService : class where TImplementation : class, TService { }
        public static void TryAddTransient(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service) { }
        public static void TryAddTransient(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service, System.Func<System.IServiceProvider, object> implementationFactory) { }
        public static void TryAddTransient(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection, System.Type service, System.Type implementationType) { }
        public static void TryAddTransient<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) where TService : class { }
        public static void TryAddTransient<TService>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider, TService> implementationFactory) where TService : class { }
        public static void TryAddTransient<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection collection) where TService : class where TImplementation : class, TService { }
    }
}
