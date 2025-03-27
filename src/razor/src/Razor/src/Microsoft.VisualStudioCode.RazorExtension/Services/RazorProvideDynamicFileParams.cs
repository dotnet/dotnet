// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorProvideDynamicFileParams
{
    [JsonPropertyName("razorDocument")]
    public required TextDocumentIdentifier RazorDocument { get; set; }

    /// <summary>
    /// When true, the full text of the document will be sent over as a single
    /// edit instead of diff edits
    /// </summary>
    [JsonPropertyName("fullText")]
    public bool FullText { get; set; }
}
