// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;

internal class TestRazorDocumentServiceProvider : IRazorDocumentServiceProvider
{
    private readonly IRazorSpanMappingService _razorSpanMappingService;

    public TestRazorDocumentServiceProvider(IRazorSpanMappingService razorSpanMappingService)
    {
        _razorSpanMappingService = razorSpanMappingService;
    }

    public bool CanApplyChange => throw new NotImplementedException();

    public bool SupportDiagnostics => true;

    TService IRazorDocumentServiceProvider.GetService<TService>()
    {
        var serviceType = typeof(TService);

        if (serviceType == typeof(IRazorSpanMappingService))
        {
            return (TService)_razorSpanMappingService;
        }

        if (serviceType == typeof(IRazorDocumentPropertiesService))
        {
            return (TService)(IRazorDocumentPropertiesService)new TestRazorDocumentPropertiesService();
        }

        return this as TService;
    }

    private class TestRazorDocumentPropertiesService : IRazorDocumentPropertiesService
    {
        public bool DesignTimeOnly => false;

        public string DiagnosticsLspClientName => "RazorCSharp";
    }
}
