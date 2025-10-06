// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// Handles project changes and notifies listeners of project updates and removal.
/// </summary>
internal interface IRazorProjectInfoDriver
{
    Task WaitForInitializationAsync();

    ImmutableArray<RazorProjectInfo> GetLatestProjectInfo();

    void AddListener(IRazorProjectInfoListener listener);
}
