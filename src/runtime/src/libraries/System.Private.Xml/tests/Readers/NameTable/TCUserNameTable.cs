// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using OLEDB.Test.ModuleCore;

namespace System.Xml.NameTableTests
{
    public partial class TCUserNameTable : CTestCase
    {
        // Type is NameTableTest.TCUserNameTable
        // Test Case
        public override void AddChildren()
        {
            // for function v1
            {
                this.AddChild(new CVariation(v1) { Attribute = new Variation("Read xml file using custom name table") });
            }
        }
    }
}
