// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Installers.Tests
{
    public class CreateRpmPackageTests : IDisposable
    {
        private const int RpmFileGhost = 1 << 6;

        private readonly string _tempDir;

        public CreateRpmPackageTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "rpmtests-" + Guid.NewGuid().ToString("n"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(_tempDir, recursive: true);
            }
            catch
            {
                // Best-effort cleanup.
            }
        }

        private static CpioEntry RegularFile(string name, string content) =>
            new(1, name, 0, 0, 0, CpioEntry.RegularFile | 0x1ED, 1, 0, 0, 0, 0, new MemoryStream(Encoding.UTF8.GetBytes(content)));

        private static CpioEntry SymbolicLink(string name, string target) =>
            new(2, name, 0, 0, 0, CpioEntry.SymbolicLink | 0x1FF, 1, 0, 0, 0, 0, new MemoryStream(Encoding.UTF8.GetBytes(target)));

        private string WritePayload(params CpioEntry[] entries)
        {
            string path = Path.Combine(_tempDir, "payload.cpio");
            using FileStream stream = new(path, FileMode.Create);
            using CpioWriter writer = new(stream, leaveOpen: true);
            foreach (CpioEntry entry in entries)
            {
                writer.WriteNextEntry(entry);
            }
            return path;
        }

        private CreateRpmPackage CreateTask(string payloadPath, IEnumerable<ITaskItem> rawFileKinds, MockBuildEngine engine)
        {
            return new CreateRpmPackage
            {
                BuildEngine = engine,
                OutputRpmPackagePath = Path.Combine(_tempDir, "out.rpm"),
                Vendor = "Test Vendor",
                Packager = "Test Packager",
                PackageName = "dotnet-host",
                PackageVersion = "11.0.0",
                PackageRelease = "1",
                PackageOS = "LINUX",
                PackageArchitecture = "x64",
                Payload = payloadPath,
                RawPayloadFileKinds = rawFileKinds.ToArray(),
                Requires = Array.Empty<ITaskItem>(),
                Conflicts = Array.Empty<ITaskItem>(),
                OwnedDirectories = Array.Empty<ITaskItem>(),
                ChangelogLines = Array.Empty<ITaskItem>(),
                License = "MIT",
                Summary = "Test summary",
                Description = "Test description",
                PackageUrl = "https://example.test",
                Scripts = Array.Empty<ITaskItem>(),
            };
        }

        [Fact]
        public void GhostFileAndFileTrigger_AreHarvestedAndEmitted()
        {
            string payload = WritePayload(
                RegularFile("usr/share/dotnet/dnx", "#!/bin/sh\nrobust dispatcher\n"),
                SymbolicLink("usr/bin/dnx", "../share/dotnet/dnx"));

            ITaskItem[] rawKinds =
            [
                new TaskItem("./usr/share/dotnet/dnx: a /bin/sh script"),
                new TaskItem("./usr/bin/dnx: symbolic link"),
            ];

            string scriptPath = Path.Combine(_tempDir, "repair.sh");
            File.WriteAllText(scriptPath, "#!/bin/sh\nrepair\n");

            MockBuildEngine engine = new();
            CreateRpmPackage task = CreateTask(payload, rawKinds, engine);
            task.GhostFiles = [new TaskItem("/usr/bin/dnx")];
            ITaskItem trigger = new TaskItem(scriptPath);
            trigger.SetMetadata("Kind", "FileTriggerIn");
            trigger.SetMetadata("Paths", "/usr/bin/dnx;/usr/share/dotnet/dnx");
            task.FileTriggers = [trigger];

            task.Execute().Should().BeTrue(BuildErrors(engine));

            using FileStream rpmStream = File.OpenRead(task.OutputRpmPackagePath);
            using RpmPackage package = RpmPackage.Read(rpmStream);

            string[] baseNames = (string[])package.Header.Entries.First(e => e.Tag == RpmHeaderTag.BaseNames).Value;
            int[] fileFlags = (int[])package.Header.Entries.First(e => e.Tag == RpmHeaderTag.FileFlags).Value;

            int ghostIndex = Array.IndexOf(baseNames, "dnx");
            // /usr/bin/dnx is the ghost; /usr/share/dotnet/dnx is shipped. Both have basename "dnx".
            IEnumerable<int> ghostFlagged = Enumerable.Range(0, baseNames.Length)
                .Where(i => baseNames[i] == "dnx" && (fileFlags[i] & RpmFileGhost) != 0);
            ghostFlagged.Should().HaveCount(1);
            ghostIndex.Should().BeGreaterThanOrEqualTo(0);

            string[] triggerNames = (string[])package.Header.Entries.First(e => e.Tag == RpmHeaderTag.FileTriggerName).Value;
            triggerNames.Should().Equal("/usr/bin/dnx", "/usr/share/dotnet/dnx");
        }

        [Fact]
        public void PostInAndPostUnScripts_AreEmittedAsRpmScriptletHeaders()
        {
            // The host dnx fix relies on wiring LinuxPostRemoveScript to the RPM Postun
            // scriptlet (final-removal cleanup). Prove both Postin and Postun scripts flow
            // through to real RPMTAG_POSTIN / RPMTAG_POSTUN header entries.
            string payload = WritePayload(
                RegularFile("usr/share/dotnet/dnx", "#!/bin/sh\nrobust dispatcher\n"));

            ITaskItem[] rawKinds =
            [
                new TaskItem("./usr/share/dotnet/dnx: a /bin/sh script"),
            ];

            string postinPath = Path.Combine(_tempDir, "postin.sh");
            File.WriteAllText(postinPath, "#!/bin/sh\npostin repair marker\n");
            string postunPath = Path.Combine(_tempDir, "postun.sh");
            File.WriteAllText(postunPath, "#!/bin/sh\npostun cleanup marker\n");

            MockBuildEngine engine = new();
            CreateRpmPackage task = CreateTask(payload, rawKinds, engine);
            ITaskItem postin = new TaskItem(postinPath);
            postin.SetMetadata("Kind", "Postin");
            ITaskItem postun = new TaskItem(postunPath);
            postun.SetMetadata("Kind", "Postun");
            task.Scripts = [postin, postun];

            task.Execute().Should().BeTrue(BuildErrors(engine));

            using FileStream rpmStream = File.OpenRead(task.OutputRpmPackagePath);
            using RpmPackage package = RpmPackage.Read(rpmStream);

            string postinValue = (string)package.Header.Entries.First(e => e.Tag == RpmHeaderTag.Postin).Value;
            string postunValue = (string)package.Header.Entries.First(e => e.Tag == RpmHeaderTag.Postun).Value;
            postinValue.Should().Contain("postin repair marker");
            postunValue.Should().Contain("postun cleanup marker");
        }

        [Fact]
        public void TransactionFileTrigger_IsEmitted()
        {
            string payload = WritePayload(
                RegularFile("usr/share/dotnet/dnx", "#!/bin/sh\nrobust dispatcher\n"));

            ITaskItem[] rawKinds =
            [
                new TaskItem("./usr/share/dotnet/dnx: a /bin/sh script"),
            ];

            string scriptPath = Path.Combine(_tempDir, "repair.sh");
            File.WriteAllText(scriptPath, "#!/bin/sh\nrepair after transaction\n");

            MockBuildEngine engine = new();
            CreateRpmPackage task = CreateTask(payload, rawKinds, engine);
            ITaskItem trigger = new TaskItem(scriptPath);
            trigger.SetMetadata("Kind", "TransFileTriggerIn");
            trigger.SetMetadata("Paths", "/usr/share/dotnet/sdk");
            task.FileTriggers = [trigger];

            task.Execute().Should().BeTrue(BuildErrors(engine));

            using FileStream rpmStream = File.OpenRead(task.OutputRpmPackagePath);
            using RpmPackage package = RpmPackage.Read(rpmStream);

            string[] triggerNames = (string[])package.Header.Entries
                .First(e => e.Tag == RpmHeaderTag.TransFileTriggerName).Value;
            triggerNames.Should().Equal("/usr/share/dotnet/sdk");
        }

        [Fact]
        public void GhostFileNotInPayload_LogsErrorAndFails()
        {
            string payload = WritePayload(
                RegularFile("usr/share/dotnet/dnx", "#!/bin/sh\nrobust dispatcher\n"));

            ITaskItem[] rawKinds =
            [
                new TaskItem("./usr/share/dotnet/dnx: a /bin/sh script"),
            ];

            MockBuildEngine engine = new();
            CreateRpmPackage task = CreateTask(payload, rawKinds, engine);
            task.GhostFiles = [new TaskItem("/usr/bin/dnx")];

            task.Execute().Should().BeFalse();
            engine.BuildErrorEvents.Should().ContainSingle()
                .Which.Message.Should().Contain("/usr/bin/dnx");
        }

        private static string BuildErrors(MockBuildEngine engine) =>
            string.Join(Environment.NewLine, engine.BuildErrorEvents.Select(e => e.Message));
    }
}
