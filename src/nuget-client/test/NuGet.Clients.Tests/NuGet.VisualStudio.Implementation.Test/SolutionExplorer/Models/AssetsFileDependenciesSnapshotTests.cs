// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio.SolutionExplorer.Models;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.SolutionExplorer.Models
{
    public class AssetsFileDependenciesSnapshotTests
    {
        [Fact]
        public void ParseLibraries_IgnoreCaseInDependenciesTree_Succeeds()
        {
            var lockFileContent = """
                {
                  "version": 3,
                  "targets": {
                    "net5.0": {
                      "System.Runtime/4.0.20-beta-22927": {
                        "type": "package",
                        "dependencies": {
                          "Frob": "4.0.20"
                        },
                        "compile": {
                          "ref/dotnet/System.Runtime.dll": {}
                        }
                      }
                    }
                  },
                  "libraries": {
                    "System.Runtime/4.0.20-beta-22927": {
                      "sha512": "sup3rs3cur3",
                      "type": "package",
                      "files": [
                        "System.Runtime.nuspec"
                      ]
                    }
                  },
                  "projectFileDependencyGroups": {
                    "": [
                      "System.Runtime [4.0.10-beta-*, )"
                    ],
                    "net5.0": []
                  },
                  "logs": [
                    {
                      "code": "NU1000",
                      "level": "Error",
                      "message": "test log message"
                    }
                  ]
                }
                """;

            var lockFilePath = """C:\repo\obj\project.assets.json""";

            var lockFile = new LockFileFormat().Parse(lockFileContent, lockFilePath);

            var dependencies = AssetsFileDependenciesSnapshot.ParseLibraries(lockFile, lockFile.Targets.First(), ImmutableArray<AssetsFileLogMessage>.Empty);

            var dependency = Assert.Single(dependencies);
            Assert.Equal("System.Runtime", dependency.Key);
        }

        [Fact]
        public void ParseLibraries_LogForUnknownLibrary_AddsUnknownLibraryType()
        {
            var lockFileContent = """
                {
                  "version": 3,
                  "targets": {
                    "net5.0": {
                    }
                  },
                  "libraries": {
                  },
                  "projectFileDependencyGroups": {
                    "": [
                      "System.Runtime [4.0.10-beta-*, )"
                    ],
                    "net5.0": []
                  },
                  "logs": [
                    {
                      "code": "NU1000",
                      "level": "Error",
                      "message": "test log message",
                      "libraryId": "UnknownLibraryId"
                    }
                  ]
                }
                """;

            var lockFilePath = """C:\repo\obj\project.assets.json""";

            var lockFile = new LockFileFormat().Parse(lockFileContent, lockFilePath);

            var logMessages = ImmutableArray.Create(new AssetsFileLogMessage(lockFilePath, lockFile.LogMessages.Single()));

            var dependencies = AssetsFileDependenciesSnapshot.ParseLibraries(lockFile, lockFile.Targets.First(), logMessages);

            var dependency = Assert.Single(dependencies);
            Assert.Equal("UnknownLibraryId", dependency.Key);
            Assert.Equal("UnknownLibraryId", dependency.Value.Name);
            Assert.Null(dependency.Value.Version);
            Assert.Empty(dependency.Value.BuildFiles);
            Assert.Empty(dependency.Value.BuildMultiTargetingFiles);
            Assert.Empty(dependency.Value.CompileTimeAssemblies);
            Assert.Empty(dependency.Value.Dependencies);
            Assert.Empty(dependency.Value.DocumentationFiles);
            Assert.Empty(dependency.Value.FrameworkAssemblies);
            Assert.Equal(AssetsFileLibraryType.Unknown, dependency.Value.Type);
        }

        [Fact]
        public void ParseLibraries_LogForUnknownLibrary_WithAbsolutePath_AddsUnknownLibraryType()
        {
            var lockFileContent = """
                {
                  "version": 3,
                  "targets": {
                    "net5.0": {
                    }
                  },
                  "libraries": {
                  },
                  "projectFileDependencyGroups": {
                    "": [
                      "System.Runtime [4.0.10-beta-*, )"
                    ],
                    "net5.0": []
                  },
                  "logs": [
                    {
                      "code": "NU1000",
                      "level": "Error",
                      "message": "test log message",
                      "libraryId": "C:\\repo\\OtherProject\\OtherProject.csproj"
                    }
                  ]
                }
                """;

            var lockFilePath = """C:\repo\obj\project.assets.json""";

            var lockFile = new LockFileFormat().Parse(lockFileContent, lockFilePath);

            var logMessages = ImmutableArray.Create(new AssetsFileLogMessage(lockFilePath, lockFile.LogMessages.Single()));

            var dependencies = AssetsFileDependenciesSnapshot.ParseLibraries(lockFile, lockFile.Targets.First(), logMessages);

            var dependency = Assert.Single(dependencies);
            Assert.Equal("""..\OtherProject\OtherProject.csproj""", dependency.Key);
            Assert.Equal("""..\OtherProject\OtherProject.csproj""", dependency.Value.Name);
            Assert.Null(dependency.Value.Version);
            Assert.Empty(dependency.Value.BuildFiles);
            Assert.Empty(dependency.Value.BuildMultiTargetingFiles);
            Assert.Empty(dependency.Value.CompileTimeAssemblies);
            Assert.Empty(dependency.Value.Dependencies);
            Assert.Empty(dependency.Value.DocumentationFiles);
            Assert.Empty(dependency.Value.FrameworkAssemblies);
            Assert.Equal(AssetsFileLibraryType.Unknown, dependency.Value.Type);
        }

        [Fact]
        public void ParseLibraries_LockFileTargetLibrariesWithDifferentCase_Throws()
        {
            var lockFileTarget = new LockFileTarget
            {
                Libraries = new LockFileTargetLibrary[]
                {
                    new()
                    {
                        Name = "packageA",
                        Type = "package",
                        Version = NuGetVersion.Parse("1.0.0")
                    },
                    new()
                    {
                        Name = "PackageA",
                        Type = "package",
                        Version = NuGetVersion.Parse("1.0.0")
                    }
                }
            };

            var exception = Assert.Throws<ArgumentException>(() => AssetsFileDependenciesSnapshot.ParseLibraries(new LockFile(), lockFileTarget, ImmutableArray<AssetsFileLogMessage>.Empty));

            Assert.Contains("PackageA", exception.Message);
        }

        [Fact]
        public void ParseLibraries_LockFileTargetLibrariesMatchesDependencies_Succeeds()
        {
            var lockFileTarget = new LockFileTarget
            {
                Libraries = new LockFileTargetLibrary[]
                {
                    new()
                    {
                        Name = "packageA",
                        Type = "package",
                        Version = NuGetVersion.Parse("1.0.0")
                    },
                    new()
                    {
                        Name = "packageB",
                        Type = "package",
                        Version = NuGetVersion.Parse("1.0.0")
                    },
                    new()
                    {
                        Name = "projectA",
                        Type = "project",
                        Version = NuGetVersion.Parse("1.0.0")
                    },
                    new()
                    {
                        Name = "projectB",
                        Type = "project",
                        Version = NuGetVersion.Parse("1.0.0")
                    }
                }
            };

            ImmutableDictionary<string, AssetsFileTargetLibrary> dependencies = AssetsFileDependenciesSnapshot.ParseLibraries(new LockFile(), lockFileTarget, ImmutableArray<AssetsFileLogMessage>.Empty);

            Assert.Equal(lockFileTarget.Libraries.Count, dependencies.Count);
            Assert.All(lockFileTarget.Libraries,
                source =>
                {
                    Assert.True(dependencies.ContainsKey(source.Name));

                    AssetsFileTargetLibrary target = dependencies[source.Name];
                    Assert.Equal(source.Name, target.Name);
                    Assert.Equal(source.Version.ToNormalizedString(), target.Version);

                    Assert.True(Enum.TryParse(source.Type, ignoreCase: true, out AssetsFileLibraryType sourceType));
                    Assert.Equal(sourceType, target.Type);
                });
        }
    }
}
