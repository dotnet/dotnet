// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;

[DataContract]
internal record FormatNewFileParams
{
    [DataMember(Name = "document")]
    public required TextDocumentIdentifier Document { get; set; }

    [DataMember(Name = "project")]
    public required TextDocumentIdentifier Project { get; set; }

    [DataMember(Name = "contents")]
    public required string Contents { get; set; }
}
