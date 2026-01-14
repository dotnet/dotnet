// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal interface IEditMappingService
{
    /// <summary>
    /// Maps C# changes in a workspace edit, to their equivalent Razor changes, modifying them in place
    /// </summary>
    Task MapWorkspaceEditAsync(IDocumentSnapshot contextDocumentSnapshot, WorkspaceEdit workspaceEdit, CancellationToken cancellationToken);
}
