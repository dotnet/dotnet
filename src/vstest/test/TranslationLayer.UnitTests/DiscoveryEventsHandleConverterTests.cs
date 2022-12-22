// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TranslationLayer.UnitTests;

[TestClass]
public class DiscoveryEventsHandleConverterTests
{
    private readonly Mock<ITestDiscoveryEventsHandler> _mockTestdiscoveryEventsHandler;
    public DiscoveryEventsHandleConverterTests()
    {
        _mockTestdiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler>();
    }

    [TestMethod]
    public void ConstructorShouldThrowArgumentExceptionIfTestDiscoveryEventHandlerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new DiscoveryEventsHandleConverter(null!));
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCallTestDiscoveryHandler1Method()
    {
        var discoveryEventsHandler = new DiscoveryEventsHandleConverter(_mockTestdiscoveryEventsHandler.Object);

        discoveryEventsHandler.HandleDiscoveryComplete(new DiscoveryCompleteEventArgs(-1, false), null);
        _mockTestdiscoveryEventsHandler.Verify(o => o.HandleDiscoveryComplete(-1, null, false), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryTestsShouldCallTestDiscoveryHandler1Method()
    {
        var discoveryEventsHandler = new DiscoveryEventsHandleConverter(_mockTestdiscoveryEventsHandler.Object);

        discoveryEventsHandler.HandleDiscoveredTests(null);

        _mockTestdiscoveryEventsHandler.Verify(o => o.HandleDiscoveredTests(null), Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldCallTestDiscoveryHandler1Method()
    {
        var discoveryEventsHandler = new DiscoveryEventsHandleConverter(_mockTestdiscoveryEventsHandler.Object);

        discoveryEventsHandler.HandleRawMessage("DummyMessage");

        _mockTestdiscoveryEventsHandler.Verify(o => o.HandleRawMessage("DummyMessage"), Times.Once);
    }

    [TestMethod]
    public void HandleLogMessageShouldCallTestDiscoveryHandler1Method()
    {
        var discoveryEventsHandler = new DiscoveryEventsHandleConverter(_mockTestdiscoveryEventsHandler.Object);

        discoveryEventsHandler.HandleLogMessage(TestMessageLevel.Warning, "DummyMessage");

        _mockTestdiscoveryEventsHandler.Verify(o => o.HandleLogMessage(TestMessageLevel.Warning, "DummyMessage"), Times.Once);
    }
}
