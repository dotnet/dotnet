// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackageSpecOperationsTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithNonPackageReferenceProjectStyle_AddsNewPackageDependencyToGenericDependencies(bool usePackageDependency)
        {
            // Arrange
            var spec = new PackageSpec(new[]
            {
                new TargetFrameworkInformation
                {
                    FrameworkName = FrameworkConstants.CommonFrameworks.Net45
                }
            });
            var identity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("1.0.0"));
            var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));

            // Act
            if (usePackageDependency)
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, packageDependency);
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, identity);
            }

            // Assert
            Assert.Equal(1, spec.Dependencies.Count);
            Assert.Empty(spec.TargetFrameworks[0].Dependencies);
            Assert.Equal(identity.Id, spec.Dependencies[0].LibraryRange.Name);
            Assert.Equal(identity.Version, spec.Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithPackageReferenceProjectStyle_AddsNewPackageDependencyToAllFrameworksIndividually(bool usePackageDependency)
        {
            // Arrange
            var spec = new PackageSpec(new[]
            {
                new TargetFrameworkInformation
                {
                    FrameworkName = FrameworkConstants.CommonFrameworks.Net45
                }
            })
            {
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectStyle = ProjectStyle.PackageReference
                }
            };

            var identity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("1.0.0"));
            var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));

            // Act
            if (usePackageDependency)
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, packageDependency);
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, identity);
            }

            // Assert
            Assert.Empty(spec.Dependencies);
            Assert.Equal(1, spec.TargetFrameworks[0].Dependencies.Count);
            Assert.Equal(identity.Id, spec.TargetFrameworks[0].Dependencies[0].LibraryRange.Name);
            Assert.Equal(identity.Version, spec.TargetFrameworks[0].Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithExistingGenericPackageDependencies_UpdatesOnlyFrameworksWherePackageExists(bool usePackageDependency)
        {
            var dependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange("NuGet.Versioning", new VersionRange(new NuGetVersion("1.0.0")), LibraryDependencyTarget.Package),
            };

            // Arrange
            var spec = new PackageSpec(new[]
            {
                new TargetFrameworkInformation
                {
                    FrameworkName = FrameworkConstants.CommonFrameworks.Net45
                }
            });
            spec.Dependencies.Add(dependency);
            var identity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("2.0.0"));
            var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));

            // Act
            if (usePackageDependency)
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, packageDependency);
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, identity);
            }

            // Assert
            spec.Dependencies.Should().HaveCount(1);
            spec.TargetFrameworks[0].Dependencies.Should().BeEmpty();
            Assert.Equal(identity.Id, spec.Dependencies[0].LibraryRange.Name);
            Assert.Equal(identity.Version, spec.Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithExistingPackageDependencies_UpdatesOnlyFrameworksWherePackageDependencyExists(bool usePackageDependency)
        {
            var dependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange("NuGet.Versioning", new VersionRange(new NuGetVersion("1.0.0")), LibraryDependencyTarget.Package),
            };

            // Arrange
            var spec = new PackageSpec(new[]
            {
                new TargetFrameworkInformation
                {
                    FrameworkName = FrameworkConstants.CommonFrameworks.Net45,
                    Dependencies = new List<LibraryDependency>() { dependency }

                },
                new TargetFrameworkInformation
                {
                    FrameworkName = FrameworkConstants.CommonFrameworks.Net46
                }
            });
            var identity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("2.0.0"));
            var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));

            // Act
            if (usePackageDependency)
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, packageDependency);
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, identity);
            }

            // Assert
            Assert.Empty(spec.Dependencies);
            Assert.Equal(1, spec.TargetFrameworks[0].Dependencies.Count);
            Assert.Equal(identity.Id, spec.TargetFrameworks[0].Dependencies[0].LibraryRange.Name);
            Assert.Equal(identity.Version, spec.TargetFrameworks[0].Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithExistingPackages_UpdatesAllPackageDependency(bool usePackageDependency)
        {
            // Arrange
            var frameworkA = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            frameworkA.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "nuget.versioning",
                    VersionRange = new VersionRange(new NuGetVersion("0.9.0"))
                }
            });
            var frameworkB = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.NetStandard16
            };
            frameworkB.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "NUGET.VERSIONING",
                    VersionRange = new VersionRange(new NuGetVersion("0.8.0"))
                }
            });
            var spec = new PackageSpec(new[] { frameworkA, frameworkB });
            var identity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("1.0.0"));
            var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));

            // Act
            if (usePackageDependency)
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, packageDependency);
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(spec, identity);
            }

            // Assert
            Assert.Empty(spec.Dependencies);

            Assert.Equal(1, spec.TargetFrameworks[0].Dependencies.Count);
            Assert.Equal("nuget.versioning", spec.TargetFrameworks[0].Dependencies[0].LibraryRange.Name);
            Assert.Equal(
                identity.Version,
                spec.TargetFrameworks[0].Dependencies[0].LibraryRange.VersionRange.MinVersion);

            Assert.Equal(1, spec.TargetFrameworks[1].Dependencies.Count);
            Assert.Equal("NUGET.VERSIONING", spec.TargetFrameworks[1].Dependencies[0].LibraryRange.Name);
            Assert.Equal(
                identity.Version,
                spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_ToSpecificFrameworks_UpdatesExistingDependencies(bool usePackageDependency)
        {
            // Arrange
            var packageId = "NuGet.Versioning";
            var oldVersion = new NuGetVersion("1.0.0");
            var newVersion = new NuGetVersion("2.0.0");

            var frameworkA = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            var ld = new LibraryDependency()
            {
                LibraryRange = new LibraryRange(packageId, new VersionRange(oldVersion), LibraryDependencyTarget.Package)
            };
            var frameworkB = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.NetStandard16,
                Dependencies = new List<LibraryDependency>() { ld }

            };
            var spec = new PackageSpec(new[] { frameworkA, frameworkB });
            var identity = new PackageIdentity(packageId, newVersion);

            //Preconditions
            Assert.Equal(
                oldVersion,
                spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);

            // Act
            if (usePackageDependency)
            {
                var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));
                PackageSpecOperations.AddOrUpdateDependency(
                      spec,
                      packageDependency,
                      new[] { frameworkB.FrameworkName });
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(
                    spec,
                    identity,
                    new[] { frameworkB.FrameworkName });
            }


            // Assert
            Assert.Empty(spec.Dependencies);

            Assert.Empty(spec.TargetFrameworks[0].Dependencies);

            Assert.Equal(1, spec.TargetFrameworks[1].Dependencies.Count);
            Assert.Equal(identity.Id, spec.TargetFrameworks[1].Dependencies[0].LibraryRange.Name);
            Assert.Equal(
                identity.Version,
                spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_ToSpecificFrameworks_AddsNewDependency(bool usePackageDependency)
        {
            // Arrange
            var packageId = "NuGet.Versioning";
            var oldVersion = new NuGetVersion("1.0.0");
            var newVersion = new NuGetVersion("2.0.0");

            var frameworkA = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            var ld = new LibraryDependency()
            {
                LibraryRange = new LibraryRange(packageId, new VersionRange(oldVersion), LibraryDependencyTarget.Package)
            };
            var frameworkB = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.NetStandard16,
                Dependencies = new List<LibraryDependency>() { ld }

            };
            var spec = new PackageSpec(new[] { frameworkA, frameworkB });
            var identity = new PackageIdentity(packageId, newVersion);

            //Preconditions
            Assert.Equal(
                oldVersion,
                spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);

            // Act
            if (usePackageDependency)
            {
                var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));
                PackageSpecOperations.AddOrUpdateDependency(
                      spec,
                      packageDependency,
                      new[] { frameworkB.FrameworkName });
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(
                    spec,
                    identity,
                    new[] { frameworkB.FrameworkName });
            }

            // Assert
            Assert.Empty(spec.Dependencies);

            Assert.Empty(spec.TargetFrameworks[0].Dependencies);

            Assert.Equal(1, spec.TargetFrameworks[1].Dependencies.Count);
            Assert.Equal(identity.Id, spec.TargetFrameworks[1].Dependencies[0].LibraryRange.Name);
            Assert.Equal(
                identity.Version,
                spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithCentralPackageManagementEnabled_AddsDependency(bool usePackageDependency)
        {
            // Arrange
            var packageIdentity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("1.0.0"));

            var targetFrameworkInformation = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };

            var spec = new PackageSpec(new[] { targetFrameworkInformation })
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    CentralPackageVersionsEnabled = true
                }
            };

            // Act
            if (usePackageDependency)
            {
                var packageDependency = new PackageDependency(packageIdentity.Id, new VersionRange(packageIdentity.Version));
                PackageSpecOperations.AddOrUpdateDependency(
                      spec,
                      packageDependency,
                      new[] { targetFrameworkInformation.FrameworkName });
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(
                    spec,
                    packageIdentity,
                    new[] { targetFrameworkInformation.FrameworkName });
            }

            // Assert
            Assert.Equal(1, spec.TargetFrameworks[0].Dependencies.Count);
            Assert.Equal(packageIdentity.Id, spec.TargetFrameworks[0].Dependencies[0].LibraryRange.Name);
            Assert.Equal(packageIdentity.Version, spec.TargetFrameworks[0].Dependencies[0].LibraryRange.VersionRange.MinVersion);
            Assert.True(spec.TargetFrameworks[0].Dependencies[0].VersionCentrallyManaged);

            Assert.True(spec.TargetFrameworks[0].CentralPackageVersions.ContainsKey(packageIdentity.Id));
            Assert.Equal(packageIdentity.Version, spec.TargetFrameworks[0].CentralPackageVersions[packageIdentity.Id].VersionRange.MinVersion);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddOrUpdateDependency_WithCentralPackageManagementEnabled_UpdatesDependency(bool usePackageDependency)
        {
            // Arrange
            var packageId = "NuGet.Versioning";
            var oldVersion = new NuGetVersion("1.0.0");
            var newVersion = new NuGetVersion("2.0.0");

            var frameworkA = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };

            var ld = new LibraryDependency
            {
                LibraryRange = new LibraryRange(packageId, new VersionRange(oldVersion), LibraryDependencyTarget.Package),
                VersionCentrallyManaged = true
            };

            var frameworkB = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.NetStandard16,
                Dependencies = new List<LibraryDependency>() { ld },
            };

            frameworkB.CentralPackageVersions[ld.Name] = new CentralPackageVersion(ld.Name, ld.LibraryRange.VersionRange);

            var spec = new PackageSpec(new[] { frameworkA, frameworkB })
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    CentralPackageVersionsEnabled = true
                }
            };
            var identity = new PackageIdentity(packageId, newVersion);

            //Preconditions
            Assert.Equal(
                oldVersion,
                spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);

            // Act
            if (usePackageDependency)
            {
                var packageDependency = new PackageDependency(identity.Id, new VersionRange(identity.Version));
                PackageSpecOperations.AddOrUpdateDependency(
                      spec,
                      packageDependency,
                      new[] { frameworkB.FrameworkName });
            }
            else
            {
                PackageSpecOperations.AddOrUpdateDependency(
                    spec,
                    identity,
                    new[] { frameworkB.FrameworkName });
            }

            // Assert
            Assert.Empty(spec.Dependencies);

            Assert.Empty(spec.TargetFrameworks[0].Dependencies);

            Assert.Equal(1, spec.TargetFrameworks[1].Dependencies.Count);
            Assert.Equal(identity.Id, spec.TargetFrameworks[1].Dependencies[0].LibraryRange.Name);
            Assert.Equal(identity.Version, spec.TargetFrameworks[1].Dependencies[0].LibraryRange.VersionRange.MinVersion);
            Assert.True(spec.TargetFrameworks[1].Dependencies[0].VersionCentrallyManaged);

            Assert.True(spec.TargetFrameworks[1].CentralPackageVersions.ContainsKey(identity.Id));
            Assert.Equal(identity.Version, spec.TargetFrameworks[1].CentralPackageVersions[identity.Id].VersionRange.MinVersion);
        }

        [Fact]
        public void RemoveDependency_RemovesFromAllFrameworkLists()
        {
            // Arrange
            var frameworkA = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            frameworkA.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "nuget.versioning",
                    VersionRange = new VersionRange(new NuGetVersion("0.9.0"))
                }
            });
            var frameworkB = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.NetStandard16
            };
            frameworkB.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "NUGET.VERSIONING",
                    VersionRange = new VersionRange(new NuGetVersion("0.8.0"))
                }
            });
            var spec = new PackageSpec(new[] { frameworkA, frameworkB });
            spec.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "NuGet.VERSIONING",
                    VersionRange = new VersionRange(new NuGetVersion("0.7.0"))
                }
            });
            var id = "NuGet.Versioning";

            // Act
            PackageSpecOperations.RemoveDependency(spec, id);

            // Assert
            Assert.Empty(spec.Dependencies);
            Assert.Empty(spec.TargetFrameworks[0].Dependencies);
            Assert.Empty(spec.TargetFrameworks[1].Dependencies);
        }

        [Fact]
        public void HasPackage_ReturnsTrueWhenIdIsInFramework()
        {
            // Arrange
            var framework = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            framework.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "nuget.versioning",
                    VersionRange = new VersionRange(new NuGetVersion("0.9.0"))
                }
            });
            var spec = new PackageSpec(new[] { framework });
            var id = "NuGet.Versioning";

            // Act
            var actual = PackageSpecOperations.HasPackage(spec, id);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void HasPackage_ReturnsTrueWhenIdIsForAllFrameworks()
        {
            // Arrange
            var framework = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            var spec = new PackageSpec(new[] { framework });
            spec.Dependencies.Add(new LibraryDependency
            {
                LibraryRange = new LibraryRange
                {
                    Name = "nuget.versioning",
                    VersionRange = new VersionRange(new NuGetVersion("0.9.0"))
                }
            });
            var id = "NuGet.Versioning";

            // Act
            var actual = PackageSpecOperations.HasPackage(spec, id);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void HasPackage_ReturnsFalseWhenIdIsNotInSpec()
        {
            // Arrange
            var framework = new TargetFrameworkInformation
            {
                FrameworkName = FrameworkConstants.CommonFrameworks.Net45
            };
            var spec = new PackageSpec(new[] { framework });
            var id = "NuGet.Versioning";

            // Act
            var actual = PackageSpecOperations.HasPackage(spec, id);

            // Assert
            Assert.False(actual);
        }
    }
}
