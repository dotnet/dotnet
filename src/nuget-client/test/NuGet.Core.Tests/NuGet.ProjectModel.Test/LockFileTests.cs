// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileTests
    {
        [Fact]
        public void LockFile_ConsidersEquivalentPackageSpec()
        {
            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 2,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }

        [Fact]
        public void LockFile_ConsidersEquivalentLockFilesWithEmptyLogsAsSame()
        {
            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            self.LogMessages = new List<IAssetsLogMessage>();

            other.LogMessages = new List<IAssetsLogMessage>();

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }

        [Fact]
        public void LockFile_ConsidersEquivalentLockFilesWithMinimalLogsAsSame()
        {
            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            self.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message"),
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    WarningLevel = WarningLevel.Severe
                },
            };

            other.LogMessages = new List<IAssetsLogMessage>
            {
            new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message"),
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    WarningLevel = WarningLevel.Severe
                },
            };

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }

        [Fact]
        public void LockFile_ConsidersEquivalentLockFilesWithFullLogsAsSame()
        {

            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            self.LogMessages = new List<IAssetsLogMessage>
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
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning",
                    WarningLevel = WarningLevel.Severe
                }
            };

            other.LogMessages = new List<IAssetsLogMessage>
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
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning",
                    WarningLevel = WarningLevel.Severe
                }
            };

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }


        [Fact]
        public void LockFile_ConsidersLockFilesWithDifferentlyOrderedLogsAsSame()
        {

            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            self.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    WarningLevel = WarningLevel.Severe
                },
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test error message 1"),
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test error message 2")
            };

            other.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test error message 2"),
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test error message 1"),
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    WarningLevel = WarningLevel.Severe
                }
            };

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }

        [Fact]
        public void LockFile_ConsidersLockFilesWithLogsWithDifferentMessagesAsDifferent()
        {
            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            self.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test error message"),
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    WarningLevel = WarningLevel.Severe
                },
            };

            other.LogMessages = new List<IAssetsLogMessage>
            {
            new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test different error message"),
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    WarningLevel = WarningLevel.Severe
                },
            };

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.NotEqual(self, other);
        }

        [Fact]
        public void LockFile_ConsidersLockFilesWithDifferentErrorsAsDifferent()
        {
            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();
            self.LogMessages = new List<IAssetsLogMessage>
            {
                new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "test log message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46" }
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning",
                    WarningLevel = WarningLevel.Severe
                }
            };

            other.LogMessages = new List<IAssetsLogMessage>
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
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning",
                    WarningLevel = WarningLevel.Severe
                }
            };

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.NotEqual(self, other);
        }

        [Fact]
        public void LockFile_ConsidersLockFilesWithDifferentLogsAsDifferent()
        {
            // Arrange
            Func<LockFile> getLockFile = () =>
            {
                var lockFile = new LockFile
                {
                    Version = 3,

                    PackageSpec = new PackageSpec(new[]
                {
                    new TargetFrameworkInformation
                    {
                        FrameworkName = FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        Dependencies = new[]
                        {
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
                        }
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
                            OriginalTargetFrameworks = new[] { "netcoreapp1.0" },
                            TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>
                            {
                                new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("netcoreapp1.0"))
                            }
                        }
                    }
                };
                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();
            self.LogMessages = new List<IAssetsLogMessage>
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
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    StartLineNumber = 11,
                    StartColumnNumber = 2,
                    EndLineNumber = 11,
                    EndColumnNumber = 10,
                    LibraryId = "nuget.versioning",
                    WarningLevel = WarningLevel.Severe
                }
            };

            other.LogMessages = new List<IAssetsLogMessage>
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
                },
                new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test warning message")
                {
                    FilePath = @"kung\fu\fighting.targets",
                    ProjectPath = @"kung\fu\fighting.csproj",
                    TargetGraphs = new List<string>{ "net46", "netcoreapp1.0", "netstandard1.6" },
                    LibraryId = "nuget.versioning",
                    WarningLevel = WarningLevel.Default
                }
            };

            // Act & Assert
            Assert.NotSame(self, other);
            Assert.NotEqual(self, other);
        }

        [Fact]
        public void LockFile_ConsiderCentralTransitiveDependencyGroupsForEquality()
        {
            // Arrange
            var dotNetFramework = FrameworkConstants.CommonFrameworks.NetCoreApp10;
            var libraryDependency_1 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                                        "Microsoft.NETCore.App",
                                        new VersionRange(
                                            minVersion: new NuGetVersion("1.0.1"),
                                            originalString: "1.0.1"),
                                        LibraryDependencyTarget.Package)
            };
            var libraryDependency_2 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                            "Microsoft.NETCore.App",
                            new VersionRange(
                                minVersion: new NuGetVersion("2.0.1"),
                                originalString: "2.0.1"),
                            LibraryDependencyTarget.Package)
            };
            var libraryDependency_3 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                            "Microsoft.NETCore.App",
                            new VersionRange(
                                minVersion: new NuGetVersion("3.0.1"),
                                originalString: "3.0.1"),
                            LibraryDependencyTarget.Package)
            };
            var libraryDependency_11 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                         "Microsoft.NETCore.App",
                         new VersionRange(
                             minVersion: new NuGetVersion("1.0.1"),
                             originalString: "1.0.1"),
                         LibraryDependencyTarget.Package)
            };
            var libraryDependency_22 = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                           "Microsoft.NETCore.App",
                           new VersionRange(
                               minVersion: new NuGetVersion("2.0.1"),
                               originalString: "2.0.1"),
                           LibraryDependencyTarget.Package)
            };
            var projCTDG_1_2 = new CentralTransitiveDependencyGroup(dotNetFramework, new List<LibraryDependency>() { libraryDependency_1, libraryDependency_2 });
            var projCTDG_11_22 = new CentralTransitiveDependencyGroup(dotNetFramework, new List<LibraryDependency>() { libraryDependency_1, libraryDependency_2 });

            var lockFile_1_2 = new LockFile
            {
                Version = 3,
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>() { projCTDG_1_2 }
            };
            var lockFile_11_22 = new LockFile
            {
                Version = 3,
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>() { projCTDG_11_22 }
            };
            var lockFile_1 = new LockFile
            {
                Version = 3,
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>()
                    {
                        new CentralTransitiveDependencyGroup(dotNetFramework, new List<LibraryDependency>(){ libraryDependency_1})
                    }
            };
            var lockFile_1_3 = new LockFile
            {
                Version = 3,
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>()
                    {
                        new CentralTransitiveDependencyGroup(dotNetFramework, new List<LibraryDependency>(){ libraryDependency_1, libraryDependency_3})
                    }
            };

            // Act & Assert
            Assert.Equal(lockFile_1_2, lockFile_1_2);
            Assert.Equal(lockFile_1_2, lockFile_11_22);
            Assert.NotEqual(lockFile_1_2, lockFile_1);
            Assert.NotEqual(lockFile_1_2, lockFile_1_3);
            Assert.Equal(lockFile_1_2.GetHashCode(), lockFile_11_22.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFile_GetTarget_WithNuGetFramework_ReturnsCorrectLockFileTarget(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var expectedJson = ResourceTestUtility.GetResource("NuGet.ProjectModel.Test.compiler.resources.sample.assets.json", typeof(LockFileTests));
            var lockFile = Parse(expectedJson, Path.GetTempPath(), environmentVariableReader);
            NuGetFramework nuGetFramework = NuGetFramework.ParseComponents(".NETCoreApp,Version=v5.0", "Windows,Version=7.0");

            // Act
            var target = lockFile.GetTarget(nuGetFramework, runtimeIdentifier: null);

            // Assert
            target.TargetFramework.Should().Be(nuGetFramework);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void LockFile_GetTarget_WithAlias_ReturnsCorrectLockFileTarget(IEnvironmentVariableReader environmentVariableReader)
        {
            // Arrange
            var expectedJson = ResourceTestUtility.GetResource("NuGet.ProjectModel.Test.compiler.resources.sample.assets.json", typeof(LockFileTests));
            var lockFile = Parse(expectedJson, Path.GetTempPath(), environmentVariableReader);
            NuGetFramework nuGetFramework = NuGetFramework.ParseComponents(".NETCoreApp,Version=v5.0", "Windows,Version=7.0");

            // Act
            var target = lockFile.GetTarget("net5.0", runtimeIdentifier: null);

            // Assert
            target.TargetFramework.Should().Be(nuGetFramework);
        }

        private LockFile Parse(string lockFileContent, string path, IEnvironmentVariableReader environmentVariableReader)
        {
            var reader = new LockFileFormat();
            byte[] byteArray = Encoding.UTF8.GetBytes(lockFileContent);
            using (var stream = new MemoryStream(byteArray))
            {
                return reader.Read(stream, NullLogger.Instance, path, environmentVariableReader, true);
            }
        }
    }
}
