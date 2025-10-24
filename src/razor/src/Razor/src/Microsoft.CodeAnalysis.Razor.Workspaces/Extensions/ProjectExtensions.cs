﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.NET.Sdk.Razor.SourceGenerators;

namespace Microsoft.CodeAnalysis;

internal static class ProjectExtensions
{
    private const string GetTagHelpersEventName = "taghelperresolver/gettaghelpers";
    private const string PropertySuffix = ".elapsedtimems";

    /// <summary>
    ///  Gets the available <see cref="TagHelperDescriptor">tag helpers</see> from the specified
    ///  <see cref="Project"/> using the given <see cref="RazorProjectEngine"/>.
    /// </summary>
    /// <remarks>
    ///  A telemetry event will be reported to <paramref name="telemetryReporter"/>.
    /// </remarks>
    public static async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        this Project project,
        RazorProjectEngine projectEngine,
        ITelemetryReporter telemetryReporter,
        CancellationToken cancellationToken)
    {
        var providers = GetTagHelperDescriptorProviders(projectEngine);

        if (providers is [])
        {
            return [];
        }

        var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (compilation is null || !CompilationTagHelperFeature.IsValidCompilation(compilation))
        {
            return [];
        }

        using var pooledHashSet = HashSetPool<TagHelperDescriptor>.GetPooledObject(out var results);
        using var pooledWatch = StopwatchPool.GetPooledObject(out var watch);
        using var pooledSpan = ArrayPool<Property>.Shared.GetPooledArraySpan(minimumLength: providers.Length, out var properties);

        var context = new TagHelperDescriptorProviderContext(compilation, results)
        {
            ExcludeHidden = true,
            IncludeDocumentation = true
        };

        var writeProperties = properties;

        foreach (var provider in providers)
        {
            watch.Restart();
            provider.Execute(context, cancellationToken);
            watch.Stop();

            writeProperties[0] = new(provider.GetType().Name + PropertySuffix, watch.ElapsedMilliseconds);
            writeProperties = writeProperties[1..];
        }

        telemetryReporter.ReportEvent(GetTagHelpersEventName, Severity.Normal, properties);

        return [.. results];
    }

    private static ImmutableArray<ITagHelperDescriptorProvider> GetTagHelperDescriptorProviders(RazorProjectEngine projectEngine)
        => projectEngine.Engine.GetFeatures<ITagHelperDescriptorProvider>().OrderByAsArray(static x => x.Order);

    public static Task<SourceGeneratedDocument?> TryGetCSharpDocumentFromGeneratedDocumentUriAsync(this Project project, Uri generatedDocumentUri, CancellationToken cancellationToken)
    {
        if (!TryGetHintNameFromGeneratedDocumentUri(project, generatedDocumentUri, out var hintName))
        {
            return SpecializedTasks.Null<SourceGeneratedDocument>();
        }

        return TryGetSourceGeneratedDocumentFromHintNameAsync(project, hintName, cancellationToken);
    }

    /// <summary>
    /// Finds source generated documents by iterating through all of them. In OOP there are better options!
    /// </summary>
    public static async Task<SourceGeneratedDocument?> TryGetSourceGeneratedDocumentFromHintNameAsync(this Project project, string? hintName, CancellationToken cancellationToken)
    {
        // TODO: use this when the location is case-insensitive on windows (https://github.com/dotnet/roslyn/issues/76869)
        //var generator = typeof(RazorSourceGenerator);
        //var generatorAssembly = generator.Assembly;
        //var generatorName = generatorAssembly.GetName();
        //var generatedDocuments = await _project.GetSourceGeneratedDocumentsForGeneratorAsync(generatorName.Name!, generatorAssembly.Location, generatorName.Version!, generator.Name, cancellationToken).ConfigureAwait(false);

        var generatedDocuments = await project.GetSourceGeneratedDocumentsAsync(cancellationToken).ConfigureAwait(false);
        return generatedDocuments.SingleOrDefault(d => d.HintName == hintName);
    }

    /// <summary>
    /// Finds source generated documents by iterating through all of them. In OOP there are better options!
    /// </summary>
    public static async Task<SourceGeneratedDocument?> TryGetSourceGeneratedDocumentForRazorDocumentAsync(this Project project, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        if (razorDocument.FilePath is null)
        {
            return null;
        }

        var generatedDocuments = await project.GetSourceGeneratedDocumentsAsync(cancellationToken).ConfigureAwait(false);

        // For misc files, and projects that don't have a globalconfig file (eg, non Razor SDK projects), the hint name will be based
        // on the full path of the file.
        var fullPathHintName = RazorSourceGenerator.GetIdentifierFromPath(razorDocument.FilePath);
        // For normal Razor SDK projects, the hint name will be based on the project-relative path of the file.
        var projectRelativeHintName = GetProjectRelativeHintName(razorDocument);

        SourceGeneratedDocument? candidateDoc = null;
        foreach (var doc in generatedDocuments)
        {
            if (!doc.IsRazorSourceGeneratedDocument())
            {
                continue;
            }

            if (doc.HintName == fullPathHintName)
            {
                // If the full path matches, we've found it for sure
                return doc;
            }
            else if (doc.HintName == projectRelativeHintName)
            {
                if (candidateDoc is not null)
                {
                    // Multiple documents with the same hint name found, can't be sure which one to return
                    // This can happen as a result of a bug in the source generator: https://github.com/dotnet/razor/issues/11578
                    candidateDoc = null;
                    break;
                }

                candidateDoc = doc;
            }
        }

        return candidateDoc;

        static string? GetProjectRelativeHintName(TextDocument razorDocument)
        {
            var filePath = razorDocument.FilePath.AsSpanOrDefault();
            if (string.IsNullOrEmpty(razorDocument.Project.FilePath))
            {
                // Misc file - no project info to get a relative path
                return null;
            }

            var projectFilePath = razorDocument.Project.FilePath.AsSpanOrDefault();
            var projectBasePath = PathUtilities.GetDirectoryName(projectFilePath);
            if (filePath.Length <= projectBasePath.Length)
            {
                // File must be from outside the project directory
                return null;
            }

            var relativeDocumentPath = filePath[projectBasePath.Length..].TrimStart(['/', '\\']);

            return RazorSourceGenerator.GetIdentifierFromPath(relativeDocumentPath);
        }
    }

    /// <summary>
    /// Finds source generated documents by iterating through all of them. In OOP there are better options!
    /// </summary>
    public static bool TryGetHintNameFromGeneratedDocumentUri(this Project project, Uri generatedDocumentUri, [NotNullWhen(true)] out string? hintName)
    {
        if (!RazorUri.IsGeneratedDocumentUri(generatedDocumentUri))
        {
            hintName = null;
            return false;
        }

        var identity = RazorUri.GetIdentityOfGeneratedDocument(project.Solution, generatedDocumentUri);

        if (!identity.IsRazorSourceGeneratedDocument())
        {
            // This is not a Razor source generated document, so we don't know the hint name.
            hintName = null;
            return false;
        }

        hintName = identity.HintName;
        return true;
    }
}
