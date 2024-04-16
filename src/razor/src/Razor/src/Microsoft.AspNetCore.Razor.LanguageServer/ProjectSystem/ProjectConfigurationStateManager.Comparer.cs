// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal partial class ProjectConfigurationStateManager
{
    /// <summary>
    /// Compares two work items from project state manager work queue.
    /// </summary>
    /// <remarks>
    /// Project updates and project removal are treated as equivalent since project removal
    /// work item should supersede any project updates in the queue. Project additions are not
    /// placed in the queue so project removal would never supersede/overwrite project addition
    /// (which would've resulted in us removing a project we never added).
    /// </remarks>
    private sealed class Comparer : IEqualityComparer<(ProjectKey ProjectKey, RazorProjectInfo? ProjectInfo)>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals((ProjectKey ProjectKey, RazorProjectInfo? ProjectInfo) x, (ProjectKey ProjectKey, RazorProjectInfo? ProjectInfo) y)
        {
            // Project removal should replace project update so treat Removal and non-Removal
            // of the same ProjectKey as equivalent work item
            return x.ProjectKey.Equals(y.ProjectKey);
        }

        public int GetHashCode((ProjectKey ProjectKey, RazorProjectInfo? ProjectInfo) obj)
        {
            var (projectKey, _) = obj;

            return projectKey.GetHashCode();
        }
    }
}
