// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class MonitorProjectConfigurationFilePathEndpointTest : LanguageServerTestBase
{
    private readonly WorkspaceDirectoryPathResolver _directoryPathResolver;

    public MonitorProjectConfigurationFilePathEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _directoryPathResolver = Mock.Of<WorkspaceDirectoryPathResolver>(resolver => resolver.Resolve() == "C:/dir", MockBehavior.Strict);
    }

    [Fact]
    public async Task Handle_Disposed_Noops()
    {
        // Arrange
        var directoryPathResolver = new Mock<WorkspaceDirectoryPathResolver>(MockBehavior.Strict);
        directoryPathResolver.Setup(resolver => resolver.Resolve())
            .Throws<XunitException>();
        var configurationFileEndpoint = new MonitorProjectConfigurationFilePathEndpoint(
            LegacyDispatcher,
            directoryPathResolver.Object,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            TestLanguageServerFeatureOptions.Instance,
            LoggerFactory);
        configurationFileEndpoint.Dispose();
        var request = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:/dir/project.csproj",
            ConfigurationFilePath = "C:/dir/obj/Debug/project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act & Assert
        await configurationFileEndpoint.HandleNotificationAsync(request, requestContext, DisposalToken);
    }

    [Fact]
    public async Task Handle_ConfigurationFilePath_UntrackedMonitorNoops()
    {
        // Arrange
        var directoryPathResolver = new Mock<WorkspaceDirectoryPathResolver>(MockBehavior.Strict);
        directoryPathResolver.Setup(resolver => resolver.Resolve())
            .Throws<XunitException>();
        var configurationFileEndpoint = new MonitorProjectConfigurationFilePathEndpoint(
            LegacyDispatcher,
            directoryPathResolver.Object,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            TestLanguageServerFeatureOptions.Instance,
            LoggerFactory);
        var request = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:/dir/project.csproj",
            ConfigurationFilePath = null,
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act & Assert
        await configurationFileEndpoint.HandleNotificationAsync(request, requestContext, DisposalToken);
    }

    [Fact]
    public async Task Handle_ConfigurationFilePath_TrackedMonitor_StopsMonitor()
    {
        // Arrange
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:/dir/project.csproj",
            ConfigurationFilePath = "C:/externaldir/obj/Debug/project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);
        var stopRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:/dir/project.csproj",
            ConfigurationFilePath = null,
        };

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(stopRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, detector.StartCount);
        Assert.Equal(1, detector.StopCount);
    }

    [Fact]
    public async Task Handle_InWorkspaceDirectory_Noops()
    {
        // Arrange
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:/dir/project.csproj",
            ConfigurationFilePath = "C:/dir/obj/Debug/project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(0, detector.StartCount);
    }

    [Fact]
    public async Task Handle_DuplicateMonitors_Noops()
    {
        // Arrange
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:/dir/project.csproj",
            ConfigurationFilePath = "C:/externaldir/obj/Debug/project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, detector.StartCount);
        Assert.Equal(0, detector.StopCount);
    }

    [Fact]
    public async Task Handle_ChangedConfigurationOutputPath_StartsWithNewPath()
    {
        // Arrange
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var debugOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:\\dir\\project.csproj",
            ConfigurationFilePath = "C:\\externaldir\\obj\\Debug\\project.razor.json",
        };
        var releaseOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = debugOutputPath.ProjectFilePath,
            ConfigurationFilePath = "C:\\externaldir\\obj\\Release\\project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(debugOutputPath, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(releaseOutputPath, requestContext, DisposalToken);

        // Assert
        Assert.Equal(new[] { "C:\\externaldir\\obj\\Debug", "C:\\externaldir\\obj\\Release" }, detector.StartedWithDirectory);
        Assert.Equal(1, detector.StopCount);
    }

    [Fact]
    public async Task Handle_ChangedConfigurationExternalToInternal_StopsWithoutRestarting()
    {
        // Arrange
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var externalRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:\\dir\\project.csproj",
            ConfigurationFilePath = "C:\\externaldir\\obj\\Debug\\project.razor.json",
        };
        var internalRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = externalRequest.ProjectFilePath,
            ConfigurationFilePath = "C:\\dir\\obj\\Release\\project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(externalRequest, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(internalRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(new[] { "C:\\externaldir\\obj\\Debug" }, detector.StartedWithDirectory);
        Assert.Equal(1, detector.StopCount);
    }

    [Fact]
    public async Task Handle_ProjectPublished()
    {
        // Arrange
        var callCount = 0;
        var projectOpenDebugDetector = new TestFileChangeDetector();
        var releaseDetector = new TestFileChangeDetector();
        var postPublishDebugDetector = new TestFileChangeDetector();
        var detectors = new[] { projectOpenDebugDetector, releaseDetector, postPublishDebugDetector };
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detectors[callCount++],
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var debugOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:\\dir\\project1.csproj",
            ConfigurationFilePath = "C:\\externaldir1\\obj\\Debug\\project.razor.json",
        };
        var releaseOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = debugOutputPath.ProjectFilePath,
            ConfigurationFilePath = "C:\\externaldir1\\obj\\Release\\project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act

        // Project opened, defaults to Debug output path
        await configurationFileEndpoint.HandleNotificationAsync(debugOutputPath, requestContext, DisposalToken);

        // Project published (temporarily moves to release output path)
        await configurationFileEndpoint.HandleNotificationAsync(releaseOutputPath, requestContext, DisposalToken);

        // Project publish finished (moves back to debug output path)
        await configurationFileEndpoint.HandleNotificationAsync(debugOutputPath, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, projectOpenDebugDetector.StartCount);
        Assert.Equal(1, projectOpenDebugDetector.StopCount);
        Assert.Equal(1, releaseDetector.StartCount);
        Assert.Equal(1, releaseDetector.StopCount);
        Assert.Equal(1, postPublishDebugDetector.StartCount);
        Assert.Equal(0, postPublishDebugDetector.StopCount);
    }

    [Fact]
    public async Task Handle_MultipleProjects_StartedAndStopped()
    {
        // Arrange
        var callCount = 0;
        var debug1Detector = new TestFileChangeDetector();
        var debug2Detector = new TestFileChangeDetector();
        var release1Detector = new TestFileChangeDetector();
        var detectors = new[] { debug1Detector, debug2Detector, release1Detector };
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detectors[callCount++],
            LegacyDispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory);
        var debugOutputPath1 = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:\\dir\\project1.csproj",
            ConfigurationFilePath = "C:\\externaldir1\\obj\\Debug\\project.razor.json",
        };
        var releaseOutputPath1 = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = debugOutputPath1.ProjectFilePath,
            ConfigurationFilePath = "C:\\externaldir1\\obj\\Release\\project.razor.json",
        };
        var debugOutputPath2 = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectFilePath = "C:\\dir\\project2.csproj",
            ConfigurationFilePath = "C:\\externaldir2\\obj\\Debug\\project.razor.json",
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(debugOutputPath1, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(debugOutputPath2, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(releaseOutputPath1, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, debug1Detector.StartCount);
        Assert.Equal(1, debug1Detector.StopCount);
        Assert.Equal(1, debug2Detector.StartCount);
        Assert.Equal(0, debug2Detector.StopCount);
        Assert.Equal(1, release1Detector.StartCount);
        Assert.Equal(0, release1Detector.StopCount);
    }

    private class TestMonitorProjectConfigurationFilePathEndpoint : MonitorProjectConfigurationFilePathEndpoint
    {
        private readonly Func<IFileChangeDetector> _fileChangeDetectorFactory;

        public TestMonitorProjectConfigurationFilePathEndpoint(
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            WorkspaceDirectoryPathResolver workspaceDirectoryPathResolver,
            IEnumerable<IProjectConfigurationFileChangeListener> listeners,
            ILoggerFactory loggerFactory)
            : this(
                fileChangeDetectorFactory: null,
                projectSnapshotManagerDispatcher,
                workspaceDirectoryPathResolver,
                listeners,
                loggerFactory)
        {
        }

        public TestMonitorProjectConfigurationFilePathEndpoint(
            Func<IFileChangeDetector> fileChangeDetectorFactory,
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            WorkspaceDirectoryPathResolver workspaceDirectoryPathResolver,
            IEnumerable<IProjectConfigurationFileChangeListener> listeners,
            ILoggerFactory loggerFactory)
            : base(
                projectSnapshotManagerDispatcher,
                workspaceDirectoryPathResolver,
                listeners,
                TestLanguageServerFeatureOptions.Instance,
                loggerFactory)
        {
            _fileChangeDetectorFactory = fileChangeDetectorFactory ?? (() => Mock.Of<IFileChangeDetector>(MockBehavior.Strict));
        }

        protected override IFileChangeDetector CreateFileChangeDetector() => _fileChangeDetectorFactory();
    }

    private class TestFileChangeDetector : IFileChangeDetector
    {
        public int StartCount => StartedWithDirectory.Count;

        public List<string> StartedWithDirectory { get; } = new List<string>();

        public int StopCount { get; private set; }

        public Task StartAsync(string workspaceDirectory, CancellationToken cancellationToken)
        {
            StartedWithDirectory.Add(workspaceDirectory);
            return Task.CompletedTask;
        }

        public void Stop()
        {
            StopCount++;
        }
    }
}
