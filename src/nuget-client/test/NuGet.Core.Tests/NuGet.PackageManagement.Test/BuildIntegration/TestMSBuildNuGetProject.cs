// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;

namespace NuGet.PackageManagement.Test
{
    public class TestMSBuildNuGetProject : MSBuildNuGetProject
    {
        public IReadOnlyList<ExternalProjectReference> ProjectClosure { get; set; }

        public TestMSBuildNuGetProject(
            IMSBuildProjectSystem msbuildProjectSystem,
            string folderNuGetProjectPath,
            string packagesConfigFolderPath) : base(
                msbuildProjectSystem,
                folderNuGetProjectPath,
                packagesConfigFolderPath)
        {
            ProjectClosure = new List<ExternalProjectReference>();
        }
    }
}
