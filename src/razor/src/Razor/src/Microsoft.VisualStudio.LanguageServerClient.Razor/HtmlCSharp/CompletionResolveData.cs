// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal class CompletionResolveData
{
    public required long ResultId { get; init; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public required object OriginalData { get; init; }
}
