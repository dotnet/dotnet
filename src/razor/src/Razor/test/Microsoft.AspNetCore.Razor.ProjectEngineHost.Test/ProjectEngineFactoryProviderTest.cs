// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;
using Mvc1_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;
using Mvc2_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;
using MvcLatest = Microsoft.AspNetCore.Mvc.Razor.Extensions;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

// Testing this here because we need references to the MVC factories.
public class ProjectEngineFactoryProviderTest : ToolingTestBase
{
    private readonly ImmutableArray<IProjectEngineFactory> _customFactories;
    private readonly IProjectSnapshot _snapshot_For_1_0;
    private readonly IProjectSnapshot _snapshot_For_1_1;
    private readonly IProjectSnapshot _snapshot_For_2_0;
    private readonly IProjectSnapshot _snapshot_For_2_1;
    private readonly IProjectSnapshot _snapshot_For_3_0;
    private readonly IProjectSnapshot _snapshot_For_UnknownConfiguration;

    public ProjectEngineFactoryProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var projectFilePath = "/TestPath/SomePath/Test.csproj";
        var intermediateOutputPath = "/TestPath/SomePath/obj";
        var hostProject_For_1_0 = new HostProject(projectFilePath, intermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, "Test");
        var hostProject_For_1_1 = new HostProject(projectFilePath, intermediateOutputPath, FallbackRazorConfiguration.MVC_1_1, "Test");
        var hostProject_For_2_0 = new HostProject(projectFilePath, intermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, "Test");

        var hostProject_For_2_1 = new HostProject(
            projectFilePath, intermediateOutputPath,
            new(RazorLanguageVersion.Version_2_1, "MVC-2.1", Extensions: []), "Test");

        var hostProject_For_3_0 = new HostProject(
            projectFilePath, intermediateOutputPath,
            new(RazorLanguageVersion.Version_3_0, "MVC-3.0", Extensions: []), "Test");

        var hostProject_For_UnknownConfiguration = new HostProject(
            projectFilePath, intermediateOutputPath,
            new(RazorLanguageVersion.Version_2_1, "Random-0.1", Extensions: []), rootNamespace: null);

        _snapshot_For_1_0 = new ProjectSnapshot(ProjectState.Create(ProjectEngineFactories.DefaultProvider, hostProject_For_1_0, ProjectWorkspaceState.Default));
        _snapshot_For_1_1 = new ProjectSnapshot(ProjectState.Create(ProjectEngineFactories.DefaultProvider, hostProject_For_1_1, ProjectWorkspaceState.Default));
        _snapshot_For_2_0 = new ProjectSnapshot(ProjectState.Create(ProjectEngineFactories.DefaultProvider, hostProject_For_2_0, ProjectWorkspaceState.Default));
        _snapshot_For_2_1 = new ProjectSnapshot(ProjectState.Create(ProjectEngineFactories.DefaultProvider, hostProject_For_2_1, ProjectWorkspaceState.Default));
        _snapshot_For_3_0 = new ProjectSnapshot(ProjectState.Create(ProjectEngineFactories.DefaultProvider, hostProject_For_3_0, ProjectWorkspaceState.Default));
        _snapshot_For_UnknownConfiguration = new ProjectSnapshot(ProjectState.Create(ProjectEngineFactories.DefaultProvider, hostProject_For_UnknownConfiguration, ProjectWorkspaceState.Default));

        _customFactories =
        [
            ProjectEngineFactories.MVC_1_0,
            ProjectEngineFactories.MVC_1_1,
            ProjectEngineFactories.MVC_2_0,
            ProjectEngineFactories.MVC_2_1,
            ProjectEngineFactories.MVC_3_0,
        ];
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion3_0()
    {
        // Arrange
        var snapshot = _snapshot_For_3_0;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
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

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.MvcViewDocumentClassifierPass>());

        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion2_0()
    {
        // Arrange
        var snapshot = _snapshot_For_2_0;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesTemplateEngine_ForVersion1_1()
    {
        // Arrange
        var snapshot = _snapshot_For_1_1;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_DoesNotSupportViewComponentTagHelpers_ForVersion1_0()
    {
        // Arrange
        var snapshot = _snapshot_For_1_0;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(snapshot, b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.Features.OfType<MyCoolNewFeature>());
        Assert.Single(engine.Engine.Features.OfType<Mvc1_X.MvcViewDocumentClassifierPass>());

        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());

        Assert.Empty(engine.Engine.Features.OfType<Mvc2_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());

        Assert.Empty(engine.Engine.Features.OfType<Mvc1_X.ViewComponentTagHelperDescriptorProvider>());
        Assert.Empty(engine.Engine.Features.OfType<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_ForUnknownConfiguration_UsesFallbackFactory()
    {
        var snapshot = _snapshot_For_UnknownConfiguration;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
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
