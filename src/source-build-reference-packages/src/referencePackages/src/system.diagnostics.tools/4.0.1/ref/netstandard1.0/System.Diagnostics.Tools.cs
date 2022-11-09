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
[assembly: AssemblyTitle("System.Diagnostics.Tools")]
[assembly: AssemblyDescription("System.Diagnostics.Tools")]
[assembly: AssemblyDefaultAlias("System.Diagnostics.Tools")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.CodeDom.Compiler
{
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited=false, AllowMultiple=false)]
    public sealed partial class GeneratedCodeAttribute : System.Attribute
    {
        public GeneratedCodeAttribute(string tool, string version) { }
        public string Tool { get { throw null; } }
        public string Version { get { throw null; } }
    }
}
namespace System.Diagnostics.CodeAnalysis
{
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited=false, AllowMultiple=true)]
    public sealed partial class SuppressMessageAttribute : System.Attribute
    {
        public SuppressMessageAttribute(string category, string checkId) { }
        public string Category { get { throw null; } }
        public string CheckId { get { throw null; } }
        public string Justification { get { throw null; } set { } }
        public string MessageId { get { throw null; } set { } }
        public string Scope { get { throw null; } set { } }
        public string Target { get { throw null; } set { } }
    }
}
