// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Tests;
using OLEDB.Test.ModuleCore;

namespace System.Xml.ReaderSettingsTests
{
    public partial class TCCloseInput : TCXMLReaderBaseGeneral
    {
        // Type is System.Xml.Tests.TCCloseInput
        // Test Case
        public override void AddChildren()
        {
            // for function v1
            {
                this.AddChild(new CVariation(v1) { Attribute = new Variation("Default Values") { Priority = 0 } });
            }
        }
    }
}
