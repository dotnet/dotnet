// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MSTestv2UnitTestExtension
{
    using Microsoft.VisualStudio.TestPlatform.TestGeneration.Data;
    using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;

    /// <summary>
    /// A unit test class for MSTest unit tests.
    /// </summary>
    public class MSTestv2UnitTestClassManager : UnitTestClassManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestv2UnitTestClassManager"/> class.
        /// </summary>
        /// <param name="configurationSettings">The configuration settings object to be used to determine how the test method is generated.</param>
        /// <param name="naming">The object to be used to give names to test projects.</param>
        public MSTestv2UnitTestClassManager(IConfigurationSettings configurationSettings, INaming naming)
            : base(configurationSettings, naming)
        {
        }

        /// <summary>
        /// Gets the attribute name for marking a class as a test class.
        /// </summary>
        public override string TestClassAttribute
        {
            get { return "TestClass"; }
        }

        /// <summary>
        /// Gets the attribute name for marking a method as a test.
        /// </summary>
        public override string TestMethodAttribute
        {
            get { return "TestMethod"; }
        }

        /// <summary>
        /// Gets the code to force a test failure.
        /// </summary>
        public override string AssertionFailure
        {
            get { return "Assert.Fail()"; }
        }
    }
}
