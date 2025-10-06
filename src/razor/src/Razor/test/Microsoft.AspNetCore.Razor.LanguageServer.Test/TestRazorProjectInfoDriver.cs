// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal sealed class TestRazorProjectInfoDriver : IRazorProjectInfoDriver
{
    public static readonly TestRazorProjectInfoDriver Instance = new();

    public void AddListener(IRazorProjectInfoListener listener)
    {
    }

    public ImmutableArray<RazorProjectInfo> GetLatestProjectInfo()
    {
        return [];
    }

    public Task WaitForInitializationAsync()
    {
        return Task.CompletedTask;
    }
}
