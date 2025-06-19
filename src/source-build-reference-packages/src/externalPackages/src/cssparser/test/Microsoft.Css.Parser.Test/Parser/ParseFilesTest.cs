// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Css.Parser.Test;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Parser
{
    [TestClass]
    public class ParseFilesTest : CssUnitTestBase
    {
        // change to true in debugger if you want all baseline tree files regenerated
        private static readonly bool s_regenerateBaselineFiles = false;

        [TestMethod]
        [DeploymentItem(@"Files\001", @"Files\001")]
        public void ParseFilesTest_Files001()
        {
            TestFilesInFolder(@"Files\001");
        }

        [TestMethod]
        [DeploymentItem(@"Files\090")]
        public void ParseFilesTest_LargeFiles()
        {
            TestFilesInFolder(@"Files\090");
        }

        private void TestFilesInFolder(string folder)
        {
            foreach (string  name in Directory.GetFiles(folder, "*.css"))
            {
                ParseFile(name);
            }
        }

#if TEST_MEM_USAGE
        // Keep a running total of memory usage for all tests
        private static long _totalMemParseFile = 0;
        private static long _totalCharsParseFile = 0;
        private static long _totalItemsParseFile = 0;
        private static long _totalTokenizeMS = 0;
        private static long _totalParseMS = 0;
#endif

        public void ParseFile(string name)
        {
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;
            string file = name + ".tree";

            try
            {
                string text = Helpers.LoadFileAsString(name);
                ITextProvider textProvider = new StringTextProvider(text);
                CssParser parser = new CssParser();

#if TEST_MEM_USAGE
                long startMem = GC.GetTotalMemory(true);
                long totalTokenizeMS = 0;
                long totalParseMS = 0;

                StyleSheet[] styleSheets = new StyleSheet[32];
                for (int i = 0; i < styleSheets.Length; i++)
                {
                    styleSheets[i] = parser.Parse(textProvider, insertComments: true);

                    totalTokenizeMS += parser.LastTokenizeMilliseconds;
                    totalParseMS += parser.LastParseMilliseconds;
                }

                StyleSheet styleSheet = styleSheets[0];
                long items = 0;

                styleSheet.Accept(new CssTreeVisitor((ParseItem) =>
                {
                    items++;
                    return VisitItemResult.Continue;
                }));

                long endMem = GC.GetTotalMemory(true);
                long totalMem = endMem - startMem;
                long avgMem = totalMem / styleSheets.Length;

                totalTokenizeMS /= styleSheets.Length;
                totalParseMS /= styleSheets.Length;

                _totalMemParseFile += avgMem;
                _totalCharsParseFile += text.Length;
                _totalItemsParseFile += items;
                _totalTokenizeMS += totalTokenizeMS;
                _totalParseMS += totalParseMS;

                System.Diagnostics.Debug.WriteLine("CSS parse file: {0}", file);
                System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------------");
                System.Diagnostics.Debug.WriteLine("    Mem usage: {0} bytes for {1} chars ({2} bytes per char)", avgMem, text.Length, avgMem / text.Length);
                System.Diagnostics.Debug.WriteLine("    Mem usage: {0} bytes for {1} items ({2} bytes per item)", avgMem, items, avgMem / items);
                System.Diagnostics.Debug.WriteLine("    Perf: Tokenize:{0}ms, Parse:{1}ms, Total:{2}ms", totalTokenizeMS, totalParseMS, totalTokenizeMS + totalParseMS);

                System.Diagnostics.Debug.WriteLine(
                    "    Running mem usage average: {0} bytes per char. {1} bytes per item.",
                    _totalMemParseFile / _totalCharsParseFile,
                    _totalMemParseFile / _totalItemsParseFile);
#else
                StyleSheet styleSheet = parser.Parse(text, insertComments: true);
#endif

                DebugWriter debugWriter = new DebugWriter();
                string actual = debugWriter.Serialize(textProvider, styleSheet);

                if (s_regenerateBaselineFiles)
                {
                    if (File.Exists(file))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }

                    streamWriter = new StreamWriter(file);
                    streamWriter.Write(actual);
                    streamWriter.Close();
                    streamWriter = null;
                }
                else
                {
                    streamReader = new StreamReader(file);
                    string expected = streamReader.ReadToEnd();
                    streamReader.Close();
                    streamReader = null;

                    // trim whitescpase in the end to avoid false positives b/c file
                    // has extra line break or whitespace at the end.
                    expected = expected.TrimEnd(new char[] { ' ', '\r', '\n', '\t' });
                    actual = actual.TrimEnd(new char[] { ' ', '\r', '\n', '\t' });

                    Assert.AreEqual(expected, actual);
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(string.Format("Test {0} has thrown an exception: {1}", name.Substring(name.LastIndexOf('\\') + 1), exception.Message));
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

        [TestMethod]
        public void Stylesheet_ParseNestedTest()
        {
            string text = "@media print { .a { color:red; } }";
            ITextProvider tp = new StringTextProvider(text);
            TokenStream tokens = Helpers.MakeTokenStream(tp);
            tokens.Position = 3;

            StyleSheet s = new StyleSheet
            {
                IsNestedBlock = true
            };

            Assert.IsTrue(s.Parse(new ItemFactory(tp, null), tp, tokens));
            Assert.AreEqual(34, s.AfterEnd);
            Assert.IsTrue(s.Children[0] is TokenItem);
            Assert.IsTrue(s.Children[1] is RuleSet);
            Assert.IsTrue(s.Children[2] is TokenItem);
            Assert.IsNotNull(s.OpenCurlyBrace);
            Assert.IsNotNull(s.CloseCurlyBrace);
        }

        [TestMethod]
        public void Stylesheet_ParseNotNestedTest()
        {
            // http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems?id=238231 - The root stylesheet must not cache child curly braces
            string text = ".foo } { } {";
            StyleSheet s = Helpers.MakeStyleSheet(text);

            Assert.IsNull(s.OpenCurlyBrace);
            Assert.IsNull(s.CloseCurlyBrace);
        }
    }
}
