// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Linq;
using NuGet.Common;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio.Telemetry
{
    [Export(typeof(IVsProjectBuildPropertiesTelemetry))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class VsProjectBuildPropertiesTelemetry : IVsProjectBuildPropertiesTelemetry
    {
        // some project systems return guid in upper case, others return guids in lower case, so ignore case.
        ConcurrentDictionary<string, ApiUsage> _apiUsages = new ConcurrentDictionary<string, ApiUsage>(StringComparer.OrdinalIgnoreCase);

        public VsProjectBuildPropertiesTelemetry()
        {
            InstanceCloseTelemetryEmitter.AddEventsOnShutdown += AddEventsOnShutdown;
        }

        public void OnDteUsed(string[] projectTypeGuids)
        {
            foreach (var typeGuid in projectTypeGuids)
            {
                var apiUsage = _apiUsages.GetOrAdd(typeGuid, NewApiUsage);
                apiUsage.DteUsed = true;
            }
        }

        public void OnPropertyStorageUsed(string[] projectTypeGuids)
        {
            foreach (var typeGuid in projectTypeGuids)
            {
                var apiUsage = _apiUsages.GetOrAdd(typeGuid, NewApiUsage);
                apiUsage.PropertyStorageUsed = true;
            }
        }

        private static ApiUsage NewApiUsage(string projectTypeGuid)
        {
            var guid = projectTypeGuid.ToUpperInvariant();
            return new ApiUsage()
            {
                ProjectType = guid
            };
        }

        internal void AddEventsOnShutdown(object sender, TelemetryEvent e)
        {
            var entries = _apiUsages.Values.ToList();
            e.ComplexData["ProjectBuildProperties"] = entries;
        }

        private class ApiUsage
        {
            public string ProjectType { get; set; }
            public bool PropertyStorageUsed { get; set; } = false;
            public bool DteUsed { get; set; } = false;
        }
    }
}
