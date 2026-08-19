// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

/// <summary>
/// Regression tests for the MTP client shutdown path.
///
/// Before the retarget, exit was a fire-and-forget notification with no cancellation token. It is
/// now an awaited request/response call, which introduced two failure modes these tests pin:
/// passing the run's own (possibly already-cancelled) token would skip the handshake exactly when a
/// run is aborted, and an unbounded await would let a wedged test application hang the run.
/// </summary>
[TestClass]
public class MtpServerClientFactoryTests
{
    [TestMethod]
    public void TryExitAsksTheServerToExit()
    {
        var client = new FakeMtpServerClient();

        MtpServerClientFactory.TryExit(client);

        Assert.IsTrue(client.ExitCalled);
    }

    /// <summary>
    /// Cancelling or aborting a run is precisely when the run's token is already cancelled. Exit
    /// must not be tied to it, or the graceful shutdown handshake would be skipped in the one case
    /// it matters most. <see cref="MtpServerClientFactory.TryExit"/> takes no token at all, so this
    /// pins that it supplies an uncancelled, cancelable one of its own; the end-to-end proof that no
    /// run token is plumbed through lives in the proxy-manager tests.
    /// </summary>
    [TestMethod]
    public void TryExitSuppliesItsOwnUncancelledToken()
    {
        var client = new FakeMtpServerClient();

        MtpServerClientFactory.TryExit(client);

        Assert.IsTrue(client.ExitCalled);
        Assert.IsFalse(
            client.ExitToken.IsCancellationRequested,
            "Exit must run on a token that is not already cancelled.");
        Assert.IsTrue(
            client.ExitToken.CanBeCanceled,
            "The token must be cancelable, otherwise the exit timeout could never fire.");
    }

    /// <summary>
    /// A test application that never acknowledges exit must not hang the run. Disposal (which the
    /// caller performs afterwards) terminates the process regardless, so abandoning the handshake is
    /// safe.
    /// </summary>
    [TestMethod]
    public void TryExitGivesUpOnAnUnresponsiveServer()
    {
        var client = new FakeMtpServerClient { ExitDelay = TimeSpan.FromMinutes(5) };

        var stopwatch = Stopwatch.StartNew();
        MtpServerClientFactory.TryExit(client);
        stopwatch.Stop();

        Assert.IsLessThan(TimeSpan.FromMinutes(1), stopwatch.Elapsed, "TryExit must be bounded by its own timeout.");
    }

    [TestMethod]
    public void TryExitSwallowsServerFailures()
    {
        var client = new FakeMtpServerClient { ThrowFromExit = new InvalidOperationException("server refused to exit") };

        MtpServerClientFactory.TryExit(client);

        Assert.IsTrue(client.ExitCalled, "A failing exit must not propagate: the caller disposes the client next.");
    }

    /// <summary>
    /// The seam must default to the factory's own launcher, not to a test double. Asserting only
    /// that it is non-null would pass for any delegate, including a fake another test class left
    /// behind. Instead assert the delegate is implemented inside <see cref="MtpServerClientFactory"/>
    /// itself - the default is a lambda, so its target lives in a compiler-generated closure nested
    /// in the factory rather than on the factory type directly.
    /// </summary>
    [TestMethod]
    public void LaunchDefaultsToTheRealClientLauncher()
    {
        Type? declaringType = MtpServerClientFactory.Launch.Method.DeclaringType;

        Assert.IsNotNull(declaringType);
        Type outermost = declaringType;
        while (outermost.DeclaringType is { } parent)
        {
            outermost = parent;
        }

        Assert.AreEqual(
            typeof(MtpServerClientFactory),
            outermost,
            "The default seam must be the factory's own launcher, not a test double left behind by another test.");
    }
}
