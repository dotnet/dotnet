// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector.UnitTests;

/// <summary>
/// The blame logger tests.
/// </summary>
[TestClass]
public class BlameLoggerTests
{
    private readonly Mock<IOutput> _mockOutput;
    private readonly Mock<IBlameReaderWriter> _mockBlameReaderWriter;
    private readonly BlameLogger _blameLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameLoggerTests"/> class.
    /// </summary>
    public BlameLoggerTests()
    {
        // Mock for ITestRunRequest
        _mockOutput = new Mock<IOutput>();
        _mockBlameReaderWriter = new Mock<IBlameReaderWriter>();
        _blameLogger = new TestableBlameLogger(_mockOutput.Object, _mockBlameReaderWriter.Object);
    }

    /// <summary>
    /// The initialize should throw exception if events is null.
    /// </summary>
    [TestMethod]
    public void InitializeShouldThrowExceptionIfEventsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _blameLogger.Initialize(null!, string.Empty));
    }

    /// <summary>
    /// The test run complete handler should get faulty test run if test run aborted.
    /// </summary>
    [TestMethod]
    public void TestRunCompleteHandlerShouldGetFaultyTestRunIfTestRunAborted()
    {
        InitializeAndVerify(1);
    }

    /// <summary>
    /// The test run complete handler should get faulty test run if test run aborted for multiple test project.
    /// </summary>
    [TestMethod]
    public void TestRunCompleteHandlerShouldGetFaultyTestRunIfTestRunAbortedForMultipleProjects()
    {
        InitializeAndVerify(2);
    }

    /// <summary>
    /// The test run complete handler should not read file if test run not aborted.
    /// </summary>
    [TestMethod]
    public void TestRunCompleteHandlerShouldNotReadFileIfTestRunNotAborted()
    {
        // Initialize Blame Logger
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        _blameLogger.Initialize(loggerEvents, null);

        // Setup and Raise event
        _mockBlameReaderWriter.Setup(x => x.ReadTestSequence(It.IsAny<string>()));
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        // Verify Call
        _mockBlameReaderWriter.Verify(x => x.ReadTestSequence(It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// The test run complete handler should return if uri attachment is null.
    /// </summary>
    [TestMethod]
    public void TestRunCompleteHandlerShouldReturnIfUriAttachmentIsNull()
    {
        // Initialize
        var attachmentSet = new AttachmentSet(new Uri("test://uri"), "Blame");
        var attachmentSetList = new List<AttachmentSet> { attachmentSet };

        // Initialize Blame Logger
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        _blameLogger.Initialize(loggerEvents, null);

        // Setup and Raise event
        loggerEvents.CompleteTestRun(null, false, true, null, new Collection<AttachmentSet>(attachmentSetList), new Collection<InvokedDataCollector>(), new TimeSpan(1, 0, 0, 0));

        // Verify Call
        _mockBlameReaderWriter.Verify(x => x.ReadTestSequence(It.IsAny<string>()), Times.Never);
    }

    private static AttachmentSet GetAttachmentSet()
    {
        var attachmentSet = new AttachmentSet(new Uri("test://uri"), "Blame");
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:/folder1/sequence.xml"), "description"));
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:/folder1/dump.dmp"), "description"));

        return attachmentSet;
    }

    private void InitializeAndVerify(int count)
    {
        // Initialize
        var attachmentSetList = new List<AttachmentSet>();

        for (int i = 0; i < count; i++)
        {
            attachmentSetList.Add(GetAttachmentSet());
        }

        // Initialize Blame Logger
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        _blameLogger.Initialize(loggerEvents, null);

        var testCaseList = new List<BlameTestObject>
        {
            new BlameTestObject(new TestCase("ABC.UnitTestMethod1", new Uri("test://uri"), "C://test/filepath")),
            new BlameTestObject(new TestCase("ABC.UnitTestMethod2", new Uri("test://uri"), "C://test/filepath"))
        };

        // Setup and Raise event
        _mockBlameReaderWriter.Setup(x => x.ReadTestSequence(It.IsAny<string>())).Returns(testCaseList);
        loggerEvents.CompleteTestRun(null, false, true, null, new Collection<AttachmentSet>(attachmentSetList), new Collection<InvokedDataCollector>(), new TimeSpan(1, 0, 0, 0));

        // Verify Call
        _mockBlameReaderWriter.Verify(x => x.ReadTestSequence(It.Is<string>(str => str.EndsWith(".xml"))), Times.Exactly(count));
    }

    /// <summary>
    /// The testable blame logger.
    /// </summary>
    internal class TestableBlameLogger : BlameLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestableBlameLogger"/> class.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="blameReaderWriter">
        /// The blame Reader Writer.
        /// </param>
        internal TestableBlameLogger(IOutput output, IBlameReaderWriter blameReaderWriter)
            : base(output, blameReaderWriter)
        {
        }
    }
}
