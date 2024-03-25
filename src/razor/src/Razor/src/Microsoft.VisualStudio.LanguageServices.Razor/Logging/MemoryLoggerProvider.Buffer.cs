// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

internal partial class MemoryLoggerProvider
{
    /// <summary>
    /// A circular in memory buffer to store logs in memory.
    /// </summary>
    private class Buffer(int bufferSize)
    {
        private string[] _memory = new string[bufferSize];

        // Start at -1 because append always increments, so we want to start at value 0
        private int _head = -1;

        public void Append(string s)
        {
            var position = Math.Abs(Interlocked.Increment(ref _head) % _memory.Length);
            _memory[position] = s;
        }
    }
}
