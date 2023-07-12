// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal static class ISnapshotResolverExtensions
{
    public static bool TryResolveProject(this ISnapshotResolver snapshotResolver, string documentFilePath, [NotNullWhen(true)] out IProjectSnapshot? projectSnapshot)
    {
        if (snapshotResolver.TryResolveDocument(documentFilePath, out var documentSnapshot))
        {
            projectSnapshot = documentSnapshot.Project;
            return true;
        }

        projectSnapshot = null;
        return false;
    }
}
