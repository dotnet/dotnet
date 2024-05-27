// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsTargetFrameworkInfo2 : IVsTargetFrameworkInfo2
    {
        public IVsReferenceItems PackageReferences { get; }

        public IVsReferenceItems ProjectReferences { get; }

        public IVsReferenceItems PackageDownloads { get; }

        public IVsProjectProperties Properties { get; }

        public string TargetFrameworkMoniker { get; }

        public IVsReferenceItems FrameworkReferences { get; }

        public VsTargetFrameworkInfo2(
            string targetFrameworkMoniker,
            IEnumerable<IVsReferenceItem> packageReferences,
            IEnumerable<IVsReferenceItem> projectReferences,
            IEnumerable<IVsReferenceItem> packageDownloads,
            IEnumerable<IVsReferenceItem> frameworkReferences,
            IEnumerable<IVsProjectProperty> projectProperties,
            string originalTargetFramework = null,
            bool addTargetFrameworkProperties = true)
        {
            if (string.IsNullOrEmpty(targetFrameworkMoniker))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(targetFrameworkMoniker));
            }

            if (packageReferences == null)
            {
                throw new ArgumentNullException(nameof(packageReferences));
            }

            if (projectReferences == null)
            {
                throw new ArgumentNullException(nameof(projectReferences));
            }

            if (packageDownloads == null)
            {
                throw new ArgumentNullException(nameof(packageDownloads));
            }

            if (frameworkReferences == null)
            {
                throw new ArgumentNullException(nameof(frameworkReferences));
            }

            if (projectProperties == null)
            {
                throw new ArgumentNullException(nameof(projectProperties));
            }

            TargetFrameworkMoniker = targetFrameworkMoniker;
            PackageReferences = new VsReferenceItems(packageReferences);
            ProjectReferences = new VsReferenceItems(projectReferences);
            PackageDownloads = new VsReferenceItems(packageDownloads);
            FrameworkReferences = new VsReferenceItems(frameworkReferences);
            Properties =
                addTargetFrameworkProperties
                ? new VsProjectProperties(ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(targetFrameworkMoniker, originalTargetFramework).Select(ToIVsProjectProperty).Concat(projectProperties))
                : new VsProjectProperties(projectProperties);
        }

        private IVsProjectProperty ToIVsProjectProperty(KeyValuePair<string, string> pair)
        {
            return new VsProjectProperty(pair.Key, pair.Value);
        }
    }
}
