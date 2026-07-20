// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NuGet.Commands;
using Xunit.Abstractions;

namespace NuGet.Build.Tasks.Pack.Test
{
    /// <remarks>
    /// <see cref="GetPackOutputItemsTaskTests"/>
    /// </remarks>
    public class PackageFileNameTestCase : IXunitSerializable
    {
        // Fixed package-id sentinels shared by every case: "proj" is the id supplied via the project's
        // PackageId property; "nusp" is the id written into the .nuspec <metadata>. They differ so the
        // produced file names reveal which id source won.
        public const string IdProjProp = "proj";
        public const string IdNuspecMeta = "nusp";

        public static IEnumerable<object[]> TestCases
        {
            get
            {
                var cases = new PackageFileNameTestCase[]
                    {
                        //// without nuspec input
                        new() { Scenario = "NoNuspec_NormalizesShortVersion", OutputNupkgNames = ["proj.1.9.0.nupkg"], VersionProjProp = "1.9", UseNuspecFile = false },
                        new() { Scenario = "NoNuspec_TrimsTrailingRevisionZero", OutputNupkgNames = ["proj.2.0.0.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = false },
                        new() { Scenario = "NoNuspec_PreservesNonZeroRevision", OutputNupkgNames = ["proj.2.0.0.1.nupkg"], VersionProjProp = "2.0.0.1", UseNuspecFile = false },
                        new() { Scenario = "NoNuspec_PreservesFourPartPrereleaseVersion", OutputNupkgNames = ["proj.2.0.0.3-preview.nupkg"], VersionProjProp = "2.0.0.3-preview", UseNuspecFile = false },
                        new() { Scenario = "NoNuspec_StripsVersionWhenConfigured", OutputNupkgNames = ["proj.nupkg"], VersionProjProp = "1.9", UseNuspecFile = false, OutputFileNamesWithoutVersion = true },

                        // with nuspec input
                        new() { Scenario = "WithNuspec_UsesMetadataVersion", OutputNupkgNames = ["nusp.4.0.0.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "       ", VersionNuspecMeta = "4.0.0.0" },
                        new() { Scenario = "WithNuspec_UsesMetadataRevision", OutputNupkgNames = ["nusp.4.0.0.3.nupkg"], VersionProjProp = "2.0.0.3", UseNuspecFile = true, VersionNuspecProperties = "       ", VersionNuspecMeta = "4.0.0.3" },
                        new() { Scenario = "WithNuspec_UsesNuspecPropertiesVersion", OutputNupkgNames = ["nusp.3.0.0.4.nupkg"], VersionProjProp = "2.0.0.4", UseNuspecFile = true, VersionNuspecProperties = "3.0.0.4", VersionNuspecMeta = "4.0.0.4" },
                        new() { Scenario = "WithNuspec_UsesMetadataPrereleaseVersion", OutputNupkgNames = ["nusp.5.0.0-preview.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "       ", VersionNuspecMeta = "5.0.0.0-preview" },
                        new() { Scenario = "WithNuspec_UsesMetadataFourPartPrereleaseVersion", OutputNupkgNames = ["nusp.5.0.0.2-preview.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "       ", VersionNuspecMeta = "5.0.0.2-preview" },
                        new() { Scenario = "WithNuspec_UsesNuspecPropertiesPrereleaseVersion", OutputNupkgNames = ["nusp.6.0.0-beta.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "6-beta ", VersionNuspecMeta = "5.0.0.3-preview" },
                        new() { Scenario = "WithNuspec_StripsVersionWhenConfigured", OutputNupkgNames = ["nusp.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "       ", VersionNuspecMeta = "4.0.0.0", OutputFileNamesWithoutVersion = true },

                        // Pinned regression case: PackTask treats NuspecProperties=id=... as $id$ substitution only;
                        // a literal <id> in the nuspec wins. GetPackOutputItemsTask must agree with that.
                        new() { Scenario = "WithNuspec_IdInNuspecPropertiesDoesNotOverrideLiteralId", OutputNupkgNames = ["nusp.4.0.0.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, IdNuspecProperties = "shouldBeIgnored", VersionNuspecProperties = "       ", VersionNuspecMeta = "4.0.0.0" },

                        // has symbol
                        new() { Scenario = "NoNuspec_SnupkgUsesNormalizedVersion", OutputNupkgNames = ["proj.2.1.0.nupkg", "proj.2.1.0.snupkg"], VersionProjProp = "2.1.0.0", UseNuspecFile = false, IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.Snupkg },
                        new() { Scenario = "WithNuspec_SnupkgUsesNuspecPropertiesVersion", OutputNupkgNames = ["nusp.7.1.2.nupkg", "nusp.7.1.2.snupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "7.1.2", VersionNuspecMeta = "5.0.0.4-preview", IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.Snupkg },
                        new() { Scenario = "NoNuspec_SnupkgStripsVersionWhenConfigured", OutputNupkgNames = ["proj.nupkg", "proj.snupkg"], VersionProjProp = "2.1.0.0", UseNuspecFile = false, OutputFileNamesWithoutVersion = true, IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.Snupkg },
                        new() { Scenario = "WithNuspec_SnupkgStripsVersionWhenConfigured", OutputNupkgNames = ["nusp.nupkg", "nusp.snupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "7.1.2", VersionNuspecMeta = "5.0.0.4-preview", OutputFileNamesWithoutVersion = true, IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.Snupkg },

                        new() { Scenario = "NoNuspec_SymbolsNupkgIncludesPrimaryAndSymbolsPackages", OutputNupkgNames = ["proj.2.2.0.nupkg", "proj.2.2.0.symbols.nupkg"], VersionProjProp = "2.2.0.0", UseNuspecFile = false, IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.SymbolsNupkg },
                        new() { Scenario = "WithNuspec_SymbolsNupkgIncludesPrimaryAndSymbolsPackages", OutputNupkgNames = ["nusp.7.2.2.nupkg", "nusp.7.2.2.symbols.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "7.2.2", VersionNuspecMeta = "5.0.0.4-preview", IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.SymbolsNupkg },
                        new() { Scenario = "NoNuspec_SymbolsNupkgStripsVersionWhenConfigured", OutputNupkgNames = ["proj.nupkg", "proj.symbols.nupkg"], VersionProjProp = "2.2.0.0", UseNuspecFile = false, OutputFileNamesWithoutVersion = true, IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.SymbolsNupkg },
                        new() { Scenario = "WithNuspec_SymbolsNupkgStripsVersionWhenConfigured", OutputNupkgNames = ["nusp.nupkg", "nusp.symbols.nupkg"], VersionProjProp = "2.0.0.0", UseNuspecFile = true, VersionNuspecProperties = "7.2.2", VersionNuspecMeta = "5.0.0.4-preview", OutputFileNamesWithoutVersion = true, IncludeSymbols = true, SymbolPackageFormat = SymbolPackageFormat.SymbolsNupkg },
                    };


                return [.. cases.Select(c => new object[] { c })];
            }
        }

        public required string Scenario { get; set; }

        public string[] OutputNupkgNames { get; set; } = Array.Empty<string>();

        public string IdNuspecProperties { get; set; } = string.Empty;

        public string VersionProjProp { get; set; } = string.Empty;

        public string VersionNuspecProperties { get; set; } = string.Empty;

        public string VersionNuspecMeta { get; set; } = string.Empty;

        public bool UseNuspecFile { get; set; }

        public bool OutputFileNamesWithoutVersion { get; set; }

        public bool IncludeSymbols { get; set; }

        public SymbolPackageFormat SymbolPackageFormat { get; set; } = SymbolPackageFormat.Snupkg;

        public override string ToString()
        {
            return Scenario;
        }

        void IXunitSerializable.Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Scenario), Scenario);
            info.AddValue(nameof(OutputNupkgNames), OutputNupkgNames);
            info.AddValue(nameof(IdNuspecProperties), IdNuspecProperties);
            info.AddValue(nameof(VersionProjProp), VersionProjProp);
            info.AddValue(nameof(VersionNuspecProperties), VersionNuspecProperties);
            info.AddValue(nameof(VersionNuspecMeta), VersionNuspecMeta);
            info.AddValue(nameof(UseNuspecFile), UseNuspecFile);
            info.AddValue(nameof(OutputFileNamesWithoutVersion), OutputFileNamesWithoutVersion);
            info.AddValue(nameof(IncludeSymbols), IncludeSymbols);
            info.AddValue(nameof(SymbolPackageFormat), SymbolPackageFormat);
        }
        void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
        {
            Scenario = (string)info.GetValue(nameof(Scenario), typeof(string));
            OutputNupkgNames = (string[])info.GetValue(nameof(OutputNupkgNames), typeof(string[]));
            IdNuspecProperties = (string)info.GetValue(nameof(IdNuspecProperties), typeof(string));
            VersionProjProp = (string)info.GetValue(nameof(VersionProjProp), typeof(string));
            VersionNuspecProperties = (string)info.GetValue(nameof(VersionNuspecProperties), typeof(string));
            VersionNuspecMeta = (string)info.GetValue(nameof(VersionNuspecMeta), typeof(string));
            UseNuspecFile = (bool)info.GetValue(nameof(UseNuspecFile), typeof(bool));
            OutputFileNamesWithoutVersion = (bool)info.GetValue(nameof(OutputFileNamesWithoutVersion), typeof(bool));
            IncludeSymbols = (bool)info.GetValue(nameof(IncludeSymbols), typeof(bool));
            SymbolPackageFormat = (SymbolPackageFormat)info.GetValue(nameof(SymbolPackageFormat), typeof(SymbolPackageFormat));
        }
    }

    internal static class PackageFileNameTestsCommon
    {
        public const string FILENAME_PROJECT_FILE = "test.csproj";
        public const string FILENAME_NUSPEC_FILE = "test.nuspec";

        public static void CreateNuspecFile(PackageFileNameTestCase testCase, string testDirectory)
        {
            if (!testCase.UseNuspecFile)
            {
                return;
            }

            var nuspecPath = Path.Combine(testDirectory, FILENAME_NUSPEC_FILE);
            var nuspecContent = $"""
<?xml version="1.0" encoding="utf-8"?>
    <package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <id>{PackageFileNameTestCase.IdNuspecMeta}</id>
        <version>{testCase.VersionNuspecMeta?.Trim()}</version>
        <authors>Unit Test</authors>
        <description>Sample Description</description>
        <language>en-US</language>
    </metadata>
</package>
""";

            File.WriteAllText(nuspecPath, nuspecContent, new UTF8Encoding(true));
        }

        public static string GetSymbolPackageFormatText(SymbolPackageFormat symbolPackageFormat)
        {
            switch (symbolPackageFormat)
            {
                case SymbolPackageFormat.Snupkg: return "snupkg";
                case SymbolPackageFormat.SymbolsNupkg: return "symbols.nupkg";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
