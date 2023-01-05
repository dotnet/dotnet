// All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.Telemetry
{
    public sealed class PackageManagerUIRefreshEvent : TelemetryEvent
    {
        private const string EventName = "PMUIRefresh";

        private PackageManagerUIRefreshEvent(
            Guid parentId,
            bool isSolutionLevel,
            RefreshOperationSource refreshSource,
            RefreshOperationStatus refreshStatus,
            ItemFilter tab,
            bool isUIFiltering,
            TimeSpan timeSinceLastRefresh,
            double? duration,
            string projectId = null,
            NuGetProjectKind? projectKind = null) : base(EventName)
        {
            base["ParentId"] = parentId.ToString();
            base["IsSolutionLevel"] = isSolutionLevel;
            base["RefreshSource"] = refreshSource;
            base["RefreshStatus"] = refreshStatus;
            base["Tab"] = tab;
            base["IsUIFiltering"] = isUIFiltering;
            base["TimeSinceLastRefresh"] = timeSinceLastRefresh.TotalMilliseconds;

            if (!isSolutionLevel)
            {
                if (projectId != null)
                {
                    base["ProjectId"] = projectId;
                }
                if (projectKind.HasValue)
                {
                    base["ProjectKind"] = projectKind;
                }
            }

            if (duration.HasValue)
            {
                base["Duration"] = duration;
            }
        }

        public static PackageManagerUIRefreshEvent ForProject(
            Guid parentId,
            RefreshOperationSource refreshSource,
            RefreshOperationStatus refreshStatus,
            ItemFilter tab,
            bool isUIFiltering,
            TimeSpan timeSinceLastRefresh,
            double? duration,
            string projectId,
            NuGetProjectKind projectKind)
        {
            var evt = new PackageManagerUIRefreshEvent(
                parentId,
                isSolutionLevel: false,
                refreshSource,
                refreshStatus,
                tab,
                isUIFiltering,
                timeSinceLastRefresh,
                duration,
                projectId,
                projectKind);

            return evt;
        }

        public static PackageManagerUIRefreshEvent ForSolution(
                Guid parentId,
                RefreshOperationSource refreshSource,
                RefreshOperationStatus refreshStatus,
                ItemFilter tab,
                bool isUIFiltering,
                TimeSpan timeSinceLastRefresh,
                double? duration)
        {
            var evt = new PackageManagerUIRefreshEvent(
                parentId,
                isSolutionLevel: true,
                refreshSource,
                refreshStatus,
                tab,
                isUIFiltering,
                timeSinceLastRefresh,
                duration);

            return evt;
        }
    }

    public enum RefreshOperationSource
    {
        ActionsExecuted,
        CacheUpdated,
        CheckboxPrereleaseChanged,
        ClearSearch,
        ExecuteAction,
        FilterSelectionChanged,
        PackageManagerLoaded,
        PackageSourcesChanged,
        ProjectsChanged,
        RestartSearchCommand,
        SourceSelectionChanged,
        PackagesMissingStatusChanged,
    }

    public enum RefreshOperationStatus
    {
        Success,
        NotApplicable,
        NoOp,
        Failed,
    }
}
