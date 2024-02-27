// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageSearchResultJsonRenderer : IPackageSearchResultRenderer
    {
        private ILoggerWithColor _logger;
        private PackageSearchVerbosity _verbosity;
        private bool _exactMatch;
        private PackageSearchMainOutput _packageSearchMainOutput;

        public PackageSearchResultJsonRenderer(ILoggerWithColor loggerWithColor, PackageSearchVerbosity verbosity, bool exactMatch)
        {
            _logger = loggerWithColor;
            _verbosity = verbosity;
            _exactMatch = exactMatch;
        }
        public void Add(PackageSource source, IEnumerable<IPackageSearchMetadata> completedSearch)
        {
            PackageSearchResult packageSearchResult = new PackageSearchResult(source.Name);

            foreach (IPackageSearchMetadata metadata in completedSearch)
            {
                packageSearchResult.Packages.Add(metadata);
            }

            _packageSearchMainOutput.SearchResult.Add(packageSearchResult);
        }

        public void Add(PackageSource source, PackageSearchProblem packageSearchProblem)
        {
            PackageSearchResult packageSearchResult = new PackageSearchResult(source.Name)
            {
                Problems = new List<PackageSearchProblem>() { packageSearchProblem }
            };
            _packageSearchMainOutput.SearchResult.Add(packageSearchResult);
        }

        public void Finish()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new SearchResultPackagesConverter(_verbosity, _exactMatch) }
            };
            var json = JsonSerializer.Serialize(_packageSearchMainOutput, options);
            _logger.LogMinimal(json);
        }

        public void RenderProblem(PackageSearchProblem packageSearchProblem)
        {
            _packageSearchMainOutput.Problems.Add(packageSearchProblem);
        }

        public void Start()
        {
            _packageSearchMainOutput = new PackageSearchMainOutput();
        }
    }
}
