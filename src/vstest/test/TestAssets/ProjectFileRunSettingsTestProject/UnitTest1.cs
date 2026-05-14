// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectFileRunSettingsTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // this project specifies runsettings in it's proj file
            // that runsettings say that inconclusive should translate to
            // failed.
            // we can then easily figure out if the settings were applied
            // correctly if we set the test as failed, or did not apply if the
            // test is shown as skipped
            Assert.Inconclusive();
        }
    }
}
