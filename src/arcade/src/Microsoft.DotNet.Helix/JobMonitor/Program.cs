// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            JobMonitorOptions options = null;
            try
            {
                options = JobMonitorOptions.Parse(args);
            }
            catch (Exception ex)
            {
                using ILoggerFactory errorLoggerFactory = CreateLoggerFactory(verbose: false);
                ILogger errorLogger = errorLoggerFactory.CreateLogger<JobMonitorRunner>();
                errorLogger.LogError(ex, "Helix Job Monitor terminated with an unhandled exception.");
                return 1;
            }

            using ILoggerFactory loggerFactory = CreateLoggerFactory(options.Verbose);
            ILogger<JobMonitorRunner> logger = loggerFactory.CreateLogger<JobMonitorRunner>();

            try
            {
                if (options.ShowHelp)
                {
                    return 2;
                }

                // Create a CancellationTokenSource driven by external signals (CTRL+C / SIGTERM).
                // When AzDO times out the pipeline job it sends these signals, giving us a chance
                // to cancel any in-flight Helix jobs before the process is killed.
                using var signalCts = new CancellationTokenSource();

                // Handle CTRL+C (Windows and Linux). The first CTRL+C triggers graceful
                // cancellation; subsequent presses fall through to default termination so
                // that a stuck shutdown can still be aborted by the user.
                bool cancelRequested = false;
                ConsoleCancelEventHandler cancelKeyPressHandler = (_, e) =>
                {
                    if (!cancelRequested)
                    {
                        cancelRequested = true;
                        e.Cancel = true; // Prevent immediate process termination.
                        signalCts.Cancel();
                    }
                };
                Console.CancelKeyPress += cancelKeyPressHandler;

                // Handle SIGTERM (the signal AzDO sends on Linux agents when cancelling).
                // PosixSignalRegistration is only supported on Unix; skip on Windows where
                // CTRL+C (above) is the cancellation mechanism.
                IDisposable sigtermRegistration = null;
                if (!OperatingSystem.IsWindows())
                {
                    sigtermRegistration = PosixSignalRegistration.Create(PosixSignal.SIGTERM, ctx =>
                    {
                        ctx.Cancel = true; // Prevent default process termination.
                        signalCts.Cancel();
                    });
                }

                // Combine the external-signal token with the configured timeout so that either
                // an AzDO cancellation or the MaximumWaitMinutes limit cancels the runner.
                // The timeout was previously managed inside JobMonitorRunner.RunAsync(); it is
                // configured here instead so that the same linked token covers both signal-based
                // cancellation and the internal time limit.
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(options.MaximumWaitMinutes));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(signalCts.Token, timeoutCts.Token);

                try
                {
                    using JobMonitorRunner runner = new(options, logger);
                    return await runner.RunAsync(linkedCts.Token);
                }
                finally
                {
                    Console.CancelKeyPress -= cancelKeyPressHandler;
                    sigtermRegistration?.Dispose();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Helix Job Monitor terminated with an unhandled exception.");
                return 1;
            }
        }

        private static ILoggerFactory CreateLoggerFactory(bool verbose)
        {
            return LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information)
                    .AddConsole(o => o.FormatterName = CompactConsoleLoggerFormatter.FormatterName)
                    .AddConsoleFormatter<CompactConsoleLoggerFormatter, SimpleConsoleFormatterOptions>();
            });
        }
    }
}
