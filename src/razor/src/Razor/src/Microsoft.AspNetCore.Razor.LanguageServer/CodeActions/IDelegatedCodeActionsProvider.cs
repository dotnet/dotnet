// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
