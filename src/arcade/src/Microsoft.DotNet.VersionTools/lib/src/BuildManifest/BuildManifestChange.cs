// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.VersionTools.BuildManifest
{
    public class BuildManifestChange
    {
        public BuildManifestLocation Location { get; }

        public string CommitMessage { get; }

        public string OrchestratedBuildId { get; }

        public IEnumerable<string> SemaphorePaths { get; }

        public Action<OrchestratedBuildModel> ApplyModelChanges { get; }

        public IEnumerable<JoinSemaphoreGroup> JoinSemaphoreGroups { get; set; }

        public IEnumerable<SupplementaryUploadRequest> SupplementaryUploads { get; set; }

        public BuildManifestChange(
            BuildManifestLocation location,
            string commitMessage,
            string orchestratedBuildId,
            IEnumerable<string> semaphorePaths,
            Action<OrchestratedBuildModel> applyModelChanges)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            if (string.IsNullOrEmpty(commitMessage))
            {
                throw new ArgumentException(nameof(commitMessage));
            }

            if (string.IsNullOrEmpty(orchestratedBuildId))
            {
                throw new ArgumentException(nameof(orchestratedBuildId));
            }

            if (applyModelChanges == null)
            {
                throw new ArgumentNullException(nameof(applyModelChanges));
            }

            if (semaphorePaths == null)
            {
                throw new ArgumentNullException(nameof(semaphorePaths));
            }

            Location = location;
            CommitMessage = commitMessage;
            OrchestratedBuildId = orchestratedBuildId;
            SemaphorePaths = semaphorePaths;
            ApplyModelChanges = applyModelChanges;
        }
    }
}
