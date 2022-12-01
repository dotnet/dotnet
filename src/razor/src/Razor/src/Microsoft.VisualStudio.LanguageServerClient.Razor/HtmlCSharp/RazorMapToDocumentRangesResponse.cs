// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

// Note: This type should be kept in sync with the one in Razor.LanguageServer assembly.
[DataContract]
internal class RazorMapToDocumentRangesResponse
{
    [DataMember(Name = "ranges")]
    public required Range[] Ranges { get; init; }

    [DataMember(Name = "hostDocumentVersion")]
    public int? HostDocumentVersion { get; init; }
}
