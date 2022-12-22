// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Test.Common;
using Microsoft.CodeAnalysis.Remote.Razor.Test;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public class OOPTagHelperResolverTest : TagHelperDescriptorTestBase
{
    private readonly ErrorReporter _errorReporter;
    private readonly ProjectSnapshotProjectEngineFactory _engineFactory;
    private readonly Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[] _customFactories;
    private readonly IFallbackProjectEngineFactory _fallbackFactory;
    private readonly HostProject _hostProject_For_2_0;
    private readonly HostProject _hostProject_For_NonSerializableConfiguration;
    private readonly ProjectSnapshotManagerBase _projectManager;
    private readonly Project _workspaceProject;
    private readonly Workspace _workspace;

    public OOPTagHelperResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostProject_For_2_0 = new HostProject("Test.csproj", FallbackRazorConfiguration.MVC_2_0, rootNamespace: null);
        _hostProject_For_NonSerializableConfiguration = new HostProject(
            "Test.csproj",
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_2_1, "Random-0.1", Array.Empty<RazorExtension>()), rootNamespace: null);

        _customFactories = new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[]
        {
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => Mock.Of<IProjectEngineFactory>(MockBehavior.Strict),
                new ExportCustomProjectEngineFactoryAttribute("MVC-2.0") { SupportsSerialization = true, }),

            // We don't really use this factory, we just use it to ensure that the call is going to go out of process.
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => Mock.Of<IProjectEngineFactory>(MockBehavior.Strict),
                new ExportCustomProjectEngineFactoryAttribute("Test-2") { SupportsSerialization = false, }),
        };

        _fallbackFactory = new FallbackProjectEngineFactory();

        _workspace = new AdhocWorkspace();
        AddDisposable(_workspace);

        var info = ProjectInfo.Create(ProjectId.CreateNewId("Test"), VersionStamp.Default, "Test", "Test", LanguageNames.CSharp, filePath: "Test.csproj");
        _workspaceProject = _workspace.CurrentSolution.AddProject(info).GetProject(info.Id);

        _errorReporter = new DefaultErrorReporter();
        _projectManager = new TestProjectSnapshotManager(_workspace);
        _engineFactory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);
    }

    [Fact]
    public async Task GetTagHelpersAsync_WithSerializableCustomFactory_GoesOutOfProcess()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject_For_2_0);

        var projectSnapshot = _projectManager.GetLoadedProject("Test.csproj");

        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance)
        {
            OnResolveOutOfProcess = (f, p) =>
            {
                Assert.Same(_customFactories[0].Value, f);
                Assert.Same(projectSnapshot, p);

                return Task.FromResult(TagHelperResolutionResult.Empty);
            },
        };

        var result = await resolver.GetTagHelpersAsync(_workspaceProject, projectSnapshot);

        // Assert
        Assert.Same(TagHelperResolutionResult.Empty, result);
    }

    [Fact]
    public async Task GetTagHelpersAsync_WithNonSerializableCustomFactory_StaysInProcess()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject_For_NonSerializableConfiguration);

        var projectSnapshot = _projectManager.GetLoadedProject("Test.csproj");

        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance)
        {
            OnResolveInProcess = (p) =>
            {
                Assert.Same(projectSnapshot, p);

                return Task.FromResult(TagHelperResolutionResult.Empty);
            },
        };

        var result = await resolver.GetTagHelpersAsync(_workspaceProject, projectSnapshot);

        // Assert
        Assert.Same(TagHelperResolutionResult.Empty, result);
    }

    [Fact]
    public async Task GetTagHelpersAsync_OperationCanceledException_DoesNotGetWrapped()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject_For_2_0);

        var projectSnapshot = _projectManager.GetLoadedProject("Test.csproj");

        var cancellationToken = new CancellationToken(canceled: true);
        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance)
        {
            OnResolveInProcess = (p) =>
            {
                Assert.Same(projectSnapshot, p);

                return Task.FromResult(TagHelperResolutionResult.Empty);
            },
            OnResolveOutOfProcess = (f, p) =>
            {
                Assert.Same(projectSnapshot, p);

                throw new OperationCanceledException();
            }
        };

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await resolver.GetTagHelpersAsync(_workspaceProject, projectSnapshot, cancellationToken));
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_NewProject()
    {
        // Arrange
        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(Delta: false, ResultId: 1, Project1TagHelpers, Array.Empty<TagHelperDescriptor>());

        // Act
        var tagHelpers = resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, lastResultId: -1, initialDelta);

        // Assert
        Assert.Equal(Project1TagHelpers, tagHelpers);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_DeltaFailedToApplyToKnownProject()
    {
        // Arrange
        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(Delta: false, ResultId: 1, Project1TagHelpers, Array.Empty<TagHelperDescriptor>());
        resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, lastResultId: -1, initialDelta);
        var newTagHelperSet = new[] { TagHelper1_Project1 };
        var failedDeltaApplication = new TagHelperDeltaResult(Delta: false, initialDelta.ResultId + 1, newTagHelperSet, Array.Empty<TagHelperDescriptor>());

        // Act
        var tagHelpers = resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, initialDelta.ResultId, failedDeltaApplication);

        // Assert
        Assert.Equal(newTagHelperSet, tagHelpers);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_NoopResult()
    {
        // Arrange
        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(Delta: false, ResultId: 1, Project1TagHelpers, Array.Empty<TagHelperDescriptor>());
        resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, lastResultId: -1, initialDelta);
        var noopDelta = new TagHelperDeltaResult(Delta: true, initialDelta.ResultId, Array.Empty<TagHelperDescriptor>(), Array.Empty<TagHelperDescriptor>());

        // Act
        var tagHelpers = resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, initialDelta.ResultId, noopDelta);

        // Assert
        Assert.Equal(Project1TagHelpers, tagHelpers);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_ReplacedTagHelpers()
    {
        // Arrange
        var resolver = new TestTagHelperResolver(_engineFactory, _errorReporter, _workspace, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(Delta: false, ResultId: 1, Project1TagHelpers, Array.Empty<TagHelperDescriptor>());
        resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, lastResultId: -1, initialDelta);
        var changedDelta = new TagHelperDeltaResult(Delta: true, initialDelta.ResultId + 1, new[] { TagHelper2_Project2 }, new[] { TagHelper2_Project1 });

        // Act
        var tagHelpers = resolver.PublicProduceTagHelpersFromDelta(Project1FilePath, initialDelta.ResultId, changedDelta);

        // Assert
        Assert.Equal(new[] { TagHelper1_Project1, TagHelper2_Project2 }, tagHelpers.OrderBy(th => th.Name));
    }

    private class TestTagHelperResolver : OOPTagHelperResolver
    {
        public TestTagHelperResolver(ProjectSnapshotProjectEngineFactory factory, ErrorReporter errorReporter, Workspace workspace, ITelemetryReporter telemetryReporter)
            : base(factory, errorReporter, workspace, telemetryReporter)
        {
        }

        public Func<IProjectEngineFactory, ProjectSnapshot, Task<TagHelperResolutionResult>> OnResolveOutOfProcess { get; set; }

        public Func<ProjectSnapshot, Task<TagHelperResolutionResult>> OnResolveInProcess { get; set; }

        protected override Task<TagHelperResolutionResult> ResolveTagHelpersOutOfProcessAsync(IProjectEngineFactory factory, Project workspaceProject, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            Assert.NotNull(OnResolveOutOfProcess);
            return OnResolveOutOfProcess(factory, projectSnapshot);
        }

        protected override Task<TagHelperResolutionResult> ResolveTagHelpersInProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
        {
            Assert.NotNull(OnResolveInProcess);
            return OnResolveInProcess(projectSnapshot);
        }

        public IReadOnlyCollection<TagHelperDescriptor> PublicProduceTagHelpersFromDelta(string projectFilePath, int lastResultId, TagHelperDeltaResult deltaResult)
            => ProduceTagHelpersFromDelta(projectFilePath, lastResultId, deltaResult);

        protected override IReadOnlyCollection<TagHelperDescriptor> ProduceTagHelpersFromDelta(string projectFilePath, int lastResultId, TagHelperDeltaResult deltaResult)
            => base.ProduceTagHelpersFromDelta(projectFilePath, lastResultId, deltaResult);
    }

    private class TestProjectSnapshotManager : DefaultProjectSnapshotManager
    {
        public TestProjectSnapshotManager(Workspace workspace)
            : base(
                  CreateProjectSnapshotManagerDispatcher(),
                  Mock.Of<ErrorReporter>(MockBehavior.Strict),
                  Enumerable.Empty<ProjectSnapshotChangeTrigger>(),
                  workspace)
        {
        }

        private static ProjectSnapshotManagerDispatcher CreateProjectSnapshotManagerDispatcher()
        {
            var dispatcher = new Mock<ProjectSnapshotManagerDispatcher>(MockBehavior.Strict);
            dispatcher.Setup(d => d.AssertDispatcherThread(It.IsAny<string>())).Verifiable();
            dispatcher.Setup(d => d.IsDispatcherThread).Returns(true);
            dispatcher.Setup(d => d.DispatcherScheduler).Returns(TaskScheduler.FromCurrentSynchronizationContext());
            return dispatcher.Object;
        }
    }
}
