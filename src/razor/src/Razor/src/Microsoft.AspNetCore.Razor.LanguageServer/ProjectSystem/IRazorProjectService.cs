// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal interface IRazorProjectService
{
    Task AddDocumentsToMiscProjectAsync(ImmutableArray<string> filePaths, CancellationToken cancellationToken);
    Task AddDocumentToMiscProjectAsync(string filePath, CancellationToken cancellationToken);
    Task OpenDocumentAsync(string filePath, SourceText sourceText, CancellationToken cancellationToken);
    Task UpdateDocumentAsync(string filePath, SourceText sourceText, CancellationToken cancellationToken);
    Task CloseDocumentAsync(string filePath, CancellationToken cancellationToken);
    Task RemoveDocumentAsync(string filePath, CancellationToken cancellationToken);
}
