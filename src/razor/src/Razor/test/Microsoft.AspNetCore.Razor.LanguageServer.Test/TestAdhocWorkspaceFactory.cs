// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal class TestAdhocWorkspaceFactory : IAdhocWorkspaceFactory
{
    public static readonly TestAdhocWorkspaceFactory Instance = new();

    private TestAdhocWorkspaceFactory()
    {
    }

    public AdhocWorkspace Create(params IWorkspaceService[] workspaceServices)
    {
        var services = TestServices.Create(workspaceServices, Enumerable.Empty<ILanguageService>());
        var workspace = TestWorkspace.Create(services);
        return workspace;
    }
}
