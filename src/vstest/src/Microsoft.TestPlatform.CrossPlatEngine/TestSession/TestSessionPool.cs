// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;

/// <summary>
/// Represents the test session pool.
/// </summary>
public class TestSessionPool
{
    private static readonly object InstanceLockObject = new();
    private static volatile TestSessionPool? s_instance;

    private readonly object _lockObject = new();
    private readonly Dictionary<TestSessionInfo, ProxyTestSessionManager> _sessionPool;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSessionPool"/> class.
    /// </summary>
    internal TestSessionPool()
    {
        _sessionPool = new Dictionary<TestSessionInfo, ProxyTestSessionManager>();
    }

    /// <summary>
    /// Gets the test session pool instance.
    /// Sets the test session pool instance for testing purposes only.
    /// </summary>
    ///
    /// <remarks>Thread-safe singleton pattern.</remarks>
    [AllowNull]
    public static TestSessionPool Instance
    {
        get
        {
            if (s_instance == null)
            {
                lock (InstanceLockObject)
                {
                    s_instance ??= new TestSessionPool();
                }
            }

            return s_instance;
        }
        internal set
        {
            s_instance = value;
        }
    }

    /// <summary>
    /// Adds a session to the pool.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="proxyManager">The proxy manager object.</param>
    ///
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public virtual bool AddSession(
        TestSessionInfo testSessionInfo,
        ProxyTestSessionManager proxyManager)
    {
        lock (_lockObject)
        {
            // Check if the session info already exists.
            if (_sessionPool.ContainsKey(testSessionInfo))
            {
                return false;
            }

            // Adds an association between session info and proxy manager to the pool.
            _sessionPool.Add(testSessionInfo, proxyManager);
            return true;
        }
    }

    /// <summary>
    /// Kills and removes a session from the pool.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="requestData">The request data.</param>
    ///
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public virtual bool KillSession(TestSessionInfo testSessionInfo, IRequestData requestData)
    {
        // TODO (copoiena): What happens if some request is running for the current session ?
        // Should we stop the request as well ? Probably yes.
        IProxyTestSessionManager? proxyManager;

        lock (_lockObject)
        {
            // Check if the session info exists.
            if (!_sessionPool.ContainsKey(testSessionInfo))
            {
                return false;
            }

            // Remove the session from the pool.
            proxyManager = _sessionPool[testSessionInfo];
            _sessionPool.Remove(testSessionInfo);
        }

        // Kill the session.
        return proxyManager.StopSession(requestData);
    }

    /// <summary>
    /// Gets a reference to the proxy object from the session pool.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="source">The source to be associated to this proxy.</param>
    /// <param name="runSettings">The run settings.</param>
    /// <param name="requestData">The request data.</param>
    ///
    /// <returns>The proxy object.</returns>
    public virtual ProxyOperationManager? TryTakeProxy(
        TestSessionInfo testSessionInfo,
        string source,
        string? runSettings,
        IRequestData requestData)
    {
        ValidateArg.NotNull(requestData, nameof(requestData));

        ProxyTestSessionManager? sessionManager;
        lock (_lockObject)
        {
            if (!_sessionPool.ContainsKey(testSessionInfo))
            {
                return null;
            }

            // Gets the session manager reference from the pool.
            sessionManager = _sessionPool[testSessionInfo];
        }

        try
        {
            // Deque an actual proxy to do work.
            var proxy = sessionManager.DequeueProxy(source, runSettings);

            // Make sure we use the per-request request data instead of the request data used when
            // creating the test session. Otherwise we can end up having irrelevant telemetry for
            // the current request being fulfilled or even duplicate telemetry which may cause an
            // exception to be thrown.
            proxy.RequestData = requestData;

            return proxy;
        }
        catch (InvalidOperationException ex)
        {
            // If we are unable to dequeue the proxy we just eat up the exception here as
            // it is safe to proceed.
            //
            // WARNING: This should not normally happen and it raises questions regarding the
            // test session pool operation and consistency.
            EqtTrace.Warning("TestSessionPool.ReturnProxy failed: {0}", ex.ToString());
        }

        return null;
    }

    /// <summary>
    /// Returns the proxy object to the session pool.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="proxyId">The proxy id to be returned.</param>
    ///
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public virtual bool ReturnProxy(TestSessionInfo testSessionInfo, int proxyId)
    {
        ProxyTestSessionManager? sessionManager;
        lock (_lockObject)
        {
            if (!_sessionPool.ContainsKey(testSessionInfo))
            {
                return false;
            }

            // Gets the session manager reference from the pool.
            sessionManager = _sessionPool[testSessionInfo];
        }

        try
        {
            // Try re-enqueueing the specified proxy.
            return sessionManager.EnqueueProxy(proxyId);
        }
        catch (Exception ex)
        {
            // If we are unable to re-enqueue the proxy, we just eat up the exception here as
            // it is safe to proceed. Returning a proxy is a fire-and-forget kind of operation,
            // and failing to return it for whatever reason should no longer be considered a
            // breaking scenario. In fact, this happens on a regular basis when two calls to
            // ReturnProxy are issued, one when handling a raw message signaling a discovery/run
            // complete, and one when actually processing this kind of message. As such, only the
            // first call will ever succeed, with the second one always failing. Another failing
            // scenario was attempting to return a "non-managed" testhost (one that can be obtained,
            // for example, by failing to match discovery/run criteria to session criteria, and as
            // such an on-demand testhost is spawned) to a test session. A "non-managed" testhost
            // has -1 for the Id, and the call to EnqueueProxy will fail and an exception will be
            // thrown. We have to make sure we catch that exception instead of relying on the caller
            // to perform sanity checks, hence why we expanded the type of exception that we handle
            // to generic exceptions too.
            EqtTrace.Warning("TestSessionPool.ReturnProxy failed: {0}", ex.ToString());
        }

        return false;
    }
}
