// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

internal class TestRazorDocumentServiceProvider(IRazorSpanMappingService spanMappingService) : IRazorDocumentServiceProvider
{
    private readonly IRazorSpanMappingService _spanMappingService = spanMappingService;

    public bool CanApplyChange => true;

    public bool SupportDiagnostics => true;

    TService? IRazorDocumentServiceProvider.GetService<TService>() where TService : class
    {
        var serviceType = typeof(TService);

        if (serviceType == typeof(IRazorSpanMappingService))
        {
            return (TService?)_spanMappingService;
        }

        if (serviceType == typeof(IRazorDocumentPropertiesService))
        {
            return (TService?)(IRazorDocumentPropertiesService)new TestRazorDocumentPropertiesService();
        }

        if (serviceType == typeof(IRazorMappingService))
        {
            return null;
        }

        return (this as TService).AssumeNotNull();
    }

    private class TestRazorDocumentPropertiesService : IRazorDocumentPropertiesService
    {
        public bool DesignTimeOnly => false;

        public string DiagnosticsLspClientName => "RazorCSharp";
    }
}
