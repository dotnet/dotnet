// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Locator;
using NuGet.CommandLine.XPlat;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using Project = Microsoft.Build.Evaluation.Project;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class MSBuildAPIUtilityTests
    {
        static MSBuildAPIUtilityTests()
        {
            MSBuildLocator.RegisterDefaults();
        }
        [PlatformFact(Platform.Windows)]
        public void GetDirectoryBuildPropsRootElementWhenItExists_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();

            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            var propsFile =
@$"<Project>
    <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "Directory.Packages.props"), propsFile);

            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
	<TargetFramework>net6.0</TargetFramework>
	</PropertyGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);

            // Act
            var result = new MSBuildAPIUtility(logger: new TestLogger()).GetDirectoryBuildPropsRootElement(project);

            // Assert
            Assert.Equal(Path.Combine(testDirectory, "Directory.Packages.props"), result.FullPath);
        }

        [PlatformFact(Platform.Windows)]
        public void AddPackageReferenceIntoProjectFileWhenItemGroupDoesNotExist_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            // Arrange project file
            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>
<TargetFramework>net6.0</TargetFramework>
</PropertyGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);

            var msObject = new MSBuildAPIUtility(logger: new TestLogger());
            // Creating an item group in the project
            var itemGroup = msObject.CreateItemGroup(project, null);

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                        name: "X",
                        versionRange: VersionRange.Parse("1.0.0"),
                        typeConstraint: LibraryDependencyTarget.Package)
            };

            // Act
            msObject.AddPackageReferenceIntoItemGroupCPM(project, itemGroup, libraryDependency);
            project.Save();

            // Assert
            string updatedProjectFile = File.ReadAllText(Path.Combine(testDirectory, "projectA.csproj"));
            Assert.Contains(@$"<PackageReference Include=""X"" />", updatedProjectFile);
            Assert.DoesNotContain(@$"<Version = ""1.0.0"" />", updatedProjectFile);
        }

        [PlatformFact(Platform.Windows)]
        public void AddPackageReferenceIntoProjectFileWhenItemGroupDoesExist_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            // Arrange project file
            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>
<TargetFramework>net6.0</TargetFramework>
</PropertyGroup>
<ItemGroup>
<PackageReference Include=""Y"" />
</ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);
            var logger = new TestLogger();
            var msObject = new MSBuildAPIUtility(logger: logger);
            // Getting all the item groups in a given project
            var itemGroups = msObject.GetItemGroups(project);
            // Getting an existing item group that has package reference(s)
            var itemGroup = msObject.GetItemGroup(itemGroups, "PackageReference");

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                        name: "X",
                        versionRange: VersionRange.Parse("1.0.0"),
                        typeConstraint: LibraryDependencyTarget.Package)
            };

            // Act
            msObject.AddPackageReferenceIntoItemGroupCPM(project, itemGroup, libraryDependency);
            project.Save();

            // Assert
            string updatedProjectFile = File.ReadAllText(Path.Combine(testDirectory, "projectA.csproj"));
            Assert.Contains(@$"<PackageReference Include=""X"" />", updatedProjectFile);
            Assert.DoesNotContain(@$"<Version = ""1.0.0"" />", updatedProjectFile);
            Assert.Contains(string.Format(Strings.Info_AddPkgCPM, "X", project.FullPath, project.GetPropertyValue("DirectoryPackagesPropsPath")), logger.InformationMessages);
        }

        [PlatformFact(Platform.Windows)]
        public void AddPackageVersionIntoPropsFileWhenItemGroupDoesNotExist_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            // Arrange Directory.Packages.props file
            var propsFile =
@$"<Project>
    <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "Directory.Packages.props"), propsFile);

            // Arrange project file
            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
	<TargetFramework>net6.0</TargetFramework>
	</PropertyGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);

            // Add item group to Directory.Packages.props
            var msObject = new MSBuildAPIUtility(logger: new TestLogger());
            var directoryBuildPropsRootElement = msObject.GetDirectoryBuildPropsRootElement(project);
            var propsItemGroup = directoryBuildPropsRootElement.AddItemGroup();

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                        name: "X",
                        versionRange: VersionRange.Parse("1.0.0"),
                        typeConstraint: LibraryDependencyTarget.Package)
            };

            // Act
            msObject.AddPackageVersionIntoPropsItemGroup(propsItemGroup, libraryDependency);
            // Save the updated props file.
            directoryBuildPropsRootElement.Save();

            // Assert
            Assert.Contains(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""1.0.0"" />
  </ItemGroup>", File.ReadAllText(Path.Combine(testDirectory, "Directory.Packages.props")));
        }

        [PlatformFact(Platform.Windows)]
        public void AddPackageVersionIntoPropsFileWhenItemGroupExists_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            // Arrange Directory.Packages.props file
            var propsFile =
@$"<Project>
    <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
    <ItemGroup>
    <PackageVersion Include=""X"" Version=""1.0.0"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "Directory.Packages.props"), propsFile);

            // Arrange project file
            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
	<TargetFramework>net6.0</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
    <PackageReference Include=""X"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);

            // Get existing item group from Directory.Packages.props
            var msObject = new MSBuildAPIUtility(logger: new TestLogger());
            var directoryBuildPropsRootElement = msObject.GetDirectoryBuildPropsRootElement(project);
            var propsItemGroup = msObject.GetItemGroup(directoryBuildPropsRootElement.ItemGroups, "PackageVersion");

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                        name: "Y",
                        versionRange: VersionRange.Parse("1.0.0"),
                        typeConstraint: LibraryDependencyTarget.Package)
            };

            // Act
            msObject.AddPackageVersionIntoPropsItemGroup(propsItemGroup, libraryDependency);
            // Save the updated props file
            directoryBuildPropsRootElement.Save();

            // Assert
            Assert.Contains(@$"<PackageVersion Include=""Y"" Version=""1.0.0"" />", File.ReadAllText(Path.Combine(testDirectory, "Directory.Packages.props")));
        }

        [PlatformFact(Platform.Windows)]
        public void UpdatePackageVersionInPropsFileWhenItExists_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            // Arrange Directory.Packages.props file
            var propsFile =
@$"<Project>
    <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
    <ItemGroup>
    <PackageVersion Include=""X"" Version=""1.0.0"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "Directory.Packages.props"), propsFile);

            // Arrange project file
            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
	<TargetFramework>net6.0</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
    <PackageReference Include=""X"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);

            var msObject = new MSBuildAPIUtility(logger: new TestLogger());
            // Get package version if it already exists in the props file. Returns null if there is no matching package version.
            ProjectItem packageVersionInProps = project.Items.LastOrDefault(i => i.ItemType == "PackageVersion" && i.EvaluatedInclude.Equals("X"));

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                        name: "X",
                        versionRange: VersionRange.Parse("2.0.0"),
                        typeConstraint: LibraryDependencyTarget.Package)
            };

            // Act
            msObject.UpdatePackageVersion(project, packageVersionInProps, "2.0.0");

            // Assert
            Assert.Equal(projectContent, File.ReadAllText(Path.Combine(testDirectory, "projectA.csproj")));
            string updatedPropsFile = File.ReadAllText(Path.Combine(testDirectory, "Directory.Packages.props"));
            Assert.Contains(@$"<PackageVersion Include=""X"" Version=""2.0.0"" />", updatedPropsFile);
            Assert.DoesNotContain(@$"<PackageVersion Include=""X"" Version=""1.0.0"" />", updatedPropsFile);
        }

        [PlatformFact(Platform.Windows)]
        public void UpdateVersionOverrideInPropsFileWhenItExists_Success()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();
            var projectCollection = new ProjectCollection(
                            globalProperties: null,
                            remoteLoggers: null,
                            loggers: null,
                            toolsetDefinitionLocations: ToolsetDefinitionLocations.Default,
                            // Having more than 1 node spins up multiple msbuild.exe instances to run builds in parallel
                            // However, these targets complete so quickly that the added overhead makes it take longer
                            maxNodeCount: 1,
                            onlyLogCriticalEvents: false,
                            loadProjectsReadOnly: false);

            var projectOptions = new ProjectOptions
            {
                LoadSettings = ProjectLoadSettings.DoNotEvaluateElementsWithFalseCondition,
                ProjectCollection = projectCollection
            };

            // Arrange Directory.Packages.props file
            var propsFile =
@$"<Project>
    <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
    <ItemGroup>
    <PackageVersion Include=""X"" Version=""1.0.0"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "Directory.Packages.props"), propsFile);

            // Arrange project file
            string projectContent =
@$"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""X"" VersionOverride=""3.0.0"" />
  </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testDirectory, "projectA.csproj"), projectContent);
            var project = Project.FromFile(Path.Combine(testDirectory, "projectA.csproj"), projectOptions);

            var msObject = new MSBuildAPIUtility(logger: new TestLogger());
            // Get package version if it already exists in the props file. Returns null if there is no matching package version.
            ProjectItem packageVersionInProps = project.Items.LastOrDefault(i => i.ItemType == "PackageReference" && i.EvaluatedInclude.Equals("X"));

            var libraryDependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                        name: "X",
                        versionRange: VersionRange.Parse("3.0.0"),
                        typeConstraint: LibraryDependencyTarget.Package)
            };

            // Act
            msObject.UpdateVersionOverride(project, packageVersionInProps, "3.0.0");

            // Assert
            Assert.Equal(projectContent, File.ReadAllText(Path.Combine(testDirectory, "projectA.csproj")));
            string updatedPropsFile = File.ReadAllText(Path.Combine(testDirectory, "Directory.Packages.props"));
            Assert.Contains(@$"<PackageVersion Include=""X"" Version=""1.0.0"" />", updatedPropsFile);
            Assert.DoesNotContain(@$"<PackageVersion Include=""X"" VersionOverride=""3.0.0"" />", updatedPropsFile);
        }

        [Fact]
        public async Task GetListOfProjectsFromPathArgument_WithProjectFile_ReturnsCorrectPaths()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var net8 = NuGetFramework.Parse("net8.0");

            var projectA = SimpleTestProjectContext.CreateNETCore("a", pathContext.SolutionRoot, net8);

            projectA.Save();

            // Act
            var projectList = await MSBuildAPIUtility.GetListOfProjectsFromPathArgumentAsync(projectA.ProjectPath);

            // Assert
            Assert.Equal(projectList.Count(), 1);
            Assert.Contains(projectA.ProjectPath, projectList);
        }

        [Fact]
        public async Task GetListOfProjectsFromPathArgument_WithProjectDirectory_ReturnsCorrectPaths()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var net8 = NuGetFramework.Parse("net8.0");

            var projectA = SimpleTestProjectContext.CreateNETCore("a", pathContext.SolutionRoot, net8);

            projectA.Save();

            // Act
            var projectList = await MSBuildAPIUtility.GetListOfProjectsFromPathArgumentAsync(Path.GetDirectoryName(projectA.ProjectPath));

            // Assert
            Assert.Equal(projectList.Count(), 1);
            Assert.Contains(projectA.ProjectPath, projectList);
        }

        [Fact]
        public async Task GetListOfProjectsFromPathArgument_WithSolutionFile_ReturnsCorrectPaths()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var net8 = NuGetFramework.Parse("net8.0");

            var projectA = SimpleTestProjectContext.CreateNETCore("a", pathContext.SolutionRoot, net8);
            var projectB = SimpleTestProjectContext.CreateNETCore("b", pathContext.SolutionRoot, net8);

            solution.Projects.Add(projectA);
            solution.Projects.Add(projectB);
            solution.Create(pathContext.SolutionRoot);

            // Act
            var projectList = await MSBuildAPIUtility.GetListOfProjectsFromPathArgumentAsync(Path.GetDirectoryName(solution.SolutionPath));

            // Assert
            Assert.Equal(projectList.Count(), 2);
            Assert.Contains(projectA.ProjectPath, projectList);
            Assert.Contains(projectB.ProjectPath, projectList);
        }

        [Fact]
        public async Task GetListOfProjectsFromPathArgument_WithSolutionDirectory_ReturnsCorrectPaths()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var net8 = NuGetFramework.Parse("net8.0");

            var projectA = SimpleTestProjectContext.CreateNETCore("a", pathContext.SolutionRoot, net8);
            var projectB = SimpleTestProjectContext.CreateNETCore("b", pathContext.SolutionRoot, net8);

            solution.Projects.Add(projectA);
            solution.Projects.Add(projectB);
            solution.Create(pathContext.SolutionRoot);

            // Act
            var projectList = await MSBuildAPIUtility.GetListOfProjectsFromPathArgumentAsync(pathContext.SolutionRoot);

            // Assert
            Assert.Equal(projectList.Count(), 2);
            Assert.Contains(projectA.ProjectPath, projectList);
            Assert.Contains(projectB.ProjectPath, projectList);
        }

        [Theory]
        [InlineData("X.sln", "Y.sln")]
        [InlineData("A.csproj", "B.csproj")]
        [InlineData("X.sln", "A.csproj")]
        [InlineData()]
        [InlineData("random.txt")]
        public async Task GetListOfProjectsFromPathArgument_WithDirectoryWithInvalidNumberOfSolutionsOrProjects_ThrowsException(params string[] directoryFiles)
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            foreach (var filename in directoryFiles)
            {
                var filePath = Path.Combine(pathContext.SolutionRoot, filename);
                File.Create(filePath);
            }

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => MSBuildAPIUtility.GetListOfProjectsFromPathArgumentAsync(pathContext.SolutionRoot));
        }

        [Fact]
        public void GetResolvedVersions_WithAPackageInDirectoryBuildProps_GetsVersion()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var net8 = NuGetFramework.Parse("net8.0");
            var projectA = SimpleTestProjectContext.CreateNETCore("a", pathContext.SolutionRoot, net8);
            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var projectOptions = new ProjectOptions();

            var BuildPropsFile =
@$"<Project>
    <ItemGroup>
    <GlobalPackageReference Include=""myPackage"" Version=""1.1.1"" />
  </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Build.props"), BuildPropsFile);

            File.WriteAllText(
                Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"),
                @$"<Project>
    <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
</Project>");

            var project = Project.FromFile(projectA.ProjectPath, projectOptions);
            var lockFile = new LockFile
            {
                Version = 3,
                Targets = new List<LockFileTarget>
                {
                    new LockFileTarget()
                    {
                        TargetFramework = net8,
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "myPackage",
                                Version = new NuGetVersion("1.2.3")
                            }
                        }
                    }
                },

                PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = net8,
                        Dependencies =
                        [
                            new LibraryDependency
                            {
                                LibraryRange = new LibraryRange("myPackage")
                            }
                        ]
                    }
                })
                {
                    Version = new NuGetVersion("1.0.0"),
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        CentralPackageVersionsEnabled = true
                    }
                }
            };

            // Act
            var result = MSBuildAPIUtility.GetResolvedVersions(project: project, new List<string>(), lockFile, false);

            // Assert
            var version = result.First().TopLevelPackages.First().OriginalRequestedVersion;
            Assert.Equal("1.1.1", version);
        }

        [Fact]
        public void GetResolvedVersions_WithAPackageInDirectoryPackageProps_GetsVersion()
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var net8 = NuGetFramework.Parse("net8.0");
            var projectA = SimpleTestProjectContext.CreateNETCore("a", pathContext.SolutionRoot, net8);
            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var projectOptions = new ProjectOptions();

            var PackagePropsFile =
@$"<Project>
    <PropertyGroup>
      <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    </PropertyGroup>
    <ItemGroup>
    <GlobalPackageReference Include=""myPackage"" Version=""1.1.1"" />
  </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), PackagePropsFile);

            var project = Project.FromFile(projectA.ProjectPath, projectOptions);
            var lockFile = new LockFile
            {
                Version = 3,
                Targets = new List<LockFileTarget>
                {
                    new LockFileTarget()
                    {
                        TargetFramework = net8,
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "myPackage",
                                Version = new NuGetVersion("1.2.3")
                            }
                        }
                    }
                },

                PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = net8,
                        Dependencies =
                        [
                            new LibraryDependency
                            {
                                LibraryRange = new LibraryRange("myPackage")
                            }
                        ]
                    }
                })
                {
                    Version = new NuGetVersion("1.0.0"),
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        CentralPackageVersionsEnabled = true
                    }
                }
            };

            // Act
            var result = MSBuildAPIUtility.GetResolvedVersions(project: project, new List<string>(), lockFile, false);

            // Assert
            var version = result.First().TopLevelPackages.First().OriginalRequestedVersion;
            Assert.Equal("1.1.1", version);
        }
    }
}
