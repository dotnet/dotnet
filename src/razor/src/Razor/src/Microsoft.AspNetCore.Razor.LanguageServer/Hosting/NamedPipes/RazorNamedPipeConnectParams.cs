// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.NamedPipes;

internal class RazorNamedPipeConnectParams
{
    [JsonPropertyName("pipeName")]
    public required string PipeName { get; set; }
}
