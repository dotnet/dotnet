// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class MsBuildUtilityTest
    {
        // Test that when msbuildVersion is null, GetMsBuildDirectoryInternal returns the highest installed version.
        [Theory]
        [MemberData("HighestPathData", MemberType = typeof(ToolsetDataSource))]
        public void HighestVersionSelectedIfMsBuildVersionIsNull(List<MsBuildToolset> toolsets, string expectedPath)
        {
            // Arrange
            // Act
            var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                userVersion: null,
                console: null,
                installedToolsets: toolsets.OrderByDescending(t => t),
                getMsBuildPathInPathVar: (reader) => null).Path;

            // Assert
            Assert.Equal(expectedPath, directory);
        }

        // Test that GetMsBuildDirectoryInternal returns the toolset that matches the msbuild version
        [Theory]
        [MemberData("PathMatchData", MemberType = typeof(ToolsetDataSource))]
        public void VersionSelectedThatMatchesPathMsBuildVersion(List<MsBuildToolset> toolsets, string expectedPath)
        {
            // Arrange
            // Act
            var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                userVersion: null,
                console: null,
                installedToolsets: toolsets.OrderByDescending(t => t),
                getMsBuildPathInPathVar: (reader) => expectedPath).Path;

            // Assert
            Assert.Equal(expectedPath, directory);
        }

        // Test that GetMsBuildDirectoryInternal deals with invalid toolsets (for example ones created from SKUs that don't ship MSBuild like VS Test Agent SKU) See https://github.com/NuGet/Home/issues/5840 for more info
        [Theory]
        [MemberData("InvalidToolsetData", MemberType = typeof(ToolsetDataSource))]
        public void HandlesToolsetsWithInvalidPaths(List<MsBuildToolset> toolsets, string expectedPath)
        {
            // Arrange
            // Act
            var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                userVersion: null,
                console: null,
                installedToolsets: toolsets.OrderByDescending(t => t),
                getMsBuildPathInPathVar: (reader) => expectedPath).Path;

            // Assert
            Assert.Equal(expectedPath, directory);
        }

        // Test that GetMsBuildDirectoryInternal returns the highest version toolset if no matches
        [Theory]
        [MemberData("HighestPathData", MemberType = typeof(ToolsetDataSource))]
        public void HighestVersionSelectedIfExeInPathDoesntMatchToolsets(List<MsBuildToolset> toolsets, string expectedPath)
        {
            // Arrange
            // Act
            var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                userVersion: null,
                console: null,
                installedToolsets: toolsets.OrderByDescending(t => t),
                getMsBuildPathInPathVar: (reader) => @"c:\foo").Path;

            // Assert
            Assert.Equal(expectedPath, directory);
        }

        // Test the GetMsBuildDirectoryInternal returns the highest version, ignoring the path, if "latest" is specified.
        [Theory]
        [MemberData("HighestPathWithLowVersionMatchData", MemberType = typeof(ToolsetDataSource))]
        public void HighestVersionSelectedIfLatestSpecified(List<MsBuildToolset> toolsets, string lowVersionPath, string expectedPath)
        {
            // Arrange
            // Act
            var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                userVersion: "latest",
                console: null,
                installedToolsets: toolsets.OrderByDescending(t => t),
                getMsBuildPathInPathVar: (reader) => lowVersionPath).Path;

            // Assert
            Assert.Equal(expectedPath, directory);
        }

        // Tests that GetMsBuildDirectoryInternal returns path of the toolset whose toolset version matches
        // the userVersion. Also tests that, when userVersion is just a number, it can be matched with version 
        // userVersion + ".0". And non-numeric/case insensitive tests.
        [Theory]
        [MemberData("VersionMatchData", MemberType = typeof(ToolsetDataSource))]
        [MemberData("IntegerVersionMatchData", MemberType = typeof(ToolsetDataSource))]
        [MemberData("NonNumericVersionMatchData", MemberType = typeof(ToolsetDataSource))]
        public void TestVersionMatch(List<MsBuildToolset> toolsets, string userVersion, string expectedPath)
        {
            // Arrange
            // Act
            var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                userVersion: userVersion,
                console: null,
                installedToolsets: toolsets.OrderByDescending(t => t),
                getMsBuildPathInPathVar: (reader) => null).Path;

            // Assert
            Assert.Equal(expectedPath, directory);
        }


        // Tests that, when userVersion cannot be parsed into Version, string comparison is used (and in these cases fails)
        [Theory]
        [MemberData("NonNumericVersionMatchFailureData", MemberType = typeof(ToolsetDataSource))]
        public void TestVersionMatchByStringFailure(List<MsBuildToolset> toolsets, string userVersion)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<CommandException>(() =>
                {
                    var directory = MsBuildUtility.GetMsBuildDirectoryInternal(
                        userVersion: userVersion,
                        console: null,
                        installedToolsets: toolsets.OrderByDescending(t => t),
                        getMsBuildPathInPathVar: (reader) => null);
                });

            // Assert
            Assert.Equal(
                $"Cannot find the specified version of msbuild: '{userVersion}'",
                ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("15")]
        [InlineData("15.0")]
        [InlineData("14")]
        public void TestGetMsbuildDirectoryForMonoOnMac(string version)
        {
            var os = Environment.GetEnvironmentVariable("OSTYPE");
            if (RuntimeEnvironmentHelper.IsMono && os != null && os.StartsWith("darwin"))
            {
                // Act;
                var toolset = MsBuildUtility.GetMsBuildFromMonoPaths(version);

                var msbuild14 = @"/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/msbuild/14.1/bin/";
                var msbuild15 = @"/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/msbuild/15.0/bin/";

                // Assert
                if (version == "15.0" || version == "15")
                {
                    Assert.Equal(toolset.Path, msbuild15);
                }
                else if (version == "14.1")
                {
                    Assert.Equal(toolset.Path, msbuild14);
                }
                else if (version == null)
                {
                    Assert.True(new List<string> { msbuild14, msbuild15 }.Contains(toolset.Path));
                }
            }
        }

        [Fact]
        public void TestMsBuildPathFromVsPath()
        {
            using (var vsPath = TestDirectory.Create())
            {
                // Arrange
                // Create this tree:
                // VS
                // |- MSBuild
                //    |- 15.0
                //    |  |- bin
                //    |     |- msbuild.exe
                //    |- 15.1
                //       |- bin
                //          |- msbuild.exe
                // We want the highest version within the VS tree chosen (typically there's only one, but that's the logic 
                // we'll go with in case there are more).
                var msBuild15BinPath = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "15.0", "Bin")).FullName;
                var msBuild151BinPath = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "15.1", "Bin")).FullName;

                // Create dummy msbuild.exe files
                var msBuild15ExePath = Path.Combine(msBuild15BinPath, "msbuild.exe").ToString();
                using (var fs15 = File.CreateText(msBuild15ExePath))
                {
                    fs15.Write("foo 15");
                }

                var msBuild151ExePath = Path.Combine(msBuild151BinPath, "msbuild.exe").ToString();
                using (var fs151 = File.CreateText(msBuild151ExePath))
                {
                    fs151.Write("foo 15.1");
                }

                // Act
                var msBuildExePath = MsBuildToolset.GetMsBuildDirFromVsDir(vsPath);

                // Assert
                Assert.Equal(msBuildExePath, msBuild151BinPath, ignoreCase: true);
            }
        }

        [Fact]
        public void TestMsBuildPathFromVsPathWithNonEnglishCulture()
        {
            CultureInfo startingCulture = Thread.CurrentThread.CurrentCulture;

            // Change culture to Ukrainian (any culture with comma and period swapped compared to english in floating point)
            Thread.CurrentThread.CurrentCulture
                = CultureInfo.GetCultureInfo("uk-UA");

            using (var vsPath = TestDirectory.Create())
            {
                // Arrange
                // Create this tree:
                // VS
                // |- MSBuild
                //    |- 15.0
                //    |  |- bin
                //    |     |- msbuild.exe
                //    |- 15.1
                //       |- bin
                //          |- msbuild.exe
                // We want the highest version within the VS tree chosen (typically there's only one, but that's the logic 
                // we'll go with in case there are more).
                var msBuild15BinPath = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "15.0", "Bin")).FullName;
                var msBuild151BinPath = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "15.1", "Bin")).FullName;

                // Create dummy msbuild.exe files
                var msBuild15ExePath = Path.Combine(msBuild15BinPath, "msbuild.exe").ToString();
                File.WriteAllText(msBuild15ExePath, "foo 15");

                var msBuild151ExePath = Path.Combine(msBuild151BinPath, "msbuild.exe").ToString();
                File.WriteAllText(msBuild151ExePath, "foo 15.1");

                // Act
                var msBuildExePath = MsBuildToolset.GetMsBuildDirFromVsDir(vsPath);

                // Assert
                Assert.Equal(msBuildExePath, msBuild151BinPath, ignoreCase: true);
            }

            // reset culture
            Thread.CurrentThread.CurrentCulture = startingCulture;
        }

        [Fact]
        public void TestGetMsbuildDirectoryFromPATHENV()
        {
            if (RuntimeEnvironmentHelper.IsMono)
            { // Mono does not have SxS installations so it's not relevant to get msbuild from the path.
                return;
            }

            using (var vsPath = TestDirectory.Create())
            {
                var msBuild159BinPath = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "15.9", "Bin")).FullName;

                var msBuild159ExePath = Path.Combine(msBuild159BinPath, "msbuild.exe").ToString();

                using (var fs15 = File.CreateText(msBuild159ExePath))
                {
                    fs15.Write("foo 15.9");
                }

                var pathValue = Environment.GetEnvironmentVariable("PATH");
                var newPathValue = msBuild159BinPath + ";" + pathValue;

                Environment.SetEnvironmentVariable("PATH", newPathValue);

                // Act;
                var toolset = MsBuildUtility.GetMsBuildToolset(userVersion: null, console: null);
                Environment.SetEnvironmentVariable("PATH", pathValue);

                // Assert
                Assert.NotNull(toolset);
                Assert.Equal(msBuild159BinPath, toolset.Path);
            }
        }

        [Fact]
        public void GetMsbuildDirectoryFromPath_PATHENVWithQuotes_Succeeds()
        {
            if (RuntimeEnvironmentHelper.IsMono)
            { // Mono does not have SxS installations so it's not relevant to get msbuild from the path.
                return;
            }

            using (var vsPath = TestDirectory.Create())
            {
                var msBuild160BinDir = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "16.0", "Bin"));
                var msBuild160BinPath = msBuild160BinDir.FullName;
                var msBuild160ExePath = Path.Combine(msBuild160BinPath, "msbuild.exe").ToString();

                File.WriteAllText(msBuild160ExePath, "foo 16.0");

                var newPathValue = new StringBuilder();
                newPathValue.Append('\"');
                newPathValue.Append(msBuild160BinPath);
                newPathValue.Append('\"');
                newPathValue.Append(';');

                var environment = new Mock<NuGet.Common.IEnvironmentVariableReader>(MockBehavior.Strict);
                environment.Setup(s => s.GetEnvironmentVariable("PATH")).Returns(newPathValue.ToString());

                // Act;
                var msBuildPath = MsBuildUtility.GetMSBuild(environment.Object);

                // Assert
                Assert.NotNull(msBuildPath);
                Assert.Equal(msBuild160ExePath, msBuildPath);
            }
        }

        [SkipMonoTheory] // Mono does not have SxS installations so it's not relevant to get msbuild from the path.
        [InlineData("arm64", true)]
        [InlineData("amd64", true)]
        [InlineData("ARM64", true)]
        [InlineData("random", false)]
        public void GetNonArchitectureDirectory_PATHENVWithArchitecture_Succeeds(string architecutre, bool isArchitectureSpecificPath)
        {
            using (var vsPath = TestDirectory.Create())
            {
                var msBuildNonArchitectureDir = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "Current", "Bin"));
                var msBuildExeNonArchitecturePath = Path.Combine(msBuildNonArchitectureDir.FullName, "msbuild.exe");
                File.WriteAllText(msBuildExeNonArchitecturePath, "foo");

                var msBuildArchitectureDir = Directory.CreateDirectory(Path.Combine(msBuildNonArchitectureDir.FullName, architecutre));
                var msBuildExeArchitecturePath = Path.Combine(msBuildArchitectureDir.FullName, "msbuild.exe");
                File.WriteAllText(msBuildExeArchitecturePath, "foo");

                // Act;
                var msBuildPath = MsBuildUtility.GetNonArchitectureDirectory(msBuildExeArchitecturePath);

                // Assert
                if (isArchitectureSpecificPath)
                {
                    Assert.Equal(msBuildNonArchitectureDir.FullName, msBuildPath);
                }
                else
                {
                    Assert.Equal(msBuildArchitectureDir.FullName, msBuildPath);
                }
            }
        }

        [SkipMono] // Mono does not have SxS installations so it's not relevant to get msbuild from the path.
        public void GetNonArchitectureDirectory_PATHENVWithArchitecture_Throws()
        {
            using (var vsPath = TestDirectory.Create())
            {
                var msBuildNonArchitectureDir = Directory.CreateDirectory(Path.Combine(vsPath, "MSBuild", "Current", "Bin"));
                var msBuildArchitectureDir = Directory.CreateDirectory(Path.Combine(msBuildNonArchitectureDir.FullName, "arm64"));
                var msBuildExeArchitecturePath = Path.Combine(msBuildArchitectureDir.FullName, "msbuild.exe");
                File.WriteAllText(msBuildExeArchitecturePath, "foo");

                // Act & Assert
                CommandException exception = Assert.Throws<CommandException>(
                    () => MsBuildUtility.GetNonArchitectureDirectory(msBuildExeArchitecturePath));

                Assert.Equal(string.Format(CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString(nameof(NuGetResources.Error_CannotFindNonArchitectureSpecificMsbuild)),
                    msBuildArchitectureDir.FullName),
                    exception.Message);
            }
        }

        [Fact]
        public void CombinePathWithVerboseError_CombinesPaths()
        {
            var paths = new[] { "C:\\", "directory/", "\\folder", "file.txt" };
            Assert.Equal(Path.Combine(paths), MsBuildUtility.CombinePathWithVerboseError(paths));
        }

        [PlatformFact(Platform.Windows)]
        public void CombinePathWithVerboseError_IllegalCharacters_MessageContainsBadPath()
        {
            const string badPath = @"C:\bad:>dir";
            var exception = Assert.Throws<ArgumentException>(() => MsBuildUtility.CombinePathWithVerboseError(badPath, "file.txt"));
            Assert.Contains(badPath, exception.Message);
        }

        public static class ToolsetDataSource
        {
            // Legacy toolsets
            private static readonly MsBuildToolset Toolset14 = new MsBuildToolset(version: "14.0", path: "v14path");
            private static readonly MsBuildToolset Toolset12 = new MsBuildToolset(version: "12.0", path: "v12path");
            private static readonly MsBuildToolset Toolset4 = new MsBuildToolset(version: "4.0", path: "v4path");
            private static readonly MsBuildToolset Toolset4_NonNumericVersion = new MsBuildToolset(version: "Foo4.0", path: "foo4path");

            // SxS toolsets
            // Make install orders different from VS version numbers in path, to emphasise the test is install date based.
            private static readonly MsBuildToolset Toolset15_Mon_ShortVersion = new MsBuildToolset(
                version: "15.1",
                path: @"c:\vs\25555.00",
                installDate: new DateTime(2016, 9, 15));
            private static readonly MsBuildToolset Toolset15_Tue_ShortVersion = new MsBuildToolset(
                version: "15.1",
                path: @"c:\vs\25555.02",
                installDate: new DateTime(2016, 9, 16));
            private static readonly MsBuildToolset Toolset15_Wed_ShortVersion = new MsBuildToolset(
                version: "15.1",
                path: @"c:\vs\25555.01",
                installDate: new DateTime(2016, 9, 17));

            private static readonly MsBuildToolset Toolset15_Mon_LongVersion = new MsBuildToolset(
                version: "15.1.137.25382",
                path: @"c:\vs\25557.00",
                installDate: new DateTime(2016, 9, 15));
            private static readonly MsBuildToolset Toolset15_Tue_LongVersion = new MsBuildToolset(
                version: "15.1.137.25382",
                path: @"c:\vs\25557.02",
                installDate: new DateTime(2016, 9, 16));
            private static readonly MsBuildToolset Toolset15_Wed_LongVersion = new MsBuildToolset(
                version: "15.1.137.25382",
                path: @"c:\vs\25557.01",
                installDate: new DateTime(2016, 9, 17));
            private static readonly MsBuildToolset InvalidToolsetVSTest = new MsBuildToolset(
                version: null,
                path: null,
                installDate: new DateTime(2017, 9, 7));

            // Toolset collections

            private static List<MsBuildToolset> LegacyToolsets = new List<MsBuildToolset> {
                Toolset14,
                Toolset12,
                Toolset4
            };

            private static List<MsBuildToolset> LegacyToolsets_NonNumericVersion = new List<MsBuildToolset> {
                Toolset14,
                Toolset12,
                Toolset4_NonNumericVersion
            };

            private static List<MsBuildToolset> SxsToolsets_AscDate_MixedVersion = new List<MsBuildToolset> {
                Toolset15_Mon_ShortVersion,
                Toolset15_Tue_LongVersion,
                Toolset15_Wed_ShortVersion
            };

            private static List<MsBuildToolset> SxsToolsets_DescDate_MixedVersion = new List<MsBuildToolset> {
                Toolset15_Wed_ShortVersion,
                Toolset15_Tue_LongVersion,
                Toolset15_Mon_ShortVersion
            };

            private static List<MsBuildToolset> CombinedToolsets_AscDate_ShortVersion = new List<MsBuildToolset> {
                Toolset15_Mon_ShortVersion,
                Toolset15_Tue_ShortVersion,
                Toolset15_Wed_ShortVersion,
                Toolset14,
                Toolset12,
                Toolset4
            };

            private static List<MsBuildToolset> CombinedToolsets_DescDate_ShortVersion = new List<MsBuildToolset> {
                Toolset15_Wed_ShortVersion,
                Toolset15_Tue_ShortVersion,
                Toolset15_Mon_ShortVersion,
                Toolset14,
                Toolset12,
                Toolset4
            };

            private static List<MsBuildToolset> CombinedToolsets_AscDate_LongVersion = new List<MsBuildToolset> {
                Toolset15_Mon_LongVersion,
                Toolset15_Tue_LongVersion,
                Toolset15_Wed_LongVersion,
                Toolset14,
                Toolset12,
                Toolset4
            };

            private static List<MsBuildToolset> CombinedToolsets_DescDate_LongVersion = new List<MsBuildToolset> {
                Toolset15_Wed_LongVersion,
                Toolset15_Tue_LongVersion,
                Toolset15_Mon_LongVersion,
                Toolset14,
                Toolset12,
                Toolset4
            };

            private static List<MsBuildToolset> CombinedToolsets_MsBuild15AndVSTestToolsets = new List<MsBuildToolset> {
                Toolset15_Wed_LongVersion,
                InvalidToolsetVSTest
            };

            // Test data sets

            private static List<MsBuildToolset> CombinedToolsetsMixedV15Versions = new List<MsBuildToolset> {
                 Toolset15_Mon_ShortVersion, Toolset15_Mon_LongVersion, Toolset14, Toolset12, Toolset4
            };

            private static readonly List<object[]> _highestPathData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets, Toolset14.Path },
                    new object[] { CombinedToolsets_AscDate_ShortVersion, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_DescDate_ShortVersion, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_AscDate_LongVersion, Toolset15_Wed_LongVersion.Path },
                    new object[] { CombinedToolsets_DescDate_LongVersion, Toolset15_Wed_LongVersion.Path }
                };

            private static readonly List<object[]> _pathMatchData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets, Toolset12.Path },
                    new object[] { CombinedToolsets_AscDate_ShortVersion, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_DescDate_ShortVersion, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_AscDate_LongVersion, Toolset15_Wed_LongVersion.Path },
                    new object[] { CombinedToolsets_DescDate_LongVersion, Toolset15_Wed_LongVersion.Path },
                    new object[] { CombinedToolsets_DescDate_LongVersion, Toolset12.Path }
                };

            private static readonly List<object[]> _versionMatchData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets, Toolset12.Version, Toolset12.Path },
                    new object[] { SxsToolsets_AscDate_MixedVersion, Toolset15_Tue_LongVersion.Version, Toolset15_Tue_LongVersion.Path },
                    new object[] { SxsToolsets_AscDate_MixedVersion, Toolset15_Mon_ShortVersion.Version, Toolset15_Wed_ShortVersion.Path }, // Direct string match 15.1
                    new object[] { SxsToolsets_DescDate_MixedVersion, Toolset15_Tue_LongVersion.Version, Toolset15_Tue_LongVersion.Path },
                    new object[] { SxsToolsets_DescDate_MixedVersion, Toolset15_Mon_ShortVersion.Version, Toolset15_Wed_ShortVersion.Path }, // Direct string match 15.1
                    new object[] { CombinedToolsets_AscDate_ShortVersion, Toolset15_Wed_ShortVersion.Version, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_DescDate_ShortVersion, Toolset15_Wed_ShortVersion.Version, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_AscDate_LongVersion, Toolset15_Wed_LongVersion.Version, Toolset15_Wed_LongVersion.Path },
                    new object[] { CombinedToolsets_DescDate_LongVersion, Toolset15_Wed_LongVersion.Version, Toolset15_Wed_LongVersion.Path },
                    new object[] { CombinedToolsets_DescDate_LongVersion, Toolset12.Version, Toolset12.Path }
                };

            private static readonly List<object[]> _integerVersionMatchData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets, "12", Toolset12.Path }
                };

            private static readonly List<object[]> _nonNumericVersionMatchData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets_NonNumericVersion, Toolset4_NonNumericVersion.Version, Toolset4_NonNumericVersion.Path },
                    new object[] { LegacyToolsets_NonNumericVersion, Toolset4_NonNumericVersion.Version.ToLower(), Toolset4_NonNumericVersion.Path }
                };

            private static readonly List<object[]> _nonNumericVersionMatchFailureData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets_NonNumericVersion, "Foo4" },
                    new object[] { LegacyToolsets_NonNumericVersion, " Foo4.0" },
                    new object[] { LegacyToolsets_NonNumericVersion, "Foo4.0 " },
                    new object[] { LegacyToolsets_NonNumericVersion, " Foo4.0 " },
                    new object[] { LegacyToolsets_NonNumericVersion, "0" },
                };

            private static readonly List<object[]> _invalidToolsetData
                = new List<object[]>
                {
                    new object[] { CombinedToolsets_MsBuild15AndVSTestToolsets, Toolset15_Wed_LongVersion.Path}
                };

            private static readonly List<object[]> _highestPathWithLowVersionMatchData
                = new List<object[]>
                {
                    new object[] { LegacyToolsets, Toolset12.Path, Toolset14.Path },
                    new object[] { CombinedToolsets_AscDate_ShortVersion, Toolset15_Mon_ShortVersion.Path, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_DescDate_ShortVersion, Toolset15_Mon_ShortVersion.Path, Toolset15_Wed_ShortVersion.Path },
                    new object[] { CombinedToolsets_AscDate_LongVersion, Toolset15_Mon_LongVersion.Path, Toolset15_Wed_LongVersion.Path },
                    new object[] { CombinedToolsets_DescDate_LongVersion, Toolset15_Mon_LongVersion.Path,Toolset15_Wed_LongVersion.Path }
                };

            public static IEnumerable<object[]> HighestPathData => _highestPathData;
            public static IEnumerable<object[]> PathMatchData => _pathMatchData;
            public static IEnumerable<object[]> VersionMatchData => _versionMatchData;
            public static IEnumerable<object[]> IntegerVersionMatchData => _integerVersionMatchData;
            public static IEnumerable<object[]> NonNumericVersionMatchData => _nonNumericVersionMatchData;
            public static IEnumerable<object[]> NonNumericVersionMatchFailureData => _nonNumericVersionMatchFailureData;
            public static IEnumerable<object[]> InvalidToolsetData => _invalidToolsetData;
            public static IEnumerable<object[]> HighestPathWithLowVersionMatchData => _highestPathWithLowVersionMatchData;

        }
    }
}
