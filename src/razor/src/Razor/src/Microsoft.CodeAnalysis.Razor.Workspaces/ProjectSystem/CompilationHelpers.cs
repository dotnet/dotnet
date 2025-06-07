// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class CompilationHelpers
{
    internal static async Task<RazorCodeDocument> GenerateCodeDocumentAsync(
        IDocumentSnapshot document,
        RazorProjectEngine projectEngine,
        RazorCompilerOptions compilerOptions,
        CancellationToken cancellationToken)
    {
        var importSources = await GetImportSourcesAsync(document, projectEngine, cancellationToken).ConfigureAwait(false);
        var tagHelpers = await document.Project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        var source = await document.GetSourceAsync(cancellationToken).ConfigureAwait(false);

        var generator = new CodeDocumentGenerator(projectEngine, compilerOptions);
        return generator.Generate(source, document.FileKind, importSources, tagHelpers, cancellationToken);
    }

    internal static async Task<RazorCodeDocument> GenerateDesignTimeCodeDocumentAsync(
        IDocumentSnapshot document,
        RazorProjectEngine projectEngine,
        CancellationToken cancellationToken)
    {
        var importSources = await GetImportSourcesAsync(document, projectEngine, cancellationToken).ConfigureAwait(false);
        var tagHelpers = await document.Project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        var source = await document.GetSourceAsync(cancellationToken).ConfigureAwait(false);

        var generator = new CodeDocumentGenerator(projectEngine, RazorCompilerOptions.None);
        return generator.GenerateDesignTime(source, document.FileKind, importSources, tagHelpers, cancellationToken);
    }

    internal static async Task<ImmutableArray<RazorSourceDocument>> GetImportSourcesAsync(IDocumentSnapshot document, RazorProjectEngine projectEngine, CancellationToken cancellationToken)
    {
        // We don't use document.FilePath when calling into GetItem(...) because
        // it could be rooted outside of the project root. document.TargetPath should
        // represent the logical relative path within the project root.
        var projectItem = projectEngine.FileSystem.GetItem(document.TargetPath, document.FileKind);

        using var importProjectItems = new PooledArrayBuilder<RazorProjectItem>();
        projectEngine.CollectImports(projectItem, ref importProjectItems.AsRef());

        if (importProjectItems.Count == 0)
        {
            return [];
        }

        var project = document.Project;

        using var importSources = new PooledArrayBuilder<RazorSourceDocument>(capacity: importProjectItems.Count);

        foreach (var importProjectItem in importProjectItems)
        {
            if (importProjectItem is NotFoundProjectItem)
            {
                continue;
            }

            if (importProjectItem is DefaultImportProjectItem)
            {
                var importSource = importProjectItem.GetSource()
                    .AssumeNotNull($"Encountered a default import with a missing {nameof(RazorSourceDocument)}: {importProjectItem.FilePath}.");

                importSources.Add(importSource);
            }
            else if (project.TryGetDocument(importProjectItem.PhysicalPath, out var importDocument))
            {
                var importSource = await importDocument.GetSourceAsync(cancellationToken).ConfigureAwait(false);

                importSources.Add(importSource);
            }
        }

        return importSources.ToImmutableAndClear();
    }
}
