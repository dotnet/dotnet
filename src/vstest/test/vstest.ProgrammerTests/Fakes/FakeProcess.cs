// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeProcess
{
    public int Id { get; internal set; }
    public string Name { get; init; }
    public string Path { get; }
    public string? Arguments { get; set; }
    public string WorkingDirectory { get; }
    public IDictionary<string, string?> EnvironmentVariables { get; }
    // TODO: Throw if already set
    public Action<object, string>? ErrorCallback { get; set; }
    // TODO: Throw if already set
    public Action<object>? ExitCallback { get; set; }
    // TODO: Throw if already set
    public Action<object, string>? OutputCallback { get; set; }
    public PlatformArchitecture Architecture { get; init; } = PlatformArchitecture.X64;
    public FakeErrorAggregator FakeErrorAggregator { get; }
    public string? ErrorOutput { get; init; }
    public int ExitCode { get; init; } = -1;
    public bool Started { get; private set; }
    public bool Exited { get; private set; }
    public TestProcessStartInfo TestProcessStartInfo { get; internal set; }

    public FakeProcess(FakeErrorAggregator fakeErrorAggregator, string path, string? arguments = null, string? workingDirectory = null, IDictionary<string, string?>? environmentVariables = null, Action<object, string>? errorCallback = null, Action<object>? exitCallBack = null, Action<object, string>? outputCallback = null)
    {
        FakeErrorAggregator = fakeErrorAggregator;
        Path = path;
        Name = System.IO.Path.GetFileName(path);
        Arguments = arguments;
        WorkingDirectory = workingDirectory ?? System.IO.Path.GetDirectoryName(path) ?? throw new InvalidOperationException($"Path {path} does not have a parent directory.");
        EnvironmentVariables = environmentVariables ?? new Dictionary<string, string?>();
        ErrorCallback = errorCallback;
        ExitCallback = exitCallBack;
        OutputCallback = outputCallback;

        TestProcessStartInfo = new TestProcessStartInfo()
        {
            FileName = Path,
            Arguments = Arguments,
            WorkingDirectory = WorkingDirectory,
            EnvironmentVariables = EnvironmentVariables,
            // TODO: is this even used anywhere
            CustomProperties = new Dictionary<string, string>(),
        };
    }

    internal static FakeProcess? EnsureFakeProcess(object? process)
    {
        return process as FakeProcess;
    }

    internal void SetId(int id)
    {
        if (Id != 0)
            throw new InvalidOperationException($"Cannot set Id to {id} for fake process {Name}, {Id}, because it was already set.");

        Id = id;
    }

    internal void Start()
    {
        if (Started)
            throw new InvalidOperationException($"Cannot start process {Name} - {Id} because it was already started before.");

        Started = true;
    }

    internal void Exit()
    {
        if (!Started)
            throw new InvalidOperationException($"Cannot exit process {Name} - {Id} because it was not started before.");

        // We want to call the exit callback just once. This is behavior inherent to being a real process,
        // that also exits only once.
        var exited = Exited;
        Exited = true;
        if (!exited && ExitCallback != null)
        {
            ExitCallback(this);
        }
    }

    public override string ToString()
    {
        var state = !Started
            ? "not started"
            : !Exited
                ? "running"
                : "exited";
        return $"{(Id != default ? Id : "<no id>")} {Name ?? "<no name>"} {state}";
    }
}
