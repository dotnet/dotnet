// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

/// <remarks>
/// Not parallelized: these tests mutate the process-wide VSTEST_CONNECTION_TIMEOUT environment
/// variable, which any concurrently running test that reads
/// <see cref="EnvironmentHelper.GetConnectionTimeout"/> would observe.
/// </remarks>
[TestClass]
[DoNotParallelize]
public class MtpClientOptionsFactoryTests
{
    private string? _originalTimeout;

    [TestInitialize]
    public void Initialize()
        => _originalTimeout = Environment.GetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout);

    [TestCleanup]
    public void Cleanup()
        => Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, _originalTimeout);

    [TestMethod]
    public void CreateOptionsIdentifiesVstestAsAStatelessClient()
    {
        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions();

        Assert.AreEqual("vstest", options.ClientName);
        Assert.IsFalse(options.IsStateful, "vstest drives a single discover-or-run session per launch.");
        Assert.IsFalse(options.DebuggerProvider);
        Assert.IsNotNull(options.Logger);
    }

    /// <summary>
    /// The MTP connection timeout must follow vstest's shared VSTEST_CONNECTION_TIMEOUT knob so a
    /// user extending the timeout for a slow environment affects the MTP path exactly as it affects
    /// every other vstest connection.
    /// </summary>
    [TestMethod]
    public void CreateOptionsHonoursTheSharedConnectionTimeoutOverride()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "300");

        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions();

        Assert.AreEqual(TimeSpan.FromSeconds(300), options.ConnectionTimeout);
    }

    [TestMethod]
    public void CreateOptionsFallsBackToTheSharedDefaultConnectionTimeout()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, null);

        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions();

        Assert.AreEqual(TimeSpan.FromSeconds(EnvironmentHelper.DefaultConnectionTimeout), options.ConnectionTimeout);
    }

    [TestMethod]
    public void CreateOptionsIgnoresAnUnparsableConnectionTimeout()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "not-a-number");

        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions();

        Assert.AreEqual(TimeSpan.FromSeconds(EnvironmentHelper.DefaultConnectionTimeout), options.ConnectionTimeout);
    }

    [TestMethod]
    public void CreateOptionsCopiesEnvironmentVariables()
    {
        var variables = new Dictionary<string, string?>
        {
            ["FOO"] = "bar",
            ["EMPTY"] = null,
        };

        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions(variables);

        Assert.AreEqual("bar", options.EnvironmentVariables["FOO"]);
        Assert.IsNull(options.EnvironmentVariables["EMPTY"]);
    }

    [TestMethod]
    public void CreateOptionsAcceptsNoEnvironmentVariables()
    {
        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions(null);

        Assert.IsEmpty(options.EnvironmentVariables);
    }

    [TestMethod]
    [DataRow("Error", TestMessageLevel.Error)]
    [DataRow("Critical", TestMessageLevel.Error)]
    [DataRow("Warning", TestMessageLevel.Warning)]
    [DataRow("Information", TestMessageLevel.Informational)]
    [DataRow("Debug", TestMessageLevel.Informational)]
    [DataRow("Trace", TestMessageLevel.Informational)]
    [DataRow("a-level-the-server-added-later", TestMessageLevel.Informational)]
    public void MapServerLogLevelMapsOntoVstestMessageLevels(string level, TestMessageLevel expected)
        => Assert.AreEqual(expected, MtpClientOptionsFactory.MapServerLogLevel(level));
}
