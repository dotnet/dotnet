// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetApplicableAssetsFromPackageReports : BuildTask
    {
        [Required]
        public string[] PackageReports { get; set; }

        /// <summary>
        /// TargetMoniker to use when resolving assets.  EG: netcoreapp1.0, netstandard1.4
        /// </summary>
        [Required]
        public string TargetMoniker { get; set; }

        /// <summary>
        /// If specified will be used when resolving runtime assets, otherwise no RID will be used.
        /// </summary>
        public string TargetRuntime { get; set; }

        [Output]
        public ITaskItem[] CompileAssets { get; set; }

        [Output]
        public ITaskItem[] RuntimeAssets { get; set; }

        [Output]
        public ITaskItem[] NativeAssets { get; set; }

        [Output]
        public ITaskItem[] BuildProjects { get; set; }

        /// <summary>
        /// Generates a table in markdown that lists the API version supported by 
        /// various packages at all levels of NETStandard.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (PackageReports == null || PackageReports.Length == 0)
            {
                Log.LogError("PackageReports argument must be specified");
                return false;
            }

            if (TargetMoniker == null || TargetMoniker.Length == 0)
            {
                Log.LogError("TargetMoniker argument must be specified");
                return false;
            }

            NuGetFramework fx = NuGetFramework.Parse(TargetMoniker);

            string targetString = String.IsNullOrEmpty(TargetRuntime) ? fx.ToString() : $"{fx}/{TargetRuntime}";

            var compileAssets = new List<ITaskItem>();
            var runtimeAssets = new List<ITaskItem>();
            var nativeAssets = new List<ITaskItem>();
            var buildProjects = new List<BuildProject>();

            foreach (var reportPath in PackageReports)
            {
                var report = PackageReport.Load(reportPath);

                Target target = null;
                if (report.Targets.TryGetValue(targetString, out target))
                {
                    compileAssets.AddRange(target.CompileAssets.Select(c => ItemFromApplicableAsset(c, report.Id, report.Version)));
                    buildProjects.AddRange(target.CompileAssets.Select(c => c.SourceProject).Where(bp => bp != null));
                    runtimeAssets.AddRange(target.RuntimeAssets.Select(r => ItemFromApplicableAsset(r, report.Id, report.Version)));
                    buildProjects.AddRange(target.RuntimeAssets.Select(r => r.SourceProject).Where(bp => bp != null));
                    nativeAssets.AddRange(target.NativeAssets.Select(r => ItemFromApplicableAsset(r, report.Id, report.Version)));
                    buildProjects.AddRange(target.NativeAssets.Select(r => r.SourceProject).Where(bp => bp != null));
                }
                else
                {
                    Log.LogMessage($"No assets found for '{report.Id}' applicable to '{targetString}'.");
                }
            }

            CompileAssets = compileAssets.ToArray();
            RuntimeAssets = runtimeAssets.ToArray();
            NativeAssets = nativeAssets.ToArray();
            BuildProjects = buildProjects.Distinct().Select(bp => bp.ToItem()).ToArray();

            return !Log.HasLoggedErrors;
        }
        private ITaskItem ItemFromApplicableAsset(PackageAsset asset, string id, string version)
        {
            var item = new TaskItem(asset.LocalPath);
            item.SetMetadata("PackagePath", asset.PackagePath);
            item.SetMetadata("NuGetPackageId", id);
            item.SetMetadata("NuGetPackageVersion", version);
            return item;
        }
    }
}
