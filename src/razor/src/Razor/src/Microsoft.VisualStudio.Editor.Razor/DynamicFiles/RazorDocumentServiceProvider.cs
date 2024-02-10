// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal class RazorDocumentServiceProvider(IDynamicDocumentContainer? documentContainer) : IRazorDocumentServiceProvider, IRazorDocumentOperationService
{
    private readonly IDynamicDocumentContainer? _documentContainer = documentContainer;
    private readonly object _lock = new object();

    private IRazorSpanMappingService? _spanMappingService;
    private IRazorDocumentExcerptServiceImplementation? _documentExcerptService;
    private IRazorDocumentPropertiesService? _documentPropertiesService;

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

        if (serviceType == typeof(IRazorSpanMappingService))
        {
            if (_spanMappingService is null)
            {
                lock (_lock)
                {
                    _spanMappingService ??= _documentContainer.GetMappingService();
                }
            }

            return (TService?)_spanMappingService;
        }

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

        return this as TService;
    }
}
