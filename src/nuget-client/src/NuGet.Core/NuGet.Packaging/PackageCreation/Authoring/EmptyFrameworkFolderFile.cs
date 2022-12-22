// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace NuGet.Packaging
{
    /// <summary>
    /// Represents an empty framework folder in NuGet 2.0+ packages. 
    /// An empty framework folder is represented by a file named "_._".
    /// </summary>
    internal sealed class EmptyFrameworkFolderFile : PhysicalPackageFile
    {
        public EmptyFrameworkFolderFile(string directoryPathInPackage) :
            base(() => Stream.Null)
        {
            if (directoryPathInPackage == null)
            {
                throw new ArgumentNullException(nameof(directoryPathInPackage));
            }

            TargetPath = System.IO.Path.Combine(directoryPathInPackage, PackagingConstants.PackageEmptyFileName);
        }
    }
}
