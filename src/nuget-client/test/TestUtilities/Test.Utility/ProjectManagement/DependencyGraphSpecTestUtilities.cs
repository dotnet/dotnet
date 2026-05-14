// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.ProjectModel;

namespace Test.Utility.ProjectManagement
{
    public static class DependencyGraphSpecTestUtilities
    {
        public static DependencyGraphSpec CreateMinimalDependencyGraphSpec(string projectPath, string outputPath)
        {
            var packageSpec = new PackageSpec();
            packageSpec.FilePath = projectPath;
            packageSpec.Name = Path.GetFileNameWithoutExtension(projectPath);
            packageSpec.RestoreMetadata = new ProjectRestoreMetadata();
            packageSpec.RestoreMetadata.ProjectUniqueName = projectPath;
            packageSpec.RestoreMetadata.ProjectStyle = ProjectStyle.PackageReference;
            packageSpec.RestoreMetadata.ProjectPath = projectPath;
            packageSpec.RestoreMetadata.OutputPath = outputPath;
            packageSpec.RestoreMetadata.CacheFilePath = Path.Combine(outputPath, "project.nuget.cache");

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(packageSpec);

            return dgSpec;
        }
    }
}
