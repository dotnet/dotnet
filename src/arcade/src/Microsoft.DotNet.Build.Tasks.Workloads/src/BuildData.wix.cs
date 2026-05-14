// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Deployment.DotNet.Releases;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Describes information related to building MSIs for a workload pack that
    /// potentially spans multiple feature bands.
    /// </summary>
    internal class BuildData
    {
        /// <summary>
        /// The workload pack to use for creating an MSI
        /// </summary>
        public WorkloadPackPackage Package
        {
            get;
        }

        /// <summary>
        /// For each platform, the set of feature bands that include contain a reference to this pack.
        /// </summary>
        public Dictionary<string, HashSet<ReleaseVersion>> FeatureBands = new();        

        public BuildData(WorkloadPackPackage package)
        {
            Package = package;
        }
    }
}
