// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    /// <summary>
    /// Helpers for dealing with the NuGet client version and package minClientVersions.
    /// </summary>
    public static class MinClientVersionUtility
    {
        private static NuGetVersion _clientVersion;

        /// <summary>
        /// Check the package minClientVersion and throw if it is greater than the current client version.
        /// </summary>
        public static void VerifyMinClientVersion(NuspecCoreReaderBase nuspecReader)
        {
            if (nuspecReader == null)
            {
                throw new ArgumentNullException(nameof(nuspecReader));
            }

            if (!IsMinClientVersionCompatible(nuspecReader))
            {
                var packageIdentity = nuspecReader.GetIdentity();
                var packageMinClientVersion = nuspecReader.GetMinClientVersion();
                var clientVersion = GetNuGetClientVersion();

                throw new MinClientVersionException(
                    string.Format(CultureInfo.CurrentCulture, Strings.PackageMinVersionNotSatisfied,
                        packageIdentity.Id + " " + packageIdentity.Version.ToNormalizedString(),
                        packageMinClientVersion.ToNormalizedString(), clientVersion.ToNormalizedString()));
            }
        }

        /// <summary>
        /// Verify minClientVersion.
        /// </summary>
        public static bool IsMinClientVersionCompatible(NuspecCoreReaderBase nuspecReader)
        {
            if (nuspecReader == null)
            {
                throw new ArgumentNullException(nameof(nuspecReader));
            }

            // Read the minClientVersion from the nuspec, this may be null
            var packageMinClientVersion = nuspecReader.GetMinClientVersion();

            return (packageMinClientVersion == null || IsMinClientVersionCompatible(packageMinClientVersion));
        }

        /// <summary>
        /// Verify minClientVersion.
        /// </summary>
        public static bool IsMinClientVersionCompatible(NuGetVersion packageMinClientVersion)
        {
            if (packageMinClientVersion == null)
            {
                throw new ArgumentNullException(nameof(packageMinClientVersion));
            }

            var clientVersion = GetNuGetClientVersion();

            var result = (packageMinClientVersion <= clientVersion);

            return result;
        }

        /// <summary>
        /// Read the NuGet client version from the assembly info as a NuGetVersion.
        /// </summary>
        public static NuGetVersion GetNuGetClientVersion()
        {
            if (_clientVersion == null)
            {
                var versionString = ClientVersionUtility.GetNuGetAssemblyVersion();

                NuGetVersion clientVersion;
                if (!NuGetVersion.TryParse(versionString, out clientVersion))
                {
                    throw new InvalidOperationException(Strings.UnableToParseClientVersion);
                }

                // Remove pre-release info from the version and return a stable version.
                _clientVersion = new NuGetVersion(
                    major: clientVersion.Major,
                    minor: clientVersion.Minor,
                    patch: clientVersion.Patch,
                    revision: clientVersion.Revision);
            }

            return _clientVersion;
        }
    }
}
