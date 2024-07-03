// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Debugging;

internal class RazorProximityExpressionsResponse
{
    [JsonPropertyName("expressions")]
    public required IReadOnlyList<string> Expressions { get; init; }
}
