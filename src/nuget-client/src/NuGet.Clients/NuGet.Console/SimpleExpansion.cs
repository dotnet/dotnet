// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGetConsole
{
    /// <summary>
    /// Simple command expansion result.
    /// </summary>
    public class SimpleExpansion
    {
        /// <summary>
        /// Get the Start position for the expansions.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Get the Length from Start position for the expansions.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Get the expansion candidates for intellisense.
        /// </summary>
        public IList<string> Expansions { get; private set; }

        /// <summary>
        /// Create a simple command expansion result.
        /// </summary>
        /// <param name="start">The start position for the expansion.</param>
        /// <param name="length">The length from start position for expansion.</param>
        /// <param name="expansions">Expansion candidates.</param>
        public SimpleExpansion(int start, int length, IList<string> expansions)
        {
            this.Start = start;
            this.Length = length;
            this.Expansions = expansions;
        }
    }
}
