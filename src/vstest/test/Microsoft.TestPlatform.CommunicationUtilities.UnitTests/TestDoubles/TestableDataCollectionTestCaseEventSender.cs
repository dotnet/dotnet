// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.TestDoubles;

internal class TestableDataCollectionTestCaseEventSender : DataCollectionTestCaseEventSender
{
    public TestableDataCollectionTestCaseEventSender(ICommunicationManager communicationManager, IDataSerializer dataSerializer)
        : base(communicationManager, dataSerializer)
    {
    }
}
