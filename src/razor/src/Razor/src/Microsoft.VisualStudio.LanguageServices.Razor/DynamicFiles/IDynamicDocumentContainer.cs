// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
