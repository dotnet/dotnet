// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

namespace NuGetConsole
{
    /// <summary>
    /// A token type used by command tokenizer.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// This token type.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1721:PropertyNamesShouldNotMatchGetMethods",
            Justification = "Type is the most appropriate word here.")]
        public TokenType Type { get; private set; }

        /// <summary>
        /// This token's start line.
        /// </summary>
        public int StartLine { get; private set; }

        /// <summary>
        /// This token's end line.
        /// </summary>
        public int EndLine { get; private set; }

        /// <summary>
        /// This token's start column.
        /// </summary>
        public int StartColumn { get; private set; }

        /// <summary>
        /// This token's end column.
        /// </summary>
        public int EndColumn { get; private set; }

        /// <summary>
        /// Create a new token.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="startLine">The token's start line.</param>
        /// <param name="endLine">The token's end line.</param>
        /// <param name="startColumn">The token's start column.</param>
        /// <param name="endColumn">The token's end column.</param>
        public Token(TokenType type, int startLine, int endLine, int startColumn, int endColumn)
        {
            this.Type = type;
            this.StartLine = startLine;
            this.EndLine = endLine;
            this.StartColumn = startColumn;
            this.EndColumn = endColumn;
        }
    }
}
