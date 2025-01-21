// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal abstract class AbstractComponentAvailabilityService : IComponentAvailabilityService
{
    public async Task<ImmutableArray<(IProjectSnapshot Project, bool IsAvailable)>> GetComponentAvailabilityAsync(
        string documentFilePath,
        string typeName,
        CancellationToken cancellationToken)
    {
        var projects = GetProjectsContainingDocument(documentFilePath);
        if (projects.IsEmpty)
        {
            return [];
        }

        using var result = new PooledArrayBuilder<(IProjectSnapshot, bool IsAvailable)>(capacity: projects.Length);

        foreach (var project in projects)
        {
            var containsTagHelper = await ContainsTagHelperAsync(project, typeName, cancellationToken).ConfigureAwait(false);

            result.Add((project, IsAvailable: containsTagHelper));
        }

        return result.DrainToImmutable();
    }

    protected abstract ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath);

    private static async Task<bool> ContainsTagHelperAsync(
        IProjectSnapshot projectSnapshot,
        string typeName,
        CancellationToken cancellationToken)
    {
        var tagHelpers = await projectSnapshot.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);

        foreach (var tagHelper in tagHelpers)
        {
            if (tagHelper.GetTypeName() == typeName)
            {
                return true;
            }
        }

        return false;
    }
}
