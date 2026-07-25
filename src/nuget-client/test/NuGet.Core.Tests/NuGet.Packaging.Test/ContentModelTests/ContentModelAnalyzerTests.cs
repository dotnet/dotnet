// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.RuntimeModel;
using Xunit;

namespace NuGet.Packaging.Test.ContentModelTests
{
    public class ContentModelAnalyzerTests
    {
        private static ManagedCodeConventions CreateConventions()
        {
            return new ManagedCodeConventions(
                new RuntimeGraph(
                    new List<CompatibilityProfile>() { new CompatibilityProfile("net46.app") }));
        }

        private static List<string> FindAnalyzers(params string[] files)
        {
            var conventions = CreateConventions();
            var collection = new ContentItemCollection();
            collection.Load(files);

            return collection
                .FindItems(conventions.Patterns.AnalyzerAssemblies)
                .Select(item => item.Path)
                .OrderBy(path => path, System.StringComparer.Ordinal)
                .ToList();
        }

        [Fact]
        public void AnalyzerAssemblies_MatchesAssembliesAtAnyDepth()
        {
            var analyzers = FindAnalyzers(
                "analyzers/Root.dll",
                "analyzers/dotnet/NeutralAnalyzer.dll",
                "analyzers/dotnet/cs/CSharpAnalyzer.dll",
                "analyzers/dotnet/roslyn4.0/cs/VersionedAnalyzer.dll",
                "analyzers/a/b/c/d/e/f/g/VeryDeepAnalyzer.dll");

            Assert.Equal(
                new[]
                {
                    "analyzers/Root.dll",
                    "analyzers/a/b/c/d/e/f/g/VeryDeepAnalyzer.dll",
                    "analyzers/dotnet/NeutralAnalyzer.dll",
                    "analyzers/dotnet/cs/CSharpAnalyzer.dll",
                    "analyzers/dotnet/roslyn4.0/cs/VersionedAnalyzer.dll",
                },
                analyzers);
        }

        [Fact]
        public void AnalyzerAssemblies_ExcludesSatelliteResourceAssemblies()
        {
            var analyzers = FindAnalyzers(
                "analyzers/dotnet/cs/CSharpAnalyzer.dll",
                "analyzers/dotnet/cs/CSharpAnalyzer.resources.dll");

            Assert.Equal(new[] { "analyzers/dotnet/cs/CSharpAnalyzer.dll" }, analyzers);
        }

        [Fact]
        public void AnalyzerAssemblies_ExcludesNonDllFiles()
        {
            var analyzers = FindAnalyzers(
                "analyzers/dotnet/cs/Analyzer.dll",
                "analyzers/dotnet/cs/NotAnAnalyzer.exe",
                "analyzers/dotnet/cs/NotAnAnalyzer.winmd",
                "analyzers/dotnet/cs/readme.txt");

            Assert.Equal(new[] { "analyzers/dotnet/cs/Analyzer.dll" }, analyzers);
        }

        [Fact]
        public void AnalyzerAssemblies_ExcludesAssembliesOutsideAnalyzersFolder()
        {
            var analyzers = FindAnalyzers(
                "analyzers/dotnet/cs/Analyzer.dll",
                "lib/netstandard2.0/Library.dll",
                "ref/netstandard2.0/Library.dll",
                "notanalyzers/Foo.dll");

            Assert.Equal(new[] { "analyzers/dotnet/cs/Analyzer.dll" }, analyzers);
        }

        [Fact]
        public void AnalyzerAssemblies_MatchesAnalyzersFolderCaseInsensitively()
        {
            // The content model matches literal path segments case-insensitively, so an 'Analyzers/' folder
            // (capital A) is detected. This differs from the previous hand-rolled detection, which matched the
            // 'analyzers/' prefix with StringComparison.Ordinal (case-sensitive).
            var analyzers = FindAnalyzers(
                "analyzers/dotnet/cs/Lower.dll",
                "Analyzers/dotnet/cs/Upper.dll");

            Assert.Equal(
                new[]
                {
                    "Analyzers/dotnet/cs/Upper.dll",
                    "analyzers/dotnet/cs/Lower.dll",
                },
                analyzers);
        }
    }
}
