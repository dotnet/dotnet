// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestTemplates.Acceptance.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    [TestClass]
    public class DotnetCoreTemplateTests : AcceptanceTestBase
    {
        /// <summary>
        /// The net core versions for which templates are present
        /// </summary>
        private static readonly string[] NetCoreVersions =
        [
            "8.0",
            "9.0",
        ];

        /// <summary>
        /// The type of the test template, combination of the test framework and language
        /// </summary>
        private static readonly string[] TemplateTypes =
        [
            "MSTest-CSharp",
            "MSTest-FSharp",
            "MSTest-VisualBasic",
            "NUnit-CSharp",
            "NUnit-FSharp",
            "NUnit-VisualBasic",
            "XUnit-CSharp",
            "XUnit-FSharp",
            "XUnit-VisualBasic",
            // Playwright templates are disabled for now, as they require extra installations.
            // This is in accordance with Playwright's owner's decision for the template design.
            // "Playwright-MSTest-CSharp",
            // "Playwright-NUnit-CSharp",
        ];

        [DataTestMethod]
        [DynamicData(nameof(GetTestTemplatesPath), DynamicDataSourceType.Method)]
        public void TemplateTest(string path)
        {
            // Invokes dotnet test <path>
            InvokeDotnetTest(path);

            // Verify the tests run as expected.
            ValidateSummaryStatus(1, 0, 0);
        }


        [DataTestMethod]
        [DynamicData(nameof(GetTestTemplatesPath), DynamicDataSourceType.Method)]
        public void TemplateTest_WrongTfmShouldFail(string path)
        {
            InvokeDotnet("build " + path);
            InvokeDotnet("test  --no-build --framework net5.0 " + path, assertExecution: false);

            Assert.IsTrue(Regex.IsMatch(_standardTestError, "The test source file.*provided was not found."));
            Assert.AreNotEqual(0, _runnerExitCode);

            // after we want to test the normal path to ensure we're not breaking it.
            TemplateTest(path);
        }

        /// <summary>
        /// Dynamic data source for the template test
        /// </summary>
        /// <returns>Paths to all possible the template projects</returns>
        private static IEnumerable<object[]> GetTestTemplatesPath()
        {
            var list = new List<string[]>();

            foreach (var netcoreVersion in NetCoreVersions)
            {
                foreach (var templateType in TemplateTypes)
                {
                    list.Add([Path.Combine("template_feed", "Microsoft.DotNet.Test.ProjectTemplates." + netcoreVersion, "content", templateType)]);
                }
            }

            return list;
        }
    }
}
