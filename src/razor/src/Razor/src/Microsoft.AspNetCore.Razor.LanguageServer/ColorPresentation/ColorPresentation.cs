// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;

// VS doesn't support textDocument/colorPresentation but VSCode does. This class is a workaround until VS adds support.
internal class ColorPresentation
{
    [DataMember(Name = "label")]
    public string Label { get; set; }

    [DataMember(Name = "textEdit")]
    public TextEdit TextEdit { get; set; }

    [DataMember(Name = "additionalTextEdits")]
    public TextEdit[] AdditionalTextEdits { get; set; }
}
