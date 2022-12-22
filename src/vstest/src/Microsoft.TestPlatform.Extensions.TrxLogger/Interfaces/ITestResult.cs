// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

internal interface ITestResult
{
    TestResultId Id { get; }
    string? ResultType { get; set; }
    string StdOut { get; set; }
    string StdErr { get; set; }
    string DebugTrace { get; set; }
    string TestResultsDirectory { get; }
    string RelativeTestResultsDirectory { get; }
    string ErrorMessage { get; set; }
    string ErrorStackTrace { get; set; }
    string ComputerName { get; }
    string[] TextMessages { get; set; }
    int DataRowInfo { get; set; }
    DateTime StartTime { get; set; }
    DateTime EndTime { get; set; }
    TimeSpan Duration { get; set; }
    TestOutcome Outcome { get; set; }
}
