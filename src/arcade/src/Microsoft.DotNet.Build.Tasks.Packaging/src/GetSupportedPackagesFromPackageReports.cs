// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetSupportedPackagesFromPackageReports : BuildTask
    {
        [Required]
        public string[] PackageReports { get; set; }

        [Output]
        public ITaskItem[] SupportedPackages { get; set; }

        public override bool Execute()
        {
            var supportedPackages = new List<ITaskItem>();
            foreach (var packageReport in PackageReports.NullAsEmpty())
            {
                var report = PackageReport.Load(packageReport);
                var packageId = report.Id;
                var packageVersion = report.Version;

                var supportedTargets = report.Targets.Values.Where(target => report.SupportedFrameworks.ContainsKey(target.Framework));
                var fxRIDGroupings = supportedTargets.GroupBy(target => target.Framework, target => target.RuntimeID);

                foreach (var fxRIDGrouping in fxRIDGroupings)
                {
                    var fx = fxRIDGrouping.Key;
                    var rids = fxRIDGrouping.ToArray();
                    var nugetFx = NuGetFramework.Parse(fx);

                    var supportedPackage = new TaskItem(packageId);
                    supportedPackage.SetMetadata("Version", packageVersion);
                    supportedPackage.SetMetadata("TargetFramework", fx);
                    supportedPackage.SetMetadata("TargetFrameworkShort", nugetFx.GetShortFolderName());

                    var ridList = string.Join(";", rids);

                    if (!String.IsNullOrEmpty(ridList))
                    {
                        supportedPackage.SetMetadata("RuntimeIdentifiers", ridList);
                    }

                    supportedPackages.Add(supportedPackage);
                }
            }

            SupportedPackages = supportedPackages.ToArray();

            return !Log.HasLoggedErrors;
        }
    }
}
