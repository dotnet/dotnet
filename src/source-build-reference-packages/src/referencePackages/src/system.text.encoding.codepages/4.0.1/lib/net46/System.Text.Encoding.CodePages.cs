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
[assembly: AssemblyTitle("System.Text.Encoding.CodePages")]
[assembly: AssemblyDescription("System.Text.Encoding.CodePages")]
[assembly: AssemblyDefaultAlias("System.Text.Encoding.CodePages")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]




namespace System.Text
{
    public sealed partial class CodePagesEncodingProvider : System.Text.EncodingProvider
    {
        internal CodePagesEncodingProvider() { }
        public static System.Text.EncodingProvider Instance { get { throw null; } }
        public override System.Text.Encoding GetEncoding(int codepage) { throw null; }
        public override System.Text.Encoding GetEncoding(string name) { throw null; }
    }
}
