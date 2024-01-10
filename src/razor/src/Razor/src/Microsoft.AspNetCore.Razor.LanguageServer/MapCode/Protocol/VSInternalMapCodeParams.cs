// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

/// <summary>
/// LSP Params for textDocument/mapCode calls.
/// </summary>
[DataContract]
internal class VSInternalMapCodeParams
{
    /// <summary>
    /// Set of code blocks, associated with documents and regions, to map.
    /// </summary>
    [DataMember(Name = "_vs_mappings")]
    public required VSInternalMapCodeMapping[] Mappings
    {
        get;
        set;
    }

    /// <summary>
    /// Changes that should be applied to the workspace by the mapper before performing
    /// the mapping operation.
    /// </summary>
    [DataMember(Name = "_vs_updates")]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public WorkspaceEdit? Updates
    {
        get;
        set;
    }
}
