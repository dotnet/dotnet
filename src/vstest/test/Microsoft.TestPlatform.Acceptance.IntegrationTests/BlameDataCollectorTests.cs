// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
#if NET
using System.Globalization;
#endif
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class BlameDataCollectorTests : AcceptanceTestBase
{
    public const string NETCOREANDFX = "net481;net11.0";
    public const string NET = "net11.0";
    private readonly string _procDumpPath;

    public BlameDataCollectorTests()
    {
        _procDumpPath = Path.Combine(_testEnvironment.LocalPackageDirectory, @"procdump\0.0.1\bin");
        var procDumpExePath = Path.Combine(_procDumpPath, "procdump.exe");
        if (!File.Exists(procDumpExePath))
        {
            throw new InvalidOperationException($"Procdump path {procDumpExePath} does not exist. "
                + "It is possible that antivirus deleted it from your nuget cache. "
                + "Delete the whole procdump folder in your nuget cache, and run tests again.");
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestMatrix(testHost: Net)]
    public void BlameDataCollectorShouldGiveCorrectTestCaseName(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("BlameUnitTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        InvokeVsTest(arguments);

        VaildateOutput(TempDirectory, "BlameUnitTestProject.UnitTest1.TestMethod2");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestMatrix(console: NetFx, testHost: NetFx)]
    [TestMatrix(console: Net, testHost: Net)]
    public void BlameDataCollectorShouldOutputDumpFile(RunnerInfo runnerInfo)
    {

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:CollectDump;DumpType=mini");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, " /testcasefilter:ExitWithStackoverFlow");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath,
        };

        InvokeVsTest(arguments, env);

        VaildateOutput(TempDirectory, "SampleUnitTestProject3.UnitTest1.ExitWithStackoverFlow", validateDumpFile: true);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestMatrix(console: Net, testHost: Net)]
    public void BlameDataCollectorShouldNotOutputDumpFileWhenNoCrashOccurs(RunnerInfo runnerInfo)
    {

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:CollectDump;DumpType=mini");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, " /testcasefilter:PassingTest");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        Assert.DoesNotContain(".dmp", StdOut, "it should not collect a dump, because nothing crashed");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    // This tests .net runner and .net framework runner, together with .net framework testhost.
    [TestMatrix(testHost: NetFx)]
    public void BlameDataCollectorShouldOutputDumpFileWhenNoCrashOccursButCollectAlwaysIsEnabled(RunnerInfo runnerInfo)
    {

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:CollectDump;DumpType=mini;CollectAlways=True");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, " /testcasefilter:PassingTest");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        Assert.MatchesRegex(new Regex("\\.dmp"), StdOut, "it should collect dump, even if nothing crashed");
    }

    [TestMethod]
    [TestMatrix(console: Net)]
    public void HangDumpOnTimeout(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("timeout.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        // Don't reduce this, 10s is about the safe minimum to not have flakiness.
        arguments = string.Concat(arguments, $@" /Blame:""CollectHangDump;HangDumpType=mini;TestTimeout=10s""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    // .NET testhost does not support dump on exit
    [TestMatrix(testHost: NetFx)]

    public void CrashDumpWhenThereIsNoTimeout(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("timeout.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=mini;CollectAlways=true;CollectHangDump;HangDumpType=mini""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    // .NET tfms do not support dump on exit, but runner does
    [TestMatrix(testHost: NetFx)]

    public void CrashDumpOnExit(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("timeout.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=mini;CollectAlways=true""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    [TestMatrix(console: Net)]
    public void CrashDumpOnStackOverflow(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("crash.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=mini""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void CrashDumpChildProcesses(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("child-crash.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=mini""");
        InvokeVsTest(arguments);

        ValidateDump(2);
    }

    [TestMethod]
    [TestMatrix(console: Net)]
    public void HangDumpChildProcesses(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("child-hang.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        // Don't reduce this, 10s is about the safe minimum to not have flakiness.
        arguments = string.Concat(arguments, $@" /Blame:""CollectHangDump;HangDumpType=mini;TestTimeout=10s""");
        InvokeVsTest(arguments);

        ValidateDump(2);
    }

    [TestMethod]
    [DoNotParallelize] // Modifies the test asset's runtimeconfig.json on disk.
    [TestMatrix(testHost: Net)]
    public void HangDumpShouldNotHangWhenTestHostFailsToStart(RunnerInfo runnerInfo)
    {
        // When testhost can't start (e.g. wrong runtime version), the inactivity timer
        // must not try to dump PID 0. It should exit cleanly because the timer only starts
        // after TestHostLaunched, which never fires here.
        SetTestEnvironment(_testEnvironment, runnerInfo);
        const string testAssetProjectName = "SimpleTestProjectMessedUpTargetFramework";
        var assemblyPath = GetTestDllForFramework(testAssetProjectName + ".dll", Core11TargetFramework);

        // Make testhost fail immediately by targeting a non-existent runtime.
        var runtimeConfigJson = Path.Combine(Path.GetDirectoryName(assemblyPath)!, testAssetProjectName + ".runtimeconfig.json");
        var originalContent = File.ReadAllText(runtimeConfigJson);
        try
        {
            var updatedContent = Regex.Replace(originalContent, @"""version""\s*:\s*""[^""]+""", @"""version"": ""9999.0.0""");
            File.WriteAllText(runtimeConfigJson, updatedContent);

            var arguments = PrepareArguments(assemblyPath, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
            arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
            arguments = string.Concat(arguments, $@" /Blame:""CollectHangDump;HangDumpType=mini;TestTimeout=30s""");
            InvokeVsTest(arguments);

            // vstest should exit with failure (testhost didn't start), but not hang and not crash.
            ExitCodeEquals(1);
            // Verify the failure was specifically because the runtime wasn't found, not some other error.
            // The .NET runtime error can appear in stderr or stdout depending on the platform/host.
            Assert.IsTrue(
                Regex.IsMatch(StdErr, "9999\\.0\\.0") || Regex.IsMatch(StdOut, "9999\\.0\\.0"),
                $"Expected '9999.0.0' in output.\nStdErr: {StdErr}\nStdOut: {StdOut}");
            Assert.DoesNotContain(".dmp", StdOut, "no dump should be collected when testhost never launched");
        }
        finally
        {
            File.WriteAllText(runtimeConfigJson, originalContent);
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [DoNotParallelize] // Installs/uninstalls procdump as machine-wide postmortem debugger via HKLM registry.
    [TestMatrix(testHost: NetFx)]
    [TestMatrix(testHost: Net)]
    public void BlameDataCollectorAeDebuggerShouldCollectDump(RunnerInfo runnerInfo)
    {
        // For convenience skip locally, but never skip in CI. If this cannot pass in CI we are not testing it at all.
        if (!IsCI && !IsAdministrator())
        {
            Assert.Inconclusive("User is not administrator, cannot setup the debugger, and cannot check the functionality.");
        }

        SetTestEnvironment(_testEnvironment, runnerInfo);

        // Install AeDebugger
        string dumpPath = Path.Combine(TempDirectory.Path, "Dumps");
        Directory.CreateDirectory(dumpPath);

        ExecuteVsTestConsole($"/AeDebugger:Install;ProcDumpToolDirectoryPath={_procDumpPath};DumpDirectoryPath={dumpPath}",
            out string standardTestOutput,
            out string standardErrorTestOutput,
            out int _);
        Assert.AreEqual(0, standardErrorTestOutput.Trim().Length);

        // Run test under postmortem monitoring
        var assemblyPaths = GetAssetFullPath("BlameUnitTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:MonitorPostmortemDebugger;DumpDirectoryPath={dumpPath}");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        InvokeVsTest(arguments);

        // Uninstall AeDebugger
        ExecuteVsTestConsole($"/AeDebugger:Uninstall;ProcDumpToolDirectoryPath={_procDumpPath}",
            out standardTestOutput,
            out standardErrorTestOutput,
            out int _);
        Assert.AreEqual(0, standardErrorTestOutput.Trim().Length);

        // We cannot be precise here procdump is at machine level so we can have more than one dump and not only the one for our test
        // We look for "at least" one dump file, is the best we can do without locking all tests.
        Assert.IsNotEmpty(Directory.GetFiles(TempDirectory.Path, "*.dmp", SearchOption.AllDirectories)
            .Where(x => Path.GetFileNameWithoutExtension(x).StartsWith("testhost")));
    }

    private static bool IsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent())
                       .IsInRole(WindowsBuiltInRole.Administrator);
    }

    private void ValidateDump(int expectedDumpCount = 1)
    {
        var attachments = StdOutWithWhiteSpace.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
            .SkipWhile(l => !l.Contains("Attachments:")).Skip(1)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        var output = string.Join(Environment.NewLine, attachments);
        if (!attachments.Any(a => a.Contains("Sequence_")))
        {
            // sequence file is pretty flaky, and easily substituted by diag log
            // throw new AssertFailedException("Expected Sequence file in Attachments, but there was none."
            //    + Environment.NewLine
            //    + output);
        }

        var dumps = attachments
            .Where(a => a.EndsWith(".dmp"))
            // On Windows we might collect conhost which tells us nothing
            // or WerFault in case we would start hanging during crash
            // we don't want these to make cross-platform checks more difficult
            // so we filter them out.
            .Where(a => !a.Contains("WerFault") && !a.Contains("conhost"))
            .Select(a => a.Trim()).ToList();

        if (dumps.Count < expectedDumpCount)
        {
            throw new AssertFailedException($"Expected at least {expectedDumpCount} dump file in Attachments, but there were {dumps.Count}."
                + Environment.NewLine
                + string.Join(Environment.NewLine, dumps));
        }

        var nonExistingDumps = new List<string>();
        var emptyDumps = new List<string>();
        foreach (var dump in dumps)
        {
            if (!File.Exists(dump))
            {
                nonExistingDumps.Add(dump);
            }
            else
            {
                var file = new FileInfo(dump);
                if (file.Length == 0)
                {
                    emptyDumps.Add(dump);
                }
            }
        }

        // allow some child dumps to be missing, they manage to terminate early from time to time
        if ((dumps.Count == 1 && nonExistingDumps.Any()) || (dumps.Count > 1 && nonExistingDumps.Count > 1)
            || emptyDumps.Any())
        {
            var err = new StringBuilder();
            err.AppendLine("Expected all dumps in the list of attachments to exist, and not be empty, but:");
            if (nonExistingDumps.Any())
            {
                err.AppendLine(
#if NET
                    CultureInfo.InvariantCulture,
#endif
                    $"{nonExistingDumps.Count} don't exist:")
                .AppendLine(string.Join(Environment.NewLine, nonExistingDumps));
            }

            if (emptyDumps.Any())
            {
                err.AppendLine(
#if NET
                    CultureInfo.InvariantCulture,
#endif
                    $"{emptyDumps.Count} are empty:")
                .AppendLine(string.Join(Environment.NewLine, emptyDumps));
            }

            err.AppendLine("Reported attachments:")
            .AppendLine(output);

            throw new AssertFailedException(err.ToString());
        }
    }

    private void VaildateOutput(TempDirectory tempDirectory, string testName, bool validateDumpFile = false)
    {
        bool isSequenceAttachmentReceived = false;
        bool isDumpAttachmentReceived = false;
        bool isValid = false;
        StdErrorContains(testName);
        StdOutputContains("Sequence_");
        var resultFiles = Directory.GetFiles(tempDirectory.Path, "*", SearchOption.AllDirectories);

        foreach (var file in resultFiles)
        {
            if (file.Contains("Sequence_"))
            {
                isSequenceAttachmentReceived = true;
                isValid = IsValidXml(file);
            }
            else if (validateDumpFile && file.Contains(".dmp"))
            {
                isDumpAttachmentReceived = true;
            }
        }

        Assert.IsTrue(isSequenceAttachmentReceived, "Sequence attachment was not received.");
        Assert.IsTrue(!validateDumpFile || isDumpAttachmentReceived, "Dump attachment was not received.");
        Assert.IsTrue(isValid, "Sequence attachment is not valid.");
    }

    private static bool IsValidXml(string xmlFilePath)
    {
        var file = File.OpenRead(xmlFilePath);
        var reader = XmlReader.Create(file);
        try
        {
            while (reader.Read())
            {
            }
            file.Dispose();
            return true;
        }
        catch (XmlException)
        {
            file.Dispose();
            return false;
        }
    }
}
