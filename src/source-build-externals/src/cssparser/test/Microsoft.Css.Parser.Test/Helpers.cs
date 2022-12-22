// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;

namespace Microsoft.Css.Parser.Test
{
    internal static class Helpers
    {
        static public string LoadFileAsString(string name)
        {
            StreamReader sr = new StreamReader(name);
            string s = sr.ReadToEnd();
            sr.Close();
            return s;
        }

        static public TokenList MakeTokens(string text)
        {
            return MakeTokens(new StringTextProvider(text));
        }

        static public TokenList MakeTokens(ITextProvider textProvider)
        {
            CssTokenizer tokenizer = new CssTokenizer();
            TokenList tokens = tokenizer.Tokenize(textProvider, 0, textProvider.Length, keepWhiteSpace: false);
            return tokens;
        }

        static public TokenStream MakeTokenStream(string text)
        {
            return new TokenStream(MakeTokens(text));
        }

        static public TokenStream MakeTokenStream(ITextProvider textProvider)
        {
            return new TokenStream(MakeTokens(textProvider));
        }

        static public StyleSheet MakeStyleSheet(string text)
        {
            CssParser parser = new CssParser();
            return parser.Parse(text, insertComments: true);
        }
    }
}
