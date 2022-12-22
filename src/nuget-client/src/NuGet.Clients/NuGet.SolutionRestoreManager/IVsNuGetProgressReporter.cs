// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Commands;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Provides updates about PackageReference solution restore. Only PackageReference projects are reported.
    /// </summary>
    /// <remarks>Projects that are "up to date" may not appear. Just because a project will be restored, doesn't mean that it will not no-op. </remarks>
    public interface IVsNuGetProgressReporter : IRestoreProgressReporter
    {
        /// <summary>
        /// List of PackageReference projects that will be restored.
        /// </summary>
        /// <param name="projects">Projects that are going to be restored.</param>
        void StartSolutionRestore(IReadOnlyList<string> projects);

        /// <summary>
        /// List of PackageReference projects that were be restored.
        /// </summary>
        /// <param name="projects">Projects that were restored.</param>
        void EndSolutionRestore(IReadOnlyList<string> projects);
    }
}
