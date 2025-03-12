// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.NamedPipes;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var logLevel = LogLevel.Information;
        var telemetryLevel = string.Empty;
        var sessionId = string.Empty;
        var telemetryExtensionPath = string.Empty;

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i].Contains("debug", StringComparison.OrdinalIgnoreCase))
            {
                await Console.Error.WriteLineAsync($"Server started with process ID {Environment.ProcessId}").ConfigureAwait(true);
                if (PlatformInformation.IsWindows)
                {
                    // Debugger.Launch() only works on Windows.
                    Debugger.Launch();
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

            if (args[i] == "--logLevel" && i + 1 < args.Length)
            {
                var logLevelArg = args[++i];
                if (!Enum.TryParse(logLevelArg, out logLevel))
                {
                    logLevel = LogLevel.Information;
                    await Console.Error.WriteLineAsync($"Invalid Razor log level '{logLevelArg}'. Defaulting to {logLevel}.").ConfigureAwait(true);
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

        using var telemetryContext = await TryGetTelemetryReporterAsync(telemetryLevel, sessionId, telemetryExtensionPath).ConfigureAwait(true);

        // Have to create a logger factory to give to the server, but can't create any logger providers until we have
        // a server.
        var loggerFactory = new LoggerFactory([]);
        var logLevelProvider = new LogLevelProvider(logLevel);

        using var host = RazorLanguageServerHost.Create(
            Console.OpenStandardInput(),
            Console.OpenStandardOutput(),
            loggerFactory,
            telemetryContext?.TelemetryReporter ?? NoOpTelemetryReporter.Instance,
            featureOptions: languageServerFeatureOptions,
            configureServices: services =>
            {
                services.AddSingleton<IRazorProjectInfoDriver, NamedPipeBasedRazorProjectInfoDriver>();
                services.AddHandler<RazorNamedPipeConnectEndpoint>();
                services.AddHandlerWithCapabilities<DocumentDiagnosticsEndpoint>();

                services.AddSingleton(logLevelProvider);
                services.AddHandler<UpdateLogLevelEndpoint>();
            });

        // Now we have a server, and hence a connection, we have somewhere to log
        var clientConnection = host.GetRequiredService<IClientConnection>();
        var loggerProvider = new LoggerProvider(logLevelProvider, clientConnection);
        loggerFactory.AddLoggerProvider(loggerProvider);

        loggerFactory.GetOrCreateLogger("RZLS").LogInformation($"Razor Language Server started successfully.");

        await host.WaitForExitAsync().ConfigureAwait(true);
    }

    private static async Task<TelemetryContext?> TryGetTelemetryReporterAsync(string telemetryLevel, string sessionId, string telemetryExtensionPath)
    {
        ExportProvider? exportProvider = null;
        if (!telemetryExtensionPath.IsNullOrEmpty())
        {
            try
            {
                exportProvider = await ExportProviderBuilder
                    .CreateExportProviderAsync(telemetryExtensionPath)
                    .ConfigureAwait(true);

                // Initialize the telemetry reporter if available
                var devKitTelemetryReporter = exportProvider.GetExports<ITelemetryReporter>().SingleOrDefault()?.Value;

                if (devKitTelemetryReporter is ITelemetryReporterInitializer initializer)
                {
                    initializer.InitializeSession(telemetryLevel, sessionId, isDefaultSession: true);
                    return new TelemetryContext(exportProvider, devKitTelemetryReporter);
                }
                else
                {
                    exportProvider.Dispose();
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Failed to load telemetry extension in {telemetryExtensionPath}.").ConfigureAwait(true);
                await Console.Error.WriteLineAsync(ex.ToString()).ConfigureAwait(true);
                exportProvider?.Dispose();
            }
        }

        return null;
    }

    private readonly record struct TelemetryContext(IDisposable ExportProvider, ITelemetryReporter TelemetryReporter) : IDisposable
    {
        public void Dispose()
        {
            // No need to explicitly dispose of the telemetry reporter. The lifetime
            // is managed by the ExportProvider and will be disposed with it.
            ExportProvider.Dispose();
        }
    }
}
