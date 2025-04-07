// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Completion;

internal class RazorInlineCompletionRequest : VSInternalInlineCompletionRequest
{
    [JsonPropertyName("razorLanguageKind")]
    public RazorLanguageKind Kind { get; set; }
}
