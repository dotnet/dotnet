// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.LiveShare.Razor;

// This type must be a public interface in order to to be implemented as an RPC proxy by live share.
public interface IRemoteHierarchyService : ICollaborationService
{
    public Task<bool> HasCapabilityAsync(Uri pathOfFileInProject, string capability, CancellationToken cancellationToken);
}
