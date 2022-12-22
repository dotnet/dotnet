// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tsp;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using Org.BouncyCastle.X509.Store;
using BcAttribute = Org.BouncyCastle.Asn1.Cms.Attribute;
using BcAttributeTable = Org.BouncyCastle.Asn1.Cms.AttributeTable;
using BcContentInfo = Org.BouncyCastle.Asn1.Cms.ContentInfo;

namespace Test.Utility.Signing
{
    // https://tools.ietf.org/html/rfc3161
    public sealed class TimestampService : HttpResponder
    {
        private const string RequestContentType = "application/timestamp-query";
        private const string ResponseContentType = "application/timestamp-response";

        private readonly AsymmetricCipherKeyPair _keyPair;
        private readonly TimestampServiceOptions _options;
        private readonly HashSet<BigInteger> _serialNumbers;
        private BigInteger _nextSerialNumber;

        /// <summary>
        /// Gets this certificate authority's certificate.
        /// </summary>
        public X509Certificate Certificate { get; }

        /// <summary>
        /// Gets the base URI specific to this HTTP responder.
        /// </summary>
        public override Uri Url { get; }

        /// <summary>
        /// Gets the issuing certificate authority.
        /// </summary>
        public CertificateAuthority CertificateAuthority { get; }

        private TimestampService(
            CertificateAuthority certificateAuthority,
            X509Certificate certificate,
            AsymmetricCipherKeyPair keyPair,
            Uri uri,
            TimestampServiceOptions options)
        {
            CertificateAuthority = certificateAuthority;
            Certificate = certificate;
            _keyPair = keyPair;
            Url = uri;
            _serialNumbers = new HashSet<BigInteger>();
            _nextSerialNumber = BigInteger.One;
            _options = options;
        }

        public static TimestampService Create(
            CertificateAuthority certificateAuthority,
            TimestampServiceOptions serviceOptions = null,
            IssueCertificateOptions issueCertificateOptions = null)
        {
            if (certificateAuthority == null)
            {
                throw new ArgumentNullException(nameof(certificateAuthority));
            }

            serviceOptions = serviceOptions ?? new TimestampServiceOptions();

            if (issueCertificateOptions == null)
            {
                issueCertificateOptions = IssueCertificateOptions.CreateDefaultForTimestampService();
            }

            void customizeCertificate(X509V3CertificateGenerator generator)
            {
                generator.AddExtension(
                    X509Extensions.AuthorityInfoAccess,
                    critical: false,
                    extensionValue: new DerSequence(
                        new AccessDescription(AccessDescription.IdADOcsp,
                            new GeneralName(GeneralName.UniformResourceIdentifier, certificateAuthority.OcspResponderUri.OriginalString)),
                        new AccessDescription(AccessDescription.IdADCAIssuers,
                            new GeneralName(GeneralName.UniformResourceIdentifier, certificateAuthority.CertificateUri.OriginalString))));
                generator.AddExtension(
                    X509Extensions.AuthorityKeyIdentifier,
                    critical: false,
                    extensionValue: new AuthorityKeyIdentifierStructure(certificateAuthority.Certificate));
                generator.AddExtension(
                    X509Extensions.SubjectKeyIdentifier,
                    critical: false,
                    extensionValue: new SubjectKeyIdentifierStructure(issueCertificateOptions.KeyPair.Public));
                generator.AddExtension(
                    X509Extensions.BasicConstraints,
                    critical: true,
                    extensionValue: new BasicConstraints(cA: false));
                generator.AddExtension(
                    X509Extensions.KeyUsage,
                    critical: true,
                    extensionValue: new KeyUsage(KeyUsage.DigitalSignature));
                generator.AddExtension(
                    X509Extensions.ExtendedKeyUsage,
                    critical: true,
                    extensionValue: ExtendedKeyUsage.GetInstance(new DerSequence(KeyPurposeID.IdKPTimeStamping)));
            }

            if (issueCertificateOptions.CustomizeCertificate == null)
            {
                issueCertificateOptions.CustomizeCertificate = customizeCertificate;
            }

            if (serviceOptions.IssuedCertificateNotBefore.HasValue)
            {
                issueCertificateOptions.NotBefore = serviceOptions.IssuedCertificateNotBefore.Value;
            }

            if (serviceOptions.IssuedCertificateNotAfter.HasValue)
            {
                issueCertificateOptions.NotAfter = serviceOptions.IssuedCertificateNotAfter.Value;
            }

            var certificate = certificateAuthority.IssueCertificate(issueCertificateOptions);
            var uri = certificateAuthority.GenerateRandomUri();

            return new TimestampService(certificateAuthority, certificate, issueCertificateOptions.KeyPair, uri, serviceOptions);
        }

#if IS_SIGNING_SUPPORTED
        public override void Respond(HttpListenerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!string.Equals(context.Request.ContentType, RequestContentType, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 400;

                return;
            }

            var bytes = ReadRequestBody(context.Request);
            var request = new TimeStampRequest(bytes);
            PkiStatusInfo statusInfo;
            BcContentInfo timeStampToken = null;

            if (_options.ReturnFailure)
            {
                statusInfo = new PkiStatusInfo(
                    (int)PkiStatus.Rejection,
                    new PkiFreeText(new DerUtf8String("Unsupported algorithm")),
                    new PkiFailureInfo(PkiFailureInfo.BadAlg));
            }
            else
            {
                statusInfo = new PkiStatusInfo((int)PkiStatus.Granted);

                var generalizedTime = DateTime.UtcNow;

                if (_options.GeneralizedTime.HasValue)
                {
                    generalizedTime = _options.GeneralizedTime.Value.UtcDateTime;
                }

                CmsSignedData timestamp = GenerateTimestamp(request, _nextSerialNumber, generalizedTime);

                timeStampToken = timestamp.ContentInfo;
            }

            _serialNumbers.Add(_nextSerialNumber);
            _nextSerialNumber = _nextSerialNumber.Add(BigInteger.One);

            context.Response.ContentType = ResponseContentType;

            var response = new TimeStampResp(statusInfo, timeStampToken);

            WriteResponseBody(context.Response, response.GetEncoded());
        }

        private CmsSignedData GenerateTimestamp(
            TimeStampRequest request,
            BigInteger serialNumber,
            DateTime generalizedTime)
        {
            var messageImprint = new MessageImprint(
                new AlgorithmIdentifier(
                    new DerObjectIdentifier(request.MessageImprintAlgOid)), request.GetMessageImprintDigest());
            DerInteger nonce = request.Nonce == null ? null : new DerInteger(request.Nonce);

            var tstInfo = new TstInfo(
                new DerObjectIdentifier(_options.Policy.Value),
                messageImprint,
                new DerInteger(serialNumber),
                new DerGeneralizedTime(generalizedTime),
                _options.Accuracy,
                DerBoolean.False,
                nonce,
                tsa: null,
                extensions: null);

            var content = new CmsProcessableByteArray(tstInfo.GetEncoded());
            var signedAttributes = new Asn1EncodableVector();
            var certificateBytes = new Lazy<byte[]>(() => Certificate.GetEncoded());

            if (_options.SigningCertificateUsage.HasFlag(SigningCertificateUsage.V1))
            {
                byte[] hash = _options.SigningCertificateV1Hash ?? DigestUtilities.CalculateDigest("SHA-1", certificateBytes.Value);
                var signingCertificate = new SigningCertificate(new EssCertID(hash));
                var attributeValue = new DerSet(signingCertificate);
                var attribute = new BcAttribute(PkcsObjectIdentifiers.IdAASigningCertificate, attributeValue);

                signedAttributes.Add(attribute);
            }

            if (_options.SigningCertificateUsage.HasFlag(SigningCertificateUsage.V2))
            {
                byte[] hash = DigestUtilities.CalculateDigest("SHA-256", certificateBytes.Value);
                var signingCertificateV2 = new SigningCertificateV2(new EssCertIDv2(hash));
                var attributeValue = new DerSet(signingCertificateV2);
                var attribute = new BcAttribute(PkcsObjectIdentifiers.IdAASigningCertificateV2, attributeValue);

                signedAttributes.Add(attribute);
            }

            var generator = new CmsSignedDataGenerator();

            if (_options.ReturnSigningCertificate)
            {
                var certificates = X509StoreFactory.Create(
                    "Certificate/Collection",
                    new X509CollectionStoreParameters(new[] { Certificate }));

                generator.AddCertificates(certificates);
            }

            generator.AddSigner(
                _keyPair.Private,
                Certificate,
                _options.SignatureHashAlgorithm.Value,
                new BcAttributeTable(signedAttributes),
                new BcAttributeTable(DerSet.Empty));

            CmsSignedData signedCms = generator.Generate(
                PkcsObjectIdentifiers.IdCTTstInfo.Id,
                content,
                encapsulate: true);

            return signedCms;
        }
#endif
    }
}
