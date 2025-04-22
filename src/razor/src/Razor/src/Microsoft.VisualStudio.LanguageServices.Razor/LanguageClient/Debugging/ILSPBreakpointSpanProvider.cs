// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

internal interface ILSPBreakpointSpanProvider
{
    Task<LspRange?> GetBreakpointSpanAsync(LSPDocumentSnapshot documentSnapshot, long hostDocumentSyncVersion, Position position, CancellationToken cancellationToken);
}
