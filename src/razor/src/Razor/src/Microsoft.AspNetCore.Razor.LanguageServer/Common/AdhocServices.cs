// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal sealed class AdhocServices : HostServices
{
    private readonly ImmutableArray<IWorkspaceService> _workspaceServices;
    private readonly ImmutableArray<ILanguageService> _languageServices;
    private readonly HostServices _fallbackHostServices;
    private readonly MethodInfo _createWorkspaceServicesMethod;

    private AdhocServices(
        ImmutableArray<IWorkspaceService> workspaceServices,
        ImmutableArray<ILanguageService> languageServices,
        HostServices fallbackHostServices)
    {
        _workspaceServices = workspaceServices;
        _languageServices = languageServices;
        _fallbackHostServices = fallbackHostServices;

        // We need to create workspace services from the provided fallback host services. To do that we need to invoke into Roslyn's
        // CreateWorkspaceServices method. Ultimately the reason behind this is to ensure that any services created by this class are
        // truly isolated from the passed in fallback services host workspace.
        _createWorkspaceServicesMethod = typeof(HostServices)
            .GetMethod(nameof(CreateWorkspaceServices), BindingFlags.Instance | BindingFlags.NonPublic)
            .AssumeNotNull();
    }

    protected override HostWorkspaceServices CreateWorkspaceServices(Workspace workspace)
    {
        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        var fallbackServices = CreateFallbackWorkspaceServices(workspace);
        return new AdhocWorkspaceServices(this, _workspaceServices, _languageServices, workspace, fallbackServices);
    }

    public static HostServices Create(
        ImmutableArray<IWorkspaceService> workspaceServices,
        ImmutableArray<ILanguageService> languageServices,
        HostServices fallbackServices)
        => new AdhocServices(workspaceServices, languageServices, fallbackServices);

    private HostWorkspaceServices CreateFallbackWorkspaceServices(Workspace workspace)
    {
        var result = _createWorkspaceServicesMethod.Invoke(_fallbackHostServices, new[] { workspace }) as HostWorkspaceServices;

        return result.AssumeNotNull();
    }
}
