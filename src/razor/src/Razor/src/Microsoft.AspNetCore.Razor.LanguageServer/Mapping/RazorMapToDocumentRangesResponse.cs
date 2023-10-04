// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

// NOTE: Changes here MUST be copied over to
// Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp.RazorMapToDocumentRangesResponse
[DataContract]
internal class RazorMapToDocumentRangesResponse
{
    [DataMember(Name = "ranges")]
    public required Range[] Ranges { get; init; }

    [DataMember(Name = "hostDocumentVersion")]
    public int? HostDocumentVersion { get; init; }
}
