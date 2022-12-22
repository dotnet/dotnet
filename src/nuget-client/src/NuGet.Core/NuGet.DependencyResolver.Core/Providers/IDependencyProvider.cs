// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.LibraryModel;

namespace NuGet.DependencyResolver
{
    public interface IDependencyProvider
    {
        bool SupportsType(LibraryDependencyTarget libraryTypeFlag);

        Library GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework);
    }
}
