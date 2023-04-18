// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    [Generator]
    public partial class RazorSourceGenerator : IIncrementalGenerator
    {
        private static RazorSourceGeneratorEventSource Log => RazorSourceGeneratorEventSource.Log;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var analyzerConfigOptions = context.AnalyzerConfigOptionsProvider;
            var parseOptions = context.ParseOptionsProvider;
            var compilation = context.CompilationProvider;

            // determine if we should suppress this run and filter out all the additional files if so
            var isGeneratorSuppressed = context.AnalyzerConfigOptionsProvider.Select(GetSuppressionStatus);
            var additionalTexts = context.AdditionalTextsProvider
                 .Combine(isGeneratorSuppressed)
                 .Where(pair => !pair.Right)
                 .Select((pair, _) => pair.Left);

            var razorSourceGeneratorOptions = analyzerConfigOptions
                .Combine(parseOptions)
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
                .WithLambdaComparer((old, @new) => (old.Right.Equals(@new.Right) && old.Left.Left.Equals(@new.Left.Left) && old.Left.Right.SequenceEqual(@new.Left.Right)), (a) => a.GetHashCode())
                .Select(static (pair, _) =>
                {

                    var ((sourceItem, importFiles), razorSourceGeneratorOptions) = pair;
                    RazorSourceGeneratorEventSource.Log.GenerateDeclarationCodeStart(sourceItem.RelativePhysicalPath);

                    var projectEngine = GetDeclarationProjectEngine(sourceItem, importFiles, razorSourceGeneratorOptions);

                    var codeGen = projectEngine.Process(sourceItem);

                    var result = codeGen.GetCSharpDocument().GeneratedCode;

                    RazorSourceGeneratorEventSource.Log.GenerateDeclarationCodeStop(sourceItem.RelativePhysicalPath);

                    return (result, sourceItem.RelativePhysicalPath);
                });

            var generatedDeclarationSyntaxTrees = generatedDeclarationCode
                .Combine(parseOptions)
                .Select(static (pair, ct) =>
                {
                    var ((generatedDeclarationCode, filePath), parseOptions) = pair;
                    return CSharpSyntaxTree.ParseText(generatedDeclarationCode, (CSharpParseOptions)parseOptions, filePath, cancellationToken: ct);
                });

            var tagHelpersFromComponents = generatedDeclarationSyntaxTrees
                .Combine(compilation)
                .Combine(razorSourceGeneratorOptions)
                .SelectMany(static (pair, ct) =>
                {

                    var ((generatedDeclarationSyntaxTree, compilation), razorSourceGeneratorOptions) = pair;
                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromComponentStart(generatedDeclarationSyntaxTree.FilePath);

                    var tagHelperFeature = new StaticCompilationTagHelperFeature();
                    var discoveryProjectEngine = GetDiscoveryProjectEngine(compilation.References.ToImmutableArray(), tagHelperFeature);

                    var compilationWithDeclarations = compilation.AddSyntaxTrees(generatedDeclarationSyntaxTree);

                    // try and find the specific root class this component is declaring, falling back to the assembly if for any reason the code is not in the shape we expect
                    ISymbol targetSymbol = compilationWithDeclarations.Assembly;
                    var root = generatedDeclarationSyntaxTree.GetRoot(ct);
                    if (root is CompilationUnitSyntax { Members: [NamespaceDeclarationSyntax { Members: [ClassDeclarationSyntax classSyntax, ..] }, ..] })
                    {
                        var declaredClass = compilationWithDeclarations.GetSemanticModel(generatedDeclarationSyntaxTree).GetDeclaredSymbol(classSyntax, ct);
                        Debug.Assert(declaredClass is null || declaredClass is { AllInterfaces: [{ Name: "IComponent" }, ..] });
                        targetSymbol = declaredClass ?? targetSymbol;
                    }

                    tagHelperFeature.Compilation = compilationWithDeclarations;
                    tagHelperFeature.TargetSymbol = targetSymbol;

                    var result = tagHelperFeature.GetDescriptors();
                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromComponentStop(generatedDeclarationSyntaxTree.FilePath);
                    return result;
                });

            var tagHelpersFromCompilation = compilation
                .Combine(razorSourceGeneratorOptions)
                .Select(static (pair, _) =>
                {
                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromCompilationStart();

                    var (compilation, razorSourceGeneratorOptions) = pair;

                    var tagHelperFeature = new StaticCompilationTagHelperFeature();
                    var discoveryProjectEngine = GetDiscoveryProjectEngine(compilation.References.ToImmutableArray(), tagHelperFeature);

                    tagHelperFeature.Compilation = compilation;
                    tagHelperFeature.TargetSymbol = compilation.Assembly;

                    var result = tagHelperFeature.GetDescriptors();
                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromCompilationStop();
                    return result;
                });

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
                },
                static item =>
                {
                    // we'll use the number of references as a hashcode.
                    var ((compilationA, razorSourceGeneratorOptionsA), hasRazorFilesA) = item;
                    return compilationA.References.GetHashCode();
                })
                .Select(static (pair, _) =>
                {
                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStart();

                    var ((compilation, razorSourceGeneratorOptions), hasRazorFiles) = pair;
                    if (!hasRazorFiles)
                    {
                        // If there's no razor code in this app, don't do anything.
                        RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStop();
                        return ImmutableArray<TagHelperDescriptor>.Empty;
                    }

                    var tagHelperFeature = new StaticCompilationTagHelperFeature();
                    var discoveryProjectEngine = GetDiscoveryProjectEngine(compilation.References.ToImmutableArray(), tagHelperFeature);

                    using var pool = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var descriptors);
                    tagHelperFeature.Compilation = compilation;
                    foreach (var reference in compilation.References)
                    {
                        if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                        {
                            tagHelperFeature.TargetSymbol = assembly;
                            descriptors.AddRange(tagHelperFeature.GetDescriptors());
                        }
                    }

                    RazorSourceGeneratorEventSource.Log.DiscoverTagHelpersFromReferencesStop();
                    return descriptors.ToImmutable();
                });

            var allTagHelpers = tagHelpersFromComponents.Collect()
                .Combine(tagHelpersFromCompilation)
                .Combine(tagHelpersFromReferences)
                .Select(static (pair, _) =>
                {
                    var ((tagHelpersFromComponents, tagHelpersFromCompilation), tagHelpersFromReferences) = pair;
                    var count = tagHelpersFromCompilation.Length + tagHelpersFromReferences.Length + tagHelpersFromComponents.Length;
                    if (count == 0)
                    {
                        return ImmutableArray<TagHelperDescriptor>.Empty;
                    }

                    using var pool = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var allTagHelpers);
					allTagHelpers.AddRange(tagHelpersFromCompilation);
                    allTagHelpers.AddRange(tagHelpersFromReferences);
                    allTagHelpers.AddRange(tagHelpersFromComponents);

                    return allTagHelpers.ToImmutable();
                });

            var generatedOutput = sourceItems
                .Combine(importFiles.Collect())
                .WithLambdaComparer((old, @new) => old.Left.Equals(@new.Left) && old.Right.SequenceEqual(@new.Right), (a) => a.GetHashCode())
                .Combine(razorSourceGeneratorOptions)
                .Select(static (pair, _) =>
                {
                    var ((sourceItem, imports), razorSourceGeneratorOptions) = pair;

                    RazorSourceGeneratorEventSource.Log.ParseRazorDocumentStart(sourceItem.RelativePhysicalPath);

                    var projectEngine = GetGenerationProjectEngine(sourceItem, imports, razorSourceGeneratorOptions);

                    var document = projectEngine.ProcessInitialParse(sourceItem);

                    RazorSourceGeneratorEventSource.Log.ParseRazorDocumentStop(sourceItem.RelativePhysicalPath);
                    return (projectEngine, sourceItem.RelativePhysicalPath, document);
                })

                // Add the tag helpers in, but ignore if they've changed or not, only reprocessing the actual document changed
                .Combine(allTagHelpers)
                .WithLambdaComparer((old, @new) => old.Left.Equals(@new.Left), (item) => item.GetHashCode())
                .Select((pair, _) =>
                {
                    var ((projectEngine, filePath, codeDocument), allTagHelpers) = pair;
                    RazorSourceGeneratorEventSource.Log.RewriteTagHelpersStart(filePath);

                    codeDocument = projectEngine.ProcessTagHelpers(codeDocument, allTagHelpers, checkForIdempotency: false);

                    RazorSourceGeneratorEventSource.Log.RewriteTagHelpersStop(filePath);
                    return (projectEngine, filePath, codeDocument);
                })

                // next we do a second parse, along with the helpers, but check for idempotency. If the tag helpers used on the previous parse match, the compiler can skip re-computing them
                .Combine(allTagHelpers)
                .Select((pair, _) =>
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
                    RazorSourceGeneratorEventSource.Log.RazorCodeGenerateStart(filePath);
                    document = projectEngine.ProcessRemaining(document);
                    var csharpDocument = document.CodeDocument.GetCSharpDocument();

                    RazorSourceGeneratorEventSource.Log.RazorCodeGenerateStop(filePath);
                    return (filePath, csharpDocument);
                })
                .WithLambdaComparer(static (a, b) =>
                {
                    if (a.csharpDocument.Diagnostics.Count > 0 || b.csharpDocument.Diagnostics.Count > 0)
                    {
                        // if there are any diagnostics, treat the documents as unequal and force RegisterSourceOutput to be called uncached.
                        return false;
                    }

                    return string.Equals(a.csharpDocument.GeneratedCode, b.csharpDocument.GeneratedCode, StringComparison.Ordinal);
                }, static a => StringComparer.Ordinal.GetHashCode(a.csharpDocument));

            context.RegisterSourceOutput(generatedOutput, static (context, pair) =>
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
        }
    }
}
