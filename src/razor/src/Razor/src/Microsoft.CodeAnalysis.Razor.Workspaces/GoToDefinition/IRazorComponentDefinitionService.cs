// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using LspLocation = Microsoft.VisualStudio.LanguageServer.Protocol.Location;

namespace Microsoft.CodeAnalysis.Razor.GoToDefinition;

/// <summary>
///  Go to Definition support for Razor components.
/// </summary>
internal interface IRazorComponentDefinitionService
{
    Task<LspLocation?> GetDefinitionAsync(
        IDocumentSnapshot documentSnapshot,
        DocumentPositionInfo positionInfo,
        ISolutionQueryOperations solutionQueryOperations,
        bool ignoreAttributes,
        CancellationToken cancellationToken);
}
