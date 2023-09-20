// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public class InstallationCompatibility : IInstallationCompatibility
    {
        private static InstallationCompatibility _instance;

        public static InstallationCompatibility Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InstallationCompatibility();
                }

                return _instance;
            }
        }

        public void EnsurePackageCompatibility(
            NuGetProject nuGetProject,
            INuGetPathContext pathContext,
            IEnumerable<NuGetProjectAction> nuGetProjectActions,
            RestoreResult restoreResult)
        {
            // Find all of the installed package identities.
            var requestedPackageIds = new HashSet<string>(
                nuGetProjectActions
                    .Where(action => action.NuGetProjectActionType == NuGetProjectActionType.Install)
                    .Select(action => action.PackageIdentity.Id),
                StringComparer.OrdinalIgnoreCase);

            var installedIdentities = restoreResult
                .RestoreGraphs
                .SelectMany(graph => graph.Flattened)
                .Where(result => result.Key.Type == LibraryType.Package)
                .Select(result => new PackageIdentity(result.Key.Name, result.Key.Version))
                .Distinct()
                .Where(identity => requestedPackageIds.Contains(identity.Id));

            // Read the .nuspec on disk and ensure package compatibility.
            var resolver = new FallbackPackagePathResolver(pathContext);
            foreach (var identity in installedIdentities)
            {
                var packageInfo = resolver.GetPackageInfo(
                    identity.Id,
                    identity.Version);

                if (packageInfo != null)
                {
                    var manifestPath = packageInfo.PathResolver.GetManifestFilePath(
                        identity.Id,
                        identity.Version);
                    var nuspecReader = new NuspecReader(manifestPath);

                    EnsurePackageCompatibility(
                        nuGetProject,
                        identity,
                        nuspecReader);
                }
            }
        }

        /// <summary>
        /// Asynchronously validates the compatibility of a single downloaded package.
        /// </summary>
        /// <param name="nuGetProject">The NuGet project. The type of the NuGet project determines the sorts or
        /// validations that are done.</param>
        /// <param name="packageIdentity">The identity of that package contained in the download result.</param>
        /// <param name="resourceResult">The downloaded package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>.
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nuGetProject" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="resourceResult" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task EnsurePackageCompatibilityAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            DownloadResourceResult resourceResult,
            CancellationToken cancellationToken)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (resourceResult == null)
            {
                throw new ArgumentNullException(nameof(resourceResult));
            }

            cancellationToken.ThrowIfCancellationRequested();

            NuspecReader nuspecReader;
            if (resourceResult.PackageReader != null)
            {
                nuspecReader = await resourceResult.PackageReader.GetNuspecReaderAsync(cancellationToken);
            }
            else
            {
                using (var packageReader = new PackageArchiveReader(resourceResult.PackageStream, leaveStreamOpen: true))
                {
                    nuspecReader = packageReader.NuspecReader;
                }
            }

            EnsurePackageCompatibility(
                nuGetProject,
                packageIdentity,
                nuspecReader);
        }

        private static void EnsurePackageCompatibility(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            NuspecReader nuspecReader)
        {
            // Validate that the current version of NuGet satisfies the minVersion attribute specified in the .nuspec
            MinClientVersionUtility.VerifyMinClientVersion(nuspecReader);

            // Validate the package type. There must be zero package types or exactly one package
            // type that is one of the recognized package types.
            var packageTypes = nuspecReader.GetPackageTypes();
            var identityString = $"{packageIdentity.Id} {packageIdentity.Version.ToNormalizedString()}";

            if (packageTypes.Count > 1)
            {
                throw new PackagingException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.MultiplePackageTypesNotSupported,
                    identityString));
            }
            else if (packageTypes.Count == 1)
            {
                var packageType = packageTypes[0];
                var packageTypeString = packageType.Name;
                if (packageType.Version != PackageType.EmptyVersion)
                {
                    packageTypeString += " " + packageType.Version;
                }

                var projectName = NuGetProject.GetUniqueNameOrName(nuGetProject);

                if (packageType == PackageType.Legacy || // Added for "quirks mode", but not yet fully implemented.
                    packageType == PackageType.Dependency) // A package explicitly stated as a dependency.
                {
                    // These types are always acceptable.
                }
                else
                {
                    throw new PackagingException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UnsupportedPackageType,
                        identityString,
                        packageTypeString,
                        projectName));
                }
            }
        }
    }
}
