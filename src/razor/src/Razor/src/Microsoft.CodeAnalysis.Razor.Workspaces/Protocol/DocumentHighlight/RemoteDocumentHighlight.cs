// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using static Roslyn.LanguageServer.Protocol.RoslynLspExtensions;
using RoslynDocumentHighlight = Roslyn.LanguageServer.Protocol.DocumentHighlight;
using VsDocumentHighlight = Microsoft.VisualStudio.LanguageServer.Protocol.DocumentHighlight;

namespace Microsoft.CodeAnalysis.Razor.Protocol.DocumentHighlight;

[DataContract]
internal readonly record struct RemoteDocumentHighlight(
    [property: DataMember(Order = 0)] LinePositionSpan Span,
    [property: DataMember(Order = 1)] DocumentHighlightKind Kind)
{
    public static RemoteDocumentHighlight FromRoslynDocumentHighlight(RoslynDocumentHighlight highlight)
        => new(highlight.Range.ToLinePositionSpan(), (DocumentHighlightKind)highlight.Kind);

    public static VsDocumentHighlight ToVsDocumentHighlight(RemoteDocumentHighlight highlight)
        => new()
        {
            Range = VsLspExtensions.ToRange(highlight.Span),
            Kind = highlight.Kind
        };
}
