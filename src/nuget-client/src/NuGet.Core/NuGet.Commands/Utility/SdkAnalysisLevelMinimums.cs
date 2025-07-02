// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;

namespace NuGet.Commands
{
    /// <summary>
    /// Defines constants that specify the minimum SDK analysis level required to enable new features.
    /// </summary>
    internal static class SdkAnalysisLevelMinimums
    {
        /// <summary>
        /// Minimum SDK Analysis Level required to enable HTTP Errors is 9.0.100.
        /// </summary>
        internal static readonly NuGetVersion V9_0_100 = new("9.0.100");

        /// <summary>
        /// Minimum SDK Analysis Level required for:
        /// <list type="bullet">
        /// <item>warning for packages and projects that cannot be pruned</item>
        /// <item>enabling the new restore algorithm for lock files</item>
        /// <item>error when CPM not used and PackageReference does not have version</item>
        /// </list>
        /// </summary>
        internal static readonly NuGetVersion V10_0_100 = new("10.0.100");

        /// <summary>
        /// Determines whether the feature is enabled based on the SDK analysis level.
        /// </summary>
        /// <param name="sdkAnalysisLevel">The project SdkAnalysisLevel value </param>
        /// <param name="usingMicrosoftNetSdk">Is it SDK project or not</param>
        /// <param name="minSdkVersion">The minimum version of the SDK required for the feature to be enabled.</param>
        /// <returns>Returns true if the feature should be enabled based on the given parameters; otherwise, false.</returns>
        internal static bool IsEnabled(NuGetVersion sdkAnalysisLevel, bool usingMicrosoftNetSdk, NuGetVersion minSdkVersion)
        {
            if (sdkAnalysisLevel != null && sdkAnalysisLevel >= minSdkVersion ||
                sdkAnalysisLevel == null && usingMicrosoftNetSdk == false)
            {
                // SdkAnalysisLevel >= minSdkVersion or SdkAnalysisLevel is null and not using Microsoft NET Sdk
                return true;
            }
            else
            {
                // SdkAnalysisLevel < minSdkVersion or SdkAnalysisLevel is null and using Microsoft NET Sdk
                return false;
            }
        }
    }
}
