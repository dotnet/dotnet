// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

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
            provider.Execute(context);
            watch.Stop();

            writeProperties[0] = new(provider.GetType().Name + PropertySuffix, watch.ElapsedMilliseconds);
            writeProperties = writeProperties[1..];
        }

        telemetryReporter.ReportEvent(GetTagHelpersEventName, Severity.Normal, properties);

        return [.. results];
    }

    private static ImmutableArray<ITagHelperDescriptorProvider> GetTagHelperDescriptorProviders(RazorProjectEngine projectEngine)
        => projectEngine.Engine.GetFeatures<ITagHelperDescriptorProvider>().OrderByAsArray(static x => x.Order);

    internal static Document GetRequiredDocument(this Project project, DocumentId documentId)
    {
        return project.GetDocument(documentId)
            ?? ThrowHelper.ThrowInvalidOperationException<Document>($"The document {documentId} did not exist in {project.Name}");
    }

    public static bool TryGetCSharpDocument(this Project project, Uri csharpDocumentUri, [NotNullWhen(true)] out Document? document)
    {
        document = null;

        var generatedDocumentIds = project.Solution.GetDocumentIdsWithUri(csharpDocumentUri);
        var generatedDocumentId = generatedDocumentIds.FirstOrDefault(d => d.ProjectId == project.Id);
        if (generatedDocumentId is null)
        {
            return false;
        }

        if (project.GetDocument(generatedDocumentId) is { } generatedDocument)
        {
            document = generatedDocument;
        }

        return document is not null;
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
    public static bool TryGetHintNameFromGeneratedDocumentUri(this Project project, Uri generatedDocumentUri, [NotNullWhen(true)] out string? hintName)
    {
        if (!RazorUri.IsGeneratedDocumentUri(generatedDocumentUri))
        {
            hintName = null;
            return false;
        }

        hintName = RazorUri.GetHintNameFromGeneratedDocumentUri(project.Solution, generatedDocumentUri);
        return true;
    }
}
