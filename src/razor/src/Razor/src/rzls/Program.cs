// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Telemetry;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var trace = Trace.Messages;

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i].Contains("debug", StringComparison.OrdinalIgnoreCase))
            {
                await Console.Error.WriteLineAsync($"Server started with process ID {Environment.ProcessId}");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Debugger.Launch() only works on Windows.
                    _ = Debugger.Launch();
                }
                else
                {
                    var timeout = TimeSpan.FromMinutes(1);
                    await Console.Error.WriteLineAsync($"Waiting {timeout:g} for a debugger to attach");
                    using var timeoutSource = new CancellationTokenSource(timeout);
                    while (!Debugger.IsAttached && !timeoutSource.Token.IsCancellationRequested)
                    {
                        await Task.Delay(100, CancellationToken.None);
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
                    await Console.Error.WriteLineAsync($"Invalid Razor trace '{traceArg}'. Defaulting to {trace}.");
                }
            }
        }

        var languageServerFeatureOptions = new ConfigurableLanguageServerFeatureOptions(args);

        var logger = new LspLogger(trace);
        var server = RazorLanguageServerWrapper.Create(
            Console.OpenStandardInput(),
            Console.OpenStandardOutput(),
            logger,
            NoOpTelemetryReporter.Instance,
            featureOptions: languageServerFeatureOptions);
        await server.WaitForExitAsync();
    }
}
