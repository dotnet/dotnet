// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtpMSTestProject;

[TestClass]
public class UnitTests
{
    // Values are produced at runtime so the MSTest analyzers do not flag the asserts as
    // always-true / always-false.
    private static int Add(int a, int b) => a + b;

    [TestMethod]
    public void TestPasses()
    {
        Assert.AreEqual(4, Add(2, 2));
    }

    [TestMethod]
    public void TestPassesToo()
    {
        // Writes to stdout and stderr (and still passes) so the MTP-under-vstest stdout acceptance test can
        // assert the captured per-test output is surfaced to the console and TRX loggers.
        System.Console.WriteLine("MTP_STDOUT_MARKER");
        System.Console.Error.WriteLine("MTP_STDERR_MARKER");
        Assert.AreEqual(2, Add(1, 1));
    }

    [TestMethod]
    public void TestFails()
    {
        Assert.Fail("intentional failure to validate outcome mapping");
    }

    [TestMethod]
    [Ignore("intentionally skipped")]
    public void TestSkipped()
    {
        Assert.Fail("should never run");
    }

    // Verifies that environment variables declared in a runsettings RunConfiguration/EnvironmentVariables
    // block are injected into the self-hosted MTP process. The check is opted into by the
    // CHECK_RUNSETTINGS_VAR control variable, which the env-var acceptance test passes as a *process*
    // environment variable (inherited by the host regardless of the fix), so the guard is decisive: when
    // runsettings injection works MTP_FROM_RUNSETTINGS carries the expected value and the test passes; when
    // it is broken the variable is absent and the assert fails. In every other run the control variable is
    // unset, so the test is a no-op and stays green.
    [TestMethod]
    public void RunSettingsEnvironmentVariableIsInjected()
    {
        if (System.Environment.GetEnvironmentVariable("CHECK_RUNSETTINGS_VAR") != "1")
        {
            return;
        }

        Assert.AreEqual("mtp-runsettings-value", System.Environment.GetEnvironmentVariable("MTP_FROM_RUNSETTINGS"));
    }
}
