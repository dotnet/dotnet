// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal class AdhocWorkspaceServices : HostWorkspaceServices
{
    private readonly HostServices _hostServices;
    private readonly HostLanguageServices _razorLanguageServices;
    private readonly IEnumerable<IWorkspaceService> _workspaceServices;
    private readonly Workspace _workspace;
    private readonly HostWorkspaceServices _fallbackServices;

    public AdhocWorkspaceServices(
        HostServices hostServices,
        IEnumerable<IWorkspaceService> workspaceServices,
        IEnumerable<ILanguageService> languageServices,
        Workspace workspace,
        HostWorkspaceServices fallbackServices)
    {
        if (hostServices is null)
        {
            throw new ArgumentNullException(nameof(hostServices));
        }

        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        if (fallbackServices is null)
        {
            throw new ArgumentNullException(nameof(fallbackServices));
        }

        _hostServices = hostServices;
        _workspaceServices = workspaceServices;
        _workspace = workspace;
        _fallbackServices = fallbackServices;
        _razorLanguageServices = new AdhocLanguageServices(this, languageServices);
    }

    public override HostServices HostServices => _hostServices;

    public override Workspace Workspace => _workspace;

    public override TWorkspaceService? GetService<TWorkspaceService>()
        where TWorkspaceService : default
    {
        var service = _workspaceServices.OfType<TWorkspaceService>().FirstOrDefault();

        if (service is null)
        {
            // Fallback to default host services to resolve roslyn specific features.
            service = _fallbackServices.GetService<TWorkspaceService>();
        }

        return service;
    }

    public override HostLanguageServices GetLanguageServices(string languageName)
    {
        if (languageName == RazorLanguage.Name)
        {
            return _razorLanguageServices;
        }

        // Fallback to default host services to resolve roslyn specific features.
        return _fallbackServices.GetLanguageServices(languageName);
    }

    public override IEnumerable<string> SupportedLanguages => new[] { RazorLanguage.Name };

    public override bool IsSupported(string languageName) => languageName == RazorLanguage.Name;

    public override IEnumerable<TLanguageService> FindLanguageServices<TLanguageService>(MetadataFilter filter) => throw new NotImplementedException();
}
