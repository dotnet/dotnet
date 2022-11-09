// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.Telemetry
{
    public sealed class GetInstalledPackagesAsyncTelemetryEvent : DiagnosticTelemetryEvent
    {
        private const string Data = "Data";

        private const string EventName = DiagnosticEventName + "/GetInstalledPackagesAsync";

        public GetInstalledPackagesAsyncTelemetryEvent()
            : base(EventName)
        {
        }

        public void AddProject(NuGetProjectType projectType, string projectId, int nullCount, int totalCount)
        {
            List<ProjectData> projectDatas;

            if (ComplexData.TryGetValue(Data, out object value) && value is List<ProjectData> data)
            {
                projectDatas = data;
            }
            else
            {
                projectDatas = new List<ProjectData>();

                ComplexData[Data] = projectDatas;
            }

            projectDatas.Add(new ProjectData(projectId, projectType, nullCount, totalCount));
        }

        private sealed class ProjectData
        {
            public TelemetryPiiProperty ProjectId { get; }
            public string ProjectType { get; }
            public int NullCount { get; }
            public int TotalCount { get; }

            internal ProjectData(string projectId, NuGetProjectType projectType, int nullCount, int totalCount)
            {
                ProjectId = new TelemetryPiiProperty(projectId);
                ProjectType = projectType.ToString();
                NullCount = nullCount;
                TotalCount = totalCount;
            }
        }
    }
}
