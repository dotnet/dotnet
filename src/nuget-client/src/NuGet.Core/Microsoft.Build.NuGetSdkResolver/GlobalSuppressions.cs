// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1031:Modify 'GetMSBuildSdkVersions' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:Microsoft.Build.NuGetSdkResolver.GlobalJsonReader.GetMSBuildSdkVersions(Microsoft.Build.Framework.SdkResolverContext,System.String)~System.Collections.Generic.Dictionary{System.String,System.String}")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'GetSdkResult' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:Microsoft.Build.NuGetSdkResolver.NuGetSdkResolver.NuGetAbstraction.GetSdkResult(Microsoft.Build.Framework.SdkReference,System.Object,Microsoft.Build.Framework.SdkResolverContext,Microsoft.Build.Framework.SdkResultFactory)~Microsoft.Build.Framework.SdkResult")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'SdkResult NuGetSdkResolver.Resolve(SdkReference sdkReference, SdkResolverContext resolverContext, SdkResultFactory factory)', validate parameter 'factory' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:Microsoft.Build.NuGetSdkResolver.NuGetSdkResolver.Resolve(Microsoft.Build.Framework.SdkReference,Microsoft.Build.Framework.SdkResolverContext,Microsoft.Build.Framework.SdkResultFactory)~Microsoft.Build.Framework.SdkResult")]
