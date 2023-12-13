// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

internal class TestRazorDocumentServiceProvider(IRazorSpanMappingService spanMappingService) : IRazorDocumentServiceProvider
{
    private readonly IRazorSpanMappingService _spanMappingService = spanMappingService;

    public bool CanApplyChange => throw new NotImplementedException();

    public bool SupportDiagnostics => true;

    TService IRazorDocumentServiceProvider.GetService<TService>()
    {
        var serviceType = typeof(TService);

        if (serviceType == typeof(IRazorSpanMappingService))
        {
            return (TService)_spanMappingService;
        }

        if (serviceType == typeof(IRazorDocumentPropertiesService))
        {
            return (TService)(IRazorDocumentPropertiesService)new TestRazorDocumentPropertiesService();
        }

        return (this as TService).AssumeNotNull();
    }

    private class TestRazorDocumentPropertiesService : IRazorDocumentPropertiesService
    {
        public bool DesignTimeOnly => false;

        public string DiagnosticsLspClientName => "RazorCSharp";
    }
}
