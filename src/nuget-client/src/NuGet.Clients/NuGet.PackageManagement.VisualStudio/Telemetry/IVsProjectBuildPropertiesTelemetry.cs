// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

namespace NuGet.PackageManagement.VisualStudio.Telemetry
{
    internal interface IVsProjectBuildPropertiesTelemetry
    {
        void OnPropertyStorageUsed(string propertyName, string[] projectTypeGuids);
        void OnDteUsed(string propertyName, string[] projectTypeGuids);
    }
}
