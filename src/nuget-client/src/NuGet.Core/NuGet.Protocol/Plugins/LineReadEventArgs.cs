// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Line read event arguments.
    /// </summary>
    public sealed class LineReadEventArgs : EventArgs
    {
        /// <summary>
        /// The output line read.
        /// </summary>
        public string Line { get; }

        /// <summary>
        /// Instantiates a new <see cref="LineReadEventArgs" /> class.
        /// </summary>
        /// <param name="line">The output line read.</param>
        public LineReadEventArgs(string line)
        {
            Line = line;
        }
    }
}
