// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

/// <summary>
/// A work item that represents a unit of work. This is intended to be overridden so consumers can represent any
/// unit of work that fits their need.
/// </summary>
internal abstract class BatchableWorkItem
{
    /// <summary>
    /// Processes a unit of work.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token for the unit of work</param>
    /// <returns>A task</returns>
    public abstract ValueTask ProcessAsync(CancellationToken cancellationToken);
}
