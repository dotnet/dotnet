// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Schema;
using Xunit;
using Xunit.Abstractions;

namespace System.Xml.XmlSchemaTests
{
    //[TestCase(Name = "TC_SchemaSet_XmlNameTable", Desc = "")]
    public class TC_SchemaSet_XmlNameTable : TC_SchemaSetBase
    {
        private ITestOutputHelper _output;

        public TC_SchemaSet_XmlNameTable(ITestOutputHelper output)
        {
            _output = output;
        }


        //-----------------------------------------------------------------------------------
        //[Variation(Desc = "v1 - Get nametable", Priority = 0)]
        [Fact]
        public void v1()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            XmlNameTable NT = sc.NameTable;
            CError.Compare(NT != null, true, "NameTable");
            return;
        }
    }
}
