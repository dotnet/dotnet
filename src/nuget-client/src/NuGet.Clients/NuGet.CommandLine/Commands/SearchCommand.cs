// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine
{

    [Command(typeof(NuGetCommand), "search", "SearchCommandDescription",
            UsageSummaryResourceName = "SearchCommandUsageSummary", UsageExampleResourceName = "SearchCommandUsageExamples")]
    public class SearchCommand : Command
    {
        private readonly int _lineSeparatorLength = 20;

        private readonly List<string> _sources = new List<string>();

        [Option(typeof(NuGetCommand), "SearchCommandSourceDescription")]
        public ICollection<string> Source => _sources;

        [Option(typeof(NuGetCommand), "SearchCommandPreRelease")]
        public bool PreRelease { get; set; } = false;

        [Option(typeof(NuGetCommand), "SearchCommandTake")]
        public int Take { get; set; } = 20;

        private IList<PackageSource> GetEndpointsAsync()
        {
            List<PackageSource> configurationSources = SourceProvider.LoadPackageSources()
                .Where(p => p.IsEnabled)
                .ToList();

            IList<PackageSource> packageSources;
            if (Source.Count > 0)
            {
                packageSources = Source
                    .Select(s => PackageSourceProviderExtensions.ResolveSource(configurationSources, s))
                    .ToList();
            }
            else
            {
                packageSources = configurationSources;
            }
            return packageSources;
        }

        public override async Task ExecuteCommandAsync()
        {
            string sourceSeparator = new string('=', _lineSeparatorLength);
            string packageSeparator = new string('-', _lineSeparatorLength);

            ILogger logger = Console;
            CancellationToken cancellationToken = CancellationToken.None;

            SearchFilter searchFilter = new SearchFilter(includePrerelease: PreRelease);

            var taskList = new List<(Task<IEnumerable<IPackageSearchMetadata>>, PackageSource)>();
            IList<PackageSource> listEndpoints = GetEndpointsAsync();

            AvoidHttpSources(listEndpoints);

            foreach (PackageSource source in listEndpoints)
            {
                SourceRepository repository = Repository.Factory.GetCoreV3(source);
                PackageSearchResource resource = await repository.GetResourceAsync<PackageSearchResource>(cancellationToken);

                if (resource is null)
                {
                    taskList.Add((null, source));
                    continue;
                }

                taskList.Add((Task.Run(() => resource.SearchAsync(
                    string.Join(" ", Arguments).Trim(), // The arguments are joined with spaces to form a single query string
                    searchFilter,
                    skip: 0,
                    take: Take,
                    logger,
                    cancellationToken)), source));
            }

            foreach (var taskItem in taskList)
            {
                var (task, source) = taskItem;

                if (task is null)
                {
                    Console.WriteLine(sourceSeparator);
                    System.Console.WriteLine($"Source: {source.Name}");
                    System.Console.WriteLine(packageSeparator);
                    System.Console.WriteLine("Failed to obtain a search resource.");
                    Console.WriteLine(packageSeparator);
                    System.Console.WriteLine();
                    continue;
                }

                var results = await task;

                Console.WriteLine(sourceSeparator);
                System.Console.WriteLine($"Source: {source.Name}"); // System.Console is used so that output is not suppressed by Verbosity.Quiet

                if (results.Any())
                {
                    if (Verbosity == Verbosity.Quiet)
                    {
                        System.Console.WriteLine(packageSeparator);
                    }

                    PrintResults(results);
                }
                else
                {
                    System.Console.WriteLine(packageSeparator);
                    System.Console.WriteLine("No results found.");
                    Console.WriteLine(packageSeparator);
                    System.Console.WriteLine();
                }
            }
        }

        private static void AvoidHttpSources(IList<PackageSource> packageSources)
        {
            List<PackageSource> httpPackageSources = null;
            foreach (PackageSource packageSource in packageSources)
            {
                if (packageSource.IsHttp && !packageSource.IsHttps && !packageSource.AllowInsecureConnections)
                {
                    if (httpPackageSources == null)
                    {
                        httpPackageSources = new();
                    }
                    httpPackageSources.Add(packageSource);
                }
            }

            if (httpPackageSources != null && httpPackageSources.Count != 0)
            {
                if (httpPackageSources.Count == 1)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture,
                        NuGetResources.Error_HttpSource_Single,
                        "search",
                        httpPackageSources[0]));
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture,
                        NuGetResources.Error_HttpSources_Multiple,
                        "search",
                        Environment.NewLine + string.Join(Environment.NewLine, httpPackageSources.Select(e => e.Name))));
                }
            }
        }

        private void PrintResults(IEnumerable<IPackageSearchMetadata> results)
        {
            string packageSeparator = new string('-', _lineSeparatorLength);

            foreach (IPackageSearchMetadata result in results)
            {
                Console.WriteLine(packageSeparator);

                CultureInfo culture = CultureInfo.CurrentCulture;

                StringBuilder content = new StringBuilder();
                content.Append($"> {result.Identity.Id} | {result.Identity.Version.ToNormalizedString()}"); // Basic info (Name | Version)

                if (Verbosity != Verbosity.Quiet)
                {
                    if (result.DownloadCount != null)
                    {
                        string downloads = string.Format(culture, "{0:N}", result.DownloadCount);
                        content.Append($" | Downloads: {downloads.Substring(0, downloads.Length - 3)}");
                    }
                    else
                    {
                        content.Append(" | Downloads: N/A");
                    }
                }

                System.Console.WriteLine(content.ToString()); // System.Console is used so that output is not suppressed by Verbosity.Quiet

                if (Verbosity != Verbosity.Quiet && result.Description != null)
                {
                    string description = result.Description;

                    if (Verbosity == Verbosity.Normal && description.Length > 100)
                    {
                        description = description.Substring(0, 100) + "...";
                    }

                    Console.PrintJustified(2, description);
                }
            }

            Console.WriteLine(packageSeparator);
            System.Console.WriteLine();
        }

        public override bool IncludedInHelp(string optionName)
        {
            if (string.Equals(optionName, "ConfigFile", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return base.IncludedInHelp(optionName);
        }
    }
}
