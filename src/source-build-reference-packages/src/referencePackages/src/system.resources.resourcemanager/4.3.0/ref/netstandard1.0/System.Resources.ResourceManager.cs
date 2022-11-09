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
[assembly: AssemblyTitle("System.Resources.ResourceManager")]
[assembly: AssemblyDescription("System.Resources.ResourceManager")]
[assembly: AssemblyDefaultAlias("System.Resources.ResourceManager")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Resources
{
    public partial class MissingManifestResourceException : System.Exception
    {
        public MissingManifestResourceException() { }
        public MissingManifestResourceException(string message) { }
        public MissingManifestResourceException(string message, System.Exception inner) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed partial class NeutralResourcesLanguageAttribute : System.Attribute
    {
        public NeutralResourcesLanguageAttribute(string cultureName) { }
        public string CultureName { get { throw null; } }
    }
    public partial class ResourceManager
    {
        public ResourceManager(string baseName, System.Reflection.Assembly assembly) { }
        public ResourceManager(System.Type resourceSource) { }
        public string GetString(string name) { throw null; }
        public virtual string GetString(string name, System.Globalization.CultureInfo culture) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed partial class SatelliteContractVersionAttribute : System.Attribute
    {
        public SatelliteContractVersionAttribute(string version) { }
        public string Version { get { throw null; } }
    }
}
