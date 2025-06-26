// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor.GoToDefinition;

internal abstract class AbstractRazorComponentDefinitionService(
    IRazorComponentSearchEngine componentSearchEngine,
    IDocumentMappingService documentMappingService,
    ILogger logger) : IRazorComponentDefinitionService
{
    private readonly IRazorComponentSearchEngine _componentSearchEngine = componentSearchEngine;
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = logger;

    public async Task<LspLocation?> GetDefinitionAsync(
        IDocumentSnapshot documentSnapshot,
        DocumentPositionInfo positionInfo,
        ISolutionQueryOperations solutionQueryOperations,
        bool ignoreAttributes,
        CancellationToken cancellationToken)
    {
        // If we're in C# then there is no point checking for a component tag, because there won't be one
        if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            return null;
        }

        if (!documentSnapshot.FileKind.IsComponent())
        {
            _logger.LogInformation($"'{documentSnapshot.FileKind}' is not a component type.");
            return null;
        }

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        if (!RazorComponentDefinitionHelpers.TryGetBoundTagHelpers(codeDocument, positionInfo.HostDocumentIndex, ignoreAttributes, _logger, out var boundTagHelper, out var boundAttribute))
        {
            _logger.LogInformation($"Could not retrieve bound tag helper information.");
            return null;
        }

        var componentDocument = await _componentSearchEngine
            .TryLocateComponentAsync(boundTagHelper, solutionQueryOperations, cancellationToken)
            .ConfigureAwait(false);

        if (componentDocument is null)
        {
            _logger.LogInformation($"Could not locate component document.");
            return null;
        }

        var componentFilePath = componentDocument.FilePath;

        _logger.LogInformation($"Definition found at file path: {componentFilePath}");

        var range = await GetNavigateRangeAsync(componentDocument, boundAttribute, cancellationToken).ConfigureAwait(false);

        return LspFactory.CreateLocation(componentFilePath, range);
    }

    private async Task<LspRange> GetNavigateRangeAsync(IDocumentSnapshot documentSnapshot, BoundAttributeDescriptor? attributeDescriptor, CancellationToken cancellationToken)
    {
        if (attributeDescriptor is not null)
        {
            _logger.LogInformation($"Attempting to get definition from an attribute directly.");

            var range = await RazorComponentDefinitionHelpers
                .TryGetPropertyRangeAsync(documentSnapshot, attributeDescriptor.GetPropertyName().AssumeNotNull(), _documentMappingService, _logger, cancellationToken)
                .ConfigureAwait(false);

            if (range is not null)
            {
                return range;
            }
        }

        // When navigating from a start or end tag, we just take the user to the top of the file.
        // If we were trying to navigate to a property, and we couldn't find it, we can at least take
        // them to the file for the component. If the property was defined in a partial class they can
        // at least then press F7 to go there.
        return LspFactory.DefaultRange;
    }
}
