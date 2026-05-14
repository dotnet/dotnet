// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace Microsoft.DotNet.Arcade.Sdk
{
    /// <summary>
    /// Used to convert a raw XML dump from IBCMerge into the set of methods which will be NGEN'd when 
    /// partial NGEN is enabled
    /// </summary>
    public sealed class ExtractNgenMethodList : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// This is the XML file produced by passing -dxml to ibcmerge. It will be transformed into the set of
        /// methods marked for NGEN in the assembly
        /// </summary>
        [Required]
        public string IbcXmlFilePath { get; set; }

        /// <summary>
        /// This is the name of the assembly for when IBC is being run. 
        /// </summary>
        [Required]
        public string AssemblyFilePath { get; set; }

        /// <summary>
        /// The current target framework for the assembly being compiled.
        /// </summary>
        public string AssemblyTargetFramework { get; set; }

        /// <summary>
        /// This is the directory the NGEN list should be output to
        /// </summary>
        [Required]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// From IBCMerge.MethodProfilingDataFlags
        /// </summary>
        private const int ReadMethodCode = 0x1;

        public override bool Execute()
        {
            var document = XDocument.Load(IbcXmlFilePath);
            var items = new List<string>();
            var parentElement = document.Root.Element("MethodProfilingData");
            if (parentElement != null)
            {
                foreach (var child in parentElement.Elements("Item"))
                {
                    var flagsInt = child.Attribute("flagsInt");
                    var name = child.Attribute("name");
                    if (int.TryParse(flagsInt?.Value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedFlagsInt) 
                        && (parsedFlagsInt & ReadMethodCode) == 1)
                    {
                        items.Add(name.Value);
                    }
                }
            }

            items.Sort();

            Directory.CreateDirectory(OutputDirectory);

            // When AssemblyTargetFramework is set then this is an assembly that is being built by the current
            // build. Appending the target framework means we will avoid name clashes. When it's not set then
            // this is a binary that is included in the build but not actually built here. Possible, remotely, 
            // that there will be multiple versions with the same target framework. Hence use the MVID as the 
            // suffix here to avoid clashes.
            var outputFileNameSuffix = string.IsNullOrEmpty(AssemblyTargetFramework)
                ? GetAssemblyMvid().ToString()
                : AssemblyTargetFramework;
            var outputFileName = $"{Path.GetFileNameWithoutExtension(AssemblyFilePath)}-{outputFileNameSuffix}.ngen.txt";
            var outputFilePath = Path.Combine(OutputDirectory, outputFileName);
            using (var outputFileStream = new StreamWriter(outputFilePath, append: false))
            {
                foreach (var item in items)
                {
                    outputFileStream.WriteLine(item);
                }
            }

            return true;
        } 

        private Guid GetAssemblyMvid()
        {
            using (var stream = File.Open(AssemblyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var peReader = new PEReader(stream);
                var metadataReader = peReader.GetMetadataReader();
                return metadataReader.GetGuid(metadataReader.GetModuleDefinition().Mvid);
            }
        }
    }
}
