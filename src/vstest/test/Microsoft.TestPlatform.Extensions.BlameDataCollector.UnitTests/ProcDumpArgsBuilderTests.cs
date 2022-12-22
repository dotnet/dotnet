// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector.UnitTests;

[TestClass]
public class ProcDumpArgsBuilderTests
{
    private readonly int _defaultProcId = 1234;
    private readonly string _defaultDumpFileName = "dump";

    [TestMethod]
    public void BuildHangBasedProcDumpArgsShouldCreateCorrectArgString()
    {
        var procDumpArgsBuilder = new ProcDumpArgsBuilder();
        var argString = procDumpArgsBuilder.BuildHangBasedProcDumpArgs(_defaultProcId, _defaultDumpFileName, false);
        Assert.AreEqual("-accepteula -n 1 1234 dump.dmp", argString);
    }

    [TestMethod]
    public void BuildHangBasedProcDumpArgsWithFullDumpEnabledShouldCreateCorrectArgString()
    {
        var procDumpArgsBuilder = new ProcDumpArgsBuilder();
        var argString = procDumpArgsBuilder.BuildHangBasedProcDumpArgs(_defaultProcId, _defaultDumpFileName, true);
        Assert.AreEqual("-accepteula -n 1 -ma 1234 dump.dmp", argString);
    }

    [TestMethod]
    public void BuildTriggerBasedProcDumpArgsShouldCreateCorrectArgString()
    {
        var procDumpArgsBuilder = new ProcDumpArgsBuilder();
        var argString = procDumpArgsBuilder.BuildTriggerBasedProcDumpArgs(_defaultProcId, _defaultDumpFileName, new List<string> { "a", "b" }, false);
        Assert.AreEqual("-accepteula -e 1 -g -t -f a -f b 1234 dump.dmp", argString);
    }

    [TestMethod]
    public void BuildTriggerProcDumpArgsWithFullDumpEnabledShouldCreateCorrectArgString()
    {
        var procDumpArgsBuilder = new ProcDumpArgsBuilder();
        var argString = procDumpArgsBuilder.BuildTriggerBasedProcDumpArgs(_defaultProcId, _defaultDumpFileName, new List<string> { "a", "b" }, true);
        Assert.AreEqual("-accepteula -e 1 -g -t -ma -f a -f b 1234 dump.dmp", argString);
    }

    [TestMethod]
    public void BuildTriggerProcDumpArgsWithAlwaysCollectShouldCreateCorrectArgString()
    {
        var procDumpArgsBuilder = new ProcDumpArgsBuilder();
        var argString = procDumpArgsBuilder.BuildTriggerBasedProcDumpArgs(_defaultProcId, _defaultDumpFileName, new List<string> { "a", "b" }, true);

        // adds -t for collect on every process exit
        Assert.AreEqual("-accepteula -e 1 -g -t -ma -f a -f b 1234 dump.dmp", argString);
    }

    [TestMethod]
    public void BuildTriggerProcDumpArgsWith_VSTEST_DUMP_PROCDUMPARGUMENTS_EnvironmentVariable()
    {
        Mock<IEnvironmentVariableHelper> environmentVariableHelper = new();
        environmentVariableHelper.Setup(e => e.GetEnvironmentVariable("VSTEST_DUMP_PROCDUMPARGUMENTS")).Returns("-e 1 -g -t -ma");

        var procDumpArgsBuilder = new ProcDumpArgsBuilder(environmentVariableHelper.Object);
        var argString = procDumpArgsBuilder.BuildTriggerBasedProcDumpArgs(_defaultProcId, _defaultDumpFileName, new List<string> { "a", "b" }, true);
        Assert.AreEqual("-accepteula -e 1 -g -t -ma 1234 dump.dmp", argString);
    }
}
