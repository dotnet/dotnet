// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
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
