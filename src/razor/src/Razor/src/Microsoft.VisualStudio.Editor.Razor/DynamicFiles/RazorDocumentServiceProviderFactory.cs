// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
