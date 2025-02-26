// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public interface IPackageArtifactModelFactory
    {
        PackageArtifactModel CreatePackageArtifactModel(ITaskItem item, string repoOrigin);
    }

    public class PackageArtifactModelFactory : IPackageArtifactModelFactory
    {
        private readonly INupkgInfoFactory _nupkgInfoFactory;
        private readonly TaskLoggingHelper _log;

        public PackageArtifactModelFactory(INupkgInfoFactory nupkgInfoFactory,
            TaskLoggingHelper logger)
        {
            _nupkgInfoFactory = nupkgInfoFactory;
            _log = logger;
        }

        public PackageArtifactModel CreatePackageArtifactModel(ITaskItem item, string repoOrigin)
        {
            _log.LogMessage($"Creating NupkgInfo based on '{item.ItemSpec}'");

            NupkgInfo info = _nupkgInfoFactory.CreateNupkgInfo(item.ItemSpec);

            return new PackageArtifactModel
            {
                Attributes = MSBuildListSplitter.GetNamedProperties(item.GetMetadata("ManifestArtifactData")),
                Id = info.Id,
                Version = info.Version,
                RepoOrigin = repoOrigin,
                OriginalFile = item.ItemSpec
            };
        }
    }
}
