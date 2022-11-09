// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class StandaloneProjectTests
    {
        [Fact]
        public async Task StandaloneProject_BasicRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var dgFile = new DependencyGraphSpec();

                var projectJson = @"
                {
                  ""frameworks"": {
                    ""netstandard1.6"": {
                      ""dependencies"": {
                        ""a"": ""1.0.0""
                      }
                    }
                  }
                }";

                var spec = JsonPackageSpecReader.GetPackageSpec(projectJson, "x", Path.Combine(pathContext.SolutionRoot, "project.json"));
                spec.RestoreMetadata = new ProjectRestoreMetadata();
                spec.RestoreMetadata.ProjectStyle = ProjectStyle.Standalone;
                spec.RestoreMetadata.OutputPath = Path.Combine(pathContext.SolutionRoot, "x");
                spec.RestoreMetadata.ProjectUniqueName = "x";
                spec.RestoreMetadata.ProjectName = "x";
                spec.RestoreMetadata.ProjectPath = Path.Combine(pathContext.SolutionRoot, "x.csproj");

                dgFile.AddProject(spec);
                dgFile.AddRestore("x");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                // Act
                var result = await CommandsTestUtility.RunSingleRestore(dgFile, pathContext, logger);
                var path = Path.Combine(spec.RestoreMetadata.OutputPath, "project.assets.json");

                // Assert
                Assert.True(result.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.True(File.Exists(path));

                var lockFormat = new LockFileFormat();
                var lockFile = lockFormat.Read(path);

                Assert.Equal(NuGetFramework.Parse("netstandard1.6"), lockFile.Targets.Single().TargetFramework);
                Assert.Equal("a", lockFile.Targets.Single().Libraries.Single().Name);
            }
        }
    }
}
