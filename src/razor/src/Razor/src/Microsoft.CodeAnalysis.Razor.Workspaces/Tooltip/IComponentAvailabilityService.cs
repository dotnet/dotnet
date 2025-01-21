// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal interface IComponentAvailabilityService
{
    /// <summary>
    ///  Returns an array of projects that contain the specified document and whether the
    ///  given component or tag helper type name is available within it.
    /// </summary>
    Task<ImmutableArray<(IProjectSnapshot Project, bool IsAvailable)>> GetComponentAvailabilityAsync(
        string documentFilePath,
        string typeName,
        CancellationToken cancellationToken);
}
