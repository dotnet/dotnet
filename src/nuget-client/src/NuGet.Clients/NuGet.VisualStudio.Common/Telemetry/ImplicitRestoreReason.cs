// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Telemetry
{
    public enum ImplicitRestoreReason
    {
        None, // The reason is not implicit.
        AllProjectsNominated, // All projects have been nominated.
        NominationsIdleTimeout, // The timeout for all nominations has been exceeded. This means that bulk restore coordination is *not* enabled.
        ProjectsReady, // The projects ready check has been completed and no projects are reporting pending nominations.
        ProjectsReadyCheckTimeout, // We have spent a considerable amount of time in the projects ready check.
    }
}
