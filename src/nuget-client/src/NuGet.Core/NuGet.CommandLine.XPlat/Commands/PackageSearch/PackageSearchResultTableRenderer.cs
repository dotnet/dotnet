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

        private readonly string[] _minimalVerbosityTableHeader = { "Package ID", "Latest Version" };
        private readonly string[] _minimalVerbosityTableHeaderForExactMatch = { "Package ID", "Version" };
        private readonly int[] _minimalColumnsToHighlight = { 0 };

        private readonly string[] _normalVerbosityTableHeader = { "Package ID", "Latest Version", "Owners", "Total Downloads" };
        private readonly string[] _normalVerbosityTableHeaderForExactMatch = { "Package ID", "Version", "Owners", "Total Downloads" };
        private readonly int[] _normalColumnsToHighlight = { 0, 2 };

        private readonly string[] _detailedVerbosityTableHeader = { "Package ID", "Latest Version", "Owners", "Total Downloads", "Vulnerable", "Deprecation", "Project URL", "Description" };
        private readonly string[] _detailedVerbosityTableHeaderForExactMatch = { "Package ID", "Version", "Owners", "Total Downloads", "Vulnerable", "Deprecation", "Project URL", "Description" };
        private readonly int[] _detailedColumnsToHighlight = { 0, 2, 6, 7 };

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

            Table table;

            if (_verbosity == PackageSearchVerbosity.Minimal)
            {
                if (_exactMatch)
                {
                    table = new Table(_minimalColumnsToHighlight, _minimalVerbosityTableHeaderForExactMatch);
                }
                else
                {
                    table = new Table(_minimalColumnsToHighlight, _minimalVerbosityTableHeader);
                }
            }
            else if (_verbosity == PackageSearchVerbosity.Detailed)
            {
                if (_exactMatch)
                {
                    table = new Table(_detailedColumnsToHighlight, _detailedVerbosityTableHeaderForExactMatch);
                }
                else
                {
                    table = new Table(_detailedColumnsToHighlight, _detailedVerbosityTableHeader);
                }
            }
            else
            {
                if (_exactMatch)
                {
                    table = new Table(_normalColumnsToHighlight, _normalVerbosityTableHeaderForExactMatch);
                }
                else
                {
                    table = new Table(_normalColumnsToHighlight, _normalVerbosityTableHeader);
                }
            }

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

                    if (result.Vulnerabilities != null && result.Vulnerabilities.Any())
                    {
                        vulnerable = "True";
                    }

                    if (result.ProjectUrl != null)
                    {
                        projectUri = result.ProjectUrl.ToString();
                    }

                    if (packageDeprecationMetadata != null)
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

        public void RenderProblem(PackageSearchProblem packageSearchProblem)
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
