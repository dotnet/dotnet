// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Rules;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    public sealed class NuGetProjectUpgradeWindowModel
    {
        public bool HasIssues { get; }

        public string Title { get; }

        public IEnumerable<PackageIdentity> NotFoundPackages { get; }

        public bool HasNotFoundPackages => NotFoundPackages.Any();

        public ObservableCollection<NuGetProjectUpgradeDependencyItem> UpgradeDependencyItems { get; }

        public IEnumerable<NuGetProjectUpgradeDependencyItem> DirectDependencies => UpgradeDependencyItems
            .Where(e => e.IsTopLevel);

        public IEnumerable<NuGetProjectUpgradeDependencyItem> TransitiveDependencies => UpgradeDependencyItems
            .Where(e => !e.IsTopLevel);

        private NuGetProjectUpgradeWindowModel(
            string title,
            ObservableCollection<NuGetProjectUpgradeDependencyItem> upgradeDependencyItems,
            HashSet<PackageIdentity> notFoundPackages,
            bool hasIssues)
        {
            Title = title;
            UpgradeDependencyItems = upgradeDependencyItems;
            NotFoundPackages = notFoundPackages;
            HasIssues = hasIssues;
        }

        public static async Task<NuGetProjectUpgradeWindowModel> CreateAsync(
            IServiceBroker serviceBroker,
            IProjectContextInfo project,
            IList<PackageDependencyInfo> packageDependencyInfos,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(project);
            Assumes.NotNull(packageDependencyInfos);

            cancellationToken.ThrowIfCancellationRequested();

            string projectName = await project.GetUniqueNameOrNameAsync(serviceBroker, CancellationToken.None);
            string title = string.Format(CultureInfo.CurrentCulture, Resources.WindowTitle_NuGetMigrator, projectName);
            var notFoundPackages = new HashSet<PackageIdentity>();
            var hasIssues = false;

            IEnumerable<NuGetProjectUpgradeDependencyItem> dependencyItems =
                PackageGraphAnalysisUtilities.GetPackagesWithDependants(packageDependencyInfos)
                    .Select(e => new NuGetProjectUpgradeDependencyItem(e.Identity, e));

            foreach (NuGetProjectUpgradeDependencyItem dependencyItem in dependencyItems)
            {
                (bool _, string packagePath) = await project.TryGetInstalledPackageFilePathAsync(
                    serviceBroker,
                    dependencyItem.Identity,
                    cancellationToken);

                InitPackageUpgradeIssues(packagePath, dependencyItem, notFoundPackages, ref hasIssues);
            }

            var upgradeDependencyItems = new ObservableCollection<NuGetProjectUpgradeDependencyItem>(dependencyItems);

            return new NuGetProjectUpgradeWindowModel(
                title,
                upgradeDependencyItems,
                notFoundPackages,
                hasIssues);
        }

        private static void InitPackageUpgradeIssues(
            string packagePath,
            NuGetProjectUpgradeDependencyItem package,
            HashSet<PackageIdentity> notFoundPackages,
            ref bool hasIssues)
        {
            var packageIdentity = new PackageIdentity(package.Id, NuGetVersion.Parse(package.Version));

            if (string.IsNullOrEmpty(packagePath))
            {
                hasIssues = true;
                notFoundPackages.Add(packageIdentity);
                package.Issues.Add(PackagingLogMessage.CreateWarning(
                    string.Format(CultureInfo.CurrentCulture, Resources.Upgrader_PackageNotFound, packageIdentity.Id),
                    NuGetLogCode.NU5500));
            }
            else
            {
                using (var reader = new PackageArchiveReader(packagePath))
                {
                    IEnumerable<IPackageRule> packageRules = RuleSet.PackagesConfigToPackageReferenceMigrationRuleSet;
                    IList<PackagingLogMessage> issues = package.Issues;

                    foreach (IPackageRule rule in packageRules)
                    {
                        IOrderedEnumerable<PackagingLogMessage> foundIssues = rule.Validate(reader)
                            .OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture);

                        if (foundIssues != null && foundIssues.Any())
                        {
                            hasIssues = true;
                        }

                        issues.AddRange(foundIssues);
                    }

                    if (!package.InstallAsTopLevel)
                    {
                        PromoteToTopLevelIfNeeded(reader, package);
                    }
                }
            }
        }

        /* The package will be installed as top level if :
         * 1) The package contains build, buildCrossTargeting, contentFiles or analyzers folder.
         * 2) The package has developmentDependency set to true.
         */
        private static void PromoteToTopLevelIfNeeded(PackageArchiveReader reader, NuGetProjectUpgradeDependencyItem item)
        {
            if (reader.GetDevelopmentDependency() ||
                reader.GetFiles(PackagingConstants.Folders.Build).Any() ||
                reader.GetFiles(PackagingConstants.Folders.BuildCrossTargeting).Any() ||
                reader.GetFiles(PackagingConstants.Folders.ContentFiles).Any() ||
                reader.GetFiles(PackagingConstants.Folders.Analyzers).Any())
            {
                item.InstallAsTopLevel = true;
            }
        }
    }
}
