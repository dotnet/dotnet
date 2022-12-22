// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class RazorDocumentOnTypeFormattingParams : DocumentOnTypeFormattingParams
{
    [DataMember(Name = "hostDocumentVersion")]
    public int HostDocumentVersion { get; set; }
}
