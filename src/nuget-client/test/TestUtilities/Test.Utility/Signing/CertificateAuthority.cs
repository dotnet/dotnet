// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;

namespace Test.Utility.Signing
{
    public sealed class CertificateAuthority : HttpResponder
    {
        private readonly Dictionary<BigInteger, X509Certificate> _issuedCertificates;
        private readonly Dictionary<BigInteger, RevocationInfo> _revokedCertificates;
        private readonly Lazy<OcspResponder> _ocspResponder;
        private BigInteger _nextSerialNumber;

        /// <summary>
        /// This base URI is shared amongst all HTTP responders hosted by the same web host instance.
        /// </summary>
        public Uri SharedUri { get; }

        public X509Certificate Certificate { get; }

        /// <summary>
        /// Gets the base URI specific to this HTTP responder.
        /// </summary>
        public override Uri Url { get; }

        public OcspResponder OcspResponder => _ocspResponder.Value;
        public CertificateAuthority Parent { get; }

        public Uri CertificateUri { get; }
        public Uri OcspResponderUri { get; }
        internal AsymmetricCipherKeyPair KeyPair { get; }

        private CertificateAuthority(
            X509Certificate certificate,
            AsymmetricCipherKeyPair keyPair,
            Uri sharedUri,
            CertificateAuthority parentCa)
        {
            Certificate = certificate;
            KeyPair = keyPair;
            SharedUri = sharedUri;
            Url = GenerateRandomUri();
            var fingerprint = CertificateUtilities.GenerateFingerprint(certificate);
            CertificateUri = new Uri(Url, $"{fingerprint}.cer");
            OcspResponderUri = GenerateRandomUri();
            Parent = parentCa;
            _nextSerialNumber = certificate.SerialNumber.Add(BigInteger.One);
            _issuedCertificates = new Dictionary<BigInteger, X509Certificate>();
            _revokedCertificates = new Dictionary<BigInteger, RevocationInfo>();
            _ocspResponder = new Lazy<OcspResponder>(() => OcspResponder.Create(this));
        }

        public X509Certificate IssueCertificate(IssueCertificateOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Action<X509V3CertificateGenerator> customizeCertificate = generator =>
                {
                    generator.AddExtension(
                        X509Extensions.AuthorityInfoAccess,
                        critical: false,
                        extensionValue: new DerSequence(
                            new AccessDescription(AccessDescription.IdADOcsp,
                                new GeneralName(GeneralName.UniformResourceIdentifier, OcspResponderUri.OriginalString)),
                            new AccessDescription(AccessDescription.IdADCAIssuers,
                                new GeneralName(GeneralName.UniformResourceIdentifier, CertificateUri.OriginalString))));
                    generator.AddExtension(
                        X509Extensions.AuthorityKeyIdentifier,
                        critical: false,
                        extensionValue: new AuthorityKeyIdentifierStructure(Certificate));
                    generator.AddExtension(
                        X509Extensions.SubjectKeyIdentifier,
                        critical: false,
                        extensionValue: new SubjectKeyIdentifierStructure(options.KeyPair.Public));
                    generator.AddExtension(
                        X509Extensions.BasicConstraints,
                        critical: true,
                        extensionValue: new BasicConstraints(cA: false));
                };

            return IssueCertificate(options, customizeCertificate);
        }

        public CertificateAuthority CreateIntermediateCertificateAuthority(IssueCertificateOptions options = null)
        {
            options = options ?? IssueCertificateOptions.CreateDefaultForIntermediateCertificateAuthority();

            Action<X509V3CertificateGenerator> customizeCertificate = generator =>
                {
                    generator.AddExtension(
                        X509Extensions.AuthorityInfoAccess,
                        critical: false,
                        extensionValue: new DerSequence(
                            new AccessDescription(AccessDescription.IdADOcsp,
                                new GeneralName(GeneralName.UniformResourceIdentifier, OcspResponderUri.OriginalString)),
                            new AccessDescription(AccessDescription.IdADCAIssuers,
                                new GeneralName(GeneralName.UniformResourceIdentifier, CertificateUri.OriginalString))));
                    generator.AddExtension(
                        X509Extensions.AuthorityKeyIdentifier,
                        critical: false,
                        extensionValue: new AuthorityKeyIdentifierStructure(Certificate));
                    generator.AddExtension(
                        X509Extensions.SubjectKeyIdentifier,
                        critical: false,
                        extensionValue: new SubjectKeyIdentifierStructure(options.KeyPair.Public));
                    generator.AddExtension(
                        X509Extensions.BasicConstraints,
                        critical: true,
                        extensionValue: new BasicConstraints(cA: true));
                };

            var certificate = IssueCertificate(options, customizeCertificate);

            return new CertificateAuthority(certificate, options.KeyPair, SharedUri, parentCa: this);
        }

        public void Revoke(X509Certificate certificate, RevocationReason reason, DateTimeOffset revocationDate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (!_issuedCertificates.ContainsKey(certificate.SerialNumber))
            {
                throw new ArgumentException("Unknown serial number.", nameof(certificate));
            }

            if (_revokedCertificates.ContainsKey(certificate.SerialNumber))
            {
                throw new ArgumentException("Certificate already revoked.", nameof(certificate));
            }

            _revokedCertificates.Add(
                certificate.SerialNumber,
                new RevocationInfo(certificate.SerialNumber, revocationDate, reason));
        }

#if IS_SIGNING_SUPPORTED
        public override void Respond(HttpListenerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (IsGet(context.Request) &&
                string.Equals(context.Request.RawUrl, CertificateUri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
            {
                WriteResponseBody(context.Response, Certificate.GetEncoded());
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
#endif

        public static CertificateAuthority Create(Uri sharedUri, IssueCertificateOptions options = null)
        {
            if (sharedUri == null)
            {
                throw new ArgumentNullException(nameof(sharedUri));
            }

            if (!sharedUri.AbsoluteUri.EndsWith("/"))
            {
                sharedUri = new Uri($"{sharedUri.AbsoluteUri}/");
            }

            options = options ?? IssueCertificateOptions.CreateDefaultForRootCertificateAuthority();

            Action<X509V3CertificateGenerator> customizeCertificate = generator =>
                {
                    generator.AddExtension(
                        X509Extensions.SubjectKeyIdentifier,
                        critical: false,
                        extensionValue: new SubjectKeyIdentifierStructure(options.KeyPair.Public));
                    generator.AddExtension(
                        X509Extensions.BasicConstraints,
                        critical: true,
                        extensionValue: new BasicConstraints(cA: true));
                    generator.AddExtension(
                        X509Extensions.KeyUsage,
                        critical: true,
                        extensionValue: new KeyUsage(KeyUsage.DigitalSignature | KeyUsage.KeyCertSign | KeyUsage.CrlSign));
                };

            var signatureFactory = new Asn1SignatureFactory(options.SignatureAlgorithmName, options.IssuerPrivateKey);

            var certificate = CreateCertificate(
                options.KeyPair.Public,
                signatureFactory,
                BigInteger.One,
                options.SubjectName,
                options.SubjectName,
                options.NotBefore,
                options.NotAfter,
                options.CustomizeCertificate ?? customizeCertificate);

            return new CertificateAuthority(certificate, options.KeyPair, sharedUri, parentCa: null);
        }

        internal CertificateStatus GetStatus(CertificateID certificateId)
        {
            if (certificateId == null)
            {
                throw new ArgumentNullException(nameof(certificateId));
            }

            if (certificateId.MatchesIssuer(Certificate) &&
                _issuedCertificates.ContainsKey(certificateId.SerialNumber))
            {
                RevocationInfo revocationInfo;

                if (!_revokedCertificates.TryGetValue(certificateId.SerialNumber, out revocationInfo))
                {
                    return CertificateStatus.Good;
                }

                var datetimeString = DerGeneralizedTimeUtility.ToDerGeneralizedTimeString(revocationInfo.RevocationDate);

                // The DateTime constructor truncates fractional seconds;
                // however, the string constructor preserves full accuracy.
                var revocationDate = new DerGeneralizedTime(datetimeString);
                var reason = new CrlReason((int)revocationInfo.Reason);
                var revokedInfo = new RevokedInfo(revocationDate, reason);

                return new RevokedStatus(revokedInfo);
            }

            return new UnknownStatus();
        }

        internal Uri GenerateRandomUri()
        {
            using (var provider = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32];

                provider.GetBytes(bytes);

                var path = BitConverter.ToString(bytes).Replace("-", "");

                return new Uri(SharedUri, $"{path}/");
            }
        }

        private X509Certificate IssueCertificate(
            IssueCertificateOptions options,
            Action<X509V3CertificateGenerator> customizeCertificate)
        {
            var serialNumber = _nextSerialNumber;
            var issuerName = PrincipalUtilities.GetSubjectX509Principal(Certificate);
            var notAfter = options.NotAfter.UtcDateTime;

            // An issued certificate should not have a validity period beyond the issuer's validity period.
            if (notAfter > Certificate.NotAfter)
            {
                notAfter = Certificate.NotAfter;
            }

            var signatureFactory = new Asn1SignatureFactory(options.SignatureAlgorithmName, options.IssuerPrivateKey ?? KeyPair.Private);

            var certificate = CreateCertificate(
                options.KeyPair.Public,
                signatureFactory,
                serialNumber,
                issuerName,
                options.SubjectName,
                options.NotBefore.UtcDateTime,
                notAfter,
                options.CustomizeCertificate ?? customizeCertificate);

            _nextSerialNumber = _nextSerialNumber.Add(BigInteger.One);
            _issuedCertificates.Add(certificate.SerialNumber, certificate);

            return certificate;
        }

        private static X509Certificate CreateCertificate(
            AsymmetricKeyParameter certificatePublicKey,
            Asn1SignatureFactory signatureFactory,
            BigInteger serialNumber,
            X509Name issuerName,
            X509Name subjectName,
            DateTimeOffset notBefore,
            DateTimeOffset notAfter,
            Action<X509V3CertificateGenerator> customizeCertificate)
        {
            var generator = new X509V3CertificateGenerator();

            generator.SetSerialNumber(serialNumber);
            generator.SetIssuerDN(issuerName);
            generator.SetNotBefore(notBefore.UtcDateTime);
            generator.SetNotAfter(notAfter.UtcDateTime);
            generator.SetSubjectDN(subjectName);
            generator.SetPublicKey(certificatePublicKey);

            customizeCertificate(generator);

            return generator.Generate(signatureFactory);
        }

        private sealed class RevocationInfo
        {
            internal BigInteger SerialNumber { get; }
            internal DateTimeOffset RevocationDate { get; }
            internal RevocationReason Reason { get; }

            internal RevocationInfo(BigInteger serialNumber, DateTimeOffset revocationDate, RevocationReason reason)
            {
                SerialNumber = serialNumber;
                RevocationDate = revocationDate;
                Reason = reason;
            }
        }
    }
}
