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
using System.Security.AccessControl;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.AccessControl")]
[assembly: AssemblyDescription("System.Security.AccessControl")]
[assembly: AssemblyDefaultAlias("System.Security.AccessControl")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.700.19.56404")]
[assembly: AssemblyInformationalVersion("4.700.19.56404 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.3.0")]

[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AccessControlActions))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AccessControlModification))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AccessControlSections))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AccessControlType))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AccessRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AccessRule<>))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AceEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AceFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AceQualifier))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AceType))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AuditFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AuditRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AuditRule<>))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AuthorizationRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.AuthorizationRuleCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CommonAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CommonAcl))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CommonObjectSecurity))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CommonSecurityDescriptor))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CompoundAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CompoundAceType))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ControlFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.CustomAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.DiscretionaryAcl))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.GenericAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.GenericAcl))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.GenericSecurityDescriptor))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.InheritanceFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.KnownAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.NativeObjectSecurity))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ObjectAccessRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ObjectAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ObjectAceFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ObjectAuditRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ObjectSecurity))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ObjectSecurity<>))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.PrivilegeNotHeldException))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.PropagationFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.QualifiedAce))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.RawAcl))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.RawSecurityDescriptor))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.ResourceType))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.SecurityInfos))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.SystemAcl))]



