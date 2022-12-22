// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

[DataContract]
internal class DelegatedCodeActionParams
{
    [DataMember(Name = "hostDocumentVersion")]
    public int HostDocumentVersion { get; set; }

    [DataMember(Name = "codeActionParams")]
    public required CodeActionParams CodeActionParams { get; set; }

    [DataMember(Name = "languageKind")]
    public RazorLanguageKind LanguageKind { get; set; }
}
