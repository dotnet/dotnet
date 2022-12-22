// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;

namespace NuGet.VisualStudio.Telemetry
{
    [Export(typeof(INuGetTelemetryProvider))]
    internal sealed class NuGetTelemetryProvider : INuGetTelemetryProvider, IDisposable
    {
        ExtensibilityTelemetryCollector _extensibilityTelemetryCollector;

        public NuGetTelemetryProvider()
        {
            // NuGet's VS extensibility APIs (should) all use INuGetTelemetryProvider.
            // Since NuGet's MEF extensibility is MEF, which doesn't participate in VS's
            // "package" and service extensibility, it's entirely possible for other components
            // to use NuGet's APIs without NuGetPackage ever being loaded/initialized.
            // Therefore, we should handle our VS extensibility telemetry without depending on
            // NuGetPackage.InitializeAsync ever being called.
            _extensibilityTelemetryCollector = new ExtensibilityTelemetryCollector();
            VsShellUtilities.ShutdownToken.Register(() =>
            {
                // It probably shouldn't be possible for IDispose to be called before this shutdown
                // token being signalled, but let's practise defensive coding and minimize risk of
                // issues if both methods are executed in parallel.
                var collector = _extensibilityTelemetryCollector;
                if (collector != null)
                {
                    _extensibilityTelemetryCollector = null;

                    // This isn't a normal, or good, pattern, but Dispose will stop the EventListener.
                    // Normally you shouldn't use a disposed object, but in our case we know the
                    // implementation, so we can call it to get telemetry data.
                    collector.Dispose();
                    TelemetryEvent telemetryEvent = collector.ToTelemetryEvent();
                    VSTelemetrySession.Instance.PostEvent(telemetryEvent);
                }
            });
        }

        public void EmitEvent(TelemetryEvent telemetryEvent)
        {
            TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
        }

        public async Task PostFaultAsync(Exception e, string callerClassName, [CallerMemberName] string callerMemberName = null, IDictionary<string, object> extraProperties = null)
        {
            await TelemetryUtility.PostFaultAsync(e, callerClassName, callerMemberName, extraProperties);
        }

        public void PostFault(Exception e, string callerClassName, [CallerMemberName] string callerMemberName = null, IDictionary<string, object> extraProperties = null)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await PostFaultAsync(e, callerClassName, callerMemberName, extraProperties);
            });
        }

        public void Dispose()
        {
            _extensibilityTelemetryCollector?.Dispose();
            _extensibilityTelemetryCollector = null;

            GC.SuppressFinalize(this);
        }
    }
}
