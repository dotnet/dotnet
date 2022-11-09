// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using Microsoft.Build.Execution;

namespace NuGet.Build.Tasks.Console
{
    /// <summary>
    /// Represents a project and its inner projects if it is multi-targeting.  Legacy projects and projects that target
    /// a single framework only have an outer project.  But if a project targets multiple frameworks, items like PackageReference,
    /// ProjectReference, and other properties need to be gathered from the set of projects for those target frameworks.
    /// </summary>
    internal sealed class ProjectWithInnerNodes : ConcurrentDictionary<string, IMSBuildProject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectWithInnerNodes"/> class.
        /// </summary>
        /// <param name="targetFramework">The target framework of the outer project if any, otherwise <code>null</code>.</param>
        /// <param name="outerProject">The <see cref="ProjectInstance"/> of the outer project if any, otherwise <code>null</code>.</param>
        public ProjectWithInnerNodes(string targetFramework, IMSBuildProject outerProject)
        {
            Add(targetFramework, outerProject);
        }

        /// <summary>
        /// Gets the <see cref="ProjectInstance"/> of the outer project.
        /// </summary>
        public IMSBuildProject OuterProject { get; private set; }


        /// <summary>
        /// Sets the outer project if <paramref name="targetFramework"/> is null, otherwise adds the inner project.
        /// </summary>
        /// <param name="targetFramework">The name of the target framework for the project if any, otherwise <code>null</code>.</param>
        /// <param name="project">The <see cref="ProjectInstance"/> of the project.</param>
        /// <returns>The current object.</returns>
        public ProjectWithInnerNodes Add(string targetFramework, IMSBuildProject project)
        {
            if (string.IsNullOrWhiteSpace(targetFramework))
            {
                OuterProject = project;
            }
            else
            {
                TryAdd(targetFramework, project);
            }

            return this;
        }
    }
}
