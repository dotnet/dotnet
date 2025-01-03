// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

internal sealed class GenerateMethodCodeActionParams
{
    [JsonPropertyName("methodName")]
    public required string MethodName { get; set; }

    [JsonPropertyName("eventParameterType")]
    public string? EventParameterType { get; set; }

    [JsonPropertyName("isAsync")]
    public required bool IsAsync { get; set; }
}
