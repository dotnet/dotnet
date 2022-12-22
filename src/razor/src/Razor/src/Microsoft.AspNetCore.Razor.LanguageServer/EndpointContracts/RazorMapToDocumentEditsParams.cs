// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

// Note: This type should be kept in sync with the one in Razor.HtmlCSharp assembly.
[DataContract]
internal class RazorMapToDocumentEditsParams
{
    [DataMember(Name = "kind")]
    public RazorLanguageKind Kind { get; init; }

    [DataMember(Name = "razorDocumentUri")]
    public required Uri RazorDocumentUri { get; init; }

    [DataMember(Name = "projectedTextEdits")]
    public required TextEdit[] ProjectedTextEdits { get; init; }

    [DataMember(Name = "textEditKind")]
    public TextEditKind TextEditKind { get; init; }

    [DataMember(Name = "formattingOptions")]
    public required FormattingOptions FormattingOptions { get; init; }
}
