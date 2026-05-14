// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestRuntimeProvider : ITestRuntimeProvider
{
    public FakeProcessHelper FakeProcessHelper { get; }
    public FakeCommunicationEndpoint FakeCommunicationEndpoint { get; }
    public FakeCommunicationChannel FakeCommunicationChannel { get; }
    public FakeErrorAggregator FakeErrorAggregator { get; }
    public FakeProcess TestHostProcess { get; private set; }
    public FakeFileHelper FileHelper { get; }
    public List<FakeTestDllFile> TestDlls { get; }

    // TODO: make this configurable?
    public bool Shared => false;

    public event EventHandler<HostProviderEventArgs>? HostLaunched;
    public event EventHandler<HostProviderEventArgs>? HostExited;

    public FakeTestRuntimeProvider(FakeProcessHelper fakeProcessHelper, FakeProcess fakeTestHostProcess, FakeFileHelper fakeFileHelper, List<FakeTestDllFile> fakeTestDlls, FakeCommunicationEndpoint fakeCommunicationEndpoint, FakeErrorAggregator fakeErrorAggregator)
    {
        FakeProcessHelper = fakeProcessHelper;
        TestHostProcess = fakeTestHostProcess;
        FileHelper = fakeFileHelper;
        TestDlls = fakeTestDlls;
        FakeCommunicationEndpoint = fakeCommunicationEndpoint;
        FakeCommunicationChannel = fakeCommunicationEndpoint.Channel;
        FakeErrorAggregator = fakeErrorAggregator;

        var architectures = fakeTestDlls.Select(dll => dll.Architecture).Distinct().ToList();
        var frameworks = fakeTestDlls.Select(dll => dll.FrameworkName).Distinct().ToList();

        if (architectures.Count > 1)
            throw new InvalidOperationException($"The provided dlls have more than 1 architecture {architectures.JoinByComma()}. Fake TestRuntimeProvider cannot have dlls with mulitple architectures, because real testhost process can also run just with a single architecture.");

        if (frameworks.Count > 1)
            throw new InvalidOperationException($"The provided dlls have more than 1 target framework {frameworks.JoinByComma()}. Fake TestRuntimeProvider cannot have dlls with mulitple target framework, because real testhost process can also run just a single target framework.");

        fakeTestDlls.ForEach(FileHelper.AddFakeFile);

        fakeProcessHelper.AddFakeProcess(fakeTestHostProcess);
        TestHostProcess.ExitCallback = p =>
        {
            // TODO: Validate the process we are passed is actually the same as TestHostProcess
            // TODO: Validate we already started the process.
            var process = (FakeProcess)p;
            // TODO: When we exit, eventually there are no subscribers, maybe we should review if we don't lose the error output sometimes, in unnecessary way
            HostExited?.Invoke(this, new HostProviderEventArgs(process.ErrorOutput!, process.ExitCode, process.Id));
        };
    }

    public bool CanExecuteCurrentRunConfiguration(string? runsettingsXml)
    {
        // <TargetPlatform>x86</TargetPlatform>
        // <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>

        return true;
    }

    public Task CleanTestHostAsync(CancellationToken cancellationToken)
    {
        if (TestHostProcess == null)
            throw new InvalidOperationException("Cannot clean testhost, no testhost process was started");
        FakeProcessHelper.TerminateProcess(TestHostProcess);
        return Task.CompletedTask;
    }

    public TestHostConnectionInfo GetTestHostConnectionInfo()
    {
        return FakeCommunicationEndpoint.TestHostConnectionInfo;
    }

    public TestProcessStartInfo GetTestHostProcessStartInfo(IEnumerable<string> sources, IDictionary<string, string?>? environmentVariables, TestRunnerConnectionInfo connectionInfo)
    {
        // TODO: do we need to do more here? How to link testhost to the fake one we "start"?
        return TestHostProcess.TestProcessStartInfo;
    }

    public IEnumerable<string> GetTestPlatformExtensions(IEnumerable<string> sources, IEnumerable<string> extensions)
    {
        // send extensions so we send InitializeExecutionMessage
        return new[] { @"c:\temp\extension.dll" };
    }

    public IEnumerable<string> GetTestSources(IEnumerable<string> sources)
    {
        // gives testhost opportunity to translate sources to something else,
        // e.g. in uwp the main exe is returned, rather than the dlls that dlls that are tested
        return sources;
    }

    public void Initialize(IMessageLogger? logger, string runsettingsXml)
    {
        // TODO: this is called twice, is that okay?
        // TODO: and also by HandlePartialRunComplete after the test run has completed and we aborted because the client disconnected

        // do nothing
    }

    public Task<bool> LaunchTestHostAsync(TestProcessStartInfo testHostStartInfo, CancellationToken cancellationToken)
    {
        if (TestHostProcess.TestProcessStartInfo.FileName != testHostStartInfo.FileName)
            throw new InvalidOperationException($"Tried to start a different process than the one associated with this provider: File name is {testHostStartInfo.FileName} is not the same as the fake process associated with this provider {TestHostProcess.TestProcessStartInfo.FileName}.");

        FakeProcessHelper.StartFakeProcess(TestHostProcess);
        HostLaunched?.Invoke(this, new HostProviderEventArgs("Fake testhost launched", 0, TestHostProcess.Id));

        return Task.FromResult(true);
    }

    public void SetCustomLauncher(ITestHostLauncher customLauncher)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{nameof(FakeTestRuntimeProvider)} - ({TestHostProcess.ToString() ?? "<no process>"}) - {FakeCommunicationChannel}";
    }
}
