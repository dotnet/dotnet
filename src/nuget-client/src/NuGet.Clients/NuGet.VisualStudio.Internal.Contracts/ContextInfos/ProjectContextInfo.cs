// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class ProjectContextInfo : IProjectContextInfo
    {
        public ProjectContextInfo(string projectId, ProjectStyle projectStyle, NuGetProjectKind projectKind)
        {
            ProjectId = projectId;
            ProjectStyle = projectStyle;
            ProjectKind = projectKind;
        }

        public string ProjectId { get; }
        public NuGetProjectKind ProjectKind { get; }
        public ProjectStyle ProjectStyle { get; }

        public static ValueTask<IProjectContextInfo> CreateAsync(NuGetProject nugetProject, CancellationToken cancellationToken)
        {
            Assumes.NotNull(nugetProject);

            if (!nugetProject.TryGetMetadata(NuGetProjectMetadataKeys.ProjectId, out string projectId))
            {
                throw new InvalidOperationException();
            }

            NuGetProjectKind projectKind = GetProjectKind(nugetProject);
            ProjectStyle projectStyle = nugetProject.ProjectStyle;

            return new ValueTask<IProjectContextInfo>(new ProjectContextInfo(projectId, projectStyle, projectKind));
        }

        private static NuGetProjectKind GetProjectKind(NuGetProject nugetProject)
        {
            // Order matters
            NuGetProjectKind projectKind = NuGetProjectKind.Unknown;
            if (nugetProject is BuildIntegratedNuGetProject)
            {
                projectKind = NuGetProjectKind.PackageReference;
            }
            else if (nugetProject is MSBuildNuGetProject)
            {
                projectKind = NuGetProjectKind.PackagesConfig;
            }

            return projectKind;
        }
    }
}
