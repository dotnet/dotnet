// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal interface IDynamicDocumentContainer
{
    string FilePath { get; }

    bool SupportsDiagnostics { get; }

    void SetSupportsDiagnostics(bool enabled);

    TextLoader GetTextLoader(string filePath);

    IRazorMappingService? GetMappingService();
    IRazorDocumentExcerptServiceImplementation? GetExcerptService();
    IRazorDocumentPropertiesService GetDocumentPropertiesService();
}
