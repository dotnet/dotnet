// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Represents the outcomes of a test case.
/// </summary>
public enum TestOutcome
{
    /// <summary>
    /// Test Case Does Not Have an outcome.
    /// </summary>
    None = 0,

    /// <summary>
    /// Test Case Passed
    /// </summary>
    Passed = 1,

    /// <summary>
    /// Test Case Failed
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Test Case Skipped
    /// </summary>
    Skipped = 3,

    /// <summary>
    /// Test Case Not found
    /// </summary>
    NotFound = 4,
}
