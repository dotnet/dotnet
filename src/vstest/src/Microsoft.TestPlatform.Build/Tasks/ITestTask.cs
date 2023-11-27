// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.TestPlatform.Build.Tasks;

internal interface ITestTask : ITask, ICancelableTask
{
    ITaskItem? TestFileFullPath { get; set; }
    string? VSTestSetting { get; set; }
    ITaskItem[]? VSTestTestAdapterPath { get; set; }
    string? VSTestFramework { get; set; }
    string? VSTestPlatform { get; set; }
    string? VSTestTestCaseFilter { get; set; }
    string[]? VSTestLogger { get; set; }
    bool VSTestListTests { get; set; }
    string? VSTestDiag { get; set; }
    string[]? VSTestCLIRunSettings { get; set; }
    ITaskItem? VSTestConsolePath { get; set; }
    ITaskItem? VSTestResultsDirectory { get; set; }
    string? VSTestVerbosity { get; set; }
    string[]? VSTestCollect { get; set; }
    bool VSTestBlame { get; set; }
    bool VSTestBlameCrash { get; set; }
    string? VSTestBlameCrashDumpType { get; set; }
    bool VSTestBlameCrashCollectAlways { get; set; }
    bool VSTestBlameHang { get; set; }
    string? VSTestBlameHangDumpType { get; set; }
    string? VSTestBlameHangTimeout { get; set; }
    ITaskItem? VSTestTraceDataCollectorDirectoryPath { get; set; }
    bool VSTestNoLogo { get; set; }
    string? VSTestArtifactsProcessingMode { get; set; }
    string? VSTestSessionCorrelationId { get; set; }

    TaskLoggingHelper Log { get; }
}
