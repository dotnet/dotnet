// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IRazorProjectInfoListener
{
    Task RemovedAsync(ProjectKey projectKey, CancellationToken cancellationToken);
    Task UpdatedAsync(RazorProjectInfo projectInfo, CancellationToken cancellationToken);
}
