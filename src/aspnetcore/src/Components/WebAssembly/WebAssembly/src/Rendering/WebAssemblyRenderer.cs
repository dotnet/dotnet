// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.AspNetCore.Components.WebAssembly.Rendering;

/// <summary>
/// Provides mechanisms for rendering <see cref="IComponent"/> instances in a
/// web browser, dispatching events to them, and refreshing the UI as required.
/// </summary>
internal sealed partial class WebAssemblyRenderer : WebRenderer
{
    private readonly RootComponentTypeCache _rootComponentCache;
    private readonly ILogger _logger;

    public WebAssemblyRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, JSComponentInterop jsComponentInterop)
        : base(serviceProvider, loggerFactory, DefaultWebAssemblyJSRuntime.Instance.ReadJsonSerializerOptions(), jsComponentInterop)
    {
        _rootComponentCache = serviceProvider.GetRequiredService<RootComponentTypeCache>();
        _logger = loggerFactory.CreateLogger<WebAssemblyRenderer>();

        ElementReferenceContext = DefaultWebAssemblyJSRuntime.Instance.ElementReferenceContext;
    }

    public override Dispatcher Dispatcher => NullDispatcher.Instance;

    public Task AddComponentAsync([DynamicallyAccessedMembers(Component)] Type componentType, ParameterView parameters, string domElementSelector)
    {
        var componentId = AddRootComponent(componentType, domElementSelector);
        return RenderRootComponentAsync(componentId, parameters);
    }

    protected override int GetWebRendererId() => (int)WebRendererId.WebAssembly;

    protected override void AttachRootComponentToBrowser(int componentId, string domElementSelector)
    {
        DefaultWebAssemblyJSRuntime.Instance.InvokeVoid(
            "Blazor._internal.attachRootComponentToElement",
            domElementSelector,
            componentId,
            RendererId);
    }

    [DynamicDependency(JsonSerialized, typeof(RootComponentOperation))]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The correct members will be preserved by the above DynamicDependency")]
    protected override void UpdateRootComponents(string operationsJson)
    {
        var operations = JsonSerializer.Deserialize<IEnumerable<RootComponentOperation>>(
            operationsJson,
            WebAssemblyComponentSerializationSettings.JsonSerializationOptions)!;

        foreach (var operation in operations)
        {
            switch (operation.Type)
            {
                case RootComponentOperationType.Add:
                    AddRootComponent(operation);
                    break;
                case RootComponentOperationType.Update:
                    UpdateRootComponent(operation);
                    break;
                case RootComponentOperationType.Remove:
                    RemoveRootComponent(operation);
                    break;
            }
        }

        return;

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Root components are expected to be defined in assemblies that do not get trimmed.")]
        void AddRootComponent(RootComponentOperation operation)
        {
            if (operation.SelectorId is not { } selectorId)
            {
                throw new InvalidOperationException($"The component operation of type '{operation.Type}' requires a '{nameof(operation.SelectorId)}' to be specified.");
            }

            if (operation.Marker is not { } marker)
            {
                throw new InvalidOperationException($"The component operation of type '{operation.Type}' requires a '{nameof(operation.Marker)}' to be specified.");
            }

            var componentType = _rootComponentCache.GetRootComponent(marker.Assembly!, marker.TypeName!)
                ?? throw new InvalidOperationException($"Root component type '{marker.TypeName}' could not be found in the assembly '{marker.Assembly}'.");

            var parameters = DeserializeComponentParameters(marker);
            _ = AddComponentAsync(componentType, parameters, selectorId.ToString(CultureInfo.InvariantCulture));
        }

        void UpdateRootComponent(RootComponentOperation operation)
        {
            if (operation.ComponentId is not { } componentId)
            {
                throw new InvalidOperationException($"The component operation of type '{operation.Type}' requires a '{nameof(operation.ComponentId)}' to be specified.");
            }

            if (operation.Marker is not { } marker)
            {
                throw new InvalidOperationException($"The component operation of type '{operation.Type}' requires a '{nameof(operation.Marker)}' to be specified.");
            }

            var parameters = DeserializeComponentParameters(marker);
            _ = RenderRootComponentAsync(componentId, parameters);
        }

        void RemoveRootComponent(RootComponentOperation operation)
        {
            if (operation.ComponentId is not { } componentId)
            {
                throw new InvalidOperationException($"The component operation of type '{operation.Type}' requires a '{nameof(operation.ComponentId)}' to be specified.");
            }

            this.RemoveRootComponent(componentId);
        }

        static ParameterView DeserializeComponentParameters(ComponentMarker marker)
        {
            var definitions = WebAssemblyComponentParameterDeserializer.GetParameterDefinitions(marker.ParameterDefinitions!);
            var values = WebAssemblyComponentParameterDeserializer.GetParameterValues(marker.ParameterValues!);
            var componentDeserializer = WebAssemblyComponentParameterDeserializer.Instance;
            var parameters = componentDeserializer.DeserializeParameters(definitions, values);

            return parameters;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override void ProcessPendingRender()
    {
        // For historical reasons, Blazor WebAssembly doesn't enforce that you use InvokeAsync
        // to dispatch calls that originated from outside the system. Changing that now would be
        // too breaking, at least until we can make it a prerequisite for multithreading.
        // So, we don't have a way to guarantee that calls to here are already on our work queue.
        //
        // We do need rendering to happen on the work queue so that incoming events can be deferred
        // until we've finished this rendering process (and other similar cases where we want
        // execution order to be consistent with Blazor Server, which queues all JS->.NET calls).
        //
        // So, if we find that we're here and are not yet on the work queue, get onto it. Either
        // way, rendering must continue synchronously here and is not deferred until later.
        if (WebAssemblyCallQueue.IsInProgress)
        {
            base.ProcessPendingRender();
        }
        else
        {
            WebAssemblyCallQueue.Schedule(this, static @this => @this.CallBaseProcessPendingRender());
        }
    }

    private void CallBaseProcessPendingRender() => base.ProcessPendingRender();

    /// <inheritdoc />
    protected override unsafe Task UpdateDisplayAsync(in RenderBatch batch)
    {
        // This is a GC hazard - it would be ideal to pin 'batch' and all its contents to prevent
        // it from getting moved, or pause the GC for the duration of the 'RenderBatch()' call.
        // The key mitigation is that the JS-side code always processes renderbatches synchronously
        // and never calls back into .NET during that process, so GC cannot run (assuming it would
        // only run on the current thread).
        // As an early-warning system in case we accidentally introduce bugs and violate that rule,
        // or for edge cases where user code can be invoked during rendering (e.g., DOM mutation
        // observers) we further enforce it on the JS side using a notion of "locking the heap"
        // during rendering, which prevents any JS-to-.NET calls that go through Blazor APIs such
        // as DotNet.invokeMethod or event handlers.
        var batchCopy = batch;
        RenderBatch(RendererId, Unsafe.AsPointer(ref batchCopy));

        if (WebAssemblyCallQueue.HasUnstartedWork)
        {
            // Because further incoming calls from JS to .NET are already queued (e.g., event notifications),
            // we have to delay the renderbatch acknowledgement until it gets to the front of that queue.
            // This is for consistency with Blazor Server which queues all JS-to-.NET calls relative to each
            // other, and because various bits of cleanup logic rely on this ordering.
            var tcs = new TaskCompletionSource();
            WebAssemblyCallQueue.Schedule(tcs, static tcs => tcs.SetResult());
            return tcs.Task;
        }
        else
        {
            // Nothing else is pending, so we can treat the renderbatch as acknowledged synchronously.
            // This lets upstream code skip an expensive code path and avoids some allocations.
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc />
    protected override void HandleException(Exception exception)
    {
        if (exception is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.Flatten().InnerExceptions)
            {
                Log.UnhandledExceptionRenderingComponent(_logger, innerException.Message, innerException);
            }
        }
        else
        {
            Log.UnhandledExceptionRenderingComponent(_logger, exception.Message, exception);
        }
    }

    protected override IComponent ResolveComponentForRenderMode([DynamicallyAccessedMembers(Component)] Type componentType, int? parentComponentId, IComponentActivator componentActivator, IComponentRenderMode renderMode)
        => renderMode switch
        {
            WebAssemblyRenderMode or AutoRenderMode => componentActivator.CreateInstance(componentType),
            _ => throw new NotSupportedException($"Cannot create a component of type '{componentType}' because its render mode '{renderMode}' is not supported by WebAssembly rendering."),
        };

    private static partial class Log
    {
        [LoggerMessage(100, LogLevel.Critical, "Unhandled exception rendering component: {Message}", EventName = "ExceptionRenderingComponent")]
        public static partial void UnhandledExceptionRenderingComponent(ILogger logger, string message, Exception exception);
    }

    [JSImport("Blazor._internal.renderBatch", "blazor-internal")]
    private static unsafe partial void RenderBatch(int id, void* batch);
}
