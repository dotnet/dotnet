// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace NuGetConsole
{
    /// <summary>
    /// Interface for command line expansion (intellisense).
    /// </summary>
    public interface ICommandExpansion
    {
        /// <summary>
        /// Get command line expansion candidates.
        /// </summary>
        /// <param name="line">The current input line content.</param>
        /// <param name="caretIndex">The caret position in the input line (starting from 0).</param>
        /// <returns>Command line expansion result.</returns>
        Task<SimpleExpansion> GetExpansionsAsync(string line, int caretIndex, CancellationToken token);
    }
}
