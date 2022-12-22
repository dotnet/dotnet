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
[assembly: AssemblyTitle("Microsoft.Extensions.Configuration.Abstractions")]
[assembly: AssemblyDescription("Microsoft.Extensions.Configuration.Abstractions")]
[assembly: AssemblyDefaultAlias("Microsoft.Extensions.Configuration.Abstractions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("2.1.1.18157")]
[assembly: AssemblyInformationalVersion("2.1.1.18157 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("2.1.1.0")]




namespace Microsoft.Extensions.Configuration
{
    public static partial class ConfigurationExtensions
    {
        public static Microsoft.Extensions.Configuration.IConfigurationBuilder Add<TSource>(this Microsoft.Extensions.Configuration.IConfigurationBuilder builder, System.Action<TSource> configureSource) where TSource : Microsoft.Extensions.Configuration.IConfigurationSource, new() { throw null; }
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> AsEnumerable(this Microsoft.Extensions.Configuration.IConfiguration configuration) { throw null; }
        public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> AsEnumerable(this Microsoft.Extensions.Configuration.IConfiguration configuration, bool makePathsRelative) { throw null; }
        public static bool Exists(this Microsoft.Extensions.Configuration.IConfigurationSection section) { throw null; }
        public static string GetConnectionString(this Microsoft.Extensions.Configuration.IConfiguration configuration, string name) { throw null; }
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
        System.Collections.Generic.IEnumerable<Microsoft.Extensions.Configuration.IConfigurationSection> GetChildren();
        Microsoft.Extensions.Primitives.IChangeToken GetReloadToken();
        Microsoft.Extensions.Configuration.IConfigurationSection GetSection(string key);
    }
    public partial interface IConfigurationBuilder
    {
        System.Collections.Generic.IDictionary<string, object> Properties { get; }
        System.Collections.Generic.IList<Microsoft.Extensions.Configuration.IConfigurationSource> Sources { get; }
        Microsoft.Extensions.Configuration.IConfigurationBuilder Add(Microsoft.Extensions.Configuration.IConfigurationSource source);
        Microsoft.Extensions.Configuration.IConfigurationRoot Build();
    }
    public partial interface IConfigurationProvider
    {
        System.Collections.Generic.IEnumerable<string> GetChildKeys(System.Collections.Generic.IEnumerable<string> earlierKeys, string parentPath);
        Microsoft.Extensions.Primitives.IChangeToken GetReloadToken();
        void Load();
        void Set(string key, string value);
        bool TryGet(string key, out string value);
    }
    public partial interface IConfigurationRoot : Microsoft.Extensions.Configuration.IConfiguration
    {
        System.Collections.Generic.IEnumerable<Microsoft.Extensions.Configuration.IConfigurationProvider> Providers { get; }
        void Reload();
    }
    public partial interface IConfigurationSection : Microsoft.Extensions.Configuration.IConfiguration
    {
        string Key { get; }
        string Path { get; }
        string Value { get; set; }
    }
    public partial interface IConfigurationSource
    {
        Microsoft.Extensions.Configuration.IConfigurationProvider Build(Microsoft.Extensions.Configuration.IConfigurationBuilder builder);
    }
}
