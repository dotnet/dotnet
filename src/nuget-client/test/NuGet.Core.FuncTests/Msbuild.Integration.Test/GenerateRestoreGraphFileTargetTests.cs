// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Test.Utility;
using Xunit;

namespace Msbuild.Integration.Test
{
    public class GenerateRestoreGraphFileTargetTests : IClassFixture<MsbuildIntegrationTestFixture>
    {
        private MsbuildIntegrationTestFixture _msbuildFixture;

        public GenerateRestoreGraphFileTargetTests(MsbuildIntegrationTestFixture fixture)
        {
            _msbuildFixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public async Task GenerateRestoreGraphFile_WithMixedProjectSolution_BothStaticGraphAndStandard_AreEqual()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var net461 = NuGetFramework.Parse("net461");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);

                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                var standardDgSpecFile = Path.Combine(pathContext.WorkingDirectory, "standard.dgspec.json");
                var staticGraphDgSpecFile = Path.Combine(pathContext.WorkingDirectory, "staticGraph.dgspec.json");
                var targetPath = projectA.ProjectPath;
                _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:GenerateRestoreGraphFile /p:RestoreGraphOutputPath=\"{standardDgSpecFile}\" {targetPath}");
                _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:GenerateRestoreGraphFile /p:RestoreGraphOutputPath=\"{staticGraphDgSpecFile}\" /p:RestoreUseStaticGraphEvaluation=true {targetPath}");

                var regularDgSpec = File.ReadAllText(standardDgSpecFile);
                var staticGraphDgSpec = File.ReadAllText(staticGraphDgSpecFile);

                regularDgSpec.Should().BeEquivalentTo(staticGraphDgSpec);
            }
        }
    }
}
