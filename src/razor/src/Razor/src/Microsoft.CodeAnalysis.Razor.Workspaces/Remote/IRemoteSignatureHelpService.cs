// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Roslyn.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Remote;

using SignatureHelp = Roslyn.LanguageServer.Protocol.SignatureHelp;

internal interface IRemoteSignatureHelpService : IRemoteJsonService
{
    ValueTask<SignatureHelp?> GetSignatureHelpAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId documentId, Position linePosition, CancellationToken cancellationToken);
}
