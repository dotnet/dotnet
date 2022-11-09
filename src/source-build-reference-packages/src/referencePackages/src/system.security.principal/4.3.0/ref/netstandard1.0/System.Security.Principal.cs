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
[assembly: AssemblyTitle("System.Security.Principal")]
[assembly: AssemblyDescription("System.Security.Principal")]
[assembly: AssemblyDefaultAlias("System.Security.Principal")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Security.Principal
{
    public partial interface IIdentity
    {
        string AuthenticationType { get; }
        bool IsAuthenticated { get; }
        string Name { get; }
    }
    public partial interface IPrincipal
    {
        System.Security.Principal.IIdentity Identity { get; }
        bool IsInRole(string role);
    }
    public enum TokenImpersonationLevel
    {
        Anonymous = 1,
        Delegation = 4,
        Identification = 2,
        Impersonation = 3,
        None = 0,
    }
}
