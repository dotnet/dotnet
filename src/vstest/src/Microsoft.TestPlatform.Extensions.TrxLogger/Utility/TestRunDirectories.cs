// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.Utility;

/// <summary>
/// Class to deal with directories.
/// </summary>
internal sealed class TestRunDirectories
{
    /// <summary>
    /// Computes the test results directory, relative to the root results directory (whatever that may be)
    /// </summary>
    /// <param name="testExecutionId">The test's execution ID</param>
    /// <returns>
    /// The test results directory (&lt;testExecutionId&gt;), under which test-specific result files should be stored
    /// </returns>
    public static string GetRelativeTestResultsDirectory(Guid testExecutionId)
    {
        return testExecutionId.ToString();
    }
}
