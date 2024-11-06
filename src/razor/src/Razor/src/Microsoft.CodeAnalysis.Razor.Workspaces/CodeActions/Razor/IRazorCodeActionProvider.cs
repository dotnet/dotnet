// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal interface IRazorCodeActionProvider
{
    Task<ImmutableArray<RazorVSInternalCodeAction>> ProvideAsync(
        RazorCodeActionContext context,
        CancellationToken cancellationToken);
}
