// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class ApplyBaseLineTests
    {
        private Log _log;
        private TestBuildEngine _engine;
        private ITaskItem[] _packageIndexes;

        public ApplyBaseLineTests(ITestOutputHelper output)
        {
            _log = new Log(output);
            _engine = new TestBuildEngine(_log);
            _packageIndexes = new[] { new TaskItem("packageIndex.json") };
        }

        [Fact]
        public void ApplyBaseLineLiftToBaseLine()
        {
            ITaskItem[] dependencies = new[]
            {
                CreateItem("System.Runtime", "4.0.0")
            };

            ApplyBaseLine task = new ApplyBaseLine()
            {
                BuildEngine = _engine,
                Apply = true,
                PackageIndexes = _packageIndexes,
                OriginalDependencies = dependencies
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.BaseLinedDependencies.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("System.Runtime");
                    item.GetMetadata("Version").Should().Be("4.0.21");
                });
        }

        [Fact]
        public void DontApplyBaseLineIfGreater()
        {

            ITaskItem[] dependencies = new[]
            {
                CreateItem("System.Runtime", "4.1.0")
            };

            ApplyBaseLine task = new ApplyBaseLine()
            {
                BuildEngine = _engine,
                Apply = true,
                PackageIndexes = _packageIndexes,
                OriginalDependencies = dependencies
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.BaseLinedDependencies.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("System.Runtime");
                    item.GetMetadata("Version").Should().Be("4.1.0");
                });
        }

        [Fact]
        public void ApplyBaselineToUnversionedDependency()
        {
            ITaskItem[] dependencies = new[]
            {
                CreateItem("System.Runtime", null)
            };

            ApplyBaseLine task = new ApplyBaseLine()
            {
                BuildEngine = _engine,
                Apply = true,
                PackageIndexes = _packageIndexes,
                OriginalDependencies = dependencies
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.BaseLinedDependencies.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("System.Runtime");
                    item.GetMetadata("Version").Should().Be("4.0.21");
                });
        }

        [Fact]
        public void ApplyBaselineToUntrackedDependency()
        {

            ITaskItem[] dependencies = new[]
            {
                CreateItem("System.Banana", "4.0.0")
            };

            ApplyBaseLine task = new ApplyBaseLine()
            {
                BuildEngine = _engine,
                Apply = true,
                PackageIndexes = _packageIndexes,
                OriginalDependencies = dependencies
            };

            _log.Reset();
            task.Execute();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            task.BaseLinedDependencies.Should().SatisfyRespectively(
                item =>
                {
                    item.ItemSpec.Should().Be("System.Banana");
                    item.GetMetadata("Version").Should().Be("4.0.0");
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
