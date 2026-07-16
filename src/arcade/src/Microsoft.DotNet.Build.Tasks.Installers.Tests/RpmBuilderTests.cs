// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Installers.Tests
{
    public class RpmBuilderTests
    {
        // RPMFILE_GHOST flag (rpmfiles.h): owned by the package but not shipped in the payload.
        private const int RpmFileGhost = 1 << 6;
        // RPMSENSE_TRIGGERIN (rpmds.h).
        private const int RpmSenseTriggerIn = 1 << 16;
        private const int RpmDefaultFileTriggerPriority = 1000000;

        private static RpmBuilder CreateBuilder() =>
            new("test-package", "1.0.0", "1", Architecture.X64, OSPlatform.Linux)
            {
                Vendor = "Test Vendor",
                Packager = "Test Packager",
                License = "MIT",
                Summary = "Test summary",
                Description = "Test description",
                Url = "https://example.test",
            };

        private static CpioEntry RegularFile(string name, string content, uint permissions = 0x1ED /* 0755 */)
        {
            return new CpioEntry(
                inode: 1,
                name: name,
                timestamp: 0,
                ownerID: 0,
                groupID: 0,
                mode: CpioEntry.RegularFile | permissions,
                numberOfLinks: 1,
                devMajor: 0,
                devMinor: 0,
                rdevMajor: 0,
                rdevMinor: 0,
                dataStream: new MemoryStream(Encoding.UTF8.GetBytes(content)));
        }

        private static CpioEntry SymbolicLink(string name, string target)
        {
            return new CpioEntry(
                inode: 2,
                name: name,
                timestamp: 0,
                ownerID: 0,
                groupID: 0,
                mode: CpioEntry.SymbolicLink | 0x1FF /* 0777 */,
                numberOfLinks: 1,
                devMajor: 0,
                devMinor: 0,
                rdevMajor: 0,
                rdevMinor: 0,
                dataStream: new MemoryStream(Encoding.UTF8.GetBytes(target)));
        }

        private static RpmPackage RoundTrip(RpmBuilder builder)
        {
            RpmPackage package = builder.Build();
            using MemoryStream stream = new();
            package.WriteTo(stream);
            stream.Position = 0;
            return RpmPackage.Read(stream);
        }

        private static T[] GetArray<T>(RpmPackage package, RpmHeaderTag tag)
        {
            RpmHeader<RpmHeaderTag>.Entry entry = package.Header.Entries.First(e => e.Tag == tag);
            return (T[])entry.Value;
        }

        private static bool HasTag(RpmPackage package, RpmHeaderTag tag) =>
            package.Header.Entries.Any(e => e.Tag == tag);

        private static List<string> ReadArchiveEntryNames(RpmPackage package)
        {
            List<string> names = new();
            package.ArchiveStream.Position = 0;
            using CpioReader reader = new(package.ArchiveStream, leaveOpen: true);
            for (CpioEntry? entry = reader.GetNextEntry(); entry is not null; entry = reader.GetNextEntry())
            {
                if (entry.Name == "TRAILER!!!")
                {
                    break;
                }
                names.Add(entry.Name);
            }
            return names;
        }

        [Fact]
        public void GhostFile_IsRecordedInHeaderWithGhostFlagAndOmittedFromPayload()
        {
            RpmBuilder builder = CreateBuilder();
            builder.AddFile(RegularFile("./usr/share/dotnet/dnx", "#!/bin/sh\nreal dispatcher\n"), "a /bin/sh script");
            builder.AddGhostFile(SymbolicLink("./usr/bin/dnx", "../share/dotnet/dnx"), "symbolic link");

            using RpmPackage package = RoundTrip(builder);

            string[] baseNames = GetArray<string>(package, RpmHeaderTag.BaseNames);
            int[] fileFlags = GetArray<int>(package, RpmHeaderTag.FileFlags);

            // Both files have basename "dnx"; find the one flagged as ghost.
            int[] dnxIndices = baseNames
                .Select((n, i) => (n, i))
                .Where(t => t.n == "dnx")
                .Select(t => t.i)
                .ToArray();
            dnxIndices.Should().HaveCount(2);

            int[] ghostFlags = dnxIndices.Where(i => (fileFlags[i] & RpmFileGhost) != 0).ToArray();
            int[] nonGhostFlags = dnxIndices.Where(i => (fileFlags[i] & RpmFileGhost) == 0).ToArray();
            ghostFlags.Should().HaveCount(1, "exactly one of the two dnx entries is a ghost");
            nonGhostFlags.Should().HaveCount(1, "the shipped dispatcher must not be a ghost");

            // The ghost entry (the /usr/bin/dnx symlink) must not be present in the CPIO payload.
            List<string> archiveNames = ReadArchiveEntryNames(package);
            archiveNames.Should().Contain("./usr/share/dotnet/dnx");
            archiveNames.Should().NotContain("./usr/bin/dnx", "ghost files are omitted from the payload");
        }

        [Fact]
        public void GhostFile_HasEmptyDigest()
        {
            RpmBuilder builder = CreateBuilder();
            builder.AddGhostFile(RegularFile("./usr/share/dotnet/dnx", "content that should not be shipped"), "a /bin/sh script");

            using RpmPackage package = RoundTrip(builder);

            string[] baseNames = GetArray<string>(package, RpmHeaderTag.BaseNames);
            string[] fileDigests = GetArray<string>(package, RpmHeaderTag.FileDigests);
            int index = Array.IndexOf(baseNames, "dnx");
            index.Should().BeGreaterThanOrEqualTo(0);
            fileDigests[index].Should().BeEmpty("ghost files ship no content, so their digest is empty");
        }

        [Fact]
        public void FileTrigger_EmitsExpectedParallelHeaderArrays()
        {
            RpmBuilder builder = CreateBuilder();
            builder.AddFile(RegularFile("./usr/share/dotnet/dnx", "dispatcher"), "a /bin/sh script");
            const string script = "#!/bin/sh\nrepair dnx\n";
            builder.AddFileTrigger("FileTriggerIn", script, new[] { "/usr/bin/dnx", "/usr/share/dotnet/dnx" });

            using RpmPackage package = RoundTrip(builder);

            // Per-script arrays (one script => one element).
            GetArray<string>(package, RpmHeaderTag.FileTriggerScripts).Should().Equal(script);
            GetArray<string>(package, RpmHeaderTag.FileTriggerScriptProg).Should().Equal("/bin/sh");
            GetArray<int>(package, RpmHeaderTag.FileTriggerScriptFlags).Should().Equal(0);
            GetArray<int>(package, RpmHeaderTag.FileTriggerPriorities).Should().Equal(RpmDefaultFileTriggerPriority);

            // Per-condition arrays (two trigger paths => two elements, both pointing back to script index 0).
            GetArray<string>(package, RpmHeaderTag.FileTriggerName).Should().Equal("/usr/bin/dnx", "/usr/share/dotnet/dnx");
            GetArray<string>(package, RpmHeaderTag.FileTriggerVersion).Should().Equal("", "");
            GetArray<int>(package, RpmHeaderTag.FileTriggerFlags).Should().Equal(RpmSenseTriggerIn, RpmSenseTriggerIn);
            GetArray<int>(package, RpmHeaderTag.FileTriggerIndex).Should().Equal(0, 0);
        }

        [Fact]
        public void FileTrigger_NotEmittedWhenNoneAdded()
        {
            RpmBuilder builder = CreateBuilder();
            builder.AddFile(RegularFile("./usr/share/dotnet/dnx", "dispatcher"), "a /bin/sh script");

            using RpmPackage package = RoundTrip(builder);

            HasTag(package, RpmHeaderTag.FileTriggerScripts).Should().BeFalse();
            HasTag(package, RpmHeaderTag.FileTriggerName).Should().BeFalse();
            HasTag(package, RpmHeaderTag.TransFileTriggerScripts).Should().BeFalse();
            HasTag(package, RpmHeaderTag.TransFileTriggerName).Should().BeFalse();
        }

        [Fact]
        public void TransactionFileTrigger_EmitsExpectedParallelHeaderArrays()
        {
            RpmBuilder builder = CreateBuilder();
            builder.AddFile(RegularFile("./usr/share/dotnet/dnx", "dispatcher"), "a /bin/sh script");
            const string script = "#!/bin/sh\nrepair dnx after transaction\n";
            builder.AddFileTrigger("TransFileTriggerIn", script, new[] { "/usr/share/dotnet/sdk" });

            using RpmPackage package = RoundTrip(builder);

            GetArray<string>(package, RpmHeaderTag.TransFileTriggerScripts).Should().Equal(script);
            GetArray<string>(package, RpmHeaderTag.TransFileTriggerScriptProg).Should().Equal("/bin/sh");
            GetArray<int>(package, RpmHeaderTag.TransFileTriggerScriptFlags).Should().Equal(0);
            GetArray<int>(package, RpmHeaderTag.TransFileTriggerPriorities).Should().Equal(RpmDefaultFileTriggerPriority);
            GetArray<string>(package, RpmHeaderTag.TransFileTriggerName).Should().Equal("/usr/share/dotnet/sdk");
            GetArray<string>(package, RpmHeaderTag.TransFileTriggerVersion).Should().Equal("");
            GetArray<int>(package, RpmHeaderTag.TransFileTriggerFlags).Should().Equal(RpmSenseTriggerIn);
            GetArray<int>(package, RpmHeaderTag.TransFileTriggerIndex).Should().Equal(0);

            HasTag(package, RpmHeaderTag.FileTriggerScripts).Should().BeFalse();
            HasTag(package, RpmHeaderTag.FileTriggerName).Should().BeFalse();
        }

        [Fact]
        public void AddFileTrigger_UnknownKind_Throws()
        {
            RpmBuilder builder = CreateBuilder();
            Action act = () => builder.AddFileTrigger("NotARealKind", "#!/bin/sh\n", new[] { "/usr/bin/dnx" });
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddFileTrigger_EmptyPaths_Throws()
        {
            RpmBuilder builder = CreateBuilder();
            Action act = () => builder.AddFileTrigger("FileTriggerIn", "#!/bin/sh\n", Array.Empty<string>());
            act.Should().Throw<ArgumentException>();
        }
    }
}
