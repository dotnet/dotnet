// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectModel;

namespace NuGet.VisualStudio.Internal.Contracts
{
    /// <summary>
    /// Contains information about a NuGetProject
    /// </summary>
    public interface IProjectContextInfo
    {
        string ProjectId { get; }
        ProjectStyle ProjectStyle { get; }
        NuGetProjectKind ProjectKind { get; }
    }
}
