// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IRazorComponentSearchEngine
{
    Task<IDocumentSnapshot?> TryLocateComponentAsync(
        TagHelperDescriptor tagHelper,
        ISolutionQueryOperations solutionQueryOperations,
        CancellationToken cancellationToken);
}
