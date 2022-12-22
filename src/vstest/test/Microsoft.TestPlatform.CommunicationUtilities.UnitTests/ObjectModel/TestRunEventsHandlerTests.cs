// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.ObjectModel;

[TestClass]
public class TestRunEventsHandlerTests
{
    private readonly Mock<ITestRequestHandler> _mockClient;
    private readonly TestRunEventsHandler _testRunEventHandler;

    public TestRunEventsHandlerTests()
    {
        _mockClient = new Mock<ITestRequestHandler>();
        _testRunEventHandler = new TestRunEventsHandler(_mockClient.Object);
    }

    [TestMethod]
    public void HandleTestRunStatsChangeShouldSendTestRunStatisticsToClient()
    {
        _testRunEventHandler.HandleTestRunStatsChange(null!);
        _mockClient.Verify(th => th.SendTestRunStatistics(null!), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldInformClient()
    {
        _testRunEventHandler.HandleTestRunComplete(null!, null!, null!, null!);
        _mockClient.Verify(th => th.SendExecutionComplete(null!, null!, null!, null!), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunMessageShouldSendMessageToClient()
    {
        _testRunEventHandler.HandleLogMessage(TestMessageLevel.Informational, string.Empty);

        _mockClient.Verify(th => th.SendLog(TestMessageLevel.Informational, string.Empty), Times.AtLeast(1));
    }
}
