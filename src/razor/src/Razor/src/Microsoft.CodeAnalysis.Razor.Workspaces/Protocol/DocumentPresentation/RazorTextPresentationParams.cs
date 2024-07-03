// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;

/// <summary>
/// Class representing the parameters sent for a textDocument/_vs_textPresentation request, plus
/// a host document version.
/// </summary>
internal class RazorTextPresentationParams : TextPresentationParams, IRazorPresentationParams
{
    [JsonPropertyName("kind")]
    public RazorLanguageKind Kind { get; set; }

    [JsonPropertyName("hostDocumentVersion")]
    public int HostDocumentVersion { get; set; }
}
