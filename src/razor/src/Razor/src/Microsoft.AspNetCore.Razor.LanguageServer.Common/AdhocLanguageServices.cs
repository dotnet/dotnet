// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

public class AdhocLanguageServices : HostLanguageServices
{
    private readonly HostWorkspaceServices _workspaceServices;
    private readonly IEnumerable<ILanguageService> _languageServices;

    public AdhocLanguageServices(HostWorkspaceServices workspaceServices, IEnumerable<ILanguageService> languageServices)
    {
        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        _workspaceServices = workspaceServices;
        _languageServices = languageServices;
    }

    public override HostWorkspaceServices WorkspaceServices => _workspaceServices;

    public override string Language => RazorLanguage.Name;

    public override TLanguageService GetService<TLanguageService>()
    {
        var service = _languageServices.OfType<TLanguageService>().FirstOrDefault();

        if (service is null)
        {
            throw new InvalidOperationException(SR.FormatLanguage_Services_Missing_Service(typeof(TLanguageService).FullName));
        }

        return service;
    }
}
