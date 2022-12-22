// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.VisualStudio
{
    internal static class CounterfactualLoggers
    {
        internal readonly static TelemetryOnceEmitter TransitiveDependencies = new("TransitiveDependenciesCounterfactual");
        internal readonly static TelemetryOnceEmitter PMUITransitiveDependencies = new("PMUITransitiveDependenciesCounterfactual");
    }
}
