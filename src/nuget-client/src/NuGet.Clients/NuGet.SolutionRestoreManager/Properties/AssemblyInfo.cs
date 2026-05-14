// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("06662133-1292-4918-90f3-36c930c0b16f")]

[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Lucene.Net.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Newtonsoft.Json.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Commands.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Common.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Configuration.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Credentials.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.DependencyResolver.Core.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Frameworks.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Indexing.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.LibraryModel.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.PackageManagement.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.PackageManagement.VisualStudio.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Packaging.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.ProjectModel.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Protocol.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Resolver.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.SolutionRestoreManager.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.Versioning.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.VisualStudio.Common.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.VisualStudio.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.VisualStudio.Internal.Contracts.dll")]

[assembly: ProvideBindingRedirection(CodeBase = @"$PackageFolder$\NuGet.VisualStudio.dll", OldVersionLowerBound = "0.0.0.0")]
