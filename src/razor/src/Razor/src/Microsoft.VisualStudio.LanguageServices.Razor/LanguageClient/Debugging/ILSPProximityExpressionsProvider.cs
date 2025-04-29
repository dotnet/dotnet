// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

internal interface ILSPProximityExpressionsProvider
{
    Task<IReadOnlyList<string>?> GetProximityExpressionsAsync(LSPDocumentSnapshot documentSnapshot, long hostDocumentSyncVersion, Position position, CancellationToken cancellationToken);
}
