// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeDataCollectorAttachmentsProcessorsFactory : IDataCollectorAttachmentsProcessorsFactory
{
    public FakeDataCollectorAttachmentsProcessorsFactory(FakeErrorAggregator fakeErrorAggregator)
    {
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public FakeErrorAggregator FakeErrorAggregator { get; }

    public DataCollectorAttachmentProcessor[] Create(InvokedDataCollector[]? invokedDataCollectors, IMessageLogger logger)
    {
        throw new NotImplementedException();
    }
}
