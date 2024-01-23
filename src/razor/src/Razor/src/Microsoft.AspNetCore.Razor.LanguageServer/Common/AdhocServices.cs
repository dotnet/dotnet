// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal class AdhocServices : HostServices
{
    private readonly IEnumerable<IWorkspaceService> _workspaceServices;
    private readonly IEnumerable<ILanguageService> _razorLanguageServices;
    private readonly HostServices _fallbackHostServices;
    private readonly MethodInfo _createWorkspaceServicesMethod;

    private AdhocServices(
        IEnumerable<IWorkspaceService> workspaceServices,
        IEnumerable<ILanguageService> razorLanguageServices,
        HostServices fallbackHostServices)
    {
        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        if (razorLanguageServices is null)
        {
            throw new ArgumentNullException(nameof(razorLanguageServices));
        }

        if (fallbackHostServices is null)
        {
            throw new ArgumentNullException(nameof(fallbackHostServices));
        }

        _workspaceServices = workspaceServices;
        _razorLanguageServices = razorLanguageServices;
        _fallbackHostServices = fallbackHostServices;

        // We need to create workspace services from the provided fallback host services. To do that we need to invoke into Roslyn's
        // CreateWorkspaceServices method. Ultimately the reason behind this is to ensure that any services created by this class are
        // truly isolated from the passed in fallback services host workspace.
        var createWorkspaceServicesMethod = typeof(HostServices).GetMethod(nameof(CreateWorkspaceServices), BindingFlags.Instance | BindingFlags.NonPublic);
        Assumes.NotNull(createWorkspaceServicesMethod);

        _createWorkspaceServicesMethod = createWorkspaceServicesMethod;
    }

    protected override HostWorkspaceServices CreateWorkspaceServices(Workspace workspace)
    {
        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        var fallbackServices = CreateFallbackWorkspaceServices(workspace);
        return new AdhocWorkspaceServices(this, _workspaceServices, _razorLanguageServices, workspace, fallbackServices);
    }

    public static HostServices Create(
        IEnumerable<IWorkspaceService> workspaceServices,
        IEnumerable<ILanguageService> razorLanguageServices,
        HostServices fallbackServices)
        => new AdhocServices(workspaceServices, razorLanguageServices, fallbackServices);

    private HostWorkspaceServices CreateFallbackWorkspaceServices(Workspace workspace)
    {
        var result = _createWorkspaceServicesMethod.Invoke(_fallbackHostServices, new[] { workspace }) as HostWorkspaceServices;
        Assumes.NotNull(result);

        return result;
    }
}
