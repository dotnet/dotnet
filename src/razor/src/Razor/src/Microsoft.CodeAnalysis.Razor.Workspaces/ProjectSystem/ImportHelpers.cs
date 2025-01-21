// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class ImportHelpers
{
    public static async Task<ImmutableArray<ImportItem>> GetImportItemsAsync(IDocumentSnapshot document, RazorProjectEngine projectEngine, CancellationToken cancellationToken)
    {
        var projectItem = projectEngine.FileSystem.GetItem(document.FilePath, document.FileKind);

        using var importProjectItems = new PooledArrayBuilder<RazorProjectItem>();

        foreach (var feature in projectEngine.GetFeatures<IImportProjectFeature>())
        {
            if (feature.GetImports(projectItem) is { } featureImports)
            {
                importProjectItems.AddRange(featureImports);
            }
        }

        if (importProjectItems.Count == 0)
        {
            return [];
        }

        var project = document.Project;

        using var importItems = new PooledArrayBuilder<ImportItem>(capacity: importProjectItems.Count);

        foreach (var importProjectItem in importProjectItems)
        {
            if (importProjectItem is NotFoundProjectItem)
            {
                continue;
            }

            if (importProjectItem.PhysicalPath is null)
            {
                // This is a default import.
                using var stream = importProjectItem.Read();
                var text = SourceText.From(stream);
                var defaultImport = ImportItem.CreateDefault(text);

                importItems.Add(defaultImport);
            }
            else if (project.TryGetDocument(importProjectItem.PhysicalPath, out var importDocument))
            {
                var text = await importDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
                var versionStamp = await importDocument.GetTextVersionAsync(cancellationToken).ConfigureAwait(false);
                var importItem = new ImportItem(importDocument.FilePath, importDocument.FileKind, text, versionStamp);

                importItems.Add(importItem);
            }
        }

        return importItems.DrainToImmutable();
    }

    public static ImmutableArray<RazorSourceDocument> GetImportSources(ImmutableArray<ImportItem> importItems, RazorProjectEngine projectEngine)
    {
        using var importSources = new PooledArrayBuilder<RazorSourceDocument>(importItems.Length);

        foreach (var importItem in importItems)
        {
            var importProjectItem = importItem is { FilePath: string filePath, FileKind: var fileKind }
                ? projectEngine.FileSystem.GetItem(filePath, fileKind)
                : null;

            var properties = RazorSourceDocumentProperties.Create(importItem.FilePath, importProjectItem?.RelativePhysicalPath);
            var importSource = RazorSourceDocument.Create(importItem.Text, properties);

            importSources.Add(importSource);
        }

        return importSources.DrainToImmutable();
    }

    public static async Task<RazorSourceDocument> GetSourceAsync(IDocumentSnapshot document, RazorProjectEngine projectEngine, CancellationToken cancellationToken)
    {
        var projectItem = document is { FilePath: string filePath, FileKind: var fileKind }
            ? projectEngine.FileSystem.GetItem(filePath, fileKind)
            : null;

        var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
        var properties = RazorSourceDocumentProperties.Create(document.FilePath, projectItem?.RelativePhysicalPath);
        return RazorSourceDocument.Create(text, properties);
    }
}
