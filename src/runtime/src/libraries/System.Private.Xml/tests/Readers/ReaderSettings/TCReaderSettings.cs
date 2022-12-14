// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Tests;
using OLEDB.Test.ModuleCore;

namespace System.Xml.ReaderSettingsTests
{
    public partial class TCReaderSettings : TCXMLReaderBaseGeneral
    {
        // Type is System.Xml.Tests.TCReaderSettings
        // Test Case
        public override void DOTEST()
        {
            base.DOTEST();
            CurVariation = new CVariation(this);
        }
    }
}
