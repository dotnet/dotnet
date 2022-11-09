// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Installers.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    /// <summary>
    ///     Creates a package drop for wixlibs produced by the lit.exe command
    ///     This is pretty simple, as the wixobj's used as inputs are simply
    ///     packaged together into the wixlib.
    /// </summary>
    public class CreateLitCommandPackageDrop : CreateWixCommandPackageDropBase
    {
        [Required]
        public string LitCommandWorkingDir { get; set; }
        /// <summary>
        /// Bind files into the library file.
        /// </summary>
        public bool Bf { get; set; }

        // The lot command that was originally used to generate the wixlib.  This is purely used for informational purposes
        // and to validate that the lit command being created by this task is correct (assist with debugging).
        public string OriginalLitCommand { get; set; }

        public override bool Execute()
        {
            string packageDropOutputFolder = Path.Combine(LitCommandWorkingDir, Path.GetFileName(InstallerFile));
            ProcessWixCommand(packageDropOutputFolder, "lit.exe", OriginalLitCommand);

            return !Log.HasLoggedErrors;
        }

        protected override void ProcessToolSpecificCommandLineParameters(string packageDropOutputFolder, StringBuilder commandString)
        {
            if (Bf)
            {
                commandString.Append(" -bf");
            }
        }
    }
}
