// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CodeAnalysis.Host;

public class TestServices : HostServices
{
    private readonly IEnumerable<IWorkspaceService> _workspaceServices;
    private readonly IEnumerable<ILanguageService> _razorLanguageServices;

    private TestServices(IEnumerable<IWorkspaceService> workspaceServices, IEnumerable<ILanguageService> razorLanguageServices)
    {
        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        if (razorLanguageServices is null)
        {
            throw new ArgumentNullException(nameof(razorLanguageServices));
        }

        _workspaceServices = workspaceServices;
        _razorLanguageServices = razorLanguageServices;
    }

    protected override HostWorkspaceServices CreateWorkspaceServices(Workspace workspace)
    {
        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        return new TestWorkspaceServices(this, _workspaceServices, _razorLanguageServices, workspace);
    }

    public static HostServices Create(IEnumerable<ILanguageService> razorLanguageServices)
        => Create(Enumerable.Empty<IWorkspaceService>(), razorLanguageServices);

    public static HostServices Create(IEnumerable<IWorkspaceService> workspaceServices, IEnumerable<ILanguageService> razorLanguageServices)
        => new TestServices(workspaceServices, razorLanguageServices);
}
