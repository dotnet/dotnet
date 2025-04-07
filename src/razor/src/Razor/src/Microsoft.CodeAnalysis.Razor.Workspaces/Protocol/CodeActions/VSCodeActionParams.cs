// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

/// <summary>
/// We can't use the CodeActionParams defined in MS.VS.LS.Protocol, so need our own version, because the platform only
/// converts on read, not write. ie, if it gets a request for a CodeActionParams, it will happily deserialize the Context
/// property to VSInternalCodeActionContext, but in our case we need to send a request to our CustomMessageTarget, and so
/// we need the Context property serialized as the internal type.
/// </summary>
[DataContract]
internal class VSCodeActionParams
{
    [JsonPropertyName("textDocument")]
    [DataMember(Name = "textDocument")]
    public required VSTextDocumentIdentifier TextDocument { get; set; }

    [JsonPropertyName("range")]
    [DataMember(Name = "range")]
    public required LspRange Range { get; set; }

    [JsonPropertyName("context")]
    [DataMember(Name = "context")]
    public required VSInternalCodeActionContext Context { get; set; }
}
