// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

/// <summary>
/// Event arguments used for raising TestRunStarted events.
/// Mainly contains the process Id of the test execution process running the tests.
/// </summary>
public class TestRunStartedEventArgs : EventArgs
{
    public int ProcessId { get; private set; }

    /// <param name="processId">The process Id of the test execution process running the tests.</param>
    public TestRunStartedEventArgs(int processId)
    {
        ProcessId = processId;
    }

    public override string ToString()
        => "ProcessId = " + ProcessId;
}
