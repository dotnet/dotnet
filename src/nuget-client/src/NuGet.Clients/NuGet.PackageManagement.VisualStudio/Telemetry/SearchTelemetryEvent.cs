// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.PackageManagement.Telemetry
{
    public class SearchTelemetryEvent : TelemetryEvent
    {
        public SearchTelemetryEvent(Guid operationId, string query, bool includePrerelease) : base("Search")
        {
            base["OperationId"] = operationId.ToString();
            AddPiiData("Query", query);
            base["IncludePrerelease"] = includePrerelease;
        }
    }
}
