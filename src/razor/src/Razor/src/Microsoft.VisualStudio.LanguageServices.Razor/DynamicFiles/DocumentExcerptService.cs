// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal abstract class DocumentExcerptService : IRazorDocumentExcerptServiceImplementation
{
    Task<RazorExcerptResult?> IRazorDocumentExcerptServiceImplementation.TryExcerptAsync(
        Document document,
        TextSpan span,
        RazorExcerptMode mode,
        RazorClassificationOptionsWrapper options,
        CancellationToken cancellationToken)
    {
        return TryGetExcerptInternalAsync(document, span, mode, options, cancellationToken);
    }

    internal abstract Task<RazorExcerptResult?> TryGetExcerptInternalAsync(
        Document document,
        TextSpan span,
        RazorExcerptMode mode,
        RazorClassificationOptionsWrapper options,
        CancellationToken cancellationToken);
}
