// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.FileCatalog;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.FileCatalog.Tests
{
    public class CatalogTests
    {
        // OIDs expected in the envelope / CTL.
        private const string SignedDataOid = "1.2.840.113549.1.7.2";
        private const string CtlOid = "1.3.6.1.4.1.311.10.1";
        private const string CatalogListOid = "1.3.6.1.4.1.311.12.1.1";
        private const string CatalogListMemberV2Oid = "1.3.6.1.4.1.311.12.1.3";

        // A real 25-byte file from the emsdk SDK pack (source-map-support/register.js):
        //   require('./').install();\n
        private static readonly byte[] s_registerJsContent =
            Convert.FromHexString("7265717569726528272e2f27292e696e7374616c6c28293b0a");

        // The exact TrustedSubject member DER blobs that makecat.exe emitted for that file
        // in emsdk's shipped emscripten-js.cat. These are the ground-truth fixtures the
        // managed encoder must reproduce byte-for-byte.
        private const string GoldenSha1MemberHex =
            "302A0414CE38E0912F3260BFB493DCEFA1717239DF3B969231123010060A2B0601040182370C020331028200";
        private const string GoldenSha256MemberHex =
            "30818D0420E07D374C1218641C6F50CD4DA6EB202F3E825BD3658AECB369C434582CC468CD31693010060A2B0601" +
            "040182370C0203310282003055060A2B060104018237020104314730453010060A2B060104018237020119A2028000" +
            "3031300D060960864801650304020105000420E07D374C1218641C6F50CD4DA6EB202F3E825BD3658AECB369C43458" +
            "2CC468CD";

        [Fact]
        public void Build_MatchesRealMakecatMembers_ByteForByte()
        {
            byte[] cat = new CatalogBuilder()
                .Add(new CatalogEntry("register.js", s_registerJsContent))
                .Build();

            Dictionary<string, byte[]> members = ExtractMembers(cat);

            string sha1Id = Convert.ToHexString(SHA1.HashData(s_registerJsContent));
            string sha256Id = Convert.ToHexString(SHA256.HashData(s_registerJsContent));

            members.Should().ContainKey(sha1Id);
            members.Should().ContainKey(sha256Id);
            Convert.ToHexString(members[sha1Id]).Should().Be(GoldenSha1MemberHex);
            Convert.ToHexString(members[sha256Id]).Should().Be(GoldenSha256MemberHex);
        }

        [Fact]
        public void Build_ProducesUnsignedPkcs7CtlEnvelope()
        {
            byte[] cat = new CatalogBuilder()
                .Add(new CatalogEntry("a", new byte[] { 1, 2, 3 }))
                .Build();

            AsnReader contentInfo = new AsnReader(cat, AsnEncodingRules.DER).ReadSequence();
            contentInfo.ReadObjectIdentifier().Should().Be(SignedDataOid);

            AsnReader signedData = contentInfo
                .ReadSequence(new Asn1Tag(TagClass.ContextSpecific, 0, true))
                .ReadSequence();
            signedData.ReadInteger().Should().Be(1);

            // digestAlgorithms and signerInfos are empty in an unsigned catalog.
            signedData.ReadSetOf().HasData.Should().BeFalse();

            AsnReader encap = signedData.ReadSequence();
            encap.ReadObjectIdentifier().Should().Be(CtlOid);
            // remaining content of SignedData is the (empty) signerInfos SET after encap.
            AsnReader signerInfos = signedData.ReadSetOf();
            signerInfos.HasData.Should().BeFalse();

            AsnReader ctl = encap.ReadSequence(new Asn1Tag(TagClass.ContextSpecific, 0, true)).ReadSequence();
            AsnReader subjectUsage = ctl.ReadSequence();
            subjectUsage.ReadObjectIdentifier().Should().Be(CatalogListOid);
        }

        [Fact]
        public void Build_EmitsSha1AndSha256MemberPerFile()
        {
            byte[] content = new byte[] { 10, 20, 30, 40 };
            byte[] cat = new CatalogBuilder().Add(new CatalogEntry("f", content)).Build();

            Dictionary<string, byte[]> members = ExtractMembers(cat);

            members.Should().HaveCount(2);
            members.Should().ContainKey(Convert.ToHexString(SHA1.HashData(content)));
            members.Should().ContainKey(Convert.ToHexString(SHA256.HashData(content)));
        }

        [Fact]
        public void Members_AreSortedByIdentifierBytes()
        {
            var builder = new CatalogBuilder();
            for (int i = 0; i < 8; i++)
            {
                builder.Add(new CatalogEntry($"f{i}", new byte[] { (byte)i, 0xAB, 0xCD }));
            }

            List<string> ids = ExtractMembers(builder.Build()).Keys.ToList();

            ids.Should().BeInAscendingOrder(StringComparer.Ordinal);
        }

        [Fact]
        public void Build_IsDeterministic_AcrossRuns()
        {
            static byte[] BuildCat()
            {
                return new CatalogBuilder()
                    .Add(new CatalogEntry("a", new byte[] { 1 }))
                    .Add(new CatalogEntry("b", new byte[] { 2, 3 }))
                    .Add(new CatalogEntry("c", new byte[] { 4, 5, 6 }))
                    .Build();
            }

            BuildCat().Should().Equal(BuildCat());
        }

        [Fact]
        public void Build_DeduplicatesIdenticalContent()
        {
            byte[] content = new byte[] { 9, 9, 9 };
            byte[] cat = new CatalogBuilder()
                .Add(new CatalogEntry("first", content))
                .Add(new CatalogEntry("second", content))
                .Build();

            // Two entries with identical content collapse to a single SHA-1 + SHA-256 pair.
            ExtractMembers(cat).Should().HaveCount(2);
        }

        [Fact]
        public void Build_WithNoEntries_Throws()
        {
            Action act = () => new CatalogBuilder().Build();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void FromFile_ProducesSameCatalogAsInMemoryContent()
        {
            string path = Path.Combine(Path.GetTempPath(), "catalog-fromfile-" + Guid.NewGuid().ToString("N") + ".js");
            try
            {
                File.WriteAllBytes(path, s_registerJsContent);

                byte[] fromFile = new CatalogBuilder().AddFile(path, "register.js").Build();
                byte[] fromMemory = new CatalogBuilder().Add(new CatalogEntry("register.js", s_registerJsContent)).Build();

                fromFile.Should().Equal(fromMemory);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Fact]
        public void ListIdentifier_WithWrongLength_Throws()
        {
            var builder = new CatalogBuilder();

            Action act = () => builder.ListIdentifier = new byte[8];

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GenerateCatalogTask_WritesCatalogAndCreatesDirectory()
        {
            string root = Path.Combine(Path.GetTempPath(), "catalog-tests-" + Guid.NewGuid().ToString("N"));
            try
            {
                string input = Path.Combine(root, "input.js");
                Directory.CreateDirectory(root);
                File.WriteAllBytes(input, s_registerJsContent);

                string output = Path.Combine(root, "nested", "out.cat");

                var task = new GenerateFileCatalog
                {
                    BuildEngine = new MockBuildEngine(),
                    Files = new[] { new TaskItem(input) },
                    OutputPath = output
                };

                task.Execute().Should().BeTrue();
                File.Exists(output).Should().BeTrue();

                Dictionary<string, byte[]> members = ExtractMembers(File.ReadAllBytes(output));
                members.Should().ContainKey(Convert.ToHexString(SHA1.HashData(s_registerJsContent)));
            }
            finally
            {
                if (Directory.Exists(root))
                {
                    Directory.Delete(root, recursive: true);
                }
            }
        }

        [Fact]
        public void GenerateCatalogTask_WithNoFiles_SucceedsWithoutWriting()
        {
            string output = Path.Combine(Path.GetTempPath(), "catalog-empty-" + Guid.NewGuid().ToString("N") + ".cat");

            var task = new GenerateFileCatalog
            {
                BuildEngine = new MockBuildEngine(),
                Files = Array.Empty<Microsoft.Build.Framework.ITaskItem>(),
                OutputPath = output
            };

            task.Execute().Should().BeTrue();
            File.Exists(output).Should().BeFalse();
        }

        [Fact]
        public void GenerateCatalogTask_WithNoFiles_DeletesStaleOutput()
        {
            string output = Path.Combine(Path.GetTempPath(), "catalog-stale-" + Guid.NewGuid().ToString("N") + ".cat");
            File.WriteAllBytes(output, new byte[] { 1, 2, 3 });

            var task = new GenerateFileCatalog
            {
                BuildEngine = new MockBuildEngine(),
                Files = Array.Empty<Microsoft.Build.Framework.ITaskItem>(),
                OutputPath = output
            };

            try
            {
                task.Execute().Should().BeTrue();
                File.Exists(output).Should().BeFalse();
            }
            finally
            {
                if (File.Exists(output))
                {
                    File.Delete(output);
                }
            }
        }

        /// <summary>
        /// Parses a catalog and returns a map of uppercase-hex member identifier to the
        /// member's full TrustedSubject DER bytes.
        /// </summary>
        private static Dictionary<string, byte[]> ExtractMembers(byte[] cat)
        {
            var result = new Dictionary<string, byte[]>();

            AsnReader contentInfo = new AsnReader(cat, AsnEncodingRules.BER).ReadSequence();
            contentInfo.ReadObjectIdentifier();
            AsnReader signedData = contentInfo
                .ReadSequence(new Asn1Tag(TagClass.ContextSpecific, 0, true))
                .ReadSequence();
            signedData.ReadInteger();
            signedData.ReadSetOf();
            AsnReader encap = signedData.ReadSequence();
            encap.ReadObjectIdentifier();
            AsnReader ctl = encap
                .ReadSequence(new Asn1Tag(TagClass.ContextSpecific, 0, true))
                .ReadSequence();

            AsnReader trustedSubjects = FindTrustedSubjects(ctl);
            while (trustedSubjects.HasData)
            {
                byte[] memberBytes = trustedSubjects.PeekEncodedValue().ToArray();
                AsnReader member = trustedSubjects.ReadSequence();
                result[Convert.ToHexString(member.ReadOctetString())] = memberBytes;
            }

            return result;
        }

        // trustedSubjects is the CTL child SEQUENCE whose first child is itself a SEQUENCE.
        private static AsnReader FindTrustedSubjects(AsnReader ctl)
        {
            while (ctl.HasData)
            {
                Asn1Tag tag = ctl.PeekTag();
                if (tag.TagClass == TagClass.Universal && tag.TagValue == (int)UniversalTagNumber.Sequence)
                {
                    byte[] seqBytes = ctl.PeekEncodedValue().ToArray();
                    ctl.ReadEncodedValue();
                    AsnReader seq = new AsnReader(seqBytes, AsnEncodingRules.BER).ReadSequence();
                    if (seq.HasData &&
                        seq.PeekTag() is { TagClass: TagClass.Universal, TagValue: (int)UniversalTagNumber.Sequence })
                    {
                        return new AsnReader(seqBytes, AsnEncodingRules.BER).ReadSequence();
                    }
                }
                else
                {
                    ctl.ReadEncodedValue();
                }
            }

            throw new InvalidOperationException("trustedSubjects not found in CTL.");
        }
    }
}
