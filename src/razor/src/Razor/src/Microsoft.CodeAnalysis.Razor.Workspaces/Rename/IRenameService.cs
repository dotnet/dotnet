// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Rename;

internal interface IRenameService
{
    Task<RenameResult> TryGetRazorRenameEditsAsync(
        DocumentContext documentContext,
        DocumentPositionInfo positionInfo,
        string newName,
        ISolutionQueryOperations solutionQueryOperations,
        CancellationToken cancellationToken);
}

internal readonly record struct RenameResult(WorkspaceEdit? Edit, bool FallbackToCSharp = true);
