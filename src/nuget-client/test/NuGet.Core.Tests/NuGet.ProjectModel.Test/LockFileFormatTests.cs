// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;
using static NuGet.Test.Utility.TestPackagesCore;

namespace NuGet.ProjectModel.Test
{
    public class LockFileFormatTests
    {
        // Verify the value of locked has no impact on the parsed lock file
        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]

        public void LockFileFormat_LockedPropertyIsIgnored(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContentTrue = @"{
  ""locked"": true,
  ""version"": 1,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""dependencies"": {
          ""Frob"": ""[4.0.20, )""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""WFRsJnfRzXYIiDJRbTXGctncx6Hw1F/uS2c5a5CzUwHuA3D/CM152F2HjWt12dLgH0BOcGvcRjKl2AfJ6MnHVg=="",
      ""type"": ""package"",
      ""files"": [
        ""_rels/.rels"",
        ""System.Runtime.nuspec"",
        ""lib/DNXCore50/System.Runtime.dll"",
        ""lib/netcore50/System.Runtime.dll"",
        ""lib/net46/_._"",
        ""ref/dotnet/System.Runtime.dll"",
        ""runtimes/win8-aot/lib/netcore50/System.Runtime.dll"",
        ""ref/net46/_._"",
        ""package/services/metadata/core-properties/b7eb2b260f1846d69b1ccf1a4e614180.psmdcp"",
        ""[Content_Types].xml""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  }
}";

            var lockFileContentMissing = @"{
  ""version"": 1,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""dependencies"": {
          ""Frob"": ""[4.0.20, )""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""WFRsJnfRzXYIiDJRbTXGctncx6Hw1F/uS2c5a5CzUwHuA3D/CM152F2HjWt12dLgH0BOcGvcRjKl2AfJ6MnHVg=="",
      ""type"": ""package"",
      ""files"": [
        ""_rels/.rels"",
        ""System.Runtime.nuspec"",
        ""lib/DNXCore50/System.Runtime.dll"",
        ""lib/netcore50/System.Runtime.dll"",
        ""lib/net46/_._"",
        ""ref/dotnet/System.Runtime.dll"",
        ""runtimes/win8-aot/lib/netcore50/System.Runtime.dll"",
        ""ref/net46/_._"",
        ""package/services/metadata/core-properties/b7eb2b260f1846d69b1ccf1a4e614180.psmdcp"",
        ""[Content_Types].xml""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  }
}";

            var lockFileContentFalse = @"{
  ""locked"": false,
  ""version"": 1,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""dependencies"": {
          ""Frob"": ""[4.0.20, )""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""WFRsJnfRzXYIiDJRbTXGctncx6Hw1F/uS2c5a5CzUwHuA3D/CM152F2HjWt12dLgH0BOcGvcRjKl2AfJ6MnHVg=="",
      ""type"": ""package"",
      ""files"": [
        ""_rels/.rels"",
        ""System.Runtime.nuspec"",
        ""lib/DNXCore50/System.Runtime.dll"",
        ""lib/netcore50/System.Runtime.dll"",
        ""lib/net46/_._"",
        ""ref/dotnet/System.Runtime.dll"",
        ""runtimes/win8-aot/lib/netcore50/System.Runtime.dll"",
        ""ref/net46/_._"",
        ""package/services/metadata/core-properties/b7eb2b260f1846d69b1ccf1a4e614180.psmdcp"",
        ""[Content_Types].xml""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  }
}";

            var lockFileFormat = new LockFileFormat();

            // Act
#pragma warning disable CS0612 // Type or member is obsolete
            var lockFileTrue = Parse(lockFileContentTrue, "In Memory", environmentVariableReader);
            var lockFileFalse = Parse(lockFileContentFalse, "In Memory", environmentVariableReader);
            var lockFileMissing = Parse(lockFileContentMissing, "In Memory", environmentVariableReader);
#pragma warning restore CS0612 // Type or member is obsolete

            var lockFileTrueString = lockFileFormat.Render(lockFileTrue);
            var lockFileFalseString = lockFileFormat.Render(lockFileFalse);
            var lockFileMissingString = lockFileFormat.Render(lockFileMissing);

            // Assert
            Assert.Equal(lockFileTrue, lockFileFalse);
            Assert.Equal(lockFileTrue, lockFileMissing);

            Assert.Equal(lockFileTrueString, lockFileFalseString);
            Assert.Equal(lockFileTrueString, lockFileMissingString);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsLockFileWithNoTools(IEnvironmentVariableReader environmentVariableReader)
        {
            var lockFileContent = @"{
  ""version"": 1,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""dependencies"": {
          ""Frob"": ""[4.0.20, )""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""WFRsJnfRzXYIiDJRbTXGctncx6Hw1F/uS2c5a5CzUwHuA3D/CM152F2HjWt12dLgH0BOcGvcRjKl2AfJ6MnHVg=="",
      ""type"": ""package"",
      ""files"": [
        ""_rels/.rels"",
        ""System.Runtime.nuspec"",
        ""lib/DNXCore50/System.Runtime.dll"",
        ""lib/netcore50/System.Runtime.dll"",
        ""lib/net46/_._"",
        ""ref/dotnet/System.Runtime.dll"",
        ""runtimes/win8-aot/lib/netcore50/System.Runtime.dll"",
        ""ref/net46/_._"",
        ""package/services/metadata/core-properties/b7eb2b260f1846d69b1ccf1a4e614180.psmdcp"",
        ""[Content_Types].xml""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  }
}";
            var lockFile = Parse(lockFileContent, "In Memory", environmentVariableReader);

            Assert.Equal(1, lockFile.Version);

            var target = lockFile.Targets.Single();
            Assert.Equal(NuGetFramework.Parse("dotnet"), target.TargetFramework);
            var runtimeTargetLibrary = target.Libraries.Single();
            Assert.Equal("System.Runtime", runtimeTargetLibrary.Name);
            Assert.Equal(NuGetVersion.Parse("4.0.20-beta-22927"), runtimeTargetLibrary.Version);
            Assert.Equal(0, runtimeTargetLibrary.NativeLibraries.Count);
            Assert.Equal(0, runtimeTargetLibrary.ResourceAssemblies.Count);
            Assert.Equal(0, runtimeTargetLibrary.FrameworkAssemblies.Count);
            Assert.Equal(0, runtimeTargetLibrary.RuntimeAssemblies.Count);
            Assert.Equal("ref/dotnet/System.Runtime.dll", runtimeTargetLibrary.CompileTimeAssemblies.Single().Path);

            var dep = runtimeTargetLibrary.Dependencies.Single();
            Assert.Equal("Frob", dep.Id);
            Assert.Equal(new VersionRange(NuGetVersion.Parse("4.0.20")), dep.VersionRange);

            var runtimeLibrary = lockFile.Libraries.Single();
            Assert.Equal("System.Runtime", runtimeLibrary.Name);
            Assert.Equal(NuGetVersion.Parse("4.0.20-beta-22927"), runtimeLibrary.Version);
            Assert.False(string.IsNullOrEmpty(runtimeLibrary.Sha512));
            Assert.Equal(LibraryType.Package, runtimeLibrary.Type);
            Assert.Equal(10, runtimeLibrary.Files.Count);

            var emptyDepGroup = lockFile.ProjectFileDependencyGroups.First();
            Assert.True(string.IsNullOrEmpty(emptyDepGroup.FrameworkName));
            Assert.Equal("System.Runtime [4.0.10-beta-*, )", emptyDepGroup.Dependencies.Single());
            var netPlatDepGroup = lockFile.ProjectFileDependencyGroups.Last();
            Assert.Equal(NuGetFramework.Parse("dotnet").DotNetFrameworkName, netPlatDepGroup.FrameworkName);
            Assert.Empty(netPlatDepGroup.Dependencies);
        }

        [Theory]
        [InlineData("1.0.0", "1.0.0")]
        [InlineData("1.0.0-beta", "1.0.0-beta")]
        [InlineData("1.0.0-*", "1.0.0")]
        [InlineData("1.0.*", "1.0.0")]
        [InlineData("(1.0.*, )", "(1.0.0, )")]
        public void Test_WritePackageDependencyWithLegacyString(string version, string expectedVersion)
        {
            var package = new PackageDependency("a", VersionRange.Parse(version));

            using var writer = new JTokenWriter();

            JsonUtility.WritePackageDependencyWithLegacyString(writer, package);

            JToken actual = ((JProperty)writer.Token).Value;

            Assert.Equal(expectedVersion, actual);
        }

        [Theory]
        [InlineData("1.0.0", "[1.0.0, )")]
        [InlineData("1.0.0-beta", "[1.0.0-beta, )")]
        [InlineData("1.0.0-*", "[1.0.0-*, )")]
        [InlineData("1.0.*", "[1.0.*, )")]
        [InlineData("(1.0.*, )", "(1.0.*, )")]
        public void Test_WritePackageDependency(string version, string expectedVersion)
        {
            var package = new PackageDependency("a", VersionRange.Parse(version));

            using var writer = new JTokenWriter();

            JsonUtility.WritePackageDependency(writer, package);

            JToken actual = ((JProperty)writer.Token).Value;

            Assert.Equal(expectedVersion, actual);
        }

        [Fact]
        public void LockFileFormat_WritesLockFile()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 2,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  }
}";
            var lockFile = new LockFile()
            {
                Version = 2
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.DotNet
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new NuGet.Packaging.Core.PackageDependency("Frob",
                new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup(FrameworkConstants.CommonFrameworks.DotNet.DotNetFrameworkName, Array.Empty<string>()));

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile));
            var expected = JObject.Parse(lockFileContent);

            // Assert
            Assert.Equal(expected.ToString(), output.ToString());
        }

        [Fact]
        public void LockFileFormat_WritesPackageSpec()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 2,
  ""targets"": {},
  ""libraries"": {},
  ""projectFileDependencyGroups"": {},
  ""project"": {
    ""frameworks"": {
      ""dotnet"": {}
    }
  }
}";
            var lockFile = new LockFile()
            {
                Version = 2,

                PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.DotNet
                    }
                })
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile));
            var expected = JObject.Parse(lockFileContent);

            // Assert
            Assert.Equal(expected.ToString(), output.ToString());
        }

        [Fact]
        public void Render_LockFileWithPackageFolder_WritesPackageFolder()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 2,
  ""targets"": {},
  ""libraries"": {},
  ""projectFileDependencyGroups"": {},
  ""packageFolders"": {
    ""a"": {}
  }
}";

            var lockFile = new LockFile
            {
                Version = 2,
                PackageFolders = new List<LockFileItem>
                {
                    new("a")
                }
            };

            var lockFileFormat = new LockFileFormat();

            // Act
            string actual = lockFileFormat.Render(lockFile);

            // Assert
            JObject expected = JObject.Parse(lockFileContent);
            JObject output = JObject.Parse(actual);

            Assert.Equal(expected.ToString(), output.ToString());
        }

        [Fact]
        public void Render_LockFileWithLibrary_WritesLibrary()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 2,
  ""targets"": {},
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""servicable"": true,
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""path"": ""foo"",
      ""msbuildProject"": ""bar"",
      ""hasTools"": true
    }
  },
  ""projectFileDependencyGroups"": {}
}";

            var lockFile = new LockFile
            {
                Version = 2,
                Libraries = new List<LockFileLibrary>
                {
                    new()
                    {
                        Name = "System.Runtime",
                        Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                        Type = LibraryType.Package,
                        Sha512 = "sup3rs3cur3",
                        IsServiceable = true,
                        Path = "foo",
                        MSBuildProject = "bar",
                        HasTools = true
                    }
                }
            };

            var lockFileFormat = new LockFileFormat();

            // Act
            string actual = lockFileFormat.Render(lockFile);

            // Assert
            JObject expected = JObject.Parse(lockFileContent);
            JObject output = JObject.Parse(actual);

            Assert.Equal(expected.ToString(), output.ToString());
        }

        [Fact]
        public void Render_LockFileWithTarget_WritesTarget()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    ""net6.0"": {
      ""Microsoft.AspNetCore.JsonPatch/6.0.4"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Microsoft.CSharp"": ""4.7.0"",
          ""Newtonsoft.Json"": ""13.0.1""
        },
        ""frameworkAssemblies"": [
          ""System.Configuration""
        ],
        ""compile"": {
          ""lib/net6.0/Microsoft.AspNetCore.JsonPatch.dll"": {}
        },
        ""runtime"": {
          ""lib/net6.0/Microsoft.AspNetCore.JsonPatch.dll"": {}
        },
        ""resource"": {
          ""lib/net45/cs/FSharp.Core.resources.dll"": {
            ""locale"": ""cs""
          }
        },
        ""contentFiles"": {
          ""baz"": {
            ""copyToOutput"": false
          },
          ""foo"": {
            ""copyToOutput"": true,
            ""outputPath"": ""bar""
          }
        }
      },
      ""Project10/1.0.0"": {
        ""type"": ""project"",
        ""framework"": "".NETCoreApp,Version=v6.0""
      },
      ""Microsoft.Extensions.ApiDescription.Server/3.0.0"": {
        ""type"": ""package"",
         ""build"": {
          ""build/Microsoft.Extensions.ApiDescription.Server.props"": {},
          ""build/Microsoft.Extensions.ApiDescription.Server.targets"": {}
        },
        ""buildMultiTargeting"": {
          ""buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.props"": {},
          ""buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.targets"": {}
        }
      },
       ""runtime.debian.8-x64.runtime.native.System.Security.Cryptography.OpenSsl/4.3.0"": {
        ""type"": ""package"",
        ""runtimeTargets"": {
          ""runtimes/debian.8-x64/native/System.Security.Cryptography.Native.OpenSsl.so"": {
            ""assetType"": ""native"",
            ""rid"": ""debian.8-x64""
          }
        }
      }
    }
  },
  ""libraries"": {},
  ""projectFileDependencyGroups"": {}
}";

            var lockFile = new LockFile
            {
                Version = 3,
                Targets = new List<LockFileTarget>
                {
                    new()
                    {
                        TargetFramework = FrameworkConstants.CommonFrameworks.Net60,
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new()
                            {
                                Name = "Microsoft.AspNetCore.JsonPatch",
                                Version = NuGetVersion.Parse("6.0.4"),
                                Type = LibraryType.Package,
                                Dependencies = new List<PackageDependency>
                                {
                                    new("Microsoft.CSharp", new VersionRange(NuGetVersion.Parse("4.7.0"))),
                                    new("Newtonsoft.Json", new VersionRange(NuGetVersion.Parse("13.0.1"))),
                                },
                                CompileTimeAssemblies = new List<LockFileItem>()
                                {
                                    new("lib/net6.0/Microsoft.AspNetCore.JsonPatch.dll")
                                },
                                FrameworkAssemblies = new List<string>
                                {
                                    "System.Configuration"
                                },
                                RuntimeAssemblies = new List<LockFileItem>
                                {
                                    new("lib/net6.0/Microsoft.AspNetCore.JsonPatch.dll")
                                },
                                ResourceAssemblies = new List<LockFileItem>
                                {
                                    new("lib/net45/cs/FSharp.Core.resources.dll")
                                    {
                                        Properties =
                                        {
                                            ["locale"] = "cs"
                                        }
                                    }
                                },
                                ContentFiles = new List<LockFileContentFile>
                                {
                                    new("foo")
                                    {
                                        OutputPath = "bar",
                                        CopyToOutput = true
                                    },
                                    new("baz")
                                    {
                                        CopyToOutput = false
                                    }
                                }
                            },
                            new()
                            {
                                Name = "Project10",
                                Version = NuGetVersion.Parse("1.0.0"),
                                Type = "project",
                                Framework = ".NETCoreApp,Version=v6.0",
                            },
                            new()
                            {
                                Name = "Microsoft.Extensions.ApiDescription.Server",
                                Version = NuGetVersion.Parse("3.0.0"),
                                Type = "package",
                                Build = new List<LockFileItem>
                                {
                                    new("build/Microsoft.Extensions.ApiDescription.Server.props"),
                                    new("build/Microsoft.Extensions.ApiDescription.Server.targets"),
                                },
                                BuildMultiTargeting = new List<LockFileItem>()
                                {
                                    new("buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.props"),
                                    new("buildMultiTargeting/Microsoft.Extensions.ApiDescription.Server.targets")
                                }
                            },
                            new()
                            {
                                Name = "runtime.debian.8-x64.runtime.native.System.Security.Cryptography.OpenSsl",
                                Version = NuGetVersion.Parse("4.3.0"),
                                Type = "package",
                                RuntimeTargets = new List<LockFileRuntimeTarget>
                                {
                                    new("runtimes/debian.8-x64/native/System.Security.Cryptography.Native.OpenSsl.so")
                                    {
                                        AssetType = "native",
                                        Runtime = "debian.8-x64"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var lockFileFormat = new LockFileFormat();

            // Act
            string actual = lockFileFormat.Render(lockFile);

            // Assert
            JObject expected = JObject.Parse(lockFileContent);
            JObject output = JObject.Parse(actual);

            Assert.Equal(expected.ToString(), output.ToString());
        }

        [Fact]
        public void LockFileFormat_ReadsPackageSpec()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 2,
  ""targets"": {},
  ""libraries"": {},
  ""projectFileDependencyGroups"": {},
  ""project"":   {
    ""version"": ""1.0.0"",
    ""restore"": {
      ""projectUniqueName"": ""X:\\ProjectPath\\ProjectPath.csproj"",
      ""projectName"": ""ProjectPath"",
      ""projectPath"": ""X:\\ProjectPath\\ProjectPath.csproj"",
      ""outputPath"": ""X:\\ProjectPath\\obj\\"",
      ""projectStyle"": ""PackageReference"",
      ""UsingMicrosoftNETSdk"": false,
      ""originalTargetFrameworks"": [
        ""netcoreapp10""
      ],
      ""frameworks"": {
        ""netcoreapp1.0"": {
          ""targetAlias"": ""netcoreapp10"",
          ""projectReferences"": {}
        }
      }
    },
    ""frameworks"": {
      ""netcoreapp1.0"": {
        ""targetAlias"": ""netcoreapp10"",
        ""dependencies"": {
         ""Microsoft.NET.Sdk"": {
                ""suppressParent"": ""All"",
                ""target"": ""Package"",
                ""version"": ""[1.0.0-alpha-20161104-2, )""
          },
          ""Microsoft.NETCore.App"": {
            ""target"": ""Package"",
            ""version"": ""[1.0.1, )""
          }
        }
      }
    }
  }
}";
            var lockFile = new LockFile()
            {
                Version = 2,

                PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        TargetAlias = "netcoreapp10",
                        Dependencies =
                        [
                            new LibraryDependency
                            {
                                LibraryRange = new LibraryRange(
                                    "Microsoft.NETCore.App",
                                    new VersionRange(
                                        minVersion: new NuGetVersion("1.0.1"),
                                        originalString: "1.0.1"),
                                    LibraryDependencyTarget.Package)
                            },
                            new LibraryDependency
                            {
                                LibraryRange = new LibraryRange(
                                    "Microsoft.NET.Sdk",
                                    new VersionRange(
                                        minVersion: new NuGetVersion("1.0.0-alpha-20161104-2"),
                                        originalString: "1.0.0-alpha-20161104-2"),
                                    LibraryDependencyTarget.Package),
                                SuppressParent = LibraryIncludeFlags.All
                            }
                        ]
                    }
                })
                {
                    Version = new NuGetVersion("1.0.0"),
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        ProjectUniqueName = @"X:\ProjectPath\ProjectPath.csproj",
                        ProjectName = "ProjectPath",
                        ProjectPath = @"X:\ProjectPath\ProjectPath.csproj",
                        OutputPath = @"X:\ProjectPath\obj\",
                        ProjectStyle = ProjectStyle.PackageReference,
                        OriginalTargetFrameworks = new[] { "netcoreapp10" },
                        TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                        {
                            new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            {
                                TargetAlias = "netcoreapp10",
                            }
                        }
                    }
                }
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile));
            var expected = JObject.Parse(lockFileContent);

            // Assert
            Assert.Equal(expected.ToString(), output.ToString());
        }

        [Fact]
        public void LockFileFormat_WritesMinimalErrorMessage()
        {

            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.DotNet
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new Packaging.Core.PackageDependency("Frob",
                new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup(FrameworkConstants.CommonFrameworks.DotNet.DotNetFrameworkName, Array.Empty<string>()));

            lockFile.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message")
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile)).ToString();
            var expected = JObject.Parse(lockFileContent).ToString();

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void LockFileFormat_WritesMultipleErrorMessages()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    },
    {
      ""code"": ""NU1000"",
      ""level"": ""Warning"",
      ""warningLevel"": 1,
      ""message"": ""test log message""
    },
    {
      ""code"": ""NU1000"",
      ""level"": ""Warning"",
      ""warningLevel"": 2,
      ""message"": ""test log message""
    },
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.DotNet
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new Packaging.Core.PackageDependency("Frob",
                new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup(FrameworkConstants.CommonFrameworks.DotNet.DotNetFrameworkName, Array.Empty<string>()));

            lockFile.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message"),
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1000, "test log message")
                {
                    WarningLevel = WarningLevel.Severe
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1000, "test log message")
                {
                    WarningLevel = WarningLevel.Important
                },
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message")
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile)).ToString();
            var expected = JObject.Parse(lockFileContent).ToString();

            // Assert
            Assert.Equal(expected, output);
        }


        [Fact]
        public void LockFileFormat_WritesFullErrorMessage()
        {

            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""filePath"": ""kung\\fu\\fighting.targets"",
      ""startLineNumber"": 11,
      ""startColumnNumber"": 2,
      ""endLineNumber"": 11,
      ""endColumnNumber"": 10,
      ""message"": ""test log message"",
      ""libraryId"": ""nuget.versioning"",
      ""targetGraphs"": [
        ""net46"",
        ""netcoreapp1.0"",
        ""netstandard1.6""
      ]
    }
  ]
}";
            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.DotNet
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new Packaging.Core.PackageDependency("Frob",
                new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup(FrameworkConstants.CommonFrameworks.DotNet.DotNetFrameworkName, Array.Empty<string>()));

            lockFile.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning"
                }
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile)).ToString();
            var expected = JObject.Parse(lockFileContent).ToString();

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void LockFileFormat_WritesErrorMessageWithFilePathSameAsProjectPath()
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""project"": {
    ""restore"": {
      ""projectPath"": ""kung\\fu\\fighting.csproj"",
      ""UsingMicrosoftNETSdk"": false,
    }
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""startLineNumber"": 11,
      ""startColumnNumber"": 2,
      ""endLineNumber"": 11,
      ""endColumnNumber"": 10,
      ""message"": ""test log message"",
      ""libraryId"": ""nuget.versioning"",
      ""targetGraphs"": [
        ""net46"",
        ""netcoreapp1.0"",
        ""netstandard1.6""
      ]
    }
  ]
}";
            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.DotNet
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new Packaging.Core.PackageDependency("Frob",
                new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup(FrameworkConstants.CommonFrameworks.DotNet.DotNetFrameworkName, Array.Empty<string>()));
            lockFile.PackageSpec = new PackageSpec(new List<TargetFrameworkInformation>())
            {
                RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectPath = "kung\\fu\\fighting.csproj"
                }
            };

            lockFile.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message")
                {
                    FilePath = @"kung\fu\fighting.csproj",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning"
                }
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile)).ToString();
            var expected = JObject.Parse(lockFileContent).ToString();

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void LockFileFormat_WritesMinimalWarningMessageWithWarningLevel()
        {

            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Warning"",
      ""warningLevel"": 2,
      ""message"": ""test log message""
    }
  ]
}";
            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.DotNet
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new Packaging.Core.PackageDependency("Frob",
                new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups.Add(
                new ProjectFileDependencyGroup(FrameworkConstants.CommonFrameworks.DotNet.DotNetFrameworkName, Array.Empty<string>()));

            lockFile.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1000, "test log message")
                {
                    WarningLevel = WarningLevel.Important
                }
            };

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile)).ToString();
            var expected = JObject.Parse(lockFileContent).ToString();

            // Assert
            Assert.Equal(expected, output);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_Read_WithMalformedJson_LogsErrorMesage(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = "{ corrupt_file: ";
            var filePath = "a/file/path";
            Mock<ILogger> logger = new Mock<ILogger>();
            var lockFile = Parse(lockFileContent, filePath, environmentVariableReader, logger.Object);

            Assert.NotNull(lockFile);
            Assert.Equal(int.MinValue, lockFile.Version);
            Assert.Equal(filePath, lockFile.Path);
            logger.Verify(x => x.LogError(It.IsAny<string>()));
        }


        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsMinimalErrorMessage(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.Null(logMessage.FilePath);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(0, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }


        [Theory]
        [InlineData(LockFileReadFlags.CentralTransitiveDependencyGroups)]
        [InlineData(LockFileReadFlags.Libraries)]
        [InlineData(LockFileReadFlags.PackageFolders)]
        [InlineData(LockFileReadFlags.PackageSpec)]
        [InlineData(LockFileReadFlags.ProjectFileDependencyGroups)]
        [InlineData(LockFileReadFlags.Targets)]
        [InlineData(LockFileReadFlags.PackageFolders | LockFileReadFlags.Targets)]
        [InlineData(LockFileReadFlags.All)]
        public void LockFileFormat_ReadUsesReadFlags_WithNewtonsoftEnvReader(LockFileReadFlags flags)
        {
            // Returns "NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING: true" which means Newtonsoft.Json code path is used to parse the file
            var environmentVariableReader = LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader().First()[0] as IEnvironmentVariableReader;

            LockFileFormat_ReadUsesReadFlags_WithEnvReader(flags, environmentVariableReader);
        }

        [Theory]
        [InlineData(LockFileReadFlags.CentralTransitiveDependencyGroups)]
        [InlineData(LockFileReadFlags.Libraries)]
        [InlineData(LockFileReadFlags.PackageFolders)]
        [InlineData(LockFileReadFlags.PackageSpec)]
        [InlineData(LockFileReadFlags.ProjectFileDependencyGroups)]
        [InlineData(LockFileReadFlags.Targets)]
        [InlineData(LockFileReadFlags.PackageFolders | LockFileReadFlags.Targets)]
        [InlineData(LockFileReadFlags.All)]
        public void LockFileFormat_ReadUsesReadFlags_WithSystemTextJsonEnvReader(LockFileReadFlags flags)
        {
            // Returns "NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING: false" which means System.Text.Json code path is used to parse the file
            var environmentVariableReader = LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader().Skip(1).First()[0] as IEnvironmentVariableReader;

            LockFileFormat_ReadUsesReadFlags_WithEnvReader(flags, environmentVariableReader);
        }

        private void LockFileFormat_ReadUsesReadFlags_WithEnvReader(LockFileReadFlags flags, IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {}
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""type"": ""package"",
      ""files"": []
    }
  },
  ""projectFileDependencyGroups"": {
    "".NETPlatform,Version=v5.0"": []
  },
  ""centralTransitiveDependencyGroups"": {
    "".NETPlatform,Version=v5.0"": {
      ""Newtonsoft.Json"": {
        ""version"": ""1.0.0""
      }
    }
  },
  ""packageFolders"": {
    ""a"": {}
  },
  ""project"":   {
    ""restore"": {
      ""projectName"": ""ProjectPath""
    }
  }
}";

            // Act
            var lockFile = Parse(lockFileContent, "In Memory", environmentVariableReader, logger: null, flags);

            // Assert
            Assert.NotNull(lockFile);
            Assert.Equal(3, lockFile.Version);
            Assert.Equal(flags.HasFlag(LockFileReadFlags.CentralTransitiveDependencyGroups) ? 1 : 0, lockFile.CentralTransitiveDependencyGroups.Count);
            Assert.Equal(flags.HasFlag(LockFileReadFlags.Libraries) ? 1 : 0, lockFile.Libraries.Count);
            Assert.Equal(flags.HasFlag(LockFileReadFlags.PackageFolders) ? 1 : 0, lockFile.PackageFolders.Count);
            Assert.Equal(flags.HasFlag(LockFileReadFlags.PackageSpec) ? "ProjectPath" : null, lockFile.PackageSpec.Name);
            Assert.Equal(flags.HasFlag(LockFileReadFlags.ProjectFileDependencyGroups) ? 1 : 0, lockFile.ProjectFileDependencyGroups.Count);
            Assert.Equal(flags.HasFlag(LockFileReadFlags.Targets) ? 1 : 0, lockFile.Targets.Count);
        }


        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_SkipsInvalidErrorMessage(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""InvalidCode"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    },
    {
      ""code"": ""NU1000"",
      ""level"": ""InvalidCode"",
      ""message"": ""test log message""
    },
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.Null(logMessage.FilePath);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(0, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsFullErrorMessage(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""filePath"": ""kung\\fu\\fighting.targets"",
      ""startLineNumber"": 11,
      ""startColumnNumber"": 2,
      ""endLineNumber"": 11,
      ""endColumnNumber"": 10,
      ""message"": ""test log message"",
      ""targetGraphs"": [
        ""net46"",
        ""netcoreapp1.0"",
        ""netstandard1.6""
      ]
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.Equal("kung\\fu\\fighting.targets", logMessage.FilePath);
            Assert.Equal(11, logMessage.StartLineNumber);
            Assert.Equal(11, logMessage.EndLineNumber);
            Assert.Equal(2, logMessage.StartColumnNumber);
            Assert.Equal(10, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(3, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_SafeRead(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Warning"",
      ""warningLevel"": 2,
      ""filePath"": ""kung\\fu\\fighting.targets"",
      ""startLineNumber"": 11,
      ""startColumnNumber"": 2,
      ""endLineNumber"": 11,
      ""endColumnNumber"": 10,
      ""message"": ""test log message"",
      ""targetGraphs"": [
        ""net46"",
        ""netcoreapp1.0"",
        ""netstandard1.6""
      ]
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                var reader = new LockFileFormat();
                lockFileObj = FileUtility.SafeRead(lockFile, (stream, path) => reader.Read(stream, NullLogger.Instance, path, environmentVariableReader, true));
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Warning, logMessage.Level);
            Assert.Equal(WarningLevel.Important, logMessage.WarningLevel);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.Equal("kung\\fu\\fighting.targets", logMessage.FilePath);
            Assert.Equal(11, logMessage.StartLineNumber);
            Assert.Equal(11, logMessage.EndLineNumber);
            Assert.Equal(2, logMessage.StartColumnNumber);
            Assert.Equal(10, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(3, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsWarningMessage(IEnvironmentVariableReader environmentVariableReader)
        {

            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Warning"",
      ""warningLevel"": 2,
      ""filePath"": ""kung\\fu\\fighting.targets"",
      ""startLineNumber"": 11,
      ""startColumnNumber"": 2,
      ""endLineNumber"": 11,
      ""endColumnNumber"": 10,
      ""message"": ""test log message"",
      ""targetGraphs"": [
        ""net46"",
        ""netcoreapp1.0"",
        ""netstandard1.6""
      ]
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Warning, logMessage.Level);
            Assert.Equal(WarningLevel.Important, logMessage.WarningLevel);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.Equal("kung\\fu\\fighting.targets", logMessage.FilePath);
            Assert.Equal(11, logMessage.StartLineNumber);
            Assert.Equal(11, logMessage.EndLineNumber);
            Assert.Equal(2, logMessage.StartColumnNumber);
            Assert.Equal(10, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(3, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsWarningMessageWithoutWarningLevel(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Warning"",
      ""filePath"": ""kung\\fu\\fighting.targets"",
      ""startLineNumber"": 11,
      ""startColumnNumber"": 2,
      ""endLineNumber"": 11,
      ""endColumnNumber"": 10,
      ""message"": ""test log message"",
      ""targetGraphs"": [
        ""net46"",
        ""netcoreapp1.0"",
        ""netstandard1.6""
      ]
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Warning, logMessage.Level);
            Assert.Equal(WarningLevel.Severe, logMessage.WarningLevel);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.Equal("kung\\fu\\fighting.targets", logMessage.FilePath);
            Assert.Equal(11, logMessage.StartLineNumber);
            Assert.Equal(11, logMessage.EndLineNumber);
            Assert.Equal(2, logMessage.StartColumnNumber);
            Assert.Equal(10, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(3, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }


        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsMultipleMessages(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    },
    {
      ""code"": ""NU1500"",
      ""level"": ""Warning"",
      ""warningLevel"": 1,
      ""message"": ""test warning message""
    },
    {
      ""code"": ""NU1500"",
      ""level"": ""Warning"",
      ""warningLevel"": 1,
      ""message"": ""test warning message""
    },
    {
      ""code"": ""NU1001"",
      ""level"": ""Error"",
      ""message"": ""test error message with type NU1001""
    },
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            LockFile lockFileObj = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.Equal(5, lockFileObj.LogMessages.Count());
            Assert.Equal(3, lockFileObj.LogMessages.Count(m => m.Level == LogLevel.Error));
            Assert.Equal(2, lockFileObj.LogMessages.Count(m => m.Level == LogLevel.Warning));
            Assert.Equal(2, lockFileObj.LogMessages.Count(m => m.Message == "test log message"));
            Assert.Equal(2, lockFileObj.LogMessages.Count(m => m.Message == "test warning message"));
            Assert.Equal(1, lockFileObj.LogMessages.Count(m => m.Message == "test error message with type NU1001"));
            Assert.Equal(2, lockFileObj.LogMessages.Count(m => m.Code == NuGetLogCode.NU1000));
            Assert.Equal(2, lockFileObj.LogMessages.Count(m => m.Code == NuGetLogCode.NU1500));
            Assert.Equal(1, lockFileObj.LogMessages.Count(m => m.Code == NuGetLogCode.NU1001));
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsLogMessageWithSameFilePathAndProjectPath(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""project"": {
    ""restore"": {
      ""projectPath"": ""kung\\fu\\fighting.csproj""
    }
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message"",
      ""filePath"": ""kung\\fu\\fighting.csproj""
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.NotNull(logMessage.FilePath);
            Assert.Equal("kung\\fu\\fighting.csproj", logMessage.FilePath);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(0, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsLogMessageWithNoFilePath(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.20-beta-22927"": {
        ""type"": ""package"",
        ""dependencies"": {
          ""Frob"": ""4.0.20""
        },
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.20-beta-22927"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""System.Runtime.nuspec""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime [4.0.10-beta-*, )""
    ],
    "".NETPlatform,Version=v5.0"": []
  },
  ""project"": {
    ""restore"": {
      ""projectPath"": ""kung\\fu\\fighting.csproj""
    }
  },
  ""logs"": [
    {
      ""code"": ""NU1000"",
      ""level"": ""Error"",
      ""message"": ""test log message""
    }
  ]
}";
            LockFile lockFileObj = null;
            IAssetsLogMessage logMessage = null;
            using (var lockFile = new TempFile())
            {

                File.WriteAllText(lockFile, lockFileContent);

                // Act
                lockFileObj = Read(lockFile, environmentVariableReader);
                logMessage = lockFileObj?.LogMessages?.First();
            }


            // Assert
            Assert.NotNull(lockFileObj);
            Assert.NotNull(logMessage);
            Assert.Equal(1, lockFileObj.LogMessages.Count());
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.Equal(NuGetLogCode.NU1000, logMessage.Code);
            Assert.NotNull(logMessage.FilePath);
            Assert.Equal("kung\\fu\\fighting.csproj", logMessage.FilePath);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(0, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsLockFileWithTools(IEnvironmentVariableReader environmentVariableReader)
        {
            var lockFileContent = @"{
              ""version"": 1,
              ""targets"": {
                "".NETPlatform,Version=v5.0"": {
                  ""GlobalTool/1.0.0"": {
                    ""dependencies"": {
                    },
                    ""tools"": {
                      ""tools/dotnet/any/test.dll"": {}
                    }
                  }
                }
              },
              ""libraries"": {
                ""GlobalTool/1.0.0"": {
                  ""sha512"": ""WFRsJnfRzXYIiDJRbTXGctncx6Hw1F/uS2c5a5CzUwHuA3D/CM152F2HjWt12dLgH0BOcGvcRjKl2AfJ6MnHVg=="",
                  ""type"": ""package"",
                  ""files"": [
                    ""_rels/.rels"",
                    ""GlobalTool.nuspec"",
                    ""tools/dotnet/any/test.dll"",
                    ""package/services/metadata/core-properties/b7eb2b260f1846d69b1ccf1a4e614180.psmdcp"",
                    ""[Content_Types].xml""
                  ]
                }
              },
              ""projectFileDependencyGroups"": {
                """": [
                  ""GlobalTool [1.0.0, )""
                ],
                "".NETPlatform,Version=v5.0"": []
              }
            }";
            var lockFile = Parse(lockFileContent, "In Memory", environmentVariableReader);

            Assert.Equal(1, lockFile.Version);

            var target = lockFile.Targets.Single();
            Assert.Equal(NuGetFramework.Parse("dotnet"), target.TargetFramework);

            var runtimeTargetLibrary = target.Libraries.Single();
            Assert.Equal("GlobalTool", runtimeTargetLibrary.Name);
            Assert.Equal(NuGetVersion.Parse("1.0.0"), runtimeTargetLibrary.Version);
            Assert.Equal(0, runtimeTargetLibrary.NativeLibraries.Count);
            Assert.Equal(0, runtimeTargetLibrary.ResourceAssemblies.Count);
            Assert.Equal(0, runtimeTargetLibrary.FrameworkAssemblies.Count);
            Assert.Equal(0, runtimeTargetLibrary.RuntimeAssemblies.Count);
            Assert.Equal(1, runtimeTargetLibrary.ToolsAssemblies.Count);
            Assert.Equal("tools/dotnet/any/test.dll", runtimeTargetLibrary.ToolsAssemblies.Single().Path);
            Assert.Equal(0, runtimeTargetLibrary.Dependencies.Count());

            var runtimeLibrary = lockFile.Libraries.Single();
            Assert.Equal("GlobalTool", runtimeLibrary.Name);
            Assert.Equal(NuGetVersion.Parse("1.0.0"), runtimeLibrary.Version);
            Assert.False(string.IsNullOrEmpty(runtimeLibrary.Sha512));
            Assert.Equal(LibraryType.Package, runtimeLibrary.Type);
            Assert.Equal(5, runtimeLibrary.Files.Count);

            var emptyDepGroup = lockFile.ProjectFileDependencyGroups.First();
            Assert.True(string.IsNullOrEmpty(emptyDepGroup.FrameworkName));
            Assert.Equal("GlobalTool [1.0.0, )", emptyDepGroup.Dependencies.Single());
            var netPlatDepGroup = lockFile.ProjectFileDependencyGroups.Last();
            Assert.Equal(NuGetFramework.Parse("dotnet").DotNetFrameworkName, netPlatDepGroup.FrameworkName);
            Assert.Empty(netPlatDepGroup.Dependencies);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFileFormat_ReadsLockFileWithEmbedAssemblies(IEnvironmentVariableReader environmentVariableReader)
        {
            var lockFileContent = @"{
              ""version"": 1,
              ""targets"": {
                "".NETCoreApp,Version=v2.1"": {
                  ""packageA.interop/1.0.0"": {
                    ""compile"": {
                      ""lib/netstandard2.0/packageA.interop.dll"": {}
                    },
                    ""embed"": {
                      ""embed/netstandard2.0/packageA.interop.dll"": {}
                    },
                    ""runtime"": {
                      ""lib/netstandard2.0/packageA.interop.dll"": {}
                    }
                  }
                }
              },
              ""libraries"": {
                ""packageA.interop/1.0.0"": {
                  ""sha512"": ""WFRsJnfRzXYIiDJRbTXGctncx6Hw1F/uS2c5a5CzUwHuA3D/CM152F2HjWt12dLgH0BOcGvcRjKl2AfJ6MnHVg=="",
                  ""type"": ""package"",
                  ""files"": [
                    ""_rels/.rels"",
                    ""packageA.interop.nuspec"",
                    ""lib/netstandard2.0/packageA.interop.dll"",
                    ""embed/netstandard2.0/packageA.interop.dll"",
                    ""package/services/metadata/core-properties/b7eb2b260f1846d69b1ccf1a4e614180.psmdcp"",
                    ""[Content_Types].xml""
                  ]
                }
              },
              ""projectFileDependencyGroups"": {
                """": [
                  ""packageA.interop [1.0.0, )""
                ],
                "".NETCoreApp,Version=v2.1"": []
              }
            }";

            var lockFile = Parse(lockFileContent, "In Memory", environmentVariableReader);

            Assert.Equal(1, lockFile.Version);

            var target = lockFile.Targets.Single();

            var targetLibrary = target.Libraries.Single();
            Assert.Equal("packageA.interop", targetLibrary.Name);
            Assert.Equal(NuGetVersion.Parse("1.0.0"), targetLibrary.Version);
            Assert.Equal("lib/netstandard2.0/packageA.interop.dll", targetLibrary.CompileTimeAssemblies.Single().Path);
            Assert.Equal("lib/netstandard2.0/packageA.interop.dll", targetLibrary.RuntimeAssemblies.Single().Path);
            Assert.Equal("embed/netstandard2.0/packageA.interop.dll", targetLibrary.EmbedAssemblies.Single().Path);
        }

        [Fact]
        public void LockFileFormat_WritesFrameworkReference()
        {

            // Arrange
            var lockFileContent = @"{
  ""version"": 3,
  ""targets"": {
                "".NETCoreApp,Version=v3.0"": {
                    ""My.Nice.Package.With.WPF.Reference/2.0.0"": {
                        ""type"": ""package"",
        ""compile"": {
                            ""lib/netcoreapp3.0/a.dll"": { }
                        },
        ""frameworkReferences"": [
          ""Microsoft.Windows.Desktop|WPF"",
          ""Microsoft.Windows.Desktop|WindowsForms""
        ]
    }
}
  },
  ""libraries"": {
    ""My.Nice.Package.With.WPF.Reference/2.0.0"": {
      ""sha512"": ""sup3rs3cur3"",
      ""type"": ""package"",
      ""files"": [
        ""My.Nice.Package.With.WPF.Reference.nuspec"",
        ""lib/netcoreapp3.0/a.dll""
      ]
    }
  },
  ""projectFileDependencyGroups"": { }
}";
            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = FrameworkConstants.CommonFrameworks.NetCoreApp30
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "My.Nice.Package.With.WPF.Reference",
                Version = NuGetVersion.Parse("2.0.0"),
                Type = LibraryType.Package
            };

            targetLib.CompileTimeAssemblies.Add(new LockFileItem("lib/netcoreapp3.0/a.dll"));
            // the order is important, the test assures that they are sorted.
            targetLib.FrameworkReferences.Add("Microsoft.Windows.Desktop|WindowsForms");
            targetLib.FrameworkReferences.Add("Microsoft.Windows.Desktop|WPF");

            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "My.Nice.Package.With.WPF.Reference",
                Version = NuGetVersion.Parse("2.0.0"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            // the order is important, the test assures that they are sorted.
            lib.Files.Add("lib/netcoreapp3.0/a.dll");
            lib.Files.Add("My.Nice.Package.With.WPF.Reference.nuspec");
            lockFile.Libraries.Add(lib);

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile)).ToString();
            var expected = JObject.Parse(lockFileContent).ToString();

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void LockFileFormat_WritesCentralTransitiveDependencyGroups()
        {
            // Arrange
            NuGetFramework framework = FrameworkConstants.CommonFrameworks.DotNet;
            var lockFileContent = @"{
            ""version"": 3,
            ""targets"": {
                "".NETPlatform,Version=v5.0"": {
                ""System.Runtime/4.0.20-beta-22927"": {
                ""type"": ""package"",
                ""dependencies"": {
                    ""Frob"": ""4.0.20""
                    },
                ""compile"": {
                    ""ref/dotnet/System.Runtime.dll"": {}
                    }
                }
                }
            },
            ""libraries"": {
                ""System.Runtime/4.0.20-beta-22927"": {
                ""sha512"": ""sup3rs3cur3"",
                ""type"": ""package"",
                ""files"": [
                    ""System.Runtime.nuspec""
                    ]
                }
            },
            ""projectFileDependencyGroups"": {
                """": [
                ""System.Runtime [4.0.10-beta-*, )""
                ],
                "".NETPlatform,Version=v5.0"": []
            },
            ""centralTransitiveDependencyGroups"": {
                "".NETPlatform,Version=v5.0"": {
                ""Newtonsoft.Json"": {
                            ""include"": ""Compile, Native, BuildTransitive"",
                            ""suppressParent"": ""All"",
                            ""version"": ""[12.0.3, )""
                        }
                    }
                }
            }";

            var lockFile = new LockFile()
            {
                Version = 3
            };

            var target = new LockFileTarget()
            {
                TargetFramework = framework
            };

            var targetLib = new LockFileTargetLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package
            };

            targetLib.Dependencies.Add(new NuGet.Packaging.Core.PackageDependency("Frob", new VersionRange(NuGetVersion.Parse("4.0.20"))));
            targetLib.CompileTimeAssemblies.Add(new LockFileItem("ref/dotnet/System.Runtime.dll"));
            target.Libraries.Add(targetLib);
            lockFile.Targets.Add(target);

            var lib = new LockFileLibrary()
            {
                Name = "System.Runtime",
                Version = NuGetVersion.Parse("4.0.20-beta-22927"),
                Type = LibraryType.Package,
                Sha512 = "sup3rs3cur3"
            };
            lib.Files.Add("System.Runtime.nuspec");
            lockFile.Libraries.Add(lib);

            lockFile.ProjectFileDependencyGroups
                .Add(new ProjectFileDependencyGroup("", new string[] { "System.Runtime [4.0.10-beta-*, )" }));
            lockFile.ProjectFileDependencyGroups
                .Add(new ProjectFileDependencyGroup(framework.DotNetFrameworkName, Array.Empty<string>()));

            var newtonSoftDependency = new LibraryDependency(
                        libraryRange: new LibraryRange("Newtonsoft.Json", VersionRange.Parse("[12.0.3, )"), LibraryDependencyTarget.Package),
                        includeType: LibraryIncludeFlags.Compile | LibraryIncludeFlags.BuildTransitive | LibraryIncludeFlags.Native,
                        suppressParent: LibraryIncludeFlags.All,
                        noWarn: [],
                        autoReferenced: true,
                        generatePathProperty: false,
                        versionCentrallyManaged: true,
                        LibraryDependencyReferenceType.Direct,
                        aliases: null,
                        versionOverride: null);

            lockFile.CentralTransitiveDependencyGroups
                .Add(new CentralTransitiveDependencyGroup(framework, new List<LibraryDependency>() { newtonSoftDependency }));

            // Act
            var lockFileFormat = new LockFileFormat();
            var output = JObject.Parse(lockFileFormat.Render(lockFile));
            var expected = JObject.Parse(lockFileContent);

            // Assert
            Assert.Equal(expected.ToString(), output.ToString());
        }

        private LockFile Read(string filePath, IEnvironmentVariableReader environmentVariableReader)
        {
            var reader = new LockFileFormat();
            using (var stream = File.OpenRead(filePath))
            {
                return reader.Read(stream, NullLogger.Instance, filePath, environmentVariableReader, true);
            }
        }

        private LockFile Parse(string lockFileContent, string path, IEnvironmentVariableReader environmentVariableReader, ILogger logger = null, LockFileReadFlags flags = LockFileReadFlags.All)
        {
            var reader = new LockFileFormat();
            byte[] byteArray = Encoding.UTF8.GetBytes(lockFileContent);
            using (var stream = new MemoryStream(byteArray))
            {
                return reader.Read(stream, logger ?? NullLogger.Instance, path, environmentVariableReader, true, flags);
            }
        }
    }
}
