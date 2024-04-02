// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;

[DataContract]
internal class SemanticTokensRangesParams : SemanticTokensParams
{
    [DataMember(Name = "ranges")]
    public required Range[] Ranges { get; set; }
}
