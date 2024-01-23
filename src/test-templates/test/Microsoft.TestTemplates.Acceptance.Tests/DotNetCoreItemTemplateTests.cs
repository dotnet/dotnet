// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.TestTemplates.Acceptance.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class DotNetCoreItemTemplateTests : AcceptanceTestBase
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
        private static readonly (string ProjectTemplateName, string ItemTemplateName, string Language)[] TemplateTypes =
        [
            ("nunit", "nunit-test", "c#"),
            ("nunit", "nunit-test", "f#"),
            ("nunit", "nunit-test", "vb"),
        ];

        [ClassInitialize]
        public static void InstallTemplates(TestContext testContext)
        {
            foreach (var netcoreVersion in NetCoreVersions)
            {
                var template = Path.Combine("template_feed", "Microsoft.DotNet.Test.ProjectTemplates." + netcoreVersion, "content");
                InvokeDotnetNewInstall(template);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTemplateItemsToTest), DynamicDataSourceType.Method)]
        public void TemplateItemTest(string targetFramework, string projectTemplate, string itemTemplate, string language)
        {
            // Avoiding VB errors because root namespace must not start with number or contain dashes
            var testProjectName = "_" + Guid.NewGuid().ToString("N");

            // Create new test project: dotnet new <projectTemplate> -n <testProjectName> -f <targetFramework> -lang <language>
            InvokeDotnetNew(projectTemplate, testProjectName, targetFramework, language);

            // Add test item to test project: dotnet new <itemTemplate> -n <test> -lang <language> -o <testProjectName>
            var itemName = "test";

            InvokeDotnetNew(itemTemplate, itemName, language: language, outputDirectory: testProjectName);

            if (language == "f#")
            {
                // f# projects don't include all files by default, so the file is created
                // but the project ignores it until you manually add it into the project
                // in the right order
                AddItemToFsproj(itemName, outputDirectory: testProjectName);
            }

            // Run tests: dotnet test <path>
            InvokeDotnetTest(testProjectName);

            // Verify the tests run as expected.
            ValidateSummaryStatus(2, 0, 0);
        }

        [ClassCleanup]
        public static void UninstallTemplates()
        {
            foreach (var netcoreVersion in NetCoreVersions)
            {
                var template = Path.Combine("template_feed", "Microsoft.DotNet.Test.ProjectTemplates." + netcoreVersion, "content");
                InvokeDotnetNewUninstall(template);
            }
        }

        private static IEnumerable<object[]> GetTemplateItemsToTest()
        {
            foreach (var netcoreVersion in NetCoreVersions)
            {
                foreach (var (projectTemplate, itemTemplate, language) in TemplateTypes)
                {
                    var targetFramework = double.Parse(netcoreVersion, CultureInfo.InvariantCulture) < 5.0
                        ? "netcoreapp" + netcoreVersion
                        : "net" + netcoreVersion;

                    yield return new object[] { targetFramework, projectTemplate, itemTemplate, language };
                }
            }
        }

        private void AddItemToFsproj(string itemName, string outputDirectory)
        {
            var fsproj = Path.Combine(outputDirectory, $"{outputDirectory}.fsproj");
            var lines = File.ReadAllLines(fsproj).ToList();

            lines.Insert(lines.IndexOf("  <ItemGroup>") + 1, $@"    <Compile Include=""{itemName}.fs""/>");
            File.WriteAllLines(fsproj, lines);
        }
    }
}
