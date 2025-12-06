// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
#pragma warning disable RS1041 // This compiler extension should not be implemented in an assembly with target framework '.NET 8.0'. References to other target frameworks will cause the compiler to behave unpredictably.
    [Generator]
#pragma warning restore RS1041 // This compiler extension should not be implemented in an assembly with target framework '.NET 8.0'. References to other target frameworks will cause the compiler to behave unpredictably.
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

            // We might get initialized before razor tooling has a chance to set this, so check it any time something changes. It's extremely cheap and we'll stop propagating directly below if it didn't changed.
            var useRazorCohostServer = context.AdditionalTextsProvider.Collect().Combine(context.CompilationProvider).Select((_, _) => RazorCohostingOptions.UseRazorCohostServer);

            // determine if we should suppress this run and filter out all the additional files and references if so
            var isGeneratorSuppressed = analyzerConfigOptions.CheckGlobalFlagSet("SuppressRazorSourceGenerator").Combine(useRazorCohostServer).Select((suppress, _) => !suppress.Right && suppress.Left);
            var additionalTexts = context.AdditionalTextsProvider.EmptyOrCachedWhen(isGeneratorSuppressed, true);
            var metadataRefs = context.MetadataReferencesProvider.EmptyOrCachedWhen(isGeneratorSuppressed, true);

            var razorSourceGeneratorOptions = analyzerConfigOptions
                .Combine(parseOptions)
                .Combine(metadataRefs.Collect())
                .SuppressIfNeeded(isGeneratorSuppressed)
                .Select(ComputeRazorSourceGeneratorOptions)
                .ReportDiagnostics(context);

            var sourceItems = additionalTexts
                .Where(static (file) => FileUtilities.IsAnyRazorFilePath(file.Path, StringComparison.OrdinalIgnoreCase))
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

            var componentFiles = sourceItems.Where(static file => FileUtilities.IsRazorComponentFilePath(file.FilePath, StringComparison.OrdinalIgnoreCase));

            var generatedDeclarationText = componentFiles
                .Combine(importFiles.Collect())
                .Combine(razorSourceGeneratorOptions)
                .WithLambdaComparer((old, @new) => old.Right.Equals(@new.Right) && old.Left.Left.Equals(@new.Left.Left) && old.Left.Right.SequenceEqual(@new.Left.Right))
                .Select(static (pair, cancellationToken) =>
                {
                    var ((sourceItem, importFiles), razorSourceGeneratorOptions) = pair;
                    RazorSourceGeneratorEventSource.Log.GenerateDeclarationCodeStart(sourceItem.FilePath);

                    var projectEngine = GetDeclarationProjectEngine(sourceItem, importFiles, razorSourceGeneratorOptions);

                    var codeGen = projectEngine.Process(sourceItem, cancellationToken);

                    var result = new SourceGeneratorText(codeGen.GetRequiredCSharpDocument().Text);

                    RazorSourceGeneratorEventSource.Log.GenerateDeclarationCodeStop(sourceItem.FilePath);

                    return result;
                });

            var generatedDeclarationSyntaxTrees = generatedDeclarationText
                .Combine(parseOptions)
                .Select(static (pair, ct) =>
                {
                    var (generatedDeclarationText, parseOptions) = pair;

                    return CSharpSyntaxTree.ParseText(generatedDeclarationText.Text, (CSharpParseOptions)parseOptions, cancellationToken: ct);
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
                .SuppressIfNeeded(isGeneratorSuppressed)
                .Select(static (pair, cancellationToken) =>
                {
                    var ((compilation, razorSourceGeneratorOptions), isGeneratorSuppressed) = pair;
                    if (isGeneratorSuppressed)
                    {
                        return [];
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromCompilationStart();
                    var tagHelperFeature = GetStaticTagHelperFeature(compilation);

                    var collection = tagHelperFeature.GetTagHelpers(compilation.Assembly, cancellationToken);

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromCompilationStop();

                    return collection;
                })
                .WithLambdaComparer(static (a, b) => a!.SequenceEqual(b!));

            var tagHelpersFromReferences = compilation
                .Combine(razorSourceGeneratorOptions)
                .Combine(hasRazorFiles)
                .WithLambdaComparer(static (a, b) =>
                {
                    var ((compilationA, razorSourceGeneratorOptionsA), hasRazorFilesA) = a;
                    var ((compilationB, razorSourceGeneratorOptionsB), hasRazorFilesB) = b;

                    // When using the generator cache in the compiler it's possible to encounter metadata references that are different instances
                    // but ultimately represent the same underlying assembly. We compare the module version ids to determine if the references are the same
                    if (!compilationA.References.SequenceEqual(compilationB.References, new LambdaComparer<MetadataReference>((old, @new) =>
                    {
                        if (ReferenceEquals(old, @new))
                        {
                            return true;
                        }

                        if (old is null || @new is null)
                        {
                            return false;
                        }

                        var oldSymbol = compilationA.GetAssemblyOrModuleSymbol(old);
                        var newSymbol = compilationB.GetAssemblyOrModuleSymbol(@new);

                        if (SymbolEqualityComparer.Default.Equals(oldSymbol, newSymbol))
                        {
                            return true;
                        }

                        if (oldSymbol is not IAssemblySymbol oldAssembly || newSymbol is not IAssemblySymbol newAssembly)
                        {
                            return false;
                        }

                        // Compare the MVIDs of the modules in each assembly. If they aren't present or don't match we don't consider them equal
                        var oldModules = oldAssembly.Modules.ToArray();
                        var newModules = newAssembly.Modules.ToArray();
                        if (oldModules.Length != newModules.Length)
                        {
                            return false;
                        }

                        for (int i = 0; i < oldModules.Length; i++)
                        {
                            var oldMetadata = oldModules[i].GetMetadata();
                            var newMetadata = newModules[i].GetMetadata();

                            if (oldMetadata is null || newMetadata is null)
                            {
                                return false;
                            }

                            if (oldMetadata.GetModuleVersionId() != newMetadata.GetModuleVersionId())
                            {
                                return false;
                            }
                        }

                        // All module MVIDs matched.
                        return true;
                    })))
                    {
                        return false;
                    }

                    if (razorSourceGeneratorOptionsA != razorSourceGeneratorOptionsB)
                    {
                        return false;
                    }

                    return hasRazorFilesA == hasRazorFilesB;
                })
                .Select(static (pair, cancellationToken) =>
                {
                    var ((compilation, razorSourceGeneratorOptions), hasRazorFiles) = pair;
                    if (!hasRazorFiles)
                    {
                        // If there's no razor code in this app, don't do anything.
                        return [];
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStart();
                    var tagHelperFeature = GetStaticTagHelperFeature(compilation);

                    using var collections = new MemoryBuilder<TagHelperCollection>(initialCapacity: 512, clearArray: true);

                    foreach (var reference in compilation.References)
                    {
                        if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                        {
                            var collection = tagHelperFeature.GetTagHelpers(assembly, cancellationToken);
                            if (!collection.IsEmpty)
                            {
                                collections.Append(collection);
                            }
                        }
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStop();

                    return TagHelperCollection.Merge(collections.AsMemory().Span);
                });

            var allTagHelpers = tagHelpersFromCompilation
                .Combine(tagHelpersFromReferences)
                .Select(static (pair, _) =>
                {
                    return TagHelperCollection.Merge(pair.Left, pair.Right);
                });

            var withOptions = sourceItems
                .Combine(importFiles.Collect())
                .WithLambdaComparer((old, @new) => old.Left.Equals(@new.Left) && old.Right.SequenceEqual(@new.Right))
                .Combine(razorSourceGeneratorOptions);

            // Currently unused. See https://github.com/dotnet/roslyn/issues/71024.
            var razorHostOutputsEnabled = analyzerConfigOptions.CheckGlobalFlagSet("EnableRazorHostOutputs");
            var withOptionsDesignTime = withOptions.EmptyOrCachedWhen(razorHostOutputsEnabled, false);

            IncrementalValuesProvider<(string, SourceGeneratorRazorCodeDocument)> processed(bool designTime)
            {
                return (designTime ? withOptionsDesignTime : withOptions)
                    .Select((pair, cancellationToken) =>
                    {
                        var ((sourceItem, imports), razorSourceGeneratorOptions) = pair;

                        RazorSourceGeneratorEventSource.Log.ParseRazorDocumentStart(sourceItem.RelativePhysicalPath);

                        var projectEngine = GetGenerationProjectEngine(sourceItem, imports, razorSourceGeneratorOptions);

                        var document = projectEngine.ProcessInitialParse(sourceItem, designTime, cancellationToken);

                        RazorSourceGeneratorEventSource.Log.ParseRazorDocumentStop(sourceItem.RelativePhysicalPath);
                        return (projectEngine, sourceItem.RelativePhysicalPath, document);
                    })

                    // Add the tag helpers in, but ignore if they've changed or not, only reprocessing the actual document changed
                    .Combine(allTagHelpers)
                    .WithLambdaComparer((old, @new) => old.Left.Equals(@new.Left))
                    .Select(static (pair, cancellationToken) =>
                    {
                        var ((projectEngine, filePath, codeDocument), allTagHelpers) = pair;
                        RazorSourceGeneratorEventSource.Log.RewriteTagHelpersStart(filePath);

                        codeDocument = projectEngine.ProcessTagHelpers(codeDocument, allTagHelpers, checkForIdempotency: false, cancellationToken);

                        RazorSourceGeneratorEventSource.Log.RewriteTagHelpersStop(filePath);
                        return (projectEngine, filePath, codeDocument);
                    })

                    // next we do a second parse, along with the helpers, but check for idempotency. If the tag helpers used on the previous parse match, the compiler can skip re-writing them
                    .Combine(allTagHelpers)
                    .Select(static (pair, cancellationToken) =>
                    {
                        var ((projectEngine, filePath, document), allTagHelpers) = pair;
                        RazorSourceGeneratorEventSource.Log.CheckAndRewriteTagHelpersStart(filePath);

                        document = projectEngine.ProcessTagHelpers(document, allTagHelpers, checkForIdempotency: true, cancellationToken);

                        RazorSourceGeneratorEventSource.Log.CheckAndRewriteTagHelpersStop(filePath);
                        return (projectEngine, filePath, document);
                    })
                    .Select((pair, cancellationToken) =>
                    {
                        var (projectEngine, filePath, document) = pair;

                        var kind = designTime ? "DesignTime" : "Runtime";
                        RazorSourceGeneratorEventSource.Log.RazorCodeGenerateStart(filePath, kind);
                        document = projectEngine.ProcessRemaining(document, cancellationToken);

                        RazorSourceGeneratorEventSource.Log.RazorCodeGenerateStop(filePath, kind);
                        return (filePath, document);
                    });
            }

            var csharpDocuments = processed(designTime: false)
                .Select(static (pair, _) =>
                {
                    var (filePath, document) = pair;
                    return (
                        hintName: GetIdentifierFromPath(filePath),
                        codeDocument: document.CodeDocument,
                        csharpDocument: document.CodeDocument.GetRequiredCSharpDocument());
                })
                .WithLambdaComparer(static (a, b) =>
                {
                    if (a.csharpDocument.Diagnostics.Length > 0 || b.csharpDocument.Diagnostics.Length > 0)
                    {
                        // if there are any diagnostics, treat the documents as unequal and force RegisterSourceOutput to be called uncached.
                        return false;
                    }

                    return a.csharpDocument.Text.ContentEquals(b.csharpDocument.Text);
                })
                .WithTrackingName("CSharpDocuments");

            var csharpDocumentsWithSuppressionFlag = csharpDocuments
                // Explicitly combine with the suppression state. We *do* want this to run even if we're in the latched state
                .Combine(isGeneratorSuppressed)
                .WithTrackingName("DocumentsWithSuppression");

            context.RegisterImplementationSourceOutput(csharpDocumentsWithSuppressionFlag, static (context, pair) =>
            {
                var ((hintName, _, csharpDocument), isGeneratorSuppressed) = pair;

                // When the generator is suppressed, we may still have a lot of cached data for perf, but we don't want to actually add any of the files to the output
                if (!isGeneratorSuppressed)
                {
                    RazorSourceGeneratorEventSource.Log.AddSyntaxTrees(hintName);
                    foreach (var razorDiagnostic in csharpDocument.Diagnostics)
                    {
                        var csharpDiagnostic = razorDiagnostic.AsDiagnostic();
                        context.ReportDiagnostic(csharpDiagnostic);
                    }

                    context.AddSource(hintName, csharpDocument.Text);
                }
            });

            var hostOutputs = csharpDocuments
                .Collect()
                .Combine(allTagHelpers)
                .Combine(isGeneratorSuppressed)
                .WithTrackingName("HostOutputs");

#pragma warning disable RSEXPERIMENTAL004 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            context.RegisterHostOutput(hostOutputs, (context, pair) =>
#pragma warning restore RSEXPERIMENTAL004 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            {
                var ((documents, tagHelpers), isGeneratorSuppressed) = pair;

                if (!isGeneratorSuppressed)
                {
                    using var filePathToDocument = new PooledDictionaryBuilder<string, (string, RazorCodeDocument)>();
                    using var hintNameToFilePath = new PooledDictionaryBuilder<string, string>();

                    foreach (var (hintName, codeDocument, _) in documents)
                    {
                        filePathToDocument.Add(codeDocument.Source.FilePath!, (hintName, codeDocument));
                        hintNameToFilePath.Add(hintName, codeDocument.Source.FilePath!);
                    }

                    context.AddOutput(nameof(RazorGeneratorResult), new RazorGeneratorResult(tagHelpers, filePathToDocument.ToImmutable(), hintNameToFilePath.ToImmutable()));
                }
            });
        }
    }
}
