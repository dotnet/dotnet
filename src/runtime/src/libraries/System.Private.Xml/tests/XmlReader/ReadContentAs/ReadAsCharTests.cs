// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlReaderTests
{
    public class CharTests
    {
        [Fact]
        public static void ReadContentAsChar1()
        {
            var reader = Utils.CreateFragmentReader("<a>2000-02-29T23:59:59+13:60</a>");
            reader.PositionOnElement("a");
            reader.Read();
            Assert.Throws<XmlException>(() => reader.ReadContentAs(typeof(char), null));
        }
    }
}
