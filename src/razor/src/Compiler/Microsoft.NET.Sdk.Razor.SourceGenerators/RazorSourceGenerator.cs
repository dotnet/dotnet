// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.RazorCompiler;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    [Generator]
    public partial class RazorSourceGenerator : IIncrementalGenerator
    {
        private static RazorSourceGeneratorEventSource Log => RazorSourceGeneratorEventSource.Log;

        // Testing usage only.
        private readonly string? _testSuppressUniqueIds;

        public RazorSourceGenerator()
        {
        }

        internal RazorSourceGenerator(string testUniqueIds)
        {
            _testSuppressUniqueIds = testUniqueIds;
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var analyzerConfigOptions = context.AnalyzerConfigOptionsProvider;
            var parseOptions = context.ParseOptionsProvider;
            var compilation = context.CompilationProvider;

            // determine if we should suppress this run and filter out all the additional files and references if so
            var isGeneratorSuppressed = analyzerConfigOptions.CheckGlobalFlagSet("SuppressRazorSourceGenerator");
            var additionalTexts = context.AdditionalTextsProvider.EmptyWhen(isGeneratorSuppressed, true);
            var metadataRefs = context.MetadataReferencesProvider.EmptyWhen(isGeneratorSuppressed, true);

            var razorSourceGeneratorOptions = analyzerConfigOptions
                .Combine(parseOptions)
                .Combine(isGeneratorSuppressed)
                .Select(ComputeRazorSourceGeneratorOptions)
                .ReportDiagnostics(context);

            var sourceItems = additionalTexts
                .Where(static (file) => file.Path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase) || file.Path.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
                .Combine(analyzerConfigOptions)
                .Select(ComputeProjectItems)
                .ReportDiagnostics(context);

            var hasRazorFiles = sourceItems.Collect()
                .Select(static (sourceItems, _) => sourceItems.Any());

            var importFiles = sourceItems.Where(static file =>
            {
                var path = file.FilePath;
                if (path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
                {
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    return string.Equals(fileName, "_Imports", StringComparison.OrdinalIgnoreCase);
                }
                else if (path.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
                {
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    return string.Equals(fileName, "_ViewImports", StringComparison.OrdinalIgnoreCase);
                }

                return false;
            });

            var componentFiles = sourceItems.Where(static file => file.FilePath.EndsWith(".razor", StringComparison.OrdinalIgnoreCase));

            var generatedDeclarationCode = componentFiles
                .Combine(importFiles.Collect())
                .Combine(razorSourceGeneratorOptions)
                .WithLambdaComparer((old, @new) => old.Right.Equals(@new.Right) && old.Left.Left.Equals(@new.Left.Left) && old.Left.Right.SequenceEqual(@new.Left.Right))
                .Select(static (pair, _) =>
                {
                    var ((sourceItem, importFiles), razorSourceGeneratorOptions) = pair;
                    RazorSourceGeneratorEventSource.Log.GenerateDeclarationCodeStart(sourceItem.FilePath);

                    var projectEngine = GetDeclarationProjectEngine(sourceItem, importFiles, razorSourceGeneratorOptions);

                    var codeGen = projectEngine.Process(sourceItem);

                    var result = codeGen.GetCSharpDocument().GeneratedCode;

                    RazorSourceGeneratorEventSource.Log.GenerateDeclarationCodeStop(sourceItem.FilePath);

                    return result;
                });

            var generatedDeclarationSyntaxTrees = generatedDeclarationCode
                .Combine(parseOptions)
                .Select(static (pair, ct) =>
                {
                    var (generatedDeclarationCode, parseOptions) = pair;
                    return CSharpSyntaxTree.ParseText(generatedDeclarationCode, (CSharpParseOptions)parseOptions, cancellationToken: ct);
                });

            var declCompilation = generatedDeclarationSyntaxTrees
                .Collect()
                .Combine(compilation)
                .Select(static (pair, _) =>
                {
                    return pair.Right.AddSyntaxTrees(pair.Left);
                });

            var tagHelpersFromCompilation = declCompilation
                .Combine(razorSourceGeneratorOptions)
                .Combine(isGeneratorSuppressed)
                .Select(static (pair, _) =>
                {

                    var ((compilation, razorSourceGeneratorOptions), isGeneratorSuppressed) = pair;
                    var results = new List<TagHelperDescriptor>();

                    if (isGeneratorSuppressed)
                    {
                        return results;
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromCompilationStart();
                    var tagHelperFeature = GetStaticTagHelperFeature(compilation);

                    tagHelperFeature.CollectDescriptors(compilation.Assembly, results);

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromCompilationStop();

                    return results;
                })
                .WithLambdaComparer(static (a, b) => a.SequenceEqual(b));

            var tagHelpersFromReferences = compilation
                .Combine(razorSourceGeneratorOptions)
                .Combine(hasRazorFiles)
                .WithLambdaComparer(static (a, b) =>
                {
                    var ((compilationA, razorSourceGeneratorOptionsA), hasRazorFilesA) = a;
                    var ((compilationB, razorSourceGeneratorOptionsB), hasRazorFilesB) = b;

                    if (!compilationA.References.SequenceEqual(compilationB.References))
                    {
                        return false;
                    }

                    if (razorSourceGeneratorOptionsA != razorSourceGeneratorOptionsB)
                    {
                        return false;
                    }

                    return hasRazorFilesA == hasRazorFilesB;
                })
                .Select(static (pair, _) =>
                {

                    var ((compilation, razorSourceGeneratorOptions), hasRazorFiles) = pair;
                    if (!hasRazorFiles)
                    {
                        // If there's no razor code in this app, don't do anything.
                        return null;
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStart();
                    var tagHelperFeature = GetStaticTagHelperFeature(compilation);

                    // Typically a project with Razor files will have many tag helpers in references.
                    // So, we start with a larger capacity to avoid extra array copies.
                    var results = new List<TagHelperDescriptor>(capacity: 128);

                    foreach (var reference in compilation.References)
                    {
                        if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                        {
                            tagHelperFeature.CollectDescriptors(assembly, results);
                        }
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStop();

                    return results;
                });

            var allTagHelpers = tagHelpersFromCompilation
                .Combine(tagHelpersFromReferences)
                .Select(static (pair, _) =>
                {
                    return AllTagHelpers.Create(tagHelpersFromCompilation: pair.Left, tagHelpersFromReferences: pair.Right);
                });

            var withOptions = sourceItems
                .Combine(importFiles.Collect())
                .WithLambdaComparer((old, @new) => old.Left.Equals(@new.Left) && old.Right.SequenceEqual(@new.Right))
                .Combine(razorSourceGeneratorOptions);

            var razorHostOutputsEnabled = analyzerConfigOptions.CheckGlobalFlagSet("EnableRazorHostOutputs");
            var withOptionsDesignTime = withOptions.EmptyWhen(razorHostOutputsEnabled, false);

            var isAddComponentParameterAvailable = metadataRefs
                .Where(r => r.Display is { } display && display.EndsWith("Microsoft.AspNetCore.Components.dll", StringComparison.Ordinal))
                .Collect()
                .Select((refs, _) =>
                {
                    var compilation = CSharpCompilation.Create("components", references: refs);
                    return compilation.GetTypesByMetadataName("Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder")
                        .Any(static (t, compilation) => t.DeclaredAccessibility == Accessibility.Public &&
                            t.GetMembers("AddComponentParameter").Any(static m => m.DeclaredAccessibility == Accessibility.Public), compilation);
                });

            IncrementalValuesProvider<(string, SourceGeneratorRazorCodeDocument)> processed(bool designTime)
            {
                return (designTime ? withOptionsDesignTime : withOptions)
                    .Combine(isAddComponentParameterAvailable)
                    .Select((pair, _) =>
                    {
                        var (((sourceItem, imports), razorSourceGeneratorOptions), isAddComponentParameterAvailable) = pair;

                        RazorSourceGeneratorEventSource.Log.ParseRazorDocumentStart(sourceItem.RelativePhysicalPath);

                        var projectEngine = GetGenerationProjectEngine(sourceItem, imports, razorSourceGeneratorOptions, isAddComponentParameterAvailable);

                        var document = projectEngine.ProcessInitialParse(sourceItem, designTime);

                        RazorSourceGeneratorEventSource.Log.ParseRazorDocumentStop(sourceItem.RelativePhysicalPath);
                        return (projectEngine, sourceItem.RelativePhysicalPath, document);
                    })

                    // Add the tag helpers in, but ignore if they've changed or not, only reprocessing the actual document changed
                    .Combine(allTagHelpers)
                    .WithLambdaComparer((old, @new) => old.Left.Equals(@new.Left))
                    .Select(static (pair, _) =>
                    {
                        var ((projectEngine, filePath, codeDocument), allTagHelpers) = pair;
                        RazorSourceGeneratorEventSource.Log.RewriteTagHelpersStart(filePath);

                        codeDocument = projectEngine.ProcessTagHelpers(codeDocument, allTagHelpers, checkForIdempotency: false);

                        RazorSourceGeneratorEventSource.Log.RewriteTagHelpersStop(filePath);
                        return (projectEngine, filePath, codeDocument);
                    })

                    // next we do a second parse, along with the helpers, but check for idempotency. If the tag helpers used on the previous parse match, the compiler can skip re-writing them
                    .Combine(allTagHelpers)
                    .Select(static (pair, _) =>
                    {
                        var ((projectEngine, filePath, document), allTagHelpers) = pair;
                        RazorSourceGeneratorEventSource.Log.CheckAndRewriteTagHelpersStart(filePath);

                        document = projectEngine.ProcessTagHelpers(document, allTagHelpers, checkForIdempotency: true);

                        RazorSourceGeneratorEventSource.Log.CheckAndRewriteTagHelpersStop(filePath);
                        return (projectEngine, filePath, document);
                    })
                    .Select((pair, _) =>
                    {
                        var (projectEngine, filePath, document) = pair;

                        var kind = designTime ? "DesignTime" : "Runtime";
                        RazorSourceGeneratorEventSource.Log.RazorCodeGenerateStart(filePath, kind);
                        document = projectEngine.ProcessRemaining(document);

                        RazorSourceGeneratorEventSource.Log.RazorCodeGenerateStop(filePath, kind);
                        return (filePath, document);
                    });
            }

            var csharpDocuments = processed(designTime: false)
                .Select(static (pair, _) =>
                {
                    var (filePath, document) = pair;
                    return (filePath, csharpDocument: document.CodeDocument.GetCSharpDocument());
                })
                .WithLambdaComparer(static (a, b) =>
                {
                    if (a.csharpDocument.Diagnostics.Count > 0 || b.csharpDocument.Diagnostics.Count > 0)
                    {
                        // if there are any diagnostics, treat the documents as unequal and force RegisterSourceOutput to be called uncached.
                        return false;
                    }

                    return string.Equals(a.csharpDocument.GeneratedCode, b.csharpDocument.GeneratedCode, StringComparison.Ordinal);
                });

            context.RegisterImplementationSourceOutput(csharpDocuments, static (context, pair) =>
            {
                var (filePath, csharpDocument) = pair;

                // Add a generated suffix so tools, such as coverlet, consider the file to be generated
                var hintName = GetIdentifierFromPath(filePath) + ".g.cs";

                RazorSourceGeneratorEventSource.Log.AddSyntaxTrees(hintName);
                for (var i = 0; i < csharpDocument.Diagnostics.Count; i++)
                {
                    var razorDiagnostic = csharpDocument.Diagnostics[i];
                    var csharpDiagnostic = razorDiagnostic.AsDiagnostic();
                    context.ReportDiagnostic(csharpDiagnostic);
                }

                context.AddSource(hintName, csharpDocument.GeneratedCode);
            });

            context.RegisterHostOutput(processed(designTime: true), static (context, pair, _) =>
            {
                var (filePath, document) = pair;
                var hintName = GetIdentifierFromPath(filePath);
                context.AddOutput(hintName + ".rsg.cs", document.CodeDocument.GetCSharpDocument().GeneratedCode);
                context.AddOutput(hintName + ".rsg.html", document.CodeDocument.GetHtmlDocument().GeneratedCode);
            });
        }
    }
}