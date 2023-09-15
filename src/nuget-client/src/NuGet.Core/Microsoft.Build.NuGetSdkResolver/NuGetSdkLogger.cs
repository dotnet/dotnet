// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using NuGet.Common;

using ILogger = NuGet.Common.ILogger;

namespace Microsoft.Build.NuGetSdkResolver
{
    /// <summary>
    /// An implementation of <see cref="ILogger" /> that logs messages to an <see cref="SdkLogger" />.
    /// </summary>
    /// <inheritdoc />
    internal sealed class NuGetSdkLogger : ILogger
    {
        /// <summary>
        /// A collection of errors that have been logged.
        /// </summary>
        private readonly List<string> _errors = new List<string>();

        /// <summary>
        /// A <see cref="SdkLogger"/> to forward events to.
        /// </summary>
        private readonly SdkLogger _sdkLogger;

        /// <summary>
        /// A collection of warnings that have been logged.
        /// </summary>
        private readonly List<string> _warnings = new List<string>();

        /// <summary>
        /// Initializes a new instance of the NuGetLogger class.
        /// </summary>
        /// <param name="sdkLogger">A <see cref="SdkLogger"/> to forward events to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sdkLogger" /> is <see langword="null" />.</exception>
        public NuGetSdkLogger(SdkLogger sdkLogger)
        {
            _sdkLogger = sdkLogger ?? throw new ArgumentNullException(nameof(sdkLogger));
        }

        /// <summary>
        /// Gets a <see cref="IReadOnlyCollection{T}" /> of error messages that have been logged.
        /// </summary>
        public IReadOnlyCollection<string> Errors => _errors;

        /// <summary>
        /// Gets a <see cref="IReadOnlyCollection{T}" /> of warning messages that have been logged.
        /// </summary>
        public IReadOnlyCollection<string> Warnings => _warnings;

        /// <inheritdoc cref="ILogger.Log(NuGet.Common.LogLevel, string)" />
        public void Log(LogLevel level, string data)
        {
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Verbose:
                    // Debug and Verbose verbosity in NuGet maps to a low importance message in MSBuild
                    _sdkLogger.LogMessage(data, MessageImportance.Low);
                    break;

                case LogLevel.Information:
                    // Information verbosity in NuGet maps to a normal importance message in MSBuild
                    _sdkLogger.LogMessage(data, MessageImportance.Normal);
                    break;

                case LogLevel.Minimal:
                    // Minimal verbosity in NuGet maps to a high importance message in MSBuild
                    _sdkLogger.LogMessage(data, MessageImportance.High);
                    break;

                case LogLevel.Warning:
                    _warnings.Add(data);
                    break;

                case LogLevel.Error:
                    _errors.Add(data);
                    break;
            }
        }

        /// <inheritdoc cref="ILogger.LogAsync(ILogMessage)" />
        public void Log(ILogMessage message) => Log(message.Level, message.Message);

        /// <inheritdoc cref="ILogger.LogAsync(NuGet.Common.LogLevel, string)" />
        public Task LogAsync(LogLevel level, string data)
        {
            Log(level, data);

            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ILogger.LogAsync(ILogMessage)" />
        public Task LogAsync(ILogMessage message)
        {
            Log(message);

            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ILogger.LogDebug(string)" />
        public void LogDebug(string data) => Log(LogLevel.Debug, data);

        /// <inheritdoc cref="ILogger.LogError(string)" />
        public void LogError(string data) => Log(LogLevel.Error, data);

        /// <inheritdoc cref="ILogger.LogInformation(string)" />
        public void LogInformation(string data) => Log(LogLevel.Information, data);

        /// <inheritdoc cref="ILogger.LogInformationSummary(string)" />
        public void LogInformationSummary(string data) => Log(LogLevel.Information, data);

        /// <inheritdoc cref="ILogger.LogMinimal(string)" />
        public void LogMinimal(string data) => Log(LogLevel.Minimal, data);

        /// <inheritdoc cref="ILogger.LogVerbose(string)" />
        public void LogVerbose(string data) => Log(LogLevel.Verbose, data);

        /// <inheritdoc cref="ILogger.LogWarning(string)" />
        public void LogWarning(string data) => Log(LogLevel.Warning, data);
    }
}
