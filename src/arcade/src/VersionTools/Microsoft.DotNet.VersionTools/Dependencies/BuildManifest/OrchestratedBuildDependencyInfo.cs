// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildManifest
{
    public class OrchestratedBuildDependencyInfo : IDependencyInfo
    {
        public static async Task<OrchestratedBuildDependencyInfo> CreateAsync(
            string simpleName,
            GitHubProject project,
            string @ref,
            string basePath,
            BuildManifestClient client)
        {
            OrchestratedBuildModel model = await client.FetchManifestAsync(
                project,
                @ref,
                basePath);

            if (model == null)
            {
                throw new ArgumentException(
                    $"Found no manifest for '{simpleName}' at " +
                    $"'{project.Segments}' '{basePath}' ref '{@ref}'");
            }

            return new OrchestratedBuildDependencyInfo(simpleName, model);
        }

        public string SimpleName { get; }

        public string SimpleVersion => OrchestratedBuildModel.Identity.BuildId;

        public OrchestratedBuildModel OrchestratedBuildModel { get; }

        public OrchestratedBuildDependencyInfo(
            string simpleName,
            OrchestratedBuildModel model)
        {
            SimpleName = simpleName;
            OrchestratedBuildModel = model;
        }

        public override string ToString() => $"{SimpleName} {SimpleVersion}";
    }
}
