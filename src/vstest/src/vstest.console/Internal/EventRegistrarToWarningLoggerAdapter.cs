// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

internal class EventRegistrarToWarningLoggerAdapter : IWarningLogger
{
    private readonly IBaseTestEventsRegistrar _testEventsRegistrar;

    public EventRegistrarToWarningLoggerAdapter(IBaseTestEventsRegistrar testEventsRegistrar)
    {
        _testEventsRegistrar = testEventsRegistrar;
    }

    public void LogWarning(string message)
    {
        _testEventsRegistrar.LogWarning(message);
    }
}
