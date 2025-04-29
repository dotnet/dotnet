// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;

// VS doesn't support textDocument/colorPresentation but VSCode does. This class is a workaround until VS adds support.
internal sealed class ColorPresentation
{
    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("textEdit")]
    public TextEdit TextEdit { get; set; }

    [JsonPropertyName("additionalTextEdits")]
    public TextEdit[] AdditionalTextEdits { get; set; }
}
