// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorMapTextChangesParams
{
    [JsonPropertyName("csharpDocument")]
    public required TextDocumentIdentifier CSharpDocument { get; set; }

    [JsonPropertyName("textChanges")]
    public required RazorTextChange[] TextChanges { get; set; }
}
