// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal abstract class HtmlCodeActionProvider : ICodeActionProvider
{
    public abstract Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions,
        CancellationToken cancellationToken);
}
