// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;
#endif
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    public static class AttributeUtility
    {
#if IS_SIGNING_SUPPORTED
        /// <summary>
        /// Create a CommitmentTypeIndication attribute.
        /// https://tools.ietf.org/html/rfc5126.html#section-5.11.1
        /// </summary>
        /// <param name="type">The signature type.</param>
        public static CryptographicAttributeObject CreateCommitmentTypeIndication(SignatureType type)
        {
            // SignatureType -> Oid
            var oid = GetSignatureTypeOid(type);

            var commitmentTypeIndication = CommitmentTypeIndication.Create(new Oid(oid));
            var value = new AsnEncodedData(Oids.CommitmentTypeIndication, commitmentTypeIndication.Encode());

            return new CryptographicAttributeObject(
                new Oid(Oids.CommitmentTypeIndication),
                new AsnEncodedDataCollection(value));
        }

        /// <summary>
        /// Gets the signature type from one or more commitment-type-indication attributes.
        /// </summary>
        /// <param name="signedAttributes">A <see cref="SignerInfo" /> signed attributes collection.</param>
        /// <remarks>Unknown OIDs are ignored.</remarks>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signedAttributes" /> is <see langword="null" />.</exception>
        /// <exception cref="SignatureException">Thrown if one or more attributes are invalid.</exception>
        public static SignatureType GetSignatureType(CryptographicAttributeObjectCollection signedAttributes)
        {
            if (signedAttributes == null)
            {
                throw new ArgumentNullException(nameof(signedAttributes));
            }

            var values = signedAttributes.GetAttributes(Oids.CommitmentTypeIndication)
                .SelectMany(attribute => GetCommitmentTypeIndicationRawValues(attribute))
                .Distinct();

            // Remove unknown values, these could be future values.
            var knownValues = values.Where(e => e != SignatureType.Unknown).ToList();

            if (knownValues.Count == 0)
            {
                return SignatureType.Unknown;
            }

            // Author and repository values are mutually exclusive in the same signature.
            // If multiple distinct known values exist then the attribute is invalid.
            if (knownValues.Count > 1)
            {
                throw new SignatureException(Strings.CommitmentTypeIndicationAttributeInvalidCombination);
            }

            return knownValues[0];
        }

        /// <summary>
        /// Creates a nuget-v3-service-index-url attribute.
        /// </summary>
        /// <param name="v3ServiceIndexUrl">The V3 service index HTTPS URL.</param>
        /// <returns>An attribute object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="v3ServiceIndexUrl" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="v3ServiceIndexUrl" /> is neither absolute
        /// nor HTTPS.</exception>
        public static CryptographicAttributeObject CreateNuGetV3ServiceIndexUrl(Uri v3ServiceIndexUrl)
        {
            if (v3ServiceIndexUrl == null)
            {
                throw new ArgumentNullException(nameof(v3ServiceIndexUrl));
            }

            if (!v3ServiceIndexUrl.IsAbsoluteUri)
            {
                throw new ArgumentException(Strings.InvalidUrl, nameof(v3ServiceIndexUrl));
            }

            if (!string.Equals(v3ServiceIndexUrl.Scheme, "https", StringComparison.Ordinal))
            {
                throw new ArgumentException(Strings.InvalidUrl, nameof(v3ServiceIndexUrl));
            }

            var nugetV3ServiceIndexUrl = new NuGetV3ServiceIndexUrl(v3ServiceIndexUrl);
            var bytes = nugetV3ServiceIndexUrl.Encode();

            return new CryptographicAttributeObject(
                new Oid(Oids.NuGetV3ServiceIndexUrl),
                new AsnEncodedDataCollection(new AsnEncodedData(Oids.NuGetV3ServiceIndexUrl, bytes)));
        }

        /// <summary>
        /// Gets the V3 service index HTTPS URL from the nuget-v3-service-index-url attribute.
        /// </summary>
        /// <param name="signedAttributes">A <see cref="SignerInfo" /> signed attributes collection.</param>
        /// <returns>The V3 service index HTTPS URL.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signedAttributes" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="SignatureException">Thrown if either exactly one attribute is not present or if
        /// the attribute does not contain exactly one attribute value.</exception>
        public static Uri GetNuGetV3ServiceIndexUrl(CryptographicAttributeObjectCollection signedAttributes)
        {
            if (signedAttributes == null)
            {
                throw new ArgumentNullException(nameof(signedAttributes));
            }

            const string attributeName = "nuget-v3-service-index-url";

            var attribute = signedAttributes.GetAttribute(Oids.NuGetV3ServiceIndexUrl);

            if (attribute == null)
            {
                throw new SignatureException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.ExactlyOneAttributeRequired,
                        attributeName));
            }

            if (attribute.Values.Count != 1)
            {
                throw new SignatureException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.ExactlyOneAttributeValueRequired,
                        attributeName));
            }

            var nugetV3ServiceIndexUrl = NuGetV3ServiceIndexUrl.Read(attribute.Values[0].RawData);

            return nugetV3ServiceIndexUrl.V3ServiceIndexUrl;
        }

        /// <summary>
        /// Creates a nuget-package-owners attribute.
        /// </summary>
        /// <param name="packageOwners">A read-only list of package owners.</param>
        /// <returns>An attribute object.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageOwners" /> is either <see langword="null" />
        /// or empty or if any package owner name is invalid.</exception>
        public static CryptographicAttributeObject CreateNuGetPackageOwners(IReadOnlyList<string> packageOwners)
        {
            if (packageOwners == null || packageOwners.Count == 0)
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageOwners));
            }

            if (packageOwners.Any(packageOwner => string.IsNullOrWhiteSpace(packageOwner)))
            {
                throw new ArgumentException(Strings.NuGetPackageOwnersInvalidValue, nameof(packageOwners));
            }

            var nugetPackageOwners = new NuGetPackageOwners(packageOwners);
            var bytes = nugetPackageOwners.Encode();

            return new CryptographicAttributeObject(
                new Oid(Oids.NuGetPackageOwners),
                new AsnEncodedDataCollection(new AsnEncodedData(Oids.NuGetPackageOwners, bytes)));
        }

        /// <summary>
        /// Gets a read-only list of package owners from an optional nuget-package-owners attribute.
        /// </summary>
        /// <param name="signedAttributes">A <see cref="SignerInfo" /> signed attributes collection.</param>
        /// <returns>A read-only list of package owners or <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signedAttributes" /> is <see langword="null" />.</exception>
        /// <exception cref="SignatureException">Thrown if the attribute does not contain exactly one
        /// attribute value.</exception>
        public static IReadOnlyList<string> GetNuGetPackageOwners(CryptographicAttributeObjectCollection signedAttributes)
        {
            if (signedAttributes == null)
            {
                throw new ArgumentNullException(nameof(signedAttributes));
            }

            var attribute = signedAttributes.GetAttribute(Oids.NuGetPackageOwners);

            if (attribute == null)
            {
                return null;
            }

            if (attribute.Values.Count != 1)
            {
                throw new SignatureException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.ExactlyOneAttributeValueRequired,
                        "nuget-package-owners"));
            }

            var nugetPackageOwners = NuGetPackageOwners.Read(attribute.Values[0].RawData);

            return nugetPackageOwners.PackageOwners;
        }

        /// <summary>
        /// Oid -> SignatureType
        /// </summary>
        /// <param name="oid">The commitment-type-indication value.</param>
        public static SignatureType GetSignatureType(string oid)
        {
            switch (oid)
            {
                case Oids.CommitmentTypeIdentifierProofOfOrigin:
                    return SignatureType.Author;
                case Oids.CommitmentTypeIdentifierProofOfReceipt:
                    return SignatureType.Repository;
                default:
                    return SignatureType.Unknown;
            }
        }

        /// <summary>
        /// SignatureType -> Oid
        /// </summary>
        /// <param name="signatureType">The signature type.</param>
        public static string GetSignatureTypeOid(SignatureType signatureType)
        {
            switch (signatureType)
            {
                case SignatureType.Author:
                    return Oids.CommitmentTypeIdentifierProofOfOrigin;
                case SignatureType.Repository:
                    return Oids.CommitmentTypeIdentifierProofOfReceipt;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(signatureType));
            }
        }

        /// <summary>
        /// Create a signing-certificate-v2 from a certificate.
        /// </summary>
        /// <param name="certificate">The signing certificate.</param>
        /// <param name="hashAlgorithm">The hash algorithm for the signing-certificate-v2 attribute.</param>
        public static CryptographicAttributeObject CreateSigningCertificateV2(
            X509Certificate2 certificate,
            Common.HashAlgorithmName hashAlgorithm)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var signingCertificateV2 = SigningCertificateV2.Create(certificate, hashAlgorithm);
            var bytes = signingCertificateV2.Encode();

            var data = new AsnEncodedData(Oids.SigningCertificateV2, bytes);

            return new CryptographicAttributeObject(
                new Oid(Oids.SigningCertificateV2),
                new AsnEncodedDataCollection(data));
        }

        /// <summary>
        /// Returns the first attribute if the Oid is found.
        /// Returns null if the attribute is not found.
        /// </summary>
        internal static CryptographicAttributeObject GetAttributeOrDefault(this CryptographicAttributeObjectCollection attributes, string oid)
        {
            if (oid == null)
            {
                throw new ArgumentNullException(nameof(oid));
            }

            foreach (var attribute in attributes)
            {
                if (StringComparer.Ordinal.Equals(oid, attribute.Oid.Value))
                {
                    return attribute;
                }
            }

            return null;
        }

        /// <summary>
        /// Throw a signature exception due to an invalid attribute. This is used for unusual situations
        /// where the format is corrupt.
        /// </summary>
        private static void ThrowInvalidAttributeException(CryptographicAttributeObject attribute)
        {
            throw new SignatureException(string.Format(CultureInfo.CurrentCulture, Strings.SignatureContainsInvalidAttribute, attribute.Oid.Value));
        }

        /// <summary>
        /// Enumerate AsnEncodedDataCollection
        /// </summary>
        private static List<AsnEncodedData> ToList(this AsnEncodedDataCollection collection)
        {
            var values = new List<AsnEncodedData>();

            foreach (var value in collection)
            {
                values.Add(value);
            }

            return values;
        }

        /// <summary>
        /// Attribute -> SignatureType values with no validation.
        /// </summary>
        private static IEnumerable<SignatureType> GetCommitmentTypeIndicationRawValues(CryptographicAttributeObject attribute)
        {
            // Most packages should have either 0 or 1 signature types.
            var values = new List<SignatureType>(capacity: 1);

            foreach (var value in attribute.Values)
            {
                var indication = CommitmentTypeIndication.Read(value.RawData);
                var signatureType = GetSignatureType(indication.CommitmentTypeId.Value);

                values.Add(signatureType);
            }

            return values;
        }

        /// <summary>
        /// Gets 0 or 1 attribute with the specified OID.  If more than one attribute is found, an exception is thrown.
        /// </summary>
        /// <param name="attributes">A collection of attributes.</param>
        /// <param name="oid">The attribute OID to search for.</param>
        /// <returns>Either a <see cref="CryptographicAttributeObject" /> or <see langword="null" />, if no attribute was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attributes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="oid" /> is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="CryptographicException">Thrown if multiple attribute instances with the specified OID were found.</exception>
        public static CryptographicAttributeObject GetAttribute(this CryptographicAttributeObjectCollection attributes, string oid)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            if (string.IsNullOrEmpty(oid))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(oid));
            }

            var matches = attributes.GetAttributes(oid);
            var instanceCount = matches.Count();

            if (instanceCount == 0)
            {
                return null;
            }

            if (instanceCount > 1)
            {
                throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Strings.MultipleAttributesDisallowed, oid));
            }

            return matches.Single();
        }

        /// <summary>
        /// Gets 0 or 1 or many attributes with the specified OID.
        /// </summary>
        /// <param name="attributes">A collection of attributes.</param>
        /// <param name="oid">The attribute OID to search for.</param>
        /// <returns>Either a <see cref="CryptographicAttributeObject" /> or <see langword="null" />, if no attribute was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attributes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="oid" /> is either <see langword="null" /> or an empty string.</exception>
        public static IEnumerable<CryptographicAttributeObject> GetAttributes(this CryptographicAttributeObjectCollection attributes, string oid)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            if (string.IsNullOrEmpty(oid))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(oid));
            }

            return attributes.Cast<CryptographicAttributeObject>()
                .Where(attribute => attribute.Oid.Value == oid);
        }
#endif
    }
}
