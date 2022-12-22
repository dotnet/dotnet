// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// Sends test case events to communication layer.
/// </summary>
internal class ProxyOutOfProcDataCollectionManager
{
    private readonly IDataCollectionTestCaseEventSender _dataCollectionTestCaseEventSender;
    private readonly ITestEventsPublisher _testEventsPublisher;
    private readonly Dictionary<Guid, Collection<AttachmentSet>> _attachmentsCache;

    /// <summary>
    /// Sync object for ensuring that only run is active at a time
    /// </summary>
    private readonly object _syncObject = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyOutOfProcDataCollectionManager"/> class.
    /// </summary>
    /// <param name="dataCollectionTestCaseEventSender">
    /// The data collection test case event sender.
    /// </param>
    /// <param name="dataCollectionTestCaseEventManager">
    /// The data collection test case event manager.
    /// </param>
    public ProxyOutOfProcDataCollectionManager(IDataCollectionTestCaseEventSender dataCollectionTestCaseEventSender, ITestEventsPublisher testEventsPublisher)
    {
        _attachmentsCache = new Dictionary<Guid, Collection<AttachmentSet>>();
        _testEventsPublisher = testEventsPublisher;
        _dataCollectionTestCaseEventSender = dataCollectionTestCaseEventSender;

        _testEventsPublisher.TestCaseStart += TriggerTestCaseStart;
        _testEventsPublisher.TestCaseEnd += TriggerTestCaseEnd;
        _testEventsPublisher.TestResult += TriggerSendTestResult;
        _testEventsPublisher.SessionEnd += TriggerTestSessionEnd;
        _attachmentsCache = new Dictionary<Guid, Collection<AttachmentSet>>();
    }

    private void TriggerTestCaseStart(object? sender, TestCaseStartEventArgs e)
    {
        _dataCollectionTestCaseEventSender.SendTestCaseStart(e);
    }

    private void TriggerTestCaseEnd(object? sender, TestCaseEndEventArgs e)
    {
        var attachments = _dataCollectionTestCaseEventSender.SendTestCaseEnd(e);

        if (attachments != null)
        {
            lock (_syncObject)
            {
                if (!_attachmentsCache.TryGetValue(e.TestCaseId, out Collection<AttachmentSet>? attachmentSets))
                {
                    attachmentSets = new Collection<AttachmentSet>();
                    _attachmentsCache.Add(e.TestCaseId, attachmentSets);
                }

                foreach (var attachment in attachments)
                {
                    attachmentSets.Add(attachment);
                }
            }
        }
    }

    private void TriggerSendTestResult(object? sender, TestResultEventArgs e)
    {
        lock (_syncObject)
        {
            if (_attachmentsCache.TryGetValue(e.TestCaseId, out Collection<AttachmentSet>? attachmentSets))
            {
                foreach (var attachment in attachmentSets)
                {
                    e.TestResult.Attachments.Add(attachment);
                }
            }

            _attachmentsCache.Remove(e.TestCaseId);
        }
    }

    private void TriggerTestSessionEnd(object? sender, SessionEndEventArgs e)
    {
        _dataCollectionTestCaseEventSender.SendTestSessionEnd(e);
    }
}
