// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.DotNet.Build.Tasks.FileCatalog
{
    /// <summary>
    /// Builds a Microsoft Catalog (.cat) file in pure managed code — no <c>makecat.exe</c>,
    /// no Windows SDK, no P/Invoke, cross-platform.
    ///
    /// The output is an <em>unsigned</em> PKCS#7 SignedData envelope (no signers, no
    /// certificates) whose encapsulated content is a Microsoft <c>CertificateTrustList</c>
    /// (szOID_CTL). This matches the artifact <c>makecat.exe</c> produces from a
    /// <c>CatalogVersion=2 / HashAlgorithms=SHA256</c> <c>.cdf</c>: a catalog ready to be
    /// Authenticode-signed by <c>signtool.exe</c> or the Arcade signing infrastructure
    /// (via <c>FileExtensionSignInfo</c>).
    ///
    /// For each cataloged file, two <c>TrustedSubject</c> members are emitted — one keyed by
    /// the raw SHA-1 hash and one by the raw SHA-256 hash — matching makecat V2 output.
    /// The output is fully deterministic: given the same inputs it is byte-for-byte identical.
    /// </summary>
    public sealed class CatalogBuilder
    {
        // Fixed, stable timestamp so catalogs are reproducible. makecat writes the build
        // time here, but the value is not meaningful for signing/verification.
        private static readonly DateTimeOffset s_defaultEffectiveTime =
            new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

        private readonly List<CatalogEntry> _entries = new();

        /// <summary>
        /// Catalog "this update" timestamp (<c>ctlThisUpdate</c>). Defaults to a fixed value
        /// so output is deterministic; override only when a specific timestamp is required.
        /// </summary>
        public DateTimeOffset? EffectiveTime { get; set; }

        /// <summary>
        /// Optional explicit 16-byte list identifier. When null (the default), a deterministic
        /// identifier is derived from the catalog members so output stays reproducible.
        /// </summary>
        public byte[]? ListIdentifier
        {
            get => _listIdentifier;
            set
            {
                if (value is not null && value.Length != ListIdentifierLength)
                {
                    throw new ArgumentException(
                        $"List identifier must be exactly {ListIdentifierLength} bytes.", nameof(value));
                }

                _listIdentifier = value;
            }
        }
        private byte[]? _listIdentifier;

        private const int ListIdentifierLength = 16;

        /// <summary>Read-only view of the entries that have been added.</summary>
        public IReadOnlyList<CatalogEntry> Entries => _entries;

        /// <summary>Adds an in-memory entry.</summary>
        public CatalogBuilder Add(CatalogEntry entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            _entries.Add(entry);
            return this;
        }

        /// <summary>Adds a file by path.</summary>
        public CatalogBuilder AddFile(string filePath, string? name = null)
            => Add(CatalogEntry.FromFile(filePath, name));

        /// <summary>Builds the DER-encoded .cat file bytes.</summary>
        public byte[] Build()
        {
            if (_entries.Count == 0)
            {
                throw new InvalidOperationException("Catalog must contain at least one entry.");
            }

            List<Member> members = BuildSortedMembers();
            byte[] ctl = EncodeCertificateTrustList(members);
            return EncodePkcs7SignedDataEnvelope(ctl);
        }

        /// <summary>Builds and writes the catalog to <paramref name="path"/>.</summary>
        public void WriteTo(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path must not be null or empty.", nameof(path));
            }

            File.WriteAllBytes(path, Build());
        }

        private List<Member> BuildSortedMembers()
        {
            List<Member> members = new(_entries.Count * 2);
            foreach (CatalogEntry entry in _entries)
            {
                // makecat V2 catalogs emit two members per file so the file is found
                // regardless of which hash the verifying component looks it up by: older
                // catalog/Authenticode code paths compute a SHA-1 file "tag" and search for a
                // SHA-1-keyed member, while newer ones use SHA-256. The SHA-1 member is purely
                // a legacy lookup key/thumbprint here - it is NOT a signature and carries no
                // digest, so SHA-1's cryptographic weakness (collision attacks) is irrelevant:
                // trust comes solely from the Authenticode signature applied over the whole
                // catalog afterward, which uses SHA-256.

                // SHA-1 member: identified by the raw SHA-1 hash, marker attribute only.
                members.Add(new Member(entry.Sha1, sha256Digest: null));

                // SHA-256 member: identified by the raw SHA-256 hash, carries the digest.
                members.Add(new Member(entry.Sha256, sha256Digest: entry.Sha256));
            }

            // makecat emits members sorted ascending by their raw identifier bytes,
            // intermixing the SHA-1 and SHA-256 members, and de-duplicates members that
            // share an identifier (e.g. files with identical content).
            members.Sort(static (a, b) => CompareBytes(a.Identifier, b.Identifier));

            var deduped = new List<Member>(members.Count);
            foreach (Member member in members)
            {
                if (deduped.Count == 0 ||
                    CompareBytes(deduped[deduped.Count - 1].Identifier, member.Identifier) != 0)
                {
                    deduped.Add(member);
                }
            }

            return deduped;
        }

        private byte[] EncodeCertificateTrustList(List<Member> members)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            writer.PushSequence();

            // version: DEFAULT v1 — omitted.

            // subjectUsage: SEQUENCE { OID szOID_CATALOG_LIST }
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.CatalogList);
            writer.PopSequence();

            // listIdentifier: OCTET STRING (deterministic unless caller overrides).
            writer.WriteOctetString(ListIdentifier ?? DeriveListIdentifier(members));

            // sequenceNumber: omitted.

            // ctlThisUpdate: UTCTime
            writer.WriteUtcTime(EffectiveTime ?? s_defaultEffectiveTime);

            // ctlNextUpdate: omitted.

            // subjectAlgorithm: AlgorithmIdentifier { OID szOID_CATALOG_LIST_MEMBER_V2, NULL }
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.CatalogListMemberV2);
            writer.WriteNull();
            writer.PopSequence();

            // trustedSubjects: SEQUENCE OF TrustedSubject
            writer.PushSequence();
            foreach (Member member in members)
            {
                CatalogMemberEncoder.WriteTrustedSubject(writer, member.Identifier, member.Sha256Digest);
            }
            writer.PopSequence();

            // ctlExtensions [0] EXPLICIT — omitted for a minimal catalog.

            writer.PopSequence();

            return writer.Encode();
        }

        private static byte[] EncodePkcs7SignedDataEnvelope(byte[] ctlContent)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            // ContentInfo SEQUENCE
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.SignedData);

            // content [0] EXPLICIT SignedData
            Asn1Tag explicit0 = new(TagClass.ContextSpecific, tagValue: 0, isConstructed: true);
            writer.PushSequence(explicit0);

            // SignedData SEQUENCE
            writer.PushSequence();

            // version INTEGER 1
            writer.WriteInteger(1);

            // digestAlgorithms SET OF AlgorithmIdentifier — empty (no signer yet).
            writer.PushSetOf();
            writer.PopSetOf();

            // encapContentInfo: SEQUENCE { OID szOID_CTL, [0] EXPLICIT <CTL DER bytes> }
            writer.PushSequence();
            writer.WriteObjectIdentifier(CatalogOids.Ctl);
            writer.PushSequence(explicit0);
            // The Microsoft catalog format places the CertificateTrustList DER directly here,
            // not wrapped in an OCTET STRING as generic CMS content would be.
            writer.WriteEncodedValue(ctlContent);
            writer.PopSequence(explicit0);
            writer.PopSequence();

            // certificates [0] IMPLICIT / crls [1] IMPLICIT — omitted (added at signing time).

            // signerInfos SET OF SignerInfo — empty (added at signing time).
            writer.PushSetOf();
            writer.PopSetOf();

            writer.PopSequence(); // SignedData
            writer.PopSequence(explicit0); // [0] EXPLICIT
            writer.PopSequence(); // ContentInfo

            return writer.Encode();
        }

        /// <summary>
        /// Derives a stable 16-byte list identifier from the (already sorted) member
        /// identifiers, so the catalog is reproducible without a random value.
        /// </summary>
        private static byte[] DeriveListIdentifier(List<Member> members)
        {
            using var sha256 = SHA256.Create();
            foreach (Member member in members)
            {
                sha256.TransformBlock(member.Identifier, 0, member.Identifier.Length, null, 0);
            }

            sha256.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

            byte[] identifier = new byte[ListIdentifierLength];
            Array.Copy(sha256.Hash!, identifier, ListIdentifierLength);
            return identifier;
        }

        private static int CompareBytes(byte[] x, byte[] y)
        {
            int min = Math.Min(x.Length, y.Length);
            for (int i = 0; i < min; i++)
            {
                int diff = x[i] - y[i];
                if (diff != 0)
                {
                    return diff;
                }
            }

            return x.Length - y.Length;
        }

        private readonly struct Member
        {
            public Member(byte[] identifier, byte[]? sha256Digest)
            {
                Identifier = identifier;
                Sha256Digest = sha256Digest;
            }

            public byte[] Identifier { get; }

            public byte[]? Sha256Digest { get; }
        }
    }
}
