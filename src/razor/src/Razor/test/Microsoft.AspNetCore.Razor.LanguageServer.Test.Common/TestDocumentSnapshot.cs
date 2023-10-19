// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal class TestDocumentSnapshot : DocumentSnapshot
{
    private RazorCodeDocument? _codeDocument;

    public static TestDocumentSnapshot Create(string filePath)
        => Create(filePath, string.Empty);

    public static TestDocumentSnapshot Create(string filePath, VersionStamp version)
        => Create(filePath, string.Empty, version);

    public static TestDocumentSnapshot Create(string filePath, string text)
        => Create(filePath, text, VersionStamp.Default);

    public static TestDocumentSnapshot Create(string filePath, string text, VersionStamp version, ProjectWorkspaceState? projectWorkspaceState = null)
        => Create(filePath, text, version, TestProjectSnapshot.Create(filePath + ".csproj", projectWorkspaceState));

    public static TestDocumentSnapshot Create(string filePath, string text, VersionStamp version, TestProjectSnapshot projectSnapshot)
    {
        using var testWorkspace = TestWorkspace.Create();
        var hostDocument = new HostDocument(filePath, filePath);
        var sourceText = SourceText.From(text);
        var documentState = new DocumentState(
            testWorkspace.Services,
            hostDocument,
            SourceText.From(text),
            version,
            () => Task.FromResult(TextAndVersion.Create(sourceText, version)));
        var testDocument = new TestDocumentSnapshot(projectSnapshot, documentState);

        return testDocument;
    }

    internal static TestDocumentSnapshot Create(Workspace workspace, ProjectSnapshot projectSnapshot, string filePath, string text = "", VersionStamp? version = null)
    {
        version ??= VersionStamp.Default;

        var targetPath = FilePathNormalizer.Normalize(filePath);
        var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(projectSnapshot.FilePath);
        if (targetPath.StartsWith(projectDirectory))
        {
            targetPath = targetPath[projectDirectory.Length..];
        }

        var hostDocument = new HostDocument(filePath, targetPath);
        var sourceText = SourceText.From(text);
        var documentState = new DocumentState(
            workspace.Services,
            hostDocument,
            SourceText.From(text),
            version,
            () => Task.FromResult(TextAndVersion.Create(sourceText, version.Value)));
        var testDocument = new TestDocumentSnapshot(projectSnapshot, documentState);

        return testDocument;
    }

    public TestDocumentSnapshot(ProjectSnapshot projectSnapshot, DocumentState documentState)
        : base(projectSnapshot, documentState)
    {
    }

    public HostDocument HostDocument => State.HostDocument;

    public override Task<RazorCodeDocument> GetGeneratedOutputAsync()
    {
        if (_codeDocument is null)
        {
            throw new ArgumentNullException(nameof(_codeDocument));
        }

        return Task.FromResult(_codeDocument);
    }

    public override ImmutableArray<IDocumentSnapshot> GetImports()
    {
        return ImmutableArray<IDocumentSnapshot>.Empty;
    }

    public override bool TryGetGeneratedOutput(out RazorCodeDocument result)
    {
        if (_codeDocument is null)
        {
            throw new InvalidOperationException($"You must call {nameof(With)} to set the code document for this document snapshot.");
        }

        result = _codeDocument;
        return true;
    }

    public TestDocumentSnapshot With(RazorCodeDocument codeDocument)
    {
        if (codeDocument is null)
        {
            throw new ArgumentNullException(nameof(codeDocument));
        }

        _codeDocument = codeDocument;
        return this;
    }
}
