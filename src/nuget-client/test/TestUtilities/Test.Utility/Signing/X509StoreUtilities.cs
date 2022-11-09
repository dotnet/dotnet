// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace Test.Utility.Signing
{
    internal static class X509StoreUtilities
    {
        private static readonly IRootX509Store CodeSigningRootX509Store =
            RuntimeEnvironmentHelper.IsWindows ? PlatformX509Store.Instance : new CodeSigningRootX509Store();
        private static readonly IRootX509Store TimestampingRootX509Store =
            RuntimeEnvironmentHelper.IsWindows ? PlatformX509Store.Instance : new TimestampingRootX509Store();
        private static readonly IX509Store OtherX509Store = PlatformX509Store.Instance;

        internal static void AddCertificateToStore(
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
                switch (storePurpose)
                {
                    case X509StorePurpose.CodeSigning:
                        CodeSigningRootX509Store.Add(storeLocation, certificate);
                        break;

                    case X509StorePurpose.Timestamping:
                        TimestampingRootX509Store.Add(storeLocation, certificate);
                        break;

                    default:
                        throw new ArgumentException("Invalid value", nameof(storePurpose));
                }
            }
            else
            {
                OtherX509Store.Add(storeLocation, storeName, certificate);
            }
        }

        internal static void RemoveCertificateFromStore(
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
                switch (storePurpose)
                {
                    case X509StorePurpose.CodeSigning:
                        CodeSigningRootX509Store.Remove(storeLocation, certificate);
                        break;

                    case X509StorePurpose.Timestamping:
                        TimestampingRootX509Store.Remove(storeLocation, certificate);
                        break;

                    default:
                        throw new ArgumentException("Invalid value", nameof(storePurpose));
                }
            }
            else
            {
                OtherX509Store.Remove(storeLocation, storeName, certificate);
            }
        }
    }
}
