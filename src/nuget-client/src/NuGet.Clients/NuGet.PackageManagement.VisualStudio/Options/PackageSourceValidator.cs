// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    internal static class PackageSourceValidator
    {
        internal static PackageSource FindExistingOrCreate(
            string source,
            string name,
            bool isEnabled,
            List<PackageSource> packageSources)
        {
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

            PackageSource? foundByName = FindByName(trimmedName, packageSources);
            PackageSource? foundBySource = FindBySource(trimmedSource, packageSources);

            PackageSource packageSource;

            // Create and validate a new Package Source since none was found by name or source.
            if (foundByName is null && foundBySource is null)
            {
                packageSource = new PackageSource(trimmedSource, trimmedName, isEnabled);
                SetAllowInsecureConnectionsProperty(packageSource);
                EnsureValidSources(packageSource);
            }
            // If both name and source match, use the existing PackageSource.
            else if (foundByName is not null && foundBySource is not null)
            {
                // Only the IsEnabled property could be changing.
                foundByName.IsEnabled = isEnabled;
                packageSource = foundByName;
            }
            // The source is changing on an existing PackageSource.
            else if (foundByName is not null)
            {
                // Preserve existing properties by cloning the package source.
                packageSource = new PackageSource(
                    trimmedSource,
                    foundByName.Name,
                    isEnabled,
                    foundByName.IsOfficial,
                    foundByName.IsPersistable)
                {
                    IsMachineWide = foundByName.IsMachineWide,
                    Credentials = foundByName.Credentials,
                    ClientCertificates = foundByName.ClientCertificates,
                    Description = foundByName.Description,
                    ProtocolVersion = foundByName.ProtocolVersion,
                    AllowInsecureConnections = foundByName.AllowInsecureConnections,
                    DisableTLSCertificateValidation = foundByName.DisableTLSCertificateValidation,
                    MaxHttpRequestsPerSource = foundByName.MaxHttpRequestsPerSource,
                };
            }
            // The name is changing on an existing PackageSource.
            else
            {
                // Preserve existing properties by cloning the package source.
                packageSource = new PackageSource(
                    foundBySource!.Source,
                    trimmedName,
                    isEnabled,
                    foundBySource.IsOfficial,
                    foundBySource.IsPersistable)
                {
                    IsMachineWide = foundBySource.IsMachineWide,
                    Credentials = foundBySource.Credentials,
                    ClientCertificates = foundBySource.ClientCertificates,
                    Description = foundBySource.Description,
                    ProtocolVersion = foundBySource.ProtocolVersion,
                    AllowInsecureConnections = foundBySource.AllowInsecureConnections,
                    DisableTLSCertificateValidation = foundBySource.DisableTLSCertificateValidation,
                    MaxHttpRequestsPerSource = foundBySource.MaxHttpRequestsPerSource,
                };
            }

            return packageSource;
        }

        /// <summary>
        /// Validates the Uri of a remote or local package source.
        /// The regex used here will eventually be supported in the Unified Settings registration.json file
        /// for the package sources page. See https://github.com/NuGet/Home/issues/14358.
        /// </summary>
        /// <param name="packageSource"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static void EnsureValidSources(PackageSource packageSource)
        {
            _ = packageSource ?? throw new ArgumentNullException(nameof(packageSource));
            string source = packageSource.Source;

            if (!Common.PathValidator.IsValidLocalPath(source) &&
                !Common.PathValidator.IsValidUncPath(source) &&
                !Common.PathValidator.IsValidUrl(source))
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(PackageSource.Source),
                    actualValue: source,
                    Strings.Error_PackageSource_InvalidSource);
            }
        }

        internal static void ValidateUniquenessOrThrow(List<PackageSource> packageSources)
        {
            _ = packageSources ?? throw new ArgumentNullException(nameof(packageSources));

            EnsureUniqueNames(packageSources);
            EnsureUniqueSources(packageSources);
        }

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

        private static void SetAllowInsecureConnectionsProperty(PackageSource packageSource)
        {
            _ = packageSource ?? throw new ArgumentNullException(nameof(packageSource));

            if (packageSource.IsHttp && !packageSource.IsHttps)
            {
                packageSource.AllowInsecureConnections = true;
            }
        }

        private static PackageSource? FindByName(string name, List<PackageSource> packageSources)
        {
            _ = packageSources ?? throw new ArgumentNullException(nameof(packageSources));

            if (name is null)
            {
                return null;
            }

            string trimmedName = name?.Trim() ?? string.Empty;

            List<PackageSource> existingPackageSource = packageSources
                .Where(packageSource =>
                    string.Equals(packageSource.Name, trimmedName, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            if (existingPackageSource.Count > 1)
            {
                throw new InvalidOperationException(message: Strings.Error_PackageSource_UniqueName);
            }

            return existingPackageSource.SingleOrDefault();
        }

        private static PackageSource? FindBySource(string source, List<PackageSource> packageSources)
        {
            _ = packageSources ?? throw new ArgumentNullException(nameof(packageSources));

            if (source is null)
            {
                return null;
            }

            string trimmedSource = source?.Trim() ?? string.Empty;

            List<PackageSource> existingPackageSource = packageSources
                .Where(packageSource =>
                {
                    string trimmedTargetSource = packageSource.Source?.Trim() ?? string.Empty;
                    bool areTrimmedStringsEqual =
                        string.Equals(
                            trimmedTargetSource,
                            trimmedSource,
                            StringComparison.OrdinalIgnoreCase)
                        && string.Equals(
                            PathValidator.GetCanonicalPath(trimmedTargetSource),
                            PathValidator.GetCanonicalPath(trimmedSource),
                            StringComparison.OrdinalIgnoreCase);
                    return areTrimmedStringsEqual;
                })
                .ToList();

            if (existingPackageSource.Count > 1)
            {
                throw new InvalidOperationException(message: Strings.Error_PackageSource_UniqueSource);
            }

            return existingPackageSource.SingleOrDefault();
        }
    }
}
