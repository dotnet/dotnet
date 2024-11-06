// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Rename;

internal interface IRenameService
{
    Task<WorkspaceEdit?> TryGetRazorRenameEditsAsync(
        DocumentContext documentContext,
        DocumentPositionInfo positionInfo,
        string newName,
        ISolutionQueryOperations solutionQueryOperations,
        CancellationToken cancellationToken);
}
