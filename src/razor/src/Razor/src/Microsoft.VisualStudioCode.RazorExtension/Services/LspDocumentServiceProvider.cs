// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class LspDocumentServiceProvider(IRazorClientLanguageServerManager razorClientLanguageServerManager) : IRazorDocumentServiceProvider
{
    public bool CanApplyChange => true;

    public bool SupportDiagnostics => true;

    private IRazorMappingService? _mappingService;

    public TService? GetService<TService>() where TService : class
    {
        var serviceType = typeof(TService);

        if (serviceType == typeof(IRazorMappingService))
        {
            var mappingService = _mappingService ?? InterlockedOperations.Initialize(ref _mappingService, CreateMappingService());
            return (TService?)mappingService;
        }

        return this as TService;
    }

    private IRazorMappingService CreateMappingService()
    {
        return new MappingService(razorClientLanguageServerManager);
    }
}
