// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

internal class RazorFoldingRangeRequestParam : FoldingRangeParams
{
    [DataMember(Name = "hostDocumentVersion")]
    public int HostDocumentVersion { get; init; }
}
