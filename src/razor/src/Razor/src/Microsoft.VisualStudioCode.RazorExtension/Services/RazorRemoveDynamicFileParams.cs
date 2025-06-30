// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal class RazorRemoveDynamicFileParams
{
    [JsonPropertyName("csharpDocument")]
    public required TextDocumentIdentifier CSharpDocument { get; set; }
}
