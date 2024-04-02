// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

[DataContract]
internal class DelegatedCodeActionParams
{
    [DataMember(Name = "hostDocumentVersion")]
    public int HostDocumentVersion { get; set; }

    [DataMember(Name = "codeActionParams")]
    public required VSCodeActionParams CodeActionParams { get; set; }

    [DataMember(Name = "languageKind")]
    public RazorLanguageKind LanguageKind { get; set; }

    [DataMember(Name = "correlationId")]
    public Guid CorrelationId { get; set; }
}
