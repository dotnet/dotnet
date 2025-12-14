// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    internal static class PackageSourceValidator
    {
        /// <summary>
        /// Finds an existing package source by its unique original package source name (identifier). If none are found, create a new package source.
        /// </summary>
        /// <param name="lookupName">This is the package source Name which is unique and serves as the identifier.</param>
        /// <param name="name">The new value for the name.</param>
        /// <exception cref="ArgumentException"></exception>
        internal static PackageSource FindExistingOrCreate(
            string lookupName,
            string source,
            string name,
            bool isEnabled,
            bool allowInsecureConnections,
            IReadOnlyList<PackageSource> packageSources)
        {
            string trimmedLookupName = lookupName?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedLookupName))
            {
                throw new ArgumentException(message: Strings.Argument_Cannot_Be_Null_Or_Empty, paramName: nameof(lookupName));
            }

            string trimmedSource = source?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedSource))
            {
                throw new ArgumentException(message: Strings.Argument_Cannot_Be_Null_Or_Empty, paramName: nameof(source));
            }

            string trimmedName = name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                throw new ArgumentException(message: Strings.Argument_Cannot_Be_Null_Or_Empty, paramName: nameof(name));
            }

            PackageSource? foundByName = FindByName(trimmedLookupName, packageSources);
            PackageSource packageSource;

            // Create and validate a new Package Source since an existing one was not found.
            if (foundByName is null)
            {
                packageSource = new PackageSource(trimmedSource, trimmedName, isEnabled)
                {
                    AllowInsecureConnections = allowInsecureConnections,
                };

                EnsureValidSources(packageSource);
            }
            else // Found an existing source to update.
            {
                // Preserve existing properties by cloning the package source.
                packageSource = new PackageSource(
                    trimmedSource,
                    trimmedName,
                    isEnabled,
                    foundByName.IsOfficial,
                    foundByName.IsPersistable)
                {
                    IsMachineWide = foundByName.IsMachineWide,
                    Credentials = foundByName.Credentials,
                    ClientCertificates = foundByName.ClientCertificates,
                    Description = foundByName.Description,
                    ProtocolVersion = foundByName.ProtocolVersion,
                    AllowInsecureConnections = allowInsecureConnections,
                    DisableTLSCertificateValidation = foundByName.DisableTLSCertificateValidation,
                    MaxHttpRequestsPerSource = foundByName.MaxHttpRequestsPerSource,
                };
            }

            return packageSource;
        }

        /// <summary>
        /// Validates the Uri of a remote or local package source.
        /// </summary>
        /// <returns>True when valid, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static bool IsValidSource(PackageSource packageSource)
        {
            _ = packageSource ?? throw new ArgumentNullException(nameof(packageSource));
            string source = packageSource.Source;

            if (!Common.PathValidator.IsValidLocalPath(source) &&
                !Common.PathValidator.IsValidUncPath(source) &&
                !Common.PathValidator.IsValidUrl(source))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the Uri of a remote or local package source and throws <see cref="ArgumentOutOfRangeException"/> if invalid.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static void EnsureValidSources(PackageSource packageSource)
        {
            if (!IsValidSource(packageSource))
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(PackageSource.Source),
                    actualValue: packageSource.Source,
                    message: Strings.Error_PackageSource_InvalidSource);
            }
        }

        // Currently not used. Follow-up in https://github.com/NuGet/Home/issues/14507.
        internal static void ValidateUniquenessOrThrow(List<PackageSource> packageSources)
        {
            _ = packageSources ?? throw new ArgumentNullException(nameof(packageSources));

            EnsureUniqueNames(packageSources);
            EnsureUniqueSources(packageSources);
        }

        // Currently not used. Follow-up in https://github.com/NuGet/Home/issues/14507.
        private static void EnsureUniqueNames(List<PackageSource> packageSources)
        {
            var seen = new HashSet<string>(
                capacity: packageSources.Count,
                comparer: StringComparer.CurrentCultureIgnoreCase);

            foreach (PackageSource packageSource in packageSources)
            {
                if (!seen.Add(packageSource.Name.Trim()))
                {
                    throw new ArgumentException(message: Strings.Error_PackageSource_UniqueName);
                }
            }
        }

        // Currently not used. Follow-up in https://github.com/NuGet/Home/issues/14507.
        private static void EnsureUniqueSources(List<PackageSource> packageSources)
        {
            var seen = new HashSet<string>(
                capacity: packageSources.Count,
                comparer: StringComparer.OrdinalIgnoreCase);

            foreach (PackageSource packageSource in packageSources)
            {
                string trimmedSource = packageSource.Source?.Trim() ?? string.Empty;

                bool isDuplicate;
                if (packageSource.IsLocal)
                {
                    string canonicalPath = PathValidator.GetCanonicalPath(trimmedSource);
                    isDuplicate = !seen.Add(canonicalPath);
                }
                else
                {
                    isDuplicate = !seen.Add(trimmedSource);
                }

                if (isDuplicate)
                {
                    throw new ArgumentException(message: Strings.Error_PackageSource_UniqueSource);
                }
            }
        }

        private static PackageSource? FindByName(string packageSourceName, IReadOnlyList<PackageSource> packageSources)
        {
            _ = packageSources ?? throw new ArgumentNullException(nameof(packageSources));

            List<PackageSource> existingPackageSource = packageSources
                .Where(packageSource =>
                    string.Equals(packageSource.Name, packageSourceName, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            if (existingPackageSource.Count > 1)
            {
                throw new InvalidOperationException(message: Strings.Error_PackageSource_UniqueName);
            }

            return existingPackageSource.SingleOrDefault();
        }
    }
}
