// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

internal interface IFallbackProjectManager
{
    bool IsFallbackProject(IProjectSnapshot project);
}
