// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageSearchResultTableRenderer : IPackageSearchResultRenderer
    {
        private string _searchTerm;
        private ILoggerWithColor _loggerWithColor;
        private const int LineSeparatorLength = 40;
        private static readonly string SourceSeparator = new('*', LineSeparatorLength);
        private PackageSearchVerbosity _verbosity;
        private bool _exactMatch;

        public PackageSearchResultTableRenderer(string searchTerm, ILoggerWithColor loggerWithColor, PackageSearchVerbosity verbosity, bool exactMatch)
        {
            _searchTerm = searchTerm;
            _loggerWithColor = loggerWithColor;
            _verbosity = verbosity;
            _exactMatch = exactMatch;
        }

        public void Add(PackageSource source, IEnumerable<IPackageSearchMetadata> completedSearch)
        {
            _loggerWithColor.LogMinimal(SourceSeparator);
            _loggerWithColor.LogMinimal($"Source: {source.Name} ({source.SourceUri})");

            ITableFormatStrategy strategy = TableFormatStrategyFactory.GetStrategy(_verbosity, _exactMatch);
            Table table = strategy.CreateTable();
            PopulateTableWithResultsAsync(completedSearch, table, _verbosity);
            table.PrintResult(_searchTerm, _loggerWithColor);
        }

        public void Add(PackageSource source, PackageSearchProblem packageSearchProblem)
        {
            _loggerWithColor.LogMinimal(SourceSeparator);
            _loggerWithColor.LogMinimal($"Source: {source.Name} ({source.SourceUri})");

            if (packageSearchProblem.ProblemType == PackageSearchProblemType.Error)
            {
                _loggerWithColor.LogError(packageSearchProblem.Text);
            }
            else
            {
                _loggerWithColor.LogWarning(packageSearchProblem.Text);
            }
        }

        public void Finish()
        {
            // We don't need to write anything at the end of the rendering for a tabular format
        }

        public void Start()
        {
            // We don't need to write anything at the beginning of the rendering for a tabular format
        }

        /// <summary>
        /// Populates the given table with package metadata results.
        /// </summary>
        /// <param name="results">An enumerable of package search metadata to be processed and added to the table.</param>
        /// <param name="table">The table where the results will be added as rows.</param>
        /// <param name="verbosity">The verbosity level for the search results.</param>
        private static async void PopulateTableWithResultsAsync(IEnumerable<IPackageSearchMetadata> results, Table table, PackageSearchVerbosity verbosity)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            NumberFormatInfo nfi = (NumberFormatInfo)culture.NumberFormat.Clone();
            nfi.NumberDecimalDigits = 0;

            foreach (IPackageSearchMetadata result in results)
            {
                string packageId = result.Identity.Id;
                string version = result.Identity.Version.ToNormalizedString();
                string owners = result.Owners;
                string downloads = "N/A";

                if (result.DownloadCount != null)
                {
                    downloads = string.Format(nfi, "{0:N}", result.DownloadCount);
                }

                if (verbosity == PackageSearchVerbosity.Minimal)
                {
                    table.AddRow(packageId, version);
                }
                else if (verbosity == PackageSearchVerbosity.Detailed)
                {
                    PackageDeprecationMetadata packageDeprecationMetadata = await result.GetDeprecationMetadataAsync();
                    string vulnerable = "N/A";
                    string projectUri = "N/A";
                    string deprecation = "N/A";

                    if (result.Vulnerabilities?.Any() ?? false)
                    {
                        vulnerable = "True";
                    }

                    if (result.ProjectUrl is not null)
                    {
                        projectUri = result.ProjectUrl.ToString();
                    }

                    if (packageDeprecationMetadata is not null)
                    {
                        deprecation = packageDeprecationMetadata.Message;
                    }

                    table.AddRow(
                        packageId,
                        version,
                        owners,
                        downloads,
                        vulnerable,
                        deprecation,
                        projectUri,
                        result.Description);
                }
                else
                {
                    table.AddRow(packageId, version, owners, downloads);
                }
            }
        }

        public void Add(PackageSearchProblem packageSearchProblem)
        {
            if (packageSearchProblem.ProblemType == PackageSearchProblemType.Error)
            {
                _loggerWithColor.LogError(packageSearchProblem.Text);
            }
            else
            {
                _loggerWithColor.LogWarning(packageSearchProblem.Text);
            }
        }
    }
}
