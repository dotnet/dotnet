// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

[JsonObject]
internal class RazorCodeActionResolutionParams
{
    [JsonProperty(PropertyName = "action", Required = Required.Always)]
    public required string Action { get; set; }

    [JsonProperty(PropertyName = "language", Required = Required.Always)]
    public required string Language { get; set; }

    [JsonProperty(PropertyName = "data", Required = Required.Always)]
    public required object Data { get; set; }
}
