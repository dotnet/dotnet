// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CommandLine
{
    /// <summary>
    /// Handles updating the executing instance of NuGet.exe
    /// </summary>
    public class SelfUpdater
    {
        private const string NuGetCommandLinePackageId = "NuGet.CommandLine";
        private const string NuGetExe = "NuGet.exe";

        private string _assemblyLocation;
        private readonly Lazy<string> _lazyAssemblyLocation = new Lazy<string>(() =>
        {
            return typeof(SelfUpdater).Assembly.Location;
        });

        private readonly IConsole _console;

        public SelfUpdater(IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }
            _console = console;
        }

        /// <summary>
        /// This property is used only for testing (so that the self updater does not replace the running test
        /// assembly).
        /// </summary>
        internal string AssemblyLocation
        {
            get
            {
                return _assemblyLocation ?? _lazyAssemblyLocation.Value;
            }
            set
            {
                _assemblyLocation = value;
            }
        }

        public Task UpdateSelfAsync(bool prerelease, PackageSource updateFeed)
        {
            Assembly assembly = typeof(SelfUpdater).Assembly;
            var version = GetNuGetVersion(assembly) ?? new NuGetVersion(assembly.GetName().Version);
            return UpdateSelfFromVersionAsync(AssemblyLocation, prerelease, version, updateFeed, CancellationToken.None);
        }

        internal async Task UpdateSelfFromVersionAsync(string exePath, bool prerelease, NuGetVersion currentVersion, PackageSource source, CancellationToken cancellationToken)
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                _console.WriteLine(LocalizedResourceManager.GetString("UpdateCommandCheckingForUpdates"), source);

                var sourceRepository = Repository.Factory.GetCoreV3(source);
                var metadataResource = await sourceRepository.GetResourceAsync<MetadataResource>(cancellationToken);
                var latestVersion = await metadataResource.GetLatestVersion(NuGetCommandLinePackageId, prerelease, includeUnlisted: false, sourceCacheContext, _console, cancellationToken);

                _console.WriteLine(LocalizedResourceManager.GetString("UpdateCommandCurrentlyRunningNuGetExe"), currentVersion);

                // Check to see if an update is needed
                if (latestVersion == null || currentVersion >= latestVersion)
                {
                    _console.WriteLine(LocalizedResourceManager.GetString("UpdateCommandNuGetUpToDate"));
                }
                else
                {
                    _console.WriteLine(LocalizedResourceManager.GetString("UpdateCommandUpdatingNuGet"), latestVersion);

                    var packageIdentity = new PackageIdentity(NuGetCommandLinePackageId, latestVersion);

                    var tempDir = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), Path.GetRandomFileName());
                    var nupkgPath = FileUtility.GetTempFilePath(tempDir);
                    try
                    {
                        Directory.CreateDirectory(tempDir);

                        DownloadResourceResult downloadResourceResult = await PackageDownloader.GetDownloadResourceResultAsync(
                                            sourceRepository,
                                            packageIdentity,
                                            new PackageDownloadContext(sourceCacheContext),
                                            tempDir,
                                            _console,
                                            cancellationToken);

                        // Get the exe path and move it to a temp file (NuGet.exe.old) so we can replace the running exe with the bits we got 
                        // from the package repository
                        IEnumerable<string> packageFiles = await downloadResourceResult.PackageReader.GetFilesAsync(CancellationToken.None);
                        string nugetExeInPackageFilePath = packageFiles.FirstOrDefault(f => Path.GetFileName(f).Equals(NuGetExe, StringComparison.OrdinalIgnoreCase));

                        // If for some reason this package doesn't have NuGet.exe then we don't want to use it
                        if (nugetExeInPackageFilePath == null)
                        {
                            throw new CommandException(LocalizedResourceManager.GetString("UpdateCommandUnableToLocateNuGetExe"));
                        }

                        string renamedPath = exePath + ".old";

                        FileUtility.Move(exePath, renamedPath);

                        using (Stream fromStream = await downloadResourceResult.PackageReader.GetStreamAsync(nugetExeInPackageFilePath, cancellationToken), toStream = File.Create(exePath))
                        {
                            fromStream.CopyTo(toStream);
                        }
                    }
                    finally
                    {
                        // Delete the temporary directory
                        try
                        {
                            Directory.Delete(tempDir, recursive: true);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            _console.WriteLine(LocalizedResourceManager.GetString("UpdateCommandUpdateSuccessful"));
        }

        internal static NuGetVersion GetNuGetVersion(Assembly assembly)
        {
            try
            {
                var assemblyInformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                return new NuGetVersion(assemblyInformationalVersion.InformationalVersion);
            }
            catch
            {
                // Don't let GetCustomAttributes throw.
            }
            return null;
        }
    }
}
