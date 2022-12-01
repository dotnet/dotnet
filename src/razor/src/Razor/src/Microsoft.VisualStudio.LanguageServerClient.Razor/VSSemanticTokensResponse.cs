// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

/// <summary>
/// Language servers such as Roslyn support multiple colorization passes and and
/// may only send us back inaccurate/incomplete tokens until the full token set is
/// available. Razor needs to know if the tokens are not finalized so we can continue
/// to queue language servers for tokens.
/// </summary>
internal class VSSemanticTokensResponse : SemanticTokens
{
    [DataMember(Name = "isFinalized")]
    public bool IsFinalized { get; set; }
}
