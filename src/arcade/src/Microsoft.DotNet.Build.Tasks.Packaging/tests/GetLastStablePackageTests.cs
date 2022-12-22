// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class GetLastStablePackageTests
    {
        private Log _log;
        private TestBuildEngine _engine;
        private ITaskItem[] _packageIndexes;

        public GetLastStablePackageTests(ITestOutputHelper output)
        {
            _log = new Log(output);
            _engine = new TestBuildEngine(_log);
            _packageIndexes = new[] { new TaskItem("packageIndex.json") };
        }

        [Fact]
        public void LaterPreReleaseGetsStable()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", "1.2.0-pre")
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("StableVersionTest");
                    item.GetMetadata("Version").Should().Be("1.1.0");
                });
        }


        [Fact]
        public void StableGetsPreviousStable()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", "1.1.0")
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("StableVersionTest");
                    item.GetMetadata("Version").Should().Be("1.0.0");
                });
        }

        [Fact]
        public void PreGetsPreviousStable()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", "1.1.0-pre")
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("StableVersionTest");
                    item.GetMetadata("Version").Should().Be("1.0.0");
                });
        }

        [Fact]
        public void PriorToStableGetsNothing()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", "1.0.0-pre")
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().BeEmpty();
        }

        [Fact]
        public void DoNotAllowSameReleasePackageVersions()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", "1.1.1-pre")
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages,
                DoNotAllowVersionsFromSameRelease = true
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("StableVersionTest");
                    item.GetMetadata("Version").Should().Be("1.0.0");
                });
        }

        [Fact]
        public void NullVersionShouldUseLatestStableVersion()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", null)
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages,
                DoNotAllowVersionsFromSameRelease = true
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("StableVersionTest");
                    item.GetMetadata("Version").Should().Be("1.1.0");
                });
        }

        [Fact]
        public void AllowSameReleasePackageVersions()
        {
            ITaskItem[] latestPackages = new[]
            {
                CreateItem("StableVersionTest", "1.1.1-pre")
            };

            GetLastStablePackage task = new GetLastStablePackage()
            {
                BuildEngine = _engine,
                PackageIndexes = _packageIndexes,
                LatestPackages = latestPackages
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.LastStablePackages.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("StableVersionTest");
                    item.GetMetadata("Version").Should().Be("1.1.0");
                });
        }

        private static ITaskItem CreateItem(string name, string version)
        {
            TaskItem item = new TaskItem(name);

            if (version != null)
            {
                item.SetMetadata("Version", version);
            }

            return item;
        }
    }
}
