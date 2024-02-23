// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine;
using NuGet.Commands;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine
{
    [Command(
         typeof(NuGetCommand),
         "list",
         "ListCommandDescription",
         UsageSummaryResourceName = "ListCommandUsageSummary",
         UsageDescriptionResourceName = "ListCommandUsageDescription",
         UsageExampleResourceName = "ListCommandUsageExamples")]
    [DeprecatedCommand(typeof(SearchCommand))]
    public class ListCommand : Command
    {
        private readonly List<string> _sources = new List<string>();

        [Option(typeof(NuGetCommand), "ListCommandSourceDescription")]
        public ICollection<string> Source
        {
            get { return _sources; }
        }

        [Option(typeof(NuGetCommand), "ListCommandVerboseListDescription")]
        public bool Verbose { get; set; }

        [Option(typeof(NuGetCommand), "ListCommandAllVersionsDescription")]
        public bool AllVersions { get; set; }

        [Option(typeof(NuGetCommand), "ListCommandPrerelease")]
        public bool Prerelease { get; set; }

        [Option(typeof(NuGetCommand), "ListCommandIncludeDelisted")]
        public bool IncludeDelisted { get; set; }

        private IList<Configuration.PackageSource> GetEndpointsAsync()
        {
            var configurationSources = SourceProvider.LoadPackageSources()
                .Where(p => p.IsEnabled)
                .ToList();

            IList<Configuration.PackageSource> packageSources;
            if (Source.Count > 0)
            {
                packageSources = Source
                    .Select(s => Common.PackageSourceProviderExtensions.ResolveSource(configurationSources, s))
                    .ToList();
            }
            else
            {
                packageSources = configurationSources;
            }
            return packageSources;
        }

        public async override Task ExecuteCommandAsync()
        {
            if (Verbose)
            {
                Console.WriteWarning(LocalizedResourceManager.GetString("Option_VerboseDeprecated"));
                Verbosity = Verbosity.Detailed;
            }

            var listCommandRunner = new ListCommandRunner();
            var listEndpoints = GetEndpointsAsync();

            var list = new ListArgs(Arguments,
                listEndpoints,
                Settings,
                Console,
                Console.PrintJustified,
                Verbosity == Verbosity.Detailed,
                LocalizedResourceManager.GetString("ListCommandNoPackages"),
                LocalizedResourceManager.GetString("ListCommand_LicenseUrl"),
                LocalizedResourceManager.GetString("ListCommand_ListNotSupported"),
                AllVersions,
                IncludeDelisted,
                Prerelease,
                CancellationToken.None);

            await listCommandRunner.ExecuteCommand(list);
        }
    }
}
