// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Extensions.Internal;
using OpenTelemetry.Internal;
using OpenTelemetry.Logs;

namespace Microsoft.Extensions.Logging;

/// <summary>
/// Contains OpenTelemetry logging SDK extensions.
/// </summary>
public static class OpenTelemetryLoggingExtensions
{
    /// <summary>
    /// Adds a <see cref="LogRecord"/> processor to the OpenTelemetry <see
    /// cref="OpenTelemetryLoggerOptions"/> which will convert log messages
    /// into <see cref="ActivityEvent"/>s attached to the active <see
    /// cref="Activity"/> when the message is written.
    /// </summary>
    /// <param name="loggerOptions"><see cref="OpenTelemetryLoggerOptions"/> options to use.</param>
    /// <param name="configure"><see cref="LogToActivityEventConversionOptions"/>.</param>
    /// <returns>The instance of <see cref="OpenTelemetryLoggerOptions"/> to chain the calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="loggerOptions"/> is <c>null</c>.</exception>
    public static OpenTelemetryLoggerOptions AttachLogsToActivityEvent(
        this OpenTelemetryLoggerOptions loggerOptions,
        Action<LogToActivityEventConversionOptions>? configure = null)
    {
        Guard.ThrowIfNull(loggerOptions);

        var options = new LogToActivityEventConversionOptions();
        configure?.Invoke(options);
#pragma warning disable CA2000 // Dispose objects before losing scope
        return loggerOptions.AddProcessor(new ActivityEventAttachingLogProcessor(options));
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    /// <summary>
    /// Adds a <see cref="LogRecord"/> processor to the OpenTelemetry <see
    /// cref="OpenTelemetryLoggerOptions"/> which will copy all
    /// baggage entries as log record attributes.
    /// </summary>
    /// <param name="loggerOptions"><see cref="OpenTelemetryLoggerOptions"/> to add the <see cref="LogRecord"/> processor to.</param>
    /// <returns>The instance of <see cref="OpenTelemetryLoggerOptions"/> to chain the calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="loggerOptions"/> is <c>null</c>.</exception>
    /// <remarks>
    /// Copies all current baggage entries to log record attributes.
    /// </remarks>
    public static OpenTelemetryLoggerOptions AddBaggageProcessor(
        this OpenTelemetryLoggerOptions loggerOptions)
    {
        return loggerOptions.AddBaggageProcessor(BaggageLogRecordProcessor.AllowAllBaggageKeys);
    }

    /// <summary>
    /// Adds a <see cref="LogRecord"/> processor to the OpenTelemetry <see
    /// cref="OpenTelemetryLoggerOptions"/> which will conditionally copy
    /// baggage entries as log record attributes.
    /// </summary>
    /// <param name="loggerOptions"><see cref="OpenTelemetryLoggerOptions"/> to add the <see cref="LogRecord"/> processor to.</param>
    /// <param name="baggageKeyPredicate">Predicate to determine which baggage keys should be added to the log record.</param>
    /// <returns>The instance of <see cref="OpenTelemetryLoggerOptions"/> to chain the calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="loggerOptions"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="baggageKeyPredicate"/> is <c>null</c>.</exception>
    /// <remarks>
    /// Conditionally copies current baggage entries to log record attributes.
    /// In case of an exception the predicate is treated as false, and the baggage entry will not be copied.
    /// </remarks>
    public static OpenTelemetryLoggerOptions AddBaggageProcessor(
        this OpenTelemetryLoggerOptions loggerOptions,
        Predicate<string> baggageKeyPredicate)
    {
        Guard.ThrowIfNull(loggerOptions);
        Guard.ThrowIfNull(baggageKeyPredicate);

        return loggerOptions.AddProcessor(_ => SetupBaggageLogRecordProcessor(baggageKeyPredicate));
    }

    /// <summary>
    /// Adds a <see cref="LogRecord"/> processor to the OpenTelemetry <see
    /// cref="LoggerProviderBuilder"/> which will copy all
    /// baggage entries as log record attributes.
    /// </summary>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/> to add the <see cref="LogRecord"/> processor to.</param>
    /// <returns>The instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    /// <remarks>
    /// Copies all current baggage entries to log record attributes.
    /// </remarks>
    public static LoggerProviderBuilder AddBaggageProcessor(
        this LoggerProviderBuilder builder)
    {
        return builder.AddBaggageProcessor(BaggageLogRecordProcessor.AllowAllBaggageKeys);
    }

    /// <summary>
    /// Adds a <see cref="LogRecord"/> processor to the OpenTelemetry <see
    /// cref="LoggerProviderBuilder"/> which will copy all
    /// baggage entries as log record attributes.
    /// </summary>
    /// <param name="builder"><see cref="LoggerProviderBuilder"/> to add the <see cref="LogRecord"/> processor to.</param>
    /// <param name="baggageKeyPredicate">Predicate to determine which baggage keys should be added to the log record.</param>
    /// <returns>The instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="baggageKeyPredicate"/> is <c>null</c>.</exception>
    /// <remarks>
    /// Conditionally copies current baggage entries to log record attributes.
    /// In case of an exception the predicate is treated as false, and the baggage entry will not be copied.
    /// </remarks>
    public static LoggerProviderBuilder AddBaggageProcessor(
        this LoggerProviderBuilder builder,
        Predicate<string> baggageKeyPredicate)
    {
        Guard.ThrowIfNull(builder);
        Guard.ThrowIfNull(baggageKeyPredicate);

        return builder.AddProcessor(_ => SetupBaggageLogRecordProcessor(baggageKeyPredicate));
    }

    private static BaggageLogRecordProcessor SetupBaggageLogRecordProcessor(Predicate<string> baggageKeyPredicate)
    {
        return new BaggageLogRecordProcessor(baggageKey =>
        {
            try
            {
                return baggageKeyPredicate(baggageKey);
            }
            catch (Exception exception)
            {
                OpenTelemetryExtensionsEventSource.Log.BaggageKeyLogRecordPredicateException(baggageKey, exception.Message);
                return false;
            }
        });
    }
}
