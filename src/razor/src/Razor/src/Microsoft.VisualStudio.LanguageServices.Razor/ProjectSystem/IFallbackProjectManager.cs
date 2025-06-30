// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

internal interface IFallbackProjectManager
{
    bool IsFallbackProject(ProjectKey projectKey);
}
