// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[FileExtension(RazorLSPConstants.RazorFileExtension)]
[Name(nameof(RazorFilePathToContentTypeProvider))]
[Export(typeof(IFilePathToContentTypeProvider))]
[method: ImportingConstructor]
internal class RazorFilePathToContentTypeProvider(
    IContentTypeRegistryService contentTypeRegistryService,
    ILspEditorFeatureDetector lspEditorFeatureDetector)
    : RazorFilePathToContentTypeProviderBase(contentTypeRegistryService, lspEditorFeatureDetector)
{
}
