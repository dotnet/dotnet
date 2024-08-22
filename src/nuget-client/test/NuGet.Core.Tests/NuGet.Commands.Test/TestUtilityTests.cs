// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class TestUtilityTests
    {
        [Fact]
        public async Task TestUtility_Solution()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange && Act
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'net45': { }
                                                  }
                                               }");

                var projectB = SimpleTestProjectContext.CreateUAP(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    projectJson);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Assert
                Assert.True(File.Exists(Path.Combine(pathContext.SolutionRoot, "solution.sln")));
                Assert.True(File.Exists(Path.Combine(pathContext.SolutionRoot, "a", "a.csproj")));
                Assert.True(File.Exists(Path.Combine(pathContext.WorkingDirectory, "NuGet.Config")));
                Assert.True(File.Exists(Path.Combine(pathContext.SolutionRoot, "b", "b.csproj")));
                Assert.True(File.Exists(Path.Combine(pathContext.SolutionRoot, "b", "project.json")));
            }
        }
    }
}
