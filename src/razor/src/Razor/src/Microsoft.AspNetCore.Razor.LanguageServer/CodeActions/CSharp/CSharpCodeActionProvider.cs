// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal abstract class CSharpCodeActionProvider : ICodeActionProvider
{
    protected static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> EmptyResult =
        Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(Array.Empty<RazorVSInternalCodeAction>());

    public abstract Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions,
        CancellationToken cancellationToken);
}
