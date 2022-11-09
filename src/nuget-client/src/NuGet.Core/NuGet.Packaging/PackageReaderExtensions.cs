// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public static class PackageReaderExtensions
    {
        public static async Task<IEnumerable<string>> GetPackageFilesAsync(
            this IAsyncPackageCoreReader packageReader,
            PackageSaveMode packageSaveMode,
            CancellationToken cancellationToken)
        {
            return (await packageReader
                .GetFilesAsync(cancellationToken))
                .Where(file => PackageHelper.IsPackageFile(file, packageSaveMode));
        }

        public static async Task<IEnumerable<string>> GetSatelliteFilesAsync(
            this IAsyncPackageContentReader packageReader,
            string packageLanguage,
            CancellationToken cancellationToken)
        {
            var satelliteFiles = new List<string>();

            // Existence of the package file is the validation that the package exists
            var libItemGroups = await packageReader.GetLibItemsAsync(cancellationToken);
            foreach (var libItemGroup in libItemGroups)
            {
                var satelliteFilesInGroup = libItemGroup.Items
                    .Where(item =>
                        Path.GetDirectoryName(item)
                            .Split(Path.DirectorySeparatorChar)
                            .Contains(packageLanguage, StringComparer.OrdinalIgnoreCase));

                satelliteFiles.AddRange(satelliteFilesInGroup);
            }

            return satelliteFiles;
        }
    }
}
