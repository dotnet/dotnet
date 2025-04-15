// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

internal class RazorLanguageQueryParams
{
    [JsonPropertyName("uri")]
    public required Uri Uri { get; set; }

    [JsonPropertyName("position")]
    public required Position Position { get; set; }
}
