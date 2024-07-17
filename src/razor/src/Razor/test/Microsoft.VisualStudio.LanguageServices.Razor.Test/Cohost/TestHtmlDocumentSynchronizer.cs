// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed class TestHtmlDocumentSynchronizer : IHtmlDocumentSynchronizer
{
    public static TestHtmlDocumentSynchronizer Instance = new();

    public Task<HtmlDocumentResult?> TryGetSynchronizedHtmlDocumentAsync(TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var filePath = razorDocument.FilePath + ".g.html";
        return Task.FromResult<HtmlDocumentResult?>(new HtmlDocumentResult(new System.Uri(filePath), VsMocks.CreateTextBuffer(core: false)));
    }

    public Task<bool> TrySynchronizeAsync(TextDocument document, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
