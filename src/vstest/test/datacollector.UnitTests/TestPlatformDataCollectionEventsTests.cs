// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class TestPlatformDataCollectionEventsTests
{
    private readonly TestPlatformDataCollectionEvents _events;

    private DataCollectionContext? _context;
    private bool _isEventRaised;

    public TestPlatformDataCollectionEventsTests()
    {
        _events = new TestPlatformDataCollectionEvents();
    }

    [TestMethod]
    public void RaiseEventsShouldThrowExceptionIfEventArgsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _events.RaiseEvent(null!));
    }

    [TestMethod]
    public void RaiseEventsShouldRaiseEventsIfSessionStartEventArgsIsPassed()
    {
        _isEventRaised = false;
        var testCase = new TestCase();
        _context = new DataCollectionContext(testCase);

        _events.SessionStart += SessionStartMessageHandler;
        var eventArgs = new SessionStartEventArgs(_context, new Dictionary<string, object?>());
        _events.RaiseEvent(eventArgs);

        Assert.IsTrue(_isEventRaised);
    }

    [TestMethod]
    public void RaiseEventsShouldNotRaiseEventsIfEventIsNotRegistered()
    {
        _isEventRaised = false;
        var testCase = new TestCase();
        _context = new DataCollectionContext(testCase);

        var eventArgs = new SessionStartEventArgs(_context, new Dictionary<string, object?>());
        _events.RaiseEvent(eventArgs);

        Assert.IsFalse(_isEventRaised);
    }

    [TestMethod]
    public void RaiseEventsShouldNotRaiseEventsIfEventIsUnRegistered()
    {
        _isEventRaised = false;
        var testCase = new TestCase();
        _context = new DataCollectionContext(testCase);

        _events.SessionStart += SessionStartMessageHandler;
        _events.SessionStart -= SessionStartMessageHandler;
        var eventArgs = new SessionStartEventArgs(_context, new Dictionary<string, object?>());
        _events.RaiseEvent(eventArgs);

        Assert.IsFalse(_isEventRaised);
    }

    [TestMethod]
    public void RaiseEventsShouldRaiseEventsIfSessionEndEventArgsIsPassed()
    {
        _isEventRaised = false;
        var testCase = new TestCase();
        _context = new DataCollectionContext(testCase);

        _events.SessionEnd += SessionEndMessageHandler;
        var eventArgs = new SessionEndEventArgs(_context);
        _events.RaiseEvent(eventArgs);

        Assert.IsTrue(_isEventRaised);
    }

    [TestMethod]
    public void RaiseEventsShouldRaiseEventsIfTestCaseStartEventArgsIsPassed()
    {
        _isEventRaised = false;
        var testCase = new TestCase();
        _context = new DataCollectionContext(testCase);

        _events.TestCaseStart += TestCaseStartMessageHandler;
        var eventArgs = new TestCaseStartEventArgs(_context, testCase);
        _events.RaiseEvent(eventArgs);

        Assert.IsTrue(_isEventRaised);
    }

    [TestMethod]
    public void RaiseEventsShouldRaiseEventsIfTestCaseEndEventArgsIsPassed()
    {
        _isEventRaised = false;
        var testCase = new TestCase();
        _context = new DataCollectionContext(testCase);

        _events.TestCaseEnd += TestCaseEndMessageHandler;
        var eventArgs = new TestCaseEndEventArgs(_context, testCase, TestOutcome.Passed);
        _events.RaiseEvent(eventArgs);

        Assert.IsTrue(_isEventRaised);
    }

    [TestMethod]
    public void AreTestCaseEventsSubscribedShouldReturnTrueIfTestCaseStartIsSubscribed()
    {
        _events.TestCaseStart += TestCaseStartMessageHandler;

        Assert.IsTrue(_events.AreTestCaseEventsSubscribed());
    }

    [TestMethod]
    public void AreTestCaseEventsSubscribedShouldReturnTrueIfTestCaseEndIsSubscribed()
    {
        _events.TestCaseEnd += TestCaseEndMessageHandler;

        Assert.IsTrue(_events.AreTestCaseEventsSubscribed());
    }

    [TestMethod]
    public void AreTestCaseEventsSubscribedShouldFalseIfTestCaseEventsAreNotSubscribed()
    {
        Assert.IsFalse(_events.AreTestCaseEventsSubscribed());
    }

    private void SessionStartMessageHandler(object? sender, SessionStartEventArgs e)
    {
        _isEventRaised = true;
    }

    private void SessionEndMessageHandler(object? sender, SessionEndEventArgs e)
    {
        _isEventRaised = true;
    }

    private void TestCaseStartMessageHandler(object? sender, TestCaseStartEventArgs e)
    {
        _isEventRaised = true;
    }

    private void TestCaseEndMessageHandler(object? sender, TestCaseEndEventArgs e)
    {
        _isEventRaised = true;
    }
}
