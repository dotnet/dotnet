// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class MinClientVersionTests
    {
        [Fact]
        public async Task MinClientVersion_DependencyVersionTooHighAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageBContext = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "1.0.0",
                    MinClientVersion = "9.0.0"
                };

                var packageAContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0",
                    MinClientVersion = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { packageBContext }
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageAContext, packageBContext);

                Exception ex = null;

                // Act
                var command = new RestoreCommand(request);

                try
                {
                    var result = await command.ExecuteAsync();
                }
                catch (Exception exception)
                {
                    ex = exception;
                }

                // Assert
                Assert.Contains("The 'packageB 1.0.0' package requires NuGet client version '9.0.0' or above, but the current NuGet version is", ex.Message);
            }
        }
    }
}
