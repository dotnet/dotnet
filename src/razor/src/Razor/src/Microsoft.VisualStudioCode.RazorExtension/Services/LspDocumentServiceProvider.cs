// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
