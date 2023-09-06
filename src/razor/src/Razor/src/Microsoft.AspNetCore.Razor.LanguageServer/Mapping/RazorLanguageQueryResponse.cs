// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

[DataContract]
internal class RazorLanguageQueryResponse
{
    [DataMember(Name = "kind")]
    public RazorLanguageKind Kind { get; set; }

    [DataMember(Name = "positionIndex")]
    public int PositionIndex { get; set; }

    [DataMember(Name = "position")]
    public required Position Position { get; set; }

    [DataMember(Name = "hostDocumentVersion")]
    public int? HostDocumentVersion { get; set; }
}
