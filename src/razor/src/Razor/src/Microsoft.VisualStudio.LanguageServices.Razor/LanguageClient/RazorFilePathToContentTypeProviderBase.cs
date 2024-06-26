// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal abstract class RazorFilePathToContentTypeProviderBase : IFilePathToContentTypeProvider
{
    private readonly IContentTypeRegistryService _contentTypeRegistryService;
    private readonly ILspEditorFeatureDetector _lspEditorFeatureDetector;

    public RazorFilePathToContentTypeProviderBase(
        IContentTypeRegistryService contentTypeRegistryService,
        ILspEditorFeatureDetector lspEditorFeatureDetector)
    {
        if (contentTypeRegistryService is null)
        {
            throw new ArgumentNullException(nameof(contentTypeRegistryService));
        }

        if (lspEditorFeatureDetector is null)
        {
            throw new ArgumentNullException(nameof(lspEditorFeatureDetector));
        }

        _contentTypeRegistryService = contentTypeRegistryService;
        _lspEditorFeatureDetector = lspEditorFeatureDetector;
    }

    public bool TryGetContentTypeForFilePath(string filePath, [NotNullWhen(true)] out IContentType? contentType)
    {
        if (_lspEditorFeatureDetector.IsLspEditorEnabled())
        {
            contentType = _contentTypeRegistryService.GetContentType(RazorConstants.RazorLSPContentTypeName);
            return true;
        }

        contentType = null;
        return false;
    }
}
