// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Versioning;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.Commands.Test
{
    public class SpecValidationUtilityTests
    {
        [Fact]
        public void SpecValidationUtility_VerifyNoRestoreInputsFails()
        {
            // Arrange
            var spec = new DependencyGraphSpec();

            // Act && Assert
            AssertError(spec, "Restore request does not contain any projects to restore.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyMissingProjectFails()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            // Act && Assert
            AssertError(spec, "Missing project 'a'.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyMissingRestoreMetadataFails()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'RestoreMetadata'.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyFrameworks_Unsupported()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("unsupported")
            };
            var info = new[] { targetFramework };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "Invalid target framework");
        }

        [Fact]
        public void SpecValidationUtility_VerifyFrameworks_Zero()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var project = new PackageSpec(new List<TargetFrameworkInformation>());
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "No target frameworks specified");
        }

        [Fact]
        public void SpecValidationUtility_VerifyFrameworks_Duplicates()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var targetFramework2 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var info = new[] { targetFramework1, targetFramework2 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "Duplicate frameworks found");
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectReferences_TopLevel_Pass()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45")));

            spec.Projects.First().RestoreMetadata.TargetFrameworks.Single()
                .ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = "b.csproj",
                    ProjectUniqueName = "b"
                });

            spec.Projects.First().Dependencies.Add(new LibraryDependency()
            {
                LibraryRange = new LibraryRange("b", LibraryDependencyTarget.PackageProjectExternal)
            });

            // Act && Assert no errors
            SpecValidationUtility.ValidateDependencySpec(spec);
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectReferences_TFMLevel_Pass()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45")));

            spec.Projects.First().RestoreMetadata.TargetFrameworks.Single()
                .ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = "b.csproj",
                    ProjectUniqueName = "b"
                });

            spec.Projects.First().TargetFrameworks.First().Dependencies.Add(new LibraryDependency()
            {
                LibraryRange = new LibraryRange("b", LibraryDependencyTarget.PackageProjectExternal)
            });

            // Act && Assert no errors
            SpecValidationUtility.ValidateDependencySpec(spec);
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectMetadata_MissingProjectName()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.ProjectName = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'ProjectName'.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectMetadata_MissingProjectPath()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.ProjectPath = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'ProjectPath'.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectMetadata_MissingProjectUniqueName()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.ProjectUniqueName = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'ProjectUniqueName'.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectMetadata_MissingSpecName()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().Name = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'Name'.");
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectMetadata_MissingSpecFilePath()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().FilePath = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'FilePath'");
        }

        [Fact]
        public void SpecValidationUtility_VerifyProjectMetadata_SameNameAsSpecName()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var info = new[] { targetFramework1 };

            var project = new PackageSpec(info)
            {
                RestoreMetadata = new ProjectRestoreMetadata(),
                Name = "a",
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "project.json")
            };

            project.RestoreMetadata.ProjectUniqueName = "some_other_path";
            project.RestoreMetadata.ProjectName = "some_other_name";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.Unknown;

            targetFramework1.Dependencies.Add(new LibraryDependency()
            {
                LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.PackageProjectExternal)
            });

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, $"Properties '{nameof(project.Name)}':'{project.Name}' and '{nameof(project.RestoreMetadata.ProjectName)}':'{project.RestoreMetadata.ProjectName}' do not match.");
        }

        [Fact]
        public void SpecValidationUtility_UAP_MultipleTFMs()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var targetFramework2 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net46")
            };

            var info = new[] { targetFramework1, targetFramework2 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "UAP projects must contain exactly one target framework");
        }

        [Fact]
        public void SpecValidationUtility_UAP_VerifyOutputPath()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45"),
            };

            var info = new[] { targetFramework1 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.OutputPath = Directory.GetCurrentDirectory();
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            project.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            spec.AddProject(project);
            // Now the output path is read in for project json projects as well
            SpecValidationUtility.ValidateDependencySpec(spec);
        }

        [Fact]
        public void SpecValidationUtility_UAP_VerifyProjectJsonPath()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var info = new[] { targetFramework1 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.OutputPath = Directory.GetCurrentDirectory();
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            project.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            spec.AddProject(project);

            SpecValidationUtility.ValidateDependencySpec(spec);

        }

        [Fact]
        public void SpecValidationUtility_UnknownType_DisallowProjectJson()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var info = new[] { targetFramework1 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.Unknown;

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "Property 'ProjectJsonPath' is not allowed.");
        }

        [Fact]
        public void SpecValidationUtility_UnknownType_DisallowDependencies()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45")
            };

            var info = new[] { targetFramework1 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.Unknown;

            targetFramework1.Dependencies.Add(new LibraryDependency()
            {
                LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.PackageProjectExternal)
            });

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "Property 'Dependencies' is not allowed");
        }

        [Fact]
        public void SpecValidationUtility_NetCore_VerifyOutputPath()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.OutputPath = null;

            // Act && Assert
            AssertError(spec, "Missing required property 'OutputPath' for project type 'PackageReference'.", "a.csproj");
        }

        [Fact]
        public void SpecValidationUtility_NetCore_VerifyOriginalFrameworks()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.OriginalTargetFrameworks.Clear();

            // Act && Assert
            AssertError(spec, "Missing required property 'OriginalTargetFrameworks' for project type 'PackageReference'.", "a.csproj");
        }

        [Fact]
        public void SpecValidationUtility_NetCore_NoProjectJsonPath()
        {
            // Arrange
            var spec = GetBasicDG();
            spec.Projects.First().RestoreMetadata.ProjectJsonPath = "project.json";

            // Act && Assert
            AssertError(spec, "Property 'ProjectJsonPath' is not allowed for project type 'PackageReference'.", "a.csproj");
        }

        [Fact]
        public void SpecValidationUtility_VerifyFrameworks_WithSameBase_DifferentAssetTargetFallback_Duplicates()
        {
            // Arrange
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");

            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = new AssetTargetFallbackFramework(CommonFrameworks.Net50, new List<NuGetFramework>() { CommonFrameworks.Net463 })
            };

            var targetFramework2 = new TargetFrameworkInformation()
            {
                FrameworkName = new AssetTargetFallbackFramework(CommonFrameworks.Net50, new List<NuGetFramework>() { CommonFrameworks.Net462 })
            };

            var info = new[] { targetFramework1, targetFramework2 };

            var project = new PackageSpec(info);
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            project.RestoreMetadata.ProjectJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "project.json");

            spec.AddProject(project);

            // Act && Assert
            AssertError(spec, "Duplicate frameworks found");
        }

        private static PackageSpec GetProjectA()
        {
            var targetFramework1 = new TargetFrameworkInformation()
            {
                FrameworkName = NuGetFramework.Parse("net45"),
                TargetAlias = "net45",
            };

            var info = new[] { targetFramework1 };

            var project = new PackageSpec(info);
            project.Name = "a";
            project.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata = new ProjectRestoreMetadata();
            project.RestoreMetadata.ProjectName = "a";
            project.RestoreMetadata.ProjectUniqueName = "a";
            project.RestoreMetadata.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "a.csproj");
            project.RestoreMetadata.OutputPath = Directory.GetCurrentDirectory();
            project.RestoreMetadata.ProjectStyle = ProjectStyle.PackageReference;
            project.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            return project;
        }

        private static DependencyGraphSpec GetBasicDG()
        {
            var spec = new DependencyGraphSpec();
            spec.AddRestore("a");
            spec.AddProject(GetProjectA());

            return spec;
        }

        private static void AssertError(DependencyGraphSpec spec, params string[] contains)
        {
            RestoreSpecException specEx = null;

            try
            {
                SpecValidationUtility.ValidateDependencySpec(spec);
            }
            catch (RestoreSpecException ex)
            {
                specEx = ex;
            }

            Assert.NotNull(specEx);

            foreach (var s in contains)
            {
                Assert.Contains(s, specEx.Message);
            }
        }
    }
}
