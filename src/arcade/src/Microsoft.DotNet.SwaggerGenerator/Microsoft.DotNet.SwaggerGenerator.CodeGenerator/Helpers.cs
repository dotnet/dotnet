using System;
using System.Text;
using Microsoft.DotNet.SwaggerGenerator.Modeler;

namespace Microsoft.DotNet.SwaggerGenerator
{
    public static class Helpers
    {
        /// <summary>
        ///   Get the next 'word' from the string.
        /// </summary>
        /// <param name="value">The string to find words in.</param>
        /// <param name="pos">The current search index in value. This will be updated to the next search index when this function returns.</param>
        /// <remarks>
        ///   A 'word' is the next logical piece of a variable/property/parameter name
        /// </remarks>
        /// <returns>The 'word'</returns>
        private static ReadOnlySpan<char> GetNextWord(ReadOnlySpan<char> value, scoped ref int pos)
        {
            int? wordStart = null;
            for (int idx = pos; idx < value.Length; idx++)
            {
                if (wordStart.HasValue)
                {
                    if (!char.IsLetterOrDigit(value[idx]) || char.IsUpper(value[idx]))
                    {
                        // word is finished, update pos and return the word.
                        pos = idx;
                        return value.Slice(wordStart.Value, idx - wordStart.Value);
                    }
                }
                else
                {
                    // The first letter or digit found marks the start of the word.
                    if (char.IsLetterOrDigit(value[idx]))
                    {
                        wordStart = idx;
                    }
                }
            }

            pos = value.Length;

            // We hit the end of the string, if we started a word return it.
            if (wordStart.HasValue)
            {
                return value.Slice(wordStart.Value);
            }

            return value.Slice(0, 0);
        }

        /// <summary>
        ///   Convert a string into PascalCase
        /// </summary>
        public static string PascalCase(ReadOnlySpan<char> value)
        {
            var builder = new StringBuilder();
            ReadOnlySpan<char> word;
            var pos = 0;
            while ((word = GetNextWord(value, ref pos)).Length != 0)
            {
                for (var i = 0; i < word.Length; i++)
                {
                    char c;
                    if (i == 0)
                    {
                        c = char.ToUpperInvariant(word[i]);
                    }
                    else
                    {
                        c = char.ToLowerInvariant(word[i]);
                    }

                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        ///   Convert a string into camelCase
        /// </summary>
        public static string CamelCase(ReadOnlySpan<char> value)
        {
            var builder = new StringBuilder();
            ReadOnlySpan<char> word;
            var pos = 0;
            var first = true;
            while ((word = GetNextWord(value, ref pos)).Length != 0)
            {
                for (var i = 0; i < word.Length; i++)
                {
                    char c;
                    if (i == 0 && !first)
                    {
                        c = char.ToUpperInvariant(word[i]);
                    }
                    else
                    {
                        c = char.ToLowerInvariant(word[i]);
                    }

                    builder.Append(c);
                }

                first = false;
            }

            return builder.ToString();
        }

        public static string KebabCase(ReadOnlySpan<char> value)
        {
            var builder = new StringBuilder();
            ReadOnlySpan<char> word;
            var pos = 0;
            var first = true;
            while ((word = GetNextWord(value, ref pos)).Length != 0)
            {
                if (!first)
                {
                    builder.Append("-");
                }
                for (var i = 0; i < word.Length; i++)
                {
                    builder.Append(char.ToLowerInvariant(word[i]));
                }

                first = false;
            }

            return builder.ToString();
        }
    }
}
