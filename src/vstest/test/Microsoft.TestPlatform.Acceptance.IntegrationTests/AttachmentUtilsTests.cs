// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class AttachmentUtilsTests
{
    private readonly List<string> _log = new();
    private TempDirectory _temp = null!;

    [TestInitialize]
    public void Initialize()
    {
        TempDirectory.NuGetConfigPath = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "NuGet.config");
        _temp = new TempDirectory();
    }

    [TestCleanup]
    public void Cleanup() => _temp.Dispose();

    [TestMethod]
    [DataRow("Simple", "Simple")]
    [DataRow("with space", "with_space")]
    [DataRow("a:b/c\\d", "a_b_c_d")]
    [DataRow("...", "test")]
    [DataRow("", "test")]
    [DataRow(null, "test")]
    public void SanitizePathSegmentReplacesEveryRunOfUnsafeCharactersWithOneUnderscore(string? name, string expected)
        => Assert.AreEqual(expected, AttachmentUtils.SanitizePathSegment(name));

    [TestMethod]
    public void SanitizePathSegmentShortensTheNameOfADataDrivenTest()
    {
        var sanitized = AttachmentUtils.SanitizePathSegment("CrashDumpOnStackOverflow (Row: 0, Runner = net10.0, TargetFramework = net10.0, InProcess)");

        Assert.IsTrue(sanitized.StartsWith("CrashDumpOnStackOverflow_Row_0_Runner_net10.0", StringComparison.Ordinal), sanitized);
        Assert.IsLessThanOrEqualTo(AttachmentUtils.MaxNameLength, sanitized.Length, sanitized);
        Assert.AreEqual(-1, sanitized.IndexOfAny(Path.GetInvalidFileNameChars()), sanitized);
    }

    [TestMethod]
    public void CopyAttachmentsCopiesTheWholeTreeOfAnAttachedDirectory()
    {
        var logs = _temp.CreateDirectory("logs").FullName;
        var destination = _temp.CreateDirectory("destination").FullName;
        File.WriteAllText(Path.Combine(logs, "log.txt"), "runner");
        File.WriteAllText(Path.Combine(Directory.CreateDirectory(Path.Combine(logs, "nested")).FullName, "log.host.txt"), "host");

        var copies = AttachmentUtils.CopyAttachments(new[] { logs }, destination, _log.Add);

        CollectionAssert.AreEquivalent(
            new[] { Path.Combine(destination, "log.txt"), Path.Combine(destination, "nested", "log.host.txt") },
            copies,
            Logged());
        Assert.AreEqual("runner", File.ReadAllText(Path.Combine(destination, "log.txt")));
        Assert.AreEqual("host", File.ReadAllText(Path.Combine(destination, "nested", "log.host.txt")));
    }

    [TestMethod]
    public void CopyAttachmentsCopiesALogThatIsStillOpenForWriting()
    {
        var logs = _temp.CreateDirectory("logs").FullName;
        var destination = _temp.CreateDirectory("destination").FullName;

        // A testhost that did not exit yet keeps its diagnostic log open, and shares it only for reading.
        // File.Copy asks for more than that and fails on exactly the logs we care about the most.
        using var open = new FileStream(Path.Combine(logs, "log.txt"), FileMode.CreateNew, FileAccess.Write, FileShare.Read);
        var content = Encoding.UTF8.GetBytes("still writing");
        open.Write(content, 0, content.Length);
        open.Flush();

        var copies = AttachmentUtils.CopyAttachments(new[] { logs }, destination, _log.Add);

        Assert.HasCount(1, copies, Logged());
        Assert.AreEqual("still writing", File.ReadAllText(copies[0]));
    }

    [TestMethod]
    public void CopyAttachmentsSkipsAnAttachmentThatIsNotThere()
    {
        var destination = _temp.CreateDirectory("destination").FullName;

        var copies = AttachmentUtils.CopyAttachments(new[] { Path.Combine(_temp.Path, "gone") }, destination, _log.Add);

        Assert.IsEmpty(copies, Logged());
        Assert.IsEmpty(Directory.GetFileSystemEntries(destination));
    }

    [TestMethod]
    public void CopyAttachmentsCopiesTheSameDirectoryOnlyOnce()
    {
        var logs = _temp.CreateDirectory("logs").FullName;
        var destination = _temp.CreateDirectory("destination").FullName;
        File.WriteAllText(Path.Combine(logs, "log.txt"), "runner");

        var copies = AttachmentUtils.CopyAttachments(new[] { logs, logs }, destination, _log.Add);

        Assert.HasCount(1, copies, Logged());
    }

    [TestMethod]
    public void CopyAttachmentsKeepsBothFilesWhenTwoAttachmentsAreNamedTheSame()
    {
        var first = _temp.CreateDirectory("first").FullName;
        var second = _temp.CreateDirectory("second").FullName;
        var destination = _temp.CreateDirectory("destination").FullName;
        File.WriteAllText(Path.Combine(first, "log.txt"), "first");
        File.WriteAllText(Path.Combine(second, "log.txt"), "second");

        var copies = AttachmentUtils.CopyAttachments(new[] { first, second }, destination, _log.Add);

        Assert.HasCount(2, copies, Logged());
        CollectionAssert.AreEquivalent(new[] { "first", "second" }, copies.Select(File.ReadAllText).ToList());
    }

    private string Logged() => string.Join(Environment.NewLine, _log);
}
