// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_DESKTOP
using System;
using System.IO;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
#endif
using NuGet.Test.Utility;
#if IS_DESKTOP
using NuGet.VisualStudio;
#endif
using Test.Utility;

namespace NuGet.PackageManagement.Test
{
    internal class TestVSSolutionManager : TestSolutionManager
    {
        public TestVSSolutionManager()
            : base()
        {
        }
        public TestVSSolutionManager(SimpleTestPathContext pathContext)
            : base(pathContext)
        {
        }

#if IS_DESKTOP
        public NuGetProject AddCPSPackageReferenceBasedProject(IProjectSystemCache projectSystemCache, PackageSpec packageSpec)
        {
            if (packageSpec == null) throw new ArgumentNullException(nameof(packageSpec));

            var cpsPackageReferenceProject = TestCpsPackageReferenceProject.CreateTestCpsPackageReferenceProject(
            packageSpec.Name, packageSpec.FilePath, projectSystemCache, assetsFilePath: packageSpec.RestoreMetadata.OutputPath, packageSpec: packageSpec);
            Directory.CreateDirectory(packageSpec.FilePath);
            NuGetProjects.Add(cpsPackageReferenceProject);
            return cpsPackageReferenceProject;
        }
#endif
    }
}
