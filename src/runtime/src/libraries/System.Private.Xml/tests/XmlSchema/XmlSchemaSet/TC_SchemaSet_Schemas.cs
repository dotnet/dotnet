// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Xml.Schema;
using Xunit;
using Xunit.Abstractions;

namespace System.Xml.XmlSchemaTests
{
    //[TestCase(Name = "TC_SchemaSet_Schemas", Desc = "")]
    public class TC_SchemaSet_Schemas : TC_SchemaSetBase
    {
        private ITestOutputHelper _output;

        public TC_SchemaSet_Schemas(ITestOutputHelper output)
        {
            _output = output;
        }


        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v1 - Schemas on empty collection, all members of ICollection")]
        [Fact]
        public void v1()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            ICollection Col = sc.Schemas();

            CError.Compare(Col.Count, 0, "Count");
            CError.Compare(Col.IsSynchronized, false, "IsSynchronized");

            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v2 - Schemas on non empty collection, all members of ICollection", Priority = 0)]
        [Fact]
        public void v2()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            XmlSchema Schema1 = sc.Add("xsdauthor1", TestData._XsdNoNs);
            XmlSchema Schema2 = sc.Add("xsdauthor", TestData._XsdAuthor);
            ICollection Col = sc.Schemas();

            CError.Compare(Col.Count, 2, "Count");

            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v3 - Schemas on non empty collection, use in foreach", Priority = 0)]
        [Fact]
        public void v3()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            XmlSchema Schema1 = sc.Add("xsdauthor1", TestData._XsdNoNs);
            XmlSchema Schema2 = sc.Add("xsdauthor", TestData._XsdAuthor);
            ICollection Col = sc.Schemas();

            CError.Compare(Col.Count, 2, "Count");
            XmlSchema[] Schemas = new XmlSchema[2];
            sc.CopyTo(Schemas, 0);

            int i = 0;
            foreach (XmlSchema Schema in Col)
            {
                CError.Compare(Schema, Schemas[i], "Schema");
                i++;
            }

            return;
        }

        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v4 - Schemas on non empty collection,call Schemas,Edit check all members of ICollection")]
        [Fact]
        public void v4()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            XmlSchema Schema1 = sc.Add("xsdauthor1", TestData._XsdNoNs);
            XmlSchema Schema2 = sc.Add("xsdauthor", TestData._XsdAuthor);
            ICollection Col = sc.Schemas();
            sc.Remove(Schema1);

            CError.Compare(Col.Count, 1, "Count");
            foreach (XmlSchema Schema in Col)
            {
                CError.Compare(Schema, Schema2, "Schema");
            }

            return;
        }
    }
}
