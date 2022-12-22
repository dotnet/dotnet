// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionDecorators;

internal class SerialTestRunDecorator : ITestExecutor, ITestExecutor2, IDisposable
{
    private readonly SemaphoreSlim _runSequentialEvent = new(1);

    public ITestExecutor OriginalTestExecutor { get; }

    public SerialTestRunDecorator(ITestExecutor originalTestExecutor)
    {
        OriginalTestExecutor = originalTestExecutor;
    }

    public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
        if (IsSerialTestRunEnabled(runContext))
        {
            EqtTrace.Info("SerializeTestRunDecorator.RunTests: Test cases will run sequentially");
            if (tests is null)
            {
                return;
            }

            foreach (TestCase testToRun in tests)
            {
                _runSequentialEvent.Wait();
                OriginalTestExecutor.RunTests(new List<TestCase> { testToRun }, runContext, new SerializeTestRunDecoratorFrameworkHandle(frameworkHandle!, _runSequentialEvent));
            }
        }
        else
        {
            OriginalTestExecutor.RunTests(tests, runContext, frameworkHandle);
        }
    }

    public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
        if (IsSerialTestRunEnabled(runContext))
        {
            EqtTrace.Error("<ForceOneTestAtTimePerTestHost>true</ForceOneTestAtTimePerTestHost> is not supported for sources test run.");
            frameworkHandle?.SendMessage(TestMessageLevel.Error, Resources.Resources.SerialTestRunInvalidScenario);
        }
        else
        {
            OriginalTestExecutor.RunTests(sources, runContext, frameworkHandle);
        }
    }

    public bool ShouldAttachToTestHost(IEnumerable<string>? sources, IRunContext runContext)
    {
        if (OriginalTestExecutor is ITestExecutor2 executor)
        {
            return executor.ShouldAttachToTestHost(sources, runContext);
        }

        // If the adapter doesn't implement the new test executor interface we should attach to
        // the default test host by default to preserve old behavior.
        return true;
    }

    public bool ShouldAttachToTestHost(IEnumerable<TestCase>? tests, IRunContext runContext)
    {
        if (OriginalTestExecutor is ITestExecutor2 executor)
        {
            return executor.ShouldAttachToTestHost(tests, runContext);
        }

        // If the adapter doesn't implement the new test executor interface we should attach to
        // the default test host by default to preserve old behavior.
        return true;
    }

    public void Cancel()
        => OriginalTestExecutor.Cancel();

    private static bool IsSerialTestRunEnabled(IRunContext? runContext)
    {
        if (runContext is null || runContext.RunSettings is null || runContext.RunSettings.SettingsXml is null)
        {
            return false;
        }

        XElement runSettings = XElement.Parse(runContext.RunSettings.SettingsXml);
        XElement? forceOneTestAtTimePerTestHost = runSettings.Element("RunConfiguration")?.Element("ForceOneTestAtTimePerTestHost");
        return forceOneTestAtTimePerTestHost is not null && bool.TryParse(forceOneTestAtTimePerTestHost.Value, out bool enabled) && enabled;
    }

    public void Dispose()
        => _runSequentialEvent.Dispose();
}

internal class SerializeTestRunDecoratorFrameworkHandle : IFrameworkHandle
{
    private readonly IFrameworkHandle _frameworkHandle;
    private readonly SemaphoreSlim _testEnd;

    public SerializeTestRunDecoratorFrameworkHandle(IFrameworkHandle frameworkHandle, SemaphoreSlim testEnd)
    {
        _frameworkHandle = frameworkHandle;
        _testEnd = testEnd;
    }

    public bool EnableShutdownAfterTestRun { get => _frameworkHandle.EnableShutdownAfterTestRun; set => _frameworkHandle.EnableShutdownAfterTestRun = value; }

    public int LaunchProcessWithDebuggerAttached(string filePath, string? workingDirectory, string? arguments, IDictionary<string, string?>? environmentVariables)
        => _frameworkHandle.LaunchProcessWithDebuggerAttached(filePath, workingDirectory, arguments, environmentVariables);

    public void RecordAttachments(IList<AttachmentSet> attachmentSets)
        => _frameworkHandle.RecordAttachments(attachmentSets);

    public void RecordEnd(TestCase testCase, TestOutcome outcome)
    {
        _frameworkHandle.RecordEnd(testCase, outcome);
        _testEnd.Release();
    }

    public void RecordResult(TestResult testResult)
        => _frameworkHandle.RecordResult(testResult);

    public void RecordStart(TestCase testCase)
        => _frameworkHandle.RecordStart(testCase);

    public void SendMessage(TestMessageLevel testMessageLevel, string message)
        => _frameworkHandle.SendMessage(testMessageLevel, message);
}
