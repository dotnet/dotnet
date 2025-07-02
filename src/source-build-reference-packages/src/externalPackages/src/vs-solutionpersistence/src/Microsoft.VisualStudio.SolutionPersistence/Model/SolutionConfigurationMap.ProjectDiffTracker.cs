// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

internal sealed partial class SolutionConfigurationMap
{
    // Keeps track of changes to all project configuration dimensions.
    // This is used to tell if the values are the same and configuration rules can be created.
    private struct ProjectDiffTracker
    {
        internal DimensionDiffTracker<string> BuildTypeTracker;
        internal DimensionDiffTracker<string> PlatformTracker;
        internal DimensionDiffTracker<bool> BuildTracker;
        internal DimensionDiffTracker<bool> DeployTracker;

        internal readonly bool HasDifferences => this.BuildTypeTracker.HasDifferences || this.PlatformTracker.HasDifferences || this.BuildTracker.HasDifferences || this.DeployTracker.HasDifferences;

        internal readonly bool HasSame => this.BuildTypeTracker.SameDifference || this.PlatformTracker.SameDifference || this.BuildTracker.SameDifference || this.DeployTracker.SameDifference;

        // The ProjectDiffTracker is a struct, so this passes the array to
        // make sure this actually clears the diffs and a boxed copy.
        internal static void ClearDiffs(BuildDimension dimension, ProjectDiffTracker[] trackers)
        {
            for (int i = 0; i < trackers.Length; i++)
            {
                ref ProjectDiffTracker tracker = ref trackers[i];
                tracker.ClearDiffs(dimension);
            }
        }

        // The ProjectDiffTracker is a struct, so this passes the array to
        // make sure this actually clears the diffs and a boxed copy.
        internal static void ClearDiffs(ProjectDiffTracker[] trackers)
        {
            for (int i = 0; i < trackers.Length; i++)
            {
                ref ProjectDiffTracker tracker = ref trackers[i];
                tracker.ClearDiffs();
            }
        }

        internal void ObserveDifferentValue(in ProjectConfigMapping currentMapping)
        {
            this.BuildTypeTracker.ObserveDifferentValue(currentMapping.BuildType);
            this.PlatformTracker.ObserveDifferentValue(PlatformNames.Canonical(currentMapping.Platform));
            this.BuildTracker.ObserveDifferentValue(currentMapping.Build);
            this.DeployTracker.ObserveDifferentValue(currentMapping.Deploy);
        }

        internal void ObserveValue(in ProjectConfigMapping expectedMapping, in ProjectConfigMapping currentMapping)
        {
            this.BuildTypeTracker.ObserveValue(expectedMapping.BuildType, currentMapping.BuildType);
            this.PlatformTracker.ObserveValue(PlatformNames.Canonical(expectedMapping.Platform), PlatformNames.Canonical(currentMapping.Platform));
            this.BuildTracker.ObserveValue(expectedMapping.Build, currentMapping.Build);
            this.DeployTracker.ObserveValue(expectedMapping.Deploy, currentMapping.Deploy);
        }

        internal void ClearDiffs()
        {
            this.BuildTypeTracker.ClearDifferences();
            this.PlatformTracker.ClearDifferences();
            this.BuildTracker.ClearDifferences();
            this.DeployTracker.ClearDifferences();
        }

        internal void ClearDiffs(BuildDimension dimension)
        {
            switch (dimension)
            {
                case BuildDimension.BuildType: this.BuildTypeTracker.ClearDifferences(); break;
                case BuildDimension.Platform: this.PlatformTracker.ClearDifferences(); break;
                case BuildDimension.Build: this.BuildTracker.ClearDifferences(); break;
                case BuildDimension.Deploy: this.DeployTracker.ClearDifferences(); break;
            }
        }
    }
}
