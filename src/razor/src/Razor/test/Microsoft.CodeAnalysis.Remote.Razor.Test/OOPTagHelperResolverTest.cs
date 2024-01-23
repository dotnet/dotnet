// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Remote.Razor.Test;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public partial class OOPTagHelperResolverTest : TagHelperDescriptorTestBase
{
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly ImmutableArray<IProjectEngineFactory> _customFactories;
    private readonly HostProject _hostProject_For_2_0;
    private readonly HostProject _hostProject_For_NonSerializableConfiguration;
    private readonly ProjectSnapshotManagerBase _projectManager;
    private readonly Project _workspaceProject;
    private readonly Workspace _workspace;

    public OOPTagHelperResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostProject_For_2_0 = new HostProject("Test.csproj", "/obj", FallbackRazorConfiguration.MVC_2_0, rootNamespace: null);
        _hostProject_For_NonSerializableConfiguration = new HostProject(
            "Test.csproj", "/obj",
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_2_1, "Random-0.1", []), rootNamespace: null);

        _customFactories = ImmutableArray.Create(
            CreateFactory("MVC-2.0"),

            // We don't really use this factory, we just use it to ensure that the call is going to go out of process.
            CreateFactory("Test-2"));

        _projectEngineFactoryProvider = new ProjectEngineFactoryProvider(_customFactories);

        var testServices = TestServices.Create([], []);

        _workspace = new AdhocWorkspace(testServices);
        AddDisposable(_workspace);

        var info = ProjectInfo.Create(ProjectId.CreateNewId("Test"), VersionStamp.Default, "Test", "Test", LanguageNames.CSharp, filePath: "Test.csproj");
        _workspaceProject = _workspace.CurrentSolution.AddProject(info).GetProject(info.Id).AssumeNotNull();

        _projectManager = new TestProjectSnapshotManager(_workspace, _projectEngineFactoryProvider);

        static IProjectEngineFactory CreateFactory(string configurationName)
        {
            var mock = new Mock<IProjectEngineFactory>(MockBehavior.Strict);

            mock.SetupGet(x => x.ConfigurationName)
                .Returns(configurationName);

            return mock.Object;
        }
    }

    [Fact]
    public async Task GetTagHelpersAsync_WithSerializableCustomFactory_GoesOutOfProcess()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject_For_2_0);

        var projectSnapshot = _projectManager.GetLoadedProject(_hostProject_For_2_0.Key).AssumeNotNull();

        var calledOutOfProcess = false;

        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance)
        {
            OnResolveOutOfProcess = (p) =>
            {
                calledOutOfProcess = true;

                Assert.Same(projectSnapshot, p);

                return new(ImmutableArray<TagHelperDescriptor>.Empty);
            },
        };

        var result = await resolver.GetTagHelpersAsync(_workspaceProject, projectSnapshot, DisposalToken);

        // Assert
        Assert.True(calledOutOfProcess);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTagHelpersAsync_WithNonSerializableCustomFactory_StaysInProcess()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject_For_NonSerializableConfiguration);

        var projectSnapshot = _projectManager.GetLoadedProject(_hostProject_For_2_0.Key).AssumeNotNull();

        var calledInProcess = false;

        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance)
        {
            OnResolveInProcess = (p) =>
            {
                calledInProcess = true;

                Assert.Same(projectSnapshot, p);

                return new(ImmutableArray<TagHelperDescriptor>.Empty);
            },
        };

        var result = await resolver.GetTagHelpersAsync(_workspaceProject, projectSnapshot, DisposalToken);

        // Assert
        Assert.True(calledInProcess);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTagHelpersAsync_OperationCanceledException_DoesNotGetWrapped()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject_For_2_0);

        var projectSnapshot = _projectManager.GetLoadedProject(_hostProject_For_2_0.Key).AssumeNotNull();

        var calledOutOfProcess = false;
        var calledInProcess = false;

        var cancellationToken = new CancellationToken(canceled: true);
        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance)
        {
            OnResolveInProcess = (p) =>
            {
                calledInProcess = true;
                Assert.Same(projectSnapshot, p);

                return new(ImmutableArray<TagHelperDescriptor>.Empty);
            },
            OnResolveOutOfProcess = (p) =>
            {
                calledOutOfProcess = true;
                Assert.Same(projectSnapshot, p);

                throw new OperationCanceledException();
            }
        };

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await resolver.GetTagHelpersAsync(_workspaceProject, projectSnapshot, cancellationToken));

        Assert.False(calledInProcess);
        Assert.True(calledOutOfProcess);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_NewProject()
    {
        // Arrange
        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(IsDelta: false, ResultId: 1, Project1TagHelperChecksums, ImmutableArray<Checksum>.Empty);

        // Act
        var tagHelpers = resolver.PublicProduceChecksumsFromDelta(Project1Id, lastResultId: -1, initialDelta);

        // Assert
        Assert.Equal<Checksum>(Project1TagHelperChecksums, tagHelpers);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_DeltaFailedToApplyToKnownProject()
    {
        // Arrange
        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(IsDelta: false, ResultId: 1, Project1TagHelperChecksums, ImmutableArray<Checksum>.Empty);
        resolver.PublicProduceChecksumsFromDelta(Project1Id, lastResultId: -1, initialDelta);
        var newTagHelperSet = ImmutableArray.Create(TagHelper1_Project1.Checksum);
        var failedDeltaApplication = new TagHelperDeltaResult(IsDelta: false, initialDelta.ResultId + 1, newTagHelperSet, ImmutableArray<Checksum>.Empty);

        // Act
        var tagHelpers = resolver.PublicProduceChecksumsFromDelta(Project1Id, initialDelta.ResultId, failedDeltaApplication);

        // Assert
        Assert.Equal<Checksum>(newTagHelperSet, tagHelpers);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_NoopResult()
    {
        // Arrange
        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(IsDelta: false, ResultId: 1, Project1TagHelperChecksums, ImmutableArray<Checksum>.Empty);
        resolver.PublicProduceChecksumsFromDelta(Project1Id, lastResultId: -1, initialDelta);
        var noopDelta = new TagHelperDeltaResult(IsDelta: true, initialDelta.ResultId, ImmutableArray<Checksum>.Empty, ImmutableArray<Checksum>.Empty);

        // Act
        var tagHelpers = resolver.PublicProduceChecksumsFromDelta(Project1Id, initialDelta.ResultId, noopDelta);

        // Assert
        Assert.Equal<Checksum>(Project1TagHelperChecksums, tagHelpers);
    }

    [Fact]
    public void CalculateTagHelpersFromDelta_ReplacedTagHelpers()
    {
        // Arrange
        var resolver = new TestResolver(_workspace, ErrorReporter, NoOpTelemetryReporter.Instance);
        var initialDelta = new TagHelperDeltaResult(IsDelta: false, ResultId: 1, Project1TagHelperChecksums, ImmutableArray<Checksum>.Empty);
        resolver.PublicProduceChecksumsFromDelta(Project1Id, lastResultId: -1, initialDelta);
        var changedDelta = new TagHelperDeltaResult(IsDelta: true, initialDelta.ResultId + 1, ImmutableArray.Create(TagHelper2_Project2.Checksum), ImmutableArray.Create(TagHelper2_Project1.Checksum));

        // Act
        var tagHelperChecksums = resolver.PublicProduceChecksumsFromDelta(Project1Id, initialDelta.ResultId, changedDelta);

        // Assert
        var set = new HashSet<Checksum>();
        set.AddRange(tagHelperChecksums);

        Assert.Equal(2, set.Count);
        Assert.Contains(TagHelper1_Project1.Checksum, set);
        Assert.Contains(TagHelper2_Project2.Checksum, set);
    }
}
