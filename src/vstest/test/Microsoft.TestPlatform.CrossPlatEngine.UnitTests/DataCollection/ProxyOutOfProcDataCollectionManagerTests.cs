// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class ProxyOutOfProcDataCollectionManagerTests
{
    private readonly Mock<ITestEventsPublisher> _mockTestEventsPublisher;
    private readonly Mock<IDataCollectionTestCaseEventSender> _mockDataCollectionTestCaseEventSender;
    private readonly Collection<AttachmentSet> _attachmentSets;
    private readonly TestCase _testcase;
    private VisualStudio.TestPlatform.ObjectModel.TestResult _testResult;

    private readonly ProxyOutOfProcDataCollectionManager _proxyOutOfProcDataCollectionManager;
    public ProxyOutOfProcDataCollectionManagerTests()
    {
        _mockTestEventsPublisher = new Mock<ITestEventsPublisher>();
        _mockDataCollectionTestCaseEventSender = new Mock<IDataCollectionTestCaseEventSender>();
        _proxyOutOfProcDataCollectionManager = new ProxyOutOfProcDataCollectionManager(_mockDataCollectionTestCaseEventSender.Object, _mockTestEventsPublisher.Object);

        var attachmentSet = new AttachmentSet(new Uri("my://datacollector"), "mydatacollector");
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("my://attachment.txt"), string.Empty));
        _attachmentSets = new Collection<AttachmentSet>
        {
            attachmentSet
        };

        _testcase = new TestCase();
        _testcase.Id = Guid.NewGuid();
        _mockDataCollectionTestCaseEventSender.Setup(x => x.SendTestCaseEnd(It.IsAny<TestCaseEndEventArgs>())).Returns(_attachmentSets);
        _mockTestEventsPublisher.Raise(x => x.TestCaseEnd += null, new TestCaseEndEventArgs(_testcase, TestOutcome.Passed));
        _testResult = new VisualStudio.TestPlatform.ObjectModel.TestResult(_testcase);
    }

    [TestMethod]
    public void TriggerTestCaseEndShouldReturnCacheAttachmentsAndAssociateWithTestResultWhenTriggerSendTestResultIsInvoked()
    {
        _mockTestEventsPublisher.Raise(x => x.TestResult += null, new TestResultEventArgs(_testResult));

        Assert.AreEqual(1, _testResult.Attachments.Count);
        Assert.IsTrue(_testResult.Attachments[0].Attachments[0].Uri.OriginalString.Contains("attachment.txt"));
    }

    [TestMethod]
    public void TriggerSendTestResultShouldDeleteTheAttachmentsFromCache()
    {
        _mockTestEventsPublisher.Raise(x => x.TestResult += null, new TestResultEventArgs(_testResult));

        _testResult = new VisualStudio.TestPlatform.ObjectModel.TestResult(_testcase);
        _mockTestEventsPublisher.Raise(x => x.TestResult += null, new TestResultEventArgs(_testResult));

        Assert.AreEqual(0, _testResult.Attachments.Count);
    }
}
