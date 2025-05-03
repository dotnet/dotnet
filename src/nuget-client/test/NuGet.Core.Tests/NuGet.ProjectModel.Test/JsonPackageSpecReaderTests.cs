// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    [UseCulture("")] // Fix tests failing on systems with non-English locales
    [Obsolete]
    public class JsonPackageSpecReaderTests
    {
        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_PackageMissingVersion(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""type"": ""build""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";

            // Act
            Exception exception = null;

            try
            {
                var spec = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Contains("specify a version range", exception.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ProjectMissingVersion(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""project""
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    }
                                }";

            // Act
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var spec = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);
            var range = spec.Dependencies.Single().LibraryRange.VersionRange;

            // Assert
            Assert.Equal(VersionRange.All, range);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_PackageEmptyVersion(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""package"",
                                            ""version"": """"
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    }
                                }";

            Exception exception = null;

            try
            {
                var spec = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Contains("specify a version range", exception.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_PackageWhitespaceVersion(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""package"",
                                            ""version"": ""   ""
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    }
                                }";

            Exception exception = null;

            try
            {
                var spec = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Contains("not a valid version string", exception.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_FrameworkAssemblyEmptyVersion(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                    ""frameworks"": {
                                        ""net46"": {
                                            ""frameworkAssemblies"": {
                                               ""packageA"": """"
                                            }
                                        }
                                    }
                                }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);
            var range = spec.TargetFrameworks.Single().Dependencies.Single().LibraryRange.VersionRange;

            // Assert
            Assert.Equal(VersionRange.All, range);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ExplicitIncludesOverrideTypePlatform(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                   ""dependencies"": {
                                     ""redist"": {
                                       ""version"": ""1.0.0"",
                                       ""type"": ""platform"",
                                       ""include"": ""analyzers""
                                     }
                                   }
                                 }";

            // Act
            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("redist"));
            Assert.NotNull(dep);

            var expected = LibraryIncludeFlags.Analyzers;
            Assert.Equal(expected, dep.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsWithoutRestoreSettings(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""package"",
                                            ""version"": ""1.0.0""
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    },
                                }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.RestoreSettings);
            Assert.False(actual.RestoreSettings.HideWarningsAndErrors);
            Assert.Null(actual.RestoreSettings.SdkVersion);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsDependencyWithMultipleNoWarn(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""package"",
                                            ""version"": ""1.0.0"",
                                            ""noWarn"": [
                                                ""NU1500"",
                                                ""NU1107""
                                              ]
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    },
                                }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("packageA"));
            Assert.NotNull(dep);
            Assert.Equal(dep.NoWarn.Length, 2);
            Assert.Contains(NuGetLogCode.NU1500, dep.NoWarn);
            Assert.Contains(NuGetLogCode.NU1107, dep.NoWarn);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsDependencyWithSingleNoWarn(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""package"",
                                            ""version"": ""1.0.0"",
                                            ""noWarn"": [
                                                ""NU1500""
                                              ]
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    },
                                }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("packageA"));
            Assert.NotNull(dep);
            Assert.Equal(dep.NoWarn.Length, 1);
            Assert.Contains(NuGetLogCode.NU1500, dep.NoWarn);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsDependencyWithSingleEmptyNoWarn(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                  ""dependencies"": {
                                        ""packageA"": {
                                            ""target"": ""package"",
                                            ""version"": ""1.0.0"",
                                            ""noWarn"": [
                                              ]
                                        }
                                    },
                                    ""frameworks"": {
                                        ""net46"": {}
                                    },
                                }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("packageA"));
            Assert.NotNull(dep);
            Assert.Equal(dep.NoWarn.Length, 0);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""configFilePaths"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""fallbackFolders"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""originalTargetFrameworks"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""sources"": {
              ""source"": {}
            },
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""projectReferences"": {}
              }
            },
            ""warningProperties"": {
              ""allWarningsAsErrors"": true,
              ""noWarn"": [
                ""NU1601"",
              ],
              ""warnAsError"": [
                ""NU1500"",
                ""NU1501""
              ],
              ""warnNotAsError"": [
                ""NU1801"",
                ""NU1802""
              ]
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
            Assert.True(warningProperties.AllWarningsAsErrors);
            Assert.Equal(1, warningProperties.NoWarn.Count);
            Assert.True(warningProperties.NoWarn.Contains(NuGetLogCode.NU1601));
            Assert.Equal(2, warningProperties.WarningsAsErrors.Count);
            Assert.True(warningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1500));
            Assert.True(warningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1501));
            Assert.Equal(2, warningProperties.WarningsNotAsErrors.Count);
            Assert.True(warningProperties.WarningsNotAsErrors.Contains(NuGetLogCode.NU1801));
            Assert.True(warningProperties.WarningsNotAsErrors.Contains(NuGetLogCode.NU1802));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningPropertiesAndNo_NoWarn(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""configFilePaths"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""fallbackFolders"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""originalTargetFrameworks"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""sources"": {
              ""source"": {}
            },
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""projectReferences"": {}
              }
            },
            ""warningProperties"": {
              ""allWarningsAsErrors"": true,
              ""warnAsError"": [
                ""NU1500"",
                ""NU1501""
              ]
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
            Assert.True(warningProperties.AllWarningsAsErrors);
            Assert.Equal(0, warningProperties.NoWarn.Count);
            Assert.Equal(2, warningProperties.WarningsAsErrors.Count);
            Assert.True(warningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1500));
            Assert.True(warningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1501));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningPropertiesAndNo_WarnAsError(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""configFilePaths"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""fallbackFolders"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""originalTargetFrameworks"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""sources"": {
              ""source"": {}
            },
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""projectReferences"": {}
              }
            },
            ""warningProperties"": {
              ""allWarningsAsErrors"": true,
              ""noWarn"": [
                ""NU1601"",
              ]
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
            Assert.True(warningProperties.AllWarningsAsErrors);
            Assert.Equal(1, warningProperties.NoWarn.Count);
            Assert.True(warningProperties.NoWarn.Contains(NuGetLogCode.NU1601));
            Assert.Equal(0, warningProperties.WarningsAsErrors.Count);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningPropertiesAndNo_AllWarningsAsErrors(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""configFilePaths"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""fallbackFolders"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""originalTargetFrameworks"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""sources"": {
              ""source"": {}
            },
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""projectReferences"": {}
              }
            },
            ""warningProperties"": {
              ""noWarn"": [
                ""NU1601"",
              ],
              ""warnAsError"": [
                ""NU1500"",
                ""NU1501""
              ]
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
            Assert.False(warningProperties.AllWarningsAsErrors);
            Assert.Equal(1, warningProperties.NoWarn.Count);
            Assert.True(warningProperties.NoWarn.Contains(NuGetLogCode.NU1601));
            Assert.Equal(2, warningProperties.WarningsAsErrors.Count);
            Assert.True(warningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1500));
            Assert.True(warningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1501));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithEmptyWarningPropertiesAnd(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""configFilePaths"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""fallbackFolders"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""originalTargetFrameworks"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""sources"": {
              ""source"": {}
            },
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""projectReferences"": {}
              }
            },
            ""warningProperties"": {
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
            Assert.False(warningProperties.AllWarningsAsErrors);
            Assert.Equal(0, warningProperties.NoWarn.Count);
            Assert.Equal(0, warningProperties.WarningsAsErrors.Count);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithNoWarningProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""configFilePaths"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""fallbackFolders"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""originalTargetFrameworks"": [
              ""b"",
              ""a"",
              ""c""
            ],
            ""sources"": {
              ""source"": {}
            },
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""projectReferences"": {}
              }
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_RuntimeIdentifierPathNullIfEmpty(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                    ""frameworks"": {
                                        ""net46"": {
                                            ""dependencies"": {
                                                ""packageA"": {
                                                ""target"": ""package"",
                                                ""version"": ""1.0.0"",
                                                ""noWarn"": [
                                                    ""NU1500""
                                                ]
                                             }
                                          }
                                        }
                                    }
                                }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            Assert.Null(spec.TargetFrameworks.First().RuntimeIdentifierGraphPath);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesPropertyIsAbsent_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSpec packageSpec = GetPackageSpec("{}", environmentVariableReader);

            Assert.Empty(packageSpec.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesValueIsNull_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSpec packageSpec = GetPackageSpec("{\"dependencies\":null}", environmentVariableReader);

            Assert.Empty(packageSpec.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyNameIsEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"\":{}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));
            Assert.Equal("Unable to resolve dependency ''.", exception.Message);
            Assert.Null(exception.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal(1, exception.Line);
                Assert.Equal(21, exception.Column);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyValueIsVersionString_ReturnsDependencyVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new LibraryRange(
                name: "a",
                new VersionRange(new NuGetVersion("1.2.3")),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange.ToShortString()}\"}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyValueIsVersionRangeString_ReturnsDependencyVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new LibraryRange(
                name: "a",
                new VersionRange(new NuGetVersion("1.2.3"), includeMinVersion: true, new NuGetVersion("4.5.6"), includeMaxVersion: false),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange}\"}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.None, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.Assembly, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.Reference, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.WinMD, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.All, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.PackageProjectExternal, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyTargetIsUnsupported_Throws(IEnvironmentVariableReader environmentVariableReader, LibraryDependencyTarget target)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"version\":\"1.2.3\",\"target\":\"{target}\"}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.Equal($"Invalid dependency target value '{target}'.", exception.Message);
            Assert.Null(exception.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal(1, exception.Line);
                // The position is after the target name, which is of variable length.
                Assert.Equal(json.IndexOf(target.ToString()) + target.ToString().Length + 1, exception.Column);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyAutoreferencedPropertyIsAbsent_ReturnsFalseAutoreferenced(IEnvironmentVariableReader environmentVariableReader)
        {
            LibraryDependency dependency = GetDependency($"{{\"dependencies\":{{\"a\":{{\"target\":\"Project\"}}}}}}", environmentVariableReader);

            Assert.False(dependency.AutoReferenced);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyAutoreferencedValueIsBool_ReturnsBoolAutoreferenced(IEnvironmentVariableReader environmentVariableReader, bool expectedValue)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"autoReferenced\":{expectedValue.ToString().ToLower()},\"target\":\"Project\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedValue, dependency.AutoReferenced);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "exclude", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "include", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "suppressParent", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyValueIsArray_Throws(IEnvironmentVariableReader environmentVariableReader, string propertyName)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"{propertyName}\":[\"b\"]}}}}}}";

            Assert.Throws<InvalidCastException>(() => GetPackageSpec(json, environmentVariableReader));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyIncludeAndExcludePropertiesAreAbsent_ReturnsAllIncludeType(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.All, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"Native\"", LibraryIncludeFlags.Native, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"Analyzers, Native\"", LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.Native, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyExcludeValueIsValid_ReturnsIncludeType(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            LibraryIncludeFlags result)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"exclude\":{value},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.All & ~result, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"Native\"", LibraryIncludeFlags.Native, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"Analyzers, Native\"", LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.Native, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyIncludeValueIsValid_ReturnsIncludeType(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            LibraryIncludeFlags expectedResult)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"include\":{value},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyIncludeValueOverridesTypeValue_ReturnsIncludeType(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"include\":\"ContentFiles\",\"type\":\"BecomesNupkgDependency, SharedFramework\",\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencySuppressParentValueOverridesTypeValue_ReturnsSuppressParent(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"suppressParent\":\"ContentFiles\",\"type\":\"SharedFramework\",\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.SuppressParent);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencySuppressParentPropertyIsAbsent_ReturnsSuppressParent(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, dependency.SuppressParent);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"Compile\"", LibraryIncludeFlags.Compile, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"Analyzers, Compile\"", LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.Compile, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencySuppressParentValueIsValid_ReturnsSuppressParent(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            LibraryIncludeFlags expectedResult
            )
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"suppressParent\":{value},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.SuppressParent);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyVersionValueIsInvalid_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"b\"}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 35 : 'b' is not a valid version string.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(35, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : 'b' is not a valid version string.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyTargetPropertyIsAbsent_ReturnsTarget(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyTargetValueIsPackageAndVersionPropertyIsAbsent_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"target\":\"Package\"}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));
            Assert.IsType<ArgumentException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 22 : Package dependencies must specify a version range.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(22, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Package dependencies must specify a version range.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyTargetValueIsProjectAndVersionPropertyIsAbsent_ReturnsAllVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            LibraryDependency dependency = GetDependency("{\"dependencies\":{\"a\":{\"target\":\"Project\"}}}", environmentVariableReader);

            Assert.Equal(VersionRange.All, dependency.LibraryRange.VersionRange);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyNoWarnPropertyIsAbsent_ReturnsEmptyNoWarns(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Empty(dependency.NoWarn);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyNoWarnValueIsValid_ReturnsNoWarns(IEnvironmentVariableReader environmentVariableReader)
        {
            NuGetLogCode[] expectedResults = { NuGetLogCode.NU1000, NuGetLogCode.NU3000 };
            var json = $"{{\"dependencies\":{{\"a\":{{\"noWarn\":[\"{expectedResults[0].ToString()}\",\"{expectedResults[1].ToString()}\"],\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Collection(
                dependency.NoWarn,
                noWarn => Assert.Equal(expectedResults[0], noWarn),
                noWarn => Assert.Equal(expectedResults[1], noWarn));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyGeneratePathPropertyPropertyIsAbsent_ReturnsFalseGeneratePathProperty(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.False(dependency.GeneratePathProperty);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyGeneratePathPropertyValueIsValid_ReturnsGeneratePathProperty(IEnvironmentVariableReader environmentVariableReader, bool expectedResult)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"generatePathProperty\":{expectedResult.ToString().ToLowerInvariant()},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.GeneratePathProperty);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyTypePropertyIsAbsent_ReturnsDefaultTypeConstraint(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference,
                dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyVersionCentrallyManagedPropertyIsAbsent_ReturnsFalseVersionCentrallyManaged(IEnvironmentVariableReader environmentVariableReader)
        {
            LibraryDependency dependency = GetDependency($"{{\"dependencies\":{{\"a\":{{\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}}", environmentVariableReader);

            Assert.False(dependency.VersionCentrallyManaged);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenDependenciesDependencyVersionCentrallyManagedValueIsBool_ReturnsBoolVersionCentrallyManaged(IEnvironmentVariableReader environmentVariableReader, bool expectedValue)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"versionCentrallyManaged\":{expectedValue.ToString().ToLower()},\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json, environmentVariableReader);

            Assert.Equal(expectedValue, dependency.VersionCentrallyManaged);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksPropertyIsAbsent_ReturnsEmptyFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSpec packageSpec = GetPackageSpec("{}", environmentVariableReader);

            Assert.Empty(packageSpec.TargetFrameworks);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksValueIsEmptyObject_ReturnsEmptyFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSpec packageSpec = GetPackageSpec("{\"frameworks\":{}}", environmentVariableReader);

            Assert.Empty(packageSpec.TargetFrameworks);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksAssetTargetFallbackPropertyIsAbsent_ReturnsFalseAssetTargetFallback(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}", environmentVariableReader);

            Assert.False(framework.AssetTargetFallback);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksAssetTargetFallbackValueIsValid_ReturnsAssetTargetFallback(IEnvironmentVariableReader environmentVariableReader, bool expectedValue)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"assetTargetFallback\":{expectedValue.ToString().ToLowerInvariant()}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Equal(expectedValue, framework.AssetTargetFallback);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithAssetTargetFallbackAndImportsValues_ReturnsValidAssetTargetFallbackFramework(IEnvironmentVariableReader environmentVariableReader)
        {
            var json = $"{{\"frameworks\":{{\"net5.0\":{{\"assetTargetFallback\": true, \"imports\": [\"net472\", \"net471\"]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            framework.AssetTargetFallback.Should().BeTrue();
            var assetTargetFallback = framework.FrameworkName as AssetTargetFallbackFramework;
            assetTargetFallback.RootFramework.Should().Be(FrameworkConstants.CommonFrameworks.Net50);
            assetTargetFallback.Fallback.Should().HaveCount(2);
            assetTargetFallback.Fallback.First().Should().Be(FrameworkConstants.CommonFrameworks.Net472);
            assetTargetFallback.Fallback.Last().Should().Be(FrameworkConstants.CommonFrameworks.Net471);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsPropertyIsAbsent_ReturnsEmptyCentralPackageVersions(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}", environmentVariableReader);

            Assert.Empty(framework.CentralPackageVersions);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsValueIsEmptyObject_ReturnsEmptyCentralPackageVersions(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{\"centralPackageVersions\":{}}}}", environmentVariableReader);

            Assert.Empty(framework.CentralPackageVersions);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsVersionPropertyNameIsEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            var json = "{\"frameworks\":{\"a\":{\"centralPackageVersions\":{\"\":\"1.0.0\"}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve central version ''.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Unable to resolve central version ''.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsVersionPropertyValueIsNullOrEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader, string value)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"centralPackageVersions\":{{\"b\":{value}}}}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : The version cannot be null or empty.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : The version cannot be null or empty.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsIsValid_ReturnsCentralPackageVersions(IEnvironmentVariableReader environmentVariableReader)
        {
            const string expectedPackageId = "b";
            VersionRange expectedVersionRange = VersionRange.Parse("[1.2.3,4.5.6)");
            var expectedCentralPackageVersion = new CentralPackageVersion(expectedPackageId, expectedVersionRange);
            var json = $"{{\"frameworks\":{{\"a\":{{\"centralPackageVersions\":{{\"{expectedPackageId}\":\"{expectedVersionRange.ToShortString()}\"}}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Collection(
                framework.CentralPackageVersions,
                actualResult =>
                {
                    Assert.Equal(expectedPackageId, actualResult.Key);
                    Assert.Equal(expectedCentralPackageVersion, actualResult.Value);
                });
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsHasDuplicateKey_LastOneWins(IEnvironmentVariableReader environmentVariableReader)
        {
            const string expectedPackageId = "b";
            VersionRange unexpectedVersionRange = VersionRange.Parse("1.2.3");
            VersionRange expectedVersionRange = VersionRange.Parse("4.5.6");
            var expectedCentralPackageVersion = new CentralPackageVersion(expectedPackageId, expectedVersionRange);
            var json = $"{{\"frameworks\":{{\"a\":{{\"centralPackageVersions\":{{\"{expectedPackageId}\":\"{unexpectedVersionRange.ToShortString()}\"," +
                $"\"{expectedPackageId}\":\"{expectedVersionRange.ToShortString()}\"}}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Collection(
                framework.CentralPackageVersions,
                actualResult =>
                {
                    Assert.Equal(expectedPackageId, actualResult.Key);
                    Assert.Equal(expectedCentralPackageVersion, actualResult.Value);
                });
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesPropertyIsAbsent_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}", environmentVariableReader);

            Assert.Empty(framework.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesValueIsNull_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{\"dependencies\":null}}}", environmentVariableReader);

            Assert.Empty(framework.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyNameIsEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"\":{}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve dependency ''.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Unable to resolve dependency ''.", exception.Message);
            }
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyValueIsVersionString_ReturnsDependencyVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new LibraryRange(
                name: "b",
                new VersionRange(new NuGetVersion("1.2.3")),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange.ToShortString()}\"}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyValueIsVersionRangeString_ReturnsDependencyVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new LibraryRange(
                name: "b",
                new VersionRange(new NuGetVersion("1.2.3"), includeMinVersion: true, new NuGetVersion("4.5.6"), includeMaxVersion: false),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange}\"}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.None, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.Assembly, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.Reference, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.WinMD, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.All, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), LibraryDependencyTarget.PackageProjectExternal, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetValueIsUnsupported_Throws(IEnvironmentVariableReader environmentVariableReader, LibraryDependencyTarget target)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"version\":\"1.2.3\",\"target\":\"{target}\"}}}}}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal($"Error reading '' at line 1 column 20 : Invalid dependency target value '{target}'.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal($"Error reading '' : Invalid dependency target value '{target}'.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyAutoreferencedPropertyIsAbsent_ReturnsFalseAutoreferenced(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Project\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.False(dependency.AutoReferenced);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyAutoreferencedValueIsBool_ReturnsBoolAutoreferenced(IEnvironmentVariableReader environmentVariableReader, bool expectedValue)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"autoReferenced\":{expectedValue.ToString().ToLower()},\"target\":\"Project\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(expectedValue, dependency.AutoReferenced);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "exclude", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "include", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "suppressParent", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyValueIsArray_Throws(IEnvironmentVariableReader environmentVariableReader, string propertyName)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"{propertyName}\":[\"c\"]}}}}}}}}}}";

            // The exception messages will not be the same because the innermost exception in the baseline
            // is a Newtonsoft.Json exception, while it's a .NET exception in the improved.
            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<InvalidCastException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Specified cast is not valid.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Specified cast is not valid.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyIncludeAndExcludePropertiesAreAbsent_ReturnsAllIncludeType(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.All, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyExcludeValueIsValid_ReturnsIncludeType(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"exclude\":\"Native\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.All & ~LibraryIncludeFlags.Native, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyIncludeValueIsValid_ReturnsIncludeType(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"include\":\"ContentFiles\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyIncludeValueOverridesTypeValue_ReturnsIncludeType(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"include\":\"ContentFiles\",\"type\":\"BecomesNupkgDependency, SharedFramework\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.IncludeType);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencySuppressParentValueOverridesTypeValue_ReturnsSuppressParent(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"suppressParent\":\"ContentFiles\",\"type\":\"SharedFramework\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.SuppressParent);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencySuppressParentPropertyIsAbsent_ReturnsSuppressParent(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, dependency.SuppressParent);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencySuppressParentValueIsValid_ReturnsSuppressParent(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"suppressParent\":\"Compile\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryIncludeFlags.Compile, dependency.SuppressParent);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyVersionValueIsInvalid_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"c\"}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Error reading '' at line 1 column 54 : 'c' is not a valid version string.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Error reading '' : 'c' is not a valid version string.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetPropertyIsAbsent_ReturnsTarget(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference,
                dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetValueIsPackageAndVersionPropertyIsAbsent_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Package\"}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Error reading '' at line 1 column 41 : Package dependencies must specify a version range.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Error reading '' : Package dependencies must specify a version range.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetValueIsProjectAndVersionPropertyIsAbsent_ReturnsAllVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Project\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(VersionRange.All, dependency.LibraryRange.VersionRange);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyNoWarnPropertyIsAbsent_ReturnsEmptyNoWarns(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Empty(dependency.NoWarn);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyNoWarnValueIsValid_ReturnsNoWarns(IEnvironmentVariableReader environmentVariableReader)
        {
            NuGetLogCode[] expectedResults = { NuGetLogCode.NU1000, NuGetLogCode.NU3000 };
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"noWarn\":[\"{expectedResults[0].ToString()}\",\"{expectedResults[1].ToString()}\"],\"version\":\"1.0.0\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Collection(
                dependency.NoWarn,
                noWarn => Assert.Equal(expectedResults[0], noWarn),
                noWarn => Assert.Equal(expectedResults[1], noWarn));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyGeneratePathPropertyPropertyIsAbsent_ReturnsFalseGeneratePathProperty(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.False(dependency.GeneratePathProperty);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyGeneratePathPropertyValueIsValid_ReturnsGeneratePathProperty(IEnvironmentVariableReader environmentVariableReader, bool expectedResult)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"generatePathProperty\":{expectedResult.ToString().ToLowerInvariant()},\"version\":\"1.0.0\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency.GeneratePathProperty);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTypePropertyIsAbsent_ReturnsDefaultTypeConstraint(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference,
                dependency.LibraryRange.TypeConstraint);
        }


        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyVersionCentrallyManagedPropertyIsAbsent_ReturnsFalseVersionCentrallyManaged(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.False(dependency.VersionCentrallyManaged);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyVersionCentrallyManagedValueIsBool_ReturnsBoolVersionCentrallyManaged(IEnvironmentVariableReader environmentVariableReader, bool expectedValue)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"versionCentrallyManaged\":{expectedValue.ToString().ToLower()},\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(expectedValue, dependency.VersionCentrallyManaged);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesPropertyIsAbsent_ReturnsEmptyDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsNull_ReturnsEmptyDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsNotArray_ReturnsEmptyDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":\"b\"}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsEmptyArray_ReturnsEmptyDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[]}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyNameIsAbsent_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[{\"version\":\"1.2.3\"}]}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve downloadDependency ''.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Unable to resolve downloadDependency ''.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyNameIsNull_ReturnsDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new DownloadDependency(name: null, new VersionRange(new NuGetVersion("1.2.3")));
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":null,\"version\":\"{expectedResult.VersionRange.ToShortString()}\"}}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            DownloadDependency actualResult = framework.DownloadDependencies.Single();

            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.VersionRange, actualResult.VersionRange);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyVersionIsAbsent_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[{\"name\":\"b\"}]}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : The version cannot be null or empty", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : The version cannot be null or empty", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "c", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyVersionIsInvalid_Throws(IEnvironmentVariableReader environmentVariableReader, string version)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":\"b\",\"version\":\"{version}\"}}]}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            int expectedColumn = json.IndexOf($"\"{version}\"") + version.Length + 2;

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal($"Error reading '' at line 1 column 20 : Error reading '' at line 1 column {expectedColumn} : '{version}' is not a valid version string.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal($"Error reading '' : Error reading '' : '{version}' is not a valid version string.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsValid_ReturnsDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new DownloadDependency(name: "b", new VersionRange(new NuGetVersion("1.2.3")));
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":\"{expectedResult.Name}\",\"version\":\"{expectedResult.VersionRange.ToShortString()}\"}}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Equal(expectedResult, framework.DownloadDependencies.Single());
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsValidWithMultipleVersions_ReturnsDownloadDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":\"b\",\"version\":\"1.2.3;;2.0.0\"}}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Equal(2, framework.DownloadDependencies.Count());
            Assert.Equal("b", framework.DownloadDependencies[0].Name);
            Assert.Equal(new VersionRange(new NuGetVersion("1.2.3")), framework.DownloadDependencies[0].VersionRange);
            Assert.Equal("b", framework.DownloadDependencies[1].Name);
            Assert.Equal(new VersionRange(new NuGetVersion("2.0.0")), framework.DownloadDependencies[1].VersionRange);

        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueHasDuplicates_PrefersFirstByName(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new DownloadDependency(name: "b", new VersionRange(new NuGetVersion("1.2.3")));
            var unexpectedResult = new DownloadDependency(name: "b", new VersionRange(new NuGetVersion("4.5.6")));
            var json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[" +
                $"{{\"name\":\"{expectedResult.Name}\",\"version\":\"{expectedResult.VersionRange.ToShortString()}\"}}," +
                $"{{\"name\":\"{unexpectedResult.Name}\",\"version\":\"{unexpectedResult.VersionRange.ToShortString()}\"}}" +
                "]}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Equal(expectedResult, framework.DownloadDependencies.Single());
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesPropertyIsAbsent_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesValueIsNull_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesValueIsEmptyObject_ReturnsEmptyDependencies(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesDependencyTargetPropertyIsAbsent_ReturnsTarget(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(LibraryDependencyTarget.Reference, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesDependencyTargetValueIsPackageAndVersionPropertyIsAbsent_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{\"b\":{\"target\":\"Package\"}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Error reading '' at line 1 column 48 : Package dependencies must specify a version range.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Error reading '' : Package dependencies must specify a version range.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesDependencyTargetValueIsProjectAndVersionPropertyIsAbsent_ReturnsAllVersionRange(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{\"b\":{\"target\":\"Project\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json, environmentVariableReader);

            Assert.Equal(VersionRange.All, dependency.LibraryRange.VersionRange);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPropertyIsAbsent_ReturnsEmptyFrameworkReferences(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.FrameworkReferences);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesValueIsNull_ReturnsEmptyFrameworkReferences(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkReferences\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.FrameworkReferences);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesValueIsEmptyObject_ReturnsEmptyFrameworkReferences(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkReferences\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.FrameworkReferences);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesFrameworkNameIsEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkReferences\":{\"\":{}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve frameworkReference.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Unable to resolve frameworkReference.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsPropertyIsAbsent_ReturnsNonePrivateAssets(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.None);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"null\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"c\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsValueIsInvalidValue_ReturnsNonePrivateAssets(IEnvironmentVariableReader environmentVariableReader, string privateAssets)
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.None);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{\"privateAssets\":{privateAssets}}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsValueIsValidString_ReturnsPrivateAssets(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.All);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{\"privateAssets\":\"{expectedResult.PrivateAssets.ToString().ToLowerInvariant()}\"}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsValueIsValidDelimitedString_ReturnsPrivateAssets(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.All);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{\"privateAssets\":\"none,all\"}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json, environmentVariableReader);

            Assert.Equal(expectedResult, dependency);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsPropertyIsAbsent_ReturnsEmptyImports(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Imports);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsValueIsArrayOfNullOrEmptyString_ImportIsSkipped(IEnvironmentVariableReader environmentVariableReader, string import)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":[{import}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Imports);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsValueIsNull_ReturnsEmptyList(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{\"imports\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Imports);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "true", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "-2", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "3.14", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "{}", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsValueIsInvalidValue_ReturnsEmptyList(IEnvironmentVariableReader environmentVariableReader, string value)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":{value}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Empty(framework.Imports);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsValueContainsInvalidValue_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            const string expectedImport = "b";

            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":[\"{expectedImport}\"]}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal(
                    $"Error reading '' at line 1 column 20 : Imports contains an invalid framework: '{expectedImport}' in ''.",
                    exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal(
                $"Error reading '' : Imports contains an invalid framework: '{expectedImport}' in ''.",
                    exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsValueIsString_ReturnsImport(IEnvironmentVariableReader environmentVariableReader)
        {
            NuGetFramework expectedResult = NuGetFramework.Parse("net48");
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":\"{expectedResult.GetShortFolderName()}\"}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Collection(
                framework.Imports,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksImportsValueIsArrayOfStrings_ReturnsImports(IEnvironmentVariableReader environmentVariableReader)
        {
            NuGetFramework[] expectedResults = { NuGetFramework.Parse("net472"), NuGetFramework.Parse("net48") };
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":[\"{expectedResults[0].GetShortFolderName()}\",\"{expectedResults[1].GetShortFolderName()}\"]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Collection(
                framework.Imports,
                actualResult => Assert.Equal(expectedResults[0], actualResult),
                actualResult => Assert.Equal(expectedResults[1], actualResult));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksRuntimeIdentifierGraphPathPropertyIsAbsent_ReturnsRuntimeIdentifierGraphPath(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Null(framework.RuntimeIdentifierGraphPath);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "b", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksRuntimeIdentifierGraphPathValueIsString_ReturnsRuntimeIdentifierGraphPath(IEnvironmentVariableReader environmentVariableReader, string expectedResult)
        {
            string runtimeIdentifierGraphPath = expectedResult == null ? "null" : $"\"{expectedResult}\"";
            var json = $"{{\"frameworks\":{{\"a\":{{\"runtimeIdentifierGraphPath\":{runtimeIdentifierGraphPath}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Equal(expectedResult, framework.RuntimeIdentifierGraphPath);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksWarnPropertyIsAbsent_ReturnsWarn(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"frameworks\":{\"a\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.False(framework.Warn);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksWarnValueIsValid_ReturnsWarn(IEnvironmentVariableReader environmentVariableReader, bool expectedResult)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"warn\":{expectedResult.ToString().ToLowerInvariant()}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Equal(expectedResult, framework.Warn);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestorePropertyIsAbsent_ReturnsNullRestoreMetadata(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Null(packageSpec.RestoreMetadata);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreValueIsEmptyObject_ReturnsRestoreMetadata(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.NotNull(packageSpec.RestoreMetadata);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreProjectStyleValueIsInvalid_ReturnsProjectStyle(IEnvironmentVariableReader environmentVariableReader, string value)
        {
            var json = $"{{\"restore\":{{\"projectStyle\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(ProjectStyle.Unknown, packageSpec.RestoreMetadata.ProjectStyle);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreProjectStyleValueIsValid_ReturnsProjectStyle(IEnvironmentVariableReader environmentVariableReader)
        {
            const ProjectStyle expectedResult = ProjectStyle.PackageReference;

            var json = $"{{\"restore\":{{\"projectStyle\":\"{expectedResult.ToString().ToLowerInvariant()}\"}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.ProjectStyle);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreProjectUniqueNameValueIsValid_ReturnsProjectUniqueName(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectUniqueName\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectUniqueName);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreOutputPathValueIsValid_ReturnsOutputPath(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"outputPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.OutputPath);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestorePackagesPathValueIsValid_ReturnsPackagesPath(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"packagesPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.PackagesPath);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreProjectJsonPathValueIsValid_ReturnsProjectJsonPath(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectJsonPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectJsonPath);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreProjectNameValueIsValid_ReturnsProjectName(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectName\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectName);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreProjectPathValueIsValid_ReturnsProjectPath(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectPath);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), null, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), true, true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenCrossTargetingValueIsValid_ReturnsCrossTargeting(
            IEnvironmentVariableReader environmentVariableReader,
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"crossTargeting\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CrossTargeting);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), null, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), true, true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenLegacyPackagesDirectoryValueIsValid_ReturnsLegacyPackagesDirectory(
            IEnvironmentVariableReader environmentVariableReader,
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"legacyPackagesDirectory\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.LegacyPackagesDirectory);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), null, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), true, true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenValidateRuntimeAssetsValueIsValid_ReturnsValidateRuntimeAssets(
            IEnvironmentVariableReader environmentVariableReader,
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"validateRuntimeAssets\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ValidateRuntimeAssets);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), null, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), true, true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenSkipContentFileWriteValueIsValid_ReturnsSkipContentFileWrite(
            IEnvironmentVariableReader environmentVariableReader,
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"skipContentFileWrite\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.SkipContentFileWrite);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), null, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), true, true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenCentralPackageVersionsManagementEnabledValueIsValid_ReturnsCentralPackageVersionsManagementEnabled(
            IEnvironmentVariableReader environmentVariableReader,
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"centralPackageVersionsManagementEnabled\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenSourcesValueIsEmptyObject_ReturnsEmptySources(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{\"sources\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Empty(packageSpec.RestoreMetadata.Sources);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenSourcesValueIsValid_ReturnsSources(IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSource[] expectedResults = { new PackageSource(source: "a"), new PackageSource(source: "b") };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult.Name}\":{{}}"));
            var json = $"{{\"restore\":{{\"sources\":{{{values}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.Sources);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFilesValueIsEmptyObject_ReturnsEmptyFiles(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{\"files\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Empty(packageSpec.RestoreMetadata.Files);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFilesValueIsValid_ReturnsFiles(IEnvironmentVariableReader environmentVariableReader)
        {
            ProjectRestoreMetadataFile[] expectedResults =
            {
                        new ProjectRestoreMetadataFile(packagePath: "a", absolutePath: "b"),
                        new ProjectRestoreMetadataFile(packagePath: "c", absolutePath:"d")
                    };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult.PackagePath}\":\"{expectedResult.AbsolutePath}\""));
            var json = $"{{\"restore\":{{\"files\":{{{values}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.Files);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreFrameworksValueIsEmptyObject_ReturnsEmptyFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{\"frameworks\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Empty(packageSpec.RestoreMetadata.TargetFrameworks);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreFrameworksFrameworkNameValueIsValid_ReturnsFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.ParseFolder("net472"));
            var json = $"{{\"restore\":{{\"frameworks\":{{\"{expectedResult.FrameworkName.GetShortFolderName()}\":{{}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Collection(
                packageSpec.RestoreMetadata.TargetFrameworks,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreFrameworksFrameworkValueHasProjectReferenceWithoutAssets_ReturnsFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            var projectReference = new ProjectRestoreReference()
            {
                ProjectUniqueName = "a",
                ProjectPath = "b"
            };
            var expectedResult = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.ParseFolder("net472"));

            expectedResult.ProjectReferences.Add(projectReference);

            var json = $"{{\"restore\":{{\"frameworks\":{{\"{expectedResult.FrameworkName.GetShortFolderName()}\":{{\"projectReferences\":{{" +
                $"\"{projectReference.ProjectUniqueName}\":{{\"projectPath\":\"{projectReference.ProjectPath}\"}}}}}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Collection(
                packageSpec.RestoreMetadata.TargetFrameworks,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreFrameworksFrameworkValueHasProjectReferenceWithAssets_ReturnsFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            var projectReference = new ProjectRestoreReference()
            {
                ProjectUniqueName = "a",
                ProjectPath = "b",
                IncludeAssets = LibraryIncludeFlags.Analyzers,
                ExcludeAssets = LibraryIncludeFlags.Native,
                PrivateAssets = LibraryIncludeFlags.Runtime
            };
            var expectedResult = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.ParseFolder("net472"));

            expectedResult.ProjectReferences.Add(projectReference);

            var json = $"{{\"restore\":{{\"frameworks\":{{\"{expectedResult.FrameworkName.GetShortFolderName()}\":{{\"projectReferences\":{{" +
                $"\"{projectReference.ProjectUniqueName}\":{{\"projectPath\":\"{projectReference.ProjectPath}\"," +
                $"\"includeAssets\":\"{projectReference.IncludeAssets}\",\"excludeAssets\":\"{projectReference.ExcludeAssets}\"," +
                $"\"privateAssets\":\"{projectReference.PrivateAssets}\"}}}}}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Collection(
                packageSpec.RestoreMetadata.TargetFrameworks,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreConfigFilePathsValueIsEmptyArray_ReturnsEmptyConfigFilePaths(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{\"configFilePaths\":[]}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Empty(packageSpec.RestoreMetadata.ConfigFilePaths);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreConfigFilePathsValueIsValid_ReturnsConfigFilePaths(IEnvironmentVariableReader environmentVariableReader)
        {
            string[] expectedResults = { "a", "b" };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""));
            var json = $"{{\"restore\":{{\"configFilePaths\":[{values}]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.ConfigFilePaths);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreFallbackFoldersValueIsEmptyArray_ReturnsEmptyFallbackFolders(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{\"fallbackFolders\":[]}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Empty(packageSpec.RestoreMetadata.FallbackFolders);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreFallbackFoldersValueIsValid_ReturnsConfigFilePaths(IEnvironmentVariableReader environmentVariableReader)
        {
            string[] expectedResults = { "a", "b" };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""));
            var json = $"{{\"restore\":{{\"fallbackFolders\":[{values}]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.FallbackFolders);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreOriginalTargetFrameworksValueIsEmptyArray_ReturnsEmptyOriginalTargetFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            const string json = "{\"restore\":{\"originalTargetFrameworks\":[]}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Empty(packageSpec.RestoreMetadata.OriginalTargetFrameworks);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreOriginalTargetFrameworksValueIsValid_ReturnsOriginalTargetFrameworks(IEnvironmentVariableReader environmentVariableReader)
        {
            string[] expectedResults = { "a", "b" };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""));
            var json = $"{{\"restore\":{{\"originalTargetFrameworks\":[{values}]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.OriginalTargetFrameworks);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreWarningPropertiesValueIsEmptyObject_ReturnsWarningProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new WarningProperties();
            const string json = "{\"restore\":{\"warningProperties\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.ProjectWideWarningProperties);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreWarningPropertiesValueIsValid_ReturnsWarningProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new WarningProperties(
                new HashSet<NuGetLogCode>() { NuGetLogCode.NU3000 },
                new HashSet<NuGetLogCode>() { NuGetLogCode.NU3001 },
                allWarningsAsErrors: true,
                new HashSet<NuGetLogCode>());
            var json = $"{{\"restore\":{{\"warningProperties\":{{\"allWarningsAsErrors\":{expectedResult.AllWarningsAsErrors.ToString().ToLowerInvariant()}," +
                $"\"warnAsError\":[\"{expectedResult.WarningsAsErrors.Single()}\"],\"noWarn\":[\"{expectedResult.NoWarn.Single()}\"]}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.ProjectWideWarningProperties);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreRestoreLockPropertiesValueIsEmptyObject_ReturnsRestoreLockProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new RestoreLockProperties();
            const string json = "{\"restore\":{\"restoreLockProperties\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.RestoreLockProperties);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreRestoreLockPropertiesValueIsValid_ReturnsRestoreLockProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new RestoreLockProperties(
                restorePackagesWithLockFile: "a",
                nuGetLockFilePath: "b",
                restoreLockedMode: true); ;
            var json = $"{{\"restore\":{{\"restoreLockProperties\":{{\"restoreLockedMode\":{expectedResult.RestoreLockedMode.ToString().ToLowerInvariant()}," +
                $"\"restorePackagesWithLockFile\":\"{expectedResult.RestorePackagesWithLockFile}\"," +
                $"\"nuGetLockFilePath\":\"{expectedResult.NuGetLockFilePath}\"}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.RestoreLockProperties);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestorePackagesConfigPathValueIsValidAndProjectStyleValueIsNotPackagesConfig_DoesNotReturnPackagesConfigPath(IEnvironmentVariableReader environmentVariableReader, string value)
        {
            var json = $"{{\"restore\":{{\"projectStyle\":\"PackageReference\",\"packagesConfigPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.IsNotType<PackagesConfigProjectRestoreMetadata>(packageSpec.RestoreMetadata);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", null, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", "", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"a\"", "a", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestorePackagesConfigPathValueIsValidAndProjectStyleValueIsPackagesConfig_ReturnsPackagesConfigPath(
            IEnvironmentVariableReader environmentVariableReader,
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectStyle\":\"PackagesConfig\",\"packagesConfigPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.IsType<PackagesConfigProjectRestoreMetadata>(packageSpec.RestoreMetadata);
            Assert.Equal(expectedValue, ((PackagesConfigProjectRestoreMetadata)packageSpec.RestoreMetadata).PackagesConfigPath);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRestoreSettingsValueIsEmptyObject_ReturnsRestoreSettings(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = new ProjectRestoreSettings();
            const string json = "{\"restoreSettings\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RestoreSettings);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRuntimesValueIsEmptyObject_ReturnsRuntimes(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = RuntimeGraph.Empty;
            const string json = "{\"runtimes\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRuntimesValueIsValidWithImports_ReturnsRuntimes(IEnvironmentVariableReader environmentVariableReader)
        {
            var runtimeDescription = new RuntimeDescription(
                runtimeIdentifier: "a",
                inheritedRuntimes: new[] { "b", "c" },
                Enumerable.Empty<RuntimeDependencySet>());
            var expectedResult = new RuntimeGraph(new[] { runtimeDescription });
            var json = $"{{\"runtimes\":{{\"{runtimeDescription.RuntimeIdentifier}\":{{\"#import\":[" +
                $"{string.Join(",", runtimeDescription.InheritedRuntimes.Select(runtime => $"\"{runtime}\""))}]}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRuntimesValueIsValidWithDependencySet_ReturnsRuntimes(IEnvironmentVariableReader environmentVariableReader)
        {
            var dependencySet = new RuntimeDependencySet(id: "b");
            var runtimeDescription = new RuntimeDescription(
                runtimeIdentifier: "a",
                inheritedRuntimes: Enumerable.Empty<string>(),
                runtimeDependencySets: new[] { dependencySet });
            var expectedResult = new RuntimeGraph(new[] { runtimeDescription });
            var json = $"{{\"runtimes\":{{\"{runtimeDescription.RuntimeIdentifier}\":{{\"{dependencySet.Id}\":{{}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenRuntimesValueIsValidWithDependencySetWithDependency_ReturnsRuntimes(IEnvironmentVariableReader environmentVariableReader)
        {
            var dependency = new RuntimePackageDependency("c", VersionRange.Parse("[1.2.3,4.5.6)"));
            var dependencySet = new RuntimeDependencySet(id: "b", new[] { dependency });
            var runtimeDescription = new RuntimeDescription(
                runtimeIdentifier: "a",
                inheritedRuntimes: Enumerable.Empty<string>(),
                runtimeDependencySets: new[] { dependencySet });
            var expectedResult = new RuntimeGraph(new[] { runtimeDescription });
            var json = $"{{\"runtimes\":{{\"{runtimeDescription.RuntimeIdentifier}\":{{\"{dependencySet.Id}\":{{" +
                $"\"{dependency.Id}\":\"{dependency.VersionRange.ToLegacyString()}\"}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenSupportsValueIsEmptyObject_ReturnsSupports(IEnvironmentVariableReader environmentVariableReader)
        {
            var expectedResult = RuntimeGraph.Empty;
            const string json = "{\"supports\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenSupportsValueIsValidWithCompatibilityProfiles_ReturnsSupports(IEnvironmentVariableReader environmentVariableReader)
        {
            var profile = new CompatibilityProfile(name: "a");
            var expectedResult = new RuntimeGraph(new[] { profile });
            var json = $"{{\"supports\":{{\"{profile.Name}\":{{}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenSupportsValueIsValidWithCompatibilityProfilesAndFrameworkRuntimePairs_ReturnsSupports(IEnvironmentVariableReader environmentVariableReader)
        {
            FrameworkRuntimePair[] restoreContexts = new[]
            {
                        new FrameworkRuntimePair(NuGetFramework.Parse("net472"), "b"),
                        new FrameworkRuntimePair(NuGetFramework.Parse("net48"), "c")
                    };
            var profile = new CompatibilityProfile(name: "a", restoreContexts);
            var expectedResult = new RuntimeGraph(new[] { profile });
            var json = $"{{\"supports\":{{\"{profile.Name}\":{{" +
                $"\"{restoreContexts[0].Framework.GetShortFolderName()}\":\"{restoreContexts[0].RuntimeIdentifier}\"," +
                $"\"{restoreContexts[1].Framework.GetShortFolderName()}\":[\"{restoreContexts[1].RuntimeIdentifier}\"]}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenNameIsNull_RestoreMetadataProvidesFallbackName(IEnvironmentVariableReader environmentVariableReader)
        {
            const string expectedResult = "a";
            var json = $"{{\"restore\":{{\"projectName\":\"{expectedResult}\"}}}}";

            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.Name);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "{\"restore\":{\"projectJsonPath\":\"a\"}}", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "{\"restore\":{\"projectPath\":\"a\"}}", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "{\"restore\":{\"projectJsonPath\":\"a\",\"projectPath\":\"b\"}}", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFilePathIsNull_RestoreMetadataProvidesFallbackFilePath(IEnvironmentVariableReader environmentVariableReader, string json)
        {
            const string expectedResult = "a";

            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            Assert.Equal(expectedResult, packageSpec.FilePath);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetTargetFrameworkInformation_WithAnAlias(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"net46\":{ \"targetAlias\" : \"alias\"}}}", environmentVariableReader);

            Assert.Equal("alias", framework.TargetAlias);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_ReadsRestoreMetadataWithAliases(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{  
                                    ""restore"": {
            ""projectUniqueName"": ""projectUniqueName"",
            ""projectName"": ""projectName"",
            ""projectPath"": ""projectPath"",
            ""projectJsonPath"": ""projectJsonPath"",
            ""packagesPath"": ""packagesPath"",
            ""outputPath"": ""outputPath"",
            ""projectStyle"": ""PackageReference"",
            ""crossTargeting"": true,
            ""frameworks"": {
              ""frameworkidentifier123-frameworkprofile"": {
                ""targetAlias"" : ""alias"",
                ""projectReferences"": {}
              }
            },
            ""warningProperties"": {
            }
          }
        }";

            var actual = GetPackageSpec(json, "TestProject", "project.json", null, environmentVariableReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.Equal("alias", metadata.TargetFrameworks.Single().TargetAlias);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_Read(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"{
                                    ""centralTransitiveDependencyGroups"": {
                                            "".NETCoreApp,Version=v3.1"": {
                                                ""Foo"": {
                                                    ""exclude"": ""Native"",
                                                    ""include"": ""Build"",
                                                    ""suppressParent"": ""All"",
                                                    ""version"": ""1.0.0""
                                            }
                                        },
                                            "".NETCoreApp,Version=v3.0"": {
                                                ""Bar"": {
                                                    ""exclude"": ""Native"",
                                                    ""include"": ""Build"",
                                                    ""suppressParent"": ""All"",
                                                    ""version"": ""2.0.0""
                                            }
                                        }
                                    }
                                }";

            // Act
            var results = new List<CentralTransitiveDependencyGroup>();
            if (environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING).Equals(bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var reader = new Utf8JsonStreamReader(stream);

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                    {
                        if (reader.Read() && reader.TokenType == JsonTokenType.StartObject)
                        {
                            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                            {
                                var frameworkPropertyName = reader.GetString();
                                NuGetFramework framework = NuGetFramework.Parse(frameworkPropertyName);

                                JsonPackageSpecReader.ReadCentralTransitiveDependencyGroup(
                                    jsonReader: ref reader,
                                    results: out var dependencies,
                                    packageSpecPath: "SomePath");
                                results.Add(new CentralTransitiveDependencyGroup(framework, dependencies));
                            }
                        }
                    }
                }
            }
            else
            {
                using (var stringReader = new StringReader(json.ToString()))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    jsonReader.ReadObject(ctdPropertyName =>
                    {
                        jsonReader.ReadObject(frameworkPropertyName =>
                        {
                            var dependencies = new List<LibraryDependency>();
                            NuGetFramework framework = NuGetFramework.Parse(frameworkPropertyName);
                            JsonPackageSpecReader.ReadCentralTransitiveDependencyGroup(
                                jsonReader: jsonReader,
                                results: dependencies,
                                packageSpecPath: "SomePath");
                            results.Add(new CentralTransitiveDependencyGroup(framework, dependencies));
                        });
                    });
                }
            }

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(".NETCoreApp,Version=v3.1", results.ElementAt(0).FrameworkName);
            var firstGroup = results.ElementAt(0);
            Assert.Equal(1, firstGroup.TransitiveDependencies.Count());
            Assert.Equal("Build", firstGroup.TransitiveDependencies.First().IncludeType.ToString());
            Assert.Equal("All", firstGroup.TransitiveDependencies.First().SuppressParent.ToString());
            Assert.Equal("[1.0.0, )", firstGroup.TransitiveDependencies.First().LibraryRange.VersionRange.ToNormalizedString());
            Assert.True(firstGroup.TransitiveDependencies.First().VersionCentrallyManaged);

            var secondGroup = results.ElementAt(1);
            Assert.Equal(1, secondGroup.TransitiveDependencies.Count());
            Assert.Equal("Build", secondGroup.TransitiveDependencies.First().IncludeType.ToString());
            Assert.Equal("All", secondGroup.TransitiveDependencies.First().SuppressParent.ToString());
            Assert.Equal("[2.0.0, )", secondGroup.TransitiveDependencies.First().LibraryRange.VersionRange.ToNormalizedString());
            Assert.True(secondGroup.TransitiveDependencies.First().VersionCentrallyManaged);
        }


        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void PackageSpecReader_Malformed_Exception(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = @"
{
    "".NETCoreApp,Version=v3.1"": {
        ""Foo"":";

            // Act
            var results = new List<CentralTransitiveDependencyGroup>();
            if (environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING).Equals(bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                Assert.ThrowsAny<System.Text.Json.JsonException>(() =>
                {
                    using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                    var reader = new Utf8JsonStreamReader(stream);

                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        reader.Read();
                        NuGetFramework framework = NuGetFramework.Parse(reader.GetString());

                        JsonPackageSpecReader.ReadCentralTransitiveDependencyGroup(
                            jsonReader: ref reader,
                            results: out var dependencies,
                            packageSpecPath: "SomePath");
                        results.Add(new CentralTransitiveDependencyGroup(framework, dependencies));
                    }
                });
            }
            else
            {
                using (var stringReader = new StringReader(json.ToString()))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    jsonReader.Read();
                    jsonReader.Read();
                    var dependencies = new List<LibraryDependency>();
                    NuGetFramework framework = NuGetFramework.Parse((string)jsonReader.Value);
                    JsonPackageSpecReader.ReadCentralTransitiveDependencyGroup(
                        jsonReader: jsonReader,
                        results: dependencies,
                        packageSpecPath: "SomePath");
                    results.Add(new CentralTransitiveDependencyGroup(framework, dependencies));
                }
                // Assert
                Assert.Equal(1, results.Count);
                var firstGroup = results.ElementAt(0);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithSecondaryFrameworks_ReturnsTargetFrameworkInformationWithDualCompatibilityFramework(IEnvironmentVariableReader environmentVariableReader)
        {
            var json = $"{{\"frameworks\":{{\"net5.0\":{{\"secondaryFramework\": \"native\"}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);
            framework.FrameworkName.Should().BeOfType<DualCompatibilityFramework>();
            var dualCompatibilityFramework = framework.FrameworkName as DualCompatibilityFramework;
            dualCompatibilityFramework.RootFramework.Should().Be(FrameworkConstants.CommonFrameworks.Net50);
            dualCompatibilityFramework.SecondaryFramework.Should().Be(FrameworkConstants.CommonFrameworks.Native);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithAssetTargetFallbackAndWithSecondaryFrameworks_ReturnsTargetFrameworkInformationWithDualCompatibilityFramework(IEnvironmentVariableReader environmentVariableReader)
        {
            var json = $"{{\"frameworks\":{{\"net5.0\":{{\"assetTargetFallback\": true, \"imports\": [\"net472\", \"net471\"], \"secondaryFramework\": \"native\" }}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);
            framework.FrameworkName.Should().BeOfType<AssetTargetFallbackFramework>();
            framework.AssetTargetFallback.Should().BeTrue();
            var assetTargetFallbackFramework = framework.FrameworkName as AssetTargetFallbackFramework;
            assetTargetFallbackFramework.RootFramework.Should().BeOfType<DualCompatibilityFramework>();
            var dualCompatibilityFramework = assetTargetFallbackFramework.RootFramework as DualCompatibilityFramework;
            dualCompatibilityFramework.RootFramework.Should().Be(FrameworkConstants.CommonFrameworks.Net50);
            dualCompatibilityFramework.SecondaryFramework.Should().Be(FrameworkConstants.CommonFrameworks.Native);
            assetTargetFallbackFramework.Fallback.Should().HaveCount(2);
            assetTargetFallbackFramework.Fallback.First().Should().Be(FrameworkConstants.CommonFrameworks.Net472);
            assetTargetFallbackFramework.Fallback.Last().Should().Be(FrameworkConstants.CommonFrameworks.Net471);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithRestoreAuditProperties_ReturnsRestoreAuditProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = $"{{\"restore\":{{\"restoreAuditProperties\":{{\"enableAudit\": \"a\", \"auditLevel\": \"b\", \"auditMode\": \"c\"}}}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            // Assert
            packageSpec.RestoreMetadata.RestoreAuditProperties.EnableAudit.Should().Be("a");
            packageSpec.RestoreMetadata.RestoreAuditProperties.AuditLevel.Should().Be("b");
            packageSpec.RestoreMetadata.RestoreAuditProperties.AuditMode.Should().Be("c");
            packageSpec.RestoreMetadata.RestoreAuditProperties.SuppressedAdvisories.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithRestoreAuditPropertiesAndSuppressions_ReturnsRestoreAuditProperties(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var json = $"{{\"restore\":{{\"restoreAuditProperties\":{{\"enableAudit\":\"a\",\"auditLevel\":\"b\",\"auditMode\":\"c\",\"suppressedAdvisories\":{{\"d\":null,\"e\":null}}}}}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            // Assert
            packageSpec.RestoreMetadata.RestoreAuditProperties.EnableAudit.Should().Be("a");
            packageSpec.RestoreMetadata.RestoreAuditProperties.AuditLevel.Should().Be("b");
            packageSpec.RestoreMetadata.RestoreAuditProperties.AuditMode.Should().Be("c");
            packageSpec.RestoreMetadata.RestoreAuditProperties.SuppressedAdvisories.Should().HaveCount(2);
            packageSpec.RestoreMetadata.RestoreAuditProperties.SuppressedAdvisories.First().Should().Be("d");
            packageSpec.RestoreMetadata.RestoreAuditProperties.SuppressedAdvisories.Last().Should().Be("e");
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "9.0.100", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "10.0.100", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "8.1.100", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithSdkAnalysisLevelValue_ReturnsSdkAnalysisLevel(
            IEnvironmentVariableReader environmentVariableReader,
            string version)
        {
            // Arrange
            NuGetVersion expectedNugetVersion = new NuGetVersion(version);
            var json = $"{{\"restore\":{{\"SdkAnalysisLevel\":\"{version}\"}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            // Assert
            Assert.Equal(expectedNugetVersion, packageSpec.RestoreMetadata.SdkAnalysisLevel);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "notGood", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "10invalid", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithAnInvalidSdkAnalysisLevelValue_ThrowsAnException(
            IEnvironmentVariableReader environmentVariableReader,
            string version)
        {
            // Arrange
            var json = $"{{\"restore\":{{\"SdkAnalysisLevel\":\"{version}\"}}}}";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GetPackageSpec(json, environmentVariableReader));
            Assert.Contains("SdkAnalysisLevel", ex.Message);
            Assert.Contains(version, ex.Message);
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithUsingMicrosoftNetSdk_ReturnsUsingMicrosoftNetSdk(
            IEnvironmentVariableReader environmentVariableReader,
            bool isSdk)
        {
            // Arrange
            var json = $"{{\"restore\":{{\"UsingMicrosoftNETSdk\":{isSdk.ToString().ToLower()}}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            // Assert
            Assert.Equal(isSdk, packageSpec.RestoreMetadata.UsingMicrosoftNETSdk);
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13849")]
        public void GetPackageSpec_WithInvalidUsingMicrosoftNetSdk_ThrowsAnException()
        {
            // Arrange
            var json = $"{{\"restore\":{{\"UsingMicrosoftNETSdk\":1}}}}";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GetPackageSpec(json));
            Assert.Contains("UsingMicrosoftNETSdk", ex.Message);
            Assert.Contains("1", ex.Message);
        }

        [Fact]
        public void GetPackageSpec_WithNoUsingMicrosoftNetSdkValuePassed_defaultsTrue()
        {
            // Arrange
            var json = $"{{\"restore\":{{}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json);

            // Assert
            Assert.True(packageSpec.RestoreMetadata.UsingMicrosoftNETSdk);
        }

        [Fact]
        public void GetPackageSpec_RestoreMetadataWithoutMacros_WithMacrosEnabled()
        {
            // Arrange
            var json = @"{  
                            ""restore"": {
    ""projectUniqueName"": ""C:\\Users\\me\\source\\code\\project.csproj"",
    ""projectName"": ""project"",
    ""projectPath"": ""C:\\Users\\me\\source\\code\\project.csproj"",
    ""projectJsonPath"": ""C:\\Users\\me\\source\\code\\project.json"",
    ""packagesPath"": ""C:\\Users\\me\\.nuget\\packages"",
    ""outputPath"": ""C:\\Users\\me\\source\\code\\obj"",
    ""projectStyle"": ""PackageReference"",
    ""crossTargeting"": true,
    ""configFilePaths"": [
        ""C:\\Users\\me\\source\\code\\NuGet.Config"",
        ""C:\\Users\\me\\AppData\\Roaming\\NuGet\\NuGet.Config"",
        ""C:\\Program Files (x86)\\NuGet\\Config\\Microsoft.VisualStudio.FallbackLocation.config"",
        ""C:\\Program Files (x86)\\NuGet\\Config\\Microsoft.VisualStudio.Offline.config""
    ],
    ""fallbackFolders"": [
        ""C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder"",
        ""C:\\Users\\me\\fallbackFolder""


    ]
  }
}";
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
                {
                    { MacroStringsUtility.NUGET_ENABLE_EXPERIMENTAL_MACROS, "true" }
            });

            PackageSpec actual = PackageSpecTestUtility.GetPackageSpec(json, environmentReader);

            // Assert
            var metadata = actual.RestoreMetadata;

            Assert.NotNull(metadata);
            metadata.ProjectUniqueName.Should().Be(@"C:\Users\me\source\code\project.csproj");
            metadata.ProjectPath.Should().Be(@"C:\Users\me\source\code\project.csproj");
            metadata.ProjectJsonPath.Should().Be(@"C:\Users\me\source\code\project.json");
            metadata.PackagesPath.Should().Be(@"C:\Users\me\.nuget\packages");
            metadata.OutputPath.Should().Be(@"C:\Users\me\source\code\obj");

            metadata.ConfigFilePaths.Should().Contain(@"C:\Users\me\source\code\NuGet.Config");
            metadata.ConfigFilePaths.Should().Contain(@"C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.FallbackLocation.config");
            metadata.ConfigFilePaths.Should().Contain(@"C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config");
            metadata.ConfigFilePaths.Should().Contain(@"C:\Users\me\AppData\Roaming\NuGet\NuGet.Config");

            metadata.FallbackFolders.Should().Contain(@"C:\Program Files\dotnet\sdk\NuGetFallbackFolder");
            metadata.FallbackFolders.Should().Contain(@"C:\Users\me\fallbackFolder");
        }

        [Fact]
        public void GetPackageSpec_RestoreMetadataWithMacros()
        {
            // Arrange
            var json = @"{  
                            ""restore"": {
    ""projectUniqueName"": ""C:\\users\\me\\source\\code\\project.csproj"",
    ""projectName"": ""project"",
    ""projectPath"": ""C:\\users\\me\\source\\code\\project.csproj"",
    ""projectJsonPath"": ""C:\\users\\me\\source\\code\\project.json"",
    ""packagesPath"": ""$(User).nuget\\packages"",
    ""outputPath"": ""C:\\users\\me\\source\\code\\obj"",
    ""projectStyle"": ""PackageReference"",
    ""crossTargeting"": true,
    ""configFilePaths"": [
        ""$(User)source\\code\\NuGet.Config"",
        ""$(User)AppData\\Roaming\\NuGet\\NuGet.Config"",
        ""C:\\Program Files (x86)\\NuGet\\Config\\Microsoft.VisualStudio.FallbackLocation.config"",
        ""C:\\Program Files (x86)\\NuGet\\Config\\Microsoft.VisualStudio.Offline.config""
    ],
    ""fallbackFolders"": [
        ""C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder"",
        ""$(User)fallbackFolder""


    ]
  }
}";
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
                {
                    { MacroStringsUtility.NUGET_ENABLE_EXPERIMENTAL_MACROS, "true" }
            });

            PackageSpec actual = PackageSpecTestUtility.GetPackageSpec(json, environmentReader);

            // Assert
            var metadata = actual.RestoreMetadata;
            var userSettingsDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);

            Assert.NotNull(metadata);
            metadata.PackagesPath.Should().Be(@$"{userSettingsDirectory}.nuget\packages");

            metadata.ConfigFilePaths.Should().Contain(@$"{userSettingsDirectory}source\code\NuGet.Config");
            metadata.ConfigFilePaths.Should().Contain(@"C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.FallbackLocation.config");
            metadata.ConfigFilePaths.Should().Contain(@"C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config");
            metadata.ConfigFilePaths.Should().Contain(@$"{userSettingsDirectory}AppData\Roaming\NuGet\NuGet.Config");

            metadata.FallbackFolders.Should().Contain(@"C:\Program Files\dotnet\sdk\NuGetFallbackFolder");
            metadata.FallbackFolders.Should().Contain(@$"{userSettingsDirectory}fallbackFolder");
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), true, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), false, MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WithRestoreUseLegacyDependencyResolver_ReturnsUseLegacyDependencyResolver(
            IEnvironmentVariableReader environmentVariableReader,
            bool useLegacyDependencyResolver)
        {
            // Arrange
            var json = $"{{\"restore\":{{\"restoreUseLegacyDependencyResolver\":{useLegacyDependencyResolver.ToString().ToLowerInvariant()}}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            // Assert
            packageSpec.RestoreMetadata.UseLegacyDependencyResolver.Should().Be(useLegacyDependencyResolver);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksPackagesToPrunePropertyIsAbsent_ReturnsEmptyPackagesToPrune(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}", environmentVariableReader);

            Assert.Empty(framework.PackagesToPrune);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksPackagesToPruneValueIsEmptyObject_ReturnsEmptyPackagesToPrune(IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{\"packagesToPrune\":{}}}}", environmentVariableReader);

            Assert.Empty(framework.PackagesToPrune);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksPackagesToPruneVersionPropertyNameIsEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader)
        {
            var json = "{\"frameworks\":{\"a\":{\"packagesToPrune\":{\"\":\"1.0.0\"}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve package to prune version ''.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : Unable to resolve package to prune ''.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(TestEnvironmentVariableReader), "null", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        [MemberData(nameof(TestEnvironmentVariableReader), "\"\"", MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksPackagesToPruneVersionPropertyValueIsNullOrEmptyString_Throws(IEnvironmentVariableReader environmentVariableReader, string value)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"packagesToPrune\":{{\"b\":{value}}}}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json, environmentVariableReader));

            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal("Error reading '' at line 1 column 20 : The version cannot be null or empty.", exception.Message);
                Assert.Equal(1, exception.Line);
                Assert.Equal(20, exception.Column);
            }
            else
            {
                Assert.Equal("Error reading '' : The version cannot be null or empty.", exception.Message);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void GetPackageSpec_WhenFrameworksPackagesToPruneIsValid_ReturnsPackagesToPrune(IEnvironmentVariableReader environmentVariableReader)
        {
            const string expectedPackageId = "b";
            VersionRange expectedVersionRange = VersionRange.Parse("[1.2.3,4.5.6)");
            var expectedPackageToPrune = new PrunePackageReference(expectedPackageId, expectedVersionRange);
            var json = $"{{\"frameworks\":{{\"a\":{{\"packagesToPrune\":{{\"{expectedPackageId}\":\"{expectedVersionRange.ToShortString()}\"}}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            Assert.Collection(
                framework.PackagesToPrune,
                actualResult =>
                {
                    Assert.Equal(expectedPackageId, actualResult.Key);
                    Assert.Equal(expectedPackageToPrune, actualResult.Value);
                });
        }

        private static PackageSpec GetPackageSpec(string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return JsonPackageSpecReader.GetPackageSpec(stream, name: null, packageSpecPath: null, snapshotValue: null);
            }
        }

        private static PackageSpec GetPackageSpec(string json, IEnvironmentVariableReader environmentVariableReader)
        {
            return GetPackageSpec(json, name: null, packageSpecPath: null, snapshotValue: null, environmentVariableReader: environmentVariableReader);
        }

        private static PackageSpec GetPackageSpec(string json, string name, string packageSpecPath, string snapshotValue, IEnvironmentVariableReader environmentVariableReader)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return JsonPackageSpecReader.GetPackageSpec(stream, name, packageSpecPath, snapshotValue, environmentVariableReader, true);
        }

        private static LibraryDependency GetDependency(string json, IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            return packageSpec.Dependencies.Single();
        }

        private static TargetFrameworkInformation GetFramework(string json, IEnvironmentVariableReader environmentVariableReader)
        {
            PackageSpec packageSpec = GetPackageSpec(json, environmentVariableReader);

            return packageSpec.TargetFrameworks.Single();
        }

        private static LibraryDependency GetFrameworksDependency(string json, IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            return framework.Dependencies.Single();
        }

        private static FrameworkDependency GetFrameworksFrameworkReference(string json, IEnvironmentVariableReader environmentVariableReader)
        {
            TargetFrameworkInformation framework = GetFramework(json, environmentVariableReader);

            return framework.FrameworkReferences.Single();
        }
    }
}
