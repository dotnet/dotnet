// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.CommandLine;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Protocol;

namespace NuGet.MSSigning.Extensions
{
    public abstract class MSSignAbstract : Command
    {
        [Option(typeof(NuGetMSSignCommand), "MSSignCommandCSPNameDescription")]
        public string CSPName { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandKeyContainerDescription")]
        public string KeyContainer { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandHashAlgorithmDescription")]
        public string HashAlgorithm { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandTimestamperDescription")]
        public string Timestamper { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandTimestampHashAlgorithmDescription")]
        public string TimestampHashAlgorithm { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandCertificateFingerprintDescription")]
        public string CertificateFingerprint { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandCertificateFileDescription")]
        public string CertificateFile { get; set; }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandOutputDirectoryDescription")]
        public string OutputDirectory { get; set; }

        protected IEnumerable<string> GetPackages()
        {
            // resolve path into multiple packages if needed.
            var packagesToSign = LocalFolderUtility.ResolvePackageFromPath(Arguments[0]);
            LocalFolderUtility.EnsurePackageFileExists(Arguments[0], packagesToSign);

            return packagesToSign;
        }

        protected CngKey GetPrivateKey(X509Certificate2 cert)
        {
            var rsakey = cert.GetRSAPrivateKey() as RSACng;

            if (rsakey != null)
            {
                return rsakey.Key;
            }

            CngKey cngkey = null;

            try
            {
                var provider = new CngProvider(CSPName);
                cngkey = CngKey.Open(KeyContainer, provider, CngKeyOpenOptions.MachineKey);

                if (cngkey == null)
                {
                    throw new InvalidOperationException(NuGetMSSignCommand.MSSignCommandNoCngKeyException);
                }

                if (cngkey.AlgorithmGroup != CngAlgorithmGroup.Rsa)
                {
                    cngkey.Dispose();
                    throw new InvalidOperationException(NuGetMSSignCommand.MSSignCommandInvalidCngKeyException);
                }

                return cngkey;
            }
            catch (CryptographicException)
            {
                cngkey?.Dispose();
                throw new InvalidOperationException(NuGetMSSignCommand.MSSignCommandNoCngKeyException);
            }
        }

        protected X509Certificate2 GetCertificate(X509Certificate2Collection certCollection)
        {
            var matchingCertCollection = certCollection.Find(X509FindType.FindByThumbprint, CertificateFingerprint, validOnly: false);

            if (matchingCertCollection == null || matchingCertCollection.Count == 0)
            {
                throw new InvalidOperationException(NuGetMSSignCommand.MSSignCommandNoCertException);
            }

            return matchingCertCollection[0];
        }

        protected X509Certificate2Collection GetCertificateCollection()
        {
            var certCollection = new X509Certificate2Collection();
            certCollection.Import(CertificateFile);

            return certCollection;
        }

        protected void ValidatePackagePath()
        {
            // Assert mandatory argument
            if (Arguments.Count < 1 ||
                string.IsNullOrEmpty(Arguments[0]))
            {
                throw new ArgumentException(NuGetMSSignCommand.MSSignCommandNoPackageException);
            }
        }

        protected void WarnIfNoTimestamper(ILogger logger)
        {
            if (string.IsNullOrEmpty(Timestamper))
            {
                logger.Log(LogMessage.CreateWarning(NuGetLogCode.NU3002, NuGetMSSignCommand.MSSignCommandNoTimestamperWarning));
            }
        }

        protected void EnsureOutputDirectory()
        {
            if (!string.IsNullOrEmpty(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }
        }

        protected void ValidateCertificateInputs()
        {
            if (string.IsNullOrEmpty(CertificateFile))
            {
                // Throw if user gave no certificate input
                throw new ArgumentException(NuGetMSSignCommand.MSSignCommandNoValidCertificateFileException);
            }

            if (string.IsNullOrEmpty(CSPName))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                        nameof(CSPName)));
            }

            if (string.IsNullOrEmpty(KeyContainer))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                        nameof(KeyContainer)));
            }

            if (string.IsNullOrEmpty(CertificateFingerprint))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                        nameof(CertificateFingerprint)));
            }
        }

        protected Common.HashAlgorithmName ValidateAndParseHashAlgorithm(string value, string name, SigningSpecifications spec)
        {
            var hashAlgorithm = Common.HashAlgorithmName.SHA256;

            if (!string.IsNullOrEmpty(value))
            {
                hashAlgorithm = CryptoHashUtility.GetHashAlgorithmName(value);

                if (!spec.AllowedHashAlgorithms.Contains(hashAlgorithm))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                        name));
                }
            }

            if (hashAlgorithm == Common.HashAlgorithmName.Unknown)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                        name));
            }

            return hashAlgorithm;
        }
    }
}
