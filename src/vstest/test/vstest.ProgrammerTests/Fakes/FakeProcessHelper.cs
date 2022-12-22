// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeProcessHelper : IProcessHelper
{
    // starting from 100 for no particular reason
    // I want to avoid processId 0 and 1 as they are
    // "reserved" on Windows (0) and Linux (both 0 and 1)
    private static readonly SequentialId IdSource = new(100);

    public FakeProcess CurrentProcess { get; }
    public List<FakeProcess> Processes { get; } = new();

    public FakeErrorAggregator FakeErrorAggregator { get; }

    public FakeProcessHelper(FakeErrorAggregator fakeErrorAggregator, FakeProcess currentProcess)
    {
        FakeErrorAggregator = fakeErrorAggregator;
        CurrentProcess = currentProcess;
        AddFakeProcess(currentProcess);
    }

    public void AddFakeProcess(FakeProcess process)
    {
        var id = IdSource.Next();
        process.SetId(id);
        Processes.Add(process);
    }

    public PlatformArchitecture GetCurrentProcessArchitecture()
    {
        return CurrentProcess.Architecture;
    }

    public string GetCurrentProcessFileName()
    {
        return CurrentProcess.Path;
    }

    public int GetCurrentProcessId()
    {
        return CurrentProcess.Id;
    }

    public string GetCurrentProcessLocation()
    {
        // TODO: how is this different from Path
        throw new NotImplementedException();
    }

    public string GetNativeDllDirectory()
    {
        throw new NotImplementedException();
    }

    public IntPtr GetProcessHandle(int processId)
    {
        throw new NotImplementedException();
    }

    public int GetProcessId(object? process)
    {
        var fakeProcess = FakeProcess.EnsureFakeProcess(process);
        return fakeProcess?.Id ?? -1;
    }

    public string GetProcessName(int processId)
    {
        var process = Processes.Single(p => p.Id == processId);
        return process.Name;
    }

    public string GetTestEngineDirectory()
    {
        throw new NotImplementedException();
    }

    public object LaunchProcess(string processPath, string? arguments, string? workingDirectory, IDictionary<string, string?>? environmentVariables, Action<object?, string>? errorCallback, Action<object>? exitCallBack, Action<object?, string>? outputCallback)
    {
        // TODO: Throw if setting says we can't start new processes;
        var process = new FakeProcess(FakeErrorAggregator, processPath, arguments, workingDirectory, environmentVariables, errorCallback, exitCallBack, outputCallback);
        Processes.Add(process);
        process.Start();

        return process;
    }

    public void SetExitCallback(int processId, Action<object?>? callbackAction)
    {
        // TODO: implement?
    }

    public void TerminateProcess(object? process)
    {
        if (process is FakeProcess fakeProcess)
            fakeProcess.Exit();
    }

    public bool TryGetExitCode(object? process, out int exitCode)
    {
        exitCode = (process as FakeProcess)?.ExitCode ?? -1;
        return true;
    }

    public void WaitForProcessExit(object? process)
    {
        // todo: implement for timeouts?
    }

    internal void StartFakeProcess(FakeProcess process)
    {
        // TODO: mark the process as started. Do not add a new process if it did not exist.
        if (!Processes.Contains(process))
            throw new InvalidOperationException($"Cannot start process {process.Name} - {process.Id} because it was not found in the list of known fake processes.");

        process.Start();
    }

    public PlatformArchitecture GetProcessArchitecture(int processId)
    {
        throw new NotImplementedException();
    }
}
