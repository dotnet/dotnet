// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class ValidateHarvestVersionIsLatestForReleaseTests
    {
        private Log _log;
        private TestBuildEngine _engine;
        private ITaskItem[] _testPackageReportPaths = new [] { new TaskItem("dummyReport.json") };
        private PackageReport _testPackageReport = new PackageReport()
        {
            Id = "TestPackage",
            Version = "4.7.0",
            Targets = new Dictionary<string, Target>
            {
                {
                    "testTarget", new Target
                    {
                        CompileAssets = new PackageAsset[]
                        {
                            new PackageAsset{ HarvestedFrom = "TestPackage/4.6.2/ref/netstandard2.0/TestPackage.dll" }
                        }
                    }
                }
            }
        };

        public ValidateHarvestVersionIsLatestForReleaseTests(ITestOutputHelper output)
        {
            _log = new Log(output);
            _engine = new TestBuildEngine(_log);
        }

        [Fact]
        public void ValidationFailsWhenHarvestVersionIsNotLatestTest()
        {
            TestableValidateHarvestVersionTask task = new TestableValidateHarvestVersionTask()
            {
                BuildEngine = _engine,
                PackageReports = _testPackageReportPaths,
                PackageReportFunc = (path) => _testPackageReport,
                GetLatestStableVersionFunc = (packageId, major, minor) => $"{major}.{minor}.3"
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(1);
            _log.WarningsLogged.Should().Be(0);
        }

        [Fact]
        public void ValidationSucceedsWhenHarvestVersionIsLatestTest()
        {
            TestableValidateHarvestVersionTask task = new TestableValidateHarvestVersionTask()
            {
                BuildEngine = _engine,
                PackageReports = _testPackageReportPaths,
                PackageReportFunc = (path) => _testPackageReport,
                GetLatestStableVersionFunc = (packageId, major, minor) => $"{major}.{minor}.2"
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
        }

        [Fact]
        public void ValidationSucceedsWhenNoPackagesWhereHarvestedTest()
        {
            TestableValidateHarvestVersionTask task = new TestableValidateHarvestVersionTask()
            {
                BuildEngine = _engine,
                PackageReports = _testPackageReportPaths,
                PackageReportFunc = (path) => new PackageReport()
                                    {
                                        Id = "TestPackage",
                                        Targets = new Dictionary<string, Target>
                                        {
                                            {
                                                "testTarget", new Target
                                                {
                                                    CompileAssets = new PackageAsset[]
                                                    {
                                                        new PackageAsset{  }
                                                    },
                                                    RuntimeAssets = new PackageAsset[]
                                                    {
                                                        new PackageAsset{  }
                                                    },
                                                    NativeAssets = new PackageAsset[]
                                                    {
                                                        new PackageAsset{  }
                                                    }
                                                }
                                            }
                                        }
                                    },
                GetLatestStableVersionFunc = (packageId, major, minor) => string.Empty
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
        }

        [Fact]
        public void ValidationFailsWhenHarvestingFromCurrentVersionTest()
        {
            TestableValidateHarvestVersionTask task = new TestableValidateHarvestVersionTask()
            {
                BuildEngine = _engine,
                PackageReports = _testPackageReportPaths,
                PackageReportFunc = (path) => new PackageReport()
                                    {
                                        Id = "TestPackage",
                                        Version = "4.6.3",
                                        Targets = new Dictionary<string, Target>
                                        {
                                            {
                                                "testTarget", new Target
                                                {
                                                    CompileAssets = new PackageAsset[]
                                                    {
                                                        new PackageAsset{ HarvestedFrom = "TestPackage/4.6.2/ref/netstandard2.0/TestPackage.dll" }
                                                    }
                                                }
                                            }
                                        }
                                    },
                GetLatestStableVersionFunc = (packageId, major, minor) => string.Empty
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(1);
            _log.WarningsLogged.Should().Be(0);
        }

        private class TestableValidateHarvestVersionTask : ValidateHarvestVersionIsLatestForRelease
        {
            public Func<string, PackageReport> PackageReportFunc { get; set; }
            public Func<string, int, int, string> GetLatestStableVersionFunc { get; set; }

            protected override PackageReport GetPackageReportFromPath(string path) => PackageReportFunc(path);

            protected override string GetLatestStableVersionForPackageRelease(string packageId, int majorVersion, int minorVersion) => GetLatestStableVersionFunc(packageId, majorVersion, minorVersion);
        }
    }
}
