// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal static class Extensions
{
    internal static async Task<string?> GetProjectAvailabilityTextAsync(
        this IComponentAvailabilityService componentAvailabilityService,
        string documentFilePath,
        string tagHelperTypeName,
        CancellationToken cancellationToken)
    {
        var projects = await componentAvailabilityService
            .GetComponentAvailabilityAsync(documentFilePath, tagHelperTypeName, cancellationToken)
            .ConfigureAwait(false);

        if (projects.IsEmpty)
        {
            return null;
        }

        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        foreach (var (project, isAvailable) in projects)
        {
            if (isAvailable)
            {
                continue;
            }

            if (builder.Length == 0)
            {
                builder.AppendLine();
                builder.Append($"⚠️ {SR.Not_Available_In}:");
            }

            builder.AppendLine();
            builder.Append("    ");
            builder.Append(project.DisplayName);
        }

        if (builder.Length == 0)
        {
            return null;
        }

        return builder.ToString();
    }
}
