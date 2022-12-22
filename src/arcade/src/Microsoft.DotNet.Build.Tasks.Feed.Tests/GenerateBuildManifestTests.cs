// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class GenerateBuildManifestTests
    {
        [Fact]
        public void AreDependenciesRegistered()
        {
            GenerateBuildManifest task = new GenerateBuildManifest();

            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();

            DependencyInjectionValidation.IsDependencyResolutionCoherent(
                    s =>
                    {
                        task.ConfigureServices(s);
                    },
                    out string message,
                    additionalSingletonTypes: task.GetExecuteParameterTypes()
                )
                .Should()
                .BeTrue(message);
        }

        [Fact]
        public void ProducesManifestWithNewPublishingVersion()
        {
            var manifestPath = Path.Combine("C:", "manifests", "TestManifest.xml");
            var publishingVersion = "3";
            var task = new GenerateBuildManifest
            {
                BuildEngine = new MockBuildEngine(),
                Artifacts = new TaskItem[0],
                OutputPath = manifestPath,
                BuildData = new string[] { $"InitialAssetsLocation=cloud" },
                PublishingVersion = publishingVersion,
            };

            // Mocks
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            IList<string> actualPath = new List<string>();
            IList<string> actualBuildModel = new List<string>();
            fileSystemMock.Setup(m => m.WriteToFile(Capture.In(actualPath), Capture.In(actualBuildModel))).Verifiable();

            // Dependency Injection setup
            var collection = new ServiceCollection()
                .AddSingleton(fileSystemMock.Object)
                .AddSingleton<IBuildModelFactory, BuildModelFactory>();
            task.ConfigureServices(collection);
            using var provider = collection.BuildServiceProvider();

            // Act and Assert
            task.InvokeExecute(provider).Should().BeTrue();
            actualPath[0].Should().Be(manifestPath);
            actualBuildModel[0].Should().Contain($"PublishingVersion=\"{publishingVersion}\"");
        }
    }
}
