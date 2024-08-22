// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Signing;

namespace NuGet.Test.Utility
{
    public static class GlobalFolderUtility
    {
        /// <summary>
        /// Add a nupkg to the global package folder.
        /// </summary>
        public static Task AddPackageToGlobalFolderAsync(FileInfo packagePath, DirectoryInfo globalFolder, bool requireSignVerify = false)
        {
            return AddPackageToGlobalFolderAsync(packagePath.FullName, globalFolder.FullName, requireSignVerify);
        }

        /// <summary>
        /// Add a nupkg to the global package folder.
        /// </summary>
        public static async Task AddPackageToGlobalFolderAsync(string packagePath, string globalFolder, bool requireSignVerify = false)
        {
            using (var reader = new PackageArchiveReader(packagePath))
            {
                var clientPolicyContext = requireSignVerify ?
                    ClientPolicyContext.GetClientPolicy(Configuration.NullSettings.Instance, Common.NullLogger.Instance) : null;

                var pathContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    clientPolicyContext,
                    Common.NullLogger.Instance);

                var versionFolderPathResolver = new VersionFolderPathResolver(globalFolder);

                using (var stream = File.OpenRead(packagePath))
                {
                    await PackageExtractor.InstallFromSourceAsync(
                        null,
                        reader.GetIdentity(),
                        async (d) => await stream.CopyToAsync(d),
                        versionFolderPathResolver,
                        pathContext,
                        CancellationToken.None);
                }
            }
        }
    }
}
