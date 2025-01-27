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
// this whole thing is complicated and depends on versions of OS and the target runtime
// keeping this for later
[TestCategory("Windows-Review")]
public class BlameDataCollectorTests : AcceptanceTestBase
{
    public const string NETCOREANDFX = "net462;net472;net8.0";
    public const string NET60 = "net8.0";
    private readonly string _procDumpPath;

    public BlameDataCollectorTests()
    {
        _procDumpPath = Path.Combine(_testEnvironment.PackageDirectory, @"procdump\0.0.1\bin");
        var procDumpExePath = Path.Combine(_procDumpPath, "procdump.exe");
        if (!File.Exists(procDumpExePath))
        {
            throw new InvalidOperationException($"Procdump path {procDumpExePath} does not exist. "
                + "It is possible that antivirus deleted it from your nuget cache. "
                + "Delete the whole procdump folder in your nuget cache, and run build, or restore");
        }
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void BlameDataCollectorShouldGiveCorrectTestCaseName(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("BlameUnitTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $" /diag:logs\\log.txt");
        InvokeVsTest(arguments);

        VaildateOutput(TempDirectory, "BlameUnitTestProject.UnitTest1.TestMethod2");
    }

    [TestMethod]
    [Ignore]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void BlameDataCollectorShouldOutputDumpFile(RunnerInfo runnerInfo)
    {

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject3.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:CollectDump");
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
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void BlameDataCollectorShouldNotOutputDumpFileWhenNoCrashOccurs(RunnerInfo runnerInfo)
    {

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:CollectDump");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, " /testcasefilter:PassingTest");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        Assert.IsFalse(StdOut.Contains(".dmp"), "it should not collect a dump, because nothing crashed");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    // This tests .net runner and .net framework runner, together with .net framework testhost
    [NetFullTargetFrameworkDataSource]
    // .NET does not support crash dump on exit
    // [NetCoreTargetFrameworkDataSource]
    public void BlameDataCollectorShouldOutputDumpFileWhenNoCrashOccursButCollectAlwaysIsEnabled(RunnerInfo runnerInfo)
    {

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /Blame:CollectDump;CollectAlways=True");
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, " /testcasefilter:PassingTest");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        StringAssert.Matches(StdOut, new Regex("\\.dmp"), "it should collect dump, even if nothing crashed");
    }

    [TestMethod]
    [NetCoreRunner("net462;net472;net8.0;net9.0")]
    // should make no difference, keeping for easy debug
    // [NetFrameworkRunner("net462;net472;net8.0;net9.0")]
    public void HangDumpOnTimeout(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("timeout.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectHangDump;HangDumpType=full;TestTimeout=3s"" /Diag:{TempDirectory.Path}/log.txt");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    // net8.0 does not support dump on exit
    [NetCoreRunner("net462;net472")]
    // should make no difference, keeping for easy debug
    // [NetFrameworkRunner("net462;net472")]

    public void CrashDumpWhenThereIsNoTimeout(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("timeout.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=full;CollectAlways=true;CollectHangDump""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    // .NET tfms do not support dump on exit, but runner does
    [NetCoreRunner("net462;net472")]
    // [NetFrameworkRunner("net462;net472")]

    public void CrashDumpOnExit(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("timeout.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=full;CollectAlways=true""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    [NetCoreRunner("net462;net472;net8.0;net9.0")]
    // should make no difference, keeping for easy debug
    // [NetFrameworkRunner("net462;net472;net8.0;net9.0")]
    public void CrashDumpOnStackOverflow(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("crash.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=full""");

        var env = new Dictionary<string, string?>
        {
            ["PROCDUMP_PATH"] = _procDumpPath
        };

        InvokeVsTest(arguments, env);

        ValidateDump();
    }

    [TestMethod]
    [NetCoreRunner(NET60)]
    // should make no difference, keeping for easy debug
    // [NetFrameworkRunner(NET50)]
    public void CrashDumpChildProcesses(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("child-crash.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectDump;DumpType=full""");
        InvokeVsTest(arguments);

        ValidateDump(2);
    }

    [TestMethod]
    [NetCoreRunner("net462;net472;net8.0;net9.0")]
    // should make no difference, keeping for easy debug
    // [NetFrameworkRunner("net462;net472;net8.0;net9.0")]
    public void HangDumpChildProcesses(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("child-hang.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");
        arguments = string.Concat(arguments, $@" /Blame:""CollectHangDump;HangDumpType=full;TestTimeout=15s""");
        InvokeVsTest(arguments);

        ValidateDump(2);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void BlameDataCollectorAeDebuggerShouldCollectDump(RunnerInfo runnerInfo)
    {
        if (!IsAdministrator())
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
        Assert.IsTrue(standardErrorTestOutput.Trim().Length == 0);

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
        Assert.IsTrue(standardErrorTestOutput.Trim().Length == 0);

        // We cannot be precise here procdump is at machine level so we can have more than one dump and not only the one for our test
        // We look for "at least" one dump file, is the best we can do without locking all tests.
        Assert.IsTrue(Directory.GetFiles(TempDirectory.Path, "*.dmp", SearchOption.AllDirectories)
            .Where(x => Path.GetFileNameWithoutExtension(x).StartsWith("testhost")).Count() > 0);
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
