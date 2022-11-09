// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.IO.Pipes.AccessControl")]
[assembly: AssemblyDescription("System.IO.Pipes.AccessControl")]
[assembly: AssemblyDefaultAlias("System.IO.Pipes.AccessControl")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]

[assembly: TypeForwardedTo(typeof(System.IO.Pipes.PipeAccessRights))]
[assembly: TypeForwardedTo(typeof(System.IO.Pipes.PipeAccessRule))]
[assembly: TypeForwardedTo(typeof(System.IO.Pipes.PipeAuditRule))]
[assembly: TypeForwardedTo(typeof(System.IO.Pipes.PipeSecurity))]



namespace System.IO.Pipes
{
    public static partial class PipesAclExtensions
    {
        public static System.IO.Pipes.PipeSecurity GetAccessControl(System.IO.Pipes.PipeStream stream) { throw null; }
        public static void SetAccessControl(System.IO.Pipes.PipeStream stream, System.IO.Pipes.PipeSecurity pipeSecurity) { }
    }
}
