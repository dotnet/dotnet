// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public class CreateLightCommandPackageDrop : CreateWixCommandPackageDropBase
    {
        [Required]
        public string LightCommandWorkingDir { get; set; }
        /// <summary>
        /// Add a FileVersion attribute to each assembly in the MsiAssemblyName table (rarely needed).
        /// </summary>
        public bool Fv { get; set; }
        public string PdbOut { get; set; }
        public string Cultures { get; set; }
        public string WixProjectFile { get; set; }
        public string ContentsFile { get; set; }
        public string OutputsFile { get; set; }
        public string BuiltOutputsFile { get; set; }
        public ITaskItem [] Sice { get; set; }

        // The light command that was originally used to generate the MSI.  This is purely used for informational purposes
        // and to validate that the light command being created by this task is correct (assist with debugging).
        public string OriginalLightCommand { get; set; }

        public override bool Execute()
        {
            string packageDropOutputFolder = Path.Combine(LightCommandWorkingDir, Path.GetFileName(InstallerFile));
            ProcessWixCommand(packageDropOutputFolder, "light.exe", OriginalLightCommand);

            return !Log.HasLoggedErrors;
        }

        protected override void ProcessToolSpecificCommandLineParameters(string packageDropOutputFolder, StringBuilder commandString)
        {
            if (Cultures != null)
            {
                commandString.Append($" -cultures:{Cultures}");
            }
            if (Fv)
            {
                commandString.Append(" -fv");
            }
            if (PdbOut != null)
            {
                commandString.Append($" -pdbout %outputfolder%{PdbOut}");
            }
            if (WixProjectFile != null)
            {
                var destinationPath = Path.Combine(packageDropOutputFolder, Path.GetFileName(WixProjectFile));
                File.Copy(WixProjectFile, destinationPath, true);
                commandString.Append($" -wixprojectfile {Path.GetFileName(WixProjectFile)}");
            }
            if (ContentsFile != null)
            {
                commandString.Append($" -contentsfile {Path.GetFileName(ContentsFile)}");
            }
            if (OutputsFile != null)
            {
                commandString.Append($" -outputsfile {Path.GetFileName(OutputsFile)}");
            }
            if (BuiltOutputsFile != null)
            {
                commandString.Append($" -builtoutputsfile {Path.GetFileName(BuiltOutputsFile)}");
            }
            if (Sice != null)
            {
                foreach (var siceItem in Sice)
                {
                    commandString.Append($" -sice:{siceItem.ItemSpec}");
                }
            }
        }
    }
}
