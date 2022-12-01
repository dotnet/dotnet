// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis.Host;

internal class TestWorkspaceServices : HostWorkspaceServices
{
    private static readonly Workspace s_defaultWorkspace = TestWorkspace.Create();

    private readonly HostServices _hostServices;
    private readonly HostLanguageServices _razorLanguageServices;
    private readonly IEnumerable<IWorkspaceService> _workspaceServices;
    private readonly Workspace _workspace;

    public TestWorkspaceServices(
        HostServices hostServices,
        IEnumerable<IWorkspaceService> workspaceServices,
        IEnumerable<ILanguageService> languageServices,
        Workspace workspace)
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

        _hostServices = hostServices;
        _workspaceServices = workspaceServices;
        _workspace = workspace;

        _razorLanguageServices = new TestLanguageServices(this, languageServices);
    }

    public override HostServices HostServices => _hostServices;

    public override Workspace Workspace => _workspace;

    public override TWorkspaceService GetService<TWorkspaceService>()
    {
        var service = _workspaceServices.OfType<TWorkspaceService>().FirstOrDefault();

        if (service is null)
        {
            // Fallback to default host services to resolve roslyn specific features.
            service = s_defaultWorkspace.Services.GetService<TWorkspaceService>();
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
        return s_defaultWorkspace.Services.GetLanguageServices(languageName);
    }

    public override IEnumerable<string> SupportedLanguages => new[] { RazorLanguage.Name };

    public override bool IsSupported(string languageName) => languageName == RazorLanguage.Name;

    public override IEnumerable<TLanguageService> FindLanguageServices<TLanguageService>(MetadataFilter filter) => throw new NotImplementedException();
}
