// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public abstract class CreateWixCommandPackageDropBase : BuildTask
    {
        private const int _fieldsArtifactId = 0;
        private const int _fieldsArtifactPath1 = 6;
        private const int _fieldsArtifactPath2 = 1;
        private const int _fieldsArtifactPath3 = 2;
        private const int _fieldsArtifactPath6 = 5;

        private readonly string _packageExtension = ".wixpack.zip";
        public bool NoLogo { get; set; }
        /// <summary>
        /// Additional set of base paths that are used for resolving paths.
        /// </summary>
        public ITaskItem[] AdditionalBasePaths { get; set; }
        /// <summary>
        /// Localization files
        /// </summary>
        public ITaskItem[] Loc { get; set; }
        [Required]
        public string InstallerFile { get; set; }
        public ITaskItem[] WixExtensions { get; set; }

        /// <summary>
        /// folder to place wixpackage output file
        /// </summary>
        [Required]
        public string OutputFolder { get; set; }
        [Required]
        public ITaskItem[] WixSrcFiles { get; set; }

        /// <summary>
        /// path of wixpackage file
        /// </summary>
        [Output]
        public string OutputFile { get; set; }

        protected abstract void ProcessToolSpecificCommandLineParameters(string packageDropOutputFolder, StringBuilder commandString);

        protected void ProcessWixCommand(string packageDropOutputFolder, string toolExecutable, string originalCommand)
        {
            if (!Directory.Exists(packageDropOutputFolder))
            {
                Directory.CreateDirectory(packageDropOutputFolder);
            }

            ProcessWixSrcFiles(packageDropOutputFolder);

            ProcessLocFiles(packageDropOutputFolder);

            CreateCommandFile(toolExecutable, originalCommand, packageDropOutputFolder);

            OutputFile = Path.Combine(OutputFolder, $"{Path.GetFileName(InstallerFile)}{_packageExtension}");
            if(File.Exists(OutputFile))
            {
                File.Delete(OutputFile);
            }
            if(!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }
            ZipFile.CreateFromDirectory(packageDropOutputFolder, OutputFile);
        }

        private void CreateCommandFile(string toolExecutable, string originalCommand, string packageDropOutputFolder)
        {
            string commandFilename = Path.Combine(packageDropOutputFolder, $"create.cmd");
            StringBuilder commandString = new StringBuilder();
            commandString.AppendLine("@echo off");
            commandString.AppendLine("set outputfolder=%1");
            commandString.AppendLine("if \"%outputfolder%\" NEQ \"\" (");
            commandString.AppendLine("  if \"%outputfolder:~-1%\" NEQ \"\\\" ( ");
            commandString.AppendLine("    set outputfolder=%outputfolder%\\");
            commandString.AppendLine("  )");
            commandString.AppendLine(")");
            if (originalCommand != null)
            {
                commandString.AppendLine($"REM Original command");
                commandString.AppendLine($"REM {originalCommand }");
            }
            commandString.AppendLine("REM Modified command");
            commandString.Append(toolExecutable);
            commandString.Append($" -out %outputfolder%{Path.GetFileName(InstallerFile)}");
            if (NoLogo)
            {
                commandString.Append(" -nologo");
            }
            if (Loc != null)
            {
                foreach (var locItem in Loc)
                {
                    commandString.Append($" -loc {Path.GetFileName(locItem.ItemSpec)}");
                }
            }
            if (WixExtensions != null)
            {
                foreach (var wixExtension in WixExtensions)
                {
                    commandString.Append($" -ext {wixExtension.ItemSpec}");
                }
            }
            if (WixSrcFiles != null)
            {
                foreach (var wixSrcFile in WixSrcFiles)
                {
                    commandString.Append($" {Path.GetFileName(wixSrcFile.ItemSpec)}");
                }
            }
            ProcessToolSpecificCommandLineParameters(packageDropOutputFolder, commandString);
            commandString.AppendLine();
            if(!Directory.Exists(packageDropOutputFolder))
            {
                Directory.CreateDirectory(packageDropOutputFolder);
            }
            File.WriteAllText(commandFilename, commandString.ToString());
        }

        /// <summary>
        ///     Process each of the wix src files
        /// </summary>
        /// <param name="packageDropOutputFolder">Drop folder to place artifacts</param>
        private void ProcessWixSrcFiles(string packageDropOutputFolder)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("wix", "http://schemas.microsoft.com/wix/2006/objects");

            foreach (var wixSrcFile in WixSrcFiles)
            {
                // copy the file to outputPath
                string newWixSrcFilePath = Path.Combine(packageDropOutputFolder, Path.GetFileName(wixSrcFile.ItemSpec));
                File.Copy(wixSrcFile.ItemSpec, newWixSrcFilePath, true);

                string wixSrcFileExtension = Path.GetExtension(wixSrcFile.ItemSpec);
                // These files are typically .wixobj. Occasionally we have a wixlib as input, which
                // is created using light and is a binary file. When doing post-build signing,
                // it's replaced in the inputs to the light command after being reconstructed from
                // its own light command drop.
                if (wixSrcFileExtension == ".wixlib")
                {
                    continue;
                }
                else if (wixSrcFileExtension != ".wixobj")
                {
                    Log.LogError($"Wix source file extension {wixSrcFileExtension} is not supported.");
                    continue;
                }

                ProcessWixObj(newWixSrcFilePath, packageDropOutputFolder, nsmgr);
            }
        }

        /// <summary>
        ///     Process the .wxl files and copy to the local drop folder
        /// </summary>
        /// <param name="packageDropOutputFolder">Drop location for wxl files</param>
        private void ProcessLocFiles(string packageDropOutputFolder)
        {
            if (Loc != null)
            {
                foreach (var locItem in Loc)
                {
                    var destinationPath = Path.Combine(packageDropOutputFolder, Path.GetFileName(locItem.ItemSpec));
                    File.Copy(locItem.ItemSpec, destinationPath, true);
                }
            }
        }

        /// <summary>
        ///     Process a .wixobj file that is an input to the light/lit command.
        /// </summary>
        /// <param name="wixObjFilePath">Path to the wixobj file in its new drop location</param>
        /// <param name="packageDropOutputFolder">Output light/lit command drop folder</param>
        /// <param name="nsmgr">xml namespace manager</param>
        private void ProcessWixObj(string wixObjFilePath, string packageDropOutputFolder, XmlNamespaceManager nsmgr)
        {
            Log.LogMessage(LogImportance.Normal, $"Creating modified wixobj file '{wixObjFilePath}'...");

            XDocument doc = XDocument.Load(wixObjFilePath);
            if (doc == null)
            {
                Log.LogError($"Failed to open the wixobj file '{wixObjFilePath}'");
                return;
            }

            // process fragment - WixFile elements
            // path in field 7
            string xpath = "//wix:wixObject/wix:section[@type='fragment']/wix:table[@name='WixFile']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath1);

            // process product - WixFile elements
            // path in field 7
            xpath = "//wix:wixObject/wix:section[@type='product']/wix:table[@name='WixFile']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath1);

            // process fragment - Binary elements
            // path in field 2
            xpath = "//wix:wixObject/wix:section[@type='fragment']/wix:table[@name='Binary']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath2);

            // process product - Icon elements
            // path in field 2
            xpath = "//wix:wixObject/wix:section[@type='product']/wix:table[@name='Icon']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath2);

            // process product - WixVariable elements
            // path in field 2
            xpath = "//wix:wixObject/wix:section[@type='product']/wix:table[@name='WixVariable']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath2);

            // Bundle specific items.

            // path in fields 3 and 6
            xpath = "//wix:wixObject/wix:section[@type='bundle']/wix:table[@name='Payload']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath3, _fieldsArtifactPath6);

            // process WixVariable data
            // path in field 2
            xpath = "//wix:wixObject/wix:section[@type='bundle']/wix:table[@name='WixVariable']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath2);

            // process Payload, in fragment section, data
            // path in fields 3 and 6
            xpath = "//wix:wixObject/wix:section[@type='fragment']/wix:table[@name='Payload']/wix:row";
            ProcessXPath(doc, xpath, packageDropOutputFolder, nsmgr, _fieldsArtifactPath3, _fieldsArtifactPath6);

            doc.Save(wixObjFilePath);
        }

        private void ProcessXPath(XDocument doc, string xpath, string outputPath, XmlNamespaceManager nsmgr, int pathField1, int pathField2 = 0)
        {
            IEnumerable<XElement> iels = doc.XPathSelectElements(xpath, nsmgr);
            if (iels != null && iels.Count() > 0)
            {

                foreach (XElement row in iels)
                {
                    IEnumerable<XElement> fields = row.XPathSelectElements("wix:field", nsmgr);
                    if (fields == null || fields.Count() == 0)
                    {
                        Log.LogError($"No fields in row ('{xpath}') of document '{doc.BaseUri}'");
                        continue;
                    }

                    int count = 0;
                    string id = null;
                    string oldPath = null;
                    string newRelativePath = null;
                    bool foundArtifact = false;
                    bool isVariableOrUriRef = false;

                    foreach (XElement field in fields)
                    {
                        if (count == _fieldsArtifactId)
                        {
                            id = field.Value;
                        }
                        else if (count == pathField1)
                        {
                            oldPath = field.Value;

                            // Potentially make oldPath the absolute if it's not, using the additional base
                            // paths. It's possible that the path is a variable or URI. In this case,
                            // we can ignore it.
                            if (oldPath.StartsWith("!(") || oldPath.StartsWith("https"))
                            {
                                isVariableOrUriRef = true;
                                break;
                            }
                            else if (!Path.IsPathRooted(oldPath))
                            {
                                if (AdditionalBasePaths == null)
                                {
                                    // Break here, will log an error below.
                                    break;
                                }
                                foreach (var additionalBasePath in AdditionalBasePaths)
                                {
                                    var possiblePath = Path.Combine(additionalBasePath.ItemSpec, oldPath);
                                    if (File.Exists(possiblePath))
                                    {
                                        oldPath = possiblePath;
                                        foundArtifact = true;
                                        break;
                                    }
                                }
                            }
                            else if (File.Exists(oldPath))
                            {
                                foundArtifact = true;
                            }
                            else
                            {
                                break;
                            }

                            newRelativePath = Path.Combine(id, Path.GetFileName(oldPath));
                            field.Value = newRelativePath;
                        }
                        else if (pathField2 != 0 && count == pathField2)
                        {
                            field.Value = newRelativePath;
                            break;
                        }
                        count++;
                    }

                    if (!isVariableOrUriRef)
                    {
                        if (foundArtifact)
                        {
                            string newFolder = Path.Combine(outputPath, id);
                            if (!Directory.Exists(newFolder))
                            {
                                Directory.CreateDirectory(newFolder);
                            }

                            File.Copy(oldPath, Path.Combine(outputPath, newRelativePath), true);
                        }
                        else if (oldPath == null)
                        {
                            Log.LogError($"Could not locate a file within {row}");
                        }
                        else
                        {
                            Log.LogError($"Could not locate file {oldPath}. Please ensure the file exists and/or pass AdditionalBasePaths for non-rooted file paths.");
                        }
                    }
                }
            }
        }
    }
}
