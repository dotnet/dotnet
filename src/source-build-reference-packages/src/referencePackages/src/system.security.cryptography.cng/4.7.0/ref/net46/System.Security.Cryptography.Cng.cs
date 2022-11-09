// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.Cryptography.Cng")]
[assembly: AssemblyDescription("System.Security.Cryptography.Cng")]
[assembly: AssemblyDefaultAlias("System.Security.Cryptography.Cng")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]

[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptHandle))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptProviderHandle))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptSecretHandle))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngAlgorithm))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngAlgorithmGroup))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngExportPolicies))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKey))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyBlobFormat))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyCreationOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyCreationParameters))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyHandleOpenOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyOpenOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyUsages))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngProperty))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngPropertyCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngPropertyOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngProvider))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngUIPolicy))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngUIProtectionLevels))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSACng))]



