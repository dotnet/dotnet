// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    public class CommandTokenizer : ICommandTokenizer
    {
        public IEnumerable<Token> Tokenize(string[] lines)
        {
            Collection<PSParseError> errors;
            Collection<PSToken> tokens = PSParser.Tokenize(lines, out errors);
            return tokens.Select(t => new Token(
                MapTokenType(t.Type), t.StartLine, t.EndLine, t.StartColumn, t.EndColumn));
        }

        private static readonly TokenType[] _tokenTypes =
            {
                /* Unknown = 0,             */ TokenType.Other,
                /* Command = 1,             */ TokenType.FormalLanguage,
                /* CommandParameter = 2,    */ TokenType.Other,
                /* CommandArgument = 3,     */ TokenType.Other,
                /* Number = 4,              */ TokenType.NumberLiteral,
                /* String = 5,              */ TokenType.StringLiteral,
                /* Variable = 6,            */ TokenType.Identifier,
                /* Member = 7,              */ TokenType.Identifier,
                /* LoopLabel = 8,           */ TokenType.Literal,
                /* Attribute = 9,           */ TokenType.SymbolReference,
                /* Type = 10,               */ TokenType.SymbolReference,
                /* Operator = 11,           */ TokenType.Operator,
                /* GroupStart = 12,         */ TokenType.Operator,
                /* GroupEnd = 13,           */ TokenType.Operator,
                /* Keyword = 14,            */ TokenType.Keyword,
                /* Comment = 15,            */ TokenType.Comment,
                /* StatementSeparator = 16, */ TokenType.Other,
                /* NewLine = 17,            */ TokenType.Other,
                /* LineContinuation = 18,   */ TokenType.Other,
                /* Position = 19,           */ TokenType.Operator
            };

        private static TokenType MapTokenType(PSTokenType psTokenType)
        {
            int i = (int)psTokenType;
            return (i >= 0 && i < _tokenTypes.Length) ? _tokenTypes[i] : TokenType.Other;
        }
    }
}
