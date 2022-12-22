// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollectorUnitTests;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class TestPlatformDataCollectionLoggerTests
{
    private readonly TestPlatformDataCollectionLogger _logger;
    private readonly Mock<IMessageSink> _messageSink;
    private readonly DataCollectorConfig _dataCollectorConfig;
    private readonly DataCollectionContext _context;

    public TestPlatformDataCollectionLoggerTests()
    {
        _messageSink = new Mock<IMessageSink>();
        _dataCollectorConfig = new DataCollectorConfig(typeof(CustomDataCollector));
        _logger = new TestPlatformDataCollectionLogger(_messageSink.Object, _dataCollectorConfig);

        var guid = Guid.NewGuid();
        var sessionId = new SessionId(guid);
        _context = new DataCollectionContext(sessionId);
    }

    [TestMethod]
    public void LogErrorShouldThrowExceptionIfContextIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(null!, string.Empty));

        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(null!, new Exception()));

        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(null!, string.Empty, new Exception()));
    }

    [TestMethod]
    public void LogErrorShouldThrowExceptionIfTextIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(_context, (string)null!));

        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(_context, null!, new Exception()));
    }

    [TestMethod]
    public void LogErrorShouldThrowExceptionIfExceptionIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(_context, (Exception?)null!));

        Assert.ThrowsException<ArgumentNullException>(() => _logger.LogError(_context, string.Empty, null!));
    }

    [TestMethod]
    public void LogErrorShouldSendMessageToMessageSink()
    {
        var text = "customtext";
        _logger.LogError(_context, text);

        _messageSink.Verify(x => x.SendMessage(It.IsAny<DataCollectionMessageEventArgs>()), Times.Once());

        _logger.LogError(_context, new Exception(text));
        _messageSink.Verify(x => x.SendMessage(It.IsAny<DataCollectionMessageEventArgs>()), Times.Exactly(2));

        _logger.LogError(_context, text, new Exception(text));
        _messageSink.Verify(x => x.SendMessage(It.IsAny<DataCollectionMessageEventArgs>()), Times.Exactly(3));
    }

    [TestMethod]
    public void LogWarningShouldSendMessageToMessageSink()
    {
        var text = "customtext";
        _logger.LogWarning(_context, text);

        _messageSink.Verify(x => x.SendMessage(It.IsAny<DataCollectionMessageEventArgs>()), Times.Once());
    }
}
