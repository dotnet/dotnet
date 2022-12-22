// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Enumeration of package assembly group types.
    /// </summary>
    /// <remarks>
    /// Used by <see cref="PackageBuildFileGroupItem"/>, <see cref="PackageBuildFileItem"/> and their <see cref="IRelation"/> types.
    /// </remarks>
    internal enum PackageBuildFileGroupType
    {
        Build,
        BuildMultiTargeting
    }
}
