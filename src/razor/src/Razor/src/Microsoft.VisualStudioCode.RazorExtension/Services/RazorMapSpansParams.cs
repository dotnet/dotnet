// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorMapSpansParams
{
    [JsonPropertyName("csharpDocument")]
    public required TextDocumentIdentifier CSharpDocument { get; set; }

    [JsonPropertyName("ranges")]
    public required LspRange[] Ranges { get; set; }
}
