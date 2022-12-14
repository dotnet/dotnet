// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Tests;
using OLEDB.Test.ModuleCore;

namespace System.Xml.ReaderSettingsTests
{
    [TestCase(Name = "LineInfo", Desc = "LineInfo")]
    public partial class TCLineInfo : TCXMLReaderBaseGeneral
    {
        [Variation("Line Number Offset negative values", Pri = 1)]
        public int ln02()
        {
            XmlReaderSettings rs = new XmlReaderSettings();
            rs.LineNumberOffset = -1;
            return TEST_PASS;
        }

        [Variation("Line Position Offset negative values", Pri = 1)]
        public int lp02()
        {
            XmlReaderSettings rs = new XmlReaderSettings();
            rs.LinePositionOffset = -1;
            return TEST_PASS;
        }
    }
}
