// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGetConsole
{
    /// <summary>
    /// Simple path expansion interface. CommandExpansion tries path expansion
    /// if tab expansion returns no result.
    /// </summary>
    public interface IPathExpansion : ITabExpansion
    {
        Task<SimpleExpansion> GetPathExpansionsAsync(string line, CancellationToken token);
    }
}
