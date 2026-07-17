// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Formats.Asn1;

namespace Microsoft.DotNet.Build.Tasks.FileCatalog
{
    /// <summary>
    /// DER-encodes the per-member ASN.1 structures inside a Microsoft Catalog (.cat) file,
    /// matching the layout produced by <c>makecat.exe</c> for a V2 (SHA-256) catalog.
    ///
    /// For each cataloged file, makecat emits two <c>TrustedSubject</c> members:
    /// one keyed by the raw SHA-1 hash and one keyed by the raw SHA-256 hash. Both carry a
    /// <c>CAT_MEMBERINFO2</c> marker attribute; the SHA-256 member additionally carries an
    /// <c>SPC_INDIRECT_DATA</c> attribute holding the SHA-256 digest.
    /// </summary>
    internal static class CatalogMemberEncoder
    {
        /// <summary>
        /// Writes a single <c>TrustedSubject</c> SEQUENCE.
        /// </summary>
        /// <param name="writer">The DER writer.</param>
        /// <param name="identifier">The raw hash bytes that identify the member.</param>
        /// <param name="sha256Digest">
        /// When non-null, the member is a SHA-256 member and also carries an
        /// <c>SPC_INDIRECT_DATA</c> attribute with this digest (equal to <paramref name="identifier"/>).
        /// When null, the member is a SHA-1 member carrying only <c>CAT_MEMBERINFO2</c>.
        /// </param>
        public static void WriteTrustedSubject(AsnWriter writer, byte[] identifier, byte[]? sha256Digest)
        {
            writer.PushSequence();

            // subjectIdentifier: OCTET STRING of the raw hash bytes.
            writer.WriteOctetString(identifier);

            // subjectAttributes: SET OF Attribute (DER-sorted by PushSetOf).
            writer.PushSetOf();

            WriteCatMemberInfo2Attribute(writer);
            if (sha256Digest is not null)
            {
                WriteSpcIndirectDataAttribute(writer, sha256Digest);
            }

            writer.PopSetOf();
            writer.PopSequence();
        }

        /// <summary>
        /// Writes the CAT_MEMBERINFO2 attribute:
        ///   Attribute { OID 1.3.6.1.4.1.311.12.2.3, SET { [2] &lt;empty&gt; } }
        /// (encoded bytes: 30 10 06 0a 2b 06 01 04 01 82 37 0c 02 03 31 02 82 00)
        /// </summary>
        private static void WriteCatMemberInfo2Attribute(AsnWriter writer)
        {
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.CatMemberInfo2);
            writer.PushSetOf();
            // A single context-[2] element with empty content ("82 00").
            writer.WriteNull(new Asn1Tag(TagClass.ContextSpecific, tagValue: 2));
            writer.PopSetOf();
            writer.PopSequence();
        }

        /// <summary>
        /// Writes the SPC_INDIRECT_DATA attribute carrying the SHA-256 file hash:
        ///   Attribute { OID 1.3.6.1.4.1.311.2.1.4,
        ///               SET { SEQUENCE {
        ///                 data   SEQUENCE { OID 1.3.6.1.4.1.311.2.1.25, [2] { [0] &lt;empty&gt; } },
        ///                 digest SEQUENCE { SEQUENCE { OID sha256, NULL }, OCTET STRING &lt;hash&gt; } } } }
        /// </summary>
        private static void WriteSpcIndirectDataAttribute(AsnWriter writer, byte[] sha256Digest)
        {
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.SpcIndirectData);
            writer.PushSetOf();

            // SpcIndirectDataContent
            writer.PushSequence();

            // data: SpcAttributeTypeAndOptionalValue { type, value }
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.SpcLinkData);
            // value: file [2] EXPLICIT { unicode [0] IMPLICIT BMPString "" }  ->  a2 02 80 00
            Asn1Tag fileTag = new(TagClass.ContextSpecific, tagValue: 2, isConstructed: true);
            writer.PushSequence(fileTag);
            Asn1Tag unicodeTag = new(TagClass.ContextSpecific, tagValue: 0, isConstructed: false);
            writer.WriteCharacterString(UniversalTagNumber.BMPString, string.Empty, unicodeTag);
            writer.PopSequence(fileTag);
            writer.PopSequence();

            // messageDigest: DigestInfo { AlgorithmIdentifier { OID sha256, NULL }, OCTET STRING digest }
            writer.PushSequence();
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.Sha256);
            writer.WriteNull();
            writer.PopSequence();
            writer.WriteOctetString(sha256Digest);
            writer.PopSequence();

            writer.PopSequence(); // SpcIndirectDataContent

            writer.PopSetOf();
            writer.PopSequence();
        }
    }
}
