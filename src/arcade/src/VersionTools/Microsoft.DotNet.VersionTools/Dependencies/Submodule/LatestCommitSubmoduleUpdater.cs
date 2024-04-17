// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.DotNet.VersionTools.Util;

namespace Microsoft.DotNet.VersionTools.Dependencies.Submodule
{
    /// <summary>
    /// Updates the submodule at the given path to the latest commit available from its target
    /// repository.
    /// </summary>
    public class LatestCommitSubmoduleUpdater : SubmoduleUpdater
    {
        public string Repository { get; }

        public string Ref { get; }

        public LatestCommitSubmoduleUpdater(string repository, string @ref)
        {
            if (string.IsNullOrEmpty(repository))
            {
                throw new ArgumentException(
                    "A repository must be specified. For example, 'origin'. Got null or empty string.",
                    nameof(repository));
            }
            Repository = repository;

            if (string.IsNullOrEmpty(@ref))
            {
                throw new ArgumentException(
                    "A ref must be specified. For example, 'master'. Got null or empty string.",
                    nameof(@ref));
            }
            Ref = @ref;
        }

        protected override string GetDesiredCommitHash(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            SubmoduleDependencyInfo[] matchingInfos = dependencyInfos
                .OfType<SubmoduleDependencyInfo>()
                .Where(info => info.Repository == Repository)
                .ToArray();

            if (matchingInfos.Length != 1)
            {
                string matchingInfoString = string.Join(", ", matchingInfos.AsEnumerable());
                int allSubmoduleInfoCount = dependencyInfos.OfType<SubmoduleDependencyInfo>().Count();

                throw new ArgumentException(
                    $"For {Path}, expected exactly 1 {nameof(SubmoduleDependencyInfo)} " +
                    $"matching '{Repository}', " +
                    $"but found {matchingInfos.Length}/{allSubmoduleInfoCount}: " +
                    $"'{matchingInfoString}'");
            }

            usedDependencyInfos = matchingInfos;

            SubmoduleDependencyInfo matchingInfo = matchingInfos[0];
            Trace.TraceInformation($"For {Path}, found: {matchingInfo}");

            return matchingInfo.Commit;
        }

        protected override void FetchRemoteBranch()
        {
            GitCommand.Fetch(Path, Repository, Ref);
        }
    }
}
