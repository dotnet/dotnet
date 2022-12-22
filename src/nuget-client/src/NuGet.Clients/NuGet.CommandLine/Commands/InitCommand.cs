// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "init", "InitCommandDescription",
    MinArgs = 2, MaxArgs = 2, UsageDescriptionResourceName = "InitCommandUsageDescription",
    UsageSummaryResourceName = "InitCommandUsageSummary", UsageExampleResourceName = "InitCommandUsageExamples")]
    public class InitCommand : Command
    {
        [Option(typeof(NuGetCommand), "ExpandDescription")]
        public bool Expand { get; set; }

        public override async Task ExecuteCommandAsync()
        {
            // Arguments[0] or Arguments[1] will not be null at this point.
            // Because, this command has MinArgs set to 2.
            var source = Arguments[0];
            var destination = Arguments[1];

            OfflineFeedUtility.ThrowIfInvalidOrNotFound(
                source,
                isDirectory: true,
                resourceString: LocalizedResourceManager.GetString(nameof(NuGetResources.InitCommand_FeedIsNotFound)));

            // If the Destination Feed Folder does not exist, it will be created.
            OfflineFeedUtility.ThrowIfInvalid(destination);

            var packagePaths = GetPackageFilePaths(source, "*" + PackagingCoreConstants.NupkgExtension);

            if (packagePaths.Count > 0)
            {
                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, Console);

                var packageExtractionContext = new PackageExtractionContext(
                    Expand ? PackageSaveMode.Defaultv3 : PackageSaveMode.Nuspec | PackageSaveMode.Nupkg,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    clientPolicyContext,
                    Console);

                foreach (var packagePath in packagePaths)
                {
                    var offlineFeedAddContext = new OfflineFeedAddContext(
                        packagePath,
                        destination,
                        Console, // IConsole is an ILogger
                        throwIfSourcePackageIsInvalid: false,
                        throwIfPackageExistsAndInvalid: false,
                        throwIfPackageExists: false,
                        extractionContext: packageExtractionContext);

                    await OfflineFeedUtility.AddPackageToSource(offlineFeedAddContext, CancellationToken.None);
                }
            }
            else
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString(nameof(NuGetResources.InitCommand_FeedContainsNoPackages)),
                    source);

                Console.LogMinimal(message);
            }
        }

        /// <summary>
        /// Helper method based on LocalPackageRepository and ExpandedPackageRepository
        /// to avoid dependency on NuGet.Core. Links to the classes are
        /// https://github.com/NuGet/NuGet2/blob/2.9/src/Core/Repositories/LocalPackageRepository.cs
        /// AND
        /// https://github.com/NuGet/NuGet2/blob/2.9/src/Core/Repositories/ExpandedPackageRepository.cs
        /// </summary>
        private static IReadOnlyList<string> GetPackageFilePaths(string source, string nupkgFilter)
        {
            var packagePaths = new List<string>();
            var isV2StyleFolderSource = IsV2StyleFolderSource(source, nupkgFilter);

            if (!isV2StyleFolderSource.HasValue)
            {
                // There are no nupkg files, v2-style or v3-style, under 'source'.
                return packagePaths;
            }

            if (isV2StyleFolderSource.Value)
            {
                foreach (var idDirectory in Directory.EnumerateDirectories(source))
                {
                    // Since we need the .nupkg file for nuget.exe init, PackageSaveMode.Nuspec is not supported.
                    // And, Default search option for EnumerateFiles is top directory only.
                    var packagesAtIdDirectory = Directory.EnumerateFiles(idDirectory, nupkgFilter);

                    packagePaths.AddRange(packagesAtIdDirectory);
                }

                var packagesAtRoot = Directory.EnumerateFiles(source, nupkgFilter);
                packagePaths.AddRange(packagesAtRoot);
            }
            else
            {
                foreach (var idDirectory in Directory.EnumerateDirectories(source))
                {
                    var packageId = Path.GetFileName(idDirectory);

                    foreach (var versionDirectory in Directory.EnumerateDirectories(idDirectory))
                    {
                        var packagesAtVersionDirectory = Directory.EnumerateFiles(versionDirectory, nupkgFilter);
                        packagePaths.AddRange(packagesAtVersionDirectory);
                    }
                }
            }

            return packagePaths;
        }

        /// <summary>
        /// Helper method based on the LazyLocalPackageRepository.cs to avoid dependency on NuGet.Core
        /// https://github.com/NuGet/NuGet2/blob/2.9/src/Core/Repositories/LazyLocalPackageRepository.cs#L74
        /// </summary>
        /// <returns>Return true if source v2 style folder. Otherwise, false.
        /// If no nupkgs were found under the source, returns null</returns>
        private static bool? IsV2StyleFolderSource(string source, string nupkgFilter)
        {
            var packagesAtRoot = Directory.EnumerateFiles(source, nupkgFilter);

            if (packagesAtRoot.Any())
            {
                return true;
            }

            foreach (var idDirectory in Directory.EnumerateDirectories(source))
            {
                // Since we need the .nupkg file for nuget.exe init, PackageSaveMode.Nuspec is not supported.
                // And, Default search option for EnumerateFiles is top directory only.
                var packagesAtIdDirectory = Directory.EnumerateFiles(idDirectory, nupkgFilter);

                if (packagesAtIdDirectory.Any())
                {
                    return true;
                }

                foreach (var versionDirectory in Directory.EnumerateDirectories(idDirectory))
                {
                    var packagesAtVersionDirectory = Directory.EnumerateFiles(versionDirectory, nupkgFilter);

                    if (packagesAtVersionDirectory.Any())
                    {
                        return false;
                    }
                }
            }

            return null;
        }
    }
}
