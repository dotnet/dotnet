// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;

internal class DelegatedColorPresentationParams : ColorPresentationParams
{
    [JsonPropertyName("_vs_requiredHostDocumentVersion")]
    public int RequiredHostDocumentVersion { get; set; }
}
