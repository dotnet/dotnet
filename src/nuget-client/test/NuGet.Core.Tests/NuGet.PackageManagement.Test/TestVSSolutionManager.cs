// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
#nullable disable

using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Commands.Test;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
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

        public NuGetProject AddBuildIntegratedProject(string projectName = null, NuGetFramework projectTargetFramework = null, JObject json = null, bool createEmpty = false)
        {
            var existingProject = Task.Run(async () => await GetNuGetProjectAsync(projectName));
            existingProject.Wait();
            if (existingProject.IsCompleted && existingProject.Result != null)
            {
                throw new ArgumentException("Project with " + projectName + " already exists");
            }

            projectName = string.IsNullOrEmpty(projectName) ? Guid.NewGuid().ToString() : projectName;


            var projectTfm = projectTargetFramework?.GetShortFolderName() ?? "net46";
            var settings = Settings.LoadSpecificSettings(SolutionDirectory, "NuGet.Config");
            PackageSpec packageSpec = GetPackageSpec(projectName, json, projectTfm, settings, createEmpty);

            var directory = Path.GetDirectoryName(packageSpec.FilePath);
            Directory.CreateDirectory(directory);
            var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(
                packageSpec.TargetFrameworks[0].FrameworkName,
                new TestNuGetProjectContext(),
                directory,
                projectName);
            var nuGetProject = new TestPackageReferenceNuGetProject(packageSpec, msBuildNuGetProjectSystem);

            NuGetProjects.Add(nuGetProject);

            return nuGetProject;
        }

        private PackageSpec GetPackageSpec(string projectName, JObject json, string projectTfm, ISettings settings, bool createEmpty)
        {
            if (json == null)
            {
                var packageSpec = ProjectTestHelpers.GetPackageSpec(settings, projectName, SolutionDirectory, projectTfm);
                if (!createEmpty)
                {
                    PackageSpecOperations.AddOrUpdateDependency(packageSpec, new PackageDependency("entityframework", VersionRange.Parse("7.0.0-beta-*")));
                }
                return packageSpec;
            }
            else
            {
                var packageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(projectName, SolutionDirectory, json.ToString());
                packageSpec.WithSettingsBasedRestoreMetadata(settings);
                return packageSpec;
            }
        }
    }
}
