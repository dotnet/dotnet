// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;

// VS doesn't support textDocument/colorPresentation but VSCode does. This class is a workaround until VS adds support.
[DataContract]
internal class ColorPresentationParams
{
    [DataMember(Name = "textDocument")]
    public TextDocumentIdentifier TextDocument { get; set; }

    [DataMember(Name = "color")]
    public Color Color { get; set; }

    [DataMember(Name = "range")]
    public Range Range { get; set; }
}
