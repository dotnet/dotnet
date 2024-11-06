// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Compiler.CSharp;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    public partial class RazorSourceGenerator
    {
        private (RazorSourceGenerationOptions?, Diagnostic?) ComputeRazorSourceGeneratorOptions((((AnalyzerConfigOptionsProvider, ParseOptions), ImmutableArray<MetadataReference>), bool) pair, CancellationToken ct)
        {
            var (((options, parseOptions), references), isSuppressed) = pair;
            var globalOptions = options.GlobalOptions;

            if (isSuppressed)
            {
                return default;
            }

            Log.ComputeRazorSourceGeneratorOptions();

            globalOptions.TryGetValue("build_property.RazorConfiguration", out var configurationName);
            globalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
            globalOptions.TryGetValue("build_property.SupportLocalizedComponentNames", out var supportLocalizedComponentNames);
            globalOptions.TryGetValue("build_property.GenerateRazorMetadataSourceChecksumAttributes", out var generateMetadataSourceChecksumAttributes);

            Diagnostic? diagnostic = null;
            if (!globalOptions.TryGetValue("build_property.RazorLangVersion", out var razorLanguageVersionString) ||
                !RazorLanguageVersion.TryParse(razorLanguageVersionString, out var razorLanguageVersion))
            {
                diagnostic = Diagnostic.Create(
                    RazorDiagnostics.InvalidRazorLangVersionDescriptor,
                    Location.None,
                    razorLanguageVersionString);
                razorLanguageVersion = RazorLanguageVersion.Latest;
            }

            var minimalReferences = references
                .Where(r => r.Display is { } display && display.EndsWith("Microsoft.AspNetCore.Components.dll", StringComparison.Ordinal))
                .ToImmutableArray();

            var isComponentParameterSupported = minimalReferences.Length == 0
                ? false
                : CSharpCompilation.Create("components", references: minimalReferences).HasAddComponentParameter();

            var razorConfiguration = new RazorConfiguration(razorLanguageVersion, configurationName ?? "default", Extensions: [], UseConsolidatedMvcViews: true, SuppressAddComponentParameter: !isComponentParameterSupported);

            // We use the new tokenizer only when requested for now.
            var useRoslynTokenizer = parseOptions.UseRoslynTokenizer();

            var razorSourceGenerationOptions = new RazorSourceGenerationOptions()
            {
                Configuration = razorConfiguration,
                GenerateMetadataSourceChecksumAttributes = generateMetadataSourceChecksumAttributes == "true",
                RootNamespace = rootNamespace ?? "ASP",
                SupportLocalizedComponentNames = supportLocalizedComponentNames == "true",
                CSharpParseOptions = (CSharpParseOptions)parseOptions,
                TestSuppressUniqueIds = _testSuppressUniqueIds,
                UseRoslynTokenizer = useRoslynTokenizer,
            };

            return (razorSourceGenerationOptions, diagnostic);
        }

        private static (SourceGeneratorProjectItem?, Diagnostic?) ComputeProjectItems((AdditionalText, AnalyzerConfigOptionsProvider) pair, CancellationToken ct)
        {
            var (additionalText, globalOptions) = pair;
            var options = globalOptions.GetOptions(additionalText);

            if (!options.TryGetValue("build_metadata.AdditionalFiles.TargetPath", out var encodedRelativePath) ||
                string.IsNullOrWhiteSpace(encodedRelativePath))
            {
                var diagnostic = Diagnostic.Create(
                    RazorDiagnostics.TargetPathNotProvided,
                    Location.None,
                    additionalText.Path);
                return (null, diagnostic);
            }

            options.TryGetValue("build_metadata.AdditionalFiles.CssScope", out var cssScope);
            var relativePath = Encoding.UTF8.GetString(Convert.FromBase64String(encodedRelativePath));

            var projectItem = new SourceGeneratorProjectItem(
                basePath: "/",
                filePath: '/' + relativePath
                    .Replace(Path.DirectorySeparatorChar, '/')
                    .Replace("//", "/"),
                relativePhysicalPath: relativePath,
                fileKind: FileKinds.GetFileKindFromFilePath(additionalText.Path),
                additionalText: additionalText,
                cssScope: cssScope);
            return (projectItem, null);
        }
    }
}
