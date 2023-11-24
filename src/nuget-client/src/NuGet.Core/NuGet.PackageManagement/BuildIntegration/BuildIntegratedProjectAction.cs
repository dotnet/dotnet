// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Commands;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.PackageManagement
{
    public class BuildIntegratedProjectAction : NuGetProjectAction
    {
        /// <summary>
        /// Before the update
        /// </summary>
        public LockFile OriginalLockFile { get; }

        /// <summary>
        /// After applying the changes
        /// </summary>
        public RestoreResult RestoreResult { get; }

        /// <summary>
        /// After applying the changes
        /// </summary>
        public RestoreResultPair RestoreResultPair { get; }

        /// <summary>
        /// Sources used for package restore.
        /// </summary>
        public IReadOnlyList<SourceRepository> Sources { get; }

        /// <summary>
        /// Original user actions.
        /// </summary>
        [Obsolete("The internal ActionAndContextList property should be used.")]
        public IReadOnlyList<NuGetProjectAction> OriginalActions { get; }

        /// <summary>
        /// The context necessary for installing a package.
        /// </summary>
        [Obsolete("The internal ActionAndContextList property should be used.")]
        public BuildIntegratedInstallationContext InstallationContext { get; }

        internal IReadOnlyList<(NuGetProjectAction, BuildIntegratedInstallationContext)> ActionAndContextList { get; }

        [Obsolete("This type is not expected to be created externally.")]
        public BuildIntegratedProjectAction(NuGetProject project,
            PackageIdentity packageIdentity,
            NuGetProjectActionType nuGetProjectActionType,
            LockFile originalLockFile,
            RestoreResultPair restoreResultPair,
            IReadOnlyList<SourceRepository> sources,
            IReadOnlyList<NuGetProjectAction> originalActions,
            BuildIntegratedInstallationContext installationContext)
            : this(project, packageIdentity, nuGetProjectActionType, originalLockFile, restoreResultPair, sources, originalActions, installationContext, versionRange: null)
        {
        }

        [Obsolete("This type is not expected to be created externally.")]
        public BuildIntegratedProjectAction(NuGetProject project,
            PackageIdentity packageIdentity,
            NuGetProjectActionType nuGetProjectActionType,
            LockFile originalLockFile,
            RestoreResultPair restoreResultPair,
            IReadOnlyList<SourceRepository> sources,
            IReadOnlyList<NuGetProjectAction> originalActions,
            BuildIntegratedInstallationContext installationContext,
            VersionRange? versionRange)
            : base(packageIdentity, nuGetProjectActionType, project, sourceRepository: null, versionRange)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (originalLockFile == null)
            {
                throw new ArgumentNullException(nameof(originalLockFile));
            }

            if (restoreResultPair == null)
            {
                throw new ArgumentNullException(nameof(restoreResultPair));
            }

            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (originalActions == null)
            {
                throw new ArgumentNullException(nameof(originalActions));
            }

            if (installationContext == null)
            {
                throw new ArgumentNullException(nameof(installationContext));
            }

            OriginalLockFile = originalLockFile;
            RestoreResult = restoreResultPair.Result;
            RestoreResultPair = restoreResultPair;
            Sources = sources;
            OriginalActions = originalActions;
            InstallationContext = installationContext;
            ActionAndContextList = originalActions.Select(e => (e, installationContext)).ToList();
        }

        internal BuildIntegratedProjectAction(NuGetProject project,
            PackageIdentity packageIdentity,
            NuGetProjectActionType nuGetProjectActionType,
            LockFile originalLockFile,
            RestoreResultPair restoreResultPair,
            IReadOnlyList<SourceRepository> sources,
            IReadOnlyList<(NuGetProjectAction, BuildIntegratedInstallationContext)> originalActionsAndInstallationContexts,
            VersionRange versionRange)
            : base(packageIdentity, nuGetProjectActionType, project, sourceRepository: null, versionRange)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (originalLockFile == null)
            {
                throw new ArgumentNullException(nameof(originalLockFile));
            }

            if (restoreResultPair == null)
            {
                throw new ArgumentNullException(nameof(restoreResultPair));
            }

            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (originalActionsAndInstallationContexts == null)
            {
                throw new ArgumentNullException(nameof(originalActionsAndInstallationContexts));
            }

            if (originalActionsAndInstallationContexts.Count < 1)
            {
                throw new ArgumentException("Must contain at least 1 element.", nameof(originalActionsAndInstallationContexts));
            }

            OriginalLockFile = originalLockFile;
            RestoreResult = restoreResultPair.Result;
            RestoreResultPair = restoreResultPair;
            Sources = sources;
            ActionAndContextList = originalActionsAndInstallationContexts;
#pragma warning disable CS0618 // Type or member is obsolete
            OriginalActions = originalActionsAndInstallationContexts.Select(e => e.Item1).ToList();
            InstallationContext = originalActionsAndInstallationContexts[0].Item2;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public IReadOnlyList<NuGetProjectAction> GetProjectActions()
        {
            var actions = new List<NuGetProjectAction>();

            if (RestoreResult.Success)
            {
                var added = BuildIntegratedRestoreUtility.GetAddedPackages(OriginalLockFile, RestoreResult.LockFile);
                var removed = BuildIntegratedRestoreUtility.GetAddedPackages(RestoreResult.LockFile, OriginalLockFile);

                foreach (var package in removed)
                {
                    actions.Add(CreateUninstallProjectAction(package, Project));
                }

                foreach (var package in added)
                {
                    actions.Add(CreateInstallProjectAction(package, sourceRepository: null, project: Project));
                }
            }

            return actions;
        }
    }
}
