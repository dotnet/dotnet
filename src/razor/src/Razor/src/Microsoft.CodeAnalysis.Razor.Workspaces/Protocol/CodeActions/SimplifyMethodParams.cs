// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

[DataContract]
internal record SimplifyMethodParams : ITextDocumentParams
{
    [DataMember(Name = "textDocument")]
    [JsonPropertyName("textDocument")]
    public required TextDocumentIdentifier TextDocument { get; set; }

    [DataMember(Name = "textEdit")]
    [JsonPropertyName("textEdit")]
    public required TextEdit TextEdit { get; set; }
}
