// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Detects CI/CD environment from environment variables.
    /// </summary>
    internal static class CIEnvironmentDetector
    {
        /// <summary>
        /// Detects the CI environment based on environment variables.
        /// </summary>
        /// <param name="environmentVariableReader">The environment variable reader to use.</param>
        /// <returns>A <see cref="string"/> if a CI environment is detected, null otherwise.</returns>
        internal static string? Detect(IEnvironmentVariableReader environmentVariableReader)
        {
            // GitHub Actions
            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("GITHUB_ACTIONS"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "GitHub Actions";
            }

            // Azure DevOps
            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("TF_BUILD"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "Azure DevOps";
            }

            // AppVeyor
            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("APPVEYOR"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "AppVeyor";
            }

            // Travis CI
            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("TRAVIS"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "Travis CI";
            }

            // CircleCI
            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("CIRCLECI"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "CircleCI";
            }

            // AWS CodeBuild
            if (!string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("CODEBUILD_BUILD_ID")))
            {
                return "AWS CodeBuild";
            }

            // Jenkins - requires both BUILD_ID and BUILD_URL
            if (!string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("BUILD_ID")) &&
                !string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("BUILD_URL")))
            {
                return "Jenkins";
            }

            // Google Cloud Build - requires both BUILD_ID and PROJECT_ID
            if (!string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("BUILD_ID")) &&
                !string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("PROJECT_ID")))
            {
                return "Google Cloud";
            }

            // TeamCity
            if (!string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("TEAMCITY_VERSION")))
            {
                return "TeamCity";
            }

            // JetBrains Space
            if (!string.IsNullOrEmpty(environmentVariableReader.GetEnvironmentVariable("JB_SPACE_API_URL")))
            {
                return "JetBrains Space";
            }

            // GitLab CI

            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("GITLAB_CI"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "GitLab CI";
            }

            // Generic CI - must be last as it's the most general
            if (string.Equals(environmentVariableReader.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return "other";
            }

            return null;
        }
    }
}
