// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.LibraryModel;

namespace NuGet.DependencyResolver
{
    /// <summary>
    /// Extends <see cref="RemoteMatch"/> to add a reference to the full Library.
    /// </summary>
    public class LocalMatch : RemoteMatch
    {
        /// <summary>
        /// Full local Library metadata
        /// </summary>
        public Library LocalLibrary { get; set; }

        /// <summary>
        /// The local provider where the library was found.
        /// </summary>
        public IDependencyProvider LocalProvider { get; set; }
    }
}
