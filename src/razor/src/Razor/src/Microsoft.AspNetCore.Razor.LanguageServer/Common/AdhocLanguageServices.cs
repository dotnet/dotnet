// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal sealed class AdhocLanguageServices(
    AdhocWorkspaceServices workspaceServices,
    ImmutableArray<ILanguageService> languageServices)
    : HostLanguageServices
{
    public override string Language => RazorLanguage.Name;

    public override HostWorkspaceServices WorkspaceServices => workspaceServices;

    public override TLanguageService GetService<TLanguageService>()
    {
        foreach (var service in languageServices)
        {
            if (service is TLanguageService languageService)
            {
                return languageService;
            }
        }

        throw new InvalidOperationException(SR.FormatLanguage_Services_Missing_Service(typeof(TLanguageService).FullName));
    }
}
