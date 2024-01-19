// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.CommandLine.XPlat.Commands.PackageSearch;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageSearchResultJsonRenderer : IPackageSearchResultRenderer
    {
        private ILoggerWithColor _logger;
        private PackageSearchVerbosity _verbosity;
        private PackageSearchMainOutput _packageSearchMainOutput;

        public PackageSearchResultJsonRenderer(ILoggerWithColor loggerWithColor, PackageSearchVerbosity verbosity)
        {
            _logger = loggerWithColor;
            _verbosity = verbosity;
        }
        public async void Add(PackageSource source, IEnumerable<IPackageSearchMetadata> completedSearch)
        {
            PackageSearchResult packageSearchResult = new PackageSearchResult(source.Name);

            foreach (IPackageSearchMetadata metadata in completedSearch)
            {
                if (_verbosity == PackageSearchVerbosity.Minimal)
                {
                    packageSearchResult.Packages.Add(new MinimalVerbosityPackage(metadata));
                }
                else if (_verbosity == PackageSearchVerbosity.Detailed)
                {
                    PackageDeprecationMetadata packageDeprecationMetadata = await metadata.GetDeprecationMetadataAsync();
                    packageSearchResult.Packages.Add(new DetailedVerbosityPackage(metadata, packageDeprecationMetadata?.Message));
                }
                else
                {
                    packageSearchResult.Packages.Add(new NormalVerbosityPackage(metadata));
                }
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
                Converters = { new JsonPolymorphicConverter<ISearchResultPackage>() }
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

    class JsonPolymorphicConverter<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T Read(ref Utf8JsonReader reader, Type typeToSerialize, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Type type = value.GetType();

            JsonSerializer.Serialize(writer, value, type, new JsonSerializerOptions());
        }
    }
}
