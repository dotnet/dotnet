// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.EventHandlers;

/// <summary>
/// The test case events handler.
/// </summary>
internal class TestCaseEventsHandler : ITestCaseEventsHandler, ITestEventsPublisher
{
    public event EventHandler<SessionStartEventArgs>? SessionStart;

    public event EventHandler<SessionEndEventArgs>? SessionEnd;

    public event EventHandler<TestCaseStartEventArgs>? TestCaseStart;

    public event EventHandler<TestCaseEndEventArgs>? TestCaseEnd;

    public event EventHandler<TestResultEventArgs>? TestResult;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEventsHandler"/> class.
    /// </summary>
    public TestCaseEventsHandler()
    {
    }

    /// <inheritdoc />
    public void SendTestCaseStart(TestCase testCase)
    {
        TestCaseStart.SafeInvoke(this, new TestCaseStartEventArgs(testCase), "TestCaseEventsHandler.RaiseTestCaseStart");
    }

    /// <inheritdoc />
    public void SendTestCaseEnd(TestCase testCase, TestOutcome outcome)
    {
        TestCaseEnd.SafeInvoke(this, new TestCaseEndEventArgs(testCase, outcome), "TestCaseEventsHandler.RaiseTestCaseEnd");
    }

    /// <inheritdoc />
    public void SendTestResult(TestResult result)
    {
        TestResult.SafeInvoke(this, new TestResultEventArgs(result), "TestCaseEventsHandler.RaiseTestCaseEnd");
    }

    /// <inheritdoc />
    public void SendSessionStart(IDictionary<string, object?> properties)
    {
        SessionStart.SafeInvoke(this, new SessionStartEventArgs(properties), "TestCaseEventsHandler.RaiseSessionStart");
    }

    /// <inheritdoc />
    public void SendSessionEnd()
    {
        SessionEnd.SafeInvoke(this, new SessionEndEventArgs(), "TestCaseEventsHandler.RaiseSessionEnd");
    }
}
