// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;
using NuGet.CommandLine.XPlat.Commands.PackageSearch;

namespace NuGet.CommandLine.XPlat
{
    /// <summary>
    /// Represents the result of a package search for a specific source.
    /// </summary>
    internal class PackageSearchResult
    {
        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; }

        [JsonPropertyName("problems")]
        public List<PackageSearchProblem> Problems { get; set; }

        [JsonPropertyName("packages")]
        public List<ISearchResultPackage> Packages { get; set; }

        public PackageSearchResult(string source)
        {
            SourceName = source;
            Packages = new List<ISearchResultPackage>();
        }
    }
}
