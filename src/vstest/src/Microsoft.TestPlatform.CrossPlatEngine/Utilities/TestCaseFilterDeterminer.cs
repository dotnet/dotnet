// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;

internal class TestCaseFilterDeterminer
{
    private const int MaxLengthOfTestCaseFilterToShow = 256;

    internal static string ShortenTestCaseFilterIfRequired(string testCaseFilter)
    {
        string shortenTestCaseFilter = testCaseFilter.Length > MaxLengthOfTestCaseFilterToShow
            ? testCaseFilter.Substring(0, MaxLengthOfTestCaseFilterToShow) + "..."
            : testCaseFilter;
        return shortenTestCaseFilter;
    }
}
