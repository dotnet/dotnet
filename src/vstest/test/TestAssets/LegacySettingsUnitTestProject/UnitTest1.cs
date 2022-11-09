// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacySettingsUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Test for scripts
        /// </summary>
        [TestMethod]
        public void ScriptsTest()
        {
            // Setup script should create a dummyfile in temp directory
            var scriptPath = Path.Combine(Path.GetTempPath() + "ScriptTestingFile.txt");
            Assert.IsTrue(File.Exists(scriptPath));
        }

        /// <summary>
        /// Test for deployment item
        /// </summary>
        [TestMethod]
        public void DeploymentItemTest()
        {
            // File exists check for deployment item and scripts
            var deploymentFullPath = Path.Combine(Path.GetDirectoryName(typeof(UnitTest1).GetTypeInfo().Assembly.Location), "DeploymentFile.xml");
            Assert.IsTrue(File.Exists(deploymentFullPath));
        }

        /// <summary>
        /// Runs for 1 seconds
        /// </summary>
        [TestMethod]
        public void OneSecTimeTest()
        {
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Runs for 3 seconds
        /// </summary>
        [TestMethod]
        public void ThreeSecTimeTest()
        {
            System.Threading.Thread.Sleep(3000);
        }

        /// <summary>
        /// Has dependency on another dll, needs assembly resolution
        /// </summary>
        [TestMethod]
        public void DependencyTest()
        {
            var unitTest = new DependencyAssemblyForTest.Class1();
        }
    }
}
