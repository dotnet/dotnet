// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Razor.Diagnostics.Analyzers;

internal static class WellKnownTypeNames
{
    public const string PooledArrayBuilderExtensions = "Microsoft.AspNetCore.Razor.PooledObjects.PooledArrayBuilderExtensions";

    public const string IRemoteJsonService = "Microsoft.CodeAnalysis.Razor.Remote.IRemoteJsonService";
    public const string RazorPinnedSolutionInfoWrapper = "Microsoft.CodeAnalysis.ExternalAccess.Razor.RazorPinnedSolutionInfoWrapper";
    public const string DocumentId = "Microsoft.CodeAnalysis.DocumentId";
}
