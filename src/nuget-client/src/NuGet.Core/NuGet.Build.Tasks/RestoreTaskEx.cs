// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents an MSBuild task that performs a command-line based restore.
    /// </summary>
    public sealed class RestoreTaskEx : StaticGraphRestoreTaskBase
    {
        public RestoreTaskEx()
            : this(EnvironmentVariableWrapper.Instance)
        {
        }

        internal RestoreTaskEx(IEnvironmentVariableReader environmentVariableReader)
            : base(environmentVariableReader)
        {
        }
        /// <summary>
        /// Gets or sets a value representing the parameters to use for the MSBuild binary logger.
        /// </summary>
        public string BinaryLoggerParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not assets should be deleted for projects that don't support PackageReference.
        /// </summary>
        public bool CleanupAssetsForUnsupportedProjects { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not parallel restore should be enabled.
        /// Defaults to <code>false</code> if the current machine only has a single processor.
        /// </summary>
        public bool DisableParallel { get; set; } = Environment.ProcessorCount == 1;

        /// <summary>
        /// Gets or sets a value indicating whether or not the MSBuild binary logger should be enabled.
        /// </summary>
        public string EnableBinaryLogger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not, in PackageReference based projects, all dependencies should be resolved
        /// even if the last restore was successful. Specifying this flag is similar to deleting the project.assets.json file. This
        /// does not bypass the http-cache.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to recompute the dependencies and update the lock file without any warning.
        /// </summary>
        public bool ForceEvaluate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not warnings and errors should be logged.
        /// </summary>
        public bool HideWarningsAndErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to ignore failing or missing package sources.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the restore is allowed to interact with the user through a prompt or dialog.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to avoid using cached packages.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to avoid using http caching.
        /// </summary>
        public bool NoHttpCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to restore projects using packages.config.
        /// </summary>
        public bool RestorePackagesConfig { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to embed files produced by restore in the MSBuild binary logger.
        /// </summary>
        public string EmbedFilesInBinlog { get; set; }

        protected override string DebugEnvironmentVariableName => "DEBUG_RESTORE_TASK_EX";

        protected override Dictionary<string, string> GetOptions()
        {
            Dictionary<string, string> options = base.GetOptions();

            // If the user specified a valid value for EnableBinaryLogger, use it. Otherwise, treat it as "unspecified"
            bool? enableBinaryLogger = !string.IsNullOrWhiteSpace(EnableBinaryLogger) && bool.TryParse(EnableBinaryLogger, out bool result)
                ? result
                : null;

            // If the user has not explicitly disabled the binary logger but has provided parameters, enable the binary logger with their parameters
            if (enableBinaryLogger != false && !string.IsNullOrWhiteSpace(BinaryLoggerParameters))
            {
                options[nameof(EnableBinaryLogger)] = bool.TrueString;
                options[nameof(BinaryLoggerParameters)] = Uri.EscapeDataString(BinaryLoggerParameters);
            }
            // If the user has explicitly enabled the binary logger, enable it
            else if (enableBinaryLogger == true)
            {
                options[nameof(EnableBinaryLogger)] = bool.TrueString;
            }

            options[nameof(CleanupAssetsForUnsupportedProjects)] = CleanupAssetsForUnsupportedProjects.ToString();
            options[nameof(DisableParallel)] = DisableParallel.ToString();
            options[nameof(Force)] = Force.ToString();
            options[nameof(ForceEvaluate)] = ForceEvaluate.ToString();
            options[nameof(HideWarningsAndErrors)] = HideWarningsAndErrors.ToString();
            options[nameof(IgnoreFailedSources)] = IgnoreFailedSources.ToString();
            options[nameof(Interactive)] = Interactive.ToString();
            options[nameof(NoCache)] = NoCache.ToString();
            options[nameof(NoHttpCache)] = NoHttpCache.ToString();
            options[nameof(RestorePackagesConfig)] = RestorePackagesConfig.ToString();
            if (!string.IsNullOrWhiteSpace(EmbedFilesInBinlog))
            {
                options[nameof(EmbedFilesInBinlog)] = EmbedFilesInBinlog;
            }

            return options;
        }
    }
}
