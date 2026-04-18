// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    public class AddSbrpAttribute : Task
    {
        [Required]
        public ITaskItem[] ILFileNames { get; set; } = null!;

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "STARTING ADDING SBRP ATTRIBUTE");

            foreach (var ilFile in ILFileNames)
            {
                bool attributeInserted = false;
                string assemblyReference = "";
                var lines = File.ReadAllLines(ilFile.ItemSpec);
                var outputLines = new List<string>();

                foreach (var line in lines)
                {
                    if (!attributeInserted && line.Contains(".custom"))
                    {
                        // Use regex to extract the substring in the format [assemblyName]
                        var match = Regex.Match(line, @"\[[^\]]*\]");
                        if (match.Success)
                        {
                            assemblyReference = match.Value;
                        }

                        AddCustomAttributeLines(outputLines, assemblyReference);
                        attributeInserted = true;
                    }

                    outputLines.Add(line);
                }

                File.WriteAllLines(ilFile.ItemSpec, outputLines);
            }

            return true;
        }

        private static void AddCustomAttributeLines(List<string> outputLines, string assemblyReference)
        {
            // Padding the string so that ctor arguments are aligned correctly.
            int length = assemblyReference.Length;
            outputLines.Add($"  .custom instance void {assemblyReference}System.Reflection.AssemblyMetadataAttribute::.ctor(string,");
            outputLines.Add($"{"".PadRight(length)}                                                                           string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source");
            outputLines.Add($"{"".PadRight(length)}                                                                                       2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference");
            outputLines.Add($"{"".PadRight(length)}                                                                                       2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..");
        }
    }
}
