// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.VisualStudio.Implementation.Extensibility;

namespace NuGet.VisualStudio
{
    internal static class PackageManagementHelpers
    {
        public static IVsPackageMetadata CreateMetadata(string nupkgPath, PackageIdentity package)
        {
            IEnumerable<string> authors = Enumerable.Empty<string>();
            string description = string.Empty;
            string title = package.Id;
            string installPath = string.Empty;

            try
            {
                // installPath is the nupkg path
                FileInfo file = new FileInfo(nupkgPath);
                installPath = file.Directory.FullName;
                using (var reader = new PackageArchiveReader(file.OpenRead()))
                using (var nuspecStream = reader.GetNuspec())
                {
                    NuspecReader nuspec = new NuspecReader(nuspecStream);

                    var metadata = nuspec.GetMetadata();

                    authors = GetNuspecValue(metadata, "authors").Split(',').ToArray();
                    title = GetNuspecValue(metadata, "title");
                    description = GetNuspecValue(metadata, "description");
                }
            }
            catch (Exception ex)
            {
                // ignore errors from reading the extra fields
                Debug.Fail(ex.ToString());
            }

            if (String.IsNullOrEmpty(title))
            {
                title = package.Id;
            }

            return new VsPackageMetadata(package, title, authors, description, installPath);
        }

        private static string GetNuspecValue(IEnumerable<KeyValuePair<string, string>> metadata, string field)
        {
            var node = metadata.FirstOrDefault(e => StringComparer.Ordinal.Equals(field, e.Key));

            return node.Value ?? string.Empty;
        }
    }
}
