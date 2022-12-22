// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.CodeAnalysis;

public static class TestWorkspace
{
    private static readonly object s_workspaceLock = new();

    public static Workspace Create(Action<AdhocWorkspace> configure = null) => Create(services: null, configure: configure);

    public static AdhocWorkspace Create(HostServices services, Action<AdhocWorkspace> configure = null)
    {
        lock (s_workspaceLock)
        {
            var workspace = services is null ? new AdhocWorkspace() : new AdhocWorkspace(services);
            configure?.Invoke(workspace);

            return workspace;
        }
    }
}
