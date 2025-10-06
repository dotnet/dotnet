// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

[Export(typeof(IRazorDocumentServiceProviderFactory))]
internal sealed class RazorDocumentServiceProviderFactory : IRazorDocumentServiceProviderFactory
{
    public IRazorDocumentServiceProvider Create(IDynamicDocumentContainer documentContainer)
    {
        if (documentContainer is null)
        {
            throw new ArgumentNullException(nameof(documentContainer));
        }

        return new RazorDocumentServiceProvider(documentContainer);
    }

    public IRazorDocumentServiceProvider CreateEmpty()
    {
        return new RazorDocumentServiceProvider();
    }
}
