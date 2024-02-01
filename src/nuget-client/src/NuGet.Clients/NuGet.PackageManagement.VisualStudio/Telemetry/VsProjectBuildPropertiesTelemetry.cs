// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        ConcurrentDictionary<string, ConcurrentDictionary<string, ApiUsage>> _projectSystems = new(StringComparer.OrdinalIgnoreCase);

        public VsProjectBuildPropertiesTelemetry()
        {
            InstanceCloseTelemetryEmitter.AddEventsOnShutdown += AddEventsOnShutdown;
        }

        public void OnDteUsed(string propertyName, string[] projectTypeGuids)
        {
            foreach (var typeGuid in projectTypeGuids)
            {
                ApiUsage apiUsage = GetApiUsage(propertyName, typeGuid);
                apiUsage.DteUsed = true;
            }
        }

        public void OnPropertyStorageUsed(string propertyName, string[] projectTypeGuids)
        {
            foreach (var typeGuid in projectTypeGuids)
            {
                ApiUsage apiUsage = GetApiUsage(propertyName, typeGuid);
                apiUsage.PropertyStorageUsed = true;
            }
        }

        private ApiUsage GetApiUsage(string propertyName, string projectTypeGuid)
        {
            ConcurrentDictionary<string, ApiUsage> propertyNames = _projectSystems.GetOrAdd(projectTypeGuid, CreatePropertyNamesDictionary);
            ApiUsage apiUsage = propertyNames.GetOrAdd(propertyName, CreateApiUsage, projectTypeGuid);
            return apiUsage;
        }

        private static ConcurrentDictionary<string, ApiUsage> CreatePropertyNamesDictionary(string _)
        {
            return new ConcurrentDictionary<string, ApiUsage>();
        }

        private static ApiUsage CreateApiUsage(string propertyName, string projectTypeGuid)
        {
            return new ApiUsage()
            {
                ProjectType = projectTypeGuid,
                PropertyName = propertyName
            };
        }

        internal void AddEventsOnShutdown(object sender, TelemetryEvent e)
        {
            int count = _projectSystems.Values.Sum(p => p.Count);
            List<ApiUsage> items = new(count);

            foreach (var projectSystem in _projectSystems)
            {
                foreach (var property in projectSystem.Value)
                {
                    ApiUsage apiUsage = property.Value;
                    items.Add(property.Value);
                }
            }

            e.ComplexData["ProjectBuildProperties"] = items;
        }

        private class ApiUsage
        {
            public required string ProjectType { get; set; }
            public required string PropertyName { get; set; }
            public bool PropertyStorageUsed { get; set; } = false;
            public bool DteUsed { get; set; } = false;
        }
    }
}
