// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Text
{
    [TestClass]
    public class StringTextProviderTests
    {
        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(0, 9, 9)]
        [DataRow(1, 0, 12)]
        [DataRow(1, 9, 21)]
        [DataRow(2, 0, 24)]
        [DataRow(2, 9, 33)]
        public void VerifyGetPositionFromLineAndColumn(int line, int column, int expectedPosition)
        {
            string text =
@"0123456789
abcefghijk
!@#$%^&*()";

            StringTextProvider stringTextProvider = new StringTextProvider(text);
            int actualPosition = stringTextProvider.GetPositionFromLineAndColumn(line, column);

            Assert.AreEqual(expectedPosition, actualPosition);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(0, 9, 9)]
        [DataRow(1, 0, 12)]
        [DataRow(1, 3, 15)]
        [DataRow(2, 0, 17)]
        [DataRow(3, 0, 19)]
        [DataRow(4, 0, 20)]
        public void VerifyGetPositionFromLineAndColumnEdgeCases(int line, int column, int expectedPosition)
        {
            // 5 lines with various column count (10, 4, 0, 0, 6).
            // Various line endings (\r\n, \r, \r\n, \n)
            string text = "0123456789\r\n0123\r\r\n\n012345";

            StringTextProvider stringTextProvider = new StringTextProvider(text);
            int actualPosition = stringTextProvider.GetPositionFromLineAndColumn(line, column);

            Assert.AreEqual(expectedPosition, actualPosition);
        }

    }
}
