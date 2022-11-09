// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    public static class CertificateUtility
    {
        private const int ChainDepthLimit = 10;

        /// <summary>
        /// Converts a X509Certificate2 to a human friendly string of the following format -
        /// Subject Name: CN=name
        /// SHA1 hash: hash
        /// Issued by: CN=issuer
        /// Valid from: issue date time to expiry date time in local time
        /// </summary>
        /// <param name="cert">X509Certificate2 to be converted to string.</param>
        /// <param name="fingerprintAlgorithm">Algorithm used to calculate certificate fingerprint</param>
        /// <returns>string representation of the X509Certificate2.</returns>
        public static string X509Certificate2ToString(X509Certificate2 cert, HashAlgorithmName fingerprintAlgorithm)
        {
            var certStringBuilder = new StringBuilder();
            X509Certificate2ToString(cert, certStringBuilder, fingerprintAlgorithm, indentation: "  ");
            return certStringBuilder.ToString();
        }

        /// <summary>
        /// Converts a X509Certificate2 to a collection of log messages for various verbosity levels -
        /// Subject Name: CN=name
        /// SHA1 hash: hash
        /// Issued by: CN=issuer
        /// Valid from: issue date time to expiry date time in local time
        /// </summary>
        /// <param name="cert">X509Certificate2 to be converted to string.</param>
        /// <param name="fingerprintAlgorithm">Algorithm used to calculate certificate fingerprint</param>
        /// <returns>string representation of the X509Certificate2.</returns>
        internal static IReadOnlyList<SignatureLog> X509Certificate2ToLogMessages(X509Certificate2 cert, HashAlgorithmName fingerprintAlgorithm, string indentation = "  ")
        {
            var certificateFingerprint = GetHashString(cert, fingerprintAlgorithm);
            var issues = new List<SignatureLog>();

            issues.Add(SignatureLog.MinimalLog($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateSubjectName, cert.Subject)}"));
            issues.Add(SignatureLog.InformationLog($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateHashSha1, cert.Thumbprint)}"));
            issues.Add(SignatureLog.MinimalLog($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateHash, fingerprintAlgorithm.ToString(), certificateFingerprint)}"));
            issues.Add(SignatureLog.InformationLog($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateIssuer, cert.IssuerName.Name)}"));
            issues.Add(SignatureLog.MinimalLog($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateValidity, cert.NotBefore, cert.NotAfter)}"));

            return issues;
        }

        private static void X509Certificate2ToString(X509Certificate2 cert, StringBuilder certStringBuilder, HashAlgorithmName fingerprintAlgorithm, string indentation)
        {
            var certificateFingerprint = GetHashString(cert, fingerprintAlgorithm);

            certStringBuilder.AppendLine($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateSubjectName, cert.Subject)}");
            certStringBuilder.AppendLine($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateHashSha1, cert.Thumbprint)}");
            certStringBuilder.AppendLine($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateHash, fingerprintAlgorithm.ToString(), certificateFingerprint)}");
            certStringBuilder.AppendLine($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateIssuer, cert.IssuerName.Name)}");
            certStringBuilder.AppendLine($"{indentation}{string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityCertificateValidity, cert.NotBefore, cert.NotAfter)}");
        }

        /// <summary>
        /// Converts a X509Certificate2Collection to a human friendly string of the following format -
        /// Subject Name: CN=name
        /// SHA1 hash: hash
        /// Issued by: CN=issuer
        /// Valid from: issue date time to expiry date time in local time
        ///
        /// Subject Name: CN=name
        /// SHA1 hash: hash
        /// Issued by: CN=issuer
        /// Valid from: issue date time to expiry date time in local time
        ///
        /// ... N more.
        /// </summary>
        /// <param name="certCollection">X509Certificate2Collection to be converted to string.</param>
        /// <param name="fingerprintAlgorithm">Algorithm used to calculate certificate fingerprint</param>
        /// <returns>string representation of the X509Certificate2Collection.</returns>
        public static string X509Certificate2CollectionToString(X509Certificate2Collection certCollection, HashAlgorithmName fingerprintAlgorithm)
        {
            var collectionStringBuilder = new StringBuilder();

            collectionStringBuilder.AppendLine(Strings.CertUtilityMultipleCertificatesHeader);

            for (var i = 0; i < Math.Min(ChainDepthLimit, certCollection.Count); i++)
            {
                var cert = certCollection[i];
                X509Certificate2ToString(cert, collectionStringBuilder, fingerprintAlgorithm, indentation: "  ");
                collectionStringBuilder.AppendLine();
            }

            if (certCollection.Count > ChainDepthLimit)
            {
                collectionStringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityMultipleCertificatesFooter, certCollection.Count - ChainDepthLimit));
            }

            return collectionStringBuilder.ToString();
        }

        public static string X509ChainToString(X509Chain chain, HashAlgorithmName fingerprintAlgorithm)
        {
            var collectionStringBuilder = new StringBuilder();
            var indentationLevel = "      ";
            var indentation = indentationLevel;

            var chainElementsCount = chain.ChainElements.Count;
            // Start in 1 to omit main certificate (only build the chain)
            for (var i = 1; i < Math.Min(ChainDepthLimit, chainElementsCount); i++)
            {
                X509Certificate2ToString(chain.ChainElements[i].Certificate, collectionStringBuilder, fingerprintAlgorithm, indentation);
                indentation += indentationLevel;
            }

            if (chainElementsCount > ChainDepthLimit)
            {
                collectionStringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, Strings.CertUtilityMultipleCertificatesFooter, chainElementsCount - ChainDepthLimit));
            }

            return collectionStringBuilder.ToString();
        }

        /// <summary>
        /// Determines if a certificate's signature algorithm is supported.
        /// </summary>
        /// <param name="certificate">Certificate to validate</param>
        /// <returns>True if the certificate's signature algorithm is supported.</returns>
        public static bool IsSignatureAlgorithmSupported(X509Certificate2 certificate)
        {
            switch (certificate.SignatureAlgorithm.Value)
            {
                case Oids.Sha256WithRSAEncryption:
                case Oids.Sha384WithRSAEncryption:
                case Oids.Sha512WithRSAEncryption:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Validates the public key requirements for a certificate
        /// </summary>
        /// <param name="certificate">Certificate to validate</param>
        /// <returns>True if the certificate's public key is valid within NuGet signature requirements</returns>
        public static bool IsCertificatePublicKeyValid(X509Certificate2 certificate)
        {
            // Check if the public key is RSA with a valid keysize
            System.Security.Cryptography.RSA RSAPublicKey = RSACertificateExtensions.GetRSAPublicKey(certificate);

            if (RSAPublicKey != null)
            {
                return RSAPublicKey.KeySize >= SigningSpecifications.V1.RSAPublicKeyMinLength;
            }

            return false;
        }

        /// <summary>
        /// Validates if the certificate contains the lifetime signing EKU
        /// </summary>
        /// <param name="certificate">Certificate to validate</param>
        /// <returns>True if the certificate has the lifetime signing EKU</returns>
        public static bool HasLifetimeSigningEku(X509Certificate2 certificate)
        {
            return HasExtendedKeyUsage(certificate, Oids.LifetimeSigningEku);
        }

        /// <summary>
        /// Checks if an X509Certificate2 contains a particular Extended Key Usage (EKU).
        /// </summary>
        /// <param name="certificate">X509Certificate2 to be checked.</param>
        /// <param name="ekuOid">String OID of the Extended Key Usage</param>
        /// <returns>A bool indicating if the X509Certificate2 contains specified OID in its Extended Key Usage.</returns>
        public static bool HasExtendedKeyUsage(X509Certificate2 certificate, string ekuOid)
        {
            foreach (var extension in certificate.Extensions)
            {
                if (string.Equals(extension.Oid.Value, Oids.EnhancedKeyUsage))
                {
                    var ekuExtension = (X509EnhancedKeyUsageExtension)extension;

                    foreach (var eku in ekuExtension.EnhancedKeyUsages)
                    {
                        if (eku.Value == ekuOid)
                        {
                            return true;
                        }
                    }

                    break;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an X509Certificate2 is valid for a particular purpose.
        /// </summary>
        /// <remarks>
        /// This must not be used in evaluation of a signed package.
        /// A more accurate test is building a chain with the specified EKU asserted in the application policy.
        /// </remarks>
        /// <param name="certificate">X509Certificate2 to be checked.</param>
        /// <param name="ekuOid">String OID of the Extended Key Usage</param>
        /// <returns>A bool indicating if the X509Certificate2 contains specified OID string in its Extended Key Usage.</returns>
        public static bool IsValidForPurposeFast(X509Certificate2 certificate, string ekuOid)
        {
            foreach (var extension in certificate.Extensions)
            {
                if (string.Equals(extension.Oid.Value, Oids.EnhancedKeyUsage))
                {
                    var ekuExtension = (X509EnhancedKeyUsageExtension)extension;

                    if (ekuExtension.EnhancedKeyUsages.Count == 0)
                    {
                        return true;
                    }

                    foreach (var eku in ekuExtension.EnhancedKeyUsages)
                    {
                        if (eku.Value == ekuOid)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        public static bool IsCertificateValidityPeriodInTheFuture(X509Certificate2 certificate)
        {
            return DateTime.Now < certificate.NotBefore;
        }

        public static bool IsDateInsideValidityPeriod(X509Certificate2 certificate, DateTimeOffset date)
        {
            DateTimeOffset signerCertExpiry = DateTime.SpecifyKind(certificate.NotAfter, DateTimeKind.Local);
            DateTimeOffset signerCertBegin = DateTime.SpecifyKind(certificate.NotBefore, DateTimeKind.Local);

            return signerCertBegin <= date && date < signerCertExpiry;
        }

        /// <summary>
        /// Gets the certificate fingerprint with the given hashing algorithm
        /// </summary>
        /// <param name="certificate">X509Certificate2 to be compute fingerprint</param>
        /// <param name="hashAlgorithm">Hash algorithm for fingerprint</param>
        /// <returns>A byte array representing the certificate hash.</returns>
        public static byte[] GetHash(X509Certificate2 certificate, HashAlgorithmName hashAlgorithm)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            return hashAlgorithm.ComputeHash(certificate.RawData);
        }


        /// <summary>
        /// Gets the certificate fingerprint string with the given hashing algorithm
        /// </summary>
        /// <param name="certificate">X509Certificate2 to be compute fingerprint</param>
        /// <param name="hashAlgorithm">Hash algorithm for fingerprint</param>
        /// <returns>A string representing the certificate hash.</returns>
        public static string GetHashString(X509Certificate2 certificate, HashAlgorithmName hashAlgorithm)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var certificateFingerprint = GetHash(certificate, hashAlgorithm);
#if NETCOREAPP
            return BitConverter.ToString(certificateFingerprint).Replace("-", "", StringComparison.Ordinal);
#else
            return BitConverter.ToString(certificateFingerprint).Replace("-", "");
#endif
        }

        /// <summary>
        /// Determines if a certificate is self-issued.
        /// </summary>
        /// <remarks>Warning:  this method does not evaluate certificate trust, revocation status, or validity!
        /// This method attempts to build a chain for the provided certificate, and although revocation status
        /// checking is explicitly skipped, the underlying chain building engine may go online to fetch
        /// additional information (e.g.:  the issuer's certificate).  This method is not a guaranteed offline
        /// check.</remarks>
        /// <param name="certificate">The certificate to check.</param>
        /// <returns><c>true</c> if the certificate is self-issued; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="certificate" /> is <c>null</c>.</exception>
        public static bool IsSelfIssued(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            {
                X509Chain chain = chainHolder.Chain;

                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority |
                    X509VerificationFlags.IgnoreRootRevocationUnknown |
                    X509VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown |
                    X509VerificationFlags.IgnoreEndRevocationUnknown;

                CertificateChainUtility.BuildWithPolicy(chain, certificate);

                if (chain.ChainElements.Count != 1)
                {
                    return false;
                }

                if (chain.ChainStatus.Any(
                    chainStatus => chainStatus.Status.HasFlag(X509ChainStatusFlags.Cyclic) ||
                                   chainStatus.Status.HasFlag(X509ChainStatusFlags.PartialChain) ||
                                   chainStatus.Status.HasFlag(X509ChainStatusFlags.NotSignatureValid)))
                {
                    return false;
                }

                if (!certificate.IssuerName.RawData.SequenceEqual(certificate.SubjectName.RawData))
                {
                    return false;
                }

                var akiExtension = certificate.Extensions[Oids.AuthorityKeyIdentifier];
                var skiExtension = certificate.Extensions[Oids.SubjectKeyIdentifier] as X509SubjectKeyIdentifierExtension;

                if (akiExtension != null && skiExtension != null)
                {
                    var reader = new DerSequenceReader(akiExtension.RawData);
                    var keyIdentifierTag = (DerSequenceReader.DerTag)DerSequenceReader.ContextSpecificTagFlag;

                    if (reader.HasTag(keyIdentifierTag))
                    {
                        var keyIdentifier = reader.ReadValue(keyIdentifierTag);
#if NETCOREAPP
                        var akiKeyIdentifier = BitConverter.ToString(keyIdentifier).Replace("-", "", StringComparison.Ordinal);
#else
                        var akiKeyIdentifier = BitConverter.ToString(keyIdentifier).Replace("-", "");
#endif

                        return string.Equals(skiExtension.SubjectKeyIdentifier, akiKeyIdentifier, StringComparison.OrdinalIgnoreCase);
                    }
                }

                return true;
            }
        }

        public static IReadOnlyList<byte[]> GetRawDataForCollection(X509Certificate2Collection certificates)
        {
            var certificatesRawData = new List<byte[]>(capacity: certificates.Count);

            foreach (var certificate in certificates)
            {
                certificatesRawData.Add(certificate.RawData);
            }

            return certificatesRawData.AsReadOnly();
        }
    }
}
