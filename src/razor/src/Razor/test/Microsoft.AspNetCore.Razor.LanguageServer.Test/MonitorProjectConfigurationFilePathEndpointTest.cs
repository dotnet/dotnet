// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
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
        var path = PathUtilities.CreateRootedPath("dir");
        _directoryPathResolver = Mock.Of<WorkspaceDirectoryPathResolver>(resolver => resolver.Resolve() == path, MockBehavior.Strict);
    }

    [Fact]
    public async Task Handle_Disposed_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var directoryPathResolver = new Mock<WorkspaceDirectoryPathResolver>(MockBehavior.Strict);
        directoryPathResolver.Setup(resolver => resolver.Resolve())
            .Throws<Exception>();
        var configurationFileEndpoint = new MonitorProjectConfigurationFilePathEndpoint(
            projectManager,
            Dispatcher,
            directoryPathResolver.Object,
            listeners: [],
            TestLanguageServerFeatureOptions.Instance,
            LoggerFactory);
        configurationFileEndpoint.Dispose();

        var debugDirectory = PathUtilities.CreateRootedPath("dir", "obj", "Debug");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var request = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act & Assert
        await configurationFileEndpoint.HandleNotificationAsync(request, requestContext, DisposalToken);
    }

    [Fact]
    public async Task Handle_ConfigurationFilePath_UntrackedMonitorNoops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var directoryPathResolver = new Mock<WorkspaceDirectoryPathResolver>(MockBehavior.Strict);
        directoryPathResolver.Setup(resolver => resolver.Resolve())
            .Throws<Exception>();
        var configurationFileEndpoint = new MonitorProjectConfigurationFilePathEndpoint(
            projectManager,
            Dispatcher,
            directoryPathResolver.Object,
            listeners: [],
            TestLanguageServerFeatureOptions.Instance,
            LoggerFactory);

        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var request = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = null!,
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act & Assert
        await configurationFileEndpoint.HandleNotificationAsync(request, requestContext, DisposalToken);
    }

    [Fact]
    public async Task Handle_ConfigurationFilePath_TrackedMonitor_StopsMonitor()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory = PathUtilities.CreateRootedPath("externaldir", "obj", "Debug");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);
        var stopRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = null!,
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
        var projectManager = CreateProjectSnapshotManager();
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory = PathUtilities.CreateRootedPath("dir", "obj", "Debug");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(0, detector.StartCount);
    }

    [Fact]
    public async Task Handle_InWorkspaceDirectory_MonitorsIfLanguageFeatureOptionSet()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager,
            options: new TestLanguageServerFeatureOptions(monitorWorkspaceFolderForConfigurationFiles: false));

        var debugDirectory = PathUtilities.CreateRootedPath("dir", "obj", "Debug");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, detector.StartCount);
    }

    [Fact]
    public async Task Handle_DuplicateMonitors_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory = PathUtilities.CreateRootedPath("externaldir", "obj", "Debug");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
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
        var projectManager = CreateProjectSnapshotManager();
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory = PathUtilities.CreateRootedPath("externaldir", "obj", "Debug");
        var releaseDirectory = PathUtilities.CreateRootedPath("externaldir", "obj", "Release");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var debugOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };

        var releaseOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(releaseDirectory, "project.razor.bin")
        };

        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(debugOutputPath, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(releaseOutputPath, requestContext, DisposalToken);

        // Assert
        Assert.Equal([debugDirectory, releaseDirectory], detector.StartedWithDirectory);
        Assert.Equal(1, detector.StopCount);
    }

    [Fact]
    public async Task Handle_ChangedConfigurationExternalToInternal_StopsWithoutRestarting()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory = PathUtilities.CreateRootedPath("externaldir", "obj", "Debug");
        var releaseDirectory = PathUtilities.CreateRootedPath("dir", "obj", "Release");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var externalRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };

        var internalRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(releaseDirectory, "project.razor.bin")
        };

        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(externalRequest, requestContext, DisposalToken);
        await configurationFileEndpoint.HandleNotificationAsync(internalRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal([debugDirectory], detector.StartedWithDirectory);
        Assert.Equal(1, detector.StopCount);
    }

    [Fact]
    public async Task Handle_ProjectPublished()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var callCount = 0;
        var projectOpenDebugDetector = new TestFileChangeDetector();
        var releaseDetector = new TestFileChangeDetector();
        var postPublishDebugDetector = new TestFileChangeDetector();
        var detectors = new[] { projectOpenDebugDetector, releaseDetector, postPublishDebugDetector };
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detectors[callCount++],
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory = PathUtilities.CreateRootedPath("externaldir1", "obj", "Debug");
        var releaseDirectory = PathUtilities.CreateRootedPath("externaldir1", "obj", "Release");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        var debugOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };

        var releaseOutputPath = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(releaseDirectory, "project.razor.bin")
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
        var projectManager = CreateProjectSnapshotManager();
        var callCount = 0;
        var debug1Detector = new TestFileChangeDetector();
        var debug2Detector = new TestFileChangeDetector();
        var release1Detector = new TestFileChangeDetector();
        var detectors = new[] { debug1Detector, debug2Detector, release1Detector };
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detectors[callCount++],
            Dispatcher,
            _directoryPathResolver,
            listeners: [],
            LoggerFactory,
            projectManager);

        var debugDirectory1 = PathUtilities.CreateRootedPath("externaldir1", "obj", "Debug");
        var releaseDirectory1 = PathUtilities.CreateRootedPath("externaldir1", "obj", "Release");
        var debugDirectory2 = PathUtilities.CreateRootedPath("externaldir2", "obj", "Debug");
        var projectKeyDirectory1 = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey1 = TestProjectKey.Create(projectKeyDirectory1);
        var projectKeyDirectory2 = PathUtilities.CreateRootedPath("dir", "obj2");
        var projectKey2 = TestProjectKey.Create(projectKeyDirectory2);

        var debugOutputPath1 = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey1.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory1, "project.razor.bin")
        };

        var releaseOutputPath1 = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey1.Id,
            ConfigurationFilePath = Path.Combine(releaseDirectory1, "project.razor.bin")
        };

        var debugOutputPath2 = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey2.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory2, "project.razor.bin")
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

    [Fact]
    public async Task Handle_ConfigurationFilePath_TrackedMonitor_RemovesProject()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var detector = new TestFileChangeDetector();
        var configurationFileEndpoint = new TestMonitorProjectConfigurationFilePathEndpoint(
            () => detector,
            Dispatcher,
            _directoryPathResolver,
            Enumerable.Empty<IProjectConfigurationFileChangeListener>(),
            LoggerFactory,
            projectManager,
            options: new TestLanguageServerFeatureOptions(monitorWorkspaceFolderForConfigurationFiles: false));

        var debugDirectory = PathUtilities.CreateRootedPath("externaldir", "obj", "Debug");
        var projectKeyDirectory = PathUtilities.CreateRootedPath("dir", "obj");
        var projectKey = TestProjectKey.Create(projectKeyDirectory);

        //projectSnapshotManagerAccessor
        //    .Setup(a => a.Instance.ProjectRemoved(It.IsAny<ProjectKey>()))
        //    .Callback((ProjectKey key) => Assert.Equal(projectKey, key));

        var startRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = Path.Combine(debugDirectory, "project.razor.bin")
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);
        await configurationFileEndpoint.HandleNotificationAsync(startRequest, requestContext, DisposalToken);
        var stopRequest = new MonitorProjectConfigurationFilePathParams()
        {
            ProjectKeyId = projectKey.Id,
            ConfigurationFilePath = null!,
        };

        // Act
        await configurationFileEndpoint.HandleNotificationAsync(stopRequest, requestContext, DisposalToken);

        // Assert
        Assert.Equal(1, detector.StartCount);
        Assert.Equal(1, detector.StopCount);
    }

    private class TestMonitorProjectConfigurationFilePathEndpoint(
        Func<IFileChangeDetector> fileChangeDetectorFactory,
        ProjectSnapshotManagerDispatcher dispatcher,
        WorkspaceDirectoryPathResolver workspaceDirectoryPathResolver,
        IEnumerable<IProjectConfigurationFileChangeListener> listeners,
        IRazorLoggerFactory loggerFactory,
        IProjectSnapshotManager projectManager,
        LanguageServerFeatureOptions? options = null) : MonitorProjectConfigurationFilePathEndpoint(
            projectManager,
            dispatcher,
            workspaceDirectoryPathResolver,
            listeners,
            options ?? TestLanguageServerFeatureOptions.Instance,
            loggerFactory)
    {
        private readonly Func<IFileChangeDetector> _fileChangeDetectorFactory = fileChangeDetectorFactory ?? (() => Mock.Of<IFileChangeDetector>(MockBehavior.Strict));

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
