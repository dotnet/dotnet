// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Tests;
using OLEDB.Test.ModuleCore;

namespace System.Xml.ReaderSettingsTests
{
    public partial class TCLineInfo : TCXMLReaderBaseGeneral
    {
        // Type is System.Xml.Tests.TCLineInfo
        // Test Case
        public override void AddChildren()
        {
            // for function ln02
            {
                this.AddChild(new CVariation(ln02) { Attribute = new Variation("Line Number Offset negative values") { Pri = 1 } });
            }


            // for function lp02
            {
                this.AddChild(new CVariation(lp02) { Attribute = new Variation("Line Position Offset negative values") { Pri = 1 } });
            }
        }
    }
}
