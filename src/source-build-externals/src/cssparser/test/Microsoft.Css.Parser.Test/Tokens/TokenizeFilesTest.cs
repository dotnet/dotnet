// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Tokens
{
    [TestClass]
    public class TokenizeFilesTest : CssUnitTestBase
    {
        private static readonly bool s_regenerateTokenFiles = false; // change to true in debugger if you want all token files regenerated

        [TestMethod]
        [DeploymentItem(@"Files\001", @"Files\001")]
        public void TokenizerTest_Files001()
        {
            TestFilesInFolder(@"Files\001");
        }

        [TestMethod]
        [DeploymentItem(@"Files\090", @"Files\090")]
        public void TokenizerTest_LargeFiles()
        {
            TestFilesInFolder(@"Files\090");
        }

        public void TestFilesInFolder(string folder)
        {
            foreach (string name in Directory.GetFiles(folder, "*.css"))
            {
                TokenizeFile(name);
            }
        }

        public void TokenizeFile(string name)
        {
            int lineNumber = 1;
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;

            try
            {
                string cssText = Helpers.LoadFileAsString(name);
                TokenList actual = Helpers.MakeTokens(cssText);
                string tokensString = TokensAsString(new StringTextProvider(cssText), actual);
                string file = name + ".tokens";

                if (s_regenerateTokenFiles)
                {
                    if (File.Exists(file))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }

                    streamWriter = new StreamWriter(file);
                    streamWriter.Write(tokensString);

                    streamWriter.Close();
                    streamWriter = null;
                }
                else
                {
                    streamReader = new StreamReader(file);
                    streamReader.ReadLine(); // first line is a header for readability, so skip it

                    foreach (CssToken token in actual)
                    {
                        string line = streamReader.ReadLine();
                        string[] chunks = line.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        Assert.AreEqual(chunks[0], token.TokenType.ToString());

                        if (token.TokenType != CssTokenType.EndOfFile)
                        {
                            Assert.AreEqual(int.Parse(chunks[1]), token.Length);
                        }

                        lineNumber++;
                    }

                    streamReader.Close();
                    streamReader = null;
                }
            }
            catch (AssertFailedException exception)
            {
                Assert.Fail(string.Format("Test {0} failed, exception {1} at line {2}", name.Substring(name.LastIndexOf('\\') + 1), exception.Message, lineNumber));
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }

        internal static void CompareTokenArrays(CssToken[] expected, TokenList actual)
        {
            Assert.AreEqual(expected.Length, actual.Count);

            for (int i = 0; i < actual.Count && i < expected.Length; i++)
            {
                Assert.AreEqual(actual[i].TokenType, expected[i].TokenType);
                Assert.AreEqual(actual[i].Start, expected[i].Start);
                Assert.AreEqual(actual[i].Length, expected[i].Length);
            }
        }

        private string TokensAsString(ITextProvider textProvider, TokenList tokens)
        {
            StringBuilder sb = new StringBuilder();
            string formatString = "{0,-20} {1,-10} {2}\r\n";
            sb.AppendFormat(formatString, "Token Type", "Length", "Token Text");

            foreach (CssToken token in tokens)
            {
                string tokenText = string.Empty;

                if (token.TokenType != CssTokenType.EndOfFile)
                {
                    tokenText = textProvider.GetText(token.Start, token.Length);
                    tokenText = tokenText.Replace("\r", "\\r").Replace("\n", "\\n");
                }

                sb.AppendFormat(formatString, token.TokenType.ToString(), token.Length, tokenText);
            }

            return sb.ToString();
        }
    }
}
