// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    /// <summary>
    /// Calculates the SessionConfiguration to be used in .runsettings for OptProf training 
    /// based on given OptProf.json configuration and VS bootstrapper information.
    /// </summary>
    public sealed class GetRunSettingsSessionConfiguration : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Absolute path to the OptProf.json config file.
        /// </summary>
        [Required]
        public string ConfigurationFile { get; set; }

        /// <summary>
        /// Product drop name, e.g. 'Products/$(System.TeamProject)/$(Build.Repository.Name)/$(Build.SourceBranchName)/$(Build.BuildNumber)'
        /// </summary>
        [Required]
        public string ProductDropName { get; set; }

        /// <summary>
        /// Path to the BootstrapperInfo.json
        /// </summary>
        [Required]
        public string BootstrapperInfoPath { get; set; }

        /// <summary>
        /// Contents of SessionConfiguration node of the .runsettings file.
        /// </summary>
        [Output]
        public string SessionConfiguration { get; private set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            try
            {
                var profilingInputsDropName = GetProfilingInputsDropName(ProductDropName);
                var buildDropName = GetTestsDropName(File.ReadAllText(BootstrapperInfoPath, Encoding.UTF8));
                var (testContainersString, testCaseFilterString) = GetTestContainersAndFilters(File.ReadAllText(ConfigurationFile, Encoding.UTF8), ConfigurationFile);

                SessionConfiguration = 
$@"<TestStores>
  <TestStore Uri=""vstsdrop:{profilingInputsDropName}"" />
  <TestStore Uri=""vstsdrop:{buildDropName}"" />
</TestStores>
<TestContainers>
{testContainersString}
</TestContainers>
<TestCaseFilter>{testCaseFilterString}</TestCaseFilter>";
            }
            catch (ApplicationException e)
            {
                Log.LogError(e.Message);
            }
        }

        internal static string GetTestsDropName(string bootstrapperInfoJson)
        {
            try
            {
                var jsonContent = JToken.Parse(bootstrapperInfoJson);
                var dropUrl = (string)((JArray)jsonContent).First["BuildDrop"];

                const string prefix = "https://vsdrop.corp.microsoft.com/file/v1/Products/";
                if (!dropUrl.StartsWith(prefix, StringComparison.Ordinal))
                {
                    throw new ApplicationException($"Invalid drop URL: '{dropUrl}'");
                }

                return $"Tests/{dropUrl.Substring(prefix.Length)}";
            }
            catch (Exception e)
            {                
                throw new InvalidDataException(
                    $"Unable to read bootstrapper info: {e.Message}{Environment.NewLine}" +
                    $"Content of BootstrapperInfo.json:{Environment.NewLine}" +
                    $"{bootstrapperInfoJson}");
            }
        }

        private static string GetProfilingInputsDropName(string vsDropName)
        {
            const string prefix = "Products/";
            if (!vsDropName.StartsWith(prefix, StringComparison.Ordinal))
            {
                throw new ApplicationException("Invalid value of vsDropName argument: must start with 'Products/'.");
            }

            return "ProfilingInputs/" + vsDropName.Substring(prefix.Length);
        }

        internal static (string containers, string filters) GetTestContainersAndFilters(string configJson, string configPath)
        {
            try
            {
                var config = OptProfTrainingConfiguration.Deserialize(configJson);
                return (GetTestContainers(config), GetTestFilters(config));
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Unable to read config file '{configPath}': {e.Message}");
            }
        }

        private static string GetTestContainers(OptProfTrainingConfiguration config)
        {
            var productContainers = config.Products?.Any() == true
              ? config.Products.SelectMany(x => x.Tests.Select(y => y.Container + ".dll"))
              : Enumerable.Empty<string>();

            var assemblyContainers = config.Assemblies?.Any() == true
                ? config.Assemblies.SelectMany(x => x.Tests.Select(y => y.Container + ".dll"))
                : Enumerable.Empty<string>();

            return string.Join(
                Environment.NewLine,
                productContainers
                    .Concat(assemblyContainers)
                    .Distinct()
                    .Select(x => $@"  <TestContainer FileName=""{x}"" />"));
        }

        private static string GetTestFilters(OptProfTrainingConfiguration config)
        {
            var productTests = config.Products?.Any() == true
                ? config.Products.SelectMany(x => x.Tests.SelectMany(y => y.TestCases ?? y.FilteredTestCases.SelectMany(z => z.TestCases)))
                : Enumerable.Empty<string>();

            var assemblyTests = config.Assemblies?.Any() == true
                ? config.Assemblies.SelectMany(x => x.Tests.SelectMany(y => y.TestCases))
                : Enumerable.Empty<string>();

            return string.Join(
                "|",
                productTests
                    .Concat(assemblyTests)
                    .Distinct()
                    .Select(x => $"FullyQualifiedName={x}"));
        }
    }
}
