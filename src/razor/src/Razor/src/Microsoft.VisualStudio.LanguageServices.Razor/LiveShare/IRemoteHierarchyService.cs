// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LiveShare;

namespace Microsoft.VisualStudio.Razor.LiveShare;

// This type must be a public interface in order to to be implemented as an RPC proxy by live share.
public interface IRemoteHierarchyService : ICollaborationService
{
    Task<bool> HasCapabilityAsync(Uri pathOfFileInProject, string capability, CancellationToken cancellationToken);
}
