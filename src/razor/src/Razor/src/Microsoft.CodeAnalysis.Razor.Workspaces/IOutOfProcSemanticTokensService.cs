// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IOutOfProcSemanticTokensService
{
    ValueTask<int[]?> GetSemanticTokensDataAsync(
        TextDocument razorDocument,
        LinePositionSpan span,
        Guid correlationId,
        CancellationToken cancellationToken);
}
