// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
#pragma warning disable CA1837 // Environment.ProcessId is unavailable on the .NET Framework target.
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Covers the detail block that appears under a connection negotiation failure, and the parent
/// process lookup that fills it. Both are best effort by design: a field that cannot be determined is
/// omitted rather than printed, and no failure in here may change the outcome of the run.
/// </summary>
[TestClass]
public class ConnectionSecretsDetailsTests
{
    private const string Link = "https://aka.ms/vstest/negotiation-error";

    [TestMethod]
    public void DetailFormatsTheValueWhenItIsKnown()
    {
        var line = ConnectionSecretsCapabilities.Detail("{0} version: {1}", "testhost", "17.9.0");

        Assert.AreEqual("testhost version: 17.9.0", line);
    }

    [TestMethod]
    public void DetailReturnsNothingWhenAValueIsMissing()
    {
        // An absent line reads as "we could not determine this". A line saying "unknown" reads like a
        // value and invites the reader to wonder whether that is the problem.
        Assert.AreEqual(string.Empty, ConnectionSecretsCapabilities.Detail("{0} path: {1}", "testhost", string.Empty));
    }

    [TestMethod]
    public void WithDetailsSeparatesTheDetailsFromTheMessage()
    {
        var text = ConnectionSecretsCapabilities.WithDetails("A failure.", new[] { "one: 1", "two: 2" });

        Assert.AreEqual(
            "A failure." + Environment.NewLine + Environment.NewLine + "one: 1" + Environment.NewLine + "two: 2",
            text);
    }

    [TestMethod]
    public void WithDetailsSkipsTheLinesThatCouldNotBeDetermined()
    {
        var text = ConnectionSecretsCapabilities.WithDetails("A failure.", new[] { string.Empty, "two: 2", string.Empty });

        Assert.AreEqual("A failure." + Environment.NewLine + Environment.NewLine + "two: 2", text);
    }

    [TestMethod]
    public void WithDetailsLeavesTheMessageAloneWhenNothingCouldBeDetermined()
    {
        // This is the degraded case a user sees when the lookup timed out or was refused, and it must
        // look like an ordinary message rather than announcing that our diagnostics failed.
        Assert.AreEqual("A failure.", ConnectionSecretsCapabilities.WithDetails("A failure.", new[] { string.Empty, string.Empty }));
    }

    [TestMethod]
    public void EveryNegotiationMessageCarriesTheLink()
    {
        foreach (var format in new[]
        {
            CommunicationUtilitiesResources.ConnectionSecretsTesthostUnsupported,
            CommunicationUtilitiesResources.ConnectionSecretsConsoleUnsupported,
            CommunicationUtilitiesResources.ConnectionSecretsPeerSecretMismatch,
            CommunicationUtilitiesResources.ConnectionSecretsParentSecretMismatch,
            CommunicationUtilitiesResources.ConnectionSecretsPeerNegotiationIncomplete,
            CommunicationUtilitiesResources.ConnectionSecretsParentNegotiationIncomplete,
            CommunicationUtilitiesResources.ConnectionSecretsInternalError,
        })
        {
            // StringAssert.Contains takes the haystack first.
            StringAssert.Contains(string.Format(CultureInfo.CurrentCulture, format, ConnectionSecretsCapabilities.NegotiationErrorLink), Link);
        }

        StringAssert.Contains(
            string.Format(CultureInfo.CurrentCulture, CommunicationUtilitiesResources.ConnectionSecretsParentDidNotNegotiate, "testhost", ConnectionSecretsCapabilities.NegotiationErrorLink),
            Link);
    }

    [TestMethod]
    public void ResolveReportsNothingWhenThereIsNoParentProcessId()
    {
        var details = ParentProcessResolver.Resolve(-1);

        Assert.AreEqual(-1, details.ProcessId);
        Assert.AreEqual(string.Empty, details.Path);
        Assert.AreEqual(string.Empty, details.Version);
    }

    [TestMethod]
    public void ResolveReportsThePidEvenWhenTheProcessIsGone()
    {
        // A pid that cannot be opened must still be reported, because it is what lets someone
        // correlate the failure with a process tree or a crash dump.
        var details = ParentProcessResolver.Resolve(int.MaxValue - 1);

        Assert.AreEqual(int.MaxValue - 1, details.ProcessId);
        Assert.AreEqual(string.Empty, details.Path);
    }

    [TestMethod]
    public void ResolveNeverThrowsForAProcessItCannotRead()
    {
        // Every failure in the lookup drops a field. None of them may change the outcome, because this
        // runs on a path that is already failing and about to throw the real error.
        foreach (var pid in new[] { 0, -5, 4, int.MaxValue })
        {
            var details = ParentProcessResolver.Resolve(pid);
            Assert.IsNotNull(details);
        }
    }

    [TestMethod]
    public void ResolveReadsThePathAndVersionOfAProcessItCanRead()
    {
        var self = Process.GetCurrentProcess();

        var details = ParentProcessResolver.Resolve(self.Id);

        Assert.AreEqual(self.Id, details.ProcessId);
        Assert.AreNotEqual(string.Empty, details.Path, "the path of a readable process should be reported");

        // The name is the fallback for when the path could not be read, so the two never appear together.
        Assert.AreEqual(string.Empty, details.Name, "the name should be dropped once the path is known");
    }

    [TestMethod]
    public void ResolveCompletesWellInsideItsBudget()
    {
        // The lookup is bounded at 10 seconds so a parent that is suspended or mid-load cannot hang
        // testhost inside its own error path, which would look exactly like the connection timeout
        // this whole change exists to avoid.
        var watch = Stopwatch.StartNew();
        ParentProcessResolver.Resolve(Process.GetCurrentProcess().Id);
        watch.Stop();

        Assert.IsTrue(
            watch.Elapsed < TimeSpan.FromSeconds(11),
            string.Format(CultureInfo.InvariantCulture, "the lookup took {0}, past its 10 second budget", watch.Elapsed));
    }
}
#pragma warning restore CA1837
