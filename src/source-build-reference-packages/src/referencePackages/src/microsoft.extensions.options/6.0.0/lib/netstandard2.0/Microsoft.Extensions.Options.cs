// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Options.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Options")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides a strongly typed way of specifying and accessing settings using dependency injection.")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Options")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
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
    public partial class ConfigureNamedOptions<TOptions>
        where TOptions : class
    {
        public ConfigureNamedOptions(string name, System.Action<TOptions> action) { }

        public System.Action<TOptions> Action { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep>
        where TOptions : class where TDep : class
    {
        public ConfigureNamedOptions(string name, TDep dependency, System.Action<TOptions, TDep> action) { }

        public System.Action<TOptions, TDep> Action { get { throw null; } }

        public TDep Dependency { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2>
        where TOptions : class where TDep1 : class where TDep2 : class
    {
        public ConfigureNamedOptions(string name, TDep1 dependency, TDep2 dependency2, System.Action<TOptions, TDep1, TDep2> action) { }

        public System.Action<TOptions, TDep1, TDep2> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public string Name { get { throw null; } }

        public void Configure(TOptions options) { }

        public virtual void Configure(string name, TOptions options) { }
    }

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3>
        where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class
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

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4>
        where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
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

    public partial class ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>
        where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
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

    public partial class ConfigureOptions<TOptions>
        where TOptions : class
    {
        public ConfigureOptions(System.Action<TOptions> action) { }

        public System.Action<TOptions> Action { get { throw null; } }

        public virtual void Configure(TOptions options) { }
    }

    public partial interface IConfigureNamedOptions<in TOptions>
        where TOptions : class
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
        where TOptions : class
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

    public partial interface IOptionsSnapshot<out TOptions>
        where TOptions : class
    {
        TOptions Get(string name);
    }

    public partial interface IOptions<out TOptions>
        where TOptions : class
    {
        TOptions Value { get; }
    }

    public partial interface IPostConfigureOptions<in TOptions>
        where TOptions : class
    {
        void PostConfigure(string name, TOptions options);
    }

    public partial interface IValidateOptions<TOptions>
        where TOptions : class
    {
        ValidateOptionsResult Validate(string name, TOptions options);
    }

    public static partial class Options
    {
        public static readonly string DefaultName;
        public static IOptions<TOptions> Create<TOptions>(TOptions options)
            where TOptions : class { throw null; }
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

        public virtual OptionsBuilder<TOptions> Validate(System.Func<TOptions, bool> validation, string failureMessage) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate(System.Func<TOptions, bool> validation) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep>(System.Func<TOptions, TDep, bool> validation, string failureMessage) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep>(System.Func<TOptions, TDep, bool> validation) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2>(System.Func<TOptions, TDep1, TDep2, bool> validation, string failureMessage) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2>(System.Func<TOptions, TDep1, TDep2, bool> validation) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3>(System.Func<TOptions, TDep1, TDep2, TDep3, bool> validation, string failureMessage) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3>(System.Func<TOptions, TDep1, TDep2, TDep3, bool> validation) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4>(System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation, string failureMessage) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4>(System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4, TDep5>(System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation, string failureMessage) { throw null; }

        public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4, TDep5>(System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation) { throw null; }
    }

    public partial class OptionsCache<TOptions>
        where TOptions : class
    {
        public OptionsCache() { }

        public void Clear() { }

        public virtual TOptions GetOrAdd(string name, System.Func<TOptions> createOptions) { throw null; }

        public virtual bool TryAdd(string name, TOptions options) { throw null; }

        public virtual bool TryRemove(string name) { throw null; }
    }

    public partial class OptionsFactory<TOptions>
        where TOptions : class
    {
        public OptionsFactory(System.Collections.Generic.IEnumerable<IConfigureOptions<TOptions>> setups, System.Collections.Generic.IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, System.Collections.Generic.IEnumerable<IValidateOptions<TOptions>> validations) { }

        public OptionsFactory(System.Collections.Generic.IEnumerable<IConfigureOptions<TOptions>> setups, System.Collections.Generic.IEnumerable<IPostConfigureOptions<TOptions>> postConfigures) { }

        public TOptions Create(string name) { throw null; }

        protected virtual TOptions CreateInstance(string name) { throw null; }
    }

    public partial class OptionsManager<TOptions>
        where TOptions : class
    {
        public OptionsManager(IOptionsFactory<TOptions> factory) { }

        public TOptions Value { get { throw null; } }

        public virtual TOptions Get(string name) { throw null; }
    }

    public static partial class OptionsMonitorExtensions
    {
        public static System.IDisposable OnChange<TOptions>(this IOptionsMonitor<TOptions> monitor, System.Action<TOptions> listener) { throw null; }
    }

    public partial class OptionsMonitor<TOptions> : System.IDisposable where TOptions : class
    {
        public OptionsMonitor(IOptionsFactory<TOptions> factory, System.Collections.Generic.IEnumerable<IOptionsChangeTokenSource<TOptions>> sources, IOptionsMonitorCache<TOptions> cache) { }

        public TOptions CurrentValue { get { throw null; } }

        public void Dispose() { }

        public virtual TOptions Get(string name) { throw null; }

        public System.IDisposable OnChange(System.Action<TOptions, string> listener) { throw null; }
    }

    public partial class OptionsValidationException : System.Exception
    {
        public OptionsValidationException(string optionsName, System.Type optionsType, System.Collections.Generic.IEnumerable<string> failureMessages) { }

        public System.Collections.Generic.IEnumerable<string> Failures { get { throw null; } }

        public override string Message { get { throw null; } }

        public string OptionsName { get { throw null; } }

        public System.Type OptionsType { get { throw null; } }
    }

    public partial class OptionsWrapper<TOptions>
        where TOptions : class
    {
        public OptionsWrapper(TOptions options) { }

        public TOptions Value { get { throw null; } }
    }

    public partial class PostConfigureOptions<TOptions>
        where TOptions : class
    {
        public PostConfigureOptions(string name, System.Action<TOptions> action) { }

        public System.Action<TOptions> Action { get { throw null; } }

        public string Name { get { throw null; } }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep>
        where TOptions : class where TDep : class
    {
        public PostConfigureOptions(string name, TDep dependency, System.Action<TOptions, TDep> action) { }

        public System.Action<TOptions, TDep> Action { get { throw null; } }

        public TDep Dependency { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2>
        where TOptions : class where TDep1 : class where TDep2 : class
    {
        public PostConfigureOptions(string name, TDep1 dependency, TDep2 dependency2, System.Action<TOptions, TDep1, TDep2> action) { }

        public System.Action<TOptions, TDep1, TDep2> Action { get { throw null; } }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public string Name { get { throw null; } }

        public void PostConfigure(TOptions options) { }

        public virtual void PostConfigure(string name, TOptions options) { }
    }

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2, TDep3>
        where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class
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

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4>
        where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
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

    public partial class PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>
        where TOptions : class where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
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

    public partial class ValidateOptionsResult
    {
        public static readonly ValidateOptionsResult Skip;
        public static readonly ValidateOptionsResult Success;
        public ValidateOptionsResult() { }

        public bool Failed { get { throw null; } protected set { } }

        public string FailureMessage { get { throw null; } protected set { } }

        public System.Collections.Generic.IEnumerable<string> Failures { get { throw null; } protected set { } }

        public bool Skipped { get { throw null; } protected set { } }

        public bool Succeeded { get { throw null; } protected set { } }

        public static ValidateOptionsResult Fail(System.Collections.Generic.IEnumerable<string> failures) { throw null; }

        public static ValidateOptionsResult Fail(string failureMessage) { throw null; }
    }

    public partial class ValidateOptions<TOptions>
        where TOptions : class
    {
        public ValidateOptions(string name, System.Func<TOptions, bool> validation, string failureMessage) { }

        public string FailureMessage { get { throw null; } }

        public string Name { get { throw null; } }

        public System.Func<TOptions, bool> Validation { get { throw null; } }

        public ValidateOptionsResult Validate(string name, TOptions options) { throw null; }
    }

    public partial class ValidateOptions<TOptions, TDep>
        where TOptions : class
    {
        public ValidateOptions(string name, TDep dependency, System.Func<TOptions, TDep, bool> validation, string failureMessage) { }

        public TDep Dependency { get { throw null; } }

        public string FailureMessage { get { throw null; } }

        public string Name { get { throw null; } }

        public System.Func<TOptions, TDep, bool> Validation { get { throw null; } }

        public ValidateOptionsResult Validate(string name, TOptions options) { throw null; }
    }

    public partial class ValidateOptions<TOptions, TDep1, TDep2>
        where TOptions : class
    {
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, System.Func<TOptions, TDep1, TDep2, bool> validation, string failureMessage) { }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public string FailureMessage { get { throw null; } }

        public string Name { get { throw null; } }

        public System.Func<TOptions, TDep1, TDep2, bool> Validation { get { throw null; } }

        public ValidateOptionsResult Validate(string name, TOptions options) { throw null; }
    }

    public partial class ValidateOptions<TOptions, TDep1, TDep2, TDep3>
        where TOptions : class
    {
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, System.Func<TOptions, TDep1, TDep2, TDep3, bool> validation, string failureMessage) { }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public string FailureMessage { get { throw null; } }

        public string Name { get { throw null; } }

        public System.Func<TOptions, TDep1, TDep2, TDep3, bool> Validation { get { throw null; } }

        public ValidateOptionsResult Validate(string name, TOptions options) { throw null; }
    }

    public partial class ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4>
        where TOptions : class
    {
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation, string failureMessage) { }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public TDep4 Dependency4 { get { throw null; } }

        public string FailureMessage { get { throw null; } }

        public string Name { get { throw null; } }

        public System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> Validation { get { throw null; } }

        public ValidateOptionsResult Validate(string name, TOptions options) { throw null; }
    }

    public partial class ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>
        where TOptions : class
    {
        public ValidateOptions(string name, TDep1 dependency1, TDep2 dependency2, TDep3 dependency3, TDep4 dependency4, TDep5 dependency5, System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation, string failureMessage) { }

        public TDep1 Dependency1 { get { throw null; } }

        public TDep2 Dependency2 { get { throw null; } }

        public TDep3 Dependency3 { get { throw null; } }

        public TDep4 Dependency4 { get { throw null; } }

        public TDep5 Dependency5 { get { throw null; } }

        public string FailureMessage { get { throw null; } }

        public string Name { get { throw null; } }

        public System.Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> Validation { get { throw null; } }

        public ValidateOptionsResult Validate(string name, TOptions options) { throw null; }
    }
}