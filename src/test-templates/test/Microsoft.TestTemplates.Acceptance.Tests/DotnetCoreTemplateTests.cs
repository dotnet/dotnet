// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestTemplates.Acceptance.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.IO;

    [TestClass]
    public class DotnetCoreTemplateTests : AcceptanceTestBase
    {
        /// <summary>
        /// The net core versions for which templates are present
        /// </summary>
        private static string[] netCoreVersions = {
            // refer to https://dotnet.microsoft.com/download/dotnet-core
            // for a list of supported dotnet versions and only include the ones
            // that are not end-of-life
            "3.1",
            "5.0",
            "6.0",
            "7.0",
            // "8.0", TODO: not yet, enable when net8.0 sdk can target net8.0
        };

        /// <summary>
        /// The type of the test template, combination of the test framework and language
        /// </summary>
        private static string[] templateTypes = {
            "MSTest-CSharp",
            "MSTest-FSharp",
            "MSTest-VisualBasic",
            "NUnit-CSharp",
            "NUnit-FSharp",
            "NUnit-VisualBasic",
            "XUnit-CSharp",
            "XUnit-FSharp",
            "XUnit-VisualBasic"
        };

        [DataTestMethod]
        [DynamicData(nameof(GetTestTemplatesPath), DynamicDataSourceType.Method)]
        public void TemplateTest(string path)
        {
            // Invokes dotnet test <path>
            InvokeDotnetTest(path);

            // Verfiy the tests run as expected.
            ValidateSummaryStatus(1, 0, 0);
        }

        /// <summary>
        /// Dynamic data source for the template test 
        /// </summary>
        /// <returns>Paths to all possible the template projects</returns>
        private static IEnumerable<object[]> GetTestTemplatesPath()
        {
            var list = new List<string[]>();

            foreach (var netcoreVersion in netCoreVersions)
            {
                foreach (var templateType in templateTypes)
                {
                    list.Add(new string[] { Path.Combine("template_feed", "Microsoft.DotNet.Test.ProjectTemplates." + netcoreVersion, "content", templateType) });
                }
            }


            // Net8 still not working.
            // list.Add(new string[] { Path.Combine("template_feed", "Microsoft.DotNet.Test.ProjectTemplates.8.0", "content", "Playwright-MSTest-CSharp") });
            // list.Add(new string[] { Path.Combine("template_feed", "Microsoft.DotNet.Test.ProjectTemplates.8.0", "content", "Playwright-NUnit-CSharp") });

            return list;
        }
    }
}
