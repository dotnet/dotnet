// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CS1591

using System;
using System.Security.Cryptography.X509Certificates;
#if NET5_0_OR_GREATER
using System.Security.Principal;
#endif

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    // On .NET 5+, root certificates are always added to both in-memory stores (code signing
    // and timestamping) regardless of the requested purpose.  This matches the platform root
    // store behavior where a single store serves all purposes.  When elevated, certificates
    // are also added to the platform root store so that out-of-process tools (e.g. dotnet.exe,
    // nuget.exe) spawned as child processes can find them.
    public static class X509StoreUtilities
    {
#if NET5_0_OR_GREATER
        private static readonly bool s_isElevated = GetIsElevated();
        private static readonly IRootX509Store InMemoryCodeSigningRootX509Store = new CodeSigningRootX509Store();
        private static readonly IRootX509Store InMemoryTimestampingRootX509Store = new TimestampingRootX509Store();
#endif
        private static readonly IX509Store OtherX509Store = PlatformX509Store.Instance;

        public static void AddCertificateToStore(
            StoreLocation storeLocation,
            StoreName storeName,
            X509Certificate2 certificate,
            X509StorePurpose storePurpose)
        {
            if (certificate is null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (storeName == StoreName.Root)
            {
#if NET5_0_OR_GREATER
                InMemoryCodeSigningRootX509Store.Add(storeLocation, certificate);
                InMemoryTimestampingRootX509Store.Add(storeLocation, certificate);

                if (s_isElevated)
                {
                    PlatformX509Store.Instance.Add(storeLocation, certificate);
                }
#else
                PlatformX509Store.Instance.Add(storeLocation, certificate);
#endif
            }
            else
            {
                OtherX509Store.Add(storeLocation, storeName, certificate);
            }
        }

        public static void RemoveCertificateFromStore(
            StoreLocation storeLocation,
            StoreName storeName,
            X509Certificate2 certificate,
            X509StorePurpose storePurpose)
        {
            if (certificate is null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (storeName == StoreName.Root)
            {
#if NET5_0_OR_GREATER
                InMemoryCodeSigningRootX509Store.Remove(storeLocation, certificate);
                InMemoryTimestampingRootX509Store.Remove(storeLocation, certificate);

                if (s_isElevated)
                {
                    PlatformX509Store.Instance.Remove(storeLocation, certificate);
                }
#else
                PlatformX509Store.Instance.Remove(storeLocation, certificate);
#endif
            }
            else
            {
                OtherX509Store.Remove(storeLocation, storeName, certificate);
            }
        }

#if NET5_0_OR_GREATER
        private static bool GetIsElevated()
        {
            if (OperatingSystem.IsWindows())
            {
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
#endif
    }
}
