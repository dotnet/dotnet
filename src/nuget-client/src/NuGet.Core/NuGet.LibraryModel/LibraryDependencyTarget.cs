// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.LibraryModel
{
    // Values are from LibraryTypes
    // LibraryDependencyTarget describes the types allowed to fill a dependency.
    [Flags]
    public enum LibraryDependencyTarget : ushort
    {
        None = 0,
        Package = 1 << 0,
        Project = 1 << 1,
        ExternalProject = 1 << 2,
        Assembly = 1 << 3,
        Reference = 1 << 4,
        WinMD = 1 << 5,
        All = Package | Project | ExternalProject | Assembly | Reference | WinMD,

        /// <summary>
        /// A package, project, or external project
        /// </summary>
        PackageProjectExternal = Package | Project | ExternalProject,
    }
}
