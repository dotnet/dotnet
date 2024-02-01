// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unit-test names don't have to follow naming style")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.PackageManagement.UI.Test.Models.LocalPackageDetailControlModelTests.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider,NuGet.Test.Utility.LocalPackageSearchMetadataFixture)")]
