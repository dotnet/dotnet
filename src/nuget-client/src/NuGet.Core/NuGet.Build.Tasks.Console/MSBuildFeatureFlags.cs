// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
#pragma warning disable RS0030 // Do not used banned APIs.  This class sets MSBuild feature flags which are environment variables, but they only apply to the NuGet.Build.Tasks.Console process.
namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents a class for enabling MSBuild feature flags.
    /// </summary>
    internal static class MSBuildFeatureFlags
    {
        /// <summary>
        /// Represents a regular expression that matches any file spec that contains a wildcard * or ? and does not end in "proj".
        /// </summary>
        private const string SkipWildcardRegularExpression = @"[*?]+.*(?<!proj)$";

        /// <summary>
        /// Gets or sets a value indicating if wildcard expansions for the entire process should be cached.
        /// </summary>
        /// <remarks>
        /// More info here: https://github.com/microsoft/msbuild/blob/master/src/Shared/Traits.cs#L55
        /// </remarks>
        public static bool EnableCacheFileEnumerations
        {
            get => string.Equals(Environment.GetEnvironmentVariable("MSBuildCacheFileEnumerations"), "1", StringComparison.Ordinal);
            set => Environment.SetEnvironmentVariable("MSBuildCacheFileEnumerations", value ? "1" : null);
        }

        /// <summary>
        /// Gets or sets a value indicating if all projects should be treated as read-only which enables an optimized way of
        /// reading them.
        /// </summary>
        /// <remarks>
        /// More info here: https://github.com/microsoft/msbuild/blob/master/src/Build/ElementLocation/XmlDocumentWithLocation.cs#L392
        /// </remarks>
        public static bool LoadAllFilesAsReadonly
        {
            get => string.Equals(Environment.GetEnvironmentVariable("MSBuildLoadAllFilesAsReadonly"), "1", StringComparison.Ordinal);
            set => Environment.SetEnvironmentVariable("MSBuildLoadAllFilesAsReadonly", value ? "1" : null);
        }

        /// <summary>
        /// Gets or sets the full path to MSBuild that should be used to evaluate projects.
        /// </summary>
        /// <remarks>
        /// MSBuild is not installed globally anymore as of version 15.0.  Processes doing evaluations must set this environment variable for the toolsets
        /// to be found by MSBuild (stuff like $(MSBuildExtensionsPath).
        /// More info here: https://github.com/microsoft/msbuild/blob/master/src/Shared/BuildEnvironmentHelper.cs#L125
        /// </remarks>
        public static string MSBuildExeFilePath
        {
            get => Environment.GetEnvironmentVariable("MSBUILD_EXE_PATH");
            set => Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", value);
        }

        /// <summary>
        /// Gets or sets a set of regular expressions that MSBuild uses in order to skip the evaluation of items. If any expression
        /// matches the item, its Include is left as a literal string rather than expanding it.  .NET Core SDK ships with default
        /// item includes like **\*, **\*.cs, and **\*.resx.
        ///
        /// Users can also unknowingly introduce a run away wildcard like:
        ///   $(MyProperty)\**
        ///
        /// If $(MyProperty) is not set this would evaluate to "\**" which would cause MSBuild to enumerate the entire disk.
        ///
        /// The only wildcard NuGet needs to respect is something like **\*.*proj which allows users to specify that they want to
        /// restore every MSBuild project in their repo.
        /// </summary>
        /// <remarks>
        /// More info here: https://github.com/microsoft/msbuild/blob/master/src/Build/Utilities/EngineFileUtilities.cs#L221
        /// </remarks>
        public static bool SkipEagerWildcardEvaluations
        {
            get => !string.Equals(Environment.GetEnvironmentVariable("MSBuildSkipEagerWildCardEvaluationRegexes"), null, StringComparison.Ordinal);
            set => Environment.SetEnvironmentVariable("MSBuildSkipEagerWildCardEvaluationRegexes", SkipWildcardRegularExpression);
        }
    }
}
