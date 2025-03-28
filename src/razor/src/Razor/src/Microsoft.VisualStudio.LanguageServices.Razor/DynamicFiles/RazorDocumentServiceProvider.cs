// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal class RazorDocumentServiceProvider(IDynamicDocumentContainer? documentContainer) : IRazorDocumentServiceProvider
{
    private readonly IDynamicDocumentContainer? _documentContainer = documentContainer;
    private readonly object _lock = new();

    private IRazorDocumentExcerptServiceImplementation? _documentExcerptService;
    private IRazorDocumentPropertiesService? _documentPropertiesService;
    private IRazorMappingService? _mappingService;

    public RazorDocumentServiceProvider()
        : this(null)
    {
    }

    public bool CanApplyChange => false;

    public bool SupportDiagnostics => _documentContainer?.SupportsDiagnostics ?? false;

    public TService? GetService<TService>() where TService : class
    {
        if (_documentContainer is null)
        {
            return this as TService;
        }

        var serviceType = typeof(TService);

        if (serviceType == typeof(IRazorDocumentExcerptServiceImplementation))
        {
            if (_documentExcerptService is null)
            {
                lock (_lock)
                {
                    _documentExcerptService ??= _documentContainer.GetExcerptService();
                }
            }

            return (TService?)_documentExcerptService;
        }

        if (serviceType == typeof(IRazorDocumentPropertiesService))
        {
            if (_documentPropertiesService is null)
            {
                lock (_lock)
                {
                    _documentPropertiesService ??= _documentContainer.GetDocumentPropertiesService();
                }
            }

            return (TService?)_documentPropertiesService;
        }

        if (serviceType == typeof(IRazorMappingService))
        {
            if (_mappingService is null)
            {
                lock (_lock)
                {
                    _mappingService ??= _documentContainer.GetMappingService();
                }
            }

            return (TService?)_mappingService;
        }

        return this as TService;
    }
}
