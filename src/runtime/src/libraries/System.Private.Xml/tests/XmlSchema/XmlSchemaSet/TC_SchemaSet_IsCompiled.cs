// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Schema;
using Xunit;
using Xunit.Abstractions;

namespace System.Xml.XmlSchemaTests
{
    //[TestCase(Name = "TC_SchemaSet_IsCompiled", Desc = "")]
    public class TC_SchemaSet_IsCompiled : TC_SchemaSetBase
    {
        private ITestOutputHelper _output;

        public TC_SchemaSet_IsCompiled(ITestOutputHelper output)
        {
            _output = output;
        }


        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v1 - IsCompiled on empty collection", Priority = 0)]
        [Fact]
        public void v1()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            CError.Compare(sc.IsCompiled, false, "IsCompiled");
            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v2 - IsCompiled, add one, Compile, IsCompiled, add one IsCompiled", Priority = 0)]
        [Fact]
        public void v2()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add("xsdauthor1", TestData._XsdNoNs);

            CError.Compare(sc.IsCompiled, false, "IsCompiled after first add");

            sc.Compile();

            CError.Compare(sc.IsCompiled, true, "IsCompiled after compile");

            sc.Add("xsdauthor", TestData._XsdAuthor);

            CError.Compare(sc.IsCompiled, false, "IsCompiled after seciond add");

            sc.Compile();

            CError.Compare(sc.IsCompiled, true, "IsCompiled after second compile");

            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v3 - Add two, Compile, remove one, IsCompiled")]
        [Fact]
        public void v3()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add("xsdauthor1", TestData._XsdNoNs);
            XmlSchema Schema1 = sc.Add("xsdauthor", TestData._XsdAuthor);
            sc.Compile();
            sc.Remove(Schema1);

            CError.Compare(sc.IsCompiled, false, "IsCompiled after compiled and remove");

            return;
        }
    }
}
