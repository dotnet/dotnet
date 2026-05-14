// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;

/// <summary>
/// The test case end args.
/// </summary>
public class TestCaseEndArgs : InProcDataCollectionArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEndArgs"/> class.
    /// </summary>
    /// <param name="dataCollectionContext">
    /// The data Collection Context.
    /// </param>
    /// <param name="outcome">
    /// The outcome.
    /// </param>
    public TestCaseEndArgs(DataCollectionContext dataCollectionContext, TestOutcome outcome)
    {
        TestOutcome = outcome;
        DataCollectionContext = dataCollectionContext;
    }

    /// <summary>
    /// Gets the outcome.
    /// </summary>
    public TestOutcome TestOutcome { get; private set; }

    /// <summary>
    /// Gets the data collection context.
    /// </summary>
    public DataCollectionContext DataCollectionContext { get; private set; }
}
