// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

internal partial class RazorProjectInfoEndpointPublisher
{
    /// <summary>
    /// Compares two work items from project info publishing work queue.
    /// </summary>
    /// <remarks>
    /// Project updates and project removal are treated as equivalent since project removal
    /// work item should supersede any project updates in the queue. Project additions are not
    /// placed in the queue so project removal would never supersede/overwrite project addition
    /// (which would've resulted in us removing a project we never added).
    /// </remarks>
    private sealed class Comparer : IEqualityComparer<(IProjectSnapshot Project, bool Removal)>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals((IProjectSnapshot Project, bool Removal) x, (IProjectSnapshot Project, bool Removal) y)
        {
            // Project removal should replace project update so treat Removal and non-Removal
            // of the same IProjectSnapshot as equivalent work item
            var (snapshotX, _) = x;
            var (snapshotY, _) = y;

            return snapshotX.Key.Equals(snapshotY.Key);
        }

        public int GetHashCode((IProjectSnapshot Project, bool Removal) obj)
        {
            var (snapshot, _) = obj;

            return snapshot.Key.GetHashCode();
        }
    }
}
