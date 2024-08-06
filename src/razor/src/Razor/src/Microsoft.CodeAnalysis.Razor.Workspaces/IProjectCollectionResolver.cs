// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IProjectCollectionResolver
{
    IEnumerable<IProjectSnapshot> EnumerateProjects(IDocumentSnapshot snapshot);
}
