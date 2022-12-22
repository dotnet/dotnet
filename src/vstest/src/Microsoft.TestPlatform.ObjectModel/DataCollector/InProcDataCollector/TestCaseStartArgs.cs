// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;

/// <summary>
/// The test case start args.
/// </summary>
public class TestCaseStartArgs : InProcDataCollectionArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseStartArgs"/> class.
    /// </summary>
    /// <param name="testCase">
    /// The test case.
    /// </param>
    public TestCaseStartArgs(TestCase? testCase)
    {
        TestCase = testCase;
    }

    /// <summary>
    /// Gets the test case.
    /// </summary>
    public TestCase? TestCase { get; private set; }
}
