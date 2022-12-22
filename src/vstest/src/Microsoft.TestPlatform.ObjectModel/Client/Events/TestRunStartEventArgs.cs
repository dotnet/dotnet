// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Event arguments used when test run starts
/// </summary>
public class TestRunStartEventArgs : EventArgs
{
    /// <summary>
    /// Constructor for creating event args object
    /// </summary>
    /// <param name="testRunCriteria"> Test run criteria to be used for test run. </param>
    public TestRunStartEventArgs(TestRunCriteria testRunCriteria)
    {
        TestRunCriteria = testRunCriteria;
    }

    /// <summary>
    /// Test run criteria to be used for test run
    /// </summary>
    public TestRunCriteria TestRunCriteria { get; private set; }
}
