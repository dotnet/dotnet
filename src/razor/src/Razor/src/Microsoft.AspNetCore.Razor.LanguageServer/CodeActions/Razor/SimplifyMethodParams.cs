// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;

//
// Summary:
//     Class representing the parameters sent from the client to the server for the
//     roslyn/simplifyMethod request.
[DataContract]
internal record SimplifyMethodParams : ITextDocumentParams
{
    //
    // Summary:
    //     Gets or sets the value which identifies the document in which the edit will be applied.
    [DataMember(Name = "textDocument")]
    public required TextDocumentIdentifier TextDocument { get; set; }

    //
    // Summary:
    //     Gets or sets the value which is the text edit to be simplified.
    [DataMember(Name = "textEdit")]
    public required TextEdit TextEdit { get; set; }
}
