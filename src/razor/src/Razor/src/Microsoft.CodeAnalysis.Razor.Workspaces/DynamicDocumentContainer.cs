// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class DynamicDocumentContainer
{
    public abstract string FilePath { get; }

    public virtual bool SupportsDiagnostics { get; set; }

    public abstract TextLoader GetTextLoader(string filePath);

    public abstract IRazorSpanMappingService GetMappingService();

    public abstract IRazorDocumentExcerptServiceImplementation GetExcerptService();

    public abstract IRazorDocumentPropertiesService GetDocumentPropertiesService();
}
