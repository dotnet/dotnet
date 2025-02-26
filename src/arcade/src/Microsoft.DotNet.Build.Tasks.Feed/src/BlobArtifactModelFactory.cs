// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public interface IBlobArtifactModelFactory
    {
        BlobArtifactModel CreateBlobArtifactModel(ITaskItem item, string repoOrigin);
    }

    public class BlobArtifactModelFactory : IBlobArtifactModelFactory
    {
        private readonly TaskLoggingHelper _log;

        public BlobArtifactModelFactory(TaskLoggingHelper logger)
        {
            _log = logger;
        }

        /// <summary>
        /// Creates a BlobArtifactModel based on the data in the ITaskItem provided. Logs errors that may occur,
        /// but does not prevent the creation of the BlobArtifactModel. Errors do not prevent the creation because 
        /// we want to allow for the capture of all errors that may occur and report back all to the user so they can 
        /// mitigate all the errors found instead of one at a time, which would require continual re-runs of this code
        /// in order to find it. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public BlobArtifactModel CreateBlobArtifactModel(ITaskItem item, string repoOrigin)
        {
            string path = item.GetMetadata("RelativeBlobPath");
            if (string.IsNullOrEmpty(path))
            {
                _log.LogError($"Missing 'RelativeBlobPath' property on blob {item.ItemSpec}");
            }

            return new BlobArtifactModel
            {
                Attributes = MSBuildListSplitter.GetNamedProperties(item.GetMetadata("ManifestArtifactData")),
                Id = path,
                RepoOrigin = repoOrigin,
                OriginalFile = item.ItemSpec
            };
        }
    }
}
