// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using OLEDB.Test.ModuleCore;
using Xunit;

namespace System.Xml.RwFactoryWriterTests
{
    public partial class CFactoryModule : CXmlDriverModule
    {
        [Theory]
        [XmlTests(nameof(Create))]
        public void RunTests(XunitTestCase testCase)
        {
            testCase.Run();
        }

        public static CTestModule Create()
        {
            var module = new CFactoryModule();

            module.Init(null);

            return module;
        }
    }
}
