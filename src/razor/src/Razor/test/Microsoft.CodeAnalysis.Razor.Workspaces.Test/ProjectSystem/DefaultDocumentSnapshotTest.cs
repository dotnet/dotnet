// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DefaultDocumentSnapshotTest : WorkspaceTestBase
{
    private readonly SourceText _sourceText;
    private readonly VersionStamp _version;
    private readonly HostDocument _componentHostDocument;
    private readonly HostDocument _componentCshtmlHostDocument;
    private readonly HostDocument _legacyHostDocument;
    private readonly DocumentSnapshot _componentDocument;
    private readonly DocumentSnapshot _componentCshtmlDocument;
    private readonly DocumentSnapshot _legacyDocument;
    private readonly HostDocument _nestedComponentHostDocument;
    private readonly DocumentSnapshot _nestedComponentDocument;

    public DefaultDocumentSnapshotTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _sourceText = SourceText.From("<p>Hello World</p>");
        _version = VersionStamp.Create();

        // Create a new HostDocument to avoid mutating the code container
        _componentCshtmlHostDocument = new HostDocument(TestProjectData.SomeProjectCshtmlComponentFile5);
        _componentHostDocument = new HostDocument(TestProjectData.SomeProjectComponentFile1);
        _legacyHostDocument = new HostDocument(TestProjectData.SomeProjectFile1);
        _nestedComponentHostDocument = new HostDocument(TestProjectData.SomeProjectNestedComponentFile3);

        var projectState = ProjectState.Create(ProjectEngineFactory, TestProjectData.SomeProject, ProjectWorkspaceState.Default);
        var project = new ProjectSnapshot(projectState);

        var textAndVersion = TextAndVersion.Create(_sourceText, _version);

        var documentState = DocumentState.Create(_legacyHostDocument, () => Task.FromResult(textAndVersion));
        _legacyDocument = new DocumentSnapshot(project, documentState);

        documentState = DocumentState.Create(_componentHostDocument, () => Task.FromResult(textAndVersion));
        _componentDocument = new DocumentSnapshot(project, documentState);

        documentState = DocumentState.Create(_componentCshtmlHostDocument, () => Task.FromResult(textAndVersion));
        _componentCshtmlDocument = new DocumentSnapshot(project, documentState);

        documentState = DocumentState.Create(_nestedComponentHostDocument, () => Task.FromResult(textAndVersion));
        _nestedComponentDocument = new DocumentSnapshot(project, documentState);
    }

    protected override void ConfigureWorkspaceServices(List<IWorkspaceService> services)
    {
        services.Add(new TestTagHelperResolver());
    }

    [Fact]
    public async Task GCCollect_OutputIsNoLongerCached()
    {
        // Arrange
        await Task.Run(async () => { await _legacyDocument.GetGeneratedOutputAsync(); });

        // Act

        // Forces collection of the cached document output
        GC.Collect();

        // Assert
        Assert.False(_legacyDocument.TryGetGeneratedOutput(out _));
    }

    [Fact]
    public async Task RegeneratingWithReference_CachesOutput()
    {
        // Arrange
        var output = await _legacyDocument.GetGeneratedOutputAsync();

        // Mostly doing this to ensure "var output" doesn't get optimized out
        Assert.NotNull(output);

        // Act & Assert
        Assert.True(_legacyDocument.TryGetGeneratedOutput(out _));
    }

    // This is a sanity test that we invoke component codegen for components.It's a little fragile but
    // necessary.

    [Fact]
    public async Task GetGeneratedOutputAsync_CshtmlComponent_ContainsComponentImports()
    {
        // Act
        var codeDocument = await _componentCshtmlDocument.GetGeneratedOutputAsync();

        // Assert
        Assert.Contains("using global::Microsoft.AspNetCore.Components", codeDocument.GetCSharpSourceText().ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetGeneratedOutputAsync_Component()
    {
        // Act
        var codeDocument = await _componentDocument.GetGeneratedOutputAsync();

        // Assert
        Assert.Contains("ComponentBase", codeDocument.GetCSharpSourceText().ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetGeneratedOutputAsync_NestedComponentDocument_SetsCorrectNamespaceAndClassName()
    {
        // Act
        var codeDocument = await _nestedComponentDocument.GetGeneratedOutputAsync();

        // Assert
        Assert.Contains("ComponentBase", codeDocument.GetCSharpSourceText().ToString(), StringComparison.Ordinal);
        Assert.Contains("namespace SomeProject.Nested", codeDocument.GetCSharpSourceText().ToString(), StringComparison.Ordinal);
        Assert.Contains("class File3", codeDocument.GetCSharpSourceText().ToString(), StringComparison.Ordinal);
    }

    // This is a sanity test that we invoke legacy codegen for .cshtml files. It's a little fragile but
    // necessary.
    [Fact]
    public async Task GetGeneratedOutputAsync_Legacy()
    {
        // Act
        var codeDocument = await _legacyDocument.GetGeneratedOutputAsync();

        // Assert
        Assert.Contains("Template", codeDocument.GetCSharpSourceText().ToString(), StringComparison.Ordinal);
    }
}
