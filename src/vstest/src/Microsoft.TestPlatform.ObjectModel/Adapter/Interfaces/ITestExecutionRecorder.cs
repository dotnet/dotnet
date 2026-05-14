// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Used for recording test results and test messages.
/// </summary>
public interface ITestExecutionRecorder : IMessageLogger
{
    /// <summary>
    /// Notify the framework about the test result.
    /// </summary>
    /// <param name="testResult">Test Result to be sent to the framework.</param>
    /// <exception cref="TestCanceledException">Exception thrown by the framework when an executor attempts to send
    /// test result to the framework when the test(s) is canceled. </exception>
    void RecordResult(TestResult testResult);


    /// <summary>
    /// Notify the framework about starting of the test case.
    /// Framework sends this event to data collectors enabled in the run. If no data collector is enabled, then the event is ignored.
    /// </summary>
    /// <param name="testCase">testcase which will be started.</param>
    void RecordStart(TestCase testCase);

    /// <summary>
    /// Notify the framework about completion of the test case.
    /// Framework sends this event to data collectors enabled in the run. If no data collector is enabled, then the event is ignored.
    /// </summary>
    /// <param name="testCase">testcase which has completed.</param>
    /// <param name="outcome">outcome of the test case.</param>
    void RecordEnd(TestCase testCase, TestOutcome outcome);


    /// <summary>
    /// Notify the framework about run level attachments.
    /// </summary>
    /// <param name="attachmentSets">attachments produced in this run.</param>
    void RecordAttachments(IList<AttachmentSet> attachmentSets);

}
