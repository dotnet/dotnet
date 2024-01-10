// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Mvc1_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;
using Mvc2_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;
using MvcLatest = Microsoft.AspNetCore.Mvc.Razor.Extensions;

namespace Microsoft.CodeAnalysis.Razor;

// Testing this here because we need references to the MVC factories.
public class DefaultProjectSnapshotProjectEngineFactoryTest : ToolingTestBase
{
    private readonly Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[] _customFactories;
    private readonly IFallbackProjectEngineFactory _fallbackFactory;
    private readonly IProjectSnapshot _snapshot_For_1_0;
    private readonly IProjectSnapshot _snapshot_For_1_1;
    private readonly IProjectSnapshot _snapshot_For_2_0;
    private readonly IProjectSnapshot _snapshot_For_2_1;
    private readonly IProjectSnapshot _snapshot_For_3_0;
    private readonly IProjectSnapshot _snapshot_For_UnknownConfiguration;

    public DefaultProjectSnapshotProjectEngineFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var projectFilePath = "/TestPath/SomePath/Test.csproj";
        var intermediateOutputPath = "/TestPath/SomePath/obj";
        var hostProject_For_1_0 = new HostProject(projectFilePath, intermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, "Test");
        var hostProject_For_1_1 = new HostProject(projectFilePath, intermediateOutputPath, FallbackRazorConfiguration.MVC_1_1, "Test");
        var hostProject_For_2_0 = new HostProject(projectFilePath, intermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, "Test");

        var hostProject_For_2_1 = new HostProject(
            projectFilePath, intermediateOutputPath,
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_2_1, "MVC-2.1", Array.Empty<RazorExtension>()), "Test");

        var hostProject_For_3_0 = new HostProject(
            projectFilePath, intermediateOutputPath,
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_3_0, "MVC-3.0", Array.Empty<RazorExtension>()), "Test");

        var hostProject_For_UnknownConfiguration = new HostProject(
            projectFilePath, intermediateOutputPath,
            new ProjectSystemRazorConfiguration(RazorLanguageVersion.Version_2_1, "Random-0.1", Array.Empty<RazorExtension>()), rootNamespace: null);

        var projectEngineFactory = Mock.Of<ProjectSnapshotProjectEngineFactory>(MockBehavior.Strict);

        _snapshot_For_1_0 = new ProjectSnapshot(ProjectState.Create(projectEngineFactory, hostProject_For_1_0, ProjectWorkspaceState.Default));
        _snapshot_For_1_1 = new ProjectSnapshot(ProjectState.Create(projectEngineFactory, hostProject_For_1_1, ProjectWorkspaceState.Default));
        _snapshot_For_2_0 = new ProjectSnapshot(ProjectState.Create(projectEngineFactory, hostProject_For_2_0, ProjectWorkspaceState.Default));
        _snapshot_For_2_1 = new ProjectSnapshot(ProjectState.Create(projectEngineFactory, hostProject_For_2_1, ProjectWorkspaceState.Default));
        _snapshot_For_3_0 = new ProjectSnapshot(ProjectState.Create(projectEngineFactory, hostProject_For_3_0, ProjectWorkspaceState.Default));
        _snapshot_For_UnknownConfiguration = new ProjectSnapshot(ProjectState.Create(projectEngineFactory, hostProject_For_UnknownConfiguration, ProjectWorkspaceState.Default));

        _customFactories =
        [
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
        ];

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
