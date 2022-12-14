// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlReaderTests
{
    public class TimeSpanAttributeTests
    {
        [Fact]
        public static void ReadContentAsTimeSpanAttribute1()
        {
            var reader = Utils.CreateFragmentReader("<Root a='   PT0S  '/>");
            reader.PositionOnElement("Root");
            reader.MoveToAttribute("a");
            Assert.Equal("00:00:00", reader.ReadContentAs(typeof(TimeSpan), null).ToString());
        }
    }
}
