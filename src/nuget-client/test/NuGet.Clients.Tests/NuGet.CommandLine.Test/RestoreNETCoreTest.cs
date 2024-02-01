// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.Test
{
    public class RestoreNetCoreTest
    {
        private readonly ITestOutputHelper _output;

        public RestoreNetCoreTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task RestoreNetCore_AddExternalTargetVerifyTargetUsedAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var pkgX = new SimpleTestPackageContext("x", "1.0.0");
                var pkgY = new SimpleTestPackageContext("y", "1.0.0");

                // Add y to the project
                projectA.AddPackageToAllFrameworks(pkgY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, pkgX, pkgY);

                // Inject dependency x
                var doc = XDocument.Load(projectA.ProjectPath);
                var ns = doc.Root.GetDefaultNamespace().NamespaceName;
                doc.Root.AddFirst(
                    new XElement(XName.Get("Target", ns),
                    new XAttribute(XName.Get("Name"), "RunMe"),
                    new XAttribute(XName.Get("BeforeTargets"), "CollectPackageReferences"),
                        new XElement(XName.Get("ItemGroup", ns),
                            new XElement(XName.Get("PackageReference", ns),
                                new XAttribute(XName.Get("Include"), "x"),
                                new XAttribute(XName.Get("Version"), "1.0.0")))));

                doc.Save(projectA.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                projectA.AssetsFile.GetLibrary("x", NuGetVersion.Parse("1.0.0")).Should().NotBeNull();
                projectA.AssetsFile.GetLibrary("y", NuGetVersion.Parse("1.0.0")).Should().NotBeNull();
            }
        }

        [PlatformFact(Platform.Windows)]
        public void RestoreNetCore_IfProjectsWitAndWithoutRestoreTargetsExistVerifyValidProjectsRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Remove all contents from B to make it invalid for restore.
                File.Delete(projectB.ProjectPath);
                File.WriteAllText(projectB.ProjectPath, "<Project ToolsVersion=\"15.0\"></Project>");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                File.Exists(projectA.AssetsFileOutputPath).Should().BeTrue();
                File.Exists(projectB.AssetsFileOutputPath).Should().BeFalse();
                r.AllOutput.Should().Contain("NU1503");
                r.AllOutput.Should().Contain("The project file may be invalid or missing targets required for restore.");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void RestoreNetCore_IfAllProjectsAreWithoutRestoreTargetsVerifySuccess()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Remove all contents from A to make it invalid for restore.
                File.Delete(projectA.ProjectPath);
                File.WriteAllText(projectA.ProjectPath, "<Project ToolsVersion=\"15.0\"></Project>");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                File.Exists(projectA.AssetsFileOutputPath).Should().BeFalse();
                r.AllOutput.Should().Contain("NU1503");
                r.AllOutput.Should().Contain("The project file may be invalid or missing targets required for restore.");
            }
        }

        /// <summary>
        /// Create 3 projects, each with their own nuget.config file and source.
        /// When restoring with a solution, the settings from the project folder should not be used.
        /// </summary>
        [Fact]
        public async Task RestoreNetCore_WithNuGetExe_WhenRestoringASolution_VerifyPerProjectConfigSourcesAreNotUsed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projects = new Dictionary<string, SimpleTestProjectContext>();
                const string packageId = "packageA";
                const string packageVersion = "1.0.0";

                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext()
                    {
                        Id = packageId,
                        Version = packageVersion
                    }
                    ); ;

                foreach (var number in new[] { "2", "3" })
                {
                    // Project
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"project{number}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    projects.Add(number, project);

                    // Package

                    var referencePackage = new SimpleTestPackageContext()
                    {
                        Id = packageId,
                        Version = "*",
                        PrivateAssets = "all",
                    };

                    project.AddPackageToAllFrameworks(referencePackage);
                    project.Properties.Clear();

                    solution.Projects.Add(project);

                    // Source
                    var source = Path.Combine(pathContext.WorkingDirectory, $"source{number}");

                    await SimpleTestPackageUtility.CreatePackagesAsync(
                        source,
                        new SimpleTestPackageContext()
                        {
                            Id = packageId,
                            Version = $"{number}.0.0"
                        });

                    // Create a nuget.config for the project specific source.
                    var projectDir = Path.GetDirectoryName(project.ProjectPath);
                    Directory.CreateDirectory(projectDir);
                    var configPath = Path.Combine(projectDir, "NuGet.Config");

                    var doc = new XDocument();
                    var configuration = new XElement(XName.Get("configuration"));
                    doc.Add(configuration);

                    var config = new XElement(XName.Get("config"));
                    configuration.Add(config);

                    var packageSources = new XElement(XName.Get("packageSources"));
                    configuration.Add(packageSources);

                    var sourceEntry = new XElement(XName.Get("add"));
                    sourceEntry.Add(new XAttribute(XName.Get("key"), "projectSource"));
                    sourceEntry.Add(new XAttribute(XName.Get("value"), source));
                    packageSources.Add(sourceEntry);

                    File.WriteAllText(configPath, doc.ToString());
                }

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, pathContext.SolutionRoot, expectedExitCode: 0);

                // Assert
                r.Success.Should().BeTrue();
                projects.Should().NotBeEmpty();

                foreach ((var number, var context) in projects)
                {
                    context.AssetsFile.Libraries.Select(e => e.Name).Should().Contain(packageId);
                    context.AssetsFile.Libraries.Single(e => e.Name.Equals(packageId)).Version.ToString().Should().Be(packageVersion);
                }
            }
        }

        /// <summary>
        /// Create 3 projects, each with their own nuget.config file and source.
        /// When restoring without a solution settings should be found from the project folder.
        /// Solution settings are verified in RestoreProjectJson_RestoreFromSlnUsesNuGetFolderSettings and RestoreNetCore_WithNuGetExe_WhenRestoringASolution_VerifyPerProjectConfigSourcesAreNotUsed
        /// </summary>
        [Fact]
        public async Task RestoreNetCore_WithNuGetExe_VerifyPerProjectConfigSourcesAreUsedForChildProjectsWithoutSolutionAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projects = new Dictionary<string, SimpleTestProjectContext>();
                var sources = new List<string>();

                foreach (var letter in new[] { "A", "B", "C", "D" })
                {
                    // Project
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"project{letter}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    projects.Add(letter, project);
                    solution.Projects.Add(project);

                    // Package
                    var package = new SimpleTestPackageContext()
                    {
                        Id = $"package{letter}",
                        Version = "1.0.0"
                    };

                    // Do not flow the reference up
                    package.PrivateAssets = "all";

                    project.AddPackageToAllFrameworks(package);
                    project.Properties.Clear();

                    // Source
                    var source = Path.Combine(pathContext.WorkingDirectory, $"source{letter}");
                    await SimpleTestPackageUtility.CreatePackagesAsync(source, package);
                    sources.Add(source);

                    // Create a nuget.config for the project specific source.
                    var projectDir = Path.GetDirectoryName(project.ProjectPath);
                    Directory.CreateDirectory(projectDir);
                    var configPath = Path.Combine(projectDir, "NuGet.Config");

                    var doc = new XDocument();
                    var configuration = new XElement(XName.Get("configuration"));
                    doc.Add(configuration);

                    var config = new XElement(XName.Get("config"));
                    configuration.Add(config);

                    var packageSources = new XElement(XName.Get("packageSources"));
                    configuration.Add(packageSources);

                    var sourceEntry = new XElement(XName.Get("add"));
                    sourceEntry.Add(new XAttribute(XName.Get("key"), "projectSource"));
                    sourceEntry.Add(new XAttribute(XName.Get("value"), source));
                    packageSources.Add(sourceEntry);

                    File.WriteAllText(configPath, doc.ToString());
                }

                // Create root project
                var projectRoot = SimpleTestProjectContext.CreateNETCore(
                    "projectRoot",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // Link the root project to all other projects
                foreach (var child in projects.Values)
                {
                    projectRoot.AddProjectToAllFrameworks(child);
                }

                projectRoot.Save();
                solution.Projects.Add(projectRoot);

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, projectRoot.ProjectPath, expectedExitCode: 0, additionalArgs: "-Recursive");

                // Assert
                Assert.True(projects.Count > 0);

                foreach (var letter in projects.Keys)
                {
                    Assert.Contains($"package{letter}", projects[letter].AssetsFile.Libraries.Select(e => e.Name));
                }
            }
        }

        /// <summary>
        /// Verify the project level config can override a solution level config's sources.
        /// </summary>
        [Fact]
        public async Task RestoreNetCore_VerifyProjectConfigCanOverrideSolutionConfigAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                // Project
                var project = SimpleTestProjectContext.CreateNETCore(
                    $"projectA",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                solution.Projects.Add(project);

                // Package
                var packageGood = new SimpleTestPackageContext()
                {
                    Id = $"packageA",
                    Version = "1.0.0"
                };

                var packageGoodDep = new SimpleTestPackageContext()
                {
                    Id = $"packageB",
                    Version = "1.0.0"
                };

                packageGood.Dependencies.Add(packageGoodDep);

                var packageBad = new SimpleTestPackageContext()
                {
                    Id = $"packageA",
                    Version = "1.0.0"
                };

                project.AddPackageToAllFrameworks(packageBad);
                project.Properties.Clear();

                // Source
                var source = Path.Combine(pathContext.WorkingDirectory, "sourceA");

                // The override source contains an extra dependency
                await SimpleTestPackageUtility.CreatePackagesAsync(source, packageGood, packageGoodDep);

                // The solution level source does not contain B
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageBad);

                // Create a nuget.config for the project specific source.
                var projectDir = Path.GetDirectoryName(project.ProjectPath);
                Directory.CreateDirectory(projectDir);
                var configPath = Path.Combine(projectDir, "NuGet.Config");

                var doc = new XDocument();
                var configuration = new XElement(XName.Get("configuration"));
                doc.Add(configuration);

                var config = new XElement(XName.Get("config"));
                configuration.Add(config);

                var packageSources = new XElement(XName.Get("packageSources"));
                configuration.Add(packageSources);
                packageSources.Add(new XElement(XName.Get("clear")));

                var sourceEntry = new XElement(XName.Get("add"));
                sourceEntry.Add(new XAttribute(XName.Get("key"), "projectSource"));
                sourceEntry.Add(new XAttribute(XName.Get("value"), source));
                packageSources.Add(sourceEntry);

                File.WriteAllText(configPath, doc.ToString());

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, project.ProjectPath);

                // Assert
                Assert.Contains("packageB", project.AssetsFile.Libraries.Select(e => e.Name));
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyProjectConfigChangeTriggersARestoreAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                //Act
                var r1 = Util.RestoreSolution(pathContext);

                //Assert.
                Assert.Equal(0, r1.ExitCode);
                Assert.Contains("Writing cache file", r1.Output);

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                //Assert.
                Assert.Equal(0, r2.ExitCode);
                Assert.DoesNotContain("Writing cache file", r2.Output);

                // create a config file
                var projectDir = Path.GetDirectoryName(projectA.ProjectPath);

                var configPath = Path.Combine(projectDir, "NuGet.Config");

                var doc = new XDocument();
                var configuration = new XElement(XName.Get("configuration"));
                doc.Add(configuration);

                var config = new XElement(XName.Get("config"));
                configuration.Add(config);

                var packageSources = new XElement(XName.Get("packageSources"));
                configuration.Add(packageSources);

                var sourceEntry = new XElement(XName.Get("add"));
                sourceEntry.Add(new XAttribute(XName.Get("key"), "projectSource"));
                sourceEntry.Add(new XAttribute(XName.Get("value"), "https://www.nuget.org/api/v2"));
                packageSources.Add(sourceEntry);

                var localSource = new XElement(XName.Get("add"));
                localSource.Add(new XAttribute(XName.Get("key"), "localSource"));
                localSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource));
                packageSources.Add(localSource);


                File.WriteAllText(configPath, doc.ToString());

                // Act
                var r3 = Util.RestoreSolution(pathContext, 0, "-configFile", "NuGet.Config");


                //Assert.
                Assert.Equal(0, r3.ExitCode);
                Assert.Contains("Writing cache file", r3.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyFallbackFoldersChangeTriggersARestoreAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                //Act
                var r1 = Util.RestoreSolution(pathContext);

                //Assert.
                Assert.Equal(0, r1.ExitCode);
                Assert.Contains("Writing cache file", r1.Output);

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                //Assert.
                Assert.Equal(0, r2.ExitCode);
                Assert.DoesNotContain("Writing cache file", r2.Output);

                // create a config file
                var projectDir = Path.GetDirectoryName(projectA.ProjectPath);

                var configPath = Path.Combine(projectDir, "NuGet.Config");

                var doc = new XDocument();
                var configuration = new XElement(XName.Get("configuration"));
                doc.Add(configuration);

                var config = new XElement(XName.Get("config"));
                configuration.Add(config);

                var packageSources = new XElement(XName.Get("fallbackFolders"));
                configuration.Add(packageSources);

                var sourceEntry = new XElement(XName.Get("add"));
                sourceEntry.Add(new XAttribute(XName.Get("key"), "folder"));
                sourceEntry.Add(new XAttribute(XName.Get("value"), "blaa"));
                packageSources.Add(sourceEntry);

                var sources = new XElement(XName.Get("packageSources"));
                configuration.Add(sources);
                var localSource = new XElement(XName.Get("add"));
                localSource.Add(new XAttribute(XName.Get("key"), "localSource"));
                localSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource));
                sources.Add(localSource);

                File.WriteAllText(configPath, doc.ToString());

                // Act
                var r3 = Util.RestoreSolution(pathContext, 0, "-configFile", "NuGet.Config");


                //Assert.
                Assert.Equal(0, r3.ExitCode);
                Assert.Contains("Writing cache file", r3.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyGlobalPackagesPathChangeTriggersARestoreAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                //Act
                var r1 = Util.RestoreSolution(pathContext);

                //Assert.
                Assert.Equal(0, r1.ExitCode);
                Assert.Contains("Writing cache file", r1.Output);

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                //Assert.
                Assert.Equal(0, r2.ExitCode);
                Assert.DoesNotContain("Writing cache file", r2.Output);

                // create a config file
                var projectDir = Path.GetDirectoryName(projectA.ProjectPath);

                var configPath = Path.Combine(projectDir, "NuGet.Config");

                var doc = new XDocument();
                var configuration = new XElement(XName.Get("configuration"));
                doc.Add(configuration);

                var config = new XElement(XName.Get("config"));
                configuration.Add(config);

                var sourceEntry = new XElement(XName.Get("add"));
                sourceEntry.Add(new XAttribute(XName.Get("key"), "globalPackagesPath"));
                sourceEntry.Add(new XAttribute(XName.Get("value"), "blaa"));
                configuration.Add(sourceEntry);

                var packageSources = new XElement(XName.Get("packageSources"));
                configuration.Add(packageSources);
                var localSource = new XElement(XName.Get("add"));
                localSource.Add(new XAttribute(XName.Get("key"), "localSource"));
                localSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource));
                packageSources.Add(localSource);

                File.WriteAllText(configPath, doc.ToString());

                // Act
                var r3 = Util.RestoreSolution(pathContext, 0, "-configFile", "NuGet.Config");


                //Assert.
                Assert.Equal(0, r3.ExitCode);
                Assert.Contains("Writing cache file", r3.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyPackageReference_WithoutRestoreProjectStyleAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.Properties.Clear();

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                var propsXML = XDocument.Load(projectA.PropsOutput);
                var styleNode = propsXML.Root.Elements().First().Elements(XName.Get("NuGetProjectStyle", "http://schemas.microsoft.com/developer/msbuild/2003")).FirstOrDefault();

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);
                Assert.Equal(ProjectStyle.PackageReference, assetsFile.PackageSpec.RestoreMetadata.ProjectStyle);
                Assert.Equal("PackageReference", styleNode.Value);
            }
        }

        [Fact]
        public async Task RestoreNetCore_SetProjectStyleWithProperty_PackageReferenceAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                // Add a project.json file which will be ignored
                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'net45': {
                                                            'x': '1.0.0'
                                                    }
                                                  }
                                               }");

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.Properties.Clear();
                projectA.Properties.Add("RestoreProjectStyle", "PackageReference");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                File.WriteAllText(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "project.json"), projectJson.ToString());

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                var propsXML = XDocument.Load(projectA.PropsOutput);
                var styleNode = propsXML.Root.Elements().First().Elements(XName.Get("NuGetProjectStyle", "http://schemas.microsoft.com/developer/msbuild/2003")).FirstOrDefault();

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);
                Assert.Equal(ProjectStyle.PackageReference, assetsFile.PackageSpec.RestoreMetadata.ProjectStyle);
                Assert.Equal("PackageReference", styleNode.Value);
            }
        }

        [Fact]
        public async Task RestoreNetCore_SetProjectStyleWithProperty_ProjectJsonAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                // Create a .NETCore project, but add a project.json file to it.
                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                        'x': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                        'net45': { }
                                                    }
                                                  }");

                // Force this project to ProjectJson
                projectA.Properties.Clear();
                projectA.Properties.Add("RestoreProjectStyle", "ProjectJson");
                projectA.Type = ProjectStyle.ProjectJson;

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                File.WriteAllText(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "project.json"), projectJson.ToString());

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/net45/x.targets");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var projectXML = XDocument.Load(projectA.ProjectPath);
                projectXML.Root.AddFirst(new XElement(XName.Get("Target", "http://schemas.microsoft.com/developer/msbuild/2003"), new XAttribute(XName.Get("Name"), "_SplitProjectReferencesByFileExistence")));
                projectXML.Save(projectA.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);
                Assert.Equal(ProjectStyle.ProjectJson, assetsFile.PackageSpec.RestoreMetadata.ProjectStyle);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_Recursive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed",
                    "-Recursive"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectC.AssetsFileOutputPath));
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_RecursiveIgnoresNonRestorable()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"));

                var projectB = SimpleTestProjectContext.CreateNonNuGet(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed",
                    "-Recursive"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert correct projects were restored.
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.False(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectC.AssetsFileOutputPath));

                // Assert transitivity is applied across non PackageReference projects.
                var ridlessTarget = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "project" && e.Name == projectB.ProjectName);
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "project" && e.Name == projectC.ProjectName);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithRIDAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);

                var assetsFile = projectA.AssetsFile;
                Assert.Equal(2, assetsFile.Targets.Count);
                Assert.Equal(NuGetFramework.Parse("net45"), assetsFile.Targets.Single(t => string.IsNullOrEmpty(t.RuntimeIdentifier)).TargetFramework);
                Assert.Equal(NuGetFramework.Parse("net45"), assetsFile.Targets.Single(t => !string.IsNullOrEmpty(t.RuntimeIdentifier)).TargetFramework);
                Assert.Equal("win7-x86", assetsFile.Targets.Single(t => !string.IsNullOrEmpty(t.RuntimeIdentifier)).RuntimeIdentifier);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithRID_ValidateRID_FailureAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("ref/net45/x.dll");
                packageX.AddFile("lib/win8/x.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.Properties.Add("ValidateRuntimeIdentifierCompatibility", "true");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);
                var output = r.Output + " " + r.Errors;

                // Assert
                Assert.True(r.ExitCode == 1);
                Assert.Contains("no run-time assembly compatible", output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithRID_ValidateRID_IgnoreFailureAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("ref/net45/x.dll");
                packageX.AddFile("lib/win8/x.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.ExitCode == 0);
                Assert.DoesNotContain("no run-time assembly compatible", r.Errors);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithRID_ValidateRID_FailureForProjectJsonAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                        'x': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                        'net45': {
                                                    }
                                                  },
                                                  'runtimes': { 'win7-x86': {} }
                                               }");

                var projectA = SimpleTestProjectContext.CreateUAP(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    projectJson);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("ref/net45/x.dll");
                packageX.AddFile("lib/win8/x.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);
                var output = r.Output + " " + r.Errors;

                // Assert
                Assert.True(r.ExitCode == 1);
                Assert.Contains("no run-time assembly compatible", output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithRIDSingleAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RuntimeIdentifier", "win7-x86");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);

                var assetsFile = projectA.AssetsFile;
                Assert.Equal(2, assetsFile.Targets.Count);
                Assert.Equal(NuGetFramework.Parse("net45"), assetsFile.Targets.Single(t => string.IsNullOrEmpty(t.RuntimeIdentifier)).TargetFramework);
                Assert.Equal(NuGetFramework.Parse("net45"), assetsFile.Targets.Single(t => !string.IsNullOrEmpty(t.RuntimeIdentifier)).TargetFramework);
                Assert.Equal("win7-x86", assetsFile.Targets.Single(t => !string.IsNullOrEmpty(t.RuntimeIdentifier)).RuntimeIdentifier);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithRIDDuplicatesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RuntimeIdentifier", "win7-x86");
                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86;win7-x86;;");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);

                var assetsFile = projectA.AssetsFile;
                Assert.Equal(2, assetsFile.Targets.Count);
                Assert.Equal(NuGetFramework.Parse("net45"), assetsFile.Targets.Single(t => string.IsNullOrEmpty(t.RuntimeIdentifier)).TargetFramework);
                Assert.Equal(NuGetFramework.Parse("net45"), assetsFile.Targets.Single(t => !string.IsNullOrEmpty(t.RuntimeIdentifier)).TargetFramework);
                Assert.Equal("win7-x86", assetsFile.Targets.Single(t => !string.IsNullOrEmpty(t.RuntimeIdentifier)).RuntimeIdentifier);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithSupportsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var guid = Guid.NewGuid().ToString();
                projectA.Properties.Add("RuntimeSupports", guid);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                var output = r.Output + r.Errors;

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), output);
                Assert.Contains($"Compatibility Profile: {guid}", output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreWithMultipleRIDsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86;win7-x64");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);

                var assetsFile = projectA.AssetsFile;
                Assert.Equal(3, assetsFile.Targets.Count);
                Assert.Equal("win7-x64", assetsFile.Targets.Single(t => t.RuntimeIdentifier == "win7-x64").RuntimeIdentifier);
                Assert.Equal("win7-x86", assetsFile.Targets.Single(t => t.RuntimeIdentifier == "win7-x86").RuntimeIdentifier);
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultipleProjects_SameToolDifferentVersionsWithMultipleHitsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Create this many different tool versions and projects
                var testCount = 10;

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var avoidVersion = $"{testCount + 100}.0.0";

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = avoidVersion
                };

                var projects = new List<SimpleTestProjectContext>();

                for (var i = 0; i < testCount; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"proj{i}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    project.AddPackageToAllFrameworks(packageX);

                    var packageZSub = new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = $"{i + 1}.0.0"
                    };

                    project.DotnetCLIToolReferences.Add(packageZSub);

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packageZSub);

                    solution.Projects.Add(project);
                    solution.Create(pathContext.SolutionRoot);
                }

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", avoidVersion, "netcoreapp1.0", "project.assets.json");
                var zPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                // Version should not be used
                Assert.False(File.Exists(path), r.Output);

                // Each project should have its own tool verion
                Assert.Equal(testCount, Directory.GetDirectories(zPath).Length);
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultipleProjects_SameToolDifferentVersionsWithMultipleHits_NoOpAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Create this many different tool versions and projects
                var testCount = 10;

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var avoidVersion = $"{testCount + 100}.0.0";

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = avoidVersion
                };

                var projects = new List<SimpleTestProjectContext>();

                for (var i = 0; i < testCount; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"proj{i}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    project.AddPackageToAllFrameworks(packageX);

                    var packageZSub = new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = $"{i + 1}.0.0"
                    };

                    project.DotnetCLIToolReferences.Add(packageZSub);

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packageZSub);

                    solution.Projects.Add(project);
                    solution.Create(pathContext.SolutionRoot);
                }

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", avoidVersion, "netcoreapp1.0", "project.assets.json");
                var cacheFile = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", avoidVersion, "netcoreapp1.0", "z.nuget.cache");
                var zPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                // Version should not be used
                Assert.False(File.Exists(path), r.Output);
                Assert.False(File.Exists(cacheFile), r.Output);

                // Each project should have its own tool verion
                Assert.Equal(testCount, Directory.GetDirectories(zPath).Length);

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                // Version should not be used
                Assert.False(File.Exists(path), r2.Output);
                Assert.False(File.Exists(cacheFile), r2.Output);
                Assert.DoesNotContain("NU1603", r2.Output);
                for (var i = 1; i <= testCount; i++)
                {
                    Assert.Contains($"The restore inputs for 'z-netcoreapp1.0-[{i}.0.0, )' have not changed. No further actions are required to complete the restore.", r2.Output);
                }
                // Each project should have its own tool verion
                Assert.Equal(testCount, Directory.GetDirectories(zPath).Length);
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_AddingNewPackageRestoresAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "20.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.Equal(0, r.ExitCode);
                Assert.Contains("Writing cache file", r.Output);

                //re-arrange again
                project.AddPackageToAllFrameworks(packageZ);
                project.Save();

                //assert
                Assert.Contains("Writing cache file", r.Output);
                Assert.Equal(0, r.ExitCode);


            }
        }

        [Fact]
        public async Task RestoreNetCore_OriginalTargetFrameworkArePreservedAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    "proj",
                    pathContext.SolutionRoot,
                    "net48",
                    "net46");

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                Assert.Equal(0, r.ExitCode);
                Assert.True(File.Exists(project.PropsOutput), r.Output);
                var propsXML = XDocument.Parse(File.ReadAllText(project.PropsOutput));

                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ItemGroup").ToList();

                Assert.Contains("'$(TargetFramework)' == 'net46' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Contains("'$(TargetFramework)' == 'net48' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[2].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_AddingANewProjectRestoresOnlyThatProjectAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "20.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.Equal(0, r.ExitCode);
                Assert.Contains("Writing cache file", r.Output);

                // build project
                var project2 = SimpleTestProjectContext.CreateNETCore(
                    $"proj2",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project2.AddPackageToAllFrameworks(packageZ);
                solution.Projects.Add(project2);
                solution.Save();
                project2.Save();

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                Assert.Equal(0, r2.ExitCode);
                Assert.Contains("Writing cache file", r2.Output);
                Assert.Contains("No further actions are required to complete", r2.Output);

            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_WarningsAndErrorsDontAffectHashAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                // Setup - set warnings As Errors
                project.WarningsAsErrors = true;

                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.Equal(0, r.ExitCode);
                Assert.Contains("Writing cache file", r.Output);

                //Setup - remove the warnings and errors
                project.WarningsAsErrors = false;
                project.Save();

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                Assert.Equal(0, r2.ExitCode);
                Assert.Contains("No further actions are required to complete", r2.Output);
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/10075")]
        public async Task RestoreNetCore_MultipleProjects_SameToolDifferentVersionsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "20.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                for (var i = 0; i < 10; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"proj{i}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    project.AddPackageToAllFrameworks(packageX);
                    project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = $"{i}.0.0"
                    });

                    solution.Projects.Add(project);
                    solution.Create(pathContext.SolutionRoot);
                }

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "20.0.0", "netcoreapp1.0", "project.assets.json");
                var zPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(path), r.Output);
                Assert.Equal(1, Directory.GetDirectories(zPath).Length);
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultipleProjects_SameToolAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                for (var i = 0; i < 10; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"proj{i}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    project.AddPackageToAllFrameworks(packageX);
                    project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = "1.0.0"
                    });

                    solution.Projects.Add(project);
                    solution.Create(pathContext.SolutionRoot);
                }

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "project.assets.json");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(path), r.Output);
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/9128")]
        public async Task RestoreNetCore_MultipleProjects_SameTool_DifferentVersionRanges_DoesNotNoOpAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();


                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj1",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.0"
                });

                var project2 = SimpleTestProjectContext.CreateNETCore(
                    $"proj2",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project2.AddPackageToAllFrameworks(packageX);
                project2.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.5.*"
                });

                solution.Projects.Add(project2);
                solution.Projects.Add(project);

                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var assetsPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.0.0", "netcoreapp1.0", "project.assets.json");
                var cachePath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.0.0", "netcoreapp1.0", "z.nuget.cache");

                // Act
                var r = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                // Act
                var r2 = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));
                // This is expected, because despite the fact that both projects resolve to the same tool, the version range they request is different so they will keep overwriting each other
                // Basically, it is impossible for both tools to no-op.
                Assert.Contains($"Writing tool assets file to disk", r2.Output);
                r = Util.RestoreSolution(pathContext);

            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/9128")]
        public async Task RestoreNetCore_MultipleProjects_SameTool_OverlappingVersionRanges_DoesNoOpAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();


                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj1",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.0"
                });

                var project2 = SimpleTestProjectContext.CreateNETCore(
                    $"proj2",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project2.AddPackageToAllFrameworks(packageX);
                project2.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.*"
                });

                solution.Projects.Add(project2);
                solution.Projects.Add(project);

                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var assetsPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.0.0", "netcoreapp1.0", "project.assets.json");
                var cachePath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.0.0", "netcoreapp1.0", "z.nuget.cache");

                // Act
                var r = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                // Act
                var r2 = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                // This is expected, because despite the fact that both projects resolve to the same tool, the version range they request is different so they will keep overwriting each other
                // Basically, it is impossible for both tools to no-op.
                Assert.Contains($"Writing tool assets file to disk", r2.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultipleProjects_SameTool_OverlappingVersionRanges_OnlyOneMatchesPackage_DoesNoOpAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.5.0"
                };


                var packageZ20 = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.0"
                };


                var projects = new List<SimpleTestProjectContext>();


                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj1",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.0"
                });

                var project2 = SimpleTestProjectContext.CreateNETCore(
                    $"proj2",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project2.AddPackageToAllFrameworks(packageX);
                project2.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "2.0.*"
                });

                solution.Projects.Add(project2);
                solution.Projects.Add(project);

                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var assetsPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.5.0", "netcoreapp1.0", "project.assets.json");
                var cachePath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.5.0", "netcoreapp1.0", "z.nuget.cache");

                // Act
                var r = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));


                // Setup Again. Add the new package....should not be picked up though
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageZ20);

                var assetsPath20 = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.0.0", "netcoreapp1.0", "project.assets.json");
                var cachePath20 = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "2.0.0", "netcoreapp1.0", "z.nuget.cache");

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                Assert.True(File.Exists(assetsPath20));
                Assert.True(File.Exists(cachePath20));

            }
        }

        [Fact]
        public async Task RestoreNetCore_MultipleProjects_SameTool_NoOpAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                for (var i = 0; i < 10; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"proj{i}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    project.AddPackageToAllFrameworks(packageX);
                    project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = "1.0.0"
                    });

                    solution.Projects.Add(project);
                    solution.Create(pathContext.SolutionRoot);
                }

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ);

                var assetsPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "project.assets.json");
                var cachePath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "z.nuget.cache");

                // Act
                var r = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                // Act
                var r2 = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));
                Assert.Contains($"The restore inputs for 'z-netcoreapp1.0-[1.0.0, )' have not changed. No further actions are required to complete the restore", r2.Output);

                r = Util.RestoreSolution(pathContext);

            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleToolRestoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                packageZ.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                });

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ,
                    packageY);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "project.assets.json");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(path), r.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleToolRestore_NoopAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                packageZ.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                });

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ,
                    packageY);

                var assetsPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "project.assets.json");
                var cachePath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "z.nuget.cache");

                // Act
                var r = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                // Act
                var r2 = Util.RestoreSolution(pathContext);
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));
                Assert.Contains($"The restore inputs for 'z-netcoreapp1.0-[1.0.0, )' have not changed. No further actions are required to complete the restore.", r2.Output);

                r = Util.RestoreSolution(pathContext);
            }
        }

        // Just utlizing the infrastracture that we have here, rather than trying to create my own directory structure to test this :)
        [Theory]
        [InlineData("[1.0.0]", "1.0.0")]
        [InlineData("[5.0.0]", "5.0.0")]
        [InlineData("[1.5.0]", null)]
        [InlineData("1.1.*", "2.0.0")]
        public async Task ToolPathResolver_FindsBestMatchingToolVersionAsync(string requestedVersion, string expectedVersion)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);


                for (var i = 0; i < 10; i++)
                {
                    var project = SimpleTestProjectContext.CreateNETCore(
                        $"proj{i}",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));

                    var packageZ = new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = $"{i}.0.0"
                    };

                    project.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                    {
                        Id = "z",
                        Version = $"{i}.0.0"
                    });

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packageZ);
                    solution.Projects.Add(project);
                }

                solution.Create(pathContext.SolutionRoot);

                var r = Util.RestoreSolution(pathContext);


                // Arrange
                var target = new ToolPathResolver(pathContext.UserPackagesFolder, isLowercase: true);

                var expected = expectedVersion != null ?
                    Path.Combine(
                    pathContext.UserPackagesFolder,
                    ".tools",
                    "z",
                    expectedVersion,
                    "netcoreapp1.0")
                    : null;
                // Act
                var actual = target.GetBestToolDirectoryPath("z", VersionRange.Parse(requestedVersion), NuGetFramework.Parse("netcoreapp1.0"));

                // Assert
                Assert.True(StringComparer.Ordinal.Equals(expected, actual), $"{expected} : {actual}");
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreToolInChildProjectWithRecursive_NoOpAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectB.DotnetCLIToolReferences.Add(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var assetsPath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "x", "1.0.0", "netcoreapp1.0", "project.assets.json");
                var cachePath = Path.Combine(pathContext.UserPackagesFolder, ".tools", "x", "1.0.0", "netcoreapp1.0", "x.nuget.cache");

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, additionalArgs: "-Recursive");
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));

                // Act
                var r2 = Util.RestoreSolution(pathContext, expectedExitCode: 0, additionalArgs: "-Recursive");
                // Assert
                Assert.True(File.Exists(assetsPath));
                Assert.True(File.Exists(cachePath));
                Assert.Contains($"The restore inputs for 'x-netcoreapp1.0-[1.0.0, )' have not changed. No further actions are required to complete the restore.", r2.Output);
                Assert.Contains($"The restore inputs for 'a' have not changed. No further actions are required to complete the restore.", r2.Output);
                Assert.Contains($"The restore inputs for 'b' have not changed. No further actions are required to complete the restore.", r2.Output);

                r = Util.RestoreSolution(pathContext);
            }
        }

        [Fact]
        public async Task RestoreNetCore_RestoreToolInChildProjectWithRecursiveAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectB.DotnetCLIToolReferences.Add(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "x", "1.0.0", "netcoreapp1.0", "project.assets.json");

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0, additionalArgs: "-Recursive");

                // Assert
                Assert.True(File.Exists(path), r.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_SkipRestoreToolInChildProjectForNonRecursiveAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectB.DotnetCLIToolReferences.Add(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "x", "1.0.0", "netcoreapp1.0", "project.assets.json");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.False(File.Exists(path), r.Output);
            }
        }
        [Fact]
        public async Task RestoreNetCore_ToolRestoreWithNoVersionAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                packageZ.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.DotnetCLIToolReferences.Add(new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = ""
                });

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageZ,
                    packageY);

                var path = Path.Combine(pathContext.UserPackagesFolder, ".tools", "z", "1.0.0", "netcoreapp1.0", "project.assets.json");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.Contains("WARNING: NU1604", r.AllOutput);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyBuildCrossTargeting_VerifyImportOrderAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                var packageS = new SimpleTestPackageContext()
                {
                    Id = "s",
                    Version = "1.0.0"
                };

                packageX.AddFile("buildCrossTargeting/x.targets");
                packageZ.AddFile("buildCrossTargeting/z.targets");
                packageS.AddFile("buildCrossTargeting/s.targets");

                packageX.AddFile("buildCrossTargeting/x.props");
                packageZ.AddFile("buildCrossTargeting/z.props");
                packageS.AddFile("buildCrossTargeting/s.props");

                packageX.AddFile("build/x.targets");
                packageZ.AddFile("build/z.targets");
                packageS.AddFile("build/s.targets");

                packageX.AddFile("build/x.props");
                packageZ.AddFile("build/z.props");
                packageS.AddFile("build/s.props");

                packageX.AddFile("lib/net45/test.dll");
                packageZ.AddFile("lib/net45/test.dll");
                packageS.AddFile("lib/net45/test.dll");

                // To avoid sorting on case accidently
                // A -> X -> Z -> S
                packageX.Dependencies.Add(packageZ);
                packageZ.Dependencies.Add(packageS);

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageZ);
                projectA.AddPackageToAllFrameworks(packageS);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);

                var targetsXML = XDocument.Parse(File.ReadAllText(projectA.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                var propsXML = XDocument.Parse(File.ReadAllText(projectA.PropsOutput));
                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                // CrossTargeting should be first
                Assert.Equal(2, targetItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == '' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'net45' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());

                Assert.Equal(3, targetItemGroups[0].Elements().Count());
                Assert.EndsWith("s.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("z.targets", targetItemGroups[0].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("x.targets", targetItemGroups[0].Elements().ToList()[2].Attribute(XName.Get("Project")).Value);

                Assert.Equal(3, targetItemGroups[1].Elements().Count());
                Assert.EndsWith("s.targets", targetItemGroups[1].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("z.targets", targetItemGroups[1].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("x.targets", targetItemGroups[1].Elements().ToList()[2].Attribute(XName.Get("Project")).Value);

                Assert.Equal(2, propsItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == '' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'net45' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());

                Assert.Equal(3, propsItemGroups[0].Elements().Count());
                Assert.EndsWith("s.props", propsItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("z.props", propsItemGroups[0].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("x.props", propsItemGroups[0].Elements().ToList()[2].Attribute(XName.Get("Project")).Value);

                Assert.Equal(3, propsItemGroups[1].Elements().Count());
                Assert.EndsWith("s.props", propsItemGroups[1].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("z.props", propsItemGroups[1].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);
                Assert.EndsWith("x.props", propsItemGroups[1].Elements().ToList()[2].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyBuildCrossTargeting_VerifyImportIsAddedAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                packageX.AddFile("buildCrossTargeting/x.targets");
                packageX.AddFile("lib/net45/test.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);

                var targetsXML = XDocument.Parse(File.ReadAllText(projectA.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(1, targetItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == '' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal(1, targetItemGroups[0].Elements().Count());
                Assert.EndsWith("x.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyBuildCrossTargeting_VerifyNoDuplicateImportsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    NuGetFramework.Parse("net46"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("buildCrossTargeting/x.targets");
                packageX.AddFile("lib/net45/test.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);

                var targetsXML = XDocument.Parse(File.ReadAllText(projectA.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(1, targetItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == '' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal(1, targetItemGroups[0].Elements().Count());
                Assert.EndsWith("x.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyBuildCrossTargeting_VerifyImportIsNotAddedForUAPAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'net45': {
                                                            'x': '1.0.0'
                                                    }
                                                  }
                                               }");

                var projectA = SimpleTestProjectContext.CreateUAP(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    projectJson);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("buildCrossTargeting/x.targets");
                packageX.AddFile("lib/net45/test.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.False(File.Exists(projectA.TargetsOutput), r.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyBuildCrossTargeting_VerifyImportRequiresPackageNameAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                packageX.AddFile("buildCrossTargeting/a.targets");
                packageX.AddFile("buildCrossTargeting/a.props");
                packageX.AddFile("buildCrossTargeting/a.txt");
                packageX.AddFile("lib/net45/test.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                var msbuildTargetsItems = TargetsUtility.GetMSBuildPackageImports(projectA.TargetsOutput);
                var msbuildPropsItems = TargetsUtility.GetMSBuildPackageImports(projectA.PropsOutput);

                // Assert
                Assert.Equal(0, msbuildTargetsItems.Count);
                Assert.Equal(0, msbuildPropsItems.Count);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyBuildCrossTargeting_VerifyImportNotAllowedInSubFolderAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                packageX.AddFile("buildCrossTargeting/net45/x.targets");
                packageX.AddFile("buildCrossTargeting/net45/x.props");
                packageX.AddFile("lib/net45/test.dll");

                packageY.AddFile("buildCrossTargeting/a.targets");
                packageY.AddFile("buildCrossTargeting/a.props");
                packageY.AddFile("buildCrossTargeting/net45/y.targets");
                packageY.AddFile("buildCrossTargeting/net45/y.props");
                packageY.AddFile("lib/net45/test.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                var msbuildTargetsItems = TargetsUtility.GetMSBuildPackageImports(projectA.TargetsOutput);
                var msbuildPropsItems = TargetsUtility.GetMSBuildPackageImports(projectA.PropsOutput);

                // Assert
                Assert.Equal(0, msbuildTargetsItems.Count);
                Assert.Equal(0, msbuildPropsItems.Count);
            }
        }

        [Fact]
        public async Task RestoreNetCore_NETCoreImports_VerifyImportFromPackageIsIgnoredAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                packageX.AddFile("build/x.props", "<Project>This is a bad props file!!!!<");
                packageX.AddFile("build/x.targets", "<Project>This is a bad target file!!!!<");
                packageX.AddFile("lib/net45/test.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Restore one
                var r = Util.RestoreSolution(pathContext);

                var msbuildTargetsItems = TargetsUtility.GetMSBuildPackageImports(projectA.TargetsOutput);
                var msbuildPropsItems = TargetsUtility.GetMSBuildPackageImports(projectA.PropsOutput);

                // Assert
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.Equal(1, msbuildTargetsItems.Count);
                Assert.Equal(1, msbuildPropsItems.Count);


                // Act
                r = Util.RestoreSolution(pathContext);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);

                msbuildTargetsItems = TargetsUtility.GetMSBuildPackageImports(projectA.TargetsOutput);
                msbuildPropsItems = TargetsUtility.GetMSBuildPackageImports(projectA.PropsOutput);

                Assert.Equal(1, msbuildTargetsItems.Count);
                Assert.Equal(1, msbuildPropsItems.Count);

                Assert.True(r.ExitCode == 0);
            }
        }

        [Fact]
        public async Task RestoreNetCore_UAPImports_VerifyImportFromPackageIsIgnoredAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                        'x': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                        'net45': {
                                                    }
                                                  }
                                               }");

                var projectA = SimpleTestProjectContext.CreateUAP(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    projectJson);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/x.props", "<Project>This is a bad props file!!!!<");
                packageX.AddFile("build/x.targets", "<Project>This is a bad target file!!!!<");
                packageX.AddFile("lib/net45/test.dll");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Restore one
                var r = Util.RestoreSolution(pathContext);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);

                // Act
                r = Util.RestoreSolution(pathContext);
                Assert.True(r.ExitCode == 0);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_ProjectToProject_InterweavingAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                //  A         B        C          D         E       F          G
                // NETCore -> UAP -> Unknown -> NETCore -> UAP -> Unknown -> NETCore

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

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

                var projectC = SimpleTestProjectContext.CreateNonNuGet(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectD = SimpleTestProjectContext.CreateNETCore(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectE = SimpleTestProjectContext.CreateUAP(
                    "e",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    projectJson);

                var projectF = SimpleTestProjectContext.CreateNonNuGet(
                    "f",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectG = SimpleTestProjectContext.CreateNETCore(
                    "g",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // Link everything
                projectA.AddProjectToAllFrameworks(projectB);
                projectB.AddProjectToAllFrameworks(projectC);
                projectC.AddProjectToAllFrameworks(projectD);
                projectD.AddProjectToAllFrameworks(projectE);
                projectE.AddProjectToAllFrameworks(projectF);
                projectF.AddProjectToAllFrameworks(projectG);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0-beta"
                };

                // G -> X
                projectG.AddPackageToAllFrameworks(packageX);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Projects.Add(projectE);
                solution.Projects.Add(projectF);
                solution.Projects.Add(projectG);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targets = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.ToDictionary(e => e.Name);
                var libs = projectA.AssetsFile.Libraries.ToDictionary(e => e.Name);

                // Verify everything showed up
                Assert.Equal(new[] { "b", "c", "d", "e", "f", "g", "x" }, libs.Keys.OrderBy(s => s, StringComparer.OrdinalIgnoreCase));

                Assert.Equal("1.0.0-beta", targets["x"].Version.ToNormalizedString());
                Assert.Equal("package", targets["x"].Type);

                Assert.Equal("1.0.0", targets["g"].Version.ToNormalizedString());
                Assert.Equal("project", targets["g"].Type);
                Assert.Equal(".NETFramework,Version=v4.5", targets["g"].Framework);

                Assert.Equal("1.0.0", libs["g"].Version.ToNormalizedString());
                Assert.Equal("project", libs["g"].Type);
                Assert.Equal("../g/g.csproj", libs["g"].MSBuildProject);
                Assert.Equal("../g/g.csproj", libs["g"].Path);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCoreToUnknown()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNonNuGet(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);

                // This is not populated for unknown project types, but this may change in the future.
                Assert.Null(targetB.Framework);
                Assert.Equal("../b/b.csproj", libB.Path);

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCoreToUAP()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

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

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Equal(".NETFramework,Version=v4.5", targetB.Framework);

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
                Assert.Equal("../b/project.json", libB.Path); // TODO: is this right?
            }
        }

        [Fact]
        public async Task RestoreNetCore_ProjectToProject_UAPToNetCoreAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'UAP10.0': { }
                                                  }
                                               }");

                var projectA = SimpleTestProjectContext.CreateUAP(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("UAP10.0"),
                    projectJson);

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard1.3"),
                    NuGetFramework.Parse("net45"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                projectB.Frameworks[0].PackageReferences.Add(packageX);
                projectB.Frameworks[1].PackageReferences.Add(packageY);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var tfm = NuGetFramework.Parse("UAP10.0");
                var target = projectA.AssetsFile.GetTarget(tfm, runtimeIdentifier: null);
                var targetB = target.Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                var targetX = projectA.AssetsFile.Targets.Single(t => string.IsNullOrEmpty(t.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "x");
                var targetY = projectA.AssetsFile.Targets.Single(t => string.IsNullOrEmpty(t.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "y");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Equal(".NETStandard,Version=v1.3", targetB.Framework);

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
                Assert.Equal("../b/b.csproj", libB.Path);

                Assert.Equal("1.0.0", targetX.Version.ToNormalizedString());
                Assert.Null(targetY);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_UAPToUnknown()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'UAP10.0': { }
                                                  }
                                               }");

                var projectA = SimpleTestProjectContext.CreateUAP(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("UAP10.0"),
                    projectJson);

                var projectB = SimpleTestProjectContext.CreateNonNuGet(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard1.3"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("UAP10.0"))).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Null(targetB.Framework);

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
                Assert.Equal("../b/b.csproj", libB.Path);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_UAPToUAP_RestoreCSProjDirect()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectJsonA = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'NETCoreApp1.0': { }
                                                  }
                                               }");

                var projectA = SimpleTestProjectContext.CreateUAP(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"),
                    projectJsonA);

                var projectJsonB = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'netstandard1.5': { }
                                                  }
                                               }");

                var projectB = SimpleTestProjectContext.CreateUAP(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"),
                    projectJsonB);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("NETCoreApp1.0"))).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Equal(NuGetFramework.Parse("netstandard1.5"), NuGetFramework.Parse(targetB.Framework));

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
                Assert.Equal("../b/project.json", libB.Path);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCore_TransitiveForAllEdges()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'net462': { }
                                                  }
                                               }");

                var projectC = SimpleTestProjectContext.CreateUAP(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"),
                    projectJson);

                var projectD = SimpleTestProjectContext.CreateNonNuGet(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));

                var projectE = SimpleTestProjectContext.CreateNETCore(
                    "e",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));

                // Straight line
                projectA.AddProjectToAllFrameworks(projectB);
                projectB.AddProjectToAllFrameworks(projectC);
                projectC.AddProjectToAllFrameworks(projectD);
                projectD.AddProjectToAllFrameworks(projectE);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Projects.Add(projectE);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var assetsFile = projectA.AssetsFile;

                // Find all non _._ compile assets
                var flowingCompile = assetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries
                    .Where(e => e.Type == "project")
                    .Where(e => e.CompileTimeAssemblies.Any(f => !f.Path.EndsWith("_._")))
                    .Select(e => e.Name)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);

                Assert.Equal("bcde", string.Join("", flowingCompile));

                // Runtime should always flow
                var flowingRuntime = assetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries
                    .Where(e => e.Type == "project")
                    .Where(e => e.RuntimeAssemblies.Any(f => !f.Path.EndsWith("_._")))
                    .Select(e => e.Name)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);

                Assert.Equal("bcde", string.Join("", flowingRuntime));
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCore_TransitiveOffForAllEdges()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));
                projectA.PrivateAssets = "compile";

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));
                projectB.PrivateAssets = "compile";

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'net462': { }
                                                  }
                                               }");

                var projectC = SimpleTestProjectContext.CreateUAP(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"),
                    projectJson);
                projectC.PrivateAssets = "compile";

                var projectD = SimpleTestProjectContext.CreateNonNuGet(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));
                projectD.PrivateAssets = "compile";

                var projectE = SimpleTestProjectContext.CreateNETCore(
                    "e",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net462"));
                projectE.PrivateAssets = "compile";

                // Straight line
                projectA.AddProjectToAllFrameworks(projectB);
                projectB.AddProjectToAllFrameworks(projectC);
                projectC.AddProjectToAllFrameworks(projectD);
                projectD.AddProjectToAllFrameworks(projectE);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Projects.Add(projectE);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var assetsFile = projectA.AssetsFile;

                // Find all non _._ compile assets
                var flowingCompile = assetsFile.Targets.Single(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Libraries
                    .Where(e => e.Type == "project")
                    .Where(e => e.CompileTimeAssemblies.Any(f => !f.Path.EndsWith("_._")))
                    .Select(e => e.Name)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);

                Assert.Equal("b", string.Join("", flowingCompile));

                // Runtime should always flow
                var flowingRuntime = assetsFile.Targets.Single(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Libraries
                    .Where(e => e.Type == "project")
                    .Where(e => e.RuntimeAssemblies.Any(f => !f.Path.EndsWith("_._")))
                    .Select(e => e.Name)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);

                Assert.Equal("bcde", string.Join("", flowingRuntime));
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCoreToNETCore_RestoreCSProjDirect()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("NETCoreApp1.0")) && string.IsNullOrEmpty(e.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Equal(NuGetFramework.Parse("netstandard1.5"), NuGetFramework.Parse(targetB.Framework));

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
                Assert.Equal("../b/b.csproj", libB.Path);
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCoreToNETCore_VerifyVersionForDependency()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectD = SimpleTestProjectContext.CreateNETCore(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // A -> C
                projectA.AddProjectToAllFrameworks(projectC);

                // C -> D
                projectC.AddProjectToAllFrameworks(projectD);

                // Set project versions
                projectB.Version = "2.4.5-alpha.1.2+build.a.b.c";
                projectC.Version = "2.4.5-ignorethis";
                projectD.Version = "1.4.9-child.project";

                // Override with PackageVersion
                projectC.Properties.Add("PackageVersion", "2.4.5-alpha.7+use.this");

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("NETCoreApp1.0")) && string.IsNullOrEmpty(e.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                var targetC = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("NETCoreApp1.0")) && string.IsNullOrEmpty(e.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "c");
                var libC = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "c");

                var dDep = targetC.Dependencies.Single();

                Assert.Equal("project", targetB.Type);
                Assert.Equal("2.4.5-alpha.1.2", targetB.Version.ToNormalizedString());
                Assert.Equal("2.4.5-alpha.1.2", libB.Version.ToNormalizedString());

                Assert.Equal("project", targetC.Type);
                Assert.Equal("2.4.5-alpha.7", targetC.Version.ToNormalizedString());
                Assert.Equal("2.4.5-alpha.7", libC.Version.ToNormalizedString());

                // Verify the correct version of D is shown under project C
                Assert.Equal("[1.4.9-child.project, )", dDep.VersionRange.ToNormalizedString());
            }
        }

        [Fact]
        public void RestoreNetCore_ProjectToProject_NETCoreToNETCore_VerifyVersionForDependency_WithSnapshotsFails()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETCoreApp1.0"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                var projectD = SimpleTestProjectContext.CreateNETCore(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard1.5"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // A -> C
                projectA.AddProjectToAllFrameworks(projectC);

                // C -> D
                projectC.AddProjectToAllFrameworks(projectD);

                // Set project versions
                projectA.Version = "2.0.0-a.*";
                projectB.Version = "2.0.0-b.*";
                projectC.Version = "2.0.0-c.*";
                projectD.Version = "2.0.0-d.*";

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.False(0 == r.ExitCode, r.Output + " " + r.Errors);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyPropsAndTargetsAreWrittenWhenRestoreFailsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                packageX.AddFile("build/net45/x.targets");

                var packageY = new SimpleTestPackageContext("y");
                packageX.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var yPath = await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageY);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageX);

                // y does not exist
                yPath.Delete();

                // Act
                // Verify failure
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                var targets = TargetsUtility.GetMSBuildPackageImports(projectA.TargetsOutput);

                // Assert
                // Verify all files were written
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);
                Assert.Equal(1, targets.Count);
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleProjectAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);
                r.AllOutput.Should().NotContain("NU1503");
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleProjectWithPackageTargetFallbackAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                packageX.AddFile("lib/dnxcore50/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                // Add imports property
                projectA.Properties.Add("PackageTargetFallback", "portable-net45+win8;dnxcore50");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                var xTarget = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.Single();

                // Assert
                Assert.Equal("lib/dnxcore50/a.dll", xTarget.CompileTimeAssemblies.Single());
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleProjectWithPackageTargetFallbackAndWhitespaceAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                packageX.AddFile("lib/dnxcore50/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                // Add imports property with whitespace
                projectA.Properties.Add("PackageTargetFallback", "\n\t   portable-net45+win8 ; ; dnxcore50\n   ");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                var xTarget = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.Single();

                // Assert
                Assert.Equal("lib/dnxcore50/a.dll", xTarget.CompileTimeAssemblies.Single());
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleProject_SingleTFMAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var xml = File.ReadAllText(projectA.ProjectPath);
                xml = xml.Replace("<TargetFrameworks>", "<TargetFramework>");
                xml = xml.Replace("</TargetFrameworks>", "</TargetFramework>");
                File.WriteAllText(projectA.ProjectPath, xml);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);

                Assert.Equal(NuGetFramework.Parse("net45"), projectA.AssetsFile.Targets.Single(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).TargetFramework);
            }
        }

        [Fact]
        public void RestoreNetCore_SingleProject_NonNuGet()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNonNuGet(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act && Assert
                // Verify this is a noop and not a failure
                var r = Util.RestoreSolution(pathContext);
            }
        }

        [Fact]
        public void RestoreNetCore_NETCore_ProjectToProject_VerifyProjectInTarget()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetB = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "b");
                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "b");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Equal(".NETFramework,Version=v4.5", targetB.Framework);

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("../b/b.csproj", libB.Path);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);
            }
        }

        [Fact]
        public void RestoreNetCore_NETCore_ProjectToProject_VerifyPackageIdUsed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // Add package ids
                projectA.Properties.Add("PackageId", "x");
                projectB.Properties.Add("PackageId", "y");

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetB = projectA.AssetsFile
                    .GetTarget(NuGetFramework.Parse("net45"), runtimeIdentifier: null)
                    .Libraries
                    .SingleOrDefault(e => e.Name == "y");

                var libB = projectA.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "y");

                Assert.Equal("1.0.0", targetB.Version.ToNormalizedString());
                Assert.Equal("project", targetB.Type);
                Assert.Equal(".NETFramework,Version=v4.5", targetB.Framework);

                Assert.Equal("1.0.0", libB.Version.ToNormalizedString());
                Assert.Equal("project", libB.Type);
                Assert.Equal("y", libB.Name);
                Assert.Equal("../b/b.csproj", libB.Path);
                Assert.Equal("../b/b.csproj", libB.MSBuildProject);

                // Verify project name is used
                var group = projectA.AssetsFile.ProjectFileDependencyGroups.ToArray()[0];
                Assert.Equal("y >= 1.0.0", group.Dependencies.Single());
            }
        }

        [Fact]
        public void RestoreNetCore_NETCore_ProjectToProject_IgnoreMissingProjectReference()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Delete B
                File.Delete(projectB.ProjectPath);

                // Act && Assert
                // Missing projects are ignored during restore. These issues are reported at build time.
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);
            }
        }

        [Fact]
        public async Task RestoreNetCore_NETCore_ProjectToProject_VerifyTransitivePackageAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // B -> X
                projectB.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetX = projectA.AssetsFile.Targets.Single(target => string.IsNullOrEmpty(target.RuntimeIdentifier)).Libraries.SingleOrDefault(e => e.Name == "x");

                Assert.Equal("1.0.0", targetX.Version.ToNormalizedString());
            }
        }

        [Fact]
        public async Task RestoreNetCore_NETCore_ProjectToProjectMultipleTFM_VerifyTransitivePackagesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net46"),
                    NuGetFramework.Parse("netstandard1.6"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"),
                    NuGetFramework.Parse("netstandard1.3"));

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                // B -> X
                projectB.Frameworks[0].PackageReferences.Add(packageX);

                // B -> Y
                projectB.Frameworks[1].PackageReferences.Add(packageY);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetNet = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net46")) && string.IsNullOrEmpty(e.RuntimeIdentifier));
                var targetNS = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("netstandard1.6")) && string.IsNullOrEmpty(e.RuntimeIdentifier));

                Assert.Equal("x", targetNet.Libraries.Single(e => e.Type == "package").Name);
                Assert.Equal("y", targetNS.Libraries.Single(e => e.Type == "package").Name);
            }
        }

        [Fact]
        public async Task RestoreNetCore_NETCoreAndUAP_ProjectToProjectMultipleTFM_VerifyTransitivePackagesAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net46"),
                    NuGetFramework.Parse("netstandard1.6"));

                var projectJson = JObject.Parse(@"{
                                                    'dependencies': {
                                                        'x': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                        'netstandard1.3': { }
                                                  }
                                               }");

                var projectB = SimpleTestProjectContext.CreateUAP(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("netstandard1.3"),
                    projectJson);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                // B -> X
                projectB.Frameworks[0].PackageReferences.Add(packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var targetNet = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("net46")) && string.IsNullOrEmpty(e.RuntimeIdentifier));
                var targetNS = projectA.AssetsFile.Targets.Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("netstandard1.6")) && string.IsNullOrEmpty(e.RuntimeIdentifier));

                Assert.Equal("x", targetNet.Libraries.Single(e => e.Type == "package").Name);
                Assert.Equal("x", targetNS.Libraries.Single(e => e.Type == "package").Name);
            }
        }

        [Fact]
        public async Task RestoreNetCore_LegacyPackagesDirectorySettingsIsIsolatedToProjectAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectA.Properties.Add("RestoreLegacyPackagesDirectory", "true");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                projectC.Properties.Add("RestoreLegacyPackagesDirectory", "true");

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "PackageX",
                    Version = "1.0.0-BETA"
                };

                // C -> X
                projectC.Frameworks[0].PackageReferences.Add(packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                var xLibraryInA = projectA.AssetsFile.Libraries.Single(x => x.Name == packageX.Id);
                var xLibraryInB = projectB.AssetsFile.Libraries.Single(x => x.Name == packageX.Id);
                var xLibraryInC = projectC.AssetsFile.Libraries.Single(x => x.Name == packageX.Id);
                Assert.Equal("PackageX/1.0.0-BETA", xLibraryInA.Path);
                Assert.Equal("packagex/1.0.0-beta", xLibraryInB.Path);
                Assert.Equal("PackageX/1.0.0-BETA", xLibraryInC.Path);
            }
        }

        [Fact]
        public async Task RestoreNetCore_LegacyPackagesDirectoryEnabledInProjectFileAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "PackageX",
                    Version = "1.0.0-BETA"
                };

                packageX.AddFile("lib/net45/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.Properties.Add("RestoreLegacyPackagesDirectory", "true");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                var xLibrary = projectA.AssetsFile.Libraries.Single();

                // Assert
                Assert.Equal("PackageX/1.0.0-BETA", xLibrary.Path);
            }
        }

        [Fact]
        public async Task RestoreNetCore_LegacyPackagesDirectoryDisabledInProjectFileAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "PackageX",
                    Version = "1.0.0-BETA"
                };

                packageX.AddFile("lib/net45/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.Properties.Add("RestoreLegacyPackagesDirectory", "false");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                var xLibrary = projectA.AssetsFile.Libraries.Single();

                // Assert
                Assert.Equal("packagex/1.0.0-beta", xLibrary.Path);
            }
        }

        [Fact]
        public async Task RestoreNetCore_AssetTargetFallbackVerifyFallbackToNet46AssetsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);
                projectA.Properties.Add("AssetTargetFallback", "net461");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                var graph = projectA.AssetsFile.GetTarget(netcoreapp2, runtimeIdentifier: null);
                var lib = graph.GetTargetLibrary("x");

                // Assert
                lib.CompileTimeAssemblies.Select(e => e.Path)
                    .Should().BeEquivalentTo(new[] { "lib/net45/a.dll" },
                    "no compatible assets were found for ns2.0");

                r.AllOutput.Should().Contain("This package may not be fully compatible with your project.");
            }
        }

        [Fact]
        public async Task RestoreNetCore_AssetTargetFallbackVerifyNoFallbackToNet46AssetsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);
                projectA.Properties.Add("AssetTargetFallback", "net461");
                projectA.Properties.Add("RuntimeIdentifiers", "win10");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");
                packageX.AddFile("ref/netstandard1.0/a.dll");
                packageX.AddFile("ref/net461/a.dll");
                packageX.AddFile("runtimes/win10/native/a.dll");
                packageX.AddFile("runtimes/win10/lib/net461/a.dll");
                packageX.AddFile("build/net461/x.targets");
                packageX.AddFile("buildMultiTargeting/net461/x.targets");
                packageX.AddFile("contentFiles/any/net461/a.txt");

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                foreach (var graph in projectA.AssetsFile.Targets)
                {
                    var lib = graph.GetTargetLibrary("x");

                    lib.CompileTimeAssemblies.Select(e => e.Path)
                        .Should().BeEquivalentTo(new[] { "ref/netstandard1.0/a.dll" },
                        "ATF does not fallback to lib/net45 if other assets were found.");

                    lib.RuntimeAssemblies.Should().BeEmpty();
                    lib.BuildMultiTargeting.Should().BeEmpty();
                    lib.Build.Should().BeEmpty();
                    lib.ContentFiles.Should().BeEmpty();
                    lib.ResourceAssemblies.Should().BeEmpty();
                    // Native will contain a.dll for RID targets
                }
            }
        }

        [Fact]
        public void RestoreNetCore_AssetTargetFallbackWithProjectReference_VerifyFallbackToNet46Assets()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");
                var ne461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);
                projectA.Properties.Add("AssetTargetFallback", "net461");
                projectA.Properties.Add("RuntimeIdentifiers", "win10");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    ne461);
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                foreach (var graph in projectA.AssetsFile.Targets)
                {
                    var lib = graph.GetTargetLibrary("b");

                    lib.CompileTimeAssemblies.Select(e => e.Path)
                        .Should().BeEquivalentTo(new[] { "bin/placeholder/b.dll" });

                    lib.RuntimeAssemblies.Select(e => e.Path)
                        .Should().BeEquivalentTo(new[] { "bin/placeholder/b.dll" });

                    lib.BuildMultiTargeting.Should().BeEmpty();
                    lib.Build.Should().BeEmpty();
                    lib.ContentFiles.Should().BeEmpty();
                    lib.ResourceAssemblies.Should().BeEmpty();
                    // Native will contain a.dll for RID targets
                }
            }
        }

        [Fact]
        public void RestoreNetCore_AssetTargetFallbackWithProjectReference_VerifyNoFallbackToNet46Assets()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");
                var ne461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);
                projectA.Properties.Add("AssetTargetFallback", "net45");
                projectA.Properties.Add("RuntimeIdentifiers", "win10");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    ne461);
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1201");
            }
        }

        [Fact]
        public async Task RestoreNetCore_BothAssetTargetFallbackPackageTargetFallbackVerifyErrorAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);
                projectA.Properties.Add("AssetTargetFallback", "net461");
                projectA.Properties.Add("PackageTargetFallback", "dnxcore50");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.AllOutput.Should().Contain("PackageTargetFallback and AssetTargetFallback cannot be used together.");
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalSourcesAppliedAsync()
        {
            // Arrange
            using (var extraSource = TestDirectory.Create())
            using (var extraFallback = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreAdditionalProjectSources", extraSource.Path);
                projectA.Properties.Add("RestoreAdditionalProjectFallbackFolders", extraFallback.Path);

                var packageM = new SimpleTestPackageContext()
                {
                    Id = "m",
                    Version = "1.0.0"
                };

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageM);
                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);
                projectA.AddPackageToAllFrameworks(packageZ);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // M is only in the fallback folder
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.FallbackFolder,
                    PackageSaveMode.Defaultv3,
                    packageM);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Y is only in the extra source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Z is only in the extra fallback
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraFallback,
                    PackageSaveMode.Defaultv3,
                    packageZ);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                projectA.AssetsFile.Libraries.Select(e => e.Name).OrderBy(e => e).Should().BeEquivalentTo(new[] { "m", "x", "y", "z" });
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalSourcesConditionalOnFrameworkAsync()
        {
            // Arrange
            using (var extraSourceA = TestDirectory.Create())
            using (var extraSourceB = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1,
                    netcoreapp2);

                // Add conditional sources
                projectA.Frameworks[0].Properties.Add("RestoreAdditionalProjectSources", extraSourceA.Path);
                projectA.Frameworks[1].Properties.Add("RestoreAdditionalProjectSources", extraSourceB.Path);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSourceA,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Y is only in the extra source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSourceB,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                projectA.AssetsFile.Libraries.Select(e => e.Name).OrderBy(e => e).Should().BeEquivalentTo(new[] { "x", "y" });
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalFallbackFolderConditionalOnFrameworkAsync()
        {
            // Arrange
            using (var extraSourceA = TestDirectory.Create())
            using (var extraSourceB = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1,
                    netcoreapp2);

                // Add conditional sources
                projectA.Frameworks[0].Properties.Add("RestoreAdditionalProjectFallbackFolders", extraSourceA.Path);
                projectA.Frameworks[1].Properties.Add("RestoreAdditionalProjectFallbackFolders", extraSourceB.Path);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSourceA,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Y is only in the extra source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSourceB,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                projectA.AssetsFile.Libraries.Select(e => e.Name).OrderBy(e => e).Should().BeEquivalentTo(new[] { "x", "y" });

                // Verify fallback folder added
                projectA.AssetsFile.PackageFolders.Select(e => e.Path).Should().Contain(extraSourceA);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalFallbackFolderExcludeAsync()
        {
            // Arrange
            using (var extraSourceA = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp1 = NuGetFramework.Parse("netcoreapp1.0");
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp1,
                    netcoreapp2);

                // Add and remove a fallback source, also add it as a source
                projectA.Frameworks[0].Properties.Add("RestoreAdditionalProjectFallbackFolders", extraSourceA.Path);
                projectA.Frameworks[1].Properties.Add("RestoreAdditionalProjectFallbackFoldersExcludes", extraSourceA.Path);
                projectA.Frameworks[1].Properties.Add("RestoreAdditionalProjectSources", extraSourceA.Path);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSourceA,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Y is only in the extra source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSourceA,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                projectA.AssetsFile.Libraries.Select(e => e.Name).OrderBy(e => e).Should().BeEquivalentTo(new[] { "x", "y" });

                // Verify the fallback folder was not added
                projectA.AssetsFile.PackageFolders.Select(e => e.Path).Should().NotContain(extraSourceA);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalSourcesAppliedWithSingleFrameworkAsync()
        {
            // Arrange
            using (var extraSource = TestDirectory.Create())
            using (var extraFallback = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp1.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreAdditionalProjectSources", extraSource.Path);
                projectA.Properties.Add("RestoreAdditionalProjectFallbackFoldersExcludes", extraFallback.Path);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSource.Path,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                projectA.AssetsFile.Libraries.Select(e => e.Name).OrderBy(e => e).Should().BeEquivalentTo(new[] { "x" });

                // Verify the fallback folder was not added
                projectA.AssetsFile.PackageFolders.Select(e => e.Path).Should().NotContain(extraFallback.Path);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalFallbackFolderAppliedWithSingleFrameworkAsync()
        {
            // Arrange
            using (var extraSource = TestDirectory.Create())
            using (var extraFallback = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreAdditionalProjectFallbackFolders", extraFallback.Path);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraFallback.Path,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                projectA.AssetsFile.Libraries.Select(e => e.Name).OrderBy(e => e).Should().BeEquivalentTo(new[] { "x" });

                // Verify the fallback folder was added
                projectA.AssetsFile.PackageFolders.Select(e => e.Path).Should().Contain(extraFallback.Path);
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyPackagesFolderPathResolvedAgainstWorkingDirAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestorePackagesPath", "invalid");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var expectedFolder = Path.Combine(pathContext.WorkingDirectory, "pkgs");
                var unexpectedFolder = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "invalid");

                // Act
                var r = Util.RestoreSolution(pathContext, 0, "-PackagesDirectory", "pkgs");

                // Assert
                Directory.GetDirectories(expectedFolder).Should().NotBeEmpty();
                Directory.Exists(unexpectedFolder).Should().BeFalse();
                Directory.GetDirectories(pathContext.UserPackagesFolder).Should().BeEmpty();
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyAdditionalSourcesAppliedToToolsAsync()
        {
            // Arrange
            using (var extraSource = TestDirectory.Create())
            using (var extraFallback = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreAdditionalProjectSources", extraSource.Path);
                projectA.Properties.Add("RestoreAdditionalProjectFallbackFolders", extraFallback.Path);

                var packageM = new SimpleTestPackageContext()
                {
                    Id = "m",
                    Version = "1.0.0",
                    PackageType = PackageType.DotnetCliTool
                };

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    PackageType = PackageType.DotnetCliTool
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0",
                    PackageType = PackageType.DotnetCliTool
                };

                var packageZ = new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0",
                    PackageType = PackageType.DotnetCliTool
                };

                projectA.DotnetCLIToolReferences.Add(packageM);
                projectA.DotnetCLIToolReferences.Add(packageX);
                projectA.DotnetCLIToolReferences.Add(packageY);
                projectA.DotnetCLIToolReferences.Add(packageZ);


                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // M is only in the fallback folder
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.FallbackFolder,
                    PackageSaveMode.Defaultv3,
                    packageM);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Y is only in the extra source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Z is only in the extra fallback
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    extraFallback,
                    PackageSaveMode.Defaultv3,
                    packageZ);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Directory.Exists(new ToolPathResolver(pathContext.UserPackagesFolder).GetToolDirectoryPath(packageM.Id, NuGetVersion.Parse(packageM.Version), netcoreapp2));
                Directory.Exists(new ToolPathResolver(pathContext.UserPackagesFolder).GetToolDirectoryPath(packageX.Id, NuGetVersion.Parse(packageX.Version), netcoreapp2));
                Directory.Exists(new ToolPathResolver(pathContext.UserPackagesFolder).GetToolDirectoryPath(packageY.Id, NuGetVersion.Parse(packageY.Version), netcoreapp2));
                Directory.Exists(new ToolPathResolver(pathContext.UserPackagesFolder).GetToolDirectoryPath(packageZ.Id, NuGetVersion.Parse(packageZ.Version), netcoreapp2));

            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyPackagesFolderPathResolvedAgainstProjectPropertyAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestorePackagesPath", "valid");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var expectedFolder = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "valid");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Directory.GetDirectories(expectedFolder).Should().NotBeEmpty();
                Directory.GetDirectories(pathContext.UserPackagesFolder).Should().BeEmpty();
            }
        }
        // The scenario here is 2 different projects are setting RestoreSources, and the caching of the sources takes this into consideration
        [Fact]
        public async Task RestoreNetCore_VerifySourcesResolvedCorrectlyForMultipleProjectsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                var source1 = Path.Combine(pathContext.SolutionRoot, "source1");
                var source2 = Path.Combine(pathContext.SolutionRoot, "source2");

                // X is only in source1
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    source1,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Y is only in source2
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    source2,
                    PackageSaveMode.Defaultv3,
                    packageY);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreSources", source1);
                projectB.Properties.Add("RestoreSources", source2);

                projectA.AddPackageToAllFrameworks(packageX);
                projectB.AddPackageToAllFrameworks(packageY);


                solution.Projects.Add(projectB);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifySourcesResolvedAgainstProjectPropertyAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreSources", "sub");
                var source = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "sub");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    source,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifySourcesResolvedAgainstWorkingDirAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreSources", "invalid");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var relativeSourceName = "valid";
                var source = Path.Combine(pathContext.WorkingDirectory, relativeSourceName);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    source,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext, 0, "-Source", relativeSourceName);

                // Assert
                r.Success.Should().BeTrue();
            }
        }


        [Fact]
        public async Task RestoreNetCore_VerifyFallbackFoldersResolvedAgainstProjectPropertyAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                projectA.Properties.Add("RestoreFallbackFolders", "sub");
                var fallback = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "sub");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // X is only in the source
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallback,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Directory.GetDirectories(pathContext.UserPackagesFolder).Should().BeEmpty();
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyDisabledSourcesAreNotUsedAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // create a config file, no disabled sources
                var projectDir = Path.GetDirectoryName(projectA.ProjectPath);

                var configPath = Path.Combine(pathContext.SolutionRoot, "NuGet.Config");

                var doc = new XDocument();
                var configuration = new XElement(XName.Get("configuration"));
                doc.Add(configuration);

                var packageSources = new XElement(XName.Get("packageSources"));
                configuration.Add(packageSources);

                packageSources.Add(new XElement(XName.Get("clear")));

                var localSource = new XElement(XName.Get("add"));
                localSource.Add(new XAttribute(XName.Get("key"), "localSource"));
                localSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource));
                packageSources.Add(localSource);

                var brokenSource = new XElement(XName.Get("add"));
                brokenSource.Add(new XAttribute(XName.Get("key"), "brokenLocalSource"));
                brokenSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource + "brokenLocalSource"));
                packageSources.Add(brokenSource);

                // Disable that config
                var disabledPackageSources = new XElement(XName.Get("disabledPackageSources"));
                var disabledBrokenSource = new XElement(XName.Get("add"));
                disabledBrokenSource.Add(new XAttribute(XName.Get("key"), "brokenLocalSource"));
                disabledBrokenSource.Add(new XAttribute(XName.Get("value"), "true"));
                disabledPackageSources.Add(disabledBrokenSource);

                configuration.Add(disabledPackageSources);
                File.WriteAllText(configPath, doc.ToString());

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                r2.Success.Should().BeTrue();
                r2.AllOutput.Should().NotContain("brokenLocalSource");

            }
        }


        [Fact]
        public async Task RestoreNetCore_VerifyOrderOfConfigsAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // create a config file, no disabled sources
                var projectDir = Path.GetDirectoryName(projectA.ProjectPath);

                var configPath = Path.Combine(pathContext.SolutionRoot, "NuGet.Config");

                var doc = new XDocument();
                var configuration = new XElement(XName.Get("configuration"));
                doc.Add(configuration);

                var packageSources = new XElement(XName.Get("packageSources"));
                configuration.Add(packageSources);

                packageSources.Add(new XElement(XName.Get("clear")));

                var localSource = new XElement(XName.Get("add"));
                localSource.Add(new XAttribute(XName.Get("key"), "localSource"));
                localSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource));
                packageSources.Add(localSource);

                var packageSourceMapping = new XElement(XName.Get("packageSourceMapping"));
                packageSourceMapping.Add(new XElement(XName.Get("clear")));
                configuration.Add(packageSourceMapping);

                File.WriteAllText(configPath, doc.ToString());

                var solutionParent = Directory.GetParent(pathContext.SolutionRoot);
                var configPath2 = Path.Combine(solutionParent.FullName, "NuGet.Config");

                var doc2 = new XDocument();
                var configuration2 = new XElement(XName.Get("configuration"));
                doc2.Add(configuration2);

                var packageSources2 = new XElement(XName.Get("packageSources"));
                configuration2.Add(packageSources2);

                packageSources2.Add(new XElement(XName.Get("clear")));

                var brokenSource = new XElement(XName.Get("add"));
                brokenSource.Add(new XAttribute(XName.Get("key"), "brokenLocalSource"));
                brokenSource.Add(new XAttribute(XName.Get("value"), pathContext.PackageSource + "brokenLocalSource"));
                packageSources2.Add(brokenSource);

                // Disable that config
                var disabledPackageSources = new XElement(XName.Get("disabledPackageSources"));
                var disabledBrokenSource = new XElement(XName.Get("add"));
                disabledBrokenSource.Add(new XAttribute(XName.Get("key"), "brokenLocalSource"));
                disabledBrokenSource.Add(new XAttribute(XName.Get("value"), "true"));
                disabledPackageSources.Add(disabledBrokenSource);

                configuration2.Add(disabledPackageSources);

                configuration2.Add(packageSourceMapping);

                File.WriteAllText(configPath2, doc2.ToString());

                // Act
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                r2.Success.Should().BeTrue();

                // Configs closer to the user should be first
                Regex.Replace(r2.AllOutput, @"\s", "").Should().Contain($"NuGetConfigfilesused:{configPath}{configPath2}");
            }
        }

        [Fact]
        public async Task RestoreNetCore_VerifyConfigFileWithRelativePathIsUsedAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var xml = File.ReadAllText(projectA.ProjectPath);
                xml = xml.Replace("<TargetFrameworks>", "<TargetFramework>");
                xml = xml.Replace("</TargetFrameworks>", "</TargetFramework>");
                File.WriteAllText(projectA.ProjectPath, xml);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var subDir = Path.Combine(pathContext.SolutionRoot, "sub");
                var configPath = Path.Combine(subDir, "nuget.config");
                Directory.CreateDirectory(subDir);
                File.Move(pathContext.NuGetConfig, configPath);

                var relativePathToConfig = PathUtility.GetRelativePath(pathContext.WorkingDirectory + Path.DirectorySeparatorChar, configPath);

                // Act
                var r = Util.RestoreSolution(pathContext, 0, $"-ConfigFile {relativePathToConfig}");

                // Assert
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.Output);
                Assert.True(File.Exists(projectA.TargetsOutput), r.Output);
                Assert.True(File.Exists(projectA.PropsOutput), r.Output);

                Assert.Equal(NuGetFramework.Parse("net45"), projectA.AssetsFile.Targets.Single(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).TargetFramework);
            }
        }

        [Fact]
        public async Task RestoreNetCore_WithMultipleProjectToProjectReferences_NoOpsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var parentProject = SimpleTestProjectContext.CreateNETCore(
                    "parent",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var one = SimpleTestProjectContext.CreateNETCore(
                    "child1",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var two = SimpleTestProjectContext.CreateNETCore(
                    "child2",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var three = SimpleTestProjectContext.CreateNETCore(
                    "child3",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX
                    );
                var rnd = new Random();

                var projects = new SimpleTestProjectContext[] { one, two, three }.OrderBy(item => rnd.Next());

                // Parent -> children. Very important that these are added in a random order

                foreach (var project in projects)
                {
                    parentProject.AddProjectToAllFrameworks(project);
                }
                solution.Projects.Add(one);
                solution.Projects.Add(two);
                solution.Projects.Add(three);
                solution.Projects.Add(parentProject);
                solution.Create(pathContext.SolutionRoot);

                // Act && Assert
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                Assert.Equal(0, r.ExitCode);
                Assert.Contains("Writing cache file", r.Output);

                // Do it again, it should no-op now.
                // Act && Assert
                var r2 = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                Assert.Equal(0, r2.ExitCode);
                Assert.DoesNotContain("Writing cache file", r2.Output);
                Assert.Contains("The restore inputs for 'parent' have not changed. No further actions are required to complete the restore.", r2.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackageTypesDoNotAffectAssetsFileAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var project = SimpleTestProjectContext.CreateNETCore(
                    "project",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.PackageTypes.Add(PackageType.Dependency);
                packageX.PackageTypes.Add(PackageType.DotnetCliTool);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);
                project.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);

                // Act && Assert
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                Assert.Equal(0, r.ExitCode);
                Assert.Contains("Writing cache file", r.Output);
                Assert.Contains("Writing assets file to disk", r.Output);

                // Pre-condition, Assert deleting the correct file
                Assert.True(File.Exists(project.CacheFileOutputPath));
                File.Delete(project.CacheFileOutputPath);

                r = Util.RestoreSolution(pathContext, expectedExitCode: 0);

                Assert.Equal(0, r.ExitCode);
                Assert.Contains("Writing cache file", r.Output);
                Assert.DoesNotContain("Writing assets file to disk", r.Output);


            }
        }

        [Fact]
        public async Task RestoreNetCore_LongPathInPackage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
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
                packageX.AddFile(@"content/2.5.6/core/store/x64/netcoreapp2.0/microsoft.extensions.configuration.environmentvariables/2.0.0/lib/netstandard2.0/Microsoft.Extensions.Configuration.EnvironmentVariables.dll ");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);


                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_MultipleProjectsInSameDirectoryDoNotNoOp()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                    $"proj",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);

                var secondaryProjectName = Path.Combine(Path.GetDirectoryName(project.ProjectPath), "proj-copy.csproj");

                File.Copy(project.ProjectPath, secondaryProjectName);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Prerequisites
                var r1 = Util.Restore(pathContext, project.ProjectPath);
                Assert.Equal(0, r1.ExitCode);
                Assert.Contains("Writing cache file", r1.Output);
                Assert.Contains("Writing assets file to disk", r1.Output);

                var r2 = Util.Restore(pathContext, secondaryProjectName);
                Assert.Contains("Writing cache file", r2.Output);
                Assert.Equal(0, r2.ExitCode);
                Assert.Contains("Writing assets file to disk", r2.Output);

                // Act
                var result = Util.Restore(pathContext, project.ProjectPath);

                // Assert
                Assert.Equal(0, result.ExitCode);
                Assert.Contains("Writing cache file", result.Output);
                Assert.Contains("Writing assets file to disk", result.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_InteropTypePackage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");
                packageX.AddFile("embed/net461/a.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);


                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.NotNull(projectA.AssetsFile);

                foreach (var target in projectA.AssetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("x"));
                    Assert.NotNull(library);
                    Assert.Contains(library.EmbedAssemblies, embed => embed.Path.Equals("embed/net461/a.dll"));
                    Assert.Contains(library.CompileTimeAssemblies, embed => embed.Path.Equals("lib/net461/a.dll"));
                    Assert.Contains(library.RuntimeAssemblies, embed => embed.Path.Equals("lib/net461/a.dll"));
                }
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultiTFM_ProjectToProject_PackagesLockFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"),
                   NuGetFramework.Parse("net45"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"),
                   NuGetFramework.Parse("net46"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                // A -> B
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.AddProjectToAllFrameworks(projectB);

                // B
                projectB.AddPackageToFramework("net45", packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Second Restore
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public void RestoreNetCore_PackagesLockFile_LowercaseProjectNameSolutionRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution and projects
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "ProjectA",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net46"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "ProjectB",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                var target = lockFile.Targets.Single(t => t.RuntimeIdentifier == null);
                var projectReference = target.Dependencies.SingleOrDefault(d => d.Type == PackageDependencyType.Project);
                StringComparer.Ordinal.Equals(projectReference.Id, projectB.ProjectName.ToLowerInvariant()).Should().BeTrue();
            }
        }

        [Fact]
        public void RestoreNetCore_PackagesLockFile_LowercaseProjectNameProjectRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution abd projects
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "ProjectA",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net46"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "ProjectB",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net45"));

                // A -> B
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.Restore(pathContext, projectA.ProjectPath);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                var net46 = NuGetFramework.Parse("net46");
                var target = lockFile.Targets.First(t => t.TargetFramework == net46);
                var projectReference = target.Dependencies.SingleOrDefault(d => d.Type == PackageDependencyType.Project);
                StringComparer.Ordinal.Equals(projectReference.Id, projectB.ProjectName.ToLowerInvariant()).Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_BuildTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");
                packageY.AddFile("build/y.targets");
                packageY.AddFile("buildCrossTargeting/y.targets");
                packageY.AddFile("buildTransitive/y.targets");
                packageY.Exclude = "build;analyzer";

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/x.dll");
                packageX.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("y"));
                    Assert.NotNull(library);
                    Assert.Contains(library.Build, build => build.Path.Equals("buildTransitive/y.targets"));
                }
            }
        }

        [Fact]
        public async Task RestoreNetCore_SkipBuildTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net461"));

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");
                packageY.AddFile("build/y.targets");
                packageY.AddFile("buildCrossTargeting/y.targets");
                packageY.AddFile("buildTransitive/y.targets");
                packageY.Exclude = "buildTransitive";

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/x.dll");
                packageX.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("y"));
                    Assert.NotNull(library);
                    Assert.DoesNotContain(library.Build, build => build.Path.Equals("buildTransitive/y.targets"));
                }
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_DgSpecJsonIsNotOverridenDuringNoOp()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var project = SimpleTestProjectContext.CreateNETCore(
                   $"proj",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX);

                // Prerequisites
                var result = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");
                Assert.Equal(0, result.ExitCode);
                Assert.Contains("Writing cache file", result.Output);
                Assert.Contains("Writing assets file to disk", result.Output);
                Assert.Contains("Persisting dg", result.Output);

                var dgSpecFileName = Path.Combine(Path.GetDirectoryName(project.AssetsFileOutputPath), $"{Path.GetFileName(project.ProjectPath)}.nuget.dgspec.json");

                var fileInfo = new FileInfo(dgSpecFileName);
                Assert.True(fileInfo.Exists);
                var lastWriteTime = fileInfo.LastWriteTime;

                // Act
                result = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                // Assert
                Assert.Equal(0, result.ExitCode);
                Assert.DoesNotContain("Writing cache file", result.Output);
                Assert.DoesNotContain("Writing assets file to disk", result.Output);
                Assert.DoesNotContain("Persisting dg", result.Output);

                fileInfo = new FileInfo(dgSpecFileName);
                Assert.True(fileInfo.Exists);
                Assert.Equal(lastWriteTime, fileInfo.LastWriteTime);
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_EnableRestorePackagesWithLockFile_BySetProperty_ThenDeletePackageLockFile()
        {
            // Related issue: https://github.com/NuGet/Home/issues/7807
            // First senario : Enable RestorePackagesWithLockFile by only setting property.
            //     First restore should fail the No-op, generate the package lock file. After deleting the package lock file, run the second restore.
            //     The second restore should fail the No-op, and generate the package lock file.

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                   $"proj",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"));

                project.Properties.Add("RestorePackagesWithLockFile", "true");

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX);


                var packageLockFileName = project.NuGetLockFileOutputPath;
                var noOpFailedMsg = "The lock file for " + project.ProjectName + " at location " + packageLockFileName + " does not exist, no-op is not possible. Continuing restore.";

                // Act
                var result1 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                // Assert
                Assert.Equal(0, result1.ExitCode);
                Assert.Contains("Writing packages lock file at disk.", result1.Output);
                Assert.True(File.Exists(packageLockFileName));

                // Act
                File.Delete(packageLockFileName);
                Assert.False(File.Exists(packageLockFileName));

                var result2 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                //Assert
                Assert.Equal(0, result2.ExitCode);
                Assert.Contains(noOpFailedMsg, result2.Output);
                Assert.Contains("Writing packages lock file at disk.", result2.Output);
                Assert.True(File.Exists(packageLockFileName));
            }
        }


        [Fact]
        public async Task RestoreNetCore_NoOp_EnableRestorePackagesWithLockFile_BySetProperty_ThenNotDeletePackageLockFile()
        {
            // Related issue: https://github.com/NuGet/Home/issues/7807
            // Contrast test to the first senario : do not delete package lock file at the end of the first restore.
            //     First restore should fail the No-op, generate the package lock file. DO NOT delete the package lock file, run the second restore.
            //     The second restore should No-op, and won't generate the package lock file.

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                   $"proj",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"));

                project.Properties.Add("RestorePackagesWithLockFile", "true");

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX);


                var packageLockFileName = project.NuGetLockFileOutputPath;
                var noOpFailedMsg = "The lock file for " + project.ProjectName + " at location " + packageLockFileName + " does not exist, no-op is not possible. Continuing restore.";
                var noOpSucceedMsg = "No-Op restore. The cache will not be updated.";

                // Act
                var result1 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                // Assert
                Assert.Equal(0, result1.ExitCode);
                Assert.Contains("Writing packages lock file at disk.", result1.Output);
                Assert.True(File.Exists(packageLockFileName));

                // Act
                var result2 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                //Assert
                Assert.Equal(0, result2.ExitCode);
                Assert.Contains(noOpSucceedMsg, result2.Output);
                Assert.DoesNotContain("Writing packages lock file at disk.", result2.Output);
                Assert.True(File.Exists(packageLockFileName));
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_EnableRestorePackagesWithLockFile_ByAddLockFile_ThenDeletePackageLockFile()
        {
            // Related issue: https://github.com/NuGet/Home/issues/7807
            // Second senario : Enable RestorePackagesWithLockFile by only adding a lock file.
            //      First restore should fail the No-op, regenerate the package lock file. After deleting the package lock file, run the second restore.
            //      The second restore: since there is no property set and no lock file exists, no lockfile will be generated. And no-op succeed.

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                   $"proj",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX);

                var packageLockFileName = project.NuGetLockFileOutputPath;
                Assert.False(File.Exists(packageLockFileName));
                File.Create(packageLockFileName).Close();
                Assert.True(File.Exists(packageLockFileName));


                var noOpFailedMsg = "The lock file for " + project.ProjectName + " at location " + packageLockFileName + " does not exist, no-op is not possible. Continuing restore.";
                var noOpSucceedMsg = "No-Op restore. The cache will not be updated.";

                // Act
                var result1 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                // Assert
                Assert.Equal(0, result1.ExitCode);
                Assert.Contains("Writing packages lock file at disk.", result1.Output);
                Assert.True(File.Exists(packageLockFileName));

                // Act
                File.Delete(packageLockFileName);
                Assert.False(File.Exists(packageLockFileName));

                var result2 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                //Assert
                Assert.Equal(0, result2.ExitCode);
                Assert.Contains(noOpSucceedMsg, result2.Output);
                Assert.DoesNotContain("Writing packages lock file at disk.", result2.Output);
                Assert.False(File.Exists(packageLockFileName));
            }
        }

        [Fact]
        public async Task RestoreNetCore_NoOp_EnableRestorePackagesWithLockFile_ByAddLockFile_ThenNotDeletePackageLockFile()
        {
            // Related issue: https://github.com/NuGet/Home/issues/7807
            // Contrast test to the second senario : do not delete package lock file at the end of the first restore.
            //      First restore should fail the No-op, regenerate the package lock file. DO NOT delete the package lock file, run the second restore.
            //      The second restore: No-op should succeed, lock file will not be regenerated.

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                   $"proj",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"));

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX);

                var packageLockFileName = project.NuGetLockFileOutputPath;
                Assert.False(File.Exists(packageLockFileName));
                File.Create(packageLockFileName).Close();
                Assert.True(File.Exists(packageLockFileName));


                var noOpFailedMsg = "The lock file for " + project.ProjectName + " at location " + packageLockFileName + " does not exist, no-op is not possible. Continuing restore.";
                var noOpSucceedMsg = "No-Op restore. The cache will not be updated.";

                // Act
                var result1 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                // Assert
                Assert.Equal(0, result1.ExitCode);
                Assert.Contains("Writing packages lock file at disk.", result1.Output);
                Assert.True(File.Exists(packageLockFileName));

                // Act
                var result2 = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");

                //Assert
                Assert.Equal(0, result2.ExitCode);
                Assert.Contains(noOpSucceedMsg, result2.Output);
                Assert.DoesNotContain("Writing packages lock file at disk.", result2.Output);
                Assert.True(File.Exists(packageLockFileName));
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_SimplePackageDownload()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageDownloadToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version)), $"{packageX.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_InstallFirstPackageDownload()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX1 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageX2 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "2.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX1,
                    packageX2);

                projectA.AddPackageDownloadToAllFrameworks(packageX1);
                projectA.AddPackageDownloadToAllFrameworks(packageX2);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks[0].DownloadDependencies.Count);
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks[0].DownloadDependencies[0].Name);
                Assert.Equal($"[{packageX1.Version}, {packageX1.Version}]", lockFile.PackageSpec.TargetFrameworks[0].DownloadDependencies[0].VersionRange.ToNormalizedString());

                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX1.Identity.Id, packageX1.Version)), $"{packageX1.ToString()} is not installed");
                Assert.False(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX2.Identity.Id, packageX2.Version)), $"{packageX2.ToString()} should not be installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_SameIdMultipleVersions_MultiPackageDownload()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX1 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageX2 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "2.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX1,
                    packageX2);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var xml = projectA.GetXML();

                var props = new Dictionary<string, string>();
                var attributes = new Dictionary<string, string>();
                attributes.Add("Version", "[1.0.0];[2.0.0]");
                ProjectFileUtils.AddItem(
                                    xml,
                                    "PackageDownload",
                                    packageX1.Id,
                                    NuGetFramework.AnyFramework,
                                    props,
                                    attributes);

                xml.Save(projectA.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                Assert.Equal($"[{packageX1.Version}, {packageX1.Version}]", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().VersionRange.ToNormalizedString());
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Last().Name);
                Assert.Equal($"[{packageX2.Version}, {packageX2.Version}]", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Last().VersionRange.ToNormalizedString());

                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX1.Identity.Id, packageX1.Version)), $"{packageX1.ToString()} is not installed");
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX2.Identity.Id, packageX2.Version)), $"{packageX2.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultiTfm_PackageDownloadAndPackageReference()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46;net48";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX1 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageX2 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "2.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX1,
                    packageX2);

                projectA.AddPackageToFramework("net46", packageX1);

                projectA.AddPackageDownloadToFramework("net48", packageX2);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);

                Assert.Equal(1, lockFile.Libraries.Count);
                Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Count);
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                Assert.Equal($"[{packageX2.Version}, {packageX2.Version}]",
                    lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().VersionRange.ToNormalizedString());
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.First().Dependencies.Last().Name);
                Assert.Equal($"[{packageX1.Version}, )",
                    lockFile.PackageSpec.TargetFrameworks.First().Dependencies.First().LibraryRange.VersionRange.ToNormalizedString());
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX1.Identity.Id, packageX1.Version)),
                    $"{packageX1.ToString()} is not installed");
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX2.Identity.Id, packageX2.Version)),
                    $"{packageX2.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_MultiTfm_MultiPackageDownload()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net472;net48";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX1 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageX2 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "2.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX1,
                    packageX2);

                projectA.AddPackageDownloadToFramework("net472", packageX1);

                projectA.AddPackageDownloadToFramework("net48", packageX2);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);

                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(0, lockFile.PackageSpec.TargetFrameworks.First().Dependencies.Count);
                Assert.Equal(0, lockFile.PackageSpec.TargetFrameworks.Last().Dependencies.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Count);

                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                Assert.Equal($"[{packageX2.Version}, {packageX2.Version}]",
                    lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().VersionRange.ToNormalizedString());
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Last().Name);
                Assert.Equal($"[{packageX1.Version}, {packageX1.Version}]",
                    lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().VersionRange.ToNormalizedString());
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX1.Identity.Id, packageX1.Version)),
                    $"{packageX1.ToString()} is not installed");
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX2.Identity.Id, packageX2.Version)),
                    $"{packageX2.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_PackageDownload_NonExactVersion_Fails()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX1 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX1);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var xml = projectA.GetXML();

                var props = new Dictionary<string, string>();
                var attributes = new Dictionary<string, string>();
                attributes.Add("Version", "1.0.0");
                ProjectFileUtils.AddItem(
                                    xml,
                                    "PackageDownload",
                                    packageX1.Id,
                                    NuGetFramework.AnyFramework,
                                    props,
                                    attributes);

                xml.Save(projectA.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                Assert.False(r.Success, r.AllOutput);
                Assert.False(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackageDownload_NoOpAccountsForMissingPackages()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageDownloadToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var r = Util.RestoreSolution(pathContext);

                // Preconditions
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);
                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                var packagePath = Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version);
                Assert.True(Directory.Exists(packagePath), $"{packageX.ToString()} is not installed");

                Directory.Delete(packagePath, true);

                Assert.False(Directory.Exists(packagePath), $"{packageX.ToString()} should not be installed anymore.");


                // Act
                r = Util.RestoreSolution(pathContext);
                Assert.True(r.Success, r.AllOutput);

                Assert.True(Directory.Exists(packagePath), $"{packageX.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackageDownload_DoesNotAFfectNoOp()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageDownloadToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var r = Util.RestoreSolution(pathContext);

                // Preconditions
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);
                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                var packagePath = Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version);
                Assert.True(Directory.Exists(packagePath), $"{packageX.ToString()} is not installed");
                Assert.Contains("Writing cache file", r.Output);


                // Act
                r = Util.RestoreSolution(pathContext);
                Assert.True(r.Success, r.AllOutput);

                Assert.True(Directory.Exists(packagePath), $"{packageX.ToString()} is not installed");

                Assert.Equal(0, r.ExitCode);
                Assert.DoesNotContain("Writing cache file", r.Output);
                Assert.Contains("No further actions are required to complete", r.Output);
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_FrameworkReferenceFromPackage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.FrameworkReferences.Add(NuGetFramework.Parse("net45"), new string[] { "FrameworkRef" });
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(1, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.Targets.First().Libraries.Count);
                Assert.Equal(1, lockFile.Targets.First().Libraries.Single().FrameworkReferences.Count);
                Assert.Equal("FrameworkRef", lockFile.Targets.First().Libraries.Single().FrameworkReferences.Single());
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version)), $"{packageX.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_FrameworkReference_TransitivePackageToPackage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.FrameworkReferences.Add(NuGetFramework.Parse("net45"), new string[] { "FrameworkRef" });
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.UseDefaultRuntimeAssemblies = false;
                packageY.AddFile("lib/net45/y.dll");
                packageY.FrameworkReferences.Add(NuGetFramework.Parse("net45"), new string[] { "FrameworkRefY" });

                packageX.Dependencies.Add(packageY);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX,
                    packageY);

                projectA.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(2, lockFile.Targets.First().Libraries.Count);
                Assert.Equal("FrameworkRef", lockFile.Targets.First().Libraries.First().FrameworkReferences.Single());
                Assert.Equal("FrameworkRefY", lockFile.Targets.First().Libraries.Last().FrameworkReferences.Single());
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version)), $"{packageX.ToString()} is not installed");
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_FrameworkReference_TransitiveProjectToProject()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.FrameworkReferences.Add(NuGetFramework.Parse("net45"), new string[] { "FrameworkRef" });
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);


                var projectB = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    projectName: "b",
                    solutionRoot: pathContext.SolutionRoot,
                    frameworks: MSBuildStringUtility.Split(projectFrameworks));

                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);

                solution.Create(pathContext.SolutionRoot);

                var xml = projectB.GetXML();

                var props = new Dictionary<string, string>();
                var attributes = new Dictionary<string, string>();
                ProjectFileUtils.AddItem(
                                    xml,
                                    "FrameworkReference",
                                    "FrameworkRefY",
                                    NuGetFramework.AnyFramework,
                                    props,
                                    attributes);

                xml.Save(projectB.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(2, lockFile.Targets.First().Libraries.Count);
                Assert.Equal("FrameworkRef", lockFile.Targets.First().Libraries.First().FrameworkReferences.Single());
                Assert.Equal("FrameworkRefY", lockFile.Targets.First().Libraries.Last().FrameworkReferences.Single());
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version)), $"{packageX.ToString()} is not installed");

                // Assert 2
                Assert.True(File.Exists(projectB.AssetsFileOutputPath), r.AllOutput);

                lockFile = LockFileUtilities.GetLockFile(projectB.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(0, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(0, lockFile.Targets.First().Libraries.Count);
                Assert.Equal("FrameworkRefY", lockFile.PackageSpec.TargetFrameworks.Single().FrameworkReferences.Single().Name);
                Assert.Equal("none", FrameworkDependencyFlagsUtils.GetFlagString(lockFile.PackageSpec.TargetFrameworks.Single().FrameworkReferences.Single().PrivateAssets));
            }
        }

        [Fact]
        public async Task RestoreNetCore_SingleTFM_FrameworkReference_TransitiveProjectToProject_PrivateAssets_SuppressesReference()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var projectFrameworks = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                            projectName: "a",
                            solutionRoot: pathContext.SolutionRoot,
                            frameworks: MSBuildStringUtility.Split(projectFrameworks));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.FrameworkReferences.Add(NuGetFramework.Parse("net45"), new string[] { "FrameworkRef" });
                packageX.Files.Clear();
                packageX.AddFile("lib/net45/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);


                var projectB = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    projectName: "b",
                    solutionRoot: pathContext.SolutionRoot,
                    frameworks: MSBuildStringUtility.Split(projectFrameworks));

                projectB.AddProjectToAllFrameworks(projectA);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);

                solution.Create(pathContext.SolutionRoot);

                var xml = projectA.GetXML();
                var props = new Dictionary<string, string>();
                var attributes = new Dictionary<string, string>();
                ProjectFileUtils.AddItem(
                                    xml,
                                    "FrameworkReference",
                                    "FrameworkRefY",
                                    NuGetFramework.AnyFramework,
                                    props,
                                    attributes);
                attributes.Add("PrivateAssets", "all");
                ProjectFileUtils.AddItem(
                                    xml,
                                    "FrameworkReference",
                                    "FrameworkRefSupressed",
                                    NuGetFramework.AnyFramework,
                                    props,
                                    attributes);

                xml.Save(projectA.ProjectPath);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                Assert.True(r.Success, r.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath), r.AllOutput);

                var lockFile = LockFileUtilities.GetLockFile(projectA.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(1, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(1, lockFile.Targets.First().Libraries.Count);
                Assert.Equal("FrameworkRef", string.Join(",", lockFile.Targets.First().Libraries.First().FrameworkReferences));
                Assert.True(Directory.Exists(Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id, packageX.Version)), $"{packageX.ToString()} is not installed");
                Assert.Equal("all", FrameworkDependencyFlagsUtils.GetFlagString(lockFile.PackageSpec.TargetFrameworks.Single().FrameworkReferences.First().PrivateAssets));
                Assert.Equal("none", FrameworkDependencyFlagsUtils.GetFlagString(lockFile.PackageSpec.TargetFrameworks.Single().FrameworkReferences.Last().PrivateAssets));

                // Assert 2
                Assert.True(File.Exists(projectB.AssetsFileOutputPath), r.AllOutput);

                lockFile = LockFileUtilities.GetLockFile(projectB.AssetsFileOutputPath, Common.NullLogger.Instance);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                Assert.Equal(2, lockFile.Targets.First().Libraries.Count);
                Assert.Equal("FrameworkRef", string.Join(",", lockFile.Targets.First().Libraries.First().FrameworkReferences));
                Assert.Equal("FrameworkRefY", string.Join(",", lockFile.Targets.First().Libraries.Last().FrameworkReferences));

            }
        }

        [Fact]
        public async Task RestoreNetCore_MovedProject_DoesNotOverwriteMSBuildPropsTargets()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCore(
                   $"proj",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net45"));

                project.SetMSBuildProjectExtensionsPath = false;
                project.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX);

                // Prerequisites
                var result = Util.Restore(pathContext, project.ProjectPath, additionalArgs: "-verbosity Detailed");
                Assert.True(result.Success);
                Assert.Contains("Writing cache file", result.AllOutput);
                Assert.Contains("Writing assets file to disk", result.AllOutput);

                // Move the project
                var movedProjectFolder = Path.Combine(pathContext.SolutionRoot, "newProjectDir");
                Directory.Move(Path.GetDirectoryName(project.ProjectPath), movedProjectFolder);
                var movedProjectPath = Path.Combine(movedProjectFolder, Path.GetFileName(project.ProjectPath));

                // Act
                result = Util.Restore(pathContext, movedProjectPath, additionalArgs: "-verbosity Detailed");

                // Assert
                Assert.True(result.Success);
                Assert.Contains("Writing cache file", result.AllOutput);
                Assert.Contains("Writing assets file to disk", result.AllOutput);
                Assert.DoesNotContain("Generating MSBuild file", result.AllOutput);

            }
        }

        [Fact]
        public async Task RestoreNetCore_IncompatiblePackageTypesFailRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var project = SimpleTestProjectContext.CreateNETCore(
                    "project",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net46"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.PackageTypes.Add(PackageType.DotnetPlatform);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                project.AddPackageToAllFrameworks(packageX);

                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);

                // Act & Assert
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);
                Assert.Contains(NuGetLogCode.NU1213.GetName(), r.AllOutput);
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_PackageRemove_UpdatesLockFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{tfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the project File name.
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 1);
                Assert.Equal(lockFile.Targets.First().Dependencies.First().Id, "x");

                // Setup - remove package
                projectA.Frameworks.First().PackageReferences.Clear();
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 0);
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_PackageRemoveTransitive_UpdatesLockFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                  "b",
                  pathContext.SolutionRoot,
                  NuGetFramework.Parse(tfm));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{tfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectB.AddPackageToAllFrameworks(packageX);
                projectA.AddProjectToAllFrameworks(projectB);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.False(File.Exists(projectB.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the project File name.
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 2);
                Assert.Equal(lockFile.Targets.First().Dependencies.FirstOrDefault(e => e.Type == PackageDependencyType.Transitive).Id, "x");
                Assert.Equal(lockFile.Targets.First().Dependencies.FirstOrDefault(e => e.Type == PackageDependencyType.Project).Id, "b");

                // Setup - remove package
                projectB.Frameworks.First().PackageReferences.Clear();
                projectB.Save();

                // Act
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 1);
                Assert.Equal(lockFile.Targets.First().Dependencies.First().Id, "b");
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_CustomAssemblyName_DoesNotBreakLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{tfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                // A -> B
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectB.Properties.Add("AssemblyName", "CustomName");

                projectA.AddProjectToAllFrameworks(projectB);

                // B
                projectB.AddPackageToFramework(tfm, packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.False(File.Exists(projectB.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the project custom name.
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 2);
                Assert.Equal("CustomName", lockFile.Targets.First().Dependencies.First(e => e.Type == PackageDependencyType.Project).Id);

                // Setup - Enable locked mode
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();
                File.Delete(projectA.AssetsFileOutputPath);

                // Act
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_CustomPackageId_DoesNotBreakLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{tfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                // A -> B
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectB.Properties.Add("PackageId", "CustomName");

                projectA.AddProjectToAllFrameworks(projectB);

                // B
                projectB.AddPackageToFramework(tfm, packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.False(File.Exists(projectB.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the custom name.
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 2);
                Assert.Equal("CustomName", lockFile.Targets.First().Dependencies.First(e => e.Type == PackageDependencyType.Project).Id);

                // Setup - Enable locked mode
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();
                File.Delete(projectA.AssetsFileOutputPath);

                // Act
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public void RestoreNetCore_PackagesLockFile_ProjectReferenceChange_UpdatesLockFile()
        {
            // Arrange
            // A -> B -> C and
            // A -> B, A -> C
            // should have different lock files
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                  "b",
                  pathContext.SolutionRoot,
                  NuGetFramework.Parse(tfm));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                  "c",
                  pathContext.SolutionRoot,
                  NuGetFramework.Parse(tfm));


                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.AddProjectToAllFrameworks(projectB);
                projectB.AddProjectToAllFrameworks(projectC);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.False(File.Exists(projectB.NuGetLockFileOutputPath));
                Assert.True(File.Exists(projectC.AssetsFileOutputPath));
                Assert.False(File.Exists(projectC.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the project File name.
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 2);
                Assert.Equal(lockFile.Targets.First().Dependencies.First().Id, "b");
                Assert.Equal(lockFile.Targets.First().Dependencies.First().Dependencies.Count, 1);
                Assert.Equal(lockFile.Targets.First().Dependencies.Last().Id, "c");

                // Setup - remove package
                projectB.Frameworks.First().ProjectReferences.Clear();
                projectB.Save();
                projectA.Frameworks.First().ProjectReferences.Add(projectC);
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 2);
                Assert.Equal(lockFile.Targets.First().Dependencies.First().Id, "b");
                Assert.Equal(lockFile.Targets.First().Dependencies.First().Dependencies.Count, 0);
                Assert.Equal(lockFile.Targets.First().Dependencies.Last().Id, "c");
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_WithProjectAndPackageReference_DoesNotBreakLockedMode()
        {
            // A -> B -> C
            //        -> X
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                  "b",
                  pathContext.SolutionRoot,
                  NuGetFramework.Parse(tfm));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                  "c",
                  pathContext.SolutionRoot,
                  NuGetFramework.Parse(tfm));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{tfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.AddProjectToAllFrameworks(projectB);
                projectB.AddPackageToAllFrameworks(packageX);
                projectB.AddProjectToAllFrameworks(projectC);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.False(File.Exists(projectB.NuGetLockFileOutputPath));
                Assert.True(File.Exists(projectC.AssetsFileOutputPath));
                Assert.False(File.Exists(projectC.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the project File name.
                Assert.Equal(lockFile.Targets.First().Dependencies.Count, 3);
                Assert.Equal(lockFile.Targets.First().Dependencies.FirstOrDefault(e => e.Type == PackageDependencyType.Transitive).Id, "x");
                Assert.Equal(lockFile.Targets.First().Dependencies.FirstOrDefault(e => e.Type == PackageDependencyType.Project).Id, "b");
                Assert.Equal(lockFile.Targets.First().Dependencies.LastOrDefault(e => e.Type == PackageDependencyType.Project).Id, "c");

                // Setup - remove package
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_ExclusiveLowerBound_RestoreSucceeds()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };


                var packageX110 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.1.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100,
                   packageX110);

                solution.Projects.Add(projectA);

                solution.Create(pathContext.SolutionRoot);

                // Inject dependency with exclusive lower bound
                var doc = XDocument.Load(projectA.ProjectPath);
                var ns = doc.Root.GetDefaultNamespace().NamespaceName;
                doc.Root.AddFirst(
                        new XElement(XName.Get("ItemGroup", ns),
                            new XElement(XName.Get("PackageReference", ns),
                                new XAttribute(XName.Get("Include"), "x"),
                                new XAttribute(XName.Get("Version"), "(1.0.0, )"))));

                doc.Save(projectA.ProjectPath);
                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(1, projectA.AssetsFile.Libraries.Count);
                var packageX = projectA.AssetsFile.Libraries.First();
                Assert.NotNull(packageX);
                Assert.Equal("1.1.0", packageX.Version.ToString());
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackageDependencyWithExclusiveLowerBound_RestoreSucceeds()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var packageY = new SimpleTestPackageContext("y", "1.0.0")
                {
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>y</id>
                            <version>1.0.0</version>
                            <title />
                            <dependencies>
                                <group targetFramework=""net45"">
                                    <dependency id=""x"" version=""(1.0.0, )"" />
                                </group>
                            </dependencies>
                        </metadata>
                        </package>")
                };

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageX110 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.1.0"
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100,
                   packageX110,
                   packageY);

                projectA.AddPackageToAllFrameworks(packageY);

                solution.Projects.Add(projectA);

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(2, projectA.AssetsFile.Libraries.Count);
                var packageX = projectA.AssetsFile.Libraries.FirstOrDefault(e => e.Name.Equals("x"));
                Assert.NotNull(packageX);
                Assert.Equal("1.1.0", packageX.Version.ToString());
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_EmptyLockFile_ErrorsInLockedMode()
        {
            // A -> X
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{tfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                projectA.Properties.Add("RestoreLockedMode", "true");
                solution.Projects.Add(projectA);

                solution.Create(pathContext.SolutionRoot);

                File.WriteAllText(projectA.NuGetLockFileOutputPath, "");

                // Act
                var r = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                r.Success.Should().BeFalse();
                r.AllOutput.Should().Contain("NU1004");
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_AssetTargetFallback_DoesNotBreakLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "netcoreapp2.0";
                var fallbackTfm = "net46";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));
                projectA.Properties.Add("AssetTargetFallback", fallbackTfm);
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(tfm));
                projectB.Properties.Add("AssetTargetFallback", fallbackTfm); // This is the important ATF.

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{fallbackTfm}/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToFramework(tfm, packageX);

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.False(File.Exists(projectB.NuGetLockFileOutputPath));
                var packagesLockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                // Assert that the project name is the project custom name.
                Assert.Equal(packagesLockFile.Targets.First().Dependencies.Count, 2);
                Assert.Equal(packagesLockFile.Targets.First().Dependencies.First(e => e.Type == PackageDependencyType.Project).Id, "b");

                // Setup - Enable locked mode
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();
                File.Delete(projectA.CacheFileOutputPath);

                // Act
                result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RestoreNetCore_ProjectProvidedRuntimeIdentifierGraph_SelectsCorrectRuntimeAssets()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp20 = "netcoreapp2.0";
                var netcoreapp21 = "netcoreapp2.1";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(netcoreapp20), NuGetFramework.Parse(netcoreapp21));
                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86");
                projectA.Properties.Add("RuntimeIdentifier", " ");

                // Set up the package and source
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/netcoreapp2.0/x.dll");
                packageX.AddFile("ref/netcoreapp2.0/x.dll");
                packageX.AddFile("runtimes/win7/lib/netcoreapp2.0/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                // set up rid graph
                var ridGraphPath = Path.Combine(pathContext.WorkingDirectory, "runtime.json");
                projectA.Frameworks.First(e => e.Framework.GetShortFolderName().Equals(netcoreapp20)).Properties.Add("RuntimeIdentifierGraphPath", ridGraphPath);
                File.WriteAllBytes(ridGraphPath, GetTestUtilityResource("runtime.json"));

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(4, projectA.AssetsFile.Targets.Count);
                Assert.Equal(1, projectA.AssetsFile.Libraries.Count);
                Assert.Equal("runtimes/win7/lib/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));
                Assert.Equal("lib/netcoreapp2.0/x.dll",
                                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));
            }
        }

        [Fact]
        public async Task RestoreNetCore_BadProjectProvidedRuntimeIdentifierGraph_FailsRestore()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp20 = "netcoreapp2.0";
                var netcoreapp21 = "netcoreapp2.1";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(netcoreapp20), NuGetFramework.Parse(netcoreapp21));
                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86");
                projectA.Properties.Add("RuntimeIdentifier", " ");

                // Set up the package and source
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/netcoreapp2.0/x.dll");
                packageX.AddFile("ref/netcoreapp2.0/x.dll");
                packageX.AddFile("runtimes/win7/lib/netcoreapp2.0/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                // set up rid graph
                var ridGraphPath = Path.Combine(pathContext.WorkingDirectory, "runtime.json");
                projectA.Frameworks.First(e => e.Framework.GetShortFolderName().Equals(netcoreapp20)).Properties.Add("RuntimeIdentifierGraphPath", ridGraphPath);
                File.WriteAllText(ridGraphPath, "{ dsadas , dasda, dsadas { } : dsada } ");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                result.Success.Should().BeFalse();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Contains("NU1007", result.AllOutput);
                Assert.Equal(NuGetLogCode.NU1007, projectA.AssetsFile.LogMessages.Single().Code);
            }
        }

        [Fact]
        public async Task RestoreNetCore_ProjectProvidedRuntimeIdentifierGraphChange_DoesNotAffectNoOp()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp20 = "netcoreapp2.0";
                var netcoreapp21 = "netcoreapp2.1";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(netcoreapp20), NuGetFramework.Parse(netcoreapp21));
                projectA.Properties.Add("RuntimeIdentifiers", "win7-x86");
                projectA.Properties.Add("RuntimeIdentifier", " ");

                // Set up the package and source
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/netcoreapp2.0/x.dll");
                packageX.AddFile("ref/netcoreapp2.0/x.dll");
                packageX.AddFile("runtimes/win7/lib/netcoreapp2.0/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);

                // setup rid graph.
                var ridGraphPath = Path.Combine(pathContext.WorkingDirectory, "runtime.json");
                projectA.Frameworks.First(e => e.Framework.GetShortFolderName().Equals(netcoreapp20)).Properties.Add("RuntimeIdentifierGraphPath", ridGraphPath);
                File.WriteAllBytes(ridGraphPath, GetTestUtilityResource("runtime.json"));

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var commandRunnerResult = Util.RestoreSolution(pathContext);

                // Assert
                commandRunnerResult.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(4, projectA.AssetsFile.Targets.Count);
                Assert.Equal(1, projectA.AssetsFile.Libraries.Count);
                Assert.Equal("runtimes/win7/lib/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));
                Assert.Equal("lib/netcoreapp2.0/x.dll",
                                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));

                // second set-up. Change the graph. Affect no-op.
                File.Delete(ridGraphPath);
                ridGraphPath = Path.Combine(pathContext.WorkingDirectory, "runtime-2.json");
                projectA.Frameworks.First(e => e.Framework.GetShortFolderName().Equals(netcoreapp20)).Properties["RuntimeIdentifierGraphPath"] = ridGraphPath;
                File.WriteAllBytes(ridGraphPath, GetTestUtilityResource("runtime.json"));
                projectA.Save();

                // Act & Assert
                commandRunnerResult = Util.RestoreSolution(pathContext);
                commandRunnerResult.Success.Should().BeTrue();
                commandRunnerResult.AllOutput.Contains("Writing cache file");
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(4, projectA.AssetsFile.Targets.Count);
                Assert.Equal(1, projectA.AssetsFile.Libraries.Count);
                Assert.Equal("runtimes/win7/lib/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));
                Assert.Equal("lib/netcoreapp2.0/x.dll",
                                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));

                // second set-up. Change the graph. Affect no-op.
                File.Delete(ridGraphPath);
                File.WriteAllText(ridGraphPath, "{ }"); // empty rid graph.
                projectA.Save();

                // Act & Assert. The result should not be affected by the runtime json change.
                commandRunnerResult = Util.RestoreSolution(pathContext);
                commandRunnerResult.Success.Should().BeTrue();
                Assert.Contains("No-Op restore", commandRunnerResult.AllOutput);
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(4, projectA.AssetsFile.Targets.Count);
                Assert.Equal(1, projectA.AssetsFile.Libraries.Count);
                Assert.Equal("runtimes/win7/lib/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp20)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));
                Assert.Equal("lib/netcoreapp2.0/x.dll",
                                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().RuntimeAssemblies.Select(e => e.Path)));
                Assert.Equal("ref/netcoreapp2.0/x.dll",
                    string.Join(";", projectA.AssetsFile.Targets.First(e => string.Equals("win7-x86", e.RuntimeIdentifier) && string.Equals(e.TargetFramework.GetShortFolderName(), netcoreapp21)).Libraries.Single().CompileTimeAssemblies.Select(e => e.Path)));


            }
        }

        [Theory]
        [InlineData(new string[] { "win7-x86" }, new string[] { "win-x64" })]
        [InlineData(new string[] { "win7-x86", "win-x64" }, new string[] { "win-x64" })]
        [InlineData(new string[] { "win7-x86" }, new string[] { "win7-x86", "win-x64" })]
        public void RestoreNetCore_PackagesLockFile_WithProjectChangeRuntimeAndLockedMode_FailsRestore(string[] intitialRuntimes, string[] updatedRuntimes)
        {
            // A project with RestoreLockedMode should fail restore if the project's runtime is changed between restores.
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var tfm = "net45";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   new NuGetFramework[] { NuGetFramework.Parse(tfm) });

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.Properties.Add("RuntimeIdentifiers", string.Join(";", intitialRuntimes));

                solution.Projects.Add(projectA);

                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                var lockRuntimes = lockFile.Targets.Where(t => t.RuntimeIdentifier != null).Select(t => t.RuntimeIdentifier).ToList();
                intitialRuntimes.Should().BeEquivalentTo(lockRuntimes);

                // Setup - change runtimes
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Remove("RuntimeIdentifiers");
                projectA.Properties.Add("RuntimeIdentifiers", string.Join(";", updatedRuntimes));
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext, 1);

                // Assert
                r.Success.Should().BeFalse();
                lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                lockRuntimes = lockFile.Targets.Where(t => t.RuntimeIdentifier != null).Select(t => t.RuntimeIdentifier).ToList();
                // No change expected in the lock file.
                intitialRuntimes.Should().BeEquivalentTo(lockRuntimes);
                Assert.Contains("NU1004", r.Errors);
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
            }
        }

        [Theory]
        [InlineData(new string[] { "net45" }, new string[] { "net46" })]
        [InlineData(new string[] { "net45", "net46" }, new string[] { "net46" })]
        [InlineData(new string[] { "net45" }, new string[] { "net45", "net46" })]
        public void RestoreNetCore_PackagesLockFile_WithProjectChangeFramweorksAndLockedMode_FailsRestore(string[] intitialFrameworks, string[] updatedFrameworks)
        {
            // A project with RestoreLockedMode should fail restore if the project's frameworks list is changed between restores.
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   intitialFrameworks.Select(tfm => NuGetFramework.Parse(tfm)).ToArray());

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // The framework as they are in the lock file
                var lockFrameworkTransformed = intitialFrameworks.Select(f => $".NETFramework,Version=v{f.Replace("net", "")[0]}.{f.Replace("net", "")[1]}").ToList();
                _output.WriteLine($"InputFrameworks: {string.Join(",", lockFrameworkTransformed)}");

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                var lockFrameworks = lockFile.Targets.Select(t => t.TargetFramework.DotNetFrameworkName).Distinct().ToList();
                _output.WriteLine($"PackageLockFrameworks First Evaluation: {string.Join(",", lockFrameworks)}");
                lockFrameworks.Should().BeEquivalentTo(lockFrameworkTransformed);

                // Setup - change frameworks
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Frameworks = updatedFrameworks.Select(tfm => new SimpleTestProjectFrameworkContext(NuGetFramework.Parse(tfm))).ToList();
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext, 1);

                // Assert
                r.Success.Should().BeFalse();
                lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                lockFrameworks = lockFile.Targets.Select(t => t.TargetFramework.DotNetFrameworkName).Distinct().ToList();
                _output.WriteLine($"PackageLockFrameworks Second Evaluation: {string.Join(",", lockFrameworks)}");
                // The frameworks should not change in the lock file.
                lockFrameworks.Should().BeEquivalentTo(lockFrameworkTransformed);
                Assert.Contains("NU1004", r.Errors);
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
            }
        }

        [Theory]
        [InlineData(new string[] { "x_lockmodedepch/1.0.0" }, new string[] { "x_lockmodedepch/2.0.0" })]
        [InlineData(new string[] { "x_lockmodedepch/1.0.0" }, new string[] { "y_lockmodedepch/1.0.0" })]
        [InlineData(new string[] { "x_lockmodewdepch/1.0.0" }, new string[] { "x_lockmodedepch/1.0.0", "y_lockmodedepch/1.0.0" })]
        [InlineData(new string[] { "x_lockmodedepch/1.0.0", "y_lockmodedepch/1.0.0" }, new string[] { "y_lockmodedepch/1.0.0" })]
        public async Task RestoreNetCore_PackagesLockFile_WithProjectChangePackageDependencyAndLockedMode_FailsRestore(
            string[] initialPackageIdAndVersion,
            string[] updatedPackageIdAndVersion)
        {
            // A project with RestoreLockedMode should fail restore if the project's package dependencies were changed between restores.
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp20 = "netcoreapp2.0";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(netcoreapp20));

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                // Set up the package and source
                var packages = initialPackageIdAndVersion.Select(p =>
                {
                    var id = p.Split('/')[0];
                    var version = p.Split('/')[1];
                    var package = new SimpleTestPackageContext()
                    {
                        Id = id,
                        Version = version
                    };
                    package.Files.Clear();
                    package.AddFile($"lib/netcoreapp2.0/{id}.dll");
                    package.AddFile($"ref/netcoreapp2.0/{id}.dll");
                    package.AddFile($"runtimes/win7/lib/netcoreapp2.0/{id}.dll");
                    return package;
                }).ToArray();

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packages);
                projectA.AddPackageToAllFrameworks(packages);

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Setup - change project's packages
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.CleanPackagesFromAllFrameworks();
                packages = updatedPackageIdAndVersion.Select(p =>
                {
                    var id = p.Split('/')[0];
                    var version = p.Split('/')[1];
                    var package = new SimpleTestPackageContext()
                    {
                        Id = id,
                        Version = version
                    };
                    package.Files.Clear();
                    package.AddFile($"lib/netcoreapp2.0/{id}.dll");
                    package.AddFile($"ref/netcoreapp2.0/{id}.dll");
                    package.AddFile($"runtimes/win7/lib/netcoreapp2.0/{id}.dll");
                    return package;
                }).ToArray();

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packages);
                projectA.AddPackageToAllFrameworks(packages);
                projectA.Save();

                // Act
                r = Util.RestoreSolution(pathContext, 1);

                // Assert
                r.Success.Should().BeFalse();
                Assert.Contains("NU1004", r.Errors);
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
            }
        }

        [Theory]
        [InlineData(new string[] { "x_lockmodetdepch/1.0.0" }, new string[] { "x_lockmodetdepch/2.0.0" })]
        [InlineData(new string[] { "x_lockmodetdepch/1.0.0" }, new string[] { "y_lockmodetdepch/1.0.0" })]
        [InlineData(new string[] { "x_lockmodetdepch/1.0.0" }, new string[] { "x_lockmodetdepch/1.0.0", "y_lockmodetdepch/1.0.0" })]
        [InlineData(new string[] { "x_lockmodetdepch/1.0.0", "y_lockmodetdepch/1.0.0" }, new string[] { "y_lockmodetdepch/1.0.0" })]
        public async Task RestoreNetCore_PackagesLockFile_WithDependentProjectChangeOfPackageDependencyAndLockedMode_FailsRestore(
           string[] initialPackageIdAndVersion,
           string[] updatedPackageIdAndVersion)
        {
            // A project with RestoreLockedMode should fail restore if the package dependencies of a dependent project were changed between restores.
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp20 = "netcoreapp2.0";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(netcoreapp20));

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(netcoreapp20));

                // Set up the package and source
                var packages = initialPackageIdAndVersion.Select(p =>
                {
                    var id = p.Split('/')[0];
                    var version = p.Split('/')[1];
                    var package = new SimpleTestPackageContext()
                    {
                        Id = id,
                        Version = version
                    };
                    package.Files.Clear();
                    package.AddFile($"lib/netcoreapp2.0/{id}.dll");
                    package.AddFile($"ref/netcoreapp2.0/{id}.dll");
                    package.AddFile($"runtimes/win7/lib/netcoreapp2.0/{id}.dll");
                    return package;
                }).ToArray();

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packages);
                projectB.AddPackageToAllFrameworks(packages);

                projectA.AddProjectToAllFrameworks(projectB);
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Setup - change project's packages
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();
                projectB.CleanPackagesFromAllFrameworks();
                packages = updatedPackageIdAndVersion.Select(p =>
                {
                    var id = p.Split('/')[0];
                    var version = p.Split('/')[1];
                    var package = new SimpleTestPackageContext()
                    {
                        Id = id,
                        Version = version
                    };
                    package.Files.Clear();
                    package.AddFile($"lib/netcoreapp2.0/{id}.dll");
                    package.AddFile($"ref/netcoreapp2.0/{id}.dll");
                    package.AddFile($"runtimes/win7/lib/netcoreapp2.0/{id}.dll");
                    return package;
                }).ToArray();

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packages);
                projectB.AddPackageToAllFrameworks(packages);
                projectB.Save();

                // Act
                r = Util.RestoreSolution(pathContext, 1);

                // Assert
                r.Success.Should().BeFalse();
                Assert.Contains("NU1004", r.Errors);
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
            }
        }

        [Theory]
        [InlineData("netcoreapp2.0", new string[] { "netcoreapp2.0" }, new string[] { "netcoreapp2.2" })]
        [InlineData("netcoreapp2.0", new string[] { "netcoreapp2.0", "netcoreapp2.2" }, new string[] { "netcoreapp2.2" })]
        public void RestoreNetCore_PackagesLockFile_WithDependentProjectChangeOfNotCompatibleFrameworksAndLockedMode_FailsRestore(
           string mainProjectFramework,
           string[] initialFrameworks,
           string[] updatedFrameworks)
        {
            // A project with RestoreLockedMode should fail restore if the frameworks of a dependent project were changed
            // with incompatible frameworks between restores.
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(mainProjectFramework));

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   initialFrameworks.Select(tfm => NuGetFramework.Parse(tfm)).ToArray());

                projectA.AddProjectToAllFrameworks(projectB);
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Setup - change package version
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();
                projectB.Frameworks = updatedFrameworks.Select(tfm => new SimpleTestProjectFrameworkContext(NuGetFramework.Parse(tfm))).ToList();
                projectB.Save();

                // Act
                r = Util.RestoreSolution(pathContext, 1);

                // Assert
                r.Success.Should().BeFalse();
                Assert.Contains("NU1004", r.Errors);
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
            }
        }

        [Theory]
        [InlineData("netcoreapp2.2", new string[] { "netcoreapp2.2" }, new string[] { "netcoreapp2.0", "netcoreapp2.2" })]
        [InlineData("netcoreapp2.2", new string[] { "netcoreapp2.0", "netcoreapp2.2" }, new string[] { "netcoreapp2.2" })]
        [InlineData("netcoreapp2.2", new string[] { "netcoreapp2.0" }, new string[] { "netcoreapp2.2" })]
        public void RestoreNetCore_PackagesLockFile_WithDependentProjectChangeOfCompatibleFrameworksAndLockedMode_PassRestore(
           string mainProjectFramework,
           string[] initialFrameworks,
           string[] updatedFrameworks)
        {
            // A project with RestoreLockedMode should pass restore if the frameworks of a dependent project were changed
            // with still compatible with main project frameworks between restores.
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(mainProjectFramework));

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   initialFrameworks.Select(tfm => NuGetFramework.Parse(tfm)).ToArray());

                projectA.AddProjectToAllFrameworks(projectB);
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Setup - change package version
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();
                projectB.Frameworks = updatedFrameworks.Select(tfm => new SimpleTestProjectFrameworkContext(NuGetFramework.Parse(tfm))).ToList();
                projectB.Save();

                // Act
                r = Util.RestoreSolution(pathContext, 0);

                // Assert
                r.Success.Should().BeTrue();
            }
        }



        [Fact]
        public async Task RestoreNetCore_PackagesLockFile_WithReorderedRuntimesInLockFile_PassRestore()
        {
            // A project with RestoreLockedMode should pass restore if the runtimes in the lock file have been reordered
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);


                var projFramework = Frameworks.FrameworkConstants.CommonFrameworks.NetCoreApp21.GetShortFolderName();

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(projFramework));

                var runtimeidentifiers = new List<string>() { "win7-x64", "win-x86", "win", "z", "a" };
                var ascending = runtimeidentifiers.OrderBy(i => i);

                projectA.Properties.Add("RuntimeIdentifiers", string.Join(";", ascending));

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                // Set up the package and source
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/netcoreapp2.0/x.dll");
                packageX.AddFile("ref/netcoreapp2.0/x.dll");
                packageX.AddFile("lib/net461/x.dll");
                packageX.AddFile("ref/net461/x.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                projectA.AddPackageToAllFrameworks(packageX);


                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);


                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                var packagesLockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                //Modify the list of target/runtimes so they are reordered in the lock file
                //Verify the passed in RIDs are within the lock file
                //Verify the RIDS are not the same after reordering.
                //Lock file is not ordered based on input RIDs so Validating the reorder here.
                var originalTargets = packagesLockFile.Targets.Where(t => t.RuntimeIdentifier != null).Select(t => t.RuntimeIdentifier).ToList();
                runtimeidentifiers.Should().BeEquivalentTo(originalTargets);

                //Nuget.exe test so reordering to make it not match.  It should still restore correctly
                packagesLockFile.Targets = packagesLockFile.Targets.
                    OrderByDescending(t => t.RuntimeIdentifier == null).
                    ThenByDescending(i => i.RuntimeIdentifier).ToList();
                var reorderedTargets = packagesLockFile.Targets.Where(t => t.RuntimeIdentifier != null).Select(t => t.RuntimeIdentifier).ToList();

                //The orders are not equal.  Then resave the lock file and project.
                //The null RID must be the first one otherwise this fails
                Assert.False(originalTargets.SequenceEqual(reorderedTargets));
                Assert.True(packagesLockFile.Targets[0].RuntimeIdentifier == null);
                PackagesLockFileFormat.Write(projectA.NuGetLockFileOutputPath, packagesLockFile);
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();


                //Run the restore and it should still properly restore.
                var r2 = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

            }
        }

        [Theory]
        [InlineData("1.0.0;2.0.0", "*", "2.0.0")]
        [InlineData("1.0.0;2.0.0", "0.*", "1.0.0")]
        public async Task RestoreNetCore_WithFloatingVersion_SelectsCorrectVersion(string availableVersions, string declaredProjectVersion, string expectedVersion)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var framework = "net472";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(framework));

                foreach (string version in availableVersions.Split(';'))
                {
                    // Set up the package and source
                    var package = new SimpleTestPackageContext()
                    {
                        Id = "x",
                        Version = version
                    };
                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                         pathContext.PackageSource,
                         PackageSaveMode.Defaultv3,
                         package);
                }

                projectA.AddPackageToAllFrameworks(new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = declaredProjectVersion
                });

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.Equal(expectedVersion, projectA.AssetsFile.Libraries.Single().Version.ToString());
            }
        }

        [Theory]
        [InlineData("1.0.0;2.0.0", "*", "2.0.0")]
        [InlineData("1.0.0;2.0.0", "0.*", "1.0.0")]
        public async Task RestoreNetCore_PackagesLockFileWithFloatingVersion_LockedModeIsRespected(string availableVersions, string declaredProjectVersion, string expectedVersion)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var framework = "net472";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(framework));
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                foreach (string version in availableVersions.Split(';'))
                {
                    // Set up the package and source
                    var package = new SimpleTestPackageContext()
                    {
                        Id = "x",
                        Version = version
                    };
                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                         pathContext.PackageSource,
                         PackageSaveMode.Defaultv3,
                         package);
                }

                projectA.AddPackageToAllFrameworks(new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = declaredProjectVersion
                });

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.Equal(expectedVersion, projectA.AssetsFile.Libraries.Single().Version.ToString());
                // Set-up again.
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Save();

                // Act
                result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
            }
        }

        [Fact]
        public async Task RestoreNetCore_PackageReferenceWithAliases_IsReflectedInAssetsFileAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var framework = "net472";

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse(framework));

                // Set up the package and source
                var package = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0",
                    Aliases = "Core"
                };
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                     pathContext.PackageSource,
                     PackageSaveMode.Defaultv3,
                     package);

                projectA.AddPackageToAllFrameworks(package);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                var library = projectA.AssetsFile.Targets.First(e => e.RuntimeIdentifier == null).Libraries.Single();
                library.Should().NotBeNull("The assets file is expect to have a single library");
                library.CompileTimeAssemblies.Count.Should().Be(1, because: "The package has only 1 compatible file");
                library.CompileTimeAssemblies.Single().Properties.Should().Contain(new KeyValuePair<string, string>(LockFileItem.AliasesProperty, "Core"));
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_DirectDependencyCentralVersionChanged_FailsRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"));
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageX100NullVersion = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };
                packageX100NullVersion.Files.Clear();
                packageX100NullVersion.AddFile("lib/net46/x.dll");

                var packageX200 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "2.0.0"
                };
                packageX200.Files.Clear();
                packageX200.AddFile("lib/net46/x.dll");

                var packageY100 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY100.Files.Clear();
                packageY100.AddFile("lib/net46/y.dll");
                packageX100.Dependencies.Add(packageY100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageY100, packageX200);

                projectA.AddPackageToAllFrameworks(packageX100NullVersion);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0")
                    .SetPackageVersion("y", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Second Restore
                r = Util.RestoreSolution(pathContext);

                // Update the cpvm
                cpvmFile.SetPackageVersion("x", "2.0.0");
                cpvmFile.Save();

                // Expect exit code 1 on this restore
                r = Util.RestoreSolution(pathContext, 1);
                Assert.True(r.AllOutput.Contains("NU1004: The package reference x version has changed from [1.0.0, ) to [2.0.0, )."));
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_TransitiveDependencyCentralVersionChanged_FailsRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"));
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageX100NullVersion = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageY200 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "2.0.0"
                };
                packageY200.Files.Clear();
                packageY200.AddFile("lib/net46/x.dll");

                var packageY100 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY100.Files.Clear();
                packageY100.AddFile("lib/net46/y.dll");
                packageX100.Dependencies.Add(packageY100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageY100, packageY200);

                projectA.AddPackageToAllFrameworks(packageX100NullVersion);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0")
                    .SetPackageVersion("y", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Update the transitive dependency in cpvm
                cpvmFile.SetPackageVersion("y", "2.0.0");
                cpvmFile.Save();

                // Expect exit code 1 on this restore
                r = Util.RestoreSolution(pathContext, 1);
                Assert.True(r.AllOutput.Contains("NU1004: Mistmatch between the requestedVersion of a lock file dependency marked as CentralTransitive and the the version specified in the central package management file. " +
                    "Lock file version [1.0.0, ), central package management version [2.0.0, )."));
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_RemovedCentralDirectDependency_FailsRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"));
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageX100NullVersion = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };
                packageX100NullVersion.Files.Clear();
                packageX100NullVersion.AddFile("lib/net46/x.dll");

                var packageY100 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY100.Files.Clear();
                packageY100.AddFile("lib/net46/y.dll");
                packageX100.Dependencies.Add(packageY100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageY100);

                projectA.AddPackageToAllFrameworks(packageX100NullVersion);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0")
                    .SetPackageVersion("y", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Update the cpvm
                cpvmFile.RemovePackageVersion("x");
                cpvmFile.Save();

                // Expect exit code 1 on this restore
                r = Util.RestoreSolution(pathContext, 1);
                Assert.True(r.AllOutput.Contains("NU1004: The package reference x version has changed from [1.0.0, ) to (, )."));
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_RemovedCentralTransitiveDependency_FailsRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"));
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageX100NullVersion = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageY100 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY100.Files.Clear();
                packageY100.AddFile("lib/net46/y.dll");
                packageX100.Dependencies.Add(packageY100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageY100);

                projectA.AddPackageToAllFrameworks(packageX100NullVersion);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0")
                    .SetPackageVersion("y", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Update the cpvm
                cpvmFile.RemovePackageVersion("y");
                cpvmFile.Save();

                // Expect exit code 1 on this restore
                r = Util.RestoreSolution(pathContext, 1);
                Assert.True(r.AllOutput.Contains("NU1004: Central package management file(s) doesn't contain version range for y package which is specified as CentralTransitive dependency in the lock file."));
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_MoveTransitiveDependnecyToCentralFile_FailsRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"));
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageX100NullVersion = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageY100 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY100.Files.Clear();
                packageY100.AddFile("lib/net46/y.dll");
                packageX100.Dependencies.Add(packageY100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageY100);

                projectA.AddPackageToAllFrameworks(packageX100NullVersion);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Update the cpvm
                cpvmFile.SetPackageVersion("y", "1.0.0");
                cpvmFile.Save();

                // Expect exit code 1 on this restore
                r = Util.RestoreSolution(pathContext, 1);
                Assert.True(r.AllOutput.Contains("NU1004: Transitive dependency y moved to be centraly managed invalidated the lock file."));
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_AddRemoveNotProjectRelatedEntriesToCentralFile_SuccessRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net46"));
                projectA.Properties.Add("RestoreLockedMode", "true");
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageX100NullVersion = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };
                packageX100.Files.Clear();
                packageX100.AddFile("lib/net46/x.dll");

                var packageY100 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY100.Files.Clear();
                packageY100.AddFile("lib/net46/y.dll");
                packageX100.Dependencies.Add(packageY100);

                var packageRandom = new SimpleTestPackageContext()
                {
                    Id = "random",
                    Version = "1.0.0"
                };
                packageRandom.Files.Clear();
                packageRandom.AddFile("lib/net46/x.dll");


                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageY100, packageRandom);

                projectA.AddPackageToAllFrameworks(packageX100NullVersion);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0")
                    .SetPackageVersion("y", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Add new package version the cpvm
                cpvmFile.SetPackageVersion("random", "1.0.0");
                cpvmFile.Save();

                // the addition should not impact this restore
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();

                // Update the cpvm
                cpvmFile.RemovePackageVersion("random");
                cpvmFile.Save();

                // the removal should not impact this restore
                r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        /// <summary>
        /// Project A -> PackageX 100
        ///           -> PackageY 200 -> PackageX 200
        ///            -> ProjectB -> ProjectC -> PackageX 100
        ///  All projects CPVM enabled; PackageX 100 and PackageY 200 in cpvm file
        ///  Expected NU1605
        /// </summary>
        [Fact]
        public async Task RestoreNetCore_CPVMProject_DowngradedByCentralDirectDependencyWithP2P_IsWarningNU1605()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var netcoreapp2 = NuGetFramework.Parse("netcoreapp2.0");

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                var projectB = SimpleTestProjectContext.CreateNETCore(
                    "b",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    netcoreapp2);

                var packageX100 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageX200 = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "2.0.0"
                };

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = null
                };

                var packageY200 = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "2.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = null
                };
                packageY200.Dependencies.Add(packageX200);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packageX100, packageX200, packageY200);

                projectA.AddPackageToAllFrameworks(packageX);
                projectA.AddPackageToAllFrameworks(packageY);
                projectA.AddProjectToAllFrameworks(projectB);

                projectB.AddProjectToAllFrameworks(projectC);

                projectC.AddPackageToAllFrameworks(packageX);

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("x", "1.0.0")
                    .SetPackageVersion("y", "2.0.0");

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                var restoreResult = Util.RestoreSolution(pathContext);

                Assert.True(restoreResult.AllOutput.Contains("NU1605"));
            }
        }

        /// <summary>
        /// A more complex graph with linked central transitive dependecies
        ///
        /// A -> B 1.0.0 -> C 1.0.0 -> D 1.0.0 -> E 1.0.0
        ///              -> P 2.0.0
        ///   -> F 1.0.0 -> C 2.0.0 -> H 2.0.0 -> M 2.0.0 -> N 2.0.0
        ///   -> G 1.0.0 -> H 1.0.0 -> D 1.0.0
        ///   -> X 1.0.0 -> Y 1.0.0 -> Z 1.0.0
        ///                         -> T 1.0.0
        ///   -> U 1.0.0 -> V 1.0.0
        ///              -> O 1.0.0 -> R 1.0.0 -> S 1.0.0 -> SS 1.0.0
        ///
        ///         D has version defined centrally 2.0.0
        ///         E has version defined centrally 3.0.0
        ///         M has version defined centrally 2.0.0
        ///         P has version defined centrally 3.0.0
        ///         Z has version defined centrally 3.0.0
        ///         T has version defined centrally 3.0.0
        ///         R has version defined centrally 3.0.0
        ///         S has version defined centrally 3.0.0
        ///
        ///  D 2.0.0 -> I 2.0.0 -> E 2.0.0
        ///  M 2.0.0 -> N 2.0.0
        ///  P 3.0.0 -> H 3.0.0
        ///          -> Y 3.0.0
        ///          -> O 3.0.0 -> S 3.0.0 -> SS 3.0.0
        ///  Z 3.0.0 -> V 3.0.0
        ///  T 3.0.0 -> W 3.0.0
        ///          -> C 1.0.0
        ///  S 3.0.0 -> SS 3.0.0
        ///
        ///  D will be rejected (because its parents C 1.0.0, H 1.0.0 are rejected)
        ///  E will be rejected (because its parent D was rejected)
        ///  M will be rejected (because its parent lost the dispute with H 3.0.0)
        ///  T will be rejected (because its parent lost the dispute with Y 3.0.0)
        ///  Z will be rejected (because its parent lost the dispute with Y 3.0.0)
        ///
        ///  P will be accepted (because its parent B is Accepted)
        ///  S will be accepted (because its parent O 300 is Accepted)
        /// </summary>
        [Fact]
        public async Task RestoreNetCore_CPVMProject_MultipleLinkedCentralTransitiveDepenencies()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var version100 = "1.0.0";
                var version200 = "2.0.0";
                var version300 = "3.0.0";

                var packagesForSource = new List<SimpleTestPackageContext>();
                var packagesForProject = new List<SimpleTestPackageContext>();
                var framework = NuGetFramework.Parse("netcoreapp2.0");

                SimpleTestPackageContext createTestPackage(string name, string version, List<SimpleTestPackageContext> source)
                {
                    var result = new SimpleTestPackageContext()
                    {
                        Id = name,
                        Version = version
                    };
                    result.Files.Clear();
                    source.Add(result);
                    return result;
                };

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "projectA",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("netcoreapp2.0"));
                projectA.Properties.Add(ProjectBuildProperties.CentralPackageTransitivePinningEnabled, bool.TrueString);

                // the package references defined in the project should not have version
                var packageBNoVersion = createTestPackage("B", null, packagesForProject);
                var packageFNoVersion = createTestPackage("F", null, packagesForProject);
                var packageGNoVersion = createTestPackage("G", null, packagesForProject);
                var packageUNoVersion = createTestPackage("U", null, packagesForProject);
                var packageXNoVersion = createTestPackage("X", null, packagesForProject);

                var packageB100 = createTestPackage("B", version100, packagesForSource);
                var packageC100 = createTestPackage("C", version100, packagesForSource);
                var packageD100 = createTestPackage("D", version100, packagesForSource);
                var packageE100 = createTestPackage("E", version100, packagesForSource);
                var packageF100 = createTestPackage("F", version100, packagesForSource);
                var packageG100 = createTestPackage("G", version100, packagesForSource);
                var packageH100 = createTestPackage("H", version100, packagesForSource);
                var packageX100 = createTestPackage("X", version100, packagesForSource);
                var packageY100 = createTestPackage("Y", version100, packagesForSource);
                var packageZ100 = createTestPackage("Z", version100, packagesForSource);
                var packageV100 = createTestPackage("V", version100, packagesForSource);
                var packageT100 = createTestPackage("T", version100, packagesForSource);
                var packageU100 = createTestPackage("U", version100, packagesForSource);
                var packageO100 = createTestPackage("O", version100, packagesForSource);
                var packageR100 = createTestPackage("R", version100, packagesForSource);
                var packageS100 = createTestPackage("S", version100, packagesForSource);
                var packageSS100 = createTestPackage("SS", version100, packagesForSource);

                var packageC200 = createTestPackage("C", version200, packagesForSource);
                var packageD200 = createTestPackage("D", version200, packagesForSource);
                var packageE200 = createTestPackage("E", version200, packagesForSource);
                var packageI200 = createTestPackage("I", version200, packagesForSource);
                var packageH200 = createTestPackage("H", version200, packagesForSource);
                var packageM200 = createTestPackage("M", version200, packagesForSource);
                var packageN200 = createTestPackage("N", version200, packagesForSource);
                var packageP200 = createTestPackage("P", version200, packagesForSource);

                var packageE300 = createTestPackage("E", version300, packagesForSource);
                var packageP300 = createTestPackage("P", version300, packagesForSource);
                var packageH300 = createTestPackage("H", version300, packagesForSource);
                var packageZ300 = createTestPackage("Z", version300, packagesForSource);
                var packageV300 = createTestPackage("V", version300, packagesForSource);
                var packageT300 = createTestPackage("T", version300, packagesForSource);
                var packageW300 = createTestPackage("W", version300, packagesForSource);
                var packageY300 = createTestPackage("Y", version300, packagesForSource);
                var packageO300 = createTestPackage("O", version300, packagesForSource);
                var packageR300 = createTestPackage("R", version300, packagesForSource);
                var packageS300 = createTestPackage("S", version300, packagesForSource);
                var packageSS300 = createTestPackage("SS", version300, packagesForSource);

                packageB100.Dependencies.Add(packageC100);
                packageC100.Dependencies.Add(packageD100);
                packageD100.Dependencies.Add(packageE100);

                packageB100.Dependencies.Add(packageP200);

                packageF100.Dependencies.Add(packageC200);
                packageC200.Dependencies.Add(packageH200);
                packageH200.Dependencies.Add(packageM200);
                packageM200.Dependencies.Add(packageN200);

                packageG100.Dependencies.Add(packageH100);
                packageH100.Dependencies.Add(packageD100);

                packageX100.Dependencies.Add(packageY100);
                packageY100.Dependencies.Add(packageZ100);
                packageY100.Dependencies.Add(packageT100);

                packageU100.Dependencies.Add(packageV100);
                packageU100.Dependencies.Add(packageO100);
                packageO100.Dependencies.Add(packageR100);
                packageR100.Dependencies.Add(packageS100);
                packageS100.Dependencies.Add(packageSS100);

                packageD200.Dependencies.Add(packageI200);
                packageI200.Dependencies.Add(packageE200);

                packageP300.Dependencies.Add(packageH300);
                packageP300.Dependencies.Add(packageY300);
                packageP300.Dependencies.Add(packageO300);
                packageO300.Dependencies.Add(packageS300);
                packageS300.Dependencies.Add(packageSS300);

                packageZ300.Dependencies.Add(packageV300);

                packageT300.Dependencies.Add(packageW300);
                packageT300.Dependencies.Add(packageC100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packagesForSource.ToArray());

                projectA.AddPackageToAllFrameworks(packagesForProject.ToArray());

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("B", version100)
                    .SetPackageVersion("F", version100)
                    .SetPackageVersion("G", version100)
                    .SetPackageVersion("E", version300)
                    .SetPackageVersion("D", version200)
                    .SetPackageVersion("M", version200)
                    .SetPackageVersion("P", version300)
                    .SetPackageVersion("Z", version300)
                    .SetPackageVersion("T", version300)
                    .SetPackageVersion("X", version100)
                    .SetPackageVersion("U", version100)
                    .SetPackageVersion("R", version300)
                    .SetPackageVersion("S", version300);

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));

                var assetFileReader = new LockFileFormat();
                var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);

                var expectedLibraries = new List<string>() { "B.1.0.0", "C.2.0.0", "F.1.0.0", "G.1.0.0", "H.3.0.0", "O.3.0.0", "P.3.0.0", "S.3.0.0", "SS.3.0.0", "U.1.0.0", "V.1.0.0", "X.1.0.0", "Y.3.0.0" };
                var libraries = assetsFile.Libraries.Select(l => $"{l.Name}.{l.Version}").OrderBy(n => n).ToList();
                Assert.Equal(expectedLibraries, libraries);

                var centralFileDependencyGroups = assetsFile
                    .CentralTransitiveDependencyGroups
                    .SelectMany(g => g.TransitiveDependencies.Select(t => $"{g.FrameworkName}_{t.LibraryRange.Name}.{t.LibraryRange.VersionRange.OriginalString}")).ToList();

                var expectedCentralFileDependencyGroups = new List<string>() { $"{framework.DotNetFrameworkName}_P.[3.0.0, )", $"{framework.DotNetFrameworkName}_S.[3.0.0, )" };

                Assert.Equal(expectedCentralFileDependencyGroups, centralFileDependencyGroups);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void RestoreNetCore_WithMultipleFrameworksWithPlatformAndAssetTargetFallback_Succeeds()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net50Windows = "net5.0-windows10.0.10000.1";
                var net50Android = "net5.0-android21";
                var frameworks = new NuGetFramework[]
                {
                    NuGetFramework.Parse(net50Windows),
                    NuGetFramework.Parse(net50Android)
                };

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    net50Windows,
                    net50Android
                    );

                projectA.Properties.Add("AssetTargetFallback", "net472");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                var targets = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Select(e => e);
                targets.Should().HaveCount(2);
                foreach (var framework in frameworks)
                {
                    targets.Select(e => e.TargetFramework).Should().Contain(framework);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task RestoreNetCore_WithCustomAliases_WritesConditionWithCorrectAlias()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var projects = new List<SimpleTestProjectContext>();

                var project = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    "proj",
                    pathContext.SolutionRoot,
                    "net8.0-windows");

                project.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(project);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var r = Util.RestoreSolution(pathContext);
                Assert.Equal(0, r.ExitCode);
                Assert.True(File.Exists(project.PropsOutput), r.Output);
                var propsXML = XDocument.Parse(File.ReadAllText(project.PropsOutput));

                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ItemGroup").ToList();

                Assert.Contains("'$(TargetFramework)' == 'net8.0-windows' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_TransitiveDependenciesAreNotPinned()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var packagesForSource = new List<SimpleTestPackageContext>();
                var packagesForProject = new List<SimpleTestPackageContext>();
                var framework = FrameworkConstants.CommonFrameworks.NetCoreApp20;

                SimpleTestPackageContext createTestPackage(string name, string version, List<SimpleTestPackageContext> source)
                {
                    var result = new SimpleTestPackageContext()
                    {
                        Id = name,
                        Version = version
                    };
                    result.Files.Clear();
                    source.Add(result);
                    return result;
                };

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "projectA",
                   pathContext.SolutionRoot,
                   framework);

                // the package references defined in the project should not have version
                var packageBNoVersion = createTestPackage("B", null, packagesForProject);
                var packageB100 = createTestPackage("B", "1.0.0", packagesForSource);
                var packageC100 = createTestPackage("C", "1.0.0", packagesForSource);
                var packageC200 = createTestPackage("C", "2.0.0", packagesForSource);

                packageB100.Dependencies.Add(packageC100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   packagesForSource.ToArray());

                projectA.AddPackageToAllFrameworks(packagesForProject.ToArray());

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("B", "1.0.0")
                    .SetPackageVersion("C", "2.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));

                var assetFileReader = new LockFileFormat();
                var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);

                var expectedLibraries = new List<string>() { "B.1.0.0", "C.1.0.0" };
                var libraries = assetsFile.Libraries.Select(l => $"{l.Name}.{l.Version}").OrderBy(n => n).ToList();
                Assert.Equal(expectedLibraries, libraries);

                var centralFileDependencyGroups = assetsFile
                    .CentralTransitiveDependencyGroups
                    .SelectMany(g => g.TransitiveDependencies.Select(t => $"{g.FrameworkName}_{t.LibraryRange.Name}.{t.LibraryRange.VersionRange.OriginalString}")).ToList();

                Assert.Equal(0, centralFileDependencyGroups.Count);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [InlineData(null)]
        public async Task RestoreNetCore_CPVMProject_ManagePackageVersionsCentrally_CanBeDisabled(bool? managePackageVersionsCentrally)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            var packageA = new SimpleTestPackageContext("A", "1.0.0");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageA);

            var projectA = SimpleTestProjectContext.CreateNETCore("projectA", pathContext.SolutionRoot, "net472");

            if (managePackageVersionsCentrally is null || managePackageVersionsCentrally == true)
            {
                packageA.Version = null;
            }

            projectA.AddPackageToAllFrameworks(packageA);

            solution.CentralPackageVersionsManagementFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot, managePackageVersionsCentrally)
                .SetPackageVersion("A", "1.0.0");

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            // Act
            var result = Util.RestoreSolution(pathContext);

            // Assert
            result.Success.Should().BeTrue();

            var assetFileReader = new LockFileFormat();
            var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);

            assetsFile.Libraries.Select(l => $"{l.Name}.{l.Version}").Should().BeEquivalentTo(new string[] { "A.1.0.0" });

            var targetFramework = assetsFile.PackageSpec.TargetFrameworks.Should().ContainSingle();

            if (managePackageVersionsCentrally is null || managePackageVersionsCentrally == true)
            {
                targetFramework.Subject.Dependencies.Should().ContainSingle()
                    .Which.VersionCentrallyManaged.Should().BeTrue();

                targetFramework.Subject.CentralPackageVersions.Should().ContainSingle()
                    .Which.Value.Should().Be(new CentralPackageVersion("A", VersionRange.Parse("1.0.0")));
            }
            else
            {
                targetFramework.Subject.Dependencies.Should().ContainSingle()
                    .Which.VersionCentrallyManaged.Should().BeFalse();
                targetFramework.Subject.CentralPackageVersions.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_WithGlobalPackageReferences_Succeeds()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var packagesForSource = new List<SimpleTestPackageContext>();

                SimpleTestProjectContext projectA = SimpleTestProjectContext.CreateNETCore("projectA", pathContext.SolutionRoot, FrameworkConstants.CommonFrameworks.NetCoreApp20);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   new[]
                   {
                       new SimpleTestPackageContext()
                        {
                            Id = "PackageA",
                            Version = "1.0.0"
                        },
                        new SimpleTestPackageContext()
                        {
                            Id = "ToolPackageA",
                            Version = "1.0.0"
                        }
                   });

                projectA.AddPackageToAllFrameworks(new[]
                {
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageA",
                        Version = null,
                    }
                });

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("PackageA", "1.0.0")
                    .SetGlobalPackageReference("ToolPackageA", "1.0.0");
                solution.Create(pathContext.SolutionRoot);

                // Act
                CommandRunnerResult result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));

                LockFile assetsFile = new LockFileFormat().Read(projectA.AssetsFileOutputPath);

                LibraryDependency packageADependency = assetsFile.PackageSpec.TargetFrameworks.SingleOrDefault().Dependencies.Single(i => i.Name == "PackageA");
                LibraryDependency toolPackageADependency = assetsFile.PackageSpec.TargetFrameworks.SingleOrDefault().Dependencies.Single(i => i.Name == "ToolPackageA");

                packageADependency.IncludeType.Should().Be(LibraryIncludeFlags.All);
                packageADependency.SuppressParent.Should().Be(LibraryIncludeFlagUtils.DefaultSuppressParent);

                toolPackageADependency.IncludeType.Should().Be(LibraryIncludeFlags.All & ~LibraryIncludeFlags.Compile & ~LibraryIncludeFlags.BuildTransitive);
                toolPackageADependency.SuppressParent.Should().Be(LibraryIncludeFlags.All);
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_WithGloballPackageReferencesButCentralPackageManagementDisabled_GlobalPackageReferencesAreIgnored()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var packagesForSource = new List<SimpleTestPackageContext>();

                SimpleTestProjectContext projectA = SimpleTestProjectContext.CreateNETCore("projectA", pathContext.SolutionRoot, FrameworkConstants.CommonFrameworks.NetCoreApp20);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   new[]
                   {
                       new SimpleTestPackageContext()
                        {
                            Id = "PackageA",
                            Version = "1.0.0"
                        }
                   });

                projectA.AddPackageToAllFrameworks(new[]
                {
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageA",
                        Version = "1.0.0",
                    }
                });

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot, managePackageVersionsCentrally: false)
                    .SetPackageVersion("PackageA", "1.0.0")
                    .SetGlobalPackageReference("ToolPackageA", "1.0.0");
                solution.Create(pathContext.SolutionRoot);

                // Act
                CommandRunnerResult result = Util.RestoreSolution(pathContext);

                // Assert
                result.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));

                LockFile assetsFile = new LockFileFormat().Read(projectA.AssetsFileOutputPath);

                LibraryDependency packageADependency = assetsFile.PackageSpec.TargetFrameworks.SingleOrDefault().Dependencies.Should().ContainSingle().Subject;

                packageADependency.IncludeType.Should().Be(LibraryIncludeFlags.All);
                packageADependency.SuppressParent.Should().Be(LibraryIncludeFlagUtils.DefaultSuppressParent);
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_WithVersionOverride_Succeeds()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var packagesForSource = new List<SimpleTestPackageContext>();
                var framework = FrameworkConstants.CommonFrameworks.NetCoreApp20;

                var projectA = SimpleTestProjectContext.CreateNETCore("projectA", pathContext.SolutionRoot, framework);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   new[]
                   {
                       new SimpleTestPackageContext()
                    {
                        Id = "PackageA",
                        Version = "1.0.0"
                    },
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageB",
                        Version = "1.0.0"
                    },
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageB",
                        Version = "2.0.0"
                    }
                   });

                projectA.AddPackageToAllFrameworks(new[]
                {
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageA",
                        Version = null,
                    },
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageB",
                        Version = null,
                        VersionOverride = "2.0.0"
                    },
                });

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("A", "1.0.0")
                    .SetPackageVersion("B", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                var r = Util.RestoreSolution(pathContext);

                // Assert
                r.Success.Should().BeTrue();
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));

                var assetFileReader = new LockFileFormat();
                var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);

                var expectedLibraries = new List<string>() { "PackageA.1.0.0", "PackageB.2.0.0" };
                var libraries = assetsFile.Libraries.Select(l => $"{l.Name}.{l.Version}").OrderBy(n => n).ToList();
                Assert.Equal(expectedLibraries, libraries);

                var centralFileDependencyGroups = assetsFile
                    .CentralTransitiveDependencyGroups
                    .SelectMany(g => g.TransitiveDependencies.Select(t => $"{g.FrameworkName}_{t.LibraryRange.Name}.{t.LibraryRange.VersionRange.OriginalString}")).ToList();

                Assert.Equal(0, centralFileDependencyGroups.Count);
            }
        }

        [Fact]
        public async Task RestoreNetCore_CPVMProject_WithVersionOverrideDisabled_Fails()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var packagesForSource = new List<SimpleTestPackageContext>();
                var framework = FrameworkConstants.CommonFrameworks.NetCoreApp20;

                var projectA = SimpleTestProjectContext.CreateNETCore("projectA", pathContext.SolutionRoot, framework);

                projectA.Properties.Add(ProjectBuildProperties.CentralPackageVersionOverrideEnabled, bool.FalseString);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   new[]
                   {
                       new SimpleTestPackageContext()
                    {
                        Id = "PackageA",
                        Version = "1.0.0"
                    },
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageB",
                        Version = "1.0.0"
                    },
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageB",
                        Version = "2.0.0"
                    }
                   });

                projectA.AddPackageToAllFrameworks(new[]
                {
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageA",
                        Version = null,
                    },
                    new SimpleTestPackageContext()
                    {
                        Id = "PackageB",
                        Version = null,
                        VersionOverride = "2.0.0"
                    },
                });

                var cpvmFile = CentralPackageVersionsManagementFile.Create(pathContext.SolutionRoot)
                    .SetPackageVersion("A", "1.0.0")
                    .SetPackageVersion("B", "1.0.0");

                solution.Projects.Add(projectA);
                solution.CentralPackageVersionsManagementFile = cpvmFile;
                solution.Create(pathContext.SolutionRoot);

                // Act
                CommandRunnerResult result = Util.RestoreSolution(pathContext, expectedExitCode: 1);

                // Assert
                result.Success.Should().BeFalse();

                result.Errors.Should().Contain("NU1013");
            }
        }

        [Fact]
        public async Task RestorePackageReference_WithPackagesConfigProjectReference_IncludesTransitivePackageReferenceProjects()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var projectB = SimpleTestProjectContext.CreateNonNuGet(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var packageX100 = new SimpleTestPackageContext("X", "1.0.0");
                var packageX110 = new SimpleTestPackageContext("X", "1.1.0");
                var packageY = new SimpleTestPackageContext("Y", "1.0.0");

                projectA.AddPackageToAllFrameworks(packageX100);
                projectC.AddPackageToAllFrameworks(packageY);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX110,
                    packageY);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);

                // B -> packages.config
                Util.CreateFile(Path.GetDirectoryName(projectB.ProjectPath), "packages.config",
@"<packages>
  <package id=""X"" version=""1.1.0"" targetFramework=""net472"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed",
                    "-Recursive"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert correct projects were restored.
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.False(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectC.AssetsFileOutputPath));

                // Assert transitivity is applied across non PackageReference projects.
                var ridlessTarget = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "project" && e.Name == projectB.ProjectName);
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "project" && e.Name == projectC.ProjectName);
                ridlessTarget.Libraries.Should().Contain(e => e.Name == "X");
                ridlessTarget.Libraries.Should().Contain(e => e.Name == "Y");
            }
        }

        /// <summary>
        /// A -> B (PrivateAssets)-> C
        /// A has packages lock file enabled. Locked should succeed and ignore `C`.
        /// </summary>
        [Fact]
        public void RestoreWithPackagesLockFile_ProjectToProjectWithPrivateAssets_SucceedsInLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                   "c",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                // A -> B
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C with PrivateAssets
                projectC.PrivateAssets = LibraryIncludeFlags.All.ToString();
                projectB.AddProjectToAllFrameworks(projectC);

                // Solution
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Pre-Conditions, Act & Assert.
                Util.RestoreSolution(pathContext).Success.Should().BeTrue();

                // Second Restore
                var r = Util.RestoreSolution(pathContext, additionalArgs: "-LockedMode");

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        /// <summary>
        /// A -> B (PrivateAssets)-> C -> PackageC
        /// A -> D -> C -> PackageC
        /// </summary>
        [Fact]
        public async Task RestoreWithPackagesLockFile_ProjectToProject_MultipleEdgesWithDifferentPrivateAssets_SucceedsInLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                   "c",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectCWithPrivateAssets = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net472"));

                var projectD = SimpleTestProjectContext.CreateNETCore(
                    "d",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("net472"));

                // Enable lock file everywhere:
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                projectB.Properties.Add("RestorePackagesWithLockFile", "true");
                projectC.Properties.Add("RestorePackagesWithLockFile", "true");
                projectD.Properties.Add("RestorePackagesWithLockFile", "true");

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C with PrivateAssets
                projectCWithPrivateAssets.PrivateAssets = LibraryIncludeFlags.All.ToString();
                projectB.AddProjectToAllFrameworks(projectCWithPrivateAssets);

                // A -> D
                projectA.AddProjectToAllFrameworks(projectD);

                // D -> C
                projectD.AddProjectToAllFrameworks(projectC);

                // C - package X
                var packageX = new SimpleTestPackageContext("X", "1.0.0");
                projectC.AddPackageToAllFrameworks(packageX);

                // Create packages
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Solution
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Projects.Add(projectD);
                solution.Create(pathContext.SolutionRoot);

                // Pre-Conditions, Act & Assert.
                Util.RestoreSolution(pathContext).Success.Should().BeTrue();

                // Second Restore
                var r = Util.RestoreSolution(pathContext, additionalArgs: "-LockedMode");

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        /// <summary>
        /// A -> B (PrivateAssets)-> C
        /// A has packages lock file enabled.
        /// A change in C's dependencies should not affect A's locked mode.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RestoreWithPackagesLockFile_ChangesInSuppressedProjects_DoNotAffectLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                   "a",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectB = SimpleTestProjectContext.CreateNETCore(
                   "b",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                   "c",
                   pathContext.SolutionRoot,
                   NuGetFramework.Parse("net472"));

                // Enable lock for A
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C with PrivateAssets
                projectC.PrivateAssets = LibraryIncludeFlags.All.ToString();
                projectB.AddProjectToAllFrameworks(projectC);

                // C - package X
                var packageX = new SimpleTestPackageContext("X", "1.0.0");
                var packageY = new SimpleTestPackageContext("Y", "1.0.0");
                projectC.AddPackageToAllFrameworks(packageX);

                // Create packages
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Solution
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Pre-Conditions, Act & Assert.
                Util.RestoreSolution(pathContext).Success.Should().BeTrue();

                // Set-up again.
                projectC.AddPackageToAllFrameworks(packageY);
                projectC.Save();
                // Second Restore - changes in C should *not* affect A.
                var r = Util.RestoreSolution(pathContext, additionalArgs: "-LockedMode");

                // Assert
                r.Success.Should().BeTrue();
            }
        }

        /// <summary>
        /// A -> B(packages.config) -> C
        /// A -> X
        /// C -> Y
        /// </summary>
        [Fact]
        public async Task RestorePackageReferenceWithLockFile_WithPackagesConfigProjectReference_IncludesTransitivePackageReferenceProjects()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var projectB = SimpleTestProjectContext.CreateNonNuGet(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var packageX100 = new SimpleTestPackageContext("X", "1.0.0");
                var packageX110 = new SimpleTestPackageContext("X", "1.1.0");
                var packageY = new SimpleTestPackageContext("Y", "1.0.0");

                projectA.AddPackageToAllFrameworks(packageX100);
                projectC.AddPackageToAllFrameworks(packageY);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX110,
                    packageY);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);

                // B -> packages.config
                Util.CreateFile(Path.GetDirectoryName(projectB.ProjectPath), "packages.config",
@"<packages>
  <package id=""X"" version=""1.1.0"" targetFramework=""net472"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed",
                    "-Recursive"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Preconditions
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Assert correct projects were restored.
                Assert.True(File.Exists(projectA.AssetsFileOutputPath));
                Assert.False(File.Exists(projectB.AssetsFileOutputPath));
                Assert.True(File.Exists(projectC.AssetsFileOutputPath));

                // Assert transitivity is applied across non PackageReference projects.
                var ridlessTarget = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "project" && e.Name == projectB.ProjectName);
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "project" && e.Name == projectC.ProjectName);
                ridlessTarget.Libraries.Should().Contain(e => e.Name == "X");
                ridlessTarget.Libraries.Should().Contain(e => e.Name == "Y");

                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);

                var lockFileTarget = lockFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                lockFileTarget.Dependencies.Should().HaveCount(4);
                lockFileTarget.Dependencies.Should().ContainSingle(e => e.Id == projectB.ProjectName);
                lockFileTarget.Dependencies.Should().ContainSingle(e => e.Id == projectC.ProjectName);
                var projectDeps = lockFileTarget.Dependencies.Where(e => e.Type == PackageDependencyType.Project).Should().HaveCount(2);
                var projectBlockFileTarget = lockFileTarget.Dependencies.Single(e => e.Id == projectB.ProjectName);
                projectBlockFileTarget.Dependencies.Should().HaveCount(1);
                projectBlockFileTarget.Dependencies.Should().ContainSingle(e => e.Id == projectC.ProjectName);

                // Check locked mode
                var lockedModeArgs = args.Append("-LockedMode").Append("-Force");

                // Act
                r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", lockedModeArgs));

                r.Success.Should().BeTrue(because: r.AllOutput);
            }
        }

        /// <summary>
        /// A -> B(packages.config) -> C
        /// A -> X
        /// C -> Y
        /// Ensure that a change in C, fails locked mode for A.
        /// </summary>
        [Fact]
        public async Task RestorePackageReferenceLockFile_WithPackagesConfigProjectReference_IncludesTransitivePackageReferenceProjects()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var projectB = SimpleTestProjectContext.CreateNonNuGet(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var projectC = SimpleTestProjectContext.CreateNETCore(
                    "c",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                var packageX100 = new SimpleTestPackageContext("X", "1.0.0");
                var packageX110 = new SimpleTestPackageContext("X", "1.1.0");
                var packageY100 = new SimpleTestPackageContext("Y", "1.0.0");
                var packageY110 = new SimpleTestPackageContext("Y", "1.1.0");

                projectA.AddPackageToAllFrameworks(packageX100);
                projectC.AddPackageToAllFrameworks(packageY100);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX110,
                    packageY100,
                    packageY110);

                // A -> B
                projectA.AddProjectToAllFrameworks(projectB);

                // B -> C
                projectB.AddProjectToAllFrameworks(projectC);

                // B -> packages.config
                Util.CreateFile(Path.GetDirectoryName(projectB.ProjectPath), "packages.config",
@"<packages>
  <package id=""X"" version=""1.1.0"" targetFramework=""net472"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Projects.Add(projectC);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[] {
                    "restore",
                    projectA.ProjectPath,
                    "-Verbosity",
                    "detailed",
                    "-Recursive"
                };

                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Pre-Conditions
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Change project C, bump the package Y version.
                projectC.CleanPackagesFromAllFrameworks();
                projectC.AddPackageToAllFrameworks(packageY110);
                projectC.Save();

                // Act
                var lockedModeArgs = args.Append("-LockedMode").Append("-Force");
                r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", lockedModeArgs));

                // Assert
                r.Success.Should().BeFalse(because: r.AllOutput);
                r.AllOutput.Should().Contain("NU1004");
            }
        }

        [Fact]
        public async Task RestoreNetCore_WhenPackageSourceMappingConfiguredInstallsPackageReferencesAndDownloadsFromExpectedSources_Success()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            var projectA = SimpleTestProjectContext.CreateNETCore(
                "a",
                pathContext.SolutionRoot,
                NuGetFramework.Parse("net5.0"));

            const string version = "1.0.0";
            const string packageX = "X", packageY = "Y", packageZ = "Z", packageK = "K";

            var packageX100 = new SimpleTestPackageContext(packageX, version);
            var packageY100 = new SimpleTestPackageContext(packageY, version);
            var packageZ100 = new SimpleTestPackageContext(packageZ, version);
            var packageK100 = new SimpleTestPackageContext(packageK, version);

            packageX100.Dependencies.Add(packageZ100);

            projectA.AddPackageToAllFrameworks(packageX100);
            projectA.AddPackageToAllFrameworks(packageY100);
            projectA.AddPackageDownloadToAllFrameworks(packageK100);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageY100,
                    packageZ100,
                    packageK100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageY100,
                    packageZ100,
                    packageK100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source"">
                <package pattern=""{packageY}*"" />
                <package pattern=""{packageZ}*"" />
            </packageSource>
            <packageSource key=""source2"">
                <package pattern=""{packageX}*"" />
                <package pattern=""{packageK}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";

            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, projectA.ProjectName, "NuGet.Config"), configFile);

            var result = Util.Restore(pathContext, projectA.ProjectPath);

            Assert.True(result.Success);
            Assert.Contains($"Installed {packageX} {version} from {packageSource2.FullName}", result.AllOutput);
            Assert.Contains($"Installed {packageZ} {version} from {pathContext.PackageSource}", result.AllOutput);
            Assert.Contains($"Installed {packageY} {version} from {pathContext.PackageSource}", result.AllOutput);
            Assert.Contains($"Installed {packageK} {version} from {packageSource2.FullName}", result.AllOutput);
        }

        [Fact]
        public async Task RestoreNetCore_WhenPackageSourceMappingConfiguredAndNoMatchingSourceFound_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            var projectA = SimpleTestProjectContext.CreateNETCore(
                "a",
                pathContext.SolutionRoot,
                NuGetFramework.Parse("net5.0"));

            const string version = "1.0.0";
            const string packageX = "X", packageY = "Y", packageZ = "Z";

            var packageX100 = new SimpleTestPackageContext(packageX, version);
            var packageY100 = new SimpleTestPackageContext(packageY, version);
            var packageZ100 = new SimpleTestPackageContext(packageZ, version);

            packageX100.Dependencies.Add(packageZ100);

            projectA.AddPackageToAllFrameworks(packageX100);
            projectA.AddPackageToAllFrameworks(packageY100);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageY100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageY100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source"">
                <package pattern=""{packageY}*"" />
            </packageSource>
            <packageSource key=""source2"">
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";

            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, projectA.ProjectName, "NuGet.Config"), configFile);

            var result = Util.Restore(pathContext, projectA.ProjectPath, expectedExitCode: 1);

            Assert.Contains($"NU1100: Unable to resolve '{packageZ} (>= {version})'", result.AllOutput);
            Assert.Contains($"Installed {packageX} {version} from {packageSource2.FullName}", result.AllOutput);
            Assert.Contains($"Installed {packageY} {version} from {pathContext.PackageSource}", result.AllOutput);
        }

        [Fact]
        public async Task RestoreCommand_PackageSourceMappingFilter_PR_WithAllRestoreSourceProperies_Succeed()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var workingPath = pathContext.WorkingDirectory;
            var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            var privateRepositoryPath = Path.Combine(workingPath, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var net461 = NuGetFramework.Parse("net461");
            var projectA = new SimpleTestProjectContext(
                "a",
                ProjectStyle.PackageReference,
                pathContext.SolutionRoot);
            projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));

            // Add both repositories as RestoreSources
            projectA.Properties.Add("RestoreSources", $"{opensourceRepositoryPath};{privateRepositoryPath}");

            var packageOpenSourceA = new SimpleTestPackageContext("Contoso.Opensource.A", "1.0.0");
            packageOpenSourceA.AddFile("lib/net461/openA.dll");

            var packageOpenSourceContosoMvc = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0"); // Package Id conflict with internally created package
            packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

            var packageContosoMvcReal = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0");
            packageContosoMvcReal.AddFile("lib/net461/realA.dll");

            projectA.AddPackageToAllFrameworks(packageOpenSourceA);
            projectA.AddPackageToAllFrameworks(packageOpenSourceContosoMvc);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
            // so we go ahead and replace that config before running MSBuild.
            var configAPath = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "NuGet.Config");
            var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.Opensource.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.MVC.*"" /> <!--Contoso.MVC.ASP package exist in both repository but it'll restore from this one -->
        </packageSource>
    </packageSourceMapping>
</configuration>";
            using (var writer = new StreamWriter(configAPath))
            {
                writer.Write(configText);
            }

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                opensourceRepositoryPath,
                packageOpenSourceA,
                packageOpenSourceContosoMvc);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                privateRepositoryPath,
                packageContosoMvcReal);

            var packagePath = Path.Combine(pathContext.WorkingDirectory, "packages");

            string[] args = new string[]
                {
                        "-OutputDirectory",
                        "packages"
                };

            // Act
            var r = Util.Restore(pathContext, projectA.ProjectPath, expectedExitCode: 0, args);

            // Assert
            // If we pass source then log include actual path to repository instead of repository name.
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.MVC.ASP' are: 'PrivateRepository'", r.Output);
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.Opensource.A' are: 'PublicRepository'", r.Output);
            var contosoRestorePath = Path.Combine(packagePath, packageContosoMvcReal.Id.ToString(), packageContosoMvcReal.Version.ToString(), packageContosoMvcReal.ToString() + ".nupkg");
            var localResolver = new VersionFolderPathResolver(packagePath);
            var contosoMvcMetadataPath = localResolver.GetNupkgMetadataPath(packageContosoMvcReal.Identity.Id, packageContosoMvcReal.Identity.Version);
            NupkgMetadataFile contosoMvcmetadata = NupkgMetadataFileFormat.Read(contosoMvcMetadataPath);
            Assert.Equal(privateRepositoryPath, contosoMvcmetadata.Source);
        }

        [Fact]
        public async Task RestoreCommand_PackageSourceMappingFilter_PR_WithNotEnoughRestoreSourceProperty_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var workingPath = pathContext.WorkingDirectory;
            var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            var privateRepositoryPath = Path.Combine(workingPath, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var net461 = NuGetFramework.Parse("net461");
            var projectA = new SimpleTestProjectContext(
                "a",
                ProjectStyle.PackageReference,
                pathContext.SolutionRoot);
            projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));

            // Add only 1 repository as RestoreSources
            projectA.Properties.Add("RestoreSources", $"{opensourceRepositoryPath}");

            var packageOpenSourceA = new SimpleTestPackageContext("Contoso.Opensource.A", "1.0.0");
            packageOpenSourceA.AddFile("lib/net461/openA.dll");

            var packageOpenSourceContosoMvc = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0"); // Package Id conflict with internally created package
            packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

            var packageContosoMvcReal = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0");
            packageContosoMvcReal.AddFile("lib/net461/realA.dll");

            projectA.AddPackageToAllFrameworks(packageOpenSourceA);
            projectA.AddPackageToAllFrameworks(packageOpenSourceContosoMvc);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
            // so we go ahead and replace that config before running MSBuild.
            var configAPath = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "NuGet.Config");
            var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.Opensource.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.MVC.*"" /> <!--Contoso.MVC.ASP package exist in both repository but it'll restore from this one -->
        </packageSource>
    </packageSourceMapping>
</configuration>";
            using (var writer = new StreamWriter(configAPath))
            {
                writer.Write(configText);
            }

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                opensourceRepositoryPath,
                packageOpenSourceA,
                packageOpenSourceContosoMvc);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                privateRepositoryPath,
                packageContosoMvcReal);

            string[] args = new string[]
                {
                        "-OutputDirectory",
                        "packages"
                };

            // Act
            var r = Util.Restore(pathContext, projectA.ProjectPath, expectedExitCode: 1, args);

            // Assert
            Assert.Contains("Package source mapping match not found for package ID 'Contoso.MVC.ASP'.", r.Output);
            // Even though there is eligible source SharedRepository exist but only opensourceRepositoryPath passed as option it'll fail to restore.
            Assert.Contains($"Failed to restore {projectA.ProjectPath}", r.Output);
        }

        [Theory]
        [InlineData("PackageReference", "NU1504")]
        [InlineData("PackageDownload", "NU1505")]
        public async Task NuGetExeRestore_WithDuplicatePackageItems_SucceedsAndDoesNotWarn(string item, string warningCode)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETFramework4.7.2"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext("X", "1.0.0")
                    );

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var xml = projectA.GetXML();

                var attributes = new Dictionary<string, string>();
                attributes.Add("Version", "[1.0.0]");
                ProjectFileUtils.AddItem(
                                    xml,
                                    item,
                                    "X",
                                    NuGetFramework.AnyFramework,
                                    new Dictionary<string, string>(),
                                    attributes);

                attributes.Clear();
                attributes.Add("Version", "[2.0.0]");
                ProjectFileUtils.AddItem(
                                    xml,
                                    item,
                                    "X",
                                    NuGetFramework.AnyFramework,
                                    new Dictionary<string, string>(),
                                    attributes);
                xml.Save(projectA.ProjectPath);

                var args = new string[] {
                    "restore",
                    solution.SolutionPath,
                    "-Verbosity",
                    "detailed",
                };

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                r.Success.Should().BeTrue(because: r.AllOutput);
                r.AllOutput.Should().NotContain(warningCode);
            }
        }

        [Fact]
        public async Task NuGetExeRestore_WithDuplicatePackageVersion_SucceedsAndDoesNotWarn()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    "a",
                    pathContext.SolutionRoot,
                    "net472");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext("X", "1.0.0")
                    );

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var xml = projectA.GetXML();

                ProjectFileUtils.AddItem(
                                    xml,
                                    "PackageReference",
                                    "X",
                                    NuGetFramework.AnyFramework,
                                    new Dictionary<string, string>(),
                                    new Dictionary<string, string>());
                xml.Save(projectA.ProjectPath);

                var directoryPackagesPropsContent =
                   @"<Project>
                        <ItemGroup>
                            <PackageVersion Include=""X"" Version=""[1.0.0]"" />
                            <PackageVersion Include=""X"" Version=""[2.0.0]"" />
                        </ItemGroup>
                    </Project>";
                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, $"Directory.Packages.Props"), directoryPackagesPropsContent);

                var args = new string[] {
                    "restore",
                    solution.SolutionPath,
                    "-Verbosity",
                    "detailed",
                };

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

                // Assert
                r.Success.Should().BeTrue(because: r.AllOutput);
                r.AllOutput.Should().NotContain("NU1506");
            }
        }

        [Fact]
        public async Task NuGetRestoreRestore_WithWarningsNotAsErrors_SucceedsAndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    "a",
                    pathContext.SolutionRoot,
                    "net472");
            // Add 1.0.0
            projectA.AddPackageToAllFrameworks(new SimpleTestPackageContext()
            {
                Id = "x",
                Version = "1.0.0"
            });
            // But create only 2.0.0 on the server.
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new SimpleTestPackageContext { Id = "x", Version = "2.0.0" });
            projectA.Properties.Add("TreatWarningsAsErrors", "true");
            projectA.Properties.Add("WarningsNotAsErrors", "NU1603");
            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var args = new string[] {
                    "restore",
                    solution.SolutionPath,
                    "-Verbosity",
                    "detailed",
                };
            CommandRunnerResult result = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args));

            // Assert
            result.Success.Should().BeTrue(because: result.AllOutput);
            result.Output.Should().Contain("WARNING: NU1603");
        }

        [Fact]
        public async Task NuGetRestore_WithMacrosEnabled_WritesAssetsFileWithMacros()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    "a",
                    pathContext.SolutionRoot,
                    "net472");
            var packageX = new SimpleTestPackageContext()
            {
                Id = "x",
                Version = "1.0.0"
            };

            projectA.AddPackageToAllFrameworks(packageX);
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageX);
            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var args = new string[] {
                    "restore",
                    solution.SolutionPath,
                    "-Verbosity",
                    "detailed",
                };
            CommandRunnerResult result = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    pathContext.WorkingDirectory.Path,
                    string.Join(" ", args),
                    environmentVariables:
                    new Dictionary<string, string>()
                    {
                        { "NUGET_ENABLE_EXPERIMENTAL_MACROS", "true" }
                    });

            // Assert
            result.Success.Should().BeTrue(because: result.AllOutput);

            projectA.AssetsFile.PackageFolders.Should().HaveCountGreaterThan(0);
            var globalPackagesFolder = projectA.AssetsFile.PackageFolders[0]; // the package folders are always in priority and gpf is always first.

            JObject assetsFileJson = JObject.Parse(File.ReadAllText(projectA.AssetsFileOutputPath));
            var configList = assetsFileJson["project"]["restore"]["configFilePaths"].ToArray();
            var configFilePaths = configList.Select(e => e.Value<string>()).ToList();


            configFilePaths.Should().Contain(("$(User)" + Path.DirectorySeparatorChar + "NuGet.Config"));
        }

        /// <summary>
        /// A 1.0 -> D 1.0 (Central transitive)
        ///       -> B 1.0 -> D 3.0 (Central transitive - should be ignored because it is not at root)
        ///                -> C 1.0 -> D 2.0
        /// </summary>
        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public async Task RestoreNetCore_TransitiveDependenciesFromNonRootLibraries_AreIgnored(bool centralPackageTransitivePinningEnabled, bool expectedSuccess)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            var projectC = CreateProject(pathContext, "C");
            var projectB = CreateProject(pathContext, "B", projectC);
            var projectA = CreateProject(pathContext, "A", projectB);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("D", "1.0.0"),
                new SimpleTestPackageContext("D", "2.0.0"),
                new SimpleTestPackageContext("D", "3.0.0")
            );

            solution.Projects.Add(projectA);
            solution.Projects.Add(projectB);
            solution.Projects.Add(projectC);
            solution.Create(pathContext.SolutionRoot);

            AddPackageReferenceToProject(projectC);

            CreateDirectoryPackagesPropsWithVersionForPackageD(pathContext, projectA, "1.0.0");
            CreateDirectoryPackagesPropsWithVersionForPackageD(pathContext, projectB, "3.0.0");
            CreateDirectoryPackagesPropsWithVersionForPackageD(pathContext, projectC, "2.0.0");

            var args = new string[] {
                    "restore",
                    solution.SolutionPath,
                    "-Verbosity",
                    "detailed",
                };

            // Act
            var r = CommandRunner.Run(
                Util.GetNuGetExePath(),
                pathContext.WorkingDirectory.Path,
                string.Join(" ", args));

            // Assert
            r.Success.Should().Be(expectedSuccess, because: r.AllOutput);

            if (expectedSuccess == false)
            {
                r.Errors.Should().Contain(
                        "NU1109: Detected package downgrade: D from 2.0.0 to centrally defined 1.0.0. Update the centrally managed package version to a higher version.");
                r.Errors.Should().Contain("A -> B -> C -> D (>= 2.0.0)");
                r.Errors.Should().Contain("A -> D (>= 1.0.0)");
            }

            // Local methods
            void CreateDirectoryPackagesPropsWithVersionForPackageD(SimpleTestPathContext pathContext, SimpleTestProjectContext projectContext, string version)
            {
                var directoryPackagesPropsContent =
                    @$"<Project>
                            <ItemGroup>
                                <PackageVersion Include=""D"" Version=""{version}"" />
                            </ItemGroup>
                        </Project>";
                var directoryName = Path.GetDirectoryName(projectContext.ProjectPath);
                File.WriteAllText(Path.Combine(directoryName, $"Directory.Packages.Props"), directoryPackagesPropsContent);
            }

            SimpleTestProjectContext CreateProject(SimpleTestPathContext pathContext, string name, SimpleTestProjectContext referencedProject = null)
            {
                var projectContext = SimpleTestProjectContext.CreateNETCoreWithSDK(
                    name,
                    pathContext.SolutionRoot,
                    "net472");

                projectContext.Properties.Add("CentralPackageTransitivePinningEnabled", centralPackageTransitivePinningEnabled.ToString());

                if (referencedProject != null)
                    projectContext.AddProjectToAllFrameworks(referencedProject);

                return projectContext;
            }

            void AddPackageReferenceToProject(SimpleTestProjectContext project)
            {
                var xml = project.GetXML();

                ProjectFileUtils.AddItem(
                    xml,
                    "PackageReference",
                    "D",
                    NuGetFramework.AnyFramework,
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>());

                xml.Save(project.ProjectPath);
            }
        }

        private static byte[] GetTestUtilityResource(string name)
        {
            return ResourceTestUtility.GetResourceBytes(
                $"Test.Utility.compiler.resources.{name}",
                typeof(ResourceTestUtility));
        }
    }
}
