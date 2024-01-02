// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Serialization;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RazorLanguageServerWrapper : IDisposable
{
    private readonly RazorLanguageServer _innerServer;
    private readonly object _disposeLock;
    private bool _disposed;

    private RazorLanguageServerWrapper(RazorLanguageServer innerServer)
    {
        if (innerServer is null)
        {
            throw new ArgumentNullException(nameof(innerServer));
        }

        _innerServer = innerServer;
        _disposeLock = new object();
    }

    public static RazorLanguageServerWrapper Create(
        Stream input,
        Stream output,
        IRazorLoggerFactory loggerFactory,
        ITelemetryReporter telemetryReporter,
        ProjectSnapshotManagerDispatcher? projectSnapshotManagerDispatcher = null,
        Action<IServiceCollection>? configure = null,
        LanguageServerFeatureOptions? featureOptions = null,
        RazorLSPOptions? razorLSPOptions = null,
        ILspServerActivationTracker? lspServerActivationTracker = null,
        TraceSource? traceSource = null)
    {
        var jsonRpc = CreateJsonRpc(input, output);

        // This ensures each request is a separate activity in LogHub
        jsonRpc.ActivityTracingStrategy = new CorrelationManagerTracingStrategy
        {
            TraceSource = traceSource
        };

        var server = new RazorLanguageServer(
            jsonRpc,
            loggerFactory,
            projectSnapshotManagerDispatcher,
            featureOptions,
            configure,
            razorLSPOptions,
            lspServerActivationTracker,
            telemetryReporter);

        var razorLanguageServer = new RazorLanguageServerWrapper(server);
        jsonRpc.StartListening();

        return razorLanguageServer;
    }

    private static JsonRpc CreateJsonRpc(Stream input, Stream output)
    {
        var messageFormatter = new JsonMessageFormatter();
        messageFormatter.JsonSerializer.AddVSInternalExtensionConverters();
        messageFormatter.JsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

        var jsonRpc = new JsonRpc(new HeaderDelimitedMessageHandler(output, input, messageFormatter));

        return jsonRpc;
    }

    public Task WaitForExitAsync()
    {
        var lspServices = _innerServer.GetLspServices();
        if (lspServices is LspServices razorServices)
        {
            // If the LSP Server is already disposed it means the server has already exited.
            if (razorServices.IsDisposed)
            {
                return Task.CompletedTask;
            }
            else
            {
                var lifeCycleManager = razorServices.GetRequiredService<RazorLifeCycleManager>();
                return lifeCycleManager.WaitForExit;
            }
        }
        else
        {
            throw new NotImplementedException($"LspServices should always be of type {nameof(LspServices)}.");
        }
    }

    internal T GetRequiredService<T>() where T : notnull
    {
        return _innerServer.GetRequiredService<T>();
    }

    public void Dispose()
    {
        lock (_disposeLock)
        {
            if (!_disposed)
            {
                _disposed = true;

                TempDirectory.Instance.Dispose();
            }
        }
    }

    internal RazorLanguageServer GetInnerLanguageServerForTesting()
    {
        return _innerServer;
    }
}
