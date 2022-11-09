// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Packaging;

namespace Microsoft.DotNet.PackageTesting
{
    class NupkgParser
    {
        public static Package CreatePackageObject(string packagePath)
        {
            PackageArchiveReader nupkgReader = new(packagePath);
            NuspecReader nuspecReader = nupkgReader.NuspecReader;

            string packageId = nuspecReader.GetId();
            string version = nuspecReader.GetVersion().ToString();

            NuGetFramework[] dependencyFrameworks = nuspecReader.GetDependencyGroups()
                .Select(dg => dg.TargetFramework)
                .Where(tfm => tfm != null)
                .ToArray();
            IEnumerable<string> files = nupkgReader.GetFiles()?.Where(t => t.EndsWith(packageId + ".dll"));

            return new Package(packageId, version, files, dependencyFrameworks);
        }
    }
}
