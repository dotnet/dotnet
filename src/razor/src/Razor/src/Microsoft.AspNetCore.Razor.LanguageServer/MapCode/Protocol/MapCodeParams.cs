// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

/// <summary>
/// LSP Params for textDocument/mapCode calls.
/// </summary>
[DataContract]
internal class MapCodeParams
{
    /// <summary>
    /// Set of code blocks, associated with documents and regions, to map.
    /// </summary>
    [DataMember(Name = "mappings")]
    public required MapCodeMapping[] Mappings { get; set; }

    /// <summary>
    /// Changes that should be applied to the workspace by the mapper before performing
    /// the mapping operation.
    /// </summary>
    [DataMember(Name = "updates")]
    public WorkspaceEdit? Updates
    {
        get;
        set;
    }
}
