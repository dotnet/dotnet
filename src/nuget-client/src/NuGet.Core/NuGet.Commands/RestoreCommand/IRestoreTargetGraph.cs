// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Client;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;

namespace NuGet.Commands
{
    public interface IRestoreTargetGraph
    {
        string Name { get; }

        string TargetGraphName { get; }

        /// <summary>
        /// Gets the runtime identifier used during the restore operation on this graph
        /// </summary>
        string RuntimeIdentifier { get; }

        /// <summary>
        /// Gets the <see cref="NuGetFramework" /> used during the restore operation on this graph
        /// </summary>
        NuGetFramework Framework { get; }

        /// <summary>
        /// Gets the <see cref="ManagedCodeConventions" /> used to resolve assets from packages in this graph
        /// </summary>
        ManagedCodeConventions Conventions { get; }

        /// <summary>
        /// Gets the <see cref="RuntimeGraph" /> that defines runtimes and their relationships for this graph
        /// </summary>
        RuntimeGraph RuntimeGraph { get; }

        /// <summary>
        /// Gets the resolved dependency graph
        /// </summary>
        IEnumerable<GraphNode<RemoteResolveResult>> Graphs { get; }

        ISet<RemoteMatch> Install { get; }

        ISet<GraphItem<RemoteResolveResult>> Flattened { get; }

        ISet<LibraryRange> Unresolved { get; }

        bool InConflict { get; }

        IEnumerable<ResolverConflict> Conflicts { get; }

        AnalyzeResult<RemoteResolveResult> AnalyzeResult { get; }

        ISet<ResolvedDependencyKey> ResolvedDependencies { get; }
    }
}
