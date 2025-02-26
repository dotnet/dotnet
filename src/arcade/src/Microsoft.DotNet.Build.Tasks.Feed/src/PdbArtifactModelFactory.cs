// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public interface IPdbArtifactModelFactory
    {
        PdbArtifactModel CreatePdbArtifactModel(ITaskItem item, string repoOrigin);
    }

    public class PdbArtifactModelFactory : IPdbArtifactModelFactory
    {
        private readonly TaskLoggingHelper _log;

        public PdbArtifactModelFactory(TaskLoggingHelper logger)
        {
            _log = logger;
        }

        /// <summary>
        /// Creates a PdbArtifactModel based on the data in the ITaskItem provided. Logs errors that may occur,
        /// but does not prevent the creation of the PdbArtifactModel. This approach captures all errors so that the user
        /// may mitigate all issues at once rather than addressing them one-by-one.
        /// </summary>
        /// <param name="item">The task item containing metadata.</param>
        /// <param name="repoOrigin">The repository origin identifier.</param>
        /// <returns>A populated PdbArtifactModel.</returns>
        public PdbArtifactModel CreatePdbArtifactModel(ITaskItem item, string repoOrigin)
        {
            string path = item.GetMetadata("RelativePdbPath");
            if (string.IsNullOrEmpty(path))
            {
                _log.LogError($"Missing 'RelativePdbPath' property on pdb {item.ItemSpec}");
            }

            return new PdbArtifactModel
            {
                Attributes = MSBuildListSplitter.GetNamedProperties(item.GetMetadata("ManifestArtifactData")),
                Id = path,
                RepoOrigin = repoOrigin,
                OriginalFile = item.ItemSpec
            };
        }
    }
}
