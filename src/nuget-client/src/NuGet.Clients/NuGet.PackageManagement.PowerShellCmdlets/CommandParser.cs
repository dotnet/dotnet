// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace NuGetConsole.Host.PowerShell
{
    /// <summary>
    /// A simple parser used for parsing commands that require completion (intellisense).
    /// </summary>
    public class CommandParser
    {
        private int _index;
        private readonly string _command;
        private readonly char[] _escapeChars = { 'n', 'r', 't', 'a', 'b', '"', '\'', '`', '0' };

        private CommandParser(string command)
        {
            _command = command;
        }

        private char CurrentChar
        {
            get { return GetChar(_index); }
        }

        private char NextChar
        {
            get { return GetChar(_index + 1); }
        }

        private bool Done
        {
            get { return _index >= _command.Length; }
        }

        public static Command Parse(string command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            return new CommandParser(command).ParseCore();
        }

        private Command ParseCore()
        {
            Collection<PSParseError> errors;
            Collection<PSToken> tokens = PSParser.Tokenize(_command, out errors);

            // Use the powershell tokenizer to find the index of the last command so we can start parsing from there
            var lastCommandToken = tokens.LastOrDefault(t => t.Type == PSTokenType.Command);

            if (lastCommandToken != null)
            {
                // Start parsing from a command
                _index = lastCommandToken.Start;
            }

            var parsedCommand = new Command();
            int positionalArgumentIndex = 0;

            // Get the command name
            parsedCommand.CommandName = ParseToken();

            while (!Done)
            {
                SkipWhitespace();

                string argument = ParseToken();

                if (argument.Length > 0
                    && argument[0] == '-')
                {
                    // Trim the -
                    argument = argument.Substring(1);

                    if (!String.IsNullOrEmpty(argument))
                    {
                        // Parse the argument value if any
                        if (SkipWhitespace()
                            && CurrentChar != '-')
                        {
                            parsedCommand.Arguments[argument] = ParseToken();
                        }
                        else
                        {
                            parsedCommand.Arguments[argument] = null;
                        }

                        parsedCommand.CompletionArgument = argument;
                    }
                    else
                    {
                        // If this was an empty argument then we aren't trying to complete anything
                        parsedCommand.CompletionArgument = null;
                    }

                    // Reset the completion index if we're completing an argument (these 2 properties are mutually exclusive)
                    parsedCommand.CompletionIndex = null;
                }
                else
                {
                    // Reset the completion argument
                    parsedCommand.CompletionArgument = null;
                    parsedCommand.CompletionIndex = positionalArgumentIndex;
                    parsedCommand.Arguments[positionalArgumentIndex++] = argument;
                }
            }

            return parsedCommand;
        }

        private string ParseSingleQuotes()
        {
            var sb = new StringBuilder();
            while (!Done)
            {
                sb.Append(ParseUntil(c => c == '\''));

                if (ParseChar() == '\''
                    && CurrentChar == '\'')
                {
                    sb.Append(ParseChar());
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }

        private string ParseDoubleQuotes()
        {
            var sb = new StringBuilder();
            while (!Done)
            {
                // Parse until we see a quote or an escape character
                sb.Append(ParseUntil(c => c == '"' || c == '`'));

                if (IsEscapeSequence())
                {
                    sb.Append(ParseChar());
                    sb.Append(ParseChar());
                }
                else
                {
                    ParseChar();
                    break;
                }
            }

            return sb.ToString();
        }

        private bool IsEscapeSequence()
        {
            return CurrentChar == '`' && Array.IndexOf(_escapeChars, NextChar) >= 0;
        }

        private char ParseChar()
        {
            char ch = CurrentChar;
            _index++;
            return ch;
        }

        private string ParseToken()
        {
            if (CurrentChar == '\'')
            {
                ParseChar();
                return ParseSingleQuotes();
            }
            if (CurrentChar == '"')
            {
                ParseChar();
                return ParseDoubleQuotes();
            }
            return ParseUntil(Char.IsWhiteSpace);
        }

        private string ParseUntil(Func<char, bool> predicate)
        {
            var sb = new StringBuilder();
            while (!Done
                   && !predicate(CurrentChar))
            {
                sb.Append(CurrentChar);
                _index++;
            }
            return sb.ToString();
        }

        private bool SkipWhitespace()
        {
            string ws = ParseUntil(c => !Char.IsWhiteSpace(c));
            return ws.Length > 0;
        }

        private char GetChar(int index)
        {
            return index < _command.Length ? _command[index] : '\0';
        }
    }
}
