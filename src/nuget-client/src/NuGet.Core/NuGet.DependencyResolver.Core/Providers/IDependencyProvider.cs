// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;
using NuGet.LibraryModel;

namespace NuGet.DependencyResolver
{
    public interface IDependencyProvider
    {
        bool SupportsType(LibraryDependencyTarget libraryTypeFlag);

        [Obsolete("This method is obsolete and will be removed in future versions. Use GetLibrary(LibraryRange, NuGetFramework, string) instead.")]
        Library? GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework);

        Library? GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework, string? alias);
    }
}
