// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.AccessControl;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("Microsoft.Win32.Registry")]
[assembly: AssemblyDescription("Microsoft.Win32.Registry")]
[assembly: AssemblyDefaultAlias("Microsoft.Win32.Registry")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.700.19.56404")]
[assembly: AssemblyInformationalVersion("4.700.19.56404 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.3.0")]

[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.RegistryAccessRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.RegistryAuditRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.RegistryRights))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.RegistrySecurity))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.Registry))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryHive))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryKey))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryKeyPermissionCheck))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryOptions))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryValueKind))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryValueOptions))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.RegistryView))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeRegistryHandle))]



