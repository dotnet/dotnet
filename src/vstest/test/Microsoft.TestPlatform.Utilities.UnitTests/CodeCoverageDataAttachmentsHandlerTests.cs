// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
#if !NETFRAMEWORK
using System.Runtime.InteropServices;
#endif
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Utilities.UnitTests;

[TestClass]
public class CodeCoverageDataAttachmentsHandlerTests
{
    private readonly Mock<IProgress<int>> _mockProgressReporter;
    private readonly Mock<IMessageLogger> _messageLogger;
    private readonly XmlElement _configurationElement;
    private readonly CodeCoverageDataAttachmentsHandler _coverageDataAttachmentsHandler;
    private readonly string _filePrefix;

    public TestContext? TestContext { get; set; }

    internal string TestFilesDirectory => Path.Combine(TestContext!.DeploymentDirectory!, "TestFiles");

    public CodeCoverageDataAttachmentsHandlerTests()
    {
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement/>");
        _configurationElement = doc.DocumentElement!;
        _mockProgressReporter = new Mock<IProgress<int>>();
        _messageLogger = new Mock<IMessageLogger>();
        _coverageDataAttachmentsHandler = new CodeCoverageDataAttachmentsHandler();
#if NETFRAMEWORK
        _filePrefix = "file:///";
#else
        _filePrefix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "file:///" : "file://";
#endif
    }

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        // Annoyingly those paths are the same, but one does not end with slash,
        // no matter how you compare, e.g. new DirectoryInfo(...).FullName, the slashes are not
        // removed. And then you get obscure error: File already exists, or File is used by other process
        // when you try to copy the file and provide both paths the same.
        var assemblyPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
        var deploymentDirectory = context.DeploymentDirectory!.TrimEnd(Path.DirectorySeparatorChar);
        if (new DirectoryInfo(deploymentDirectory).FullName != new DirectoryInfo(assemblyPath).FullName)
        {
            // Copying test files to deployment directory place,
            var testFilesDirectory = Path.Combine(deploymentDirectory!, "TestFiles");
            Directory.CreateDirectory(testFilesDirectory);
            var files = Directory.GetFiles(Path.Combine(assemblyPath, "TestFiles"));
            foreach (var file in files)
                File.Copy(file, Path.Combine(testFilesDirectory, Path.GetFileName(file)), overwrite: true);
        }
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldReturnEmptySetWhenNoAttachmentsOrAttachmentsAreNull()
    {
        Collection<AttachmentSet> attachment = new();
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.IsEmpty(resultAttachmentSets);

        resultAttachmentSets = await _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, null, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.IsEmpty(resultAttachmentSets);

        _mockProgressReporter.Verify(p => p.Report(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldReturnInputIfOnly1Attachment()
    {
        var attachmentSet = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\aa.coverage"), "coverage"));

        Collection<AttachmentSet> attachment = [attachmentSet];
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets.First().Attachments);
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", resultAttachmentSets.First().Uri.AbsoluteUri);
        Assert.AreEqual("file:///C:/temp/aa.coverage", resultAttachmentSets.First().Attachments.First().Uri.AbsoluteUri);
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldReturnInputIf2DifferentFormatAttachments()
    {
        var file1Path = Path.Combine(TestFilesDirectory, "fullcovered.cobertura.xml");
        var file2Path = Path.Combine(Path.Combine(TestFilesDirectory, "fullcovered.coverage"));
        var attachmentSet = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri(file1Path), "coverage"));
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri(file2Path), "coverage"));

        Collection<AttachmentSet> attachment = [attachmentSet];
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets);
        Assert.HasCount(2, resultAttachmentSets.First().Attachments);
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", resultAttachmentSets.First().Uri.AbsoluteUri);
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", resultAttachmentSets.Last().Uri.AbsoluteUri);
        Assert.AreEqual(_filePrefix + file1Path.Replace("\\", "/").Replace(" ", "%20"), resultAttachmentSets.First().Attachments.First().Uri.AbsoluteUri);
        Assert.AreEqual(_filePrefix + file2Path.Replace("\\", "/").Replace(" ", "%20"), resultAttachmentSets.First().Attachments.Last().Uri.AbsoluteUri);
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldReturnInputIf2SameFormatAttachments()
    {
        var file1Path = Path.Combine(TestFilesDirectory, "fullcovered.cobertura.xml");
        var attachmentSet = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri(file1Path), "coverage"));
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri(file1Path), "coverage"));

        Collection<AttachmentSet> attachment = [attachmentSet];
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets.First().Attachments);
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", resultAttachmentSets.First().Uri.AbsoluteUri);
        Assert.AreEqual(_filePrefix + file1Path.Replace("\\", "/").Replace(" ", "%20"), resultAttachmentSets.First().Attachments.First().Uri.AbsoluteUri);
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldReturnInputIfOnly1LogsAttachment()
    {
        var attachmentSet = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\aa.logs"), "coverage"));

        Collection<AttachmentSet> attachment = [attachmentSet];
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets.First().Attachments);
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", resultAttachmentSets.First().Uri.AbsoluteUri);
        Assert.AreEqual("file:///C:/temp/aa.logs", resultAttachmentSets.First().Attachments.First().Uri.AbsoluteUri);
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldReturnInputIfOnlySeveralLogsAttachmentAnd1Report()
    {
        var attachmentSet = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\aa.coverage"), "coverage"));

        var attachmentSet1 = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet1.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\aa.logs"), "coverage"));
        attachmentSet1.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\bb.logs"), "coverage"));

        Collection<AttachmentSet> attachment = [attachmentSet, attachmentSet1];
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.HasCount(2, resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets.First().Attachments);
        Assert.HasCount(2, resultAttachmentSets.Last().Attachments);
    }

    [TestMethod]
    public async Task HandleDataCollectionAttachmentSetsShouldThrowIfCancellationRequested()
    {
        var attachmentSet = new AttachmentSet(new Uri("//badrui//"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\aa.coverage"), "coverage"));
        CancellationTokenSource cts = new();
        cts.Cancel();

        Collection<AttachmentSet> attachment =
        [
            attachmentSet,
            attachmentSet
        ];

        await Assert.ThrowsExactlyAsync<OperationCanceledException>(async () => await _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(_configurationElement, attachment, _mockProgressReporter.Object, null, cts.Token));

        Assert.HasCount(2, attachment);

        _mockProgressReporter.Verify(p => p.Report(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task MergingPerTestCodeCoverageReturnsOneCoverageFile()
    {
        string file1Path = Path.Combine(TestFilesDirectory, "fullcovered.cobertura.xml");
        var attachmentSet = new AttachmentSet(new Uri("datacollector://microsoft/CodeCoverage/2.0"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri(file1Path), "coverage"));
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri(file1Path), "coverage"));

        var attachment = new Collection<AttachmentSet> { attachmentSet };

        var doc = new XmlDocument();
        doc.LoadXml("<Configuration><PerTestCodeCoverage>TrUe</PerTestCodeCoverage></Configuration>");
        ICollection<AttachmentSet> resultAttachmentSets = await
            _coverageDataAttachmentsHandler.ProcessAttachmentSetsAsync(doc.DocumentElement!, attachment, _mockProgressReporter.Object, _messageLogger.Object, CancellationToken.None);

        Assert.IsNotNull(resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets);
        Assert.HasCount(1, resultAttachmentSets.First().Attachments);
        Assert.AreEqual("datacollector://microsoft/CodeCoverage/2.0", resultAttachmentSets.First().Uri.AbsoluteUri);
    }
}
