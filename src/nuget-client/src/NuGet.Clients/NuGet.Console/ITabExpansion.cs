// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGetConsole
{
    /// <summary>
    /// Simple (line, lastWord) based tab expansion interface. A host can implement
    /// this interface and reuse CommandExpansion/CommandExpansionProvider.
    /// </summary>
    public interface ITabExpansion
    {
        Task<string[]> GetExpansionsAsync(string line, string lastWord, CancellationToken token);
    }
}
