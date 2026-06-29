// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;

#nullable enable
namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class CreateMTPWorkItemsTests
    {
        private static CreateMTPWorkItems CreateTask() =>
            new CreateMTPWorkItems
            {
                BuildEngine = new MockBuildEngine(),
                IsPosixShell = false,
            };

        private static ITaskItem CreateProject(
            string itemSpec,
            string? publishDirectory = "/publish",
            string? targetPath = "/publish/MyApp.Tests.dll",
            string? arguments = null,
            string? additionalProperties = null)
        {
            var metadata = new Dictionary<string, string>();
            if (publishDirectory != null) metadata["PublishDirectory"] = publishDirectory;
            if (targetPath != null) metadata["TargetPath"] = targetPath;
            if (arguments != null) metadata["Arguments"] = arguments;
            if (additionalProperties != null) metadata["AdditionalProperties"] = additionalProperties;
            return new TaskItem(itemSpec, metadata);
        }

        [Fact]
        public void GeneratedCommandHasExpectedShape()
        {
            var task = CreateTask();
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj") };

            task.Execute().Should().BeTrue();

            task.MTPWorkItems.Should().HaveCount(1);
            var workItem = task.MTPWorkItems.Single();
            workItem.GetMetadata("Identity").Should().Be("MyApp.Tests.dll");
            workItem.GetMetadata("PayloadDirectory").Should().Be("/publish");
            workItem.GetMetadata("Timeout").Should().Be("00:05:00");

            var command = workItem.GetMetadata("Command");
            command.Should().StartWith("dotnet exec --roll-forward Major ");
            command.Should().Contain("--runtimeconfig MyApp.Tests.runtimeconfig.json");
            command.Should().Contain("--depsfile MyApp.Tests.deps.json");
            command.Should().Contain("MyApp.Tests.dll");
            command.Should().EndWith(" --results-directory .");
            command.Should().NotContain("--report-trx");
            command.Should().NotContain("--auto-reporters");
        }

        [Fact]
        public void ArgumentsMetadataIsAppendedAfterReporterArgs()
        {
            var task = CreateTask();
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj", arguments: "--filter Category=Smoke") };

            task.Execute().Should().BeTrue();

            var command = task.MTPWorkItems.Single().GetMetadata("Command");
            command.Should().EndWith("--results-directory . --filter Category=Smoke");
        }

        [Fact]
        public void MTPAdditionalArgumentsIsInsertedBeforePerProjectArguments()
        {
            var task = CreateTask();
            task.MTPAdditionalArguments = "--report-trx --report-trx-filename testResults.trx --auto-reporters off";
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj", arguments: "--filter Category=Smoke") };

            task.Execute().Should().BeTrue();

            var command = task.MTPWorkItems.Single().GetMetadata("Command");
            command.Should().EndWith("--results-directory . --report-trx --report-trx-filename testResults.trx --auto-reporters off --filter Category=Smoke");
        }

        [Fact]
        public void MTPAdditionalArgumentsAloneIsAppendedAfterReporterArgs()
        {
            var task = CreateTask();
            task.MTPAdditionalArguments = "--auto-reporters off";
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj") };

            task.Execute().Should().BeTrue();

            var command = task.MTPWorkItems.Single().GetMetadata("Command");
            command.Should().EndWith("--results-directory . --auto-reporters off");
        }

        [Fact]
        public void MissingPublishDirectoryReturnsNoWorkItem()
        {
            var task = CreateTask();
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj", publishDirectory: null) };

            task.Execute().Should().BeFalse();
            task.MTPWorkItems.Should().BeEmpty();
        }

        [Fact]
        public void MissingTargetPathReturnsNoWorkItem()
        {
            var task = CreateTask();
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj", targetPath: null) };

            task.Execute().Should().BeFalse();
            task.MTPWorkItems.Should().BeEmpty();
        }

        [Fact]
        public void ValidTimeoutIsApplied()
        {
            var task = CreateTask();
            task.MTPWorkItemTimeout = "00:12:34";
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj") };

            task.Execute().Should().BeTrue();
            task.MTPWorkItems.Single().GetMetadata("Timeout").Should().Be("00:12:34");
        }

        [Fact]
        public void InvalidTimeoutFallsBackToDefaultWithWarning()
        {
            var task = CreateTask();
            var buildEngine = (MockBuildEngine)task.BuildEngine;
            task.MTPWorkItemTimeout = "not-a-timespan";
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj") };

            task.Execute().Should().BeTrue();
            task.MTPWorkItems.Single().GetMetadata("Timeout").Should().Be("00:05:00");
            buildEngine.BuildWarningEvents.Should().Contain(e => e.Message != null && e.Message.Contains("not-a-timespan"));
        }

        [Fact]
        public void DuplicateInputsArePassedThroughAsSeparateWorkItems()
        {
            // The task does not deduplicate - it mirrors the old CreateXUnitV3WorkItems behavior
            // and trusts the caller to provide a clean item list (MSBuild's RemoveDuplicates can
            // be used upstream if needed).
            var task = CreateTask();
            task.MTPProjects = new[]
            {
                CreateProject("MyApp.Tests.csproj"),
                CreateProject("MyApp.Tests.csproj"),
            };

            task.Execute().Should().BeTrue();
            task.MTPWorkItems.Should().HaveCount(2);
        }

        [Fact]
        public void SameIdentityWithDifferentAdditionalPropertiesProducesSeparateWorkItems()
        {
            var task = CreateTask();
            task.MTPProjects = new[]
            {
                CreateProject(
                    "MyApp.Tests.csproj",
                    publishDirectory: "/publish/net8.0",
                    targetPath: "/publish/net8.0/MyApp.Tests.dll",
                    additionalProperties: "TargetFramework=net8.0"),
                CreateProject(
                    "MyApp.Tests.csproj",
                    publishDirectory: "/publish/net9.0",
                    targetPath: "/publish/net9.0/MyApp.Tests.dll",
                    additionalProperties: "TargetFramework=net9.0"),
            };

            task.Execute().Should().BeTrue();
            task.MTPWorkItems.Should().HaveCount(2);
            task.MTPWorkItems.Select(w => w.GetMetadata("PayloadDirectory"))
                .Should().BeEquivalentTo(new[] { "/publish/net8.0", "/publish/net9.0" });
        }

        [Fact]
        public void PathToDotnetIsHonored()
        {
            var task = CreateTask();
            task.PathToDotnet = "/custom/dotnet";
            task.MTPProjects = new[] { CreateProject("MyApp.Tests.csproj") };

            task.Execute().Should().BeTrue();
            task.MTPWorkItems.Single().GetMetadata("Command").Should().StartWith("/custom/dotnet exec ");
        }
    }
}
