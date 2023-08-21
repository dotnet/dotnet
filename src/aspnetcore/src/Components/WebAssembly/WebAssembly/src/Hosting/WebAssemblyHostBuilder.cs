// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting;

/// <summary>
/// A builder for configuring and creating a <see cref="WebAssemblyHost"/>.
/// </summary>
public sealed class WebAssemblyHostBuilder
{
    private readonly JsonSerializerOptions _jsonOptions;
    private Func<IServiceProvider> _createServiceProvider;
    private RootComponentTypeCache? _rootComponentCache;
    private string? _persistedState;

    /// <summary>
    /// Creates an instance of <see cref="WebAssemblyHostBuilder"/> using the most common
    /// conventions and settings.
    /// </summary>
    /// <param name="args">The argument passed to the application's main method.</param>
    /// <returns>A <see cref="WebAssemblyHostBuilder"/>.</returns>
    [DynamicDependency(nameof(JSInteropMethods.NotifyLocationChanged), typeof(JSInteropMethods))]
    [DynamicDependency(nameof(JSInteropMethods.NotifyLocationChangingAsync), typeof(JSInteropMethods))]
    [DynamicDependency(JsonSerialized, typeof(WebEventDescriptor))]
    // The following dependency prevents HeadOutlet from getting trimmed away in
    // WebAssembly prerendered apps.
    [DynamicDependency(Component, typeof(HeadOutlet))]
    public static WebAssemblyHostBuilder CreateDefault(string[]? args = default)
    {
        // We don't use the args for anything right now, but we want to accept them
        // here so that it shows up this way in the project templates.
        var builder = new WebAssemblyHostBuilder(
            InternalJSImportMethods.Instance,
            DefaultWebAssemblyJSRuntime.Instance.ReadJsonSerializerOptions());

        WebAssemblyCultureProvider.Initialize();

        // Right now we don't have conventions or behaviors that are specific to this method
        // however, making this the default for the template allows us to add things like that
        // in the future, while giving `new WebAssemblyHostBuilder` as an opt-out of opinionated
        // settings.
        return builder;
    }

    /// <summary>
    /// Creates an instance of <see cref="WebAssemblyHostBuilder"/> with the minimal configuration.
    /// </summary>
    internal WebAssemblyHostBuilder(
        IInternalJSImportMethods jsMethods,
        JsonSerializerOptions jsonOptions)
    {
        // Private right now because we don't have much reason to expose it. This can be exposed
        // in the future if we want to give people a choice between CreateDefault and something
        // less opinionated.
        _jsonOptions = jsonOptions;
        Configuration = new WebAssemblyHostConfiguration();
        RootComponents = new RootComponentMappingCollection();
        Services = new ServiceCollection();
        Logging = new LoggingBuilder(Services);

        // Retrieve required attributes from JSRuntimeInvoker
        InitializeNavigationManager(jsMethods);
        InitializeRegisteredRootComponents(jsMethods);
        InitializePersistedState(jsMethods);
        InitializeDefaultServices();

        var hostEnvironment = InitializeEnvironment(jsMethods);
        HostEnvironment = hostEnvironment;

        _createServiceProvider = () =>
        {
            return Services.BuildServiceProvider(validateScopes: WebAssemblyHostEnvironmentExtensions.IsDevelopment(hostEnvironment));
        };
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Root components are expected to be defined in assemblies that do not get trimmed.")]
    private void InitializeRegisteredRootComponents(IInternalJSImportMethods jsMethods)
    {
        var componentsCount = jsMethods.RegisteredComponents_GetRegisteredComponentsCount();
        if (componentsCount == 0)
        {
            return;
        }

        var registeredComponents = new ComponentMarker[componentsCount];
        for (var i = 0; i < componentsCount; i++)
        {
            var assembly = jsMethods.RegisteredComponents_GetAssembly(i);
            var typeName = jsMethods.RegisteredComponents_GetTypeName(i);
            var serializedParameterDefinitions = jsMethods.RegisteredComponents_GetParameterDefinitions(i);
            var serializedParameterValues = jsMethods.RegisteredComponents_GetParameterValues(i);
            registeredComponents[i] = ComponentMarker.Create(ComponentMarker.WebAssemblyMarkerType, false, null);
            registeredComponents[i].WriteWebAssemblyData(
                assembly,
                typeName,
                serializedParameterDefinitions,
                serializedParameterValues);
            registeredComponents[i].PrerenderId = i.ToString(CultureInfo.InvariantCulture);
        }

        _rootComponentCache = new RootComponentTypeCache();
        var componentDeserializer = WebAssemblyComponentParameterDeserializer.Instance;
        foreach (var registeredComponent in registeredComponents)
        {
            var componentType = _rootComponentCache.GetRootComponent(registeredComponent.Assembly!, registeredComponent.TypeName!);
            if (componentType is null)
            {
                throw new InvalidOperationException(
                    $"Root component type '{registeredComponent.TypeName}' could not be found in the assembly '{registeredComponent.Assembly}'. " +
                    $"This is likely a result of trimming (tree shaking).");
            }

            var definitions = WebAssemblyComponentParameterDeserializer.GetParameterDefinitions(registeredComponent.ParameterDefinitions!);
            var values = WebAssemblyComponentParameterDeserializer.GetParameterValues(registeredComponent.ParameterValues!);
            var parameters = componentDeserializer.DeserializeParameters(definitions, values);

            RootComponents.Add(componentType, registeredComponent.PrerenderId!, parameters);
        }
    }

    private void InitializePersistedState(IInternalJSImportMethods jsMethods)
    {
        _persistedState = jsMethods.GetPersistedState();
    }

    private static void InitializeNavigationManager(IInternalJSImportMethods jsMethods)
    {
        var baseUri = jsMethods.NavigationManager_GetBaseUri();
        var uri = jsMethods.NavigationManager_GetLocationHref();

        WebAssemblyNavigationManager.Instance = new WebAssemblyNavigationManager(baseUri, uri);
    }

    private WebAssemblyHostEnvironment InitializeEnvironment(IInternalJSImportMethods jsMethods)
    {
        var applicationEnvironment = jsMethods.GetApplicationEnvironment();
        var hostEnvironment = new WebAssemblyHostEnvironment(applicationEnvironment, WebAssemblyNavigationManager.Instance.BaseUri);

        Services.AddSingleton<IWebAssemblyHostEnvironment>(hostEnvironment);

        var configFiles = new[]
        {
            "appsettings.json",
            $"appsettings.{applicationEnvironment}.json"
        };

        foreach (var configFile in configFiles)
        {
            if (File.Exists(configFile))
            {
                var appSettingsJson = File.ReadAllBytes(configFile);

                // Perf: Using this over AddJsonStream. This allows the linker to trim out the "File"-specific APIs and assemblies
                // for Configuration, of where there are several.
                Configuration.Add<JsonStreamConfigurationSource>(s => s.Stream = new MemoryStream(appSettingsJson));
            }
        }

        return hostEnvironment;
    }

    /// <summary>
    /// Gets an <see cref="WebAssemblyHostConfiguration"/> that can be used to customize the application's
    /// configuration sources and read configuration attributes.
    /// </summary>
    public WebAssemblyHostConfiguration Configuration { get; }

    /// <summary>
    /// Gets the collection of root component mappings configured for the application.
    /// </summary>
    public RootComponentMappingCollection RootComponents { get; }

    /// <summary>
    /// Gets the service collection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets information about the app's host environment.
    /// </summary>
    public IWebAssemblyHostEnvironment HostEnvironment { get; }

    /// <summary>
    /// Gets the logging builder for configuring logging services.
    /// </summary>
    public ILoggingBuilder Logging { get; }

    /// <summary>
    /// Registers a <see cref="IServiceProviderFactory{TBuilder}" /> instance to be used to create the <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="factory">The <see cref="IServiceProviderFactory{TBuilder}" />.</param>
    /// <param name="configure">
    /// A delegate used to configure the <typeparamref T="TBuilder" />. This can be used to configure services using
    /// APIS specific to the <see cref="IServiceProviderFactory{TBuilder}" /> implementation.
    /// </param>
    /// <typeparam name="TBuilder">The type of builder provided by the <see cref="IServiceProviderFactory{TBuilder}" />.</typeparam>
    /// <remarks>
    /// <para>
    /// <see cref="ConfigureContainer{TBuilder}(IServiceProviderFactory{TBuilder}, Action{TBuilder})"/> is called by <see cref="Build"/>
    /// and so the delegate provided by <paramref name="configure"/> will run after all other services have been registered.
    /// </para>
    /// <para>
    /// Multiple calls to <see cref="ConfigureContainer{TBuilder}(IServiceProviderFactory{TBuilder}, Action{TBuilder})"/> will replace
    /// the previously stored <paramref name="factory"/> and <paramref name="configure"/> delegate.
    /// </para>
    /// </remarks>
    public void ConfigureContainer<TBuilder>(IServiceProviderFactory<TBuilder> factory, Action<TBuilder>? configure = null) where TBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(factory);

        _createServiceProvider = () =>
        {
            var container = factory.CreateBuilder(Services);
            configure?.Invoke(container);
            return factory.CreateServiceProvider(container);
        };
    }

    /// <summary>
    /// Builds a <see cref="WebAssemblyHost"/> instance based on the configuration of this builder.
    /// </summary>
    /// <returns>A <see cref="WebAssemblyHost"/> object.</returns>
    public WebAssemblyHost Build()
    {
        // Intentionally overwrite configuration with the one we're creating.
        Services.AddSingleton<IConfiguration>(Configuration);

        // A Blazor application always runs in a scope. Since we want to make it possible for the user
        // to configure services inside *that scope* inside their startup code, we create *both* the
        // service provider and the scope here.
        var services = _createServiceProvider();
        var scope = services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();

        return new WebAssemblyHost(this, services, scope, _persistedState);
    }

    internal void InitializeDefaultServices()
    {
        Services.AddSingleton<IJSRuntime>(DefaultWebAssemblyJSRuntime.Instance);
        Services.AddSingleton<NavigationManager>(WebAssemblyNavigationManager.Instance);
        Services.AddSingleton<INavigationInterception>(WebAssemblyNavigationInterception.Instance);
        Services.AddSingleton<IScrollToLocationHash>(WebAssemblyScrollToLocationHash.Instance);
        Services.AddSingleton(new LazyAssemblyLoader(DefaultWebAssemblyJSRuntime.Instance));
        Services.AddSingleton<RootComponentTypeCache>(_ => _rootComponentCache ?? new());
        Services.AddSingleton<ComponentStatePersistenceManager>();
        Services.AddSingleton<PersistentComponentState>(sp => sp.GetRequiredService<ComponentStatePersistenceManager>().State);
        Services.AddSingleton<AntiforgeryStateProvider, DefaultAntiforgeryStateProvider>();
        Services.AddSingleton<IErrorBoundaryLogger, WebAssemblyErrorBoundaryLogger>();
        Services.AddLogging(builder =>
        {
            builder.AddProvider(new WebAssemblyConsoleLoggerProvider(DefaultWebAssemblyJSRuntime.Instance));
        });
        Services.AddSupplyValueFromQueryProvider();
    }
}
