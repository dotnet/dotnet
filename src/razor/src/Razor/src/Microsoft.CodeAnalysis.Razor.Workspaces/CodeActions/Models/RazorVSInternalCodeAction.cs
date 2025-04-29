// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

[DataContract]
internal sealed class RazorVSInternalCodeAction : VSInternalCodeAction
{
    [JsonPropertyName("name")]
    [DataMember(Name = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// The order code actions should appear. This is not serialized as its just used in the code actions service
    /// </summary>
    [JsonIgnore]
    public int Order { get; set; }
}
