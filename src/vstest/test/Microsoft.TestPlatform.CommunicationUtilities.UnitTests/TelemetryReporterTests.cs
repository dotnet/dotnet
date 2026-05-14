// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class TelemetryReporterTests
{
    private readonly Mock<IRequestData> _requestData;
    private readonly Mock<ICommunicationManager> _communicationManager;
    private readonly Mock<IDataSerializer> _dataSerializer;
    private readonly ITelemetryReporter _telemetryReporter;

    public TelemetryReporterTests()
    {
        _requestData = new();
        _communicationManager = new();
        _dataSerializer = new();
        _telemetryReporter = new TelemetryReporter(_requestData.Object, _communicationManager.Object, _dataSerializer.Object);
    }

    [TestMethod]
    public void Report_ShouldDoNothing_IfTelemetryDisabled()
    {
        _requestData.Setup(r => r.IsTelemetryOptedIn).Returns(false);
        TelemetryEvent telemetryEvent = new("name", new Dictionary<string, object>());

        _telemetryReporter.Report(telemetryEvent);

        _requestData.VerifyAll();
        _requestData.VerifyNoOtherCalls();
        _dataSerializer.VerifyAll();
        _dataSerializer.VerifyNoOtherCalls();
        _communicationManager.VerifyAll();
        _communicationManager.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Report_ShouldSendMessage_IfTelemetryEnabled()
    {
        _requestData.Setup(r => r.IsTelemetryOptedIn).Returns(true);
        TelemetryEvent telemetryEvent = new("name", new Dictionary<string, object>());
        var rawMessage = "rawMessage";
        _dataSerializer.Setup(d => d.SerializePayload(MessageType.TelemetryEventMessage, telemetryEvent)).Returns(rawMessage);

        _telemetryReporter.Report(telemetryEvent);

        _communicationManager.Verify(c => c.SendRawMessage(rawMessage));
        _requestData.VerifyAll();
        _requestData.VerifyNoOtherCalls();
        _dataSerializer.VerifyAll();
        _dataSerializer.VerifyNoOtherCalls();
        _communicationManager.VerifyAll();
        _communicationManager.VerifyNoOtherCalls();
    }
}
