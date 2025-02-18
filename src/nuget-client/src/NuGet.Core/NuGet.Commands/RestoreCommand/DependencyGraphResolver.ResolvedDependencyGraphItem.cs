// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;

namespace NuGet.Commands
{
    internal sealed partial class DependencyGraphResolver
    {
        /// <summary>
        /// Represents a resolved dependency graph item for the dependency graph resolver.
        /// </summary>
        [DebuggerDisplay("{LibraryDependency}, RangeIndex={LibraryRangeIndex}")]
        public class ResolvedDependencyGraphItem
        {
            /// <summary>
            /// Stores an array containing all of the <see cref="LibraryDependencyIndex" /> of dependencies of this item. This allows fast lookup of their indices rather than having to convert the name to an index.
            /// </summary>
            private LibraryDependencyIndex[] _dependencyIndices;

            /// <summary>
            /// Stores an array containing all of the <see cref="LibraryRangeIndex" /> of dependencies of this item. This allows fast lookup of their range indices rather than having to convert the <see cref="LibraryRange" /> to an index.
            /// </summary>
            private LibraryRangeIndex[] _rangeIndices;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResolvedDependencyGraphItem" /> class.
            /// </summary>
            /// <param name="item">The <see cref="GraphItem{TItem}" /> with a <see cref="RemoteResolveResult" /> of the resolved library.</param>
            /// <param name="dependencyGraphItem">The <see cref="DependencyGraphItem" /> that is being resolved.</param>
            /// <param name="indexingTable">The <see cref="DependencyGraphItemIndexer" /> to use when indexing dependencies.</param>
            /// <param name="parents">An optional <see cref="HashSet{T}" /> containing parent <see cref="LibraryRangeIndex" /> values.</param>
            public ResolvedDependencyGraphItem(
                GraphItem<RemoteResolveResult> item,
                DependencyGraphItem dependencyGraphItem,
                DependencyGraphItemIndexer indexingTable,
                HashSet<LibraryRangeIndex>? parents = null)
            {
                Item = item;

                LibraryDependency = dependencyGraphItem.LibraryDependency;
                LibraryRangeIndex = dependencyGraphItem.LibraryRangeIndex;
                Path = dependencyGraphItem.Path;

                if (parents != null)
                {
                    Parents = parents;
                }
                else if (dependencyGraphItem.IsCentrallyPinnedTransitivePackage && !dependencyGraphItem.IsRootPackageReference)
                {
                    // Keep track of parents if the item is transitively pinned
                    Parents = new HashSet<LibraryRangeIndex> { dependencyGraphItem.Parent };
                }

                int dependencyCount = item.Data.Dependencies.Count;

                if (dependencyCount == 0)
                {
                    _dependencyIndices = Array.Empty<LibraryDependencyIndex>();
                    _rangeIndices = Array.Empty<LibraryRangeIndex>();
                }
                else
                {
                    // Index all of the dependencies ahead of time so that later on the value can be retrieved very quickly
                    _dependencyIndices = new LibraryDependencyIndex[dependencyCount];
                    _rangeIndices = new LibraryRangeIndex[dependencyCount];

                    for (int i = 0; i < dependencyCount; i++)
                    {
                        LibraryDependency dependency = item.Data.Dependencies[i];

                        _dependencyIndices[i] = indexingTable.Index(dependency);
                        _rangeIndices[i] = indexingTable.Index(dependency.LibraryRange);
                    }
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether or not the dependency graph item is transitively pinned.
            /// </summary>
            public bool IsCentrallyPinnedTransitivePackage { get; init; }

            /// <summary>
            /// Gets or sets a value indicating whether or not the dependency graph item is a direct package reference.
            /// </summary>
            public bool IsRootPackageReference { get; init; }

            /// <summary>
            /// Gets the <see cref="GraphItem{TItem}" /> with a <see cref="RemoteResolveResult" /> of the resolved library.
            /// </summary>
            public GraphItem<RemoteResolveResult> Item { get; }

            /// <summary>
            /// Gets the <see cref="LibraryDependency" /> of the declared dependency in the graph.
            /// </summary>
            public LibraryDependency LibraryDependency { get; }

            /// <summary>
            /// Gets the <see cref="LibraryRangeIndex" /> of this dependency graph item.
            /// </summary>
            public LibraryRangeIndex LibraryRangeIndex { get; }

            /// <summary>
            /// Gets or sets a <see cref="HashSet{T}" /> of parent <see cref="LibraryRangeIndex" /> that have been eclipsed by this dependency graph item.
            /// </summary>
            public HashSet<LibraryRangeIndex>? ParentPathsThatHaveBeenEclipsed { get; set; }

            /// <summary>
            /// Gets or sets a <see cref="HashSet{T}" /> of all of the parent <see cref="LibraryRangeIndex" /> of this dependency graph item.
            /// </summary>
            public HashSet<LibraryRangeIndex>? Parents { get; }

            /// <summary>
            /// Gets an array containing the <see cref="LibraryRangeIndex" /> values of all parent dependency graph items and their parent up to the root.
            /// </summary>
            public LibraryRangeIndex[] Path { get; }

            /// <summary>
            /// Gets or sets a <see cref="List{T}" /> of <see cref="HashSet{T}" /> containing <see cref="LibraryDependencyIndex" /> representing dependencies that should be suppressed under this item.
            /// </summary>
            public required List<HashSet<LibraryDependencyIndex>> Suppressions { get; init; }

            /// <summary>
            /// Gets the <see cref="LibraryDependencyIndex" /> of the dependency at the specified position.
            /// </summary>
            /// <param name="position">The position of the dependency to get the <see cref="LibraryDependencyIndex" /> of.</param>
            /// <returns>The <see cref="LibraryDependencyIndex" /> of the dependency at the specified position.</returns>
            public LibraryDependencyIndex GetDependencyIndexForDependencyAt(int position)
            {
                return _dependencyIndices[position];
            }

            /// <summary>
            /// Gets the <see cref="LibraryRangeIndex" /> of the dependency at the specified position.
            /// </summary>
            /// <param name="position">The position of the dependency to get the <see cref="LibraryRangeIndex" /> of.</param>
            /// <returns>The <see cref="LibraryRangeIndex" /> of the dependency at the specified position.</returns>
            public LibraryRangeIndex GetRangeIndexForDependencyAt(int position)
            {
                return _rangeIndices[position];
            }

            /// <summary>
            /// Sets the <see cref="LibraryRangeIndex" /> of the dependency at the specified position.
            /// </summary>
            /// <param name="position">The position of the dependency to set the <see cref="LibraryRangeIndex" /> of.</param>
            /// <param name="libraryRangeIndex">The new <see cref="LibraryRangeIndex" /> to set.</param>
            public void SetRangeIndexForDependencyAt(int position, LibraryRangeIndex libraryRangeIndex)
            {
                _rangeIndices[position] = libraryRangeIndex;
            }
        }
    }
}
