// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Xml.Schema;
using Xunit;
using Xunit.Abstractions;

namespace System.Xml.XmlSchemaTests
{
    //[TestCase(Name = "TC_SchemaSet_Count", Desc = "", Priority = 0)]
    public class TC_SchemaSet_Count : TC_SchemaSetBase
    {
        private ITestOutputHelper _output;

        public TC_SchemaSet_Count(ITestOutputHelper output)
        {
            _output = output;
        }


        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v1 - Count on empty collection", Priority = 1)]
        [Fact]
        public void v1()
        {
            XmlSchemaSet sc = new XmlSchemaSet();

            CError.Compare(sc.Count, 0, "Count");

            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v2 - Add two schemas for same ns and Count")]
        [Fact]
        public void v2()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add("xsdauthor", TestData._XsdNoNs);
            sc.Add("xsdauthor", TestData._XsdAuthor);

            CError.Compare(sc.Count, 2, "Count");

            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v3 - Add two schemas for diff ns with imports/includes and Count")]
        [Fact]
        public void v3()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add("xsdauthor", TestData._XsdAuthor);
            sc.Add(null, Path.Combine(TestData._Root, "xsdbookexternal.xsd"));

            CError.Compare(sc.Count, 2, "Count");

            return;
        }
    }
}
