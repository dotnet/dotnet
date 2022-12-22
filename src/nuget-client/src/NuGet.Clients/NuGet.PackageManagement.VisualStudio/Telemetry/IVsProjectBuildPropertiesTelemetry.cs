// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio.Telemetry
{
    internal interface IVsProjectBuildPropertiesTelemetry
    {
        void OnPropertyStorageUsed(string[] projectTypeGuids);
        void OnDteUsed(string[] projectTypeGuids);
    }
}
