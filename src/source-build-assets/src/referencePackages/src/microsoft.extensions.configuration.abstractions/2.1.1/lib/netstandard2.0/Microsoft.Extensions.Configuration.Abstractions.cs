// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("CommitHash", "2679cfd94694a29230726708717ab9c4579a5d80")]
[assembly: System.Reflection.AssemblyMetadata("BuildNumber", "30846")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation.")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Abstractions of key-value pair based configuration.\nCommonly used types:\nMicrosoft.Extensions.Configuration.IConfiguration\nMicrosoft.Extensions.Configuration.IConfigurationBuilder\nMicrosoft.Extensions.Configuration.IConfigurationProvider\nMicrosoft.Extensions.Configuration.IConfigurationRoot\nMicrosoft.Extensions.Configuration.IConfigurationSection")]
[assembly: System.Reflection.AssemblyFileVersion("2.1.1.18157")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.1.1-rtm-30846")]
[assembly: System.Reflection.AssemblyProduct("Microsoft .NET Extensions")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Configuration.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.1.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Configuration
{
    public static partial class ConfigurationExtensions
    {
        public static IConfigurationBuilder Add<TSource>(this IConfigurationBuilder builder, System.Action<TSource> configureSource)
            where TSource : IConfigurationSource, new() { throw null; }

        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration, bool makePathsRelative) { throw null; }

        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration) { throw null; }

        public static bool Exists(this IConfigurationSection section) { throw null; }

        public static string GetConnectionString(this IConfiguration configuration, string name) { throw null; }
    }

    public static partial class ConfigurationPath
    {
        public static readonly string KeyDelimiter;
        public static string Combine(System.Collections.Generic.IEnumerable<string> pathSegments) { throw null; }

        public static string Combine(params string[] pathSegments) { throw null; }

        public static string GetParentPath(string path) { throw null; }

        public static string GetSectionKey(string path) { throw null; }
    }

    public partial interface IConfiguration
    {
        string this[string key] { get; set; }

        System.Collections.Generic.IEnumerable<IConfigurationSection> GetChildren();
        Primitives.IChangeToken GetReloadToken();
        IConfigurationSection GetSection(string key);
    }

    public partial interface IConfigurationBuilder
    {
        System.Collections.Generic.IDictionary<string, object> Properties { get; }

        System.Collections.Generic.IList<IConfigurationSource> Sources { get; }

        IConfigurationBuilder Add(IConfigurationSource source);
        IConfigurationRoot Build();
    }

    public partial interface IConfigurationProvider
    {
        System.Collections.Generic.IEnumerable<string> GetChildKeys(System.Collections.Generic.IEnumerable<string> earlierKeys, string parentPath);
        Primitives.IChangeToken GetReloadToken();
        void Load();
        void Set(string key, string value);
        bool TryGet(string key, out string value);
    }

    public partial interface IConfigurationRoot : IConfiguration
    {
        System.Collections.Generic.IEnumerable<IConfigurationProvider> Providers { get; }

        void Reload();
    }

    public partial interface IConfigurationSection : IConfiguration
    {
        string Key { get; }

        string Path { get; }

        string Value { get; set; }
    }

    public partial interface IConfigurationSource
    {
        IConfigurationProvider Build(IConfigurationBuilder builder);
    }
}