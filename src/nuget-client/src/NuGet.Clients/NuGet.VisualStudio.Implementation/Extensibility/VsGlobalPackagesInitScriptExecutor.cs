// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsGlobalPackagesInitScriptExecutor))]
    public class VsGlobalPackagesInitScriptExecutor : IVsGlobalPackagesInitScriptExecutor
    {
        private readonly IScriptExecutor _scriptExecutor;
        private readonly INuGetTelemetryProvider _telemetryProvider;

        [ImportingConstructor]
        public VsGlobalPackagesInitScriptExecutor(IScriptExecutor scriptExecutor, INuGetTelemetryProvider telemetryProvider)
        {
            _scriptExecutor = scriptExecutor ?? throw new ArgumentNullException(nameof(scriptExecutor));
            _telemetryProvider = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));
        }

        public async Task<bool> ExecuteInitScriptAsync(string packageId, string packageVersion)
        {
            const string eventName = nameof(IVsGlobalPackagesInitScriptExecutor) + "." + nameof(ExecuteInitScriptAsync);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName,
                new
                {
                    PackageId = packageId,
                    PackageVersion = packageVersion
                });

            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageId));
            }

            if (string.IsNullOrEmpty(packageVersion))
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageVersion));
            }

            // Exceptions from parsing package id or version should not be logged as faults
            var version = new NuGetVersion(packageVersion);
            var packageIdentity = new PackageIdentity(packageId, version);

            try
            {
                return await _scriptExecutor.ExecuteInitScriptAsync(packageIdentity);
            }
            catch (Exception exception)
            {
                await _telemetryProvider.PostFaultAsync(exception, typeof(VsGlobalPackagesInitScriptExecutor).FullName);
                throw;
            }
        }
    }
}
