// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal interface IDelegatedCodeActionsProvider
{
    Task<RazorVSInternalCodeAction[]> GetDelegatedCodeActionsAsync(RazorLanguageKind languageKind, VSCodeActionParams request, int hostDocumentVersion, Guid correlationId, CancellationToken cancellationToken);
}
