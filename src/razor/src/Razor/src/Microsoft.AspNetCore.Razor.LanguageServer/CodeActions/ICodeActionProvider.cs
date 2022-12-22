// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal interface ICodeActionProvider
{
    /// <summary>
    /// Takes code actions provided by a child language, and provides code actions that should be returned to the LSP client.
    /// </summary>
    /// <remarks>
    /// The list of code actions returned from all providers will be combined together in a list. A null result and an empty
    /// result are effectively the same.
    /// </remarks>
    Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(RazorCodeActionContext context, IEnumerable<RazorVSInternalCodeAction> codeActions, CancellationToken cancellationToken);
}
