// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class ComponentAvailabilityService(RemoteSolutionSnapshot solutionSnapshot) : AbstractComponentAvailabilityService
{
    private readonly RemoteSolutionSnapshot _solutionSnapshot = solutionSnapshot;

    protected override ImmutableArray<IProjectSnapshot> GetProjectsContainingDocument(string documentFilePath)
        => _solutionSnapshot.GetProjectsContainingDocument(documentFilePath);
}
