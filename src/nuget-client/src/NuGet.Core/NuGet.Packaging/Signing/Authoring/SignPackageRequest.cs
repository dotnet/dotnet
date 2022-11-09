// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Contains a request for generating package signature.
    /// </summary>
    public abstract class SignPackageRequest : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Hash algorithm used to create the package signature.
        /// </summary>
        public HashAlgorithmName SignatureHashAlgorithm { get; }

        /// <summary>
        /// Hash algorithm used to timestamp the signed package.
        /// </summary>
        public HashAlgorithmName TimestampHashAlgorithm { get; }

        /// <summary>
        /// Certificate used to sign the package.
        /// </summary>
        public X509Certificate2 Certificate { get; }

        /// <summary>
        /// Gets a collection of additional certificates for building a chain for the signing certificate.
        /// </summary>
        public X509Certificate2Collection AdditionalCertificates { get; }

        /// <summary>
        /// Gets the signature type.
        /// </summary>
        public abstract SignatureType SignatureType { get; }

        internal IX509CertificateChain Chain { get; private set; }

#if IS_SIGNING_SUPPORTED
        /// <summary>
        /// PrivateKey is only used in mssign command.
        /// </summary>
        public System.Security.Cryptography.CngKey PrivateKey { get; set; }
#endif

        protected SignPackageRequest(
            X509Certificate2 certificate,
            HashAlgorithmName signatureHashAlgorithm,
            HashAlgorithmName timestampHashAlgorithm)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (!Enum.IsDefined(typeof(HashAlgorithmName), signatureHashAlgorithm) ||
                signatureHashAlgorithm == HashAlgorithmName.Unknown)
            {
                throw new ArgumentException(Strings.InvalidArgument, nameof(signatureHashAlgorithm));
            }

            if (!Enum.IsDefined(typeof(HashAlgorithmName), timestampHashAlgorithm) ||
                timestampHashAlgorithm == HashAlgorithmName.Unknown)
            {
                throw new ArgumentException(Strings.InvalidArgument, nameof(timestampHashAlgorithm));
            }

            Certificate = certificate;
            SignatureHashAlgorithm = signatureHashAlgorithm;
            TimestampHashAlgorithm = timestampHashAlgorithm;
            AdditionalCertificates = new X509Certificate2Collection();
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                Certificate?.Dispose();
                Chain?.Dispose();

#if IS_SIGNING_SUPPORTED
                PrivateKey?.Dispose();
#endif
            }

            _isDisposed = true;
        }

        internal void BuildSigningCertificateChainOnce(ILogger logger)
        {
            if (Chain == null)
            {
                Chain = CertificateChainUtility.GetCertificateChain(
                    Certificate,
                    AdditionalCertificates,
                    logger,
                    CertificateType.Signature);
            }
        }
    }
}
