// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal interface IProjectWorkspaceStateGenerator
{
    void Update(Project? workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken);
}
