// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.PackageManagement
{
    public class ActionTelemetryStepEvent : TelemetryEvent
    {
        public ActionTelemetryStepEvent(string parentId, string stepName, double duration) :
            base(NugetActionStepsEventName, new Dictionary<string, object>
                {
                    { nameof(ParentId), parentId },
                    { nameof(SubStepName), string.Join(",", stepName) },
                    { nameof(Duration), duration }
                })
        {
        }

        public const string NugetActionStepsEventName = "NugetActionSteps";

        public string SubStepName => (string)base[nameof(SubStepName)];
        public double Duration => (double)base[nameof(Duration)];
        public string ParentId => (string)base[nameof(ParentId)];
    }
}
