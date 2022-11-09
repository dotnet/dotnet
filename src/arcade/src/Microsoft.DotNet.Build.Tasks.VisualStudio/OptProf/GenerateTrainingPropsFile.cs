// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    /// <summary>
    /// Generates a .props file pointing to a drops URL where IBC optimization inputs will be uploaded.
    /// </summary>
    public sealed class GenerateTrainingPropsFile : Microsoft.Build.Utilities.Task
    {
        private const string ProductDropNamePrefix = "Products/";

        /// <summary>
        /// GitHub repository name (e.g. 'dotnet/roslyn'). If unspecified a dummy value is used.
        /// </summary>
        public string RepositoryName { get; set; }

        /// <summary>
        /// Product drop name, e.g. 'Products/$(System.TeamProject)/$(Build.Repository.Name)/$(Build.SourceBranchName)/$(Build.BuildNumber)'. If unspecified a dummy value is used.
        /// </summary>
        public string ProductDropName { get; set; }

        /// <summary>
        /// Directory to output the props file to.
        /// </summary>
        [Required]
        public string OutputDirectory { get; set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            bool hasDropName = !string.IsNullOrEmpty(ProductDropName);
            if (hasDropName && !ProductDropName.StartsWith(ProductDropNamePrefix, StringComparison.Ordinal))
            {
                Log.LogError($"Invalid value of vsDropName argument: must start with '{ProductDropNamePrefix}'.");
                return;
            }

            var dropName = hasDropName ? ProductDropName.Substring(ProductDropNamePrefix.Length) : "dummy";
            var outputFileNameNoExt = string.IsNullOrEmpty(RepositoryName) ? "ProfilingInputs" : RepositoryName.Replace('/', '.');
            var outputFilePath = Path.Combine(OutputDirectory, outputFileNameNoExt + ".props");

            Directory.CreateDirectory(OutputDirectory);
            File.WriteAllText(outputFilePath,
$@"<?xml version=""1.0""?>
<Project>
  <ItemGroup>
    <TestStore Include=""vstsdrop:ProfilingInputs/{dropName}"" />
  </ItemGroup>
</Project>", Encoding.UTF8);
        }
    }
}
