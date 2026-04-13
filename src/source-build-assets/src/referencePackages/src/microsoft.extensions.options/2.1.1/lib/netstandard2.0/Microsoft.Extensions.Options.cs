// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Options.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("CommitHash", "686e26b3a2999c841d736b689b0a1a768fdecbf8")]
[assembly: System.Reflection.AssemblyMetadata("BuildNumber", "30846")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation.")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides a strongly typed way of specifying and accessing settings using dependency injection.")]
[assembly: System.Reflection.AssemblyFileVersion("2.1.1.18157")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.1.1-rtm-30846")]
[assembly: System.Reflection.AssemblyProduct("Microsoft .NET Extensions")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Options")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.1.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class OptionsServiceCollectionExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services) { throw null; }

        public static Options.OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services, string name)
            where TOptions : class { throw null; }

        public static Options.OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services)
            where TOptions : class { throw null; }

        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, System.Action<TOptions> configureOptions)
            where TOptions : class { throw null; }

        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string name, System.Action<TOptions> configureOptions)
            where TOptions : class { throw null; }

        public static IServiceCollection ConfigureAll<TOptions>(this IServiceCollection services, System.Action<TOptions> configureOptions)
            where TOptions : class { throw null; }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, object configureInstance) { throw null; }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, System.Type configureType) { throw null; }

        public static IServiceCollection ConfigureOptions<TConfigureOptions>(this IServiceCollection services)
            where TConfigureOptions : class { throw null; }

        public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, System.Action<TOptions> configureOptions)
            where TOptions : class { throw null; }

        public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, string name, System.Action<TOptions> configureOptions)
            where TOptions : class { throw null; }

        public static IServiceCollection PostConfigureAll<TOptions>(this IServiceCollection services, System.Action<TOptions> configureOptions)
            where TOptions : class { throw null; }
    }
}

namespace Microsoft.Extensions.Options
{
    public partial class ConfigureNamedOptions<TOptions> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class
    {
        public ConfigureNamedOptions(string name, System.Action<TOptions> action) { }

        public System.Action<TOptions> Action { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep : class
    {
        public ConfigureNamedOptions(string name, TDep dependency, System.Action<TOptions, TDep> action) { }

        public System.Action<TOptions, TDep> Action { get { throw null; } }

        public TDep Dependency { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class
    {
        public ConfigureNamedOptions(string name, TDep1 dependency, TDep2 dependency2, System.Action<TOptions, TDep1, TDep2> action) { }

        public System.Action<TOptions, TDep1, TDep2> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class
    {
        public ConfigureNamedOptions(string name, TDep1 dependency, TDep2 dependency2, TDep3 dependency3, System.Action<TOptions, TDep1, TDep2, TDep3> action) { }

        public System.Action<TOptions, TDep1, TDep2, TDep3> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
    {
        public ConfigureNamedOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, System.Action<TOptions, TDep1, TDep2, TDep3, TDep4> action) { }

        public System.Action<TOptions, TDep1, TDep2, TDep3, TDep4> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public TDep4 Dependency4 { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> : IConfigureNamedOptions<TOptions>, IConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
    {
        public ConfigureNamedOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, TDep5 dependency5, System.Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> action) { }

        public System.Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public TDep4 Dependency4 { get { throw null; } }

        public TDep5 Dependency5 { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        public ConfigureOptions(System.Action<TOptions> action) { }

        public System.Action<TOptions> Action { get { throw null; } }

        public virtual void Configure(TOptions options) { }
    }

    public partial interface IConfigureNamedOptions<in TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        void Configure(string name, TOptions options);
    }

    public partial interface IConfigureOptions<in TOptions>
        where TOptions : class
    {
        void Configure(TOptions options);
    }

    public partial interface IOptionsChangeTokenSource<out TOptions>
    {
        string Name { get; }

        Primitives.IChangeToken GetChangeToken();
    }

    public partial interface IOptionsFactory<TOptions>
        where TOptions : class, new()
    {
        TOptions Create(string name);
    }

    public partial interface IOptionsMonitorCache<TOptions>
        where TOptions : class
    {
        void Clear();
        TOptions GetOrAdd(string name, System.Func<TOptions> createOptions);
        bool TryAdd(string name, TOptions options);
        bool TryRemove(string name);
    }

    public partial interface IOptionsMonitor<out TOptions>
    {
        TOptions CurrentValue { get; }

        TOptions Get(string name);
        System.IDisposable OnChange(System.Action<TOptions, string> listener);
    }

    public partial interface IOptionsSnapshot<out TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        TOptions Get(string name);
    }

    public partial interface IOptions<out TOptions>
        where TOptions : class, new()
    {
        TOptions Value { get; }
    }

    public partial interface IPostConfigureOptions<in TOptions>
        where TOptions : class
    {
        void PostConfigure(string name, TOptions options);
    }

    public static partial class Options
    {
        public static readonly string DefaultName;
        public static IOptions<TOptions> Create<TOptions>(TOptions options)
            where TOptions : class, new() { throw null; }
    }

    public partial class OptionsBuilder<TOptions>
        where TOptions : class
    {
        public OptionsBuilder(DependencyInjection.IServiceCollection services, string name) { }

        public string Name { get { throw null; } }

        public DependencyInjection.IServiceCollection Services { get { throw null; } }

        public virtual OptionsBuilder<TOptions> Configure(System.Action<TOptions> configureOptions) { throw null; }

        public virtual OptionsBuilder<TOptions> Configure<TDep>(System.Action<TOptions, TDep> configureOptions)
            where TDep : class { throw null; }

        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2>(System.Action<TOptions, TDep1, TDep2> configureOptions)
            where TDep1 : class where TDep2 : class { throw null; }

        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3>(System.Action<TOptions, TDep1, TDep2, TDep3> configureOptions)
            where TDep1 : class where TDep2 : class where TDep3 : class { throw null; }

        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4>(System.Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions)
            where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class { throw null; }

        public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4, TDep5>(System.Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions)
            where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class { throw null; }

        public virtual OptionsBuilder<TOptions> PostConfigure(System.Action<TOptions> configureOptions) { throw null; }

        public virtual OptionsBuilder<TOptions> PostConfigure<TDep>(System.Action<TOptions, TDep> configureOptions)
            where TDep : class { throw null; }

        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2>(System.Action<TOptions, TDep1, TDep2> configureOptions)
            where TDep1 : class where TDep2 : class { throw null; }

        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3>(System.Action<TOptions, TDep1, TDep2, TDep3> configureOptions)
            where TDep1 : class where TDep2 : class where TDep3 : class { throw null; }

        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4>(System.Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions)
            where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class { throw null; }

        public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4, TDep5>(System.Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions)
            where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class { throw null; }
    }

    public partial class OptionsCache<TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class
    {
        public void Clear() { }

        public virtual TOptions GetOrAdd(string name, System.Func<TOptions> createOptions) { throw null; }

        public virtual bool TryAdd(string name, TOptions options) { throw null; }

        public virtual bool TryRemove(string name) { throw null; }
    }

    public partial class OptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
    {
        public OptionsFactory(System.Collections.Generic.IEnumerable<IConfigureOptions<TOptions>> setups, System.Collections.Generic.IEnumerable<IPostConfigureOptions<TOptions>> postConfigures) { }

        public TOptions Create(string name) { throw null; }
    }

    public partial class OptionsManager<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        public OptionsManager(IOptionsFactory<TOptions> factory) { }

        public TOptions Value { get { throw null; } }

        public virtual TOptions Get(string name) { throw null; }
    }

    public static partial class OptionsMonitorExtensions
    {
        public static System.IDisposable OnChange<TOptions>(this IOptionsMonitor<TOptions> monitor, System.Action<TOptions> listener) { throw null; }
    }

    public partial class OptionsMonitor<TOptions> : IOptionsMonitor<TOptions> where TOptions : class, new()
    {
        public OptionsMonitor(IOptionsFactory<TOptions> factory, System.Collections.Generic.IEnumerable<IOptionsChangeTokenSource<TOptions>> sources, IOptionsMonitorCache<TOptions> cache) { }

        public TOptions CurrentValue { get { throw null; } }

        public virtual TOptions Get(string name) { throw null; }

        public System.IDisposable OnChange(System.Action<TOptions, string> listener) { throw null; }
    }

    public partial class OptionsWrapper<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        public OptionsWrapper(TOptions options) { }

        public TOptions Value { get { throw null; } }

        [System.Obsolete("This method is obsolete and will be removed in a future version.")]
        public void Add(string name, TOptions options) { }

        [System.Obsolete("This method is obsolete and will be removed in a future version.")]
        public TOptions Get(string name) { throw null; }

        [System.Obsolete("This method is obsolete and will be removed in a future version.")]
        public bool Remove(string name) { throw null; }
    }

    public partial class PostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions> where TOptions : class
    {
        public PostConfigureOptions(string name, System.Action<TOptions> action) { }

        public System.Action<TOptions> Action { get { throw null; } }

        public string Name { get { throw null; } }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep> : IPostConfigureOptions<TOptions> where TOptions : class where TDep : class
    {
        public PostConfigureOptions(string name, TDep dependency, System.Action<TOptions, TDep> action) { }

        public System.Action<TOptions, TDep> Action { get { throw null; } }

        public TDep Dependency { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2> : IPostConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class
    {
        public PostConfigureOptions(string name, TDep1 dependency, TDep2 dependency2, System.Action<TOptions, TDep1, TDep2> action) { }

        public System.Action<TOptions, TDep1, TDep2> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2, TDep3> : IPostConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class
    {
        public PostConfigureOptions(string name, TDep1 dependency, TDep2 dependency2, TDep3 dependency3, System.Action<TOptions, TDep1, TDep2, TDep3> action) { }

        public System.Action<TOptions, TDep1, TDep2, TDep3> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4> : IPostConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
    {
        public PostConfigureOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, System.Action<TOptions, TDep1, TDep2, TDep3, TDep4> action) { }

        public System.Action<TOptions, TDep1, TDep2, TDep3, TDep4> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public TDep4 Dependency4 { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> : IPostConfigureOptions<TOptions> where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
    {
        public PostConfigureOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, TDep5 dependency5, System.Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> action) { }

        public System.Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public TDep4 Dependency4 { get { throw null; } }

        public TDep5 Dependency5 { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }
}