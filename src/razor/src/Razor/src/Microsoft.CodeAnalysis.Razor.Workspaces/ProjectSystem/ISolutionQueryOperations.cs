// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface ISolutionQueryOperations
{
    /// <summary>
    /// Returns all Razor project snapshots.
    /// </summary>
    IEnumerable<IProjectSnapshot> GetProjects();

    /// <summary>
    ///  Returns all Razor valid project snapshots that contain the given document file path.
    /// </summary>
    /// <param name="documentFilePath">A file path to a Razor document.</param>
    /// <remarks>
    ///  In multi-targeting scenarios, this will return a project for each target that the
    ///  contains the document.
    /// </remarks>
    ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath);
}
