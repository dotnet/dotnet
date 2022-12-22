// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionDecorators;

internal class SerialTestRunDecoratorFrameworkHandle : IFrameworkHandle
{
    private readonly IFrameworkHandle _frameworkHandle;
    private readonly SemaphoreSlim _testEnd;

    public SerialTestRunDecoratorFrameworkHandle(IFrameworkHandle frameworkHandle, SemaphoreSlim testEnd)
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
