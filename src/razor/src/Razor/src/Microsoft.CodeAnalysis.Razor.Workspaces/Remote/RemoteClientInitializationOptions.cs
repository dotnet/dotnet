// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal struct RemoteClientInitializationOptions
{
    [JsonPropertyName("useRazorCohostServer")]
    public required bool UseRazorCohostServer { get; set; }

    [JsonPropertyName("usePreciseSemanticTokenRanges")]
    public required bool UsePreciseSemanticTokenRanges { get; set; }

    [JsonPropertyName("csharpVirtualDocumentSuffix")]
    public required string CSharpVirtualDocumentSuffix { get; set; }

    [JsonPropertyName("htmlVirtualDocumentSuffix")]
    public required string HtmlVirtualDocumentSuffix { get; set; }

    [JsonPropertyName("includeProjectKeyInGeneratedFilePath")]
    public required bool IncludeProjectKeyInGeneratedFilePath { get; set; }

    [JsonPropertyName("returnCodeActionAndRenamePathsWithPrefixedSlash")]
    public required bool ReturnCodeActionAndRenamePathsWithPrefixedSlash { get; set; }

    [JsonPropertyName("forceRuntimeCodeGeneration")]
    public required bool ForceRuntimeCodeGeneration { get; set; }
}
