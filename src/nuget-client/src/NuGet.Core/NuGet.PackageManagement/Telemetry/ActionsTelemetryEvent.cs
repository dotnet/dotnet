// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Telemetry event data for nuget operations like install, update, or uninstall.
    /// </summary>
    public class ActionsTelemetryEvent : ActionEventBase
    {
        public ActionsTelemetryEvent(
            string operationId,
            string[] projectIds,
            NuGetOperationType operationType,
            DateTimeOffset startTime,
            NuGetOperationStatus status,
            int packageCount,
            DateTimeOffset endTime,
            double duration) : base(NugetActionEventName, operationId, projectIds, startTime, status, packageCount, endTime, duration)
        {
            base[nameof(OperationType)] = operationType;
        }

        public const string NugetActionEventName = "NugetAction";

        public NuGetOperationType OperationType => (NuGetOperationType)base[nameof(OperationType)];
    }

    /// <summary>
    /// Define nuget operation type values.
    /// </summary>
    public enum NuGetOperationType
    {
        /// <summary>
        /// Install package action.
        /// </summary>
        Install = 0,

        /// <summary>
        /// Update package action.
        /// </summary>
        Update = 1,

        /// <summary>
        /// Uninstall package action.
        /// </summary>
        Uninstall = 2,
    }
}
