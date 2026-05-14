// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Arcade.Common
{
    public static class ArgumentEscaper
    {
        /// <summary>
        /// Escapes and quote arguments that need it (contain a space, a quote...).
        /// </summary>
        public static string EscapeAndConcatenateArgArrayForProcessStart(IEnumerable<string> args)
        {
            return string.Join(" ", args.Select(EscapeArg));
        }

        /// <summary>
        /// Escapes and quote arguments that need it.
        /// 
        /// This prefixes every character with the '^' character to force cmd to
        /// interpret the argument string literally. An alternative option would 
        /// be to do this only for cmd metacharacters.
        /// 
        /// See here for more info:
        /// http://blogs.msdn.com/b/twistylittlepassagesallalike/archive/2011/04/23/everyone-quotes-arguments-the-wrong-way.aspx
        /// </summary>
        public static string EscapeAndConcatenateArgArrayForCmdProcessStart(IEnumerable<string> args)
        {
            return string.Join(" ", args.Select(EscapeArgForCmd));
        }

        private static string EscapeArg(string argument)
        {
            var sb = new StringBuilder();

            // Don't quote already quoted strings
            bool quoted = IsQuoted(argument);
            var shouldQuote = !quoted && ShouldSurroundWithQuotes(argument);
            if (shouldQuote || quoted)
            {
                sb.Append("\"");
            }

            for (int i = 0; i < argument.Length; ++i)
            {
                if (quoted && (i == 0 || i == argument.Length - 1))
                {
                    continue;
                }

                var backslashCount = 0;

                // Consume All Backslashes
                while (i < argument.Length && argument[i] == '\\')
                {
                    backslashCount++;
                    i++;
                }

                // Escape any backslashes at the end of the arg
                // This ensures the outside quote is interpreted as
                // an argument delimiter
                if (i == argument.Length)
                {
                    sb.Append('\\', 2 * backslashCount);
                }

                // Escape any preceding backslashes and the quote
                else if (argument[i] == '"')
                {
                    sb.Append('\\', (2 * backslashCount) + 1);
                    sb.Append('"');
                }

                // Output any consumed backslashes and the character
                else
                {
                    sb.Append('\\', backslashCount);
                    sb.Append(argument[i]);
                }
            }

            if (shouldQuote || quoted)
            {
                sb.Append("\"");
            }

            return sb.ToString();
        }

        private static string EscapeArgForCmd(string argument)
        {
            var sb = new StringBuilder();

            var quoted = IsQuoted(argument);
            var shouldQuote = !quoted && ShouldSurroundWithQuotes(argument);

            if (shouldQuote)
            {
                sb.Append("^\"");
            }

            foreach (var character in argument)
            {
                if (character == '"')
                {
                    sb.Append('^');
                    sb.Append('"');
                    sb.Append('^');
                    sb.Append(character);
                }
                else
                {
                    sb.Append("^");
                    sb.Append(character);
                }
            }

            if (shouldQuote)
            {
                sb.Append("^\"");
            }

            return sb.ToString();
        }

        private static bool ShouldSurroundWithQuotes(string argument)
        {
            // Only quote if whitespace exists in the string
            return argument.Contains(' ') || argument.Contains('\t') || argument.Contains('\n') || argument.Contains('"');
        }


        private static bool IsQuoted(string argument)
        {
            return argument.Length > 1 && (argument[0] == '\"' || argument[argument.Length - 1] == '\"');
        }
    }
}
