// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

[DataContract]
internal record FormatNewFileParams
{
    [DataMember(Name = "document")]
    [JsonPropertyName("document")]
    public required TextDocumentIdentifier Document { get; set; }

    [DataMember(Name = "project")]
    [JsonPropertyName("project")]
    public required TextDocumentIdentifier Project { get; set; }

    [DataMember(Name = "contents")]
    [JsonPropertyName("contents")]
    public required string Contents { get; set; }
}
