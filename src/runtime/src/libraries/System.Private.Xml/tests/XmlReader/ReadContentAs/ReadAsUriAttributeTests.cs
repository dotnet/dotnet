// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlReaderTests
{
    public class UriAttributeTests
    {
        [Fact]
        public static void ReadContentAsTypeOfUriAttribute1()
        {
            var reader = Utils.CreateFragmentReader("<a b=' http://wddata '> http://wddata </a>");
            reader.PositionOnElement("a");
            reader.MoveToAttribute("b");
            Assert.Equal(new Uri("http://wddata/"), reader.ReadContentAs(typeof(Uri), null));
        }

        [Fact]
        public static void ReadContentAsTypeOfUriAttribute2()
        {
            var reader = Utils.CreateFragmentReader("<a b='    '>    </a>");
            reader.PositionOnElement("a");
            reader.MoveToAttribute("b");
            Assert.Throws<XmlException>(() => reader.ReadContentAs(typeof(Uri), null));
        }
    }
}
