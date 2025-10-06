// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal sealed partial class RazorLanguageServerHost : IDisposable
{
    private readonly RazorLanguageServer _server;
    private bool _disposed;

    private RazorLanguageServerHost(RazorLanguageServer server)
    {
        _server = server;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _server.Dispose();
    }

    public static RazorLanguageServerHost Create(
        Stream input,
        Stream output,
        ILoggerFactory loggerFactory,
        ITelemetryReporter telemetryReporter,
        Action<IServiceCollection>? configureServices = null,
        LanguageServerFeatureOptions? featureOptions = null,
        RazorLSPOptions? razorLSPOptions = null,
        ILspServerActivationTracker? lspServerActivationTracker = null,
        TraceSource? traceSource = null)
    {
        var (jsonRpc, jsonSerializer) = CreateJsonRpc(input, output);

        // This ensures each request is a separate activity in LogHub
        jsonRpc.ActivityTracingStrategy = new CorrelationManagerTracingStrategy
        {
            TraceSource = traceSource
        };

        var server = new RazorLanguageServer(
            jsonRpc,
            jsonSerializer,
            loggerFactory,
            featureOptions,
            configureServices,
            razorLSPOptions,
            lspServerActivationTracker,
            telemetryReporter);

        var host = new RazorLanguageServerHost(server);

        jsonRpc.StartListening();

        return host;
    }

    private static (JsonRpc, JsonSerializerOptions) CreateJsonRpc(Stream input, Stream output)
    {
        var messageFormatter = new SystemTextJsonFormatter()
        {
            JsonSerializerOptions = JsonHelpers.JsonSerializerOptions
        };

        var jsonRpc = new JsonRpc(new HeaderDelimitedMessageHandler(output, input, messageFormatter));

        // Get more information about exceptions that occur during RPC method invocations.
        jsonRpc.ExceptionStrategy = ExceptionProcessing.ISerializable;

        return (jsonRpc, messageFormatter.JsonSerializerOptions);
    }

    public Task WaitForExitAsync()
    {
        var lspServices = _server.GetLspServices();
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
        var lspServices = _server.GetLspServices();
        return lspServices.GetRequiredService<T>();
    }
}
