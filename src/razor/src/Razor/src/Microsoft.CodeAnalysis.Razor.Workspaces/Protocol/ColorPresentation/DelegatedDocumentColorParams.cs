// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;

[DataContract]
internal class DelegatedDocumentColorParams : DocumentColorParams
{
    [DataMember(Name = "_razor_hostDocumentVersion")]
    public int HostDocumentVersion { get; set; }
}
