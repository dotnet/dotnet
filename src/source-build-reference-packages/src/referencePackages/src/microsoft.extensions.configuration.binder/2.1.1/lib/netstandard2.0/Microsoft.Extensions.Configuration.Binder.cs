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
[assembly: AssemblyTitle("Microsoft.Extensions.Configuration.Binder")]
[assembly: AssemblyDescription("Microsoft.Extensions.Configuration.Binder")]
[assembly: AssemblyDefaultAlias("Microsoft.Extensions.Configuration.Binder")]
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
    public partial class BinderOptions
    {
        public BinderOptions() { }
        public bool BindNonPublicProperties { get { throw null; } set { } }
    }
    public static partial class ConfigurationBinder
    {
        public static void Bind(this Microsoft.Extensions.Configuration.IConfiguration configuration, object instance) { }
        public static void Bind(this Microsoft.Extensions.Configuration.IConfiguration configuration, object instance, System.Action<Microsoft.Extensions.Configuration.BinderOptions> configureOptions) { }
        public static void Bind(this Microsoft.Extensions.Configuration.IConfiguration configuration, string key, object instance) { }
        public static object Get(this Microsoft.Extensions.Configuration.IConfiguration configuration, System.Type type) { throw null; }
        public static object Get(this Microsoft.Extensions.Configuration.IConfiguration configuration, System.Type type, System.Action<Microsoft.Extensions.Configuration.BinderOptions> configureOptions) { throw null; }
        public static object GetValue(this Microsoft.Extensions.Configuration.IConfiguration configuration, System.Type type, string key) { throw null; }
        public static object GetValue(this Microsoft.Extensions.Configuration.IConfiguration configuration, System.Type type, string key, object defaultValue) { throw null; }
        public static T GetValue<T>(this Microsoft.Extensions.Configuration.IConfiguration configuration, string key) { throw null; }
        public static T GetValue<T>(this Microsoft.Extensions.Configuration.IConfiguration configuration, string key, T defaultValue) { throw null; }
        public static T Get<T>(this Microsoft.Extensions.Configuration.IConfiguration configuration) { throw null; }
        public static T Get<T>(this Microsoft.Extensions.Configuration.IConfiguration configuration, System.Action<Microsoft.Extensions.Configuration.BinderOptions> configureOptions) { throw null; }
    }
}
