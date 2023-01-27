// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

[DataContract]
internal class DelegatedColorPresentationParams : ColorPresentationParams
{
    [DataMember(Name = "_vs_requiredHostDocumentVersion")]
    public int RequiredHostDocumentVersion { get; set; }
}
