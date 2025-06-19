// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Text
{

    [TestClass]
    public class TextHelperTests
    {
        [TestMethod]
        public void Test_TextHelper_GetNewLineCount()
        {
            // 1 = \r
            // 2 = \r\n
            // 3 = \r
            // 4 = \r\n
            // 5 = \n
            // 6 = \r\n
            string text = "\r\r\n\r\r\n\n\r\n";

            int newLineCount = TextHelper.GetNewLineCount(text);

            Assert.AreEqual<int>(6, newLineCount);
        }
    }
}
