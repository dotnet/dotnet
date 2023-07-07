// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Configuration.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Configuration")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Implementation of key-value pair based configuration for Microsoft.Extensions.Configuration. Includes the memory configuration provider.")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Configuration")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Configuration
{
    public static partial class ChainedBuilderExtensions
    {
        public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config, bool shouldDisposeConfiguration) { throw null; }

        public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration config) { throw null; }
    }

    public partial class ChainedConfigurationProvider : IConfigurationProvider, System.IDisposable
    {
        public ChainedConfigurationProvider(ChainedConfigurationSource source) { }

        public void Dispose() { }

        public System.Collections.Generic.IEnumerable<string> GetChildKeys(System.Collections.Generic.IEnumerable<string> earlierKeys, string parentPath) { throw null; }

        public Primitives.IChangeToken GetReloadToken() { throw null; }

        public void Load() { }

        public void Set(string key, string value) { }

        public bool TryGet(string key, out string value) { throw null; }
    }

    public partial class ChainedConfigurationSource : IConfigurationSource
    {
        public IConfiguration Configuration { get { throw null; } set { } }

        public bool ShouldDisposeConfiguration { get { throw null; } set { } }

        public IConfigurationProvider Build(IConfigurationBuilder builder) { throw null; }
    }

    public partial class ConfigurationBuilder : IConfigurationBuilder
    {
        public System.Collections.Generic.IDictionary<string, object> Properties { get { throw null; } }

        public System.Collections.Generic.IList<IConfigurationSource> Sources { get { throw null; } }

        public IConfigurationBuilder Add(IConfigurationSource source) { throw null; }

        public IConfigurationRoot Build() { throw null; }
    }

    public partial class ConfigurationKeyComparer : System.Collections.Generic.IComparer<string>
    {
        public static ConfigurationKeyComparer Instance { get { throw null; } }

        public int Compare(string x, string y) { throw null; }
    }

    public sealed partial class ConfigurationManager : IConfigurationBuilder, IConfigurationRoot, IConfiguration, System.IDisposable
    {
        public string this[string key] { get { throw null; } set { } }

        System.Collections.Generic.IDictionary<string, object> IConfigurationBuilder.Properties { get { throw null; } }

        System.Collections.Generic.IList<IConfigurationSource> IConfigurationBuilder.Sources { get { throw null; } }

        System.Collections.Generic.IEnumerable<IConfigurationProvider> IConfigurationRoot.Providers { get { throw null; } }

        public void Dispose() { }

        public System.Collections.Generic.IEnumerable<IConfigurationSection> GetChildren() { throw null; }

        public IConfigurationSection GetSection(string key) { throw null; }

        Primitives.IChangeToken IConfiguration.GetReloadToken() { throw null; }

        IConfigurationBuilder IConfigurationBuilder.Add(IConfigurationSource source) { throw null; }

        IConfigurationRoot IConfigurationBuilder.Build() { throw null; }

        void IConfigurationRoot.Reload() { }
    }

    public abstract partial class ConfigurationProvider : IConfigurationProvider
    {
        protected System.Collections.Generic.IDictionary<string, string> Data { get { throw null; } set { } }

        public virtual System.Collections.Generic.IEnumerable<string> GetChildKeys(System.Collections.Generic.IEnumerable<string> earlierKeys, string parentPath) { throw null; }

        public Primitives.IChangeToken GetReloadToken() { throw null; }

        public virtual void Load() { }

        protected void OnReload() { }

        public virtual void Set(string key, string value) { }

        public override string ToString() { throw null; }

        public virtual bool TryGet(string key, out string value) { throw null; }
    }

    public partial class ConfigurationReloadToken : Primitives.IChangeToken
    {
        public bool ActiveChangeCallbacks { get { throw null; } }

        public bool HasChanged { get { throw null; } }

        public void OnReload() { }

        public System.IDisposable RegisterChangeCallback(System.Action<object> callback, object state) { throw null; }
    }

    public partial class ConfigurationRoot : IConfigurationRoot, IConfiguration, System.IDisposable
    {
        public ConfigurationRoot(System.Collections.Generic.IList<IConfigurationProvider> providers) { }

        public string this[string key] { get { throw null; } set { } }

        public System.Collections.Generic.IEnumerable<IConfigurationProvider> Providers { get { throw null; } }

        public void Dispose() { }

        public System.Collections.Generic.IEnumerable<IConfigurationSection> GetChildren() { throw null; }

        public Primitives.IChangeToken GetReloadToken() { throw null; }

        public IConfigurationSection GetSection(string key) { throw null; }

        public void Reload() { }
    }

    public partial class ConfigurationSection : IConfigurationSection, IConfiguration
    {
        public ConfigurationSection(IConfigurationRoot root, string path) { }

        public string this[string key] { get { throw null; } set { } }

        public string Key { get { throw null; } }

        public string Path { get { throw null; } }

        public string Value { get { throw null; } set { } }

        public System.Collections.Generic.IEnumerable<IConfigurationSection> GetChildren() { throw null; }

        public Primitives.IChangeToken GetReloadToken() { throw null; }

        public IConfigurationSection GetSection(string key) { throw null; }
    }

    public static partial class MemoryConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> initialData) { throw null; }

        public static IConfigurationBuilder AddInMemoryCollection(this IConfigurationBuilder configurationBuilder) { throw null; }
    }

    public abstract partial class StreamConfigurationProvider : ConfigurationProvider
    {
        public StreamConfigurationProvider(StreamConfigurationSource source) { }

        public StreamConfigurationSource Source { get { throw null; } }

        public override void Load() { }

        public abstract void Load(System.IO.Stream stream);
    }

    public abstract partial class StreamConfigurationSource : IConfigurationSource
    {
        public System.IO.Stream Stream { get { throw null; } set { } }

        public abstract IConfigurationProvider Build(IConfigurationBuilder builder);
    }
}

namespace Microsoft.Extensions.Configuration.Memory
{
    public partial class MemoryConfigurationProvider : ConfigurationProvider, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>>, System.Collections.IEnumerable
    {
        public MemoryConfigurationProvider(MemoryConfigurationSource source) { }

        public void Add(string key, string value) { }

        public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> GetEnumerator() { throw null; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class MemoryConfigurationSource : IConfigurationSource
    {
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> InitialData { get { throw null; } set { } }

        public IConfigurationProvider Build(IConfigurationBuilder builder) { throw null; }
    }
}