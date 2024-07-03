// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal class GenerateMethodCodeActionParams
{
    [JsonPropertyName("uri")]
    public required Uri Uri { get; set; }

    [JsonPropertyName("methodName")]
    public required string MethodName { get; set; }

    [JsonPropertyName("eventName")]
    public required string EventName { get; set; }

    [JsonPropertyName("isAsync")]
    public required bool IsAsync { get; set; }
}
