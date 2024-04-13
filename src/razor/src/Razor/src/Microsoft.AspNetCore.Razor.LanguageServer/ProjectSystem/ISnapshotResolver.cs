// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal interface ISnapshotResolver
{
    /// <summary>
    /// Finds all the projects where the document path starts with the path of the folder that contains the project file.
    /// </summary>
    ImmutableArray<IProjectSnapshot> FindPotentialProjects(string documentFilePath);

    Task<IProjectSnapshot> GetMiscellaneousProjectAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Finds a <see cref="IDocumentSnapshot"/> for the given document path that is contained within any project, and returns the first
    /// one found if it does. This method should be avoided where possible, and the overload that takes a <see cref="ProjectKey"/> should be used instead
    /// </summary>
    Task<IDocumentSnapshot?> ResolveDocumentInAnyProjectAsync(string documentFilePath, CancellationToken cancellationToken);
}
