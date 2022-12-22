// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    /// <summary>
    /// Generates OptProf training input files for VS components listed in OptProf.json file and 
    /// their VSIX files located in the specified directory.
    /// </summary>
    public sealed class GenerateTrainingInputFiles : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Absolute path to the OptProf.json config file.
        /// </summary>
        [Required]
        public string ConfigurationFile { get; set; }

        /// <summary>
        /// Absolute path to the directory that contains VSIXes that will be inserted.
        /// </summary>
        [Required]
        public string InsertionDirectory { get; set; }

        /// <summary>
        /// Directory to output the results optprof data to.
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
            OptProfTrainingConfiguration config;
            try
            {
                config = OptProfTrainingConfiguration.Deserialize(File.ReadAllText(ConfigurationFile, Encoding.UTF8));
            }
            catch (Exception e)
            {
                Log.LogError($"Unable to open the config file '{ConfigurationFile}': {e.Message}");
                return;
            }

            if (config.Products == null)
            {
                Log.LogError($"Invalid configuration file format: missing 'products' element in '{ConfigurationFile}'.");
            }

            if (config.Assemblies == null)
            {
                Log.LogError($"Invalid configuration file format: missing 'assemblies' element in '{ConfigurationFile}'.");
            }

            if (!Directory.Exists(InsertionDirectory))
            {
                Log.LogError($"Directory specified in InsertionDirectory does not exist: '{InsertionDirectory}'.");
            }

            if (Log.HasLoggedErrors)
            {
                return;
            }

            // Handle product entries
            foreach (var product in config.Products)
            {
                string vsixFilePath = Path.Combine(InsertionDirectory, product.Name);

                var jsonManifest = ReadVsixJsonManifest(vsixFilePath);
                var ibcEntries = IbcEntry.GetEntriesFromVsixJsonManifest(jsonManifest).ToArray();

                WriteEntries(product.Tests, ibcEntries);
            }

            // Handle assembly entries
            foreach (var assembly in config.Assemblies)
            {
                var assemblyEntries = IbcEntry.GetEntriesFromAssembly(assembly).ToArray();
                WriteEntries(assembly.Tests, assemblyEntries);
            }
        }

        private static JObject ReadVsixJsonManifest(string vsixPath)
        {
            using (var archive = new ZipArchive(File.Open(vsixPath, FileMode.Open), ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry("manifest.json");

                using (var stream = entry.Open())
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 2048, leaveOpen: true))
                    {
                        var content = reader.ReadToEnd();
                        return JObject.Parse(content);
                    }
                }
            }
        }

        private void WriteEntries(OptProfTrainingTest[] tests, IbcEntry[] ibcEntries)
        {
            foreach (var test in tests)
            {
                var configurationsDir = Path.Combine(OutputDirectory, test.Container, "Configurations");
                if (test.TestCases is object)
                {
                    foreach (var fullyQualifiedName in test.TestCases)
                    {
                        WriteEntries(ibcEntries, Path.Combine(configurationsDir, fullyQualifiedName));
                    }
                }
                else if (test.FilteredTestCases is object)
                {
                    foreach(var filteredTestCase in test.FilteredTestCases)
                    {
                        var filteredIbcEntries = ibcEntries.Where(ibc => ibc.EntryName == filteredTestCase.FileName).ToArray();
                        foreach (var fullyQualifiedName in filteredTestCase.TestCases)
                        {
                            WriteEntries(filteredIbcEntries, Path.Combine(configurationsDir, fullyQualifiedName));
                        }
                    }
                }
            }
        }

        private static void WriteEntries(IbcEntry[] ibcEntries, string outDir)
        {
            Directory.CreateDirectory(outDir);

            foreach (var entry in ibcEntries)
            {
                int index = 0;
                string basePath = Path.Combine(outDir, entry.RelativeDirectoryPath.Replace("\\", "") + Path.GetFileNameWithoutExtension(entry.RelativeInstallationPath));

                string fullPath;
                do
                {
                    fullPath = basePath + "." + index + ".IBC.json";
                    index++;
                }
                while (File.Exists(fullPath));

                using (var writer = new StreamWriter(File.Open(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read)))
                {
                    writer.WriteLine(entry.ToJson().ToString());
                }

            }
        }
    }
}
