// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class ExportProviderBuilder
{
    public static async Task<ExportProvider> CreateExportProviderAsync(string extensionPath)
    {
        var baseDirectory = Path.GetDirectoryName(extensionPath);
        var assemblyLoader = new CustomExportAssemblyLoader(baseDirectory!);
        var resolver = new Resolver(assemblyLoader);

        var discovery = PartDiscovery.Combine(
            resolver,
            new AttributedPartDiscovery(resolver, isNonPublicSupported: true), // "NuGet MEF" attributes (Microsoft.Composition)
            new AttributedPartDiscoveryV1(resolver));

        // TODO - we should likely cache the catalog so we don't have to rebuild it every time.
        var parts = await discovery.CreatePartsAsync(new[] { extensionPath! }).ConfigureAwait(true);
        var catalog = ComposableCatalog.Create(resolver)
            .AddParts(parts)
            .WithCompositionService(); // Makes an ICompositionService export available to MEF parts to import

        // Assemble the parts into a valid graph.
        var config = CompositionConfiguration.Create(catalog);

        // Verify we have no errors.
        config.ThrowOnErrors();

        // Prepare an ExportProvider factory based on this graph.
        var exportProviderFactory = config.CreateExportProviderFactory();

        // Create an export provider, which represents a unique container of values.
        // You can create as many of these as you want, but typically an app needs just one.
        var exportProvider = exportProviderFactory.CreateExportProvider();

        return exportProvider;
    }
}
