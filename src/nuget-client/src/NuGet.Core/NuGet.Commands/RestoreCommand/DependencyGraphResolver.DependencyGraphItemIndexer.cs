// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Concurrent;
using NuGet.LibraryModel;

namespace NuGet.Commands
{
    internal sealed partial class DependencyGraphResolver
    {
        /// <summary>
        /// Represents a unique number for a library by its name, case-insensitive, so that any libraries with the same name will have the same index.
        /// </summary>
        public enum LibraryDependencyIndex : int
        {
            /// <summary>
            /// The index of the root project.
            /// </summary>
            Project = 0,
        }

        /// <summary>
        /// Represents a unique number for a library by its library range.  This generally means that two libraries with the same name and version will have the same index.
        /// </summary>
        public enum LibraryRangeIndex : int
        {
            /// <summary>
            /// The index used as a parent of the root project.
            /// </summary>
            None = -1,

            /// <summary>
            /// The index of the root project.
            /// </summary>
            Project = 0,
        }

        /// <summary>
        /// A class used to translate library dependency names and versions to integers for faster dictionary lookup.
        /// </summary>
        /// <remarks>
        /// Using a dictionary key of a string is relatively slow compared to an integer.  This class uses a dictionary and assigned incrementing numbers to the libraries by name and version and stores them.
        /// </remarks>
        public sealed class DependencyGraphItemIndexer
        {
            /// <summary>
            /// An object used to <see langword="lock" /> access to the library dependency table.
            /// </summary>
            private readonly object _libraryDependencyLockObject = new();

            /// <summary>
            /// An object used to <see langword="lock" /> access to the library range table.
            /// </summary>
            private readonly object _libraryRangeLockObject = new();

            /// <summary>
            /// A case-insensitive dictionary that stores a <see cref="LibraryDependencyIndex" /> by the name of a library.
            /// </summary>
            private readonly ConcurrentDictionary<string, LibraryDependencyIndex> _libraryDependencyTable = new ConcurrentDictionary<string, LibraryDependencyIndex>(StringComparer.OrdinalIgnoreCase);

            /// <summary>
            /// A case-insensitive dictionary that stores a <see cref="LibraryRangeIndex" /> by the <see cref="LibraryRange" /> of a library.  This dictionary uses the <see cref="LibraryRangeComparer" /> to compare <see cref="LibraryRange" /> objects.
            /// </summary>
            private readonly ConcurrentDictionary<LibraryRange, LibraryRangeIndex> _libraryRangeTable = new(LibraryRangeComparer.Instance);

            /// <summary>
            /// Stores the next index to use if a unique library is indexed.
            /// </summary>
            private LibraryDependencyIndex _nextLibraryDependencyIndex = LibraryDependencyIndex.Project + 1;

            /// <summary>
            /// Stores the next index to use if a unique <see cref="LibraryRange" /> is indexed.
            /// </summary>
            private LibraryRangeIndex _nextLibraryRangeIndex = LibraryRangeIndex.Project + 1;

            /// <summary>
            /// Initializes a new instance of the <see cref="DependencyGraphItemIndexer" /> class.
            /// </summary>
            /// <param name="libraryDependency">A <see cref="LibraryDependency" /> representing the root project.</param>
            public DependencyGraphItemIndexer(LibraryDependency libraryDependency)
            {
                _libraryDependencyTable.TryAdd(libraryDependency.Name, LibraryDependencyIndex.Project);
                _libraryRangeTable.TryAdd(libraryDependency.LibraryRange, LibraryRangeIndex.Project);
            }

            /// <summary>
            /// Indexes a <see cref="LibraryDependency" /> and returns a <see cref="LibraryDependencyIndex" /> associated with the its name.
            /// </summary>
            /// <param name="libraryDependency">The <see cref="LibraryDependency" /> of the library to index.</param>
            /// <returns>A <see cref="LibraryDependencyIndex" /> representing the unique integer associated with the name of the library.</returns>
            public LibraryDependencyIndex Index(LibraryDependency libraryDependency) => Index(libraryDependency.Name);

            /// <summary>
            /// Indexes a <see cref="CentralPackageVersion" /> and returns a <see cref="LibraryDependencyIndex" /> associated with the its name."/>
            /// </summary>
            /// <param name="centralPackageVersion">The <see cref="CentralPackageVersion" /> to index.</param>
            /// <returns>A <see cref="LibraryDependencyIndex" /> representing the unique integer associated with the name of the library.</returns>
            public LibraryDependencyIndex Index(CentralPackageVersion centralPackageVersion) => Index(centralPackageVersion.Name);

            /// <summary>
            /// Indexes a library by its name and returns a <see cref="LibraryDependencyIndex" /> associated with it.
            /// </summary>
            /// <param name="name">The name of the library.</param>
            /// <returns>A <see cref="LibraryDependencyIndex" /> representing the unique integer associated with the name of the library.</returns>
            private LibraryDependencyIndex Index(string name)
            {
                lock (_libraryDependencyLockObject)
                {
                    // Attempt to get an existing index value
                    if (_libraryDependencyTable.TryGetValue(name, out LibraryDependencyIndex index))
                    {
                        return index;
                    }

                    // Increment the index since this is a new library
                    index = _nextLibraryDependencyIndex++;

                    // Store the index in the table so it can be used for equal library names
                    _libraryDependencyTable.TryAdd(name, index);

                    return index;
                }
            }

            /// <summary>
            /// Indexes a <see cref="LibraryRange" /> and returns a unique <see cref="LibraryRangeIndex" /> associated with it.
            /// </summary>
            /// <remarks>
            /// In general, this method treats version ranges as equal by name and version.  However, certain type constraints are treated as different.
            /// The <see cref="LibraryRangeComparer" /> is used to determine if two <see cref="LibraryRange" /> objects are equal.
            /// </remarks>
            /// <param name="libraryRange">The <see cref="LibraryRange" /> to index.</param>
            /// <returns>A <see cref="LibraryRangeIndex" /> associated with the <see cref="LibraryRange" />.</returns>
            public LibraryRangeIndex Index(LibraryRange libraryRange)
            {
                lock (_libraryRangeLockObject)
                {
                    // Attempt to get an existing index value
                    if (_libraryRangeTable.TryGetValue(libraryRange, out LibraryRangeIndex index))
                    {
                        return index;
                    }

                    // Increment the index since this is a new library range
                    index = _nextLibraryRangeIndex++;

                    // Store the index in the table so it can be re-used by equal library ranges
                    _libraryRangeTable.TryAdd(libraryRange, index);

                    return index;
                }
            }

            /// <summary>
            /// Creates an array containing the parents of a dependency representing its path.
            /// </summary>
            /// <param name="existingPath">An existing array of <see cref="LibraryRangeIndex" /> values of the parent.</param>
            /// <param name="libraryRangeIndex">The <see cref="LibraryRangeIndex" /> of the library range.</param>
            /// <returns>An array containing the existing path and the specified <see cref="LibraryRangeIndex" />.</returns>
            public static LibraryRangeIndex[] CreatePathToRef(LibraryRangeIndex[] existingPath, LibraryRangeIndex libraryRangeIndex)
            {
                // Create a new array, copy the existing path into it, and add the new library range index
                LibraryRangeIndex[] newPath = new LibraryRangeIndex[existingPath.Length + 1];
                Array.Copy(existingPath, newPath, existingPath.Length);
                newPath[newPath.Length - 1] = libraryRangeIndex;

                return newPath;
            }
        }
    }
}
