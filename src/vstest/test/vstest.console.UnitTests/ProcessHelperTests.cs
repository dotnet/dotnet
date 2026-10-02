// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;

/// <summary>
/// Tests for <see cref="ProcessHelper"/>.
/// </summary>
/// <remarks>
/// <see cref="ProcessHelper.WaitForErrorStreamToDrain"/> is the bounded wait that lets the process
/// exit callback observe the complete standard error output of a crashed test host. Without it, the exit
/// callback could read the asynchronously-collected stderr before all ErrorDataReceived callbacks had run,
/// dropping a crash callstack such as "Stack overflow." (the cause of the flaky
/// RunTestsShouldThrowOnStackOverflowException test).
/// </remarks>
[TestClass]
public class ProcessHelperTests
{
    private const int BudgetMs = 500;

    [TestMethod]
    public void GetCurrentProcessFileNameShouldReturnThePathOfTheRunningExecutable()
    {
        var processHelper = new ProcessHelper();

        string? currentProcessFileName = processHelper.GetCurrentProcessFileName();

        using var currentProcess = Process.GetCurrentProcess();
        string? mainModuleFileName = currentProcess.MainModule?.FileName;

#if NET
        // On .NET we prefer Environment.ProcessPath, because MainModule can report an injected loader
        // rather than the running executable under sandboxes such as proot. See issue #16446.
        // ProcessPath is nullable, so mirror the fallback the production code performs instead of
        // assuming it is always set.
        string? expectedFileName = Environment.ProcessPath ?? mainModuleFileName;
#else
        // .NET Framework has no Environment.ProcessPath, so the MainModule behavior must be preserved.
        string? expectedFileName = mainModuleFileName;
#endif

        Assert.AreEqual(expectedFileName, currentProcessFileName);
    }

#if NET
    [TestMethod]
    public void GetCurrentProcessFileNameShouldPreferProcessPathOverDifferentMainModuleFileName()
    {
        var processHelper = new ProcessHelper();

        string? currentProcessFileName = processHelper.GetCurrentProcessFileName(
            getProcessPath: () => "/usr/share/dotnet/dotnet",
            getMainModuleFileName: () => "/proot/loader");

        Assert.AreEqual("/usr/share/dotnet/dotnet", currentProcessFileName);
    }

    [TestMethod]
    public void GetCurrentProcessFileNameShouldNotReadMainModuleWhenProcessPathIsAvailable()
    {
        var processHelper = new ProcessHelper();

        string? currentProcessFileName = processHelper.GetCurrentProcessFileName(
            getProcessPath: () => "/usr/share/dotnet/dotnet",
            getMainModuleFileName: () => throw new InvalidOperationException("MainModule must not be read when ProcessPath is available."));

        Assert.AreEqual("/usr/share/dotnet/dotnet", currentProcessFileName);
    }
#else
    [TestMethod]
    public void GetCurrentProcessFileNameShouldNotReadProcessPathOnNetFramework()
    {
        var processHelper = new ProcessHelper();

        string? currentProcessFileName = processHelper.GetCurrentProcessFileName(
            getProcessPath: () => throw new InvalidOperationException("ProcessPath is unavailable on .NET Framework."),
            getMainModuleFileName: () => "dotnet.exe");

        Assert.AreEqual("dotnet.exe", currentProcessFileName);
    }
#endif

    [TestMethod]
    [DataRow("/usr/share/dotnet/dotnet")]
    [DataRow(null)]
    public void GetCurrentProcessFileNameShouldFallBackToMainModuleWhenProcessPathIsNull(string? mainModuleFileName)
    {
        var processHelper = new ProcessHelper();
        int mainModuleReads = 0;

        string? currentProcessFileName = processHelper.GetCurrentProcessFileName(
            getProcessPath: () => null,
            getMainModuleFileName: () =>
            {
                mainModuleReads++;
                return mainModuleFileName;
            });

        Assert.AreEqual(mainModuleFileName, currentProcessFileName);
        Assert.AreEqual(1, mainModuleReads);
    }

    [TestMethod]
    public void WaitForErrorStreamToDrainShouldReturnOnceTheErrorStreamCloses()
    {
        using var errorStreamClosed = new ManualResetEventSlim(initialState: false);

        // The stream reaches EOF a little later, mimicking a slow ErrorDataReceived delivery.
        var setter = new Thread(() =>
        {
            Thread.Sleep(150);
            errorStreamClosed.Set();
        })
        { IsBackground = true };

        var stopwatch = Stopwatch.StartNew();
        setter.Start();
        ProcessHelper.WaitForErrorStreamToDrain(errorStreamClosed, budgetMilliseconds: 5000, elapsedMilliseconds: 0);
        stopwatch.Stop();
        setter.Join();

        Assert.IsTrue(errorStreamClosed.IsSet, "The method must wait until the error stream is drained.");
        Assert.IsLessThan(
            3000L,
            stopwatch.ElapsedMilliseconds,
            $"The method should return shortly after the stream closes, not at the budget timeout (took {stopwatch.ElapsedMilliseconds} ms).");
    }

    [TestMethod]
    public void WaitForErrorStreamToDrainShouldBeBoundedWhenTheErrorStreamNeverCloses()
    {
        // Models a grandchild process keeping the pipe open: EOF never arrives.
        using var errorStreamClosed = new ManualResetEventSlim(initialState: false);

        var stopwatch = Stopwatch.StartNew();
        ProcessHelper.WaitForErrorStreamToDrain(errorStreamClosed, BudgetMs, elapsedMilliseconds: 0);
        stopwatch.Stop();

        Assert.IsFalse(errorStreamClosed.IsSet, "Precondition: the stream never closes in this test.");
        Assert.IsGreaterThanOrEqualTo(
            150L,
            stopwatch.ElapsedMilliseconds,
            $"The method should wait roughly the budget for the stream (waited only {stopwatch.ElapsedMilliseconds} ms).");
        Assert.IsLessThan(
            5000L,
            stopwatch.ElapsedMilliseconds,
            $"The wait must be bounded so it cannot hang (took {stopwatch.ElapsedMilliseconds} ms).");
    }

    [TestMethod]
    public void WaitForErrorStreamToDrainShouldNotWaitWhenTheBudgetIsAlreadyExhausted()
    {
        // The exit wait above already consumed the whole budget (e.g. a slow grandchild), so there is no
        // time left to wait for stderr - we must not add any latency on top.
        using var errorStreamClosed = new ManualResetEventSlim(initialState: false);

        var stopwatch = Stopwatch.StartNew();
        ProcessHelper.WaitForErrorStreamToDrain(errorStreamClosed, BudgetMs, elapsedMilliseconds: BudgetMs + 100);
        stopwatch.Stop();

        Assert.IsFalse(errorStreamClosed.IsSet);
        Assert.IsLessThan(
            250L,
            stopwatch.ElapsedMilliseconds,
            $"With the budget exhausted the method must return immediately (took {stopwatch.ElapsedMilliseconds} ms).");
    }

    [TestMethod]
    public void WaitForErrorStreamToDrainShouldReturnImmediatelyWhenThereIsNoErrorStream()
    {
        var stopwatch = Stopwatch.StartNew();
        ProcessHelper.WaitForErrorStreamToDrain(errorStreamClosed: null, BudgetMs, elapsedMilliseconds: 0);
        stopwatch.Stop();

        Assert.IsLessThan(
            250L,
            stopwatch.ElapsedMilliseconds,
            $"With no redirected error stream the method must be a no-op (took {stopwatch.ElapsedMilliseconds} ms).");
    }
}
