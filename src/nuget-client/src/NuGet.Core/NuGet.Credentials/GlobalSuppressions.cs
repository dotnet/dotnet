// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1056:Change the type of property PluginCredentialRequest.Uri from string to System.Uri.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Credentials.PluginCredentialRequest.Uri")]
[assembly: SuppressMessage("Build", "CA2227:Change 'AuthTypes' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Credentials.PluginCredentialResponse.AuthTypes")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Command line argument for NuGet plugin with a finite set of verbosity values. No need to be uppercase", Scope = "member", Target = "~M:NuGet.Credentials.PluginCredentialProvider.GetPluginResponse(NuGet.Credentials.PluginCredentialRequest,System.Threading.CancellationToken)~NuGet.Credentials.PluginCredentialResponse")]
