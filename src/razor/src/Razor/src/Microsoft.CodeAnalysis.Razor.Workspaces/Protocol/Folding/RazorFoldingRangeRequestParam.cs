// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Folding;

internal class RazorFoldingRangeRequestParam : FoldingRangeParams
{
    [JsonPropertyName("hostDocumentVersion")]
    public int HostDocumentVersion { get; init; }
}
