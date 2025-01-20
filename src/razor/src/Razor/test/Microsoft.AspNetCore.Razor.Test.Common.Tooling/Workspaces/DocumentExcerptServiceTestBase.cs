// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;
using TestFileMarkupParser = Microsoft.CodeAnalysis.Testing.TestFileMarkupParser;

namespace Microsoft.AspNetCore.Razor.Test.Common.Workspaces;

public abstract class DocumentExcerptServiceTestBase(ITestOutputHelper testOutput) : WorkspaceTestBase(testOutput)
{
    private readonly HostProject _hostProject = TestProjectData.SomeProject;
    private readonly HostDocument _hostDocument = TestProjectData.SomeProjectFile1;

    public static (SourceText sourceText, TextSpan span) CreateText(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        // Since we're using positions, normalize to Windows style
#pragma warning disable CA1307 // Specify StringComparison
        text = text.Replace("\r", "").Replace("\n", "\r\n");
#pragma warning restore CA1307 // Specify StringComparison

        TestFileMarkupParser.GetSpan(text, out text, out var span);
        return (SourceText.From(text), span);
    }

    // Adds the text to a ProjectSnapshot, generates code, and updates the workspace.
    private (IDocumentSnapshot primary, Document secondary) InitializeDocument(SourceText sourceText)
    {
        var state = ProjectState
            .Create(_hostProject, LanguageServerFeatureOptions.ToCompilerOptions(), ProjectEngineFactoryProvider)
            .AddDocument(_hostDocument, sourceText);

        var project = new ProjectSnapshot(state);

        var primary = project.GetRequiredDocument(_hostDocument.FilePath);

        var solution = Workspace.CurrentSolution.AddProject(ProjectInfo.Create(
            ProjectId.CreateNewId(Path.GetFileNameWithoutExtension(_hostDocument.FilePath)),
            VersionStamp.Create(),
            Path.GetFileNameWithoutExtension(_hostDocument.FilePath),
            Path.GetFileNameWithoutExtension(_hostDocument.FilePath),
            LanguageNames.CSharp,
            _hostDocument.FilePath));

        solution = solution.AddDocument(
            DocumentId.CreateNewId(solution.ProjectIds.Single(), _hostDocument.FilePath),
            _hostDocument.FilePath,
            new GeneratedDocumentTextLoader(primary, _hostDocument.FilePath));

        var secondary = solution.Projects.Single().Documents.Single();

        return (primary, secondary);
    }

    // Maps a span in the primary buffer to the secondary buffer. This is only valid for C# code
    // that appears in the primary buffer.
    private static async Task<TextSpan> GetSecondarySpanAsync(IDocumentSnapshot primary, TextSpan primarySpan, Document secondary, CancellationToken cancellationToken)
    {
        var output = await primary.GetGeneratedOutputAsync(cancellationToken);

        foreach (var mapping in output.GetCSharpDocument().SourceMappings)
        {
            if (mapping.OriginalSpan.AbsoluteIndex <= primarySpan.Start &&
                (mapping.OriginalSpan.AbsoluteIndex + mapping.OriginalSpan.Length) >= primarySpan.End)
            {
                var offset = mapping.GeneratedSpan.AbsoluteIndex - mapping.OriginalSpan.AbsoluteIndex;
                var secondarySpan = new TextSpan(primarySpan.Start + offset, primarySpan.Length);
                Assert.Equal(
                    (await primary.GetTextAsync(cancellationToken)).ToString(primarySpan),
                    (await secondary.GetTextAsync(cancellationToken)).ToString(secondarySpan));
                return secondarySpan;
            }
        }

        throw new InvalidOperationException("Could not map the primary span to the generated code.");
    }

    public async Task<(Document generatedDocument, SourceText razorSourceText, TextSpan primarySpan, TextSpan generatedSpan)> InitializeAsync(string razorSource, CancellationToken cancellationToken)
    {
        var (razorSourceText, primarySpan) = CreateText(razorSource);
        var (primary, generatedDocument) = InitializeDocument(razorSourceText);
        var generatedSpan = await GetSecondarySpanAsync(primary, primarySpan, generatedDocument, cancellationToken);
        return (generatedDocument, razorSourceText, primarySpan, generatedSpan);
    }

    internal async Task<(IDocumentSnapshot primary, Document generatedDocument, TextSpan generatedSpan)> InitializeWithSnapshotAsync(string razorSource, CancellationToken cancellationToken)
    {
        var (razorSourceText, primarySpan) = CreateText(razorSource);
        var (primary, generatedDocument) = InitializeDocument(razorSourceText);
        var generatedSpan = await GetSecondarySpanAsync(primary, primarySpan, generatedDocument, cancellationToken);
        return (primary, generatedDocument, generatedSpan);
    }
}
