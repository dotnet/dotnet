// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Defines an event handler delegate for package related events.
    /// </summary>
    /// <param name="metadata">Description of the package.</param>
    public delegate void VsPackageEventHandler(IVsPackageMetadata metadata);

    /// <summary>
    /// Defines an event handler delegate for nuget batch events with projects with packages.config file.
    /// </summary>
    /// <param name="metadata">Description of the package.</param>
    public delegate void VsPackageProjectEventHandler(IVsPackageProjectMetadata metadata);
}
