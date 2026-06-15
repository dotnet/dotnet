// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;
using VSLangProj150;

namespace NuGet.PackageManagement.VisualStudio.Projects;

public interface ILegacyPackageReferenceProjectServices : INuGetProjectServices
{
    VSProject4 Project4 { get; }
}
