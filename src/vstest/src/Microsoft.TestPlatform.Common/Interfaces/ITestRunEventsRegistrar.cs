// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

public interface ITestRunEventsRegistrar : IBaseTestEventsRegistrar
{
    /// <summary>
    /// Registers to receive events from the provided test run request.
    /// These events will then be broadcast to any registered loggers.
    /// </summary>
    /// <param name="testRunRequest">The run request to register for events on.</param>
    void RegisterTestRunEvents(ITestRunRequest testRunRequest);

    /// <summary>
    /// Unregisters the events from the test run request.
    /// </summary>
    /// <param name="testRunRequest">The run request from which events should be unregistered.</param>
    void UnregisterTestRunEvents(ITestRunRequest testRunRequest);
}
