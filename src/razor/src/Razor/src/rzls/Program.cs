// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Exports;
using Microsoft.AspNetCore.Razor.Telemetry;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var trace = Trace.Messages;
        var telemetryLevel = string.Empty;
        var sessionId = string.Empty;
        var telemetryExtensionPath = string.Empty;

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i].Contains("debug", StringComparison.OrdinalIgnoreCase))
            {
                await Console.Error.WriteLineAsync($"Server started with process ID {Environment.ProcessId}").ConfigureAwait(true);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Debugger.Launch() only works on Windows.
                    _ = Debugger.Launch();
                }
                else
                {
                    var timeout = TimeSpan.FromMinutes(1);
                    await Console.Error.WriteLineAsync($"Waiting {timeout:g} for a debugger to attach").ConfigureAwait(true);
                    using var timeoutSource = new CancellationTokenSource(timeout);
                    while (!Debugger.IsAttached && !timeoutSource.Token.IsCancellationRequested)
                    {
                        await Task.Delay(100, CancellationToken.None).ConfigureAwait(true);
                    }
                }

                continue;
            }

            if (args[i] == "--trace" && i + 1 < args.Length)
            {
                var traceArg = args[++i];
                if (!Enum.TryParse(traceArg, out trace))
                {
                    trace = Trace.Messages;
                    await Console.Error.WriteLineAsync($"Invalid Razor trace '{traceArg}'. Defaulting to {trace}.").ConfigureAwait(true);
                }
            }

            if (args[i] == "--telemetryLevel" && i + 1 < args.Length)
            {
                telemetryLevel = args[++i];
            }

            if (args[i] == "--sessionId" && i + 1 < args.Length)
            {
                sessionId = args[++i];
            }

            if (args[i] == "--telemetryExtensionPath" && i + 1 < args.Length)
            {
                telemetryExtensionPath = args[++i];
            }
        }

        var languageServerFeatureOptions = new ConfigurableLanguageServerFeatureOptions(args);

        ITelemetryReporter? devKitTelemetryReporter = null;
        if (!telemetryExtensionPath.IsNullOrEmpty())
        {
            using var exportProvider = await ExportProviderBuilder.CreateExportProviderAsync(
              telemetryExtensionPath).ConfigureAwait(true);

            // Initialize the telemetry reporter if available
            devKitTelemetryReporter = exportProvider.GetExports<ITelemetryReporter>().SingleOrDefault()?.Value;
            devKitTelemetryReporter?.InitializeSession(telemetryLevel, sessionId, isDefaultSession: true);
        }

        var logger = new LspLogger(trace);
        var server = RazorLanguageServerWrapper.Create(
            Console.OpenStandardInput(),
            Console.OpenStandardOutput(),
            logger,
            devKitTelemetryReporter ?? NoOpTelemetryReporter.Instance,
            featureOptions: languageServerFeatureOptions);

        logger.LogInformation("Razor Language Server started successfully.");

        await server.WaitForExitAsync().ConfigureAwait(true);
    }
}
