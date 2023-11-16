// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    [UseCulture("")] // Fix tests failing on systems with non-English locales
    public class JsonPackageSpecReaderTests
    {
        [Fact]
        public void PackageSpecReader_PackageMissingVersion()
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
                var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Contains("specify a version range", exception.Message);
        }

        [Fact]
        public void PackageSpecReader_ProjectMissingVersion()
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
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var range = spec.Dependencies.Single().LibraryRange.VersionRange;

            // Assert
            Assert.Equal(VersionRange.All, range);
        }

        [Fact]
        public void PackageSpecReader_PackageEmptyVersion()
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
                var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Contains("specify a version range", exception.Message);
        }

        [Fact]
        public void PackageSpecReader_PackageWhitespaceVersion()
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
                var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.Contains("not a valid version string", exception.Message);
        }

        [Fact]
        public void PackageSpecReader_FrameworkAssemblyEmptyVersion()
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
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var range = spec.TargetFrameworks.Single().Dependencies.Single().LibraryRange.VersionRange;

            // Assert
            Assert.Equal(VersionRange.All, range);
        }

        [Fact]
        public void PackageSpecReader_ExplicitIncludesOverrideTypePlatform()
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
            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("redist"));
            Assert.NotNull(dep);

            var expected = LibraryIncludeFlags.Analyzers;
            Assert.Equal(expected, dep.IncludeType);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData(@"{
                        ""packOptions"": {}
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""foo"": [1, 2]
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": null
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": []
                        }
                      }")]
#pragma warning disable CS0612 // Type or member is obsolete
        public void PackageSpecReader_PackOptions_Default(string json)
        {
            // Arrange & Act
            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            Assert.NotNull(actual.PackOptions);
            Assert.NotNull(actual.PackOptions.PackageType);
            Assert.Empty(actual.PackOptions.PackageType);
        }

        [Theory]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": ""foo""
                        }
                      }", new[] { "foo" })]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": ""foo, bar""
                        }
                      }", new[] { "foo, bar" })]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": [ ""foo"" ]
                        }
                      }", new[] { "foo" })]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": [ ""foo, bar"" ]
                        }
                      }", new[] { "foo, bar" })]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": [ ""foo"", ""bar"" ]
                        }
                      }", new[] { "foo", "bar" })]
        public void PackageSpecReader_PackOptions_ValidPackageType(string json, string[] expectedNames)
        {
            // Arrange
            var expected = expectedNames
                .Select(n => new PackageType(n, PackageType.EmptyVersion))
                .ToArray();

            // Act
            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            Assert.NotNull(actual.PackOptions);
            Assert.NotNull(actual.PackOptions.PackageType);
            Assert.Equal(expected, actual.PackOptions.PackageType.ToArray());
        }

        [Theory]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": 1
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": false
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": 1.0
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": {}
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": {
                            ""name"": ""foo""
                          }
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": [
                            { ""name"": ""foo"" },
                            { ""name"": ""bar"" }
                          ]
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": [
                            ""foo"",
                            null
                          ]
                        }
                      }")]
        [InlineData(@"{
                        ""packOptions"": {
                          ""packageType"": [
                            ""foo"",
                            true
                          ]
                        }
                      }")]
        public void PackageSpecReader_PackOptions_InvalidPackageType(string json)
        {
            // Arrange & Act & Assert
            var actual = Assert.Throws<FileFormatException>(
                () => JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json"));

            Assert.Contains("The pack options package type must be a string or array of strings in 'project.json'.", actual.Message);
        }

        [Fact]
        public void PackageSpecReader_PackOptions_Files1()
        {
            // Arrange & Act
            var json = @"{
                        ""packOptions"": {
                          ""files"": {
                            ""include"": ""file1"",
                            ""exclude"": ""file2"",
                            ""includeFiles"": ""file3"",
                            ""excludeFiles"": ""file4"",
                            ""mappings"": {
                              ""dest/path"": ""./src/path""
                            }
                          }
                        }
                      }";
            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            Assert.NotNull(actual.PackOptions);
            Assert.Equal(1, actual.PackOptions.IncludeExcludeFiles.Include.Count);
            Assert.Equal(1, actual.PackOptions.IncludeExcludeFiles.Exclude.Count);
            Assert.Equal(1, actual.PackOptions.IncludeExcludeFiles.IncludeFiles.Count);
            Assert.Equal(1, actual.PackOptions.IncludeExcludeFiles.ExcludeFiles.Count);
            Assert.Equal("file1", actual.PackOptions.IncludeExcludeFiles.Include.First());
            Assert.Equal("file2", actual.PackOptions.IncludeExcludeFiles.Exclude.First());
            Assert.Equal("file3", actual.PackOptions.IncludeExcludeFiles.IncludeFiles.First());
            Assert.Equal("file4", actual.PackOptions.IncludeExcludeFiles.ExcludeFiles.First());
            Assert.NotNull(actual.PackOptions.Mappings);
            Assert.Equal(1, actual.PackOptions.Mappings.Count());
            Assert.Equal("dest/path", actual.PackOptions.Mappings.First().Key);
            Assert.Equal(1, actual.PackOptions.Mappings.First().Value.Include.Count());
            Assert.Null(actual.PackOptions.Mappings.First().Value.Exclude);
            Assert.Null(actual.PackOptions.Mappings.First().Value.IncludeFiles);
            Assert.Null(actual.PackOptions.Mappings.First().Value.ExcludeFiles);
            Assert.Equal("./src/path", actual.PackOptions.Mappings.First().Value.Include.First());
        }

        [Fact]
        public void PackageSpecReader_PackOptions_Files2()
        {
            // Arrange & Act
            var json = @"{
                        ""packOptions"": {
                          ""files"": {
                            ""include"": [""file1a"", ""file1b""],
                            ""exclude"": [""file2a"", ""file2b""],
                            ""includeFiles"": [""file3a"", ""file3b""],
                            ""excludeFiles"": [""file4a"", ""file4b""],
                            ""mappings"": {
                              ""dest/path1"": [""./src/path1"", ""./src/path2""],
                              ""dest/path2"": {
                                ""includeFiles"": [""map1a"", ""map1b""],
                              },
                            }
                          }
                        }
                      }";
            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            Assert.NotNull(actual.PackOptions);
            Assert.Equal(2, actual.PackOptions.IncludeExcludeFiles.Include.Count);
            Assert.Equal(2, actual.PackOptions.IncludeExcludeFiles.Exclude.Count);
            Assert.Equal(2, actual.PackOptions.IncludeExcludeFiles.IncludeFiles.Count);
            Assert.Equal(2, actual.PackOptions.IncludeExcludeFiles.ExcludeFiles.Count);
            Assert.Equal("file1a", actual.PackOptions.IncludeExcludeFiles.Include.First());
            Assert.Equal("file2a", actual.PackOptions.IncludeExcludeFiles.Exclude.First());
            Assert.Equal("file3a", actual.PackOptions.IncludeExcludeFiles.IncludeFiles.First());
            Assert.Equal("file4a", actual.PackOptions.IncludeExcludeFiles.ExcludeFiles.First());
            Assert.Equal("file1b", actual.PackOptions.IncludeExcludeFiles.Include.Last());
            Assert.Equal("file2b", actual.PackOptions.IncludeExcludeFiles.Exclude.Last());
            Assert.Equal("file3b", actual.PackOptions.IncludeExcludeFiles.IncludeFiles.Last());
            Assert.Equal("file4b", actual.PackOptions.IncludeExcludeFiles.ExcludeFiles.Last());
            Assert.NotNull(actual.PackOptions.Mappings);
            Assert.Equal(2, actual.PackOptions.Mappings.Count());
            Assert.Equal("dest/path1", actual.PackOptions.Mappings.First().Key);
            Assert.Equal("dest/path2", actual.PackOptions.Mappings.Last().Key);
            Assert.Equal(2, actual.PackOptions.Mappings.First().Value.Include.Count());
            Assert.Null(actual.PackOptions.Mappings.First().Value.Exclude);
            Assert.Null(actual.PackOptions.Mappings.First().Value.IncludeFiles);
            Assert.Null(actual.PackOptions.Mappings.First().Value.ExcludeFiles);
            Assert.Equal("./src/path1", actual.PackOptions.Mappings.First().Value.Include.First());
            Assert.Equal("./src/path2", actual.PackOptions.Mappings.First().Value.Include.Last());
            Assert.Null(actual.PackOptions.Mappings.Last().Value.Include);
            Assert.Null(actual.PackOptions.Mappings.Last().Value.Exclude);
            Assert.Null(actual.PackOptions.Mappings.Last().Value.ExcludeFiles);
            Assert.Equal("map1a", actual.PackOptions.Mappings.Last().Value.IncludeFiles.First());
            Assert.Equal("map1b", actual.PackOptions.Mappings.Last().Value.IncludeFiles.Last());
        }

        [Theory]
        [InlineData("{}", null, true)]
        [InlineData(@"{
                        ""buildOptions"": {}
                      }", null, false)]
        [InlineData(@"{
                        ""buildOptions"": {
                          ""outputName"": ""dllName""
                        }
                      }", "dllName", false)]
        [InlineData(@"{
                        ""buildOptions"": {
                          ""outputName"": ""dllName2"",
                          ""emitEntryPoint"": true
                        }
                      }", "dllName2", false)]
        [InlineData(@"{
                        ""buildOptions"": {
                          ""outputName"": null
                        }
                      }", null, false)]
        public void PackageSpecReader_BuildOptions(string json, string expectedValue, bool nullBuildOptions)
        {
            // Arrange & Act
            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            if (nullBuildOptions)
            {
                Assert.Null(actual.BuildOptions);
            }
            else
            {
                Assert.NotNull(actual.BuildOptions);
                Assert.Equal(expectedValue, actual.BuildOptions.OutputName);
            }
        }
#pragma warning restore CS0612 // Type or member is obsolete

        [Fact]
        public void PackageSpecReader_ReadsWithoutRestoreSettings()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.RestoreSettings);
            Assert.False(actual.RestoreSettings.HideWarningsAndErrors);
        }

        [Fact]
        public void PackageSpecReader_ReadsDependencyWithMultipleNoWarn()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("packageA"));
            Assert.NotNull(dep);
            Assert.NotNull(dep.NoWarn);
            Assert.Equal(dep.NoWarn.Count, 2);
            Assert.True(dep.NoWarn.Contains(NuGetLogCode.NU1500));
            Assert.True(dep.NoWarn.Contains(NuGetLogCode.NU1107));
        }

        [Fact]
        public void PackageSpecReader_ReadsDependencyWithSingleNoWarn()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("packageA"));
            Assert.NotNull(dep);
            Assert.NotNull(dep.NoWarn);
            Assert.Equal(dep.NoWarn.Count, 1);
            Assert.True(dep.NoWarn.Contains(NuGetLogCode.NU1500));
        }

        [Fact]
        public void PackageSpecReader_ReadsDependencyWithSingleEmptyNoWarn()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var dep = actual.Dependencies.FirstOrDefault(d => d.Name.Equals("packageA"));
            Assert.NotNull(dep);
            Assert.NotNull(dep.NoWarn);
            Assert.Equal(dep.NoWarn.Count, 0);
        }

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningProperties()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

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

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningPropertiesAndNo_NoWarn()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

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

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningPropertiesAndNo_WarnAsError()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

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

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithWarningPropertiesAndNo_AllWarningsAsErrors()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

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

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithEmptyWarningPropertiesAnd()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
            Assert.False(warningProperties.AllWarningsAsErrors);
            Assert.Equal(0, warningProperties.NoWarn.Count);
            Assert.Equal(0, warningProperties.WarningsAsErrors.Count);
        }

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithNoWarningProperties()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.NotNull(warningProperties);
        }

        [Fact]
        public void PackageSpecReader_RuntimeIdentifierPathNullIfEmpty()
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
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            Assert.Null(spec.TargetFrameworks.First().RuntimeIdentifierGraphPath);
        }

#pragma warning disable CS0612 // Type or member is obsolete
        [Fact]
        public void GetPackageSpec_WhenAuthorsPropertyIsAbsent_ReturnsEmptyAuthors()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Empty(packageSpec.Authors);
        }

        [Fact]
        public void GetPackageSpec_WhenAuthorsValueIsNull_ReturnsEmptyAuthors()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"authors\":null}");

            Assert.Empty(packageSpec.Authors);
        }

        [Fact]
        public void GetPackageSpec_WhenAuthorsValueIsString_ReturnsEmptyAuthors()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"authors\":\"b\"}");

            Assert.Empty(packageSpec.Authors);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/**/")]
        public void GetPackageSpec_WhenAuthorsValueIsEmptyArray_ReturnsEmptyAuthors(string value)
        {
            PackageSpec packageSpec = GetPackageSpec($"{{\"authors\":[{value}]}}");

            Assert.Empty(packageSpec.Authors);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("[]")]
        public void GetPackageSpec_WhenAuthorsValueElementIsNotConvertibleToString_Throws(string value)
        {
            var json = $"{{\"authors\":[{value}]}}";

            Assert.Throws<InvalidCastException>(() => GetPackageSpec(json));
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("true", "True")]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        public void GetPackageSpec_WhenAuthorsValueElementIsConvertibleToString_ReturnsAuthor(string value, string expectedValue)
        {
            PackageSpec packageSpec = GetPackageSpec($"{{\"authors\":[{value}]}}");

            Assert.Collection(packageSpec.Authors, author => Assert.Equal(expectedValue, author));
        }

        [Fact]
        public void GetPackageSpec_WhenBuildOptionsPropertyIsAbsent_ReturnsNullBuildOptions()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Null(packageSpec.BuildOptions);
        }

        [Fact]
        public void GetPackageSpec_WhenBuildOptionsValueIsEmptyObject_ReturnsBuildOptions()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"buildOptions\":{}}");

            Assert.NotNull(packageSpec.BuildOptions);
            Assert.Null(packageSpec.BuildOptions.OutputName);
        }

        [Fact]
        public void GetPackageSpec_WhenBuildOptionsValueOutputNameIsNull_ReturnsNullOutputName()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"buildOptions\":{\"outputName\":null}}");

            Assert.Null(packageSpec.BuildOptions.OutputName);
        }

        [Fact]
        public void GetPackageSpec_WhenBuildOptionsValueOutputNameIsValid_ReturnsOutputName()
        {
            const string expectedResult = "a";

            var json = $"{{\"buildOptions\":{{\"outputName\":\"{expectedResult}\"}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.BuildOptions.OutputName);
        }

        [Theory]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        [InlineData("true", "True")]
        public void GetPackageSpec_WhenBuildOptionsValueOutputNameIsConvertibleToString_ReturnsOutputName(string outputName, string expectedValue)
        {
            PackageSpec packageSpec = GetPackageSpec($"{{\"buildOptions\":{{\"outputName\":{outputName}}}}}");

            Assert.Equal(expectedValue, packageSpec.BuildOptions.OutputName);
        }

        [Fact]
        public void GetPackageSpec_WhenContentFilesPropertyIsAbsent_ReturnsEmptyContentFiles()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Empty(packageSpec.ContentFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenContentFilesValueIsNull_ReturnsEmptyContentFiles()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"contentFiles\":null}");

            Assert.Empty(packageSpec.ContentFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenContentFilesValueIsString_ReturnsEmptyContentFiles()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"contentFiles\":\"a\"}");

            Assert.Empty(packageSpec.ContentFiles);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/**/")]
        public void GetPackageSpec_WhenContentFilesValueIsEmptyArray_ReturnsEmptyContentFiles(string value)
        {
            PackageSpec packageSpec = GetPackageSpec($"{{\"contentFiles\":[{value}]}}");

            Assert.Empty(packageSpec.ContentFiles);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("[]")]
        public void GetPackageSpec_WhenContentFilesValueElementIsNotConvertibleToString_Throws(string value)
        {
            var json = $"{{\"contentFiles\":[{value}]}}";

            Assert.Throws<InvalidCastException>(() => GetPackageSpec(json));
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("true", "True")]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        public void GetPackageSpec_WhenContentFilesValueElementIsConvertibleToString_ReturnsContentFile(string value, string expectedValue)
        {
            PackageSpec packageSpec = GetPackageSpec($"{{\"contentFiles\":[{value}]}}");

            Assert.Collection(packageSpec.ContentFiles, contentFile => Assert.Equal(expectedValue, contentFile));
        }

        [Fact]
        public void GetPackageSpec_WhenCopyrightPropertyIsAbsent_ReturnsNullCopyright()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Null(packageSpec.Copyright);
        }

        [Fact]
        public void GetPackageSpec_WhenCopyrightValueIsNull_ReturnsNullCopyright()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"copyright\":null}");

            Assert.Null(packageSpec.Copyright);
        }

        [Fact]
        public void GetPackageSpec_WhenCopyrightValueIsString_ReturnsCopyright()
        {
            const string expectedResult = "a";

            PackageSpec packageSpec = GetPackageSpec($"{{\"copyright\":\"{expectedResult}\"}}");

            Assert.Equal(expectedResult, packageSpec.Copyright);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("true", "True")]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        public void GetPackageSpec_WhenCopyrightValueIsConvertibleToString_ReturnsCopyright(string value, string expectedValue)
        {
            PackageSpec packageSpec = GetPackageSpec($"{{\"copyright\":{value}}}");

            Assert.Equal(expectedValue, packageSpec.Copyright);
        }
#pragma warning restore CS0612 // Type or member is obsolete

        [Fact]
        public void GetPackageSpec_WhenDependenciesPropertyIsAbsent_ReturnsEmptyDependencies()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Empty(packageSpec.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesValueIsNull_ReturnsEmptyDependencies()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"dependencies\":null}");

            Assert.Empty(packageSpec.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyNameIsEmptyString_Throws()
        {
            const string json = "{\"dependencies\":{\"\":{}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Unable to resolve dependency ''.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(21, exception.Column);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyValueIsVersionString_ReturnsDependencyVersionRange()
        {
            var expectedResult = new LibraryRange(
                name: "a",
                new VersionRange(new NuGetVersion("1.2.3")),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange.ToShortString()}\"}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyValueIsVersionRangeString_ReturnsDependencyVersionRange()
        {
            var expectedResult = new LibraryRange(
                name: "a",
                new VersionRange(new NuGetVersion("1.2.3"), includeMinVersion: true, new NuGetVersion("4.5.6"), includeMaxVersion: false),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange}\"}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Theory]
        [InlineData(LibraryDependencyTarget.None)]
        [InlineData(LibraryDependencyTarget.Assembly)]
        [InlineData(LibraryDependencyTarget.Reference)]
        [InlineData(LibraryDependencyTarget.WinMD)]
        [InlineData(LibraryDependencyTarget.All)]
        [InlineData(LibraryDependencyTarget.PackageProjectExternal)]
        public void GetPackageSpec_WhenDependenciesDependencyTargetIsUnsupported_Throws(LibraryDependencyTarget target)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"version\":\"1.2.3\",\"target\":\"{target}\"}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal($"Invalid dependency target value '{target}'.", exception.Message);
            Assert.Equal(1, exception.Line);
            // The position is after the target name, which is of variable length.
            Assert.Equal(json.IndexOf(target.ToString()) + target.ToString().Length + 1, exception.Column);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyAutoreferencedPropertyIsAbsent_ReturnsFalseAutoreferenced()
        {
            LibraryDependency dependency = GetDependency($"{{\"dependencies\":{{\"a\":{{\"target\":\"Project\"}}}}}}");

            Assert.False(dependency.AutoReferenced);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenDependenciesDependencyAutoreferencedValueIsBool_ReturnsBoolAutoreferenced(bool expectedValue)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"autoReferenced\":{expectedValue.ToString().ToLower()},\"target\":\"Project\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedValue, dependency.AutoReferenced);
        }

        [Theory]
        [InlineData("exclude")]
        [InlineData("include")]
        [InlineData("suppressParent")]
        public void GetPackageSpec_WhenDependenciesDependencyValueIsArray_Throws(string propertyName)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"{propertyName}\":[\"b\"]}}}}}}";

            Assert.Throws<InvalidCastException>(() => GetPackageSpec(json));
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyIncludeAndExcludePropertiesAreAbsent_ReturnsAllIncludeType()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(LibraryIncludeFlags.All, dependency.IncludeType);
        }

        [Theory]
        [InlineData("\"Native\"", LibraryIncludeFlags.Native)]
        [InlineData("\"Analyzers, Native\"", LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.Native)]
        public void GetPackageSpec_WhenDependenciesDependencyExcludeValueIsValid_ReturnsIncludeType(
            string value,
            LibraryIncludeFlags result)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"exclude\":{value},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(LibraryIncludeFlags.All & ~result, dependency.IncludeType);
        }

        [Theory]
        [InlineData("\"Native\"", LibraryIncludeFlags.Native)]
        [InlineData("\"Analyzers, Native\"", LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.Native)]
        public void GetPackageSpec_WhenDependenciesDependencyIncludeValueIsValid_ReturnsIncludeType(
            string value,
            LibraryIncludeFlags expectedResult)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"include\":{value},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedResult, dependency.IncludeType);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyIncludeValueOverridesTypeValue_ReturnsIncludeType()
        {
            const string json = "{\"dependencies\":{\"a\":{\"include\":\"ContentFiles\",\"type\":\"BecomesNupkgDependency, SharedFramework\",\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.IncludeType);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencySuppressParentValueOverridesTypeValue_ReturnsSuppressParent()
        {
            const string json = "{\"dependencies\":{\"a\":{\"suppressParent\":\"ContentFiles\",\"type\":\"SharedFramework\",\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.SuppressParent);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencySuppressParentPropertyIsAbsent_ReturnsSuppressParent()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, dependency.SuppressParent);
        }

        [Theory]
        [InlineData("\"Compile\"", LibraryIncludeFlags.Compile)]
        [InlineData("\"Analyzers, Compile\"", LibraryIncludeFlags.Analyzers | LibraryIncludeFlags.Compile)]
        public void GetPackageSpec_WhenDependenciesDependencySuppressParentValueIsValid_ReturnsSuppressParent(
            string value,
            LibraryIncludeFlags expectedResult)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"suppressParent\":{value},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedResult, dependency.SuppressParent);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyVersionValueIsInvalid_Throws()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"b\"}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 35 : 'b' is not a valid version string.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(35, exception.Column);
            Assert.IsType<ArgumentException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyTargetPropertyIsAbsent_ReturnsTarget()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyTargetValueIsPackageAndVersionPropertyIsAbsent_Throws()
        {
            const string json = "{\"dependencies\":{\"a\":{\"target\":\"Package\"}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 22 : Package dependencies must specify a version range.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(22, exception.Column);
            Assert.IsType<ArgumentException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyTargetValueIsProjectAndVersionPropertyIsAbsent_ReturnsAllVersionRange()
        {
            LibraryDependency dependency = GetDependency("{\"dependencies\":{\"a\":{\"target\":\"Project\"}}}");

            Assert.Equal(VersionRange.All, dependency.LibraryRange.VersionRange);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyNoWarnPropertyIsAbsent_ReturnsEmptyNoWarns()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Empty(dependency.NoWarn);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyNoWarnValueIsValid_ReturnsNoWarns()
        {
            NuGetLogCode[] expectedResults = { NuGetLogCode.NU1000, NuGetLogCode.NU3000 };
            var json = $"{{\"dependencies\":{{\"a\":{{\"noWarn\":[\"{expectedResults[0].ToString()}\",\"{expectedResults[1].ToString()}\"],\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Collection(
                dependency.NoWarn,
                noWarn => Assert.Equal(expectedResults[0], noWarn),
                noWarn => Assert.Equal(expectedResults[1], noWarn));
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyGeneratePathPropertyPropertyIsAbsent_ReturnsFalseGeneratePathProperty()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.False(dependency.GeneratePathProperty);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenDependenciesDependencyGeneratePathPropertyValueIsValid_ReturnsGeneratePathProperty(bool expectedResult)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"generatePathProperty\":{expectedResult.ToString().ToLowerInvariant()},\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedResult, dependency.GeneratePathProperty);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyTypePropertyIsAbsent_ReturnsDefaultTypeConstraint()
        {
            const string json = "{\"dependencies\":{\"a\":{\"version\":\"1.0.0\"}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference,
                dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void GetPackageSpec_WhenDependenciesDependencyVersionCentrallyManagedPropertyIsAbsent_ReturnsFalseVersionCentrallyManaged()
        {
            LibraryDependency dependency = GetDependency($"{{\"dependencies\":{{\"a\":{{\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}}");

            Assert.False(dependency.VersionCentrallyManaged);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenDependenciesDependencyVersionCentrallyManagedValueIsBool_ReturnsBoolVersionCentrallyManaged(bool expectedValue)
        {
            var json = $"{{\"dependencies\":{{\"a\":{{\"versionCentrallyManaged\":{expectedValue.ToString().ToLower()},\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}}";

            LibraryDependency dependency = GetDependency(json);

            Assert.Equal(expectedValue, dependency.VersionCentrallyManaged);
        }

#pragma warning disable CS0612 // Type or member is obsolete
        [Fact]
        public void GetPackageSpec_WhenDescriptionPropertyIsAbsent_ReturnsNullDescription()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Null(packageSpec.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("b")]
        public void GetPackageSpec_WhenDescriptionValueIsValid_ReturnsDescription(string expectedResult)
        {
            string description = expectedResult == null ? "null" : $"\"{expectedResult}\"";
            PackageSpec packageSpec = GetPackageSpec($"{{\"description\":{description}}}");

            Assert.Equal(expectedResult, packageSpec.Description);
        }

        [Fact]
        public void GetPackageSpec_WhenLanguagePropertyIsAbsent_ReturnsNullLanguage()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Null(packageSpec.Language);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("b")]
        public void GetPackageSpec_WhenLanguageValueIsValid_ReturnsLanguage(string expectedResult)
        {
            string language = expectedResult == null ? "null" : $"\"{expectedResult}\"";
            PackageSpec packageSpec = GetPackageSpec($"{{\"language\":{language}}}");

            Assert.Equal(expectedResult, packageSpec.Language);
        }
#pragma warning restore CS0612 // Type or member is obsolete

        [Fact]
        public void GetPackageSpec_WhenFrameworksPropertyIsAbsent_ReturnsEmptyFrameworks()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Empty(packageSpec.TargetFrameworks);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksValueIsEmptyObject_ReturnsEmptyFrameworks()
        {
            PackageSpec packageSpec = GetPackageSpec("{\"frameworks\":{}}");

            Assert.Empty(packageSpec.TargetFrameworks);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksAssetTargetFallbackPropertyIsAbsent_ReturnsFalseAssetTargetFallback()
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}");

            Assert.False(framework.AssetTargetFallback);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenFrameworksAssetTargetFallbackValueIsValid_ReturnsAssetTargetFallback(bool expectedValue)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"assetTargetFallback\":{expectedValue.ToString().ToLowerInvariant()}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Equal(expectedValue, framework.AssetTargetFallback);
        }

        [Fact]
        public void GetPackageSpec_WithAssetTargetFallbackAndImportsValues_ReturnsValidAssetTargetFallbackFramework()
        {
            var json = $"{{\"frameworks\":{{\"net5.0\":{{\"assetTargetFallback\": true, \"imports\": [\"net472\", \"net471\"]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            framework.AssetTargetFallback.Should().BeTrue();
            var assetTargetFallback = framework.FrameworkName as AssetTargetFallbackFramework;
            assetTargetFallback.RootFramework.Should().Be(FrameworkConstants.CommonFrameworks.Net50);
            assetTargetFallback.Fallback.Should().HaveCount(2);
            assetTargetFallback.Fallback.First().Should().Be(FrameworkConstants.CommonFrameworks.Net472);
            assetTargetFallback.Fallback.Last().Should().Be(FrameworkConstants.CommonFrameworks.Net471);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsPropertyIsAbsent_ReturnsEmptyCentralPackageVersions()
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}");

            Assert.Empty(framework.CentralPackageVersions);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsValueIsEmptyObject_ReturnsEmptyCentralPackageVersions()
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{\"centralPackageVersions\":{}}}}");

            Assert.Empty(framework.CentralPackageVersions);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsVersionPropertyNameIsEmptyString_Throws()
        {
            var json = "{\"frameworks\":{\"a\":{\"centralPackageVersions\":{\"\":\"1.0.0\"}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve central version ''.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("\"\"")]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsVersionPropertyValueIsNullOrEmptyString_Throws(string value)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"centralPackageVersions\":{{\"b\":{value}}}}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : The version cannot be null or empty.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsIsValid_ReturnsCentralPackageVersions()
        {
            const string expectedPackageId = "b";
            VersionRange expectedVersionRange = VersionRange.Parse("[1.2.3,4.5.6)");
            var expectedCentralPackageVersion = new CentralPackageVersion(expectedPackageId, expectedVersionRange);
            var json = $"{{\"frameworks\":{{\"a\":{{\"centralPackageVersions\":{{\"{expectedPackageId}\":\"{expectedVersionRange.ToShortString()}\"}}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Collection(
                framework.CentralPackageVersions,
                actualResult =>
                {
                    Assert.Equal(expectedPackageId, actualResult.Key);
                    Assert.Equal(expectedCentralPackageVersion, actualResult.Value);
                });
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksCentralPackageVersionsHasDuplicateKey_LastOneWins()
        {
            const string expectedPackageId = "b";
            VersionRange unexpectedVersionRange = VersionRange.Parse("1.2.3");
            VersionRange expectedVersionRange = VersionRange.Parse("4.5.6");
            var expectedCentralPackageVersion = new CentralPackageVersion(expectedPackageId, expectedVersionRange);
            var json = $"{{\"frameworks\":{{\"a\":{{\"centralPackageVersions\":{{\"{expectedPackageId}\":\"{unexpectedVersionRange.ToShortString()}\"," +
                $"\"{expectedPackageId}\":\"{expectedVersionRange.ToShortString()}\"}}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Collection(
                framework.CentralPackageVersions,
                actualResult =>
                {
                    Assert.Equal(expectedPackageId, actualResult.Key);
                    Assert.Equal(expectedCentralPackageVersion, actualResult.Value);
                });
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesPropertyIsAbsent_ReturnsEmptyDependencies()
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{}}}");

            Assert.Empty(framework.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesValueIsNull_ReturnsEmptyDependencies()
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"a\":{\"dependencies\":null}}}");

            Assert.Empty(framework.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyNameIsEmptyString_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"\":{}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve dependency ''.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyValueIsVersionString_ReturnsDependencyVersionRange()
        {
            var expectedResult = new LibraryRange(
                name: "b",
                new VersionRange(new NuGetVersion("1.2.3")),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange.ToShortString()}\"}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyValueIsVersionRangeString_ReturnsDependencyVersionRange()
        {
            var expectedResult = new LibraryRange(
                name: "b",
                new VersionRange(new NuGetVersion("1.2.3"), includeMinVersion: true, new NuGetVersion("4.5.6"), includeMaxVersion: false),
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference);
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"{expectedResult.Name}\":\"{expectedResult.VersionRange}\"}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(expectedResult, dependency.LibraryRange);
        }

        [Theory]
        [InlineData(LibraryDependencyTarget.None)]
        [InlineData(LibraryDependencyTarget.Assembly)]
        [InlineData(LibraryDependencyTarget.Reference)]
        [InlineData(LibraryDependencyTarget.WinMD)]
        [InlineData(LibraryDependencyTarget.All)]
        [InlineData(LibraryDependencyTarget.PackageProjectExternal)]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetValueIsUnsupported_Throws(LibraryDependencyTarget target)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"version\":\"1.2.3\",\"target\":\"{target}\"}}}}}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal($"Error reading '' at line 1 column 20 : Invalid dependency target value '{target}'.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyAutoreferencedPropertyIsAbsent_ReturnsFalseAutoreferenced()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Project\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.False(dependency.AutoReferenced);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyAutoreferencedValueIsBool_ReturnsBoolAutoreferenced(bool expectedValue)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"autoReferenced\":{expectedValue.ToString().ToLower()},\"target\":\"Project\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(expectedValue, dependency.AutoReferenced);
        }

        [Theory]
        [InlineData("exclude")]
        [InlineData("include")]
        [InlineData("suppressParent")]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyValueIsArray_Throws(string propertyName)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"{propertyName}\":[\"c\"]}}}}}}}}}}";

            // The exception messages will not be the same because the innermost exception in the baseline
            // is a Newtonsoft.Json exception, while it's a .NET exception in the improved.
            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Specified cast is not valid.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<InvalidCastException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyIncludeAndExcludePropertiesAreAbsent_ReturnsAllIncludeType()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlags.All, dependency.IncludeType);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyExcludeValueIsValid_ReturnsIncludeType()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"exclude\":\"Native\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlags.All & ~LibraryIncludeFlags.Native, dependency.IncludeType);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyIncludeValueIsValid_ReturnsIncludeType()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"include\":\"ContentFiles\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.IncludeType);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyIncludeValueOverridesTypeValue_ReturnsIncludeType()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"include\":\"ContentFiles\",\"type\":\"BecomesNupkgDependency, SharedFramework\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.IncludeType);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencySuppressParentValueOverridesTypeValue_ReturnsSuppressParent()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"suppressParent\":\"ContentFiles\",\"type\":\"SharedFramework\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlags.ContentFiles, dependency.SuppressParent);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencySuppressParentPropertyIsAbsent_ReturnsSuppressParent()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, dependency.SuppressParent);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencySuppressParentValueIsValid_ReturnsSuppressParent()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"suppressParent\":\"Compile\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryIncludeFlags.Compile, dependency.SuppressParent);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyVersionValueIsInvalid_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"c\"}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Error reading '' at line 1 column 54 : 'c' is not a valid version string.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetPropertyIsAbsent_ReturnsTarget()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference,
                dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetValueIsPackageAndVersionPropertyIsAbsent_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Package\"}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Error reading '' at line 1 column 41 : Package dependencies must specify a version range.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTargetValueIsProjectAndVersionPropertyIsAbsent_ReturnsAllVersionRange()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Project\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(VersionRange.All, dependency.LibraryRange.VersionRange);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyNoWarnPropertyIsAbsent_ReturnsEmptyNoWarns()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Empty(dependency.NoWarn);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyNoWarnValueIsValid_ReturnsNoWarns()
        {
            NuGetLogCode[] expectedResults = { NuGetLogCode.NU1000, NuGetLogCode.NU3000 };
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"noWarn\":[\"{expectedResults[0].ToString()}\",\"{expectedResults[1].ToString()}\"],\"version\":\"1.0.0\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Collection(
                dependency.NoWarn,
                noWarn => Assert.Equal(expectedResults[0], noWarn),
                noWarn => Assert.Equal(expectedResults[1], noWarn));
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyGeneratePathPropertyPropertyIsAbsent_ReturnsFalseGeneratePathProperty()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.False(dependency.GeneratePathProperty);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyGeneratePathPropertyValueIsValid_ReturnsGeneratePathProperty(bool expectedResult)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"generatePathProperty\":{expectedResult.ToString().ToLowerInvariant()},\"version\":\"1.0.0\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(expectedResult, dependency.GeneratePathProperty);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyTypePropertyIsAbsent_ReturnsDefaultTypeConstraint()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(
                LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference,
                dependency.LibraryRange.TypeConstraint);
        }


        [Fact]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyVersionCentrallyManagedPropertyIsAbsent_ReturnsFalseVersionCentrallyManaged()
        {
            const string json = "{\"frameworks\":{\"a\":{\"dependencies\":{\"b\":{\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.False(dependency.VersionCentrallyManaged);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenFrameworksDependenciesDependencyVersionCentrallyManagedValueIsBool_ReturnsBoolVersionCentrallyManaged(bool expectedValue)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"dependencies\":{{\"b\":{{\"versionCentrallyManaged\":{expectedValue.ToString().ToLower()},\"target\":\"Package\",\"version\":\"1.0.0\"}}}}}}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(expectedValue, dependency.VersionCentrallyManaged);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesPropertyIsAbsent_ReturnsEmptyDownloadDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsNull_ReturnsEmptyDownloadDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsNotArray_ReturnsEmptyDownloadDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":\"b\"}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsEmptyArray_ReturnsEmptyDownloadDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[]}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.DownloadDependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyNameIsAbsent_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[{\"version\":\"1.2.3\"}]}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve downloadDependency ''.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyNameIsNull_ReturnsDownloadDependencies()
        {
            var expectedResult = new DownloadDependency(name: null, new VersionRange(new NuGetVersion("1.2.3")));
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":null,\"version\":\"{expectedResult.VersionRange.ToShortString()}\"}}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            DownloadDependency actualResult = framework.DownloadDependencies.Single();

            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.VersionRange, actualResult.VersionRange);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyVersionIsAbsent_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[{\"name\":\"b\"}]}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : The version cannot be null or empty", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("c")]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesDependencyVersionIsInvalid_Throws(string version)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":\"b\",\"version\":\"{version}\"}}]}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            int expectedColumn = json.IndexOf($"\"{version}\"") + version.Length + 2;

            Assert.Equal($"Error reading '' at line 1 column 20 : Error reading '' at line 1 column {expectedColumn} : '{version}' is not a valid version string.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueIsValid_ReturnsDownloadDependencies()
        {
            var expectedResult = new DownloadDependency(name: "b", new VersionRange(new NuGetVersion("1.2.3")));
            var json = $"{{\"frameworks\":{{\"a\":{{\"downloadDependencies\":[{{\"name\":\"{expectedResult.Name}\",\"version\":\"{expectedResult.VersionRange.ToShortString()}\"}}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Equal(expectedResult, framework.DownloadDependencies.Single());
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksDownloadDependenciesValueHasDuplicates_PrefersFirstByName()
        {
            var expectedResult = new DownloadDependency(name: "b", new VersionRange(new NuGetVersion("1.2.3")));
            var unexpectedResult = new DownloadDependency(name: "b", new VersionRange(new NuGetVersion("4.5.6")));
            var json = "{\"frameworks\":{\"a\":{\"downloadDependencies\":[" +
                $"{{\"name\":\"{expectedResult.Name}\",\"version\":\"{expectedResult.VersionRange.ToShortString()}\"}}," +
                $"{{\"name\":\"{unexpectedResult.Name}\",\"version\":\"{unexpectedResult.VersionRange.ToShortString()}\"}}" +
                "]}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Equal(expectedResult, framework.DownloadDependencies.Single());
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesPropertyIsAbsent_ReturnsEmptyDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesValueIsNull_ReturnsEmptyDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesValueIsEmptyObject_ReturnsEmptyDependencies()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Dependencies);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesDependencyTargetPropertyIsAbsent_ReturnsTarget()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{\"b\":{\"version\":\"1.0.0\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(LibraryDependencyTarget.Reference, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesDependencyTargetValueIsPackageAndVersionPropertyIsAbsent_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{\"b\":{\"target\":\"Package\"}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Error reading '' at line 1 column 48 : Package dependencies must specify a version range.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.IsType<ArgumentException>(exception.InnerException.InnerException);
            Assert.Null(exception.InnerException.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkAssembliesDependencyTargetValueIsProjectAndVersionPropertyIsAbsent_ReturnsAllVersionRange()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkAssemblies\":{\"b\":{\"target\":\"Project\"}}}}}";

            LibraryDependency dependency = GetFrameworksDependency(json);

            Assert.Equal(VersionRange.All, dependency.LibraryRange.VersionRange);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPropertyIsAbsent_ReturnsEmptyFrameworkReferences()
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.FrameworkReferences);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesValueIsNull_ReturnsEmptyFrameworkReferences()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkReferences\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.FrameworkReferences);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesValueIsEmptyObject_ReturnsEmptyFrameworkReferences()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkReferences\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.FrameworkReferences);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesFrameworkNameIsEmptyString_Throws()
        {
            const string json = "{\"frameworks\":{\"a\":{\"frameworkReferences\":{\"\":{}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("Error reading '' at line 1 column 20 : Unable to resolve frameworkReference.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsPropertyIsAbsent_ReturnsNonePrivateAssets()
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.None);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json);

            Assert.Equal(expectedResult, dependency);
        }

        [Theory]
        [InlineData("\"null\"")]
        [InlineData("\"\"")]
        [InlineData("\"c\"")]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsValueIsInvalidValue_ReturnsNonePrivateAssets(string privateAssets)
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.None);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{\"privateAssets\":{privateAssets}}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json);

            Assert.Equal(expectedResult, dependency);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsValueIsValidString_ReturnsPrivateAssets()
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.All);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{\"privateAssets\":\"{expectedResult.PrivateAssets.ToString().ToLowerInvariant()}\"}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json);

            Assert.Equal(expectedResult, dependency);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksFrameworkReferencesPrivateAssetsValueIsValidDelimitedString_ReturnsPrivateAssets()
        {
            var expectedResult = new FrameworkDependency(name: "b", FrameworkDependencyFlags.All);
            var json = $"{{\"frameworks\":{{\"a\":{{\"frameworkReferences\":{{\"{expectedResult.Name}\":{{\"privateAssets\":\"none,all\"}}}}}}}}}}";

            FrameworkDependency dependency = GetFrameworksFrameworkReference(json);

            Assert.Equal(expectedResult, dependency);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksImportsPropertyIsAbsent_ReturnsEmptyImports()
        {
            const string json = "{\"frameworks\":{\"a\":{}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Imports);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("\"\"")]
        public void GetPackageSpec_WhenFrameworksImportsValueIsArrayOfNullOrEmptyString_ImportIsSkipped(string import)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":[{import}]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Imports);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksImportsValueIsNull_ReturnsEmptyList()
        {
            const string json = "{\"frameworks\":{\"a\":{\"imports\":null}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Imports);
        }

        [Theory]
        [InlineData("true")]
        [InlineData("-2")]
        [InlineData("3.14")]
        [InlineData("{}")]
        public void GetPackageSpec_WhenFrameworksImportsValueIsInvalidValue_ReturnsEmptyList(string value)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":{value}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Empty(framework.Imports);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksImportsValueContainsInvalidValue_Throws()
        {
            const string expectedImport = "b";

            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":[\"{expectedImport}\"]}}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal(
                $"Error reading '' at line 1 column 20 : Imports contains an invalid framework: '{expectedImport}' in 'project.json'.",
                exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
            Assert.IsType<FileFormatException>(exception.InnerException);
            Assert.Null(exception.InnerException.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksImportsValueIsString_ReturnsImport()
        {
            NuGetFramework expectedResult = NuGetFramework.Parse("net48");
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":\"{expectedResult.GetShortFolderName()}\"}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Collection(
                framework.Imports,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksImportsValueIsArrayOfStrings_ReturnsImports()
        {
            NuGetFramework[] expectedResults = { NuGetFramework.Parse("net472"), NuGetFramework.Parse("net48") };
            var json = $"{{\"frameworks\":{{\"a\":{{\"imports\":[\"{expectedResults[0].GetShortFolderName()}\",\"{expectedResults[1].GetShortFolderName()}\"]}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Collection(
                framework.Imports,
                actualResult => Assert.Equal(expectedResults[0], actualResult),
                actualResult => Assert.Equal(expectedResults[1], actualResult));
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksRuntimeIdentifierGraphPathPropertyIsAbsent_ReturnsRuntimeIdentifierGraphPath()
        {
            const string json = "{\"frameworks\":{\"a\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Null(framework.RuntimeIdentifierGraphPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("b")]
        public void GetPackageSpec_WhenFrameworksRuntimeIdentifierGraphPathValueIsString_ReturnsRuntimeIdentifierGraphPath(string expectedResult)
        {
            string runtimeIdentifierGraphPath = expectedResult == null ? "null" : $"\"{expectedResult}\"";
            var json = $"{{\"frameworks\":{{\"a\":{{\"runtimeIdentifierGraphPath\":{runtimeIdentifierGraphPath}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Equal(expectedResult, framework.RuntimeIdentifierGraphPath);
        }

        [Fact]
        public void GetPackageSpec_WhenFrameworksWarnPropertyIsAbsent_ReturnsWarn()
        {
            const string json = "{\"frameworks\":{\"a\":{}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.False(framework.Warn);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageSpec_WhenFrameworksWarnValueIsValid_ReturnsWarn(bool expectedResult)
        {
            var json = $"{{\"frameworks\":{{\"a\":{{\"warn\":{expectedResult.ToString().ToLowerInvariant()}}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);

            Assert.Equal(expectedResult, framework.Warn);
        }

#pragma warning disable CS0612 // Type or member is obsolete
        [Fact]
        public void GetPackageSpec_WhenPackIncludePropertyIsAbsent_ReturnsEmptyPackInclude()
        {
            PackageSpec packageSpec = GetPackageSpec("{}");

            Assert.Empty(packageSpec.PackInclude);
        }

        [Fact]
        public void GetPackageSpec_WhenPackIncludePropertyIsValid_ReturnsPackInclude()
        {
            var expectedResults = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("a", "b"), new KeyValuePair<string, string>("c", "d") };
            var json = $"{{\"packInclude\":{{\"{expectedResults[0].Key}\":\"{expectedResults[0].Value}\",\"{expectedResults[1].Key}\":\"{expectedResults[1].Value}\"}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Collection(
                packageSpec.PackInclude,
                actualResult => Assert.Equal(expectedResults[0], actualResult),
                actualResult => Assert.Equal(expectedResults[1], actualResult));
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"packOptions\":null}")]
        public void GetPackageSpec_WhenPackOptionsPropertyIsAbsentOrValueIsNull_ReturnsPackOptions(string json)
        {
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.NotNull(packageSpec.PackOptions);
            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
            Assert.Empty(packageSpec.PackOptions.Mappings);
            Assert.Empty(packageSpec.PackOptions.PackageType);

            Assert.Null(packageSpec.IconUrl);
            Assert.Null(packageSpec.LicenseUrl);
            Assert.Empty(packageSpec.Owners);
            Assert.Null(packageSpec.ProjectUrl);
            Assert.Null(packageSpec.ReleaseNotes);
            Assert.False(packageSpec.RequireLicenseAcceptance);
            Assert.Null(packageSpec.Summary);
            Assert.Empty(packageSpec.Tags);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsPropertyIsAbsent_OwnersAndTagsAreEmpty()
        {
            const string json = "{\"owners\":[\"a\"],\"tags\":[\"b\"]}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.Owners);
            Assert.Empty(packageSpec.Tags);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsPropertyIsEmptyObject_ReturnsPackOptions()
        {
            string json = "{\"packOptions\":{}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.NotNull(packageSpec.PackOptions);
            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
            Assert.Null(packageSpec.PackOptions.Mappings);
            Assert.Empty(packageSpec.PackOptions.PackageType);

            Assert.Null(packageSpec.IconUrl);
            Assert.Null(packageSpec.LicenseUrl);
            Assert.Empty(packageSpec.Owners);
            Assert.Null(packageSpec.ProjectUrl);
            Assert.Null(packageSpec.ReleaseNotes);
            Assert.False(packageSpec.RequireLicenseAcceptance);
            Assert.Null(packageSpec.Summary);
            Assert.Empty(packageSpec.Tags);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsValueIsValid_ReturnsPackOptions()
        {
            const string iconUrl = "a";
            const string licenseUrl = "b";
            string[] owners = { "c", "d" };
            const string projectUrl = "e";
            const string releaseNotes = "f";
            const bool requireLicenseAcceptance = true;
            const string summary = "g";
            string[] tags = { "h", "i" };

            var json = $"{{\"packOptions\":{{\"iconUrl\":\"{iconUrl}\",\"licenseUrl\":\"{licenseUrl}\",\"owners\":[{string.Join(",", owners.Select(owner => $"\"{owner}\""))}]," +
                $"\"projectUrl\":\"{projectUrl}\",\"releaseNotes\":\"{releaseNotes}\",\"requireLicenseAcceptance\":{requireLicenseAcceptance.ToString().ToLowerInvariant()}," +
                $"\"summary\":\"{summary}\",\"tags\":[{string.Join(",", tags.Select(tag => $"\"{tag}\""))}]}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.NotNull(packageSpec.PackOptions);
            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
            Assert.Null(packageSpec.PackOptions.Mappings);
            Assert.Empty(packageSpec.PackOptions.PackageType);
            Assert.Equal(iconUrl, packageSpec.IconUrl);
            Assert.Equal(licenseUrl, packageSpec.LicenseUrl);
            Assert.Equal(owners, packageSpec.Owners);
            Assert.Equal(projectUrl, packageSpec.ProjectUrl);
            Assert.Equal(releaseNotes, packageSpec.ReleaseNotes);
            Assert.Equal(requireLicenseAcceptance, packageSpec.RequireLicenseAcceptance);
            Assert.Equal(summary, packageSpec.Summary);
            Assert.Equal(tags, packageSpec.Tags);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsPackageTypeValueIsNull_ReturnsEmptyPackageTypes()
        {
            const string json = "{\"packOptions\":{\"packageType\":null}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.PackOptions.PackageType);
        }

        [Theory]
        [InlineData("true", 34)]
        [InlineData("-2", 32)]
        [InlineData("3.14", 34)]
        [InlineData("{}", 31)]
        [InlineData("[true]", 31)]
        [InlineData("[-2]", 31)]
        [InlineData("[3.14]", 31)]
        [InlineData("[null]", 31)]
        [InlineData("[{}]", 31)]
        [InlineData("[[]]", 31)]
        public void GetPackageSpec_WhenPackOptionsPackageTypeIsInvalid_Throws(string value, int expectedColumn)
        {
            var json = $"{{\"packOptions\":{{\"packageType\":{value}}}}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("The pack options package type must be a string or array of strings in 'project.json'.", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(expectedColumn, exception.Column);
            Assert.Null(exception.InnerException);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\"a,b\"", "a,b")]
        [InlineData("[\"a\"]", "a")]
        [InlineData("[\"a b\"]", "a b")]
        public void GetPackageSpec_WhenPackOptionsPackageTypeValueIsValid_ReturnsPackageTypes(string value, string expectedName)
        {
            var expectedResult = new PackageType(expectedName, PackageType.EmptyVersion);
            var json = $"{{\"packOptions\":{{\"packageType\":{value}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Collection(
                packageSpec.PackOptions.PackageType,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesValueIsNull_ReturnsNullInclude()
        {
            const string json = "{\"packOptions\":{\"files\":null}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesValueIsEmptyObject_ReturnsNullInclude()
        {
            const string json = "{\"packOptions\":{\"files\":{}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesIncludeValueIsNull_ReturnsNullIncludeExcludeFiles()
        {
            const string json = "{\"packOptions\":{\"files\":{\"include\":null}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesIncludeValueIsEmptyArray_ReturnsEmptyInclude()
        {
            const string json = "{\"packOptions\":{\"files\":{\"include\":[]}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.PackOptions.IncludeExcludeFiles.Include);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\"a, b\"", "a, b")]
        [InlineData("[null]", null)]
        [InlineData("[\"\"]", "")]
        [InlineData("[\"a\"]", "a")]
        [InlineData("[\"a, b\"]", "a, b")]
        [InlineData("[\"a\", \"b\"]", "a", "b")]
        public void GetPackageSpec_WhenPackOptionsFilesIncludeValueIsValid_ReturnsInclude(string value, params string[] expectedResults)
        {
            expectedResults = expectedResults ?? new string[] { null };

            var json = $"{{\"packOptions\":{{\"files\":{{\"include\":{value}}}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.IncludeExcludeFiles.Include);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesIncludeFilesValueIsNull_ReturnsNullIncludeExcludeFiles()
        {
            const string json = "{\"packOptions\":{\"files\":{\"includeFiles\":null}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesIncludeFilesValueIsEmptyArray_ReturnsEmptyIncludeFiles()
        {
            const string json = "{\"packOptions\":{\"files\":{\"includeFiles\":[]}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.PackOptions.IncludeExcludeFiles.IncludeFiles);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\"a, b\"", "a, b")]
        [InlineData("[null]", null)]
        [InlineData("[\"\"]", "")]
        [InlineData("[\"a\"]", "a")]
        [InlineData("[\"a, b\"]", "a, b")]
        [InlineData("[\"a\", \"b\"]", "a", "b")]
        public void GetPackageSpec_WhenPackOptionsFilesIncludeFilesValueIsValid_ReturnsIncludeFiles(string value, params string[] expectedResults)
        {
            expectedResults = expectedResults ?? new string[] { null };

            var json = $"{{\"packOptions\":{{\"files\":{{\"includeFiles\":{value}}}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.IncludeExcludeFiles.IncludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesExcludeValueIsNull_ReturnsNullIncludeExcludeFiles()
        {
            const string json = "{\"packOptions\":{\"files\":{\"exclude\":null}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesExcludeValueIsEmptyArray_ReturnsEmptyExclude()
        {
            const string json = "{\"packOptions\":{\"files\":{\"exclude\":[]}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.PackOptions.IncludeExcludeFiles.Exclude);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\"a, b\"", "a, b")]
        [InlineData("[null]", null)]
        [InlineData("[\"\"]", "")]
        [InlineData("[\"a\"]", "a")]
        [InlineData("[\"a, b\"]", "a, b")]
        [InlineData("[\"a\", \"b\"]", "a", "b")]
        public void GetPackageSpec_WhenPackOptionsFilesExcludeValueIsValid_ReturnsExclude(string value, params string[] expectedResults)
        {
            expectedResults = expectedResults ?? new string[] { null };

            var json = $"{{\"packOptions\":{{\"files\":{{\"exclude\":{value}}}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.IncludeExcludeFiles.Exclude);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesExcludeFilesValueIsNull_ReturnsNullIncludeExcludeFiles()
        {
            const string json = "{\"packOptions\":{\"files\":{\"excludeFiles\":null}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.IncludeExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesExcludeFilesValueIsEmptyArray_ReturnsEmptyExcludeFiles()
        {
            const string json = "{\"packOptions\":{\"files\":{\"excludeFiles\":[]}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.PackOptions.IncludeExcludeFiles.ExcludeFiles);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\"a, b\"", "a, b")]
        [InlineData("[null]", null)]
        [InlineData("[\"\"]", "")]
        [InlineData("[\"a\"]", "a")]
        [InlineData("[\"a, b\"]", "a, b")]
        [InlineData("[\"a\", \"b\"]", "a", "b")]
        public void GetPackageSpec_WhenPackOptionsFilesExcludeFilesValueIsValid_ReturnsExcludeFiles(string value, params string[] expectedResults)
        {
            expectedResults = expectedResults ?? new string[] { null };

            var json = $"{{\"packOptions\":{{\"files\":{{\"excludeFiles\":{value}}}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.IncludeExcludeFiles.ExcludeFiles);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesMappingsPropertyIsAbsent_ReturnsNullMappings()
        {
            const string json = "{\"packOptions\":{\"files\":{}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.Mappings);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesMappingsValueIsNull_ReturnsNullMappings()
        {
            const string json = "{\"packOptions\":{\"files\":{\"mappings\":null}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.PackOptions.Mappings);
        }

        [Theory]
        [InlineData("\"b\"", "b")]
        [InlineData("\"b,c\"", "b,c")]
        [InlineData("[\"b\", \"c\"]", "b", "c")]
        public void GetPackageSpec_WhenPackOptionsFilesMappingsValueIsValid_ReturnsMappings(string value, params string[] expectedIncludes)
        {
            var expectedResults = new Dictionary<string, IncludeExcludeFiles>()
            {
                { "a", new IncludeExcludeFiles() { Include = expectedIncludes } }
            };
            var json = $"{{\"packOptions\":{{\"files\":{{\"mappings\":{{\"a\":{value}}}}}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.Mappings);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesMappingsValueHasMultipleMappings_ReturnsMappings()
        {
            var expectedResults = new Dictionary<string, IncludeExcludeFiles>()
            {
                { "a", new IncludeExcludeFiles() { Include = new[] { "b" } } },
                { "c", new IncludeExcludeFiles() { Include = new[] { "d", "e" } } }
            };
            const string json = "{\"packOptions\":{\"files\":{\"mappings\":{\"a\":\"b\",\"c\":[\"d\", \"e\"]}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.Mappings);
        }

        [Fact]
        public void GetPackageSpec_WhenPackOptionsFilesMappingsValueHasFiles_ReturnsMappings()
        {
            var expectedResults = new Dictionary<string, IncludeExcludeFiles>()
            {
                {
                    "a",
                    new IncludeExcludeFiles()
                    {
                        Include = new [] { "b" },
                        IncludeFiles = new [] { "c" },
                        Exclude = new [] { "d" },
                        ExcludeFiles = new [] { "e" }
                    }
                }
            };
            const string json = "{\"packOptions\":{\"files\":{\"mappings\":{\"a\":{\"include\":\"b\",\"includeFiles\":\"c\",\"exclude\":\"d\",\"excludeFiles\":\"e\"}}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.PackOptions.Mappings);
        }
#pragma warning restore CS0612 // Type or member is obsolete

        [Fact]
        public void GetPackageSpec_WhenRestorePropertyIsAbsent_ReturnsNullRestoreMetadata()
        {
            const string json = "{}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Null(packageSpec.RestoreMetadata);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreValueIsEmptyObject_ReturnsRestoreMetadata()
        {
            const string json = "{\"restore\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.NotNull(packageSpec.RestoreMetadata);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("\"\"")]
        [InlineData("\"a\"")]
        public void GetPackageSpec_WhenRestoreProjectStyleValueIsInvalid_ReturnsProjectStyle(string value)
        {
            var json = $"{{\"restore\":{{\"projectStyle\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(ProjectStyle.Unknown, packageSpec.RestoreMetadata.ProjectStyle);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreProjectStyleValueIsValid_ReturnsProjectStyle()
        {
            const ProjectStyle expectedResult = ProjectStyle.PackageReference;

            var json = $"{{\"restore\":{{\"projectStyle\":\"{expectedResult.ToString().ToLowerInvariant()}\"}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.ProjectStyle);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestoreProjectUniqueNameValueIsValid_ReturnsProjectUniqueName(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectUniqueName\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectUniqueName);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestoreOutputPathValueIsValid_ReturnsOutputPath(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"outputPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.OutputPath);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestorePackagesPathValueIsValid_ReturnsPackagesPath(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"packagesPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.PackagesPath);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestoreProjectJsonPathValueIsValid_ReturnsProjectJsonPath(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectJsonPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectJsonPath);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestoreProjectNameValueIsValid_ReturnsProjectName(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectName\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectName);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestoreProjectPathValueIsValid_ReturnsProjectPath(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ProjectPath);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenCrossTargetingValueIsValid_ReturnsCrossTargeting(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"crossTargeting\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CrossTargeting);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenLegacyPackagesDirectoryValueIsValid_ReturnsLegacyPackagesDirectory(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"legacyPackagesDirectory\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.LegacyPackagesDirectory);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenValidateRuntimeAssetsValueIsValid_ReturnsValidateRuntimeAssets(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"validateRuntimeAssets\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.ValidateRuntimeAssets);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenSkipContentFileWriteValueIsValid_ReturnsSkipContentFileWrite(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"skipContentFileWrite\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.SkipContentFileWrite);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenCentralPackageVersionsManagementEnabledValueIsValid_ReturnsCentralPackageVersionsManagementEnabled(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"centralPackageVersionsManagementEnabled\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenCentralPackageFloatingVersionsEnabledValueIsValid_ReturnsCentralPackageFloatingVersionsEnabled(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"centralPackageFloatingVersionsEnabled\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CentralPackageFloatingVersionsEnabled);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenCentralPackageVersionOverrideDisabledValueIsValid_ReturnsCentralPackageVersionOverrideDisabled(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"centralPackageVersionOverrideDisabled\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CentralPackageVersionOverrideDisabled);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void GetPackageSpec_WhenCentralPackageTransitivePinningEnabledValueIsValid_ReturnsCentralPackageTransitivePinningEnabled(
            bool? value,
            bool expectedValue)
        {
            var json = $"{{\"restore\":{{\"CentralPackageTransitivePinningEnabled\":{(value.HasValue ? value.ToString().ToLowerInvariant() : "null")}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedValue, packageSpec.RestoreMetadata.CentralPackageTransitivePinningEnabled);
        }

        [Fact]
        public void GetPackageSpec_WhenSourcesValueIsEmptyObject_ReturnsEmptySources()
        {
            const string json = "{\"restore\":{\"sources\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.RestoreMetadata.Sources);
        }

        [Fact]
        public void GetPackageSpec_WhenSourcesValueIsValid_ReturnsSources()
        {
            PackageSource[] expectedResults = { new PackageSource(source: "a"), new PackageSource(source: "b") };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult.Name}\":{{}}"));
            var json = $"{{\"restore\":{{\"sources\":{{{values}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.Sources);
        }

        [Fact]
        public void GetPackageSpec_WhenFilesValueIsEmptyObject_ReturnsEmptyFiles()
        {
            const string json = "{\"restore\":{\"files\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.RestoreMetadata.Files);
        }

        [Fact]
        public void GetPackageSpec_WhenFilesValueIsValid_ReturnsFiles()
        {
            ProjectRestoreMetadataFile[] expectedResults =
            {
                new ProjectRestoreMetadataFile(packagePath: "a", absolutePath: "b"),
                new ProjectRestoreMetadataFile(packagePath: "c", absolutePath:"d")
            };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult.PackagePath}\":\"{expectedResult.AbsolutePath}\""));
            var json = $"{{\"restore\":{{\"files\":{{{values}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.Files);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreFrameworksValueIsEmptyObject_ReturnsEmptyFrameworks()
        {
            const string json = "{\"restore\":{\"frameworks\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.RestoreMetadata.TargetFrameworks);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreFrameworksFrameworkNameValueIsValid_ReturnsFrameworks()
        {
            var expectedResult = new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.ParseFolder("net472"));
            var json = $"{{\"restore\":{{\"frameworks\":{{\"{expectedResult.FrameworkName.GetShortFolderName()}\":{{}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Collection(
                packageSpec.RestoreMetadata.TargetFrameworks,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreFrameworksFrameworkValueHasProjectReferenceWithoutAssets_ReturnsFrameworks()
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
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Collection(
                packageSpec.RestoreMetadata.TargetFrameworks,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreFrameworksFrameworkValueHasProjectReferenceWithAssets_ReturnsFrameworks()
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
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Collection(
                packageSpec.RestoreMetadata.TargetFrameworks,
                actualResult => Assert.Equal(expectedResult, actualResult));
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreConfigFilePathsValueIsEmptyArray_ReturnsEmptyConfigFilePaths()
        {
            const string json = "{\"restore\":{\"configFilePaths\":[]}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.RestoreMetadata.ConfigFilePaths);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreConfigFilePathsValueIsValid_ReturnsConfigFilePaths()
        {
            string[] expectedResults = { "a", "b" };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""));
            var json = $"{{\"restore\":{{\"configFilePaths\":[{values}]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.ConfigFilePaths);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreFallbackFoldersValueIsEmptyArray_ReturnsEmptyFallbackFolders()
        {
            const string json = "{\"restore\":{\"fallbackFolders\":[]}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.RestoreMetadata.FallbackFolders);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreFallbackFoldersValueIsValid_ReturnsConfigFilePaths()
        {
            string[] expectedResults = { "a", "b" };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""));
            var json = $"{{\"restore\":{{\"fallbackFolders\":[{values}]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.FallbackFolders);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreOriginalTargetFrameworksValueIsEmptyArray_ReturnsEmptyOriginalTargetFrameworks()
        {
            const string json = "{\"restore\":{\"originalTargetFrameworks\":[]}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.RestoreMetadata.OriginalTargetFrameworks);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreOriginalTargetFrameworksValueIsValid_ReturnsOriginalTargetFrameworks()
        {
            string[] expectedResults = { "a", "b" };
            string values = string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""));
            var json = $"{{\"restore\":{{\"originalTargetFrameworks\":[{values}]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResults, packageSpec.RestoreMetadata.OriginalTargetFrameworks);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreWarningPropertiesValueIsEmptyObject_ReturnsWarningProperties()
        {
            var expectedResult = new WarningProperties();
            const string json = "{\"restore\":{\"warningProperties\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.ProjectWideWarningProperties);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreWarningPropertiesValueIsValid_ReturnsWarningProperties()
        {
            var expectedResult = new WarningProperties(
                new HashSet<NuGetLogCode>() { NuGetLogCode.NU3000 },
                new HashSet<NuGetLogCode>() { NuGetLogCode.NU3001 },
                allWarningsAsErrors: true,
                new HashSet<NuGetLogCode>());
            var json = $"{{\"restore\":{{\"warningProperties\":{{\"allWarningsAsErrors\":{expectedResult.AllWarningsAsErrors.ToString().ToLowerInvariant()}," +
                $"\"warnAsError\":[\"{expectedResult.WarningsAsErrors.Single()}\"],\"noWarn\":[\"{expectedResult.NoWarn.Single()}\"]}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.ProjectWideWarningProperties);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreRestoreLockPropertiesValueIsEmptyObject_ReturnsRestoreLockProperties()
        {
            var expectedResult = new RestoreLockProperties();
            const string json = "{\"restore\":{\"restoreLockProperties\":{}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.RestoreLockProperties);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreRestoreLockPropertiesValueIsValid_ReturnsRestoreLockProperties()
        {
            var expectedResult = new RestoreLockProperties(
                restorePackagesWithLockFile: "a",
                nuGetLockFilePath: "b",
                restoreLockedMode: true); ;
            var json = $"{{\"restore\":{{\"restoreLockProperties\":{{\"restoreLockedMode\":{expectedResult.RestoreLockedMode.ToString().ToLowerInvariant()}," +
                $"\"restorePackagesWithLockFile\":\"{expectedResult.RestorePackagesWithLockFile}\"," +
                $"\"nuGetLockFilePath\":\"{expectedResult.NuGetLockFilePath}\"}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RestoreMetadata.RestoreLockProperties);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("\"\"")]
        [InlineData("\"a\"")]
        public void GetPackageSpec_WhenRestorePackagesConfigPathValueIsValidAndProjectStyleValueIsNotPackagesConfig_DoesNotReturnPackagesConfigPath(
            string value)
        {
            var json = $"{{\"restore\":{{\"projectStyle\":\"PackageReference\",\"packagesConfigPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.IsNotType<PackagesConfigProjectRestoreMetadata>(packageSpec.RestoreMetadata);
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenRestorePackagesConfigPathValueIsValidAndProjectStyleValueIsPackagesConfig_ReturnsPackagesConfigPath(
            string value,
            string expectedValue)
        {
            var json = $"{{\"restore\":{{\"projectStyle\":\"PackagesConfig\",\"packagesConfigPath\":{value}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.IsType<PackagesConfigProjectRestoreMetadata>(packageSpec.RestoreMetadata);
            Assert.Equal(expectedValue, ((PackagesConfigProjectRestoreMetadata)packageSpec.RestoreMetadata).PackagesConfigPath);
        }

        [Fact]
        public void GetPackageSpec_WhenRestoreSettingsValueIsEmptyObject_ReturnsRestoreSettings()
        {
            var expectedResult = new ProjectRestoreSettings();
            const string json = "{\"restoreSettings\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RestoreSettings);
        }

        [Fact]
        public void GetPackageSpec_WhenRuntimesValueIsEmptyObject_ReturnsRuntimes()
        {
            var expectedResult = RuntimeGraph.Empty;
            const string json = "{\"runtimes\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_WhenRuntimesValueIsValidWithImports_ReturnsRuntimes()
        {
            var runtimeDescription = new RuntimeDescription(
                runtimeIdentifier: "a",
                inheritedRuntimes: new[] { "b", "c" },
                Enumerable.Empty<RuntimeDependencySet>());
            var expectedResult = new RuntimeGraph(new[] { runtimeDescription });
            var json = $"{{\"runtimes\":{{\"{runtimeDescription.RuntimeIdentifier}\":{{\"#import\":[" +
                $"{string.Join(",", runtimeDescription.InheritedRuntimes.Select(runtime => $"\"{runtime}\""))}]}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_WhenRuntimesValueIsValidWithDependencySet_ReturnsRuntimes()
        {
            var dependencySet = new RuntimeDependencySet(id: "b");
            var runtimeDescription = new RuntimeDescription(
                runtimeIdentifier: "a",
                inheritedRuntimes: Enumerable.Empty<string>(),
                runtimeDependencySets: new[] { dependencySet });
            var expectedResult = new RuntimeGraph(new[] { runtimeDescription });
            var json = $"{{\"runtimes\":{{\"{runtimeDescription.RuntimeIdentifier}\":{{\"{dependencySet.Id}\":{{}}}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_WhenRuntimesValueIsValidWithDependencySetWithDependency_ReturnsRuntimes()
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
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_WhenSupportsValueIsEmptyObject_ReturnsSupports()
        {
            var expectedResult = RuntimeGraph.Empty;
            const string json = "{\"supports\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_WhenSupportsValueIsValidWithCompatibilityProfiles_ReturnsSupports()
        {
            var profile = new CompatibilityProfile(name: "a");
            var expectedResult = new RuntimeGraph(new[] { profile });
            var json = $"{{\"supports\":{{\"{profile.Name}\":{{}}}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

        [Fact]
        public void GetPackageSpec_WhenSupportsValueIsValidWithCompatibilityProfilesAndFrameworkRuntimePairs_ReturnsSupports()
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
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.RuntimeGraph);
        }

#pragma warning disable CS0612 // Type or member is obsolete
        [Fact]
        public void GetPackageSpec_WhenScriptsValueIsEmptyObject_ReturnsScripts()
        {
            const string json = "{\"scripts\":{}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Empty(packageSpec.Scripts);
        }

        [Fact]
        public void GetPackageSpec_WhenScriptsValueIsInvalid_Throws()
        {
            var json = "{\"scripts\":{\"a\":0}}";

            FileFormatException exception = Assert.Throws<FileFormatException>(() => GetPackageSpec(json));

            Assert.Equal("The value of a script in 'project.json' can only be a string or an array of strings", exception.Message);
            Assert.Equal(1, exception.Line);
            Assert.Equal(17, exception.Column);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void GetPackageSpec_WhenScriptsValueIsValid_ReturnsScripts()
        {
            const string name0 = "a";
            const string name1 = "b";
            const string script0 = "c";
            const string script1 = "d";
            const string script2 = "e";

            var json = $"{{\"scripts\":{{\"{name0}\":\"{script0}\",\"{name1}\":[\"{script1}\",\"{script2}\"]}}}}";
            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Collection(
                packageSpec.Scripts,
                actualResult =>
                {
                    Assert.Equal(name0, actualResult.Key);
                    Assert.Collection(
                        actualResult.Value,
                        actualScript => Assert.Equal(script0, actualScript));
                },
                actualResult =>
                {
                    Assert.Equal(name1, actualResult.Key);
                    Assert.Collection(
                        actualResult.Value,
                        actualScript => Assert.Equal(script1, actualScript),
                        actualScript => Assert.Equal(script2, actualScript));
                });
        }
#pragma warning restore CS0612 // Type or member is obsolete

        [Theory]
        [InlineData("null", null)]
        [InlineData("\"\"", "")]
        [InlineData("\"a\"", "a")]
        public void GetPackageSpec_WhenTitleValueIsValid_ReturnsTitle(string value, string expectedResult)
        {
            var json = $"{{\"title\":{value}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.Title);
        }

        [Fact]
        public void GetPackageSpec_WhenNameIsNull_RestoreMetadataProvidesFallbackName()
        {
            const string expectedResult = "a";
            var json = $"{{\"restore\":{{\"projectName\":\"{expectedResult}\"}}}}";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.Name);
        }

        [Theory]
        [InlineData("{\"restore\":{\"projectJsonPath\":\"a\"}}")]
        [InlineData("{\"restore\":{\"projectPath\":\"a\"}}")]
        [InlineData("{\"restore\":{\"projectJsonPath\":\"a\",\"projectPath\":\"b\"}}")]
        public void GetPackageSpec_WhenFilePathIsNull_RestoreMetadataProvidesFallbackFilePath(string json)
        {
            const string expectedResult = "a";

            PackageSpec packageSpec = GetPackageSpec(json);

            Assert.Equal(expectedResult, packageSpec.FilePath);
        }

        [Fact]
        public void GetTargetFrameworkInformation_WithAnAlias()
        {
            TargetFrameworkInformation framework = GetFramework("{\"frameworks\":{\"net46\":{ \"targetAlias\" : \"alias\"}}}");

            Assert.Equal("alias", framework.TargetAlias);
        }

        [Fact]
        public void PackageSpecReader_ReadsRestoreMetadataWithAliases()
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

            var actual = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var metadata = actual.RestoreMetadata;
            var warningProperties = actual.RestoreMetadata.ProjectWideWarningProperties;

            Assert.NotNull(metadata);
            Assert.Equal("alias", metadata.TargetFrameworks.Single().TargetAlias);
        }


        [Fact]
        public void PackageSpecReader_Read()
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

        [Fact]
        public void GetPackageSpec_WithSecondaryFrameworks_ReturnsTargetFrameworkInformationWithDualCompatibilityFramework()
        {
            var json = $"{{\"frameworks\":{{\"net5.0\":{{\"secondaryFramework\": \"native\"}}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);
            framework.FrameworkName.Should().BeOfType<DualCompatibilityFramework>();
            var dualCompatibilityFramework = framework.FrameworkName as DualCompatibilityFramework;
            dualCompatibilityFramework.RootFramework.Should().Be(FrameworkConstants.CommonFrameworks.Net50);
            dualCompatibilityFramework.SecondaryFramework.Should().Be(FrameworkConstants.CommonFrameworks.Native);
        }

        [Fact]
        public void GetPackageSpec_WithAssetTargetFallbackAndWithSecondaryFrameworks_ReturnsTargetFrameworkInformationWithDualCompatibilityFramework()
        {
            var json = $"{{\"frameworks\":{{\"net5.0\":{{\"assetTargetFallback\": true, \"imports\": [\"net472\", \"net471\"], \"secondaryFramework\": \"native\" }}}}}}";

            TargetFrameworkInformation framework = GetFramework(json);
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

        [Fact]
        public void GetPackageSpec_WithRestoreAuditProperties_ReturnsRestoreAuditProperties()
        {
            // Arrange
            var json = $"{{\"restore\":{{\"restoreAuditProperties\":{{\"enableAudit\": \"a\", \"auditLevel\": \"b\", \"auditMode\": \"c\"}}}}}}";

            // Act
            PackageSpec packageSpec = GetPackageSpec(json);

            // Assert
            packageSpec.RestoreMetadata.RestoreAuditProperties.EnableAudit.Should().Be("a");
            packageSpec.RestoreMetadata.RestoreAuditProperties.AuditLevel.Should().Be("b");
            packageSpec.RestoreMetadata.RestoreAuditProperties.AuditMode.Should().Be("c");
        }

        private static PackageSpec GetPackageSpec(string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return JsonPackageSpecReader.GetPackageSpec(stream, name: null, packageSpecPath: null, snapshotValue: null);
            }
        }

        private static LibraryDependency GetDependency(string json)
        {
            PackageSpec packageSpec = GetPackageSpec(json);

            return packageSpec.Dependencies.Single();
        }

        private static TargetFrameworkInformation GetFramework(string json)
        {
            PackageSpec packageSpec = GetPackageSpec(json);

            return packageSpec.TargetFrameworks.Single();
        }

        private static LibraryDependency GetFrameworksDependency(string json)
        {
            TargetFrameworkInformation framework = GetFramework(json);

            return framework.Dependencies.Single();
        }

        private static FrameworkDependency GetFrameworksFrameworkReference(string json)
        {
            TargetFrameworkInformation framework = GetFramework(json);

            return framework.FrameworkReferences.Single();
        }
    }
}
