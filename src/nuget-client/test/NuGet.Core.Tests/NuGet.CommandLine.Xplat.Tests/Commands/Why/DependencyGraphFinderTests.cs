// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Why;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Why
{
    public class DependencyGraphFinderTests
    {
        [Fact]
        public void MultipleDependencyPathsForTargetPackage_AllPathsFound()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "System.Text.Json";
            var frameworks = new List<string>();

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // direct dependency on target package is found
            Assert.Contains(dependencyGraphs["net472"], dep => (dep.Id == "System.Text.Json") && (dep.Version == "8.0.0"));

            // transitive dependency from a top-level package is found, with the correct resolved version
            Assert.Contains(dependencyGraphs["net472"].First(dep => dep.Id == "Azure.Core").Children, dep => (dep.Id == "System.Text.Json") && (dep.Version == "8.0.0"));

            // transitive dependency from a top-level project reference is found, with the correct resolved version
            Assert.Contains(dependencyGraphs["net472"].First(dep => dep.Id == "DotnetNuGetWhyPackage").Children, dep => (dep.Id == "System.Text.Json") && (dep.Version == "8.0.0"));
        }

        [Fact]
        public void DependencyOnTargetProject_AllPathsFound()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "DotnetNuGetWhyPackage"; // project reference
            var frameworks = new List<string>();

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // direct dependency on target project reference is found
            Assert.Contains(dependencyGraphs["net472"], dep => dep.Id == "DotnetNuGetWhyPackage");

            // transitive dependency on target project reference is found
            Assert.Contains(dependencyGraphs["net472"].First(dep => dep.Id == "CustomProjectName").Children, dep => dep.Id == "DotnetNuGetWhyPackage");
        }

        [Fact]
        public void NoDependencyOnTargetPackage_ReturnsNullGraph()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "NotARealPackage";
            var frameworks = new List<string>();

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // no paths found for any framework
            Assert.Null(dependencyGraphs);
        }

        [Fact]
        public void DependencyOnTargetPackageForOnlyOneFramework_ReturnsCorrectGraphs()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "Azure.Core";
            var frameworks = new List<string>();

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // paths found for one framework
            Assert.Contains(dependencyGraphs["net472"], dep => dep.Id == "Azure.Core");

            // no paths found for the other framework
            Assert.Null(dependencyGraphs["net6.0"]);
        }

        [Fact]
        public void DependencyOnTargetPackage_FrameworkOptionSpecified_PathIsFound()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "Azure.Core";
            List<string> frameworks = ["net472"];

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // Path found
            Assert.Contains(dependencyGraphs["net472"], dep => dep.Id == "Azure.Core");
        }

        [Fact]
        public void DependencyOnTargetPackageForOnlyOneFramework_DifferentFrameworkSpecified_ReturnsNullGraph()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "Azure.Core";
            List<string> frameworks = ["net6.0"];

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // no paths found
            Assert.Null(dependencyGraphs);
        }

        [Fact]
        public void DifferentCaseUsedForTargetPackageId_MatchesAreCaseInsensitive_AllPathsFound()
        {
            // Arrange
            var lockFileFormat = new LockFileFormat();
            var lockFileContent = ProtocolUtility.GetResource("NuGet.CommandLine.Xplat.Tests.compiler.resources.DNW.Test.SampleProject1.project.assets.json", GetType());
            var assetsFile = lockFileFormat.Parse(lockFileContent, "In Memory");

            if (XunitAttributeUtility.CurrentPlatform == Platform.Linux || XunitAttributeUtility.CurrentPlatform == Platform.Darwin)
            {
                ConvertRelevantWindowsPathsToUnix(assetsFile);
            }

            string targetPackage = "system.text.jsoN";
            var frameworks = new List<string>();

            // Act
            var dependencyGraphs = DependencyGraphFinder.GetAllDependencyGraphsForTarget(assetsFile, targetPackage, frameworks);

            // Assert

            // direct dependency on target package is found
            Assert.Contains(dependencyGraphs["net472"], dep => (dep.Id == "System.Text.Json") && (dep.Version == "8.0.0"));

            // transitive dependency from a top-level package is found, with the correct resolved version
            Assert.Contains(dependencyGraphs["net472"].First(dep => dep.Id == "Azure.Core").Children, dep => (dep.Id == "System.Text.Json") && (dep.Version == "8.0.0"));

            // transitive dependency from a top-level project reference is found, with the correct resolved version
            Assert.Contains(dependencyGraphs["net472"].First(dep => dep.Id == "DotnetNuGetWhyPackage").Children, dep => (dep.Id == "System.Text.Json") && (dep.Version == "8.0.0"));
        }

        private static void ConvertRelevantWindowsPathsToUnix(LockFile assetsFile)
        {
            assetsFile.PackageSpec.FilePath = ConvertWindowsPathToUnix(assetsFile.PackageSpec.FilePath);

            var projectLibraries = assetsFile.Libraries.Where(l => l.Type == "project");

            foreach (var library in projectLibraries)
            {
                library.Path = ConvertWindowsPathToUnix(library.Path);
            }

            var packageSpecTargets = assetsFile.PackageSpec.RestoreMetadata.TargetFrameworks;

            foreach (var target in packageSpecTargets)
            {
                var projectReferences = target.ProjectReferences;
                foreach (var projectReference in projectReferences)
                {
                    projectReference.ProjectPath = ConvertWindowsPathToUnix(projectReference.ProjectPath);
                }
            }
        }

        private static string ConvertWindowsPathToUnix(string path)
        {
            char[] trimChars = ['C', ':'];
            return path.TrimStart(trimChars).Replace("\\", "/");
        }
    }
}
