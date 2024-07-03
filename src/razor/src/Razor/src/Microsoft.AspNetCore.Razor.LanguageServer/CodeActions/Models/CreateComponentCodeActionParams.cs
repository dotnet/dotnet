// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class CreateComponentCodeActionParams
{
    [JsonPropertyName("uri")]
    public required Uri Uri { get; set; }
    [JsonPropertyName("path")]
    public required string Path { get; set; }
}
