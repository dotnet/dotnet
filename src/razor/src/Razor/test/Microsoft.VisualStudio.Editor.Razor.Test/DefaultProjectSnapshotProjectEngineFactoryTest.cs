// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Xunit;
using Xunit.Abstractions;
using Mvc1_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;
using Mvc2_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;
using MvcLatest = Microsoft.AspNetCore.Mvc.Razor.Extensions;

namespace Microsoft.CodeAnalysis.Razor;

// Testing this here because we need references to the MVC factories.
public class DefaultProjectSnapshotProjectEngineFactoryTest : TestBase
{
    private readonly Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[] _customFactories;
    private readonly IFallbackProjectEngineFactory _fallbackFactory;
    private readonly HostProject _hostProject_For_1_0;
    private readonly HostProject _hostProject_For_1_1;
    private readonly HostProject _hostProject_For_2_0;
    private readonly HostProject _hostProject_For_2_1;
    private readonly HostProject _hostProject_For_3_0;
    private readonly HostProject _hostProject_For_UnknownConfiguration;
    private readonly ProjectSnapshot _snapshot_For_1_0;
    private readonly ProjectSnapshot _snapshot_For_1_1;
    private readonly ProjectSnapshot _snapshot_For_2_0;
    private readonly ProjectSnapshot _snapshot_For_2_1;
    private readonly ProjectSnapshot _snapshot_For_3_0;
    private readonly ProjectSnapshot _snapshot_For_UnknownConfiguration;
    private readonly ProjectWorkspaceState _projectWorkspaceState;
    private readonly Workspace _workspace;

    public DefaultProjectSnapshotProjectEngineFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _workspace = TestWorkspace.Create();
        AddDisposable(_workspace);

        _projectWorkspaceState = ProjectWorkspaceState.Default;

        _hostProject_For_1_0 = new HostProject("/TestPath/SomePath/Test.csproj", FallbackRazorConfiguration.MVC_1_0, "Test");
        _hostProject_For_1_1 = new HostProject("/TestPath/SomePath/Test.csproj", FallbackRazorConfiguration.MVC_1_1, "Test");
        _hostProject_For_2_0 = new HostProject("/TestPath/SomePath/Test.csproj", FallbackRazorConfiguration.MVC_2_0, "Test");

        _hostProject_For_2_1 = new HostProject(
            "/TestPath/SomePath/Test.csproj",
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_2_1, "MVC-2.1", Array.Empty<RazorExtension>()), "Test");

        _hostProject_For_3_0 = new HostProject(
            "/TestPath/SomePath/Test.csproj",
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_3_0, "MVC-3.0", Array.Empty<RazorExtension>()), "Test");

        _hostProject_For_UnknownConfiguration = new HostProject(
            "/TestPath/SomePath/Test.csproj",
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_2_1, "Random-0.1", Array.Empty<RazorExtension>()), rootNamespace: null);

        _snapshot_For_1_0 = new DefaultProjectSnapshot(ProjectState.Create(_workspace.Services, _hostProject_For_1_0, _projectWorkspaceState));
        _snapshot_For_1_1 = new DefaultProjectSnapshot(ProjectState.Create(_workspace.Services, _hostProject_For_1_1, _projectWorkspaceState));
        _snapshot_For_2_0 = new DefaultProjectSnapshot(ProjectState.Create(_workspace.Services, _hostProject_For_2_0, _projectWorkspaceState));
        _snapshot_For_2_1 = new DefaultProjectSnapshot(ProjectState.Create(_workspace.Services, _hostProject_For_2_1, _projectWorkspaceState));
        _snapshot_For_3_0 = new DefaultProjectSnapshot(ProjectState.Create(_workspace.Services, _hostProject_For_3_0, _projectWorkspaceState));
        _snapshot_For_UnknownConfiguration = new DefaultProjectSnapshot(ProjectState.Create(_workspace.Services, _hostProject_For_UnknownConfiguration, _projectWorkspaceState));

        _customFactories = new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[]
        {
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => new LegacyProjectEngineFactory_1_0(),
                typeof(LegacyProjectEngineFactory_1_0).GetCustomAttribute<ExportCustomProjectEngineFactoryAttribute>()),
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => new LegacyProjectEngineFactory_1_1(),
                typeof(LegacyProjectEngineFactory_1_1).GetCustomAttribute<ExportCustomProjectEngineFactoryAttribute>()),
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => new LegacyProjectEngineFactory_2_0(),
                typeof(LegacyProjectEngineFactory_2_0).GetCustomAttribute<ExportCustomProjectEngineFactoryAttribute>()),
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => new LegacyProjectEngineFactory_2_1(),
                typeof(LegacyProjectEngineFactory_2_1).GetCustomAttribute<ExportCustomProjectEngineFactoryAttribute>()),
            new Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>(
                () => new LegacyProjectEngineFactory_3_0(),
                typeof(LegacyProjectEngineFactory_3_0).GetCustomAttribute<ExportCustomProjectEngineFactoryAttribute>()),
        };

        _fallbackFactory = new FallbackProjectEngineFactory();
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion3_0()
    {
        // Arrange
        var snapshot = _snapshot_For_3_0;

        var factory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);

        // Act
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion2_1()
    {
        // Arrange
        var snapshot = _snapshot_For_2_1;

        var factory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);

        // Act
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.MvcViewDocumentClassifierPass>());

        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion2_0()
    {
        // Arrange
        var snapshot = _snapshot_For_2_0;

        var factory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);

        // Act
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesTemplateEngine_ForVersion1_1()
    {
        // Arrange
        var snapshot = _snapshot_For_1_1;

        var factory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);

        // Act
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_DoesNotSupportViewComponentTagHelpers_ForVersion1_0()
    {
        // Arrange
        var snapshot = _snapshot_For_1_0;

        var factory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);

        // Act
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.MvcViewDocumentClassifierPass>());

        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());

        Assert.Empty(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperPass>());

        Assert.Empty(engine.Engine.Features.OfType<Mvc1_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<Mvc1_X.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_ForUnknownConfiguration_UsesFallbackFactory()
    {
        var snapshot = _snapshot_For_UnknownConfiguration;

        var factory = new DefaultProjectSnapshotProjectEngineFactory(_fallbackFactory, _customFactories);

        // Act
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Empty(engine.Engine.Features.OfType<DefaultTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());
    }

    private class MyCoolNewFeature : IRazorEngineFeature
    {
        public RazorEngine Engine { get; set; }
    }
}
