// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.CommandLine.XPlat
{
    internal class ListPackageArgs
    {
        public ILogger Logger { get; }
        public string Path { get; }
        public List<PackageSource> PackageSources { get; }
        public List<string> Frameworks { get; }
        public ReportType ReportType { get; }
        public IReportRenderer Renderer { get; }
        public string ArgumentText { get; }
        public bool IncludeTransitive { get; }
        public bool Prerelease { get; }
        public bool HighestPatch { get; }
        public bool HighestMinor { get; }
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// A constructor for the arguments of list package
        /// command. This is used to execute the runner's
        /// method
        /// </summary>
        /// <param name="path"> The path to the solution or project file </param>
        /// <param name="packageSources"> The sources for the packages to check in the case of --outdated </param>
        /// <param name="frameworks"> The user inputed frameworks to look up for their packages </param>
        /// <param name="reportType"> Which report we're producing (e.g. --outdated) </param>
        /// <param name="renderer">The report output renderer (e.g. console, json)</param>
        /// <param name="includeTransitive"> Bool for --include-transitive present </param>
        /// <param name="prerelease"> Bool for --include-prerelease present </param>
        /// <param name="highestPatch"> Bool for --highest-patch present </param>
        /// <param name="highestMinor"> Bool for --highest-minor present </param>
        /// <param name="logger"></param>
        /// <param name="cancellationToken"></param>
        public ListPackageArgs(
            string path,
            List<PackageSource> packageSources,
            List<string> frameworks,
            ReportType reportType,
            IReportRenderer renderer,
            bool includeTransitive,
            bool prerelease,
            bool highestPatch,
            bool highestMinor,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            PackageSources = packageSources ?? throw new ArgumentNullException(nameof(packageSources));
            Frameworks = frameworks ?? throw new ArgumentNullException(nameof(frameworks));
            ReportType = reportType;
            Renderer = renderer;
            IncludeTransitive = includeTransitive;
            Prerelease = prerelease;
            HighestPatch = highestPatch;
            HighestMinor = highestMinor;
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CancellationToken = cancellationToken;
            ArgumentText = GetReportParameters();
        }

        private string GetReportParameters()
        {
            StringBuilder sb = new StringBuilder();

            switch (ReportType)
            {
                case ReportType.Default:
                    break;
                case ReportType.Deprecated:
                    sb.Append(" --deprecated");
                    break;
                case ReportType.Outdated:
                    sb.Append(" --outdated");
                    break;
                case ReportType.Vulnerable:
                    sb.Append(" --vulnerable");
                    break;
                default:
                    break;
            }

            if (IncludeTransitive)
            {
                sb.Append(" --include-transitive");
            }

            if (Frameworks != null && Frameworks.Any())
            {
                sb.Append(" --framework " + string.Join(" ", Frameworks));
            }

            if (Prerelease)
            {
                sb.Append(" --include-prerelease");
            }

            if (HighestMinor)
            {
                sb.Append(" --highest-minor");
            }

            if (HighestPatch)
            {
                sb.Append("--highest-patch");
            }

            return sb.ToString().Trim();
        }
    }
}
