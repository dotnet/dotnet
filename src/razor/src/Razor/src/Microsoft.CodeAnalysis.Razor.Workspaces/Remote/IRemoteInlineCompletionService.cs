// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteInlineCompletionService
{
    ValueTask<InlineCompletionRequestInfo?> GetInlineCompletionInfoAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition position,
        CancellationToken cancellationToken);

    ValueTask<FormattedInlineCompletionInfo?> FormatInlineCompletionAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        RazorFormattingOptions options,
        LinePositionSpan span,
        string text,
        CancellationToken cancellationToken);
}

[DataContract]
internal record struct InlineCompletionRequestInfo(
    [property: DataMember(Order = 0)] Uri GeneratedDocumentUri,
    [property: DataMember(Order = 1)] LinePosition Position);

[DataContract]
internal record struct FormattedInlineCompletionInfo(
    [property: DataMember(Order = 0)] LinePositionSpan Span,
    [property: DataMember(Order = 1)] string FormattedText);
