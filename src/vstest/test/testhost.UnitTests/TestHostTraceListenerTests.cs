// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// define trace and debug to trigger the TPDebug.Assert calls even when we build in Release
#define DEBUG

#if NETCOREAPP

using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testhost.UnitTests;

[TestClass]
public class TestHostTraceListenerTests
{
    private readonly TraceListener[] _listeners;

    public TestHostTraceListenerTests()
    {
        _listeners = new TraceListener[Trace.Listeners.Count];
        Trace.Listeners.CopyTo(_listeners, 0);
        // not using the TestHostTraceListener.Setup method here
        // because that detects only default trace listeners and there won't
        // be any when this is in production, so this would end up testing against
        // an older version of the trace listener
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new TestHostTraceListener());
    }

    [TestCleanup]
    public void Cleanup()
    {
        Trace.Listeners.Clear();
        foreach (var listener in _listeners)
        {
            Trace.Listeners.Add(listener);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(DebugAssertException))]
    public void DebugAssertThrowsDebugAssertException()
    {
        Debug.Assert(false);
    }

    [TestMethod]
    [ExpectedException(typeof(DebugAssertException))]
    public void DebugFailThrowsDebugAssertException()
    {
        Debug.Fail("fail");
    }

    [TestMethod]
    [ExpectedException(typeof(DebugAssertException))]
    public void TraceAssertThrowsDebugAssertException()
    {
        Trace.Assert(false);
    }

    [TestMethod]
    [ExpectedException(typeof(DebugAssertException))]
    public void TraceFailThrowsDebugAssertException()
    {
        Trace.Fail("fail");
    }

    [TestMethod]
    public void TraceWriteDoesNotFailTheTest()
    {
        Trace.Write("hello");
    }

    [TestMethod]
    public void TraceWriteLineDoesNotFailTheTest()
    {
        Trace.WriteLine("hello");
    }

    [TestMethod]
    public void DebugWriteDoesNotFailTheTest()
    {
        Debug.Write("hello");
    }

    [TestMethod]
    public void DebugWriteLineDoesNotFailTheTest()
    {
        Debug.WriteLine("hello");
    }
}

[TestClass]
public class TestHostTraceListenerRegistrationTests
{
    private readonly TraceListener[] _listeners;

    public TestHostTraceListenerRegistrationTests()
    {
        _listeners = new TraceListener[Trace.Listeners.Count];
        Trace.Listeners.CopyTo(_listeners, 0);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Trace.Listeners.Clear();
        foreach (var listener in _listeners)
        {
            Trace.Listeners.Add(listener);
        }
    }

    [TestMethod]
    public void SetupReplacesDefaultTraceListener()
    {
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new DefaultTraceListener());
        TestHostTraceListener.Setup();

        // this is what will happen in the majority of cases, there will be a single
        // trace listener that will be the default trace listener and we will replace it
        // with ours
        Assert.IsInstanceOfType(Trace.Listeners[0], typeof(TestHostTraceListener));
    }

    [TestMethod]
    public void SetupKeepsNonDefaultTraceListeners()
    {
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new DummyTraceListener());
        Trace.Listeners.Add(new DefaultTraceListener());
        Trace.Listeners.Add(new DummyTraceListener());
        TestHostTraceListener.Setup();

        Assert.IsInstanceOfType(Trace.Listeners[0], typeof(DummyTraceListener));
        Assert.IsInstanceOfType(Trace.Listeners[1], typeof(TestHostTraceListener));
        Assert.IsInstanceOfType(Trace.Listeners[2], typeof(DummyTraceListener));
    }

    private class DummyTraceListener : TraceListener
    {
        public List<string?> Lines { get; } = new();

        public override void Write(string? message)
        {
            Lines.Add(message);
        }

        public override void WriteLine(string? message)
        {
            Lines.Add(message);
        }
    }
}

#endif
