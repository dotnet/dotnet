// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.Telemetry
{
    public enum NavigationOrigin
    {
        None,
        Options_PackageSourceMapping_Add,
        Options_PackageSourceMapping_Remove,
        Options_PackageSourceMapping_RemoveAll,
        PMUI_ExternalLink,
        PMUI_PackageSourceMapping_Configure
    }
}
