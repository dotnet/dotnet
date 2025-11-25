// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.TagHelpers.Producers;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;
using Mvc1_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;
using Mvc2_X = Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;
using MvcLatest = Microsoft.AspNetCore.Mvc.Razor.Extensions;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost.Test;

// Testing this here because we need references to the MVC factories.
public class ProjectEngineFactoryProviderTest : ToolingTestBase
{
    private readonly ImmutableArray<IProjectEngineFactory> _customFactories;
    private readonly ProjectSnapshot _snapshot_For_1_0;
    private readonly ProjectSnapshot _snapshot_For_1_1;
    private readonly ProjectSnapshot _snapshot_For_2_0;
    private readonly ProjectSnapshot _snapshot_For_2_1;
    private readonly ProjectSnapshot _snapshot_For_3_0;
    private readonly ProjectSnapshot _snapshot_For_UnknownConfiguration;

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

        _snapshot_For_1_0 = new ProjectSnapshot(ProjectState.Create(hostProject_For_1_0, ProjectEngineFactories.DefaultProvider));
        _snapshot_For_1_1 = new ProjectSnapshot(ProjectState.Create(hostProject_For_1_1, ProjectEngineFactories.DefaultProvider));
        _snapshot_For_2_0 = new ProjectSnapshot(ProjectState.Create(hostProject_For_2_0, ProjectEngineFactories.DefaultProvider));
        _snapshot_For_2_1 = new ProjectSnapshot(ProjectState.Create(hostProject_For_2_1, ProjectEngineFactories.DefaultProvider));
        _snapshot_For_3_0 = new ProjectSnapshot(ProjectState.Create(hostProject_For_3_0, ProjectEngineFactories.DefaultProvider));
        _snapshot_For_UnknownConfiguration = new ProjectSnapshot(ProjectState.Create(hostProject_For_UnknownConfiguration, ProjectEngineFactories.DefaultProvider));

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
        var engine = factory.Create(
            snapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(snapshot.FilePath)),
            b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.GetFeatures<MyCoolNewFeature>());
        Assert.Single(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperProducer.Factory>());
        Assert.Single(engine.Engine.GetFeatures<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion2_1()
    {
        // Arrange
        var snapshot = _snapshot_For_2_1;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(
            snapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(snapshot.FilePath)),
            b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.GetFeatures<MyCoolNewFeature>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.MvcViewDocumentClassifierPass>());

        Assert.Single(engine.Engine.GetFeatures<Mvc2_X.ViewComponentTagHelperProducer.Factory>());
        Assert.Single(engine.Engine.GetFeatures<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesDesignTimeTemplateEngine_ForVersion2_0()
    {
        // Arrange
        var snapshot = _snapshot_For_2_0;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(
            snapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(snapshot.FilePath)),
            b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.GetFeatures<MyCoolNewFeature>());
        Assert.Single(engine.Engine.GetFeatures<Mvc2_X.ViewComponentTagHelperProducer.Factory>());
        Assert.Single(engine.Engine.GetFeatures<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_CreatesTemplateEngine_ForVersion1_1()
    {
        // Arrange
        var snapshot = _snapshot_For_1_1;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(
            snapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(snapshot.FilePath)),
            b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.GetFeatures<MyCoolNewFeature>());
        Assert.Single(engine.Engine.GetFeatures<Mvc1_X.ViewComponentTagHelperProducer.Factory>());
        Assert.Single(engine.Engine.GetFeatures<Mvc1_X.MvcViewDocumentClassifierPass>());
        Assert.Single(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_DoesNotSupportViewComponentTagHelpers_ForVersion1_0()
    {
        // Arrange
        var snapshot = _snapshot_For_1_0;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(
            snapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(snapshot.FilePath)),
            b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.GetFeatures<MyCoolNewFeature>());
        Assert.Single(engine.Engine.GetFeatures<Mvc1_X.MvcViewDocumentClassifierPass>());

        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperProducer.Factory>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());

        Assert.Empty(engine.Engine.GetFeatures<Mvc2_X.ViewComponentTagHelperProducer.Factory>());
        Assert.Empty(engine.Engine.GetFeatures<Mvc2_X.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());

        Assert.Empty(engine.Engine.GetFeatures<Mvc1_X.ViewComponentTagHelperProducer.Factory>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());
    }

    [Fact]
    public void Create_ForUnknownConfiguration_UsesFallbackFactory()
    {
        var snapshot = _snapshot_For_UnknownConfiguration;

        var provider = new ProjectEngineFactoryProvider(_customFactories);

        // Act
        var factory = provider.GetFactory(snapshot.Configuration);
        var engine = factory.Create(
            snapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(snapshot.FilePath)),
            b => b.Features.Add(new MyCoolNewFeature()));

        // Assert
        Assert.Single(engine.Engine.GetFeatures<MyCoolNewFeature>());
        Assert.Empty(engine.Engine.GetFeatures<DefaultTagHelperProducer.Factory>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperProducer.Factory>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.MvcViewDocumentClassifierPass>());
        Assert.Empty(engine.Engine.GetFeatures<MvcLatest.ViewComponentTagHelperPass>());
    }

    private class MyCoolNewFeature : RazorEngineFeatureBase
    {
    }
}
