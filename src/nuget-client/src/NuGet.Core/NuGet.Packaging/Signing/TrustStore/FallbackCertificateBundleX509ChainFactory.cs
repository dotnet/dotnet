// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace NuGet.Packaging.Signing
{
    internal sealed class FallbackCertificateBundleX509ChainFactory : CertificateBundleX509ChainFactory
    {
        // These constants are dictated by the .NET SDK.
        internal const string SubdirectoryName = "trustedroots";
        internal const string CodeSigningFileName = "codesignctl.pem";
        internal const string TimestampingFileName = "timestampctl.pem";

        private static readonly Lazy<string> ThisAssemblyDirectoryPath = new(GetThisAssemblyDirectoryPath, LazyThreadSafetyMode.ExecutionAndPublication);

        private FallbackCertificateBundleX509ChainFactory(X509Certificate2Collection certificates, string filePath)
            : base(certificates, filePath)
        {
        }

        internal static bool TryCreate(
            X509StorePurpose storePurpose,
            string fileName,
            out FallbackCertificateBundleX509ChainFactory factory)
        {
            factory = null;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = storePurpose switch
                {
                    X509StorePurpose.CodeSigning => CodeSigningFileName,
                    X509StorePurpose.Timestamping => TimestampingFileName,
                    _ => throw new ArgumentException(Strings.InvalidX509StorePurpose, nameof(storePurpose))
                };
            }

            string fullFilePath = Path.Combine(
                ThisAssemblyDirectoryPath.Value,
                SubdirectoryName,
                fileName);

            if (TryImportFromPemFile(fullFilePath, out X509Certificate2Collection certificates))
            {
                factory = new FallbackCertificateBundleX509ChainFactory(certificates, fullFilePath);

                return true;
            }

            return false;
        }

        private static string GetThisAssemblyDirectoryPath()
        {
            string location = typeof(FallbackCertificateBundleX509ChainFactory).Assembly.Location;
            FileInfo thisAssembly = new(location);

            return thisAssembly.DirectoryName;
        }
    }
}

#endif
