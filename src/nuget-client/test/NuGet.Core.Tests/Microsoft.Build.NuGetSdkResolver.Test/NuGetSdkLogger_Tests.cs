// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Build.Framework;
using NuGet.Common;
using Xunit;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    /// <summary>
    /// Tests for LazyFormattedEventArgs
    /// </summary>
    public class NuGetSdkLoggerTests
    {
        /// <summary>
        /// Verifies that <see cref="NuGetSdkLogger.Log(LogLevel, string)" /> tranlates to MSBuild logged messages with the correct type (message, error, warning) and message importance.
        /// </summary>
        /// <param name="logLevel">THe <see cref="LogLevel" /> of a message.</param>
        /// <param name="message">The message to be logged</param>
        /// <param name="expectedMessageImportance">The expected <see cref="MessageImportance" /> of the message.</param>
        [Theory]
        [InlineData(LogLevel.Verbose, nameof(LogLevel.Verbose), MessageImportance.Low)]
        [InlineData(LogLevel.Debug, nameof(LogLevel.Debug), MessageImportance.Low)]
        [InlineData(LogLevel.Information, nameof(LogLevel.Information), MessageImportance.Normal)]
        [InlineData(LogLevel.Minimal, nameof(LogLevel.Verbose), MessageImportance.High)]
        [InlineData(LogLevel.Error, nameof(LogLevel.Error), null)]
        [InlineData(LogLevel.Warning, nameof(LogLevel.Warning), null)]
        public void Log_UsesCorrectMessageImportance_WhenLogLevelIsSpecified(LogLevel logLevel, string message, MessageImportance? expectedMessageImportance)
        {
            var mockLogger = new MockSdkLogger();

            var logger = new NuGetSdkLogger(mockLogger);

            logger.Log(logLevel, message);

            switch (logLevel)
            {
                case LogLevel.Warning:
                    logger.Errors.Should().BeEmpty();
                    logger.Warnings.Should().BeEquivalentTo(new[] { message });
                    break;

                case LogLevel.Error:
                    logger.Errors.Should().BeEquivalentTo(new[] { message });
                    logger.Warnings.Should().BeEmpty();
                    break;

                case LogLevel.Debug:
                case LogLevel.Verbose:
                case LogLevel.Information:
                case LogLevel.Minimal:
                    logger.Errors.Should().BeEmpty();
                    logger.Warnings.Should().BeEmpty();
                    mockLogger.LoggedMessages.Should().BeEquivalentTo(new[] { (message, expectedMessageImportance) });
                    break;
            }
        }

        /// <summary>
        /// Verifies that <see cref="NuGetSdkLogger.LogAsync(LogLevel, string)" /> tranlates to MSBuild logged messages with the correct type (message, error, warning) and message importance.
        /// </summary>
        /// <param name="logLevel">THe <see cref="LogLevel" /> of a message.</param>
        /// <param name="message">The message to be logged</param>
        /// <param name="expectedMessageImportance">The expected <see cref="MessageImportance" /> of the message.</param>
        [Theory]
        [InlineData(LogLevel.Verbose, nameof(LogLevel.Verbose), MessageImportance.Low)]
        [InlineData(LogLevel.Debug, nameof(LogLevel.Debug), MessageImportance.Low)]
        [InlineData(LogLevel.Information, nameof(LogLevel.Information), MessageImportance.Normal)]
        [InlineData(LogLevel.Minimal, nameof(LogLevel.Verbose), MessageImportance.High)]
        [InlineData(LogLevel.Error, nameof(LogLevel.Error), null)]
        [InlineData(LogLevel.Warning, nameof(LogLevel.Warning), null)]
        public async Task LogAsync_UsesCorrectMessageImportance_WhenLogLevelIsSpecified(LogLevel logLevel, string message, MessageImportance? expectedMessageImportance)
        {
            var mockLogger = new MockSdkLogger();

            var sdkLogger = new NuGetSdkLogger(mockLogger);

            await sdkLogger.LogAsync(logLevel, message);

            switch (logLevel)
            {
                case LogLevel.Warning:
                    sdkLogger.Errors.Should().BeEmpty();
                    sdkLogger.Warnings.Should().BeEquivalentTo(new[] { message });
                    break;

                case LogLevel.Error:
                    sdkLogger.Errors.Should().BeEquivalentTo(new[] { message });
                    sdkLogger.Warnings.Should().BeEmpty();
                    break;

                case LogLevel.Debug:
                case LogLevel.Verbose:
                case LogLevel.Information:
                case LogLevel.Minimal:
                    sdkLogger.Errors.Should().BeEmpty();
                    sdkLogger.Warnings.Should().BeEmpty();
                    mockLogger.LoggedMessages.Should().BeEquivalentTo(new[] { (message, expectedMessageImportance) });
                    break;
            }
        }
    }
}
