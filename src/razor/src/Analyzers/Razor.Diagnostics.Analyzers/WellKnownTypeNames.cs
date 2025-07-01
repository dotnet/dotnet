// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Razor.Diagnostics.Analyzers;

internal static class WellKnownTypeNames
{
    public const string PooledArrayBuilderExtensions = "Microsoft.AspNetCore.Razor.PooledObjects.PooledArrayBuilderExtensions";

    public const string IRemoteJsonService = "Microsoft.CodeAnalysis.Razor.Remote.IRemoteJsonService";
    public const string RazorPinnedSolutionInfoWrapper = "Microsoft.CodeAnalysis.ExternalAccess.Razor.RazorPinnedSolutionInfoWrapper";
    public const string DocumentId = "Microsoft.CodeAnalysis.DocumentId";
}
