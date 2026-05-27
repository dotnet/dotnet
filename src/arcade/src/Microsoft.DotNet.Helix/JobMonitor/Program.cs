// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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

                using JobMonitorRunner runner = new(options, logger);
                return await runner.RunAsync();
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
