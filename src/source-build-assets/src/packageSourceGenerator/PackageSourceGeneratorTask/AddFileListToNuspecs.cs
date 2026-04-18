// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.SourceBuild.Tasks
{
    // Adds the files to include in the nuspec file of the package(s)
    // that we're generating source for.  This will allow the re-pack
    // to pick up the correct files.
     public class AddFileListToNuspecs : Task
    {
        [Required]
        public ITaskItem[] NuspecFileNames { get; set; } = null!;

        public override bool Execute()
        {
            string[] filesClause =
            {
                "  <files>",
                "    <file src=\"*/**\" target=\"/\" exclude=\"obj/**/*.*;**/Debug/**/*.*\" />",
                "    <file src=\"*\" target=\"/\" exclude=\"*.csproj\" />",
                "  </files>"
            };

            foreach (var nuspecFiles in NuspecFileNames)
            {
                var lines = File.ReadAllLines(nuspecFiles.ItemSpec);
                var outputLines = new List<string>();

                foreach (var line in lines)
                {
                    if (line.StartsWith("</package>"))
                    {
                        outputLines.AddRange(filesClause);
                    }
                    outputLines.Add(line);
                }
                File.WriteAllLines(nuspecFiles.ItemSpec, outputLines);
            }

            return true;
        }
    }
}
