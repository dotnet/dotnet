// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;

/// <summary>
/// Manager for sending test case events to data collectors.
/// </summary>
internal interface ITestEventsPublisher
{
    /// <summary>
    /// The session start event.
    /// </summary>
    event EventHandler<SessionStartEventArgs> SessionStart;

    /// <summary>
    /// The session end event.
    /// </summary>
    event EventHandler<SessionEndEventArgs> SessionEnd;

    /// <summary>
    /// The test case start event.
    /// </summary>
    event EventHandler<TestCaseStartEventArgs> TestCaseStart;

    /// <summary>
    /// The test case end event.
    /// </summary>
    event EventHandler<TestCaseEndEventArgs> TestCaseEnd;

    /// <summary>
    /// The test result event.
    /// </summary>
    event EventHandler<TestResultEventArgs> TestResult;
}
