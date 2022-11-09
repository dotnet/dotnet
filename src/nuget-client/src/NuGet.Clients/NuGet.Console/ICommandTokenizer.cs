// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGetConsole
{
    /// <summary>
    /// Interface for command line tokenizer (syntax highlighting).
    /// </summary>
    public interface ICommandTokenizer
    {
        /// <summary>
        /// Tokenize a sequence of command lines.
        /// </summary>
        /// <param name="lines">The command lines.</param>
        /// <returns>A sequence of Tokens.</returns>
        IEnumerable<Token> Tokenize(string[] lines);
    }
}
