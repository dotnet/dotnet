// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    /*
     * This task creates a Wixpack package from the provided source files and configuration.
     * It processes the source files, copies necessary content files to a working directory,
     * updates paths and variables in source-files, and generates a command line file
     * for building the Wixpack. Content files get copied to a subfolder named after the File@Id
     * or similar unique value, based on the content element type.
     * We are including extensions in wixpack, which allows us to skip restoring these packages
     * and discover extension binaries during signing/repacking.
     * Finally, this task creates a zip package containing all the necessary files.
     * The task supports various configurations such as cultures, define constants, extensions,
     * include search paths, installer platform, output folder, and more.
     */
    public class CreateWixBuildWixpack : Task
    {
        public string AdditionalOptions { get; set; }

        public ITaskItem BindTrackingFile { get; set; }

        public ITaskItem[] BindPaths { get; set; }

        public string[] Cultures { get; set; }

        public string[] DefineConstants { get; set; }

        public ITaskItem[] Extensions { get; set; }

        public string[] IncludeSearchPaths { get; set; }

        public string InstallerPlatform { get; set; }

        [Required]
        public string InstallerFile { get; set; }

        [Required]
        public ITaskItem IntermediateDirectory { get; set; }

        public ITaskItem[] LocalizationFiles { get; set; }

        /// <summary>
        /// path of wixpackage file
        /// </summary>
        [Output]
        public string OutputFile { get; set; }

        /// <summary>
        /// folder to place wixpackage output file
        /// </summary>
        [Required]
        public string OutputFolder { get; set; }

        public string OutputType { get; set; }

        public ITaskItem PdbFile { get; set; }

        public string PdbType { get; set; }

        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        public string[] SuppressSpecificWarnings { get; set; }

        [Required]
        public string WixpackWorkingDir { get; set; }

        private Dictionary<string, string> _defineConstantsDictionary;
        private Dictionary<string, string> _defineVariablesDictionary;
        private Dictionary<string, string> _systemVariablesDictionary;
        private string _wixprojDir;
        private string _installerFilename;

        private const string _packageExtension = ".wixpack.zip";

        public override bool Execute()
        {
            try
            {
                _defineConstantsDictionary = GetDefineConstantsDictionary();
                _systemVariablesDictionary = GetSystemVariablesDictionary();

                if (string.IsNullOrWhiteSpace(WixpackWorkingDir))
                {
                    WixpackWorkingDir = Path.Combine(Path.GetTempPath(), "WixpackTemp", Guid.NewGuid().ToString().Split('-')[0]);
                }

                _installerFilename = Path.GetFileName(InstallerFile);

                if (Directory.Exists(WixpackWorkingDir))
                {
                    Directory.Delete(WixpackWorkingDir, true);
                }
                Directory.CreateDirectory(WixpackWorkingDir);

                if (_defineConstantsDictionary.TryGetValue("ProjectDir", out _wixprojDir))
                {
                    // Copy wixproj file - fail if ProjectPath is not defined
                    if (_defineConstantsDictionary.TryGetValue("ProjectPath", out var projectPath))
                    {
                        string destPath = Path.Combine(WixpackWorkingDir, Path.GetFileName(projectPath));
                        File.Copy(projectPath, destPath, overwrite: true);
                    }
                    else
                    {
                        throw new InvalidOperationException("ProjectPath not defined in DefineConstants. Task cannot proceed.");
                    }
                }
                else
                {
                    _wixprojDir = string.Empty;
                }

                CopyIncludeSearchPathsContents();
                ProcessIncludeFilesInSearchPaths();
                CopySourceFilesAndContent();
                CopyExtensions();
                CopyBindPathContents();
                CopyLocalizationFiles();
                UpdatePaths();
                GenerateWixBuildCommandLineFile();
                CreateWixpackPackage();
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }

        private void CreateWixpackPackage()
        {
            OutputFile = Path.Combine(OutputFolder, $"{_installerFilename}{_packageExtension}");
            if (File.Exists(OutputFile))
            {
                File.Delete(OutputFile);
            }

            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            ZipFile.CreateFromDirectory(WixpackWorkingDir, OutputFile);
        }

        private void CopyIncludeSearchPathsContents()
        {
            if (IncludeSearchPaths == null || IncludeSearchPaths.Length == 0)
            {
                return;
            }

            for (int i = 0; i < IncludeSearchPaths.Length; i++)
            {
                // If not rooted, resolve relative to _wixprojDir
                var fullSourceDir = GetAbsoluteSourcePath(IncludeSearchPaths[i]);
                if (!Directory.Exists(fullSourceDir))
                {
                    Log.LogWarning($"IncludeSearchPath directory not found: {fullSourceDir}");
                    continue;
                }

                // Use a random directory name for the destination
                var randomDirName = Path.GetRandomFileName();
                IncludeSearchPaths[i] = randomDirName;

                CopyDirectoryRecursive(fullSourceDir, Path.Combine(WixpackWorkingDir, randomDirName));
            }
        }

        private void ProcessIncludeFilesInSearchPaths()
        {
            _defineVariablesDictionary = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            foreach (var includeFile in Directory.GetFiles(WixpackWorkingDir, "*.wxi", SearchOption.AllDirectories))
            {
                ProcessIncludeFile(includeFile);
            }
        }

        private void ProcessIncludeFile(string includeFile)
        {
            // Copy includeFile to %temp% folder
            // We want to keep original files in wixpack, and only preprocess
            // them for wixpack creation. This ensures that repacking process would not be
            // affected by some unintentional change, or a bug in preprocessor.
            var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.Copy(includeFile, tempFilePath, overwrite: true);

            // We're processing a Wix include file, which contains preprocessor
            // and other custom elements that are processed as strings using Regex.
            // It can also contain XML comments that we need to remove, so we don't ingest
            // variables from elements that are commented out.
            RemoveAllXmlComments(tempFilePath);
            PreprocessWixSourceFile(tempFilePath);
            IngestDefineVariablesFromWixFile(tempFilePath);

            File.Delete(tempFilePath);
        }

        private void IngestDefineVariablesFromWixFile(string file)
        {
            try
            {
                IngestDefineVariablesFromString(XDocument.Load(file).ToString());
            }
            catch (Exception ex)
            {
                Log.LogError($"Error ingesting variables from include file {file}: {ex.Message}");
            }
        }

        private void IngestDefineVariablesFromString(string content)
        {
            // We use regular expressions to process wix preprocessor defines
            var regex = new Regex(@"<\?define\s+(\w+)\s*=\s*""([^""]*)""\s*\?>");

            foreach (Match match in regex.Matches(content))
            {
                if (match.Groups.Count == 3)
                {
                    _defineVariablesDictionary[match.Groups[1].Value] = ResolvePath(match.Groups[2].Value);
                }
            }
        }

        private static void RemoveAllXmlComments(string file)
        {
            XDocument xmlDocument = XDocument.Load(file);
            xmlDocument.DescendantNodes()
                       .OfType<XComment>()
                       .ToList()
                       .ForEach(comment => comment.Remove());
            xmlDocument.Save(file);
        }

        private void UpdatePaths()
        {
            // Update ProjectDir to just '.'
            if (_defineConstantsDictionary.ContainsKey("ProjectDir"))
            {
                _defineConstantsDictionary["ProjectDir"] = ".";
            }

            // Update ProjectPath to just the project file name
            if (_defineConstantsDictionary.TryGetValue("ProjectPath", out var projectPath))
            {
                _defineConstantsDictionary["ProjectPath"] = Path.GetFileName(projectPath);
            }

            // Update OutDir to just '.''
            if (_defineConstantsDictionary.ContainsKey("OutDir"))
            {
                _defineConstantsDictionary["OutDir"] = "%outputfolder%";
            }

            // Update TargetDir to just '.''
            if (_defineConstantsDictionary.ContainsKey("TargetDir"))
            {
                _defineConstantsDictionary["TargetDir"] = "%outputfolder%";
            }

            // Update TargetPath to %outputfolder%\<target file name>
            if (_defineConstantsDictionary.TryGetValue("TargetPath", out var targetPath))
            {
                _defineConstantsDictionary["TargetPath"] = Path.Combine("%outputfolder%", Path.GetFileName(targetPath));
            }

            // Update InstallerFile to %outputfolder%\<installer filename>
            InstallerFile = Path.Combine("%outputfolder%", Path.GetFileName(InstallerFile));

            // Update IntermediateDirectory to %outputfolder%
            IntermediateDirectory.ItemSpec = "%outputfolder%";

            // Update PdbFile to %outputfolder%\<pdb file name>
            if (PdbFile != null && !string.IsNullOrEmpty(PdbFile.ItemSpec))
            {
                PdbFile.ItemSpec = Path.Combine("%outputfolder%", Path.GetFileName(PdbFile.ItemSpec));
            }

            // Update BindTrackingFile to %outputfolder%\<bind tracking file name>
            if (BindTrackingFile != null && !string.IsNullOrEmpty(BindTrackingFile.ItemSpec))
            {
                BindTrackingFile.ItemSpec = Path.Combine("%outputfolder%", Path.GetFileName(BindTrackingFile.ItemSpec));
            }
        }

        private void GenerateWixBuildCommandLineFile()
        {
            var commandLineArgs = new List<string>();

            // Add InstallerPlatform if specified
            if (!string.IsNullOrEmpty(InstallerPlatform))
            {
                commandLineArgs.Add($"-platform {InstallerPlatform}");
            }

            commandLineArgs.Add($"-out {InstallerFile}");

            // Add OutputType if specified
            if (!string.IsNullOrEmpty(OutputType))
            {
                commandLineArgs.Add($"-outputType {OutputType}");
            }

            // Add PdbFile if specified
            if (PdbFile != null && !string.IsNullOrEmpty(PdbFile.ItemSpec))
            {
                commandLineArgs.Add($"-pdb {PdbFile.ItemSpec}");
            }

            // Add PdbType if specified
            if (!string.IsNullOrEmpty(PdbType))
            {
                commandLineArgs.Add($"-pdbType {PdbType}");
            }

            // Add each culture from Cultures array
            if (Cultures != null && Cultures.Length > 0)
            {
                foreach (var culture in Cultures)
                {
                    commandLineArgs.Add($"-culture {culture}");
                }
            }

            // Add each Warning from SuppressSpecificWarnings array
            if (SuppressSpecificWarnings != null && SuppressSpecificWarnings.Length > 0)
            {
                foreach (var warning in SuppressSpecificWarnings)
                {
                    commandLineArgs.Add($"-sw{warning}");
                }
            }

            // Add all define constants from dictionary
            if (_defineConstantsDictionary != null && _defineConstantsDictionary.Count > 0)
            {
                foreach (var kvp in _defineConstantsDictionary)
                {
                    // Escape strings only if there is a space in the value
                    string kv = $"{kvp.Key}={kvp.Value}";
                    commandLineArgs.Add($"-d {(kv.Contains(' ') ? $"\"{kv}\"" : kv)}");
                }
            }

            // Add IncludeSearchPaths
            if (IncludeSearchPaths != null && IncludeSearchPaths.Length > 0)
            {
                foreach (var includePath in IncludeSearchPaths)
                {
                    commandLineArgs.Add($"-I {includePath}");
                }
            }

            // Add Extensions
            if (Extensions != null)
            {
                foreach (var extension in Extensions)
                {
                    commandLineArgs.Add($"-ext {extension.ItemSpec}");
                }
            }

            // Add LocalizationFiles
            if (LocalizationFiles != null)
            {
                foreach (var localizationFile in LocalizationFiles)
                {
                    commandLineArgs.Add($"-loc {localizationFile.ItemSpec}");
                }
            }

            // Add BindPaths
            if (BindPaths != null && BindPaths.Length > 0)
            {
                foreach (var bindPath in BindPaths)
                {
                    string bindName = bindPath.GetMetadata("BindName");
                    if (!string.IsNullOrEmpty(bindName))
                    {
                        // Wix build specifies both arguments - matching it here.
                        commandLineArgs.Add($"-bindPath {bindPath.ItemSpec}");
                        commandLineArgs.Add($"-bindPath {bindName}={bindPath.ItemSpec}");
                    }
                }
            }

            // Add IntermediateDirectory
            commandLineArgs.Add($"-intermediatefolder {IntermediateDirectory.ItemSpec}");

            // Add BindTrackingFile if specified
            if (BindTrackingFile != null && !string.IsNullOrEmpty(BindTrackingFile.ItemSpec))
            {
                commandLineArgs.Add($"-trackingfile {BindTrackingFile.ItemSpec}");
            }

            // Add AdditionalOptions if specified
            if (!string.IsNullOrEmpty(AdditionalOptions))
            {
                commandLineArgs.Add(AdditionalOptions);
            }

            commandLineArgs.Add($"-nologo");
            commandLineArgs.Add($"-wx");

            // Add SourceFiles
            if (SourceFiles != null && SourceFiles.Length > 0)
            {
                foreach (var sourceFile in SourceFiles)
                {
                    commandLineArgs.Add($"{Path.GetFileName(sourceFile.ItemSpec)}");
                }
            }

            // The command lines can be quite long, and cmd would reject them. Wix does support
            // response files, so create a response file (create.rsp) to package alongside.
            File.WriteAllText(Path.Combine(WixpackWorkingDir, "create.rsp"), string.Join(System.Environment.NewLine, commandLineArgs)); 

            string commandLine = "wix.exe build @create.rsp";

            StringBuilder createCmdFileContents = new();
            createCmdFileContents.AppendLine("@echo off");
            createCmdFileContents.AppendLine("set outputfolder=%1");
            createCmdFileContents.AppendLine("if \"%outputfolder%\" NEQ \"\" (");
            createCmdFileContents.AppendLine("  if \"%outputfolder:~-1%\" NEQ \"\\\" ( ");
            createCmdFileContents.AppendLine("    set outputfolder=%outputfolder%\\");
            createCmdFileContents.AppendLine("  )");
            createCmdFileContents.AppendLine(")");
            createCmdFileContents.AppendLine("REM Wix build command");
            createCmdFileContents.AppendLine(commandLine);
            File.WriteAllText(Path.Combine(WixpackWorkingDir, "create.cmd"), createCmdFileContents.ToString());
        }

        /// <summary>
        /// Gets a Dictionary from DefineConstants string array (format: key=value)
        /// </summary>
        private Dictionary<string, string> GetDefineConstantsDictionary()
        {
            var dict = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            if (DefineConstants == null)
            {
                return dict;
            }

            foreach (var entry in DefineConstants)
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    continue;
                }

                var idx = entry.IndexOf('=');
                if (idx > -1)
                {
                    var key = entry.Substring(0, idx);
                    var value = entry.Substring(idx + 1);

                    if (!string.IsNullOrEmpty(key))
                    {
                        dict[key] = value;
                    }
                }
            }

            return dict;
        }

        private Dictionary<string, string> GetSystemVariablesDictionary()
        {
            var dict = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            // Add support for known used system variables
            if (_defineConstantsDictionary.TryGetValue("InstallerPlatform", out var installerPlatform))
            {
                dict.Add("BUILDARCH", installerPlatform);
            }

            return dict;
        }

        /// <summary>
        /// For each item in SourceFiles, reads the XML, finds all File elements, gets File@Id and File@Source values.
        /// If File@Source contains $(<value>), replaces it with the value from _defineConstantsDictionary.
        /// Creates a subfolder in WixpackWorkingDir with the name equal to File@Id value.
        /// </summary>
        private void CopySourceFilesAndContent()
        {
            if (SourceFiles == null || _defineConstantsDictionary == null || string.IsNullOrEmpty(WixpackWorkingDir))
            {
                throw new InvalidOperationException("Task not initialized. Run Execute() first.");
            }

            foreach (var sourceFile in SourceFiles)
            {
                var xmlPath = GetAbsoluteSourcePath(sourceFile.ItemSpec);
                if (!File.Exists(xmlPath))
                {
                    Log.LogError($"Source file not found: {sourceFile.ItemSpec}");
                    continue;
                }

                // Copy the sourceFile to WixpackWorkingDir
                var copiedXmlPath = Path.Combine(WixpackWorkingDir, Path.GetFileName(xmlPath));
                File.Copy(xmlPath, copiedXmlPath, overwrite: true);
                string sourceFileFolder = Path.GetDirectoryName(xmlPath);

                // First preprocess the source file to remove non-applicable include files.
                // We defer ingestion of variables, until all variables from include files
                // were ingested, as they may reference each other.
                RemoveAllXmlComments(copiedXmlPath);
                PreprocessWixSourceFile(copiedXmlPath);

                // Ingest variables after file preprocessing
                ProcessAllReferencedIncludeFiles(copiedXmlPath, sourceFileFolder);
                IngestDefineVariablesFromWixFile(copiedXmlPath);

                try
                {
                    var doc = XDocument.Load(copiedXmlPath);

                    var contentElements = new (string, string, string[])[]
                    {
                        ("File", "Id", ["Source"]),
                        ("Binary", "Id", ["SourceFile"]),
                        ("MsiPackage", "Id", ["SourceFile"]),
                        ("ExePackage", "Id", ["SourceFile"]),
                        ("Payload", "Id", ["SourceFile"]),
                        ("WixStandardBootstrapperApplication", "Id", ["LicenseFile", "LocalizationFile", "ThemeFile", "LogoFile"]),
                        ("WixVariable", "Id", ["Value"]),
                        ("Icon", "Id", ["SourceFile"])
                    };

                    foreach (var (elementName, idAttr, sourceAttrArray) in contentElements)
                    {
                        var elements = doc.Descendants().Where(e => e.Name.LocalName == elementName);
                        foreach (var element in elements)
                        {
                            foreach (var sourceAttr in sourceAttrArray)
                            {
                                var source = element.Attribute(sourceAttr)?.Value;

                                if (string.IsNullOrEmpty(source))
                                {
                                    continue;
                                }

                                source = ResolvePath(source);

                                // If there are any unprocessed tokens, process all matching file patterns
                                // We only support one unprocessed token, specified as immediate file parent
                                // [<path>\\]{pattern}\\<filename>
                                if (source.Contains("$("))
                                {
                                    int startIdx = source.IndexOf("$(");
                                    if (startIdx != source.LastIndexOf("$("))
                                    {
                                        Log.LogError($"Multiple unprocessed tokens found in source: {source}.");
                                        continue;
                                    }

                                    string pattern = source.Substring(startIdx, source.IndexOf(')', startIdx + 2) - startIdx + 1);

                                    source = GetAbsoluteSourcePath(source, sourceFileFolder);

                                    var parts = source.Split([$"\\{pattern}\\"], StringSplitOptions.None);
                                    if (parts.Length < 2)
                                    {
                                        Log.LogError($"Unprocessed token: {pattern} in {xmlPath}");
                                        continue;
                                    }

                                    if (parts.Length > 2 || parts[1].Contains('\\'))
                                    {
                                        Log.LogError($"Unsupported source format: {source}");
                                        continue;
                                    }

                                    // Enumerate directories in parts[0]
                                    var dirs = Directory.GetDirectories(parts[0], "*", SearchOption.TopDirectoryOnly);
                                    foreach (var dir in dirs)
                                    {
                                        var filePath = Path.Combine(dir, Path.GetFileName(source));
                                        CopySourceFile(Path.GetFileName(dir), filePath);
                                    }

                                    element.SetAttributeValue(sourceAttr, $"{pattern}\\{parts[1]}");
                                }
                                else
                                {
                                    // Resolved source is a single file, copy it to a subfolder
                                    var id = element.Attribute(idAttr)?.Value;
                                    if (string.IsNullOrEmpty(id))
                                    {
                                        id = Path.GetFileName(source);
                                    }

                                    CopySourceFile(id, source, sourceFileFolder);

                                    // Update the original attribute to "<id>\\<filename>"
                                    var newSourceValue = $"{id}\\{Path.GetFileName(source)}";
                                    element.SetAttributeValue(sourceAttr, newSourceValue);
                                }
                            }
                        }
                    }

                    doc.Save(copiedXmlPath);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Error processing {copiedXmlPath}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// This method will process all include files, from this source file.
        /// It will update the source file with new paths for included files,
        /// in the wixpack working directory.
        /// </summary>
        /// <param name="file"></param>
        private void ProcessAllReferencedIncludeFiles(string file, string relativeRoot)
        {
            string content = File.ReadAllText(file);

            // Regex to match <?include value ?>
            var regex = new Regex(@"<\?include\s+([^\s\?>]+)\s*\?>", RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(content))
            {
                if (match.Groups.Count > 1)
                {
                    string filename = match.Groups[1].Value.Trim('\"');
                    string includeFilePath = GetAbsoluteSourcePath(ResolvePath(filename), relativeRoot);
                    if (File.Exists(includeFilePath))
                    {
                        // Copy the include file, update the source file with new path
                        // and ingest the variables.
                        string id = Path.GetFileName(includeFilePath);
                        string path = CopySourceFile(id, includeFilePath, relativeRoot);
                        ProcessIncludeFile(path);
                        content = content.Replace(filename, $"{id}\\{id}");
                    }
                    else
                    {
                        // Include file could be in IncludeSearchPaths and already copied and processed.
                        bool foundInSearchPath = false;
                        if (IncludeSearchPaths != null)
                        {
                            foreach (var searchPath in IncludeSearchPaths)
                            {
                                var potentialPath = Path.Combine(WixpackWorkingDir, searchPath, Path.GetFileName(includeFilePath));
                                if (File.Exists(potentialPath))
                                {
                                    foundInSearchPath = true;
                                    break;
                                }
                            }
                        }

                        if (!foundInSearchPath)
                        {
                            Log.LogError($"Included file not found: {includeFilePath}");
                        }
                    }
                }
            }

            File.WriteAllText(file, content);
        }

        private string ResolvePath(string path)
        {
            // Replace $(<value>) with value from _defineConstantsDictionary
            int startIdx = path.IndexOf("$(");
            while (startIdx != -1)
            {
                int endIdx = path.IndexOf(')', startIdx + 2);
                if (endIdx == -1)
                {
                    Log.LogError($"Unmatched $() in path: {path}");
                    break;
                }

                var varName = path.Substring(startIdx + 2, endIdx - (startIdx + 2));
                if (varName.StartsWith("sys."))
                {
                    if (_systemVariablesDictionary.TryGetValue(varName.Substring(4), out var varValue))
                    {
                        path = path.Substring(0, startIdx) + varValue + path.Substring(endIdx + 1);
                    }
                    else
                    {
                        // We support tokenized paths
                        break;
                    }
                }
                else
                {
                    if (varName.StartsWith("var."))
                    {
                        varName = varName.Substring(4);
                    }

                    if (_defineConstantsDictionary.TryGetValue(varName, out var varValue) ||
                        _defineVariablesDictionary.TryGetValue(varName, out varValue))
                    {
                        path = path.Substring(0, startIdx) + varValue + path.Substring(endIdx + 1);
                    }
                    else
                    {
                        // We support tokenized paths
                        break;
                    }
                }

                startIdx = path.IndexOf("$(");
            }

            return path;
        }

        /// <summary>
        /// Simple preprocessor of Wix preprocessor tokens.
        /// This uses simple string processing and Regex as tokens are not valid XML elements.
        /// Supported tokens and blocks:
        /// <?if ... ?> <?elseif ... ?> <?else?> <?endif?>
        /// <?ifdef ... ?>
        /// <?ifndef ... ?>
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void PreprocessWixSourceFile(string sourceFile)
        {
            string input = File.ReadAllText(sourceFile);
            var output = new StringBuilder();

            int pos = 0;
            while (pos < input.Length)
            {
                // Find the next preprocessor block
                int ifStart = input.IndexOf("<?if ", pos, StringComparison.OrdinalIgnoreCase);
                int ifdefStart = input.IndexOf("<?ifdef ", pos, StringComparison.OrdinalIgnoreCase);
                int ifndefStart = input.IndexOf("<?ifndef ", pos, StringComparison.OrdinalIgnoreCase);

                // Find the earliest block
                int nextBlockStart = -1;
                string blockType = null;
                if (ifStart != -1 && (ifdefStart == -1 || ifStart < ifdefStart) && (ifndefStart == -1 || ifStart < ifndefStart))
                {
                    nextBlockStart = ifStart;
                    blockType = "if";
                }
                else if (ifdefStart != -1 && (ifndefStart == -1 || ifdefStart < ifndefStart))
                {
                    nextBlockStart = ifdefStart;
                    blockType = "ifdef";
                }
                else if (ifndefStart != -1)
                {
                    nextBlockStart = ifndefStart;
                    blockType = "ifndef";
                }

                if (nextBlockStart == -1)
                {
                    // No more preprocessor blocks, copy the rest
                    output.Append(input.Substring(pos));
                    break;
                }

                // Copy up to the next block
                output.Append(input.Substring(pos, nextBlockStart - pos));

                if (blockType == "if")
                {
                    int ifEnd = input.IndexOf("?>", nextBlockStart, StringComparison.OrdinalIgnoreCase);
                    if (ifEnd == -1)
                    {
                        throw new InvalidOperationException("Malformed <?if?> block.");
                    }

                    // Store all blocks and conditions
                    var blocks = new List<(string condition, string content)>();
                    // Parse the initial <?if ... ?>
                    string ifCondition = input.Substring(nextBlockStart + 4, ifEnd - (nextBlockStart + 4)).Trim();
                    int blockStart = ifEnd + 2;

                    int searchPos = blockStart;
                    int endifStart = input.IndexOf("<?endif?>", searchPos, StringComparison.OrdinalIgnoreCase);
                    if (endifStart == -1)
                    {
                        throw new InvalidOperationException("Missing <?endif?> for <?if?> block.");
                    }

                    // Find all <?elseif ... ?> and <?else?>
                    var blockBoundaries = new List<(int start, int end, string condition, bool isElse)>();
                    int nextTagStart = searchPos;
                    string currentCondition = ifCondition;
                    while (true)
                    {
                        int elseifStart = input.IndexOf("<?elseif", nextTagStart, StringComparison.OrdinalIgnoreCase);
                        int elseStart = input.IndexOf("<?else?>", nextTagStart, StringComparison.OrdinalIgnoreCase);
                        int nextTag = -1;
                        string elseifCondition = null;

                        if ((elseifStart != -1 && elseifStart < endifStart) &&
                            (elseStart == -1 || elseifStart < elseStart))
                        {
                            nextTag = elseifStart;
                            int elseifEnd = input.IndexOf("?>", elseifStart, StringComparison.OrdinalIgnoreCase);
                            if (elseifEnd == -1)
                            {
                                throw new InvalidOperationException("Malformed <?elseif?> block.");
                            }

                            elseifCondition = input.Substring(elseifStart + 8, elseifEnd - (elseifStart + 8)).Trim();
                            blockBoundaries.Add((blockStart, elseifStart, currentCondition, false));
                            blockStart = elseifEnd + 2;
                            nextTagStart = blockStart;
                            currentCondition = elseifCondition;
                        }
                        else if (elseStart != -1 && elseStart < endifStart)
                        {
                            nextTag = elseStart;
                            blockBoundaries.Add((blockStart, elseStart, currentCondition, false));
                            blockStart = elseStart + 8;
                            nextTagStart = blockStart;
                            currentCondition = null;
                            break;
                        }
                        else
                        {
                            // No more elseif/else before endif
                            blockBoundaries.Add((blockStart, endifStart, currentCondition, false));
                            break;
                        }
                    }

                    // If there was an else, add its block
                    if (currentCondition == null)
                    {
                        blockBoundaries.Add((blockStart, endifStart, null, true));
                    }

                    // Find the first block whose condition evaluates to true, or else block
                    string selectedContent = "";
                    foreach (var (start, end, cond, isElse) in blockBoundaries)
                    {
                        if (cond == null && isElse)
                        {
                            selectedContent = input.Substring(start, end - start);
                            break;
                        }
                        else if (cond != null && EvaluateCondition(cond))
                        {
                            selectedContent = input.Substring(start, end - start);
                            break;
                        }
                    }

                    output.Append(selectedContent);

                    // Ingest variables from the selected content asap as
                    // variables could be referenced in subsequent blocks or conditions.
                    IngestDefineVariablesFromString(selectedContent);

                    pos = endifStart + 9; // Move past <?endif?>
                }
                else if (blockType == "ifdef" || blockType == "ifndef")
                {
                    int tagEnd = input.IndexOf("?>", nextBlockStart, StringComparison.OrdinalIgnoreCase);
                    if (tagEnd == -1) throw new InvalidOperationException($"Malformed <?{blockType}?> block.");
                    string variableName = input.Substring(nextBlockStart + (blockType == "ifdef" ? 7 : 8), tagEnd - (nextBlockStart + (blockType == "ifdef" ? 7 : 8))).Trim();

                    bool isDefined;
                    if (variableName.StartsWith("$(var.", StringComparison.OrdinalIgnoreCase) && variableName.EndsWith(")"))
                    {
                        // Extract the variable name inside $(var.something)
                        string innerVar = variableName.Substring(6, variableName.Length - 7);
                        isDefined = _defineVariablesDictionary.ContainsKey(innerVar);
                    }
                    else if (variableName.StartsWith("$(") && variableName.EndsWith(")"))
                    {
                        // Extract the variable name inside $(something)
                        string innerConst = variableName.Substring(2, variableName.Length - 3);
                        isDefined = _defineConstantsDictionary.ContainsKey(innerConst);
                    }
                    else
                    {
                        // Fallback: treat as a plain variable name
                        isDefined = _defineConstantsDictionary.ContainsKey(variableName);
                    }

                    int endifStart = input.IndexOf("<?endif?>", tagEnd + 2, StringComparison.OrdinalIgnoreCase);
                    if (endifStart == -1)
                    {
                        throw new InvalidOperationException($"Missing <?endif?> for <?{blockType}?> block.");
                    }

                    int blockStart = tagEnd + 2;
                    int blockEnd = endifStart;

                    bool keepBlock = (blockType == "ifdef" && isDefined) || (blockType == "ifndef" && !isDefined);

                    if (keepBlock)
                    {
                        string selectedContent = input.Substring(blockStart, blockEnd - blockStart);
                        output.Append(selectedContent);

                        // Ingest variables from the selected content asap as
                        // variables could be referenced in subsequent blocks or conditions.
                        IngestDefineVariablesFromString(selectedContent);
                    }
                    pos = endifStart + 9; // Move past <?endif?>
                }
            }

            File.WriteAllText(sourceFile, output.ToString());
        }

        /// <summary>
        /// Simple conditions evaluator for Wix preprocessor conditions.
        /// Supports =, != (case-sensitive), ~= (case-insensitive), and quoted/unquoted values and variables.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private bool EvaluateCondition(string condition)
        {
            condition = condition.Trim();

            // Supports both:
            // $(Variable) <op> "value"
            // and
            // "value" <op> $(Variable)
            // Supports all combinations of quoted and unquoted values and variables.

            // Regex for both forms: variable on left or right, quoted or unquoted
            var eqMatch = Regex.Match(
                condition,
                @"^(?:(?:""?\$\(([^)]+)\)""?)\s*(=|!=|~=)\s*(?:""([^""]+)""|([^\s]+))|(?:""([^""]+)""|([^\s]+))\s*(=|!=|~=)\s*(""?\$\(([^)]+)\)""?))$"
            );

            if (!eqMatch.Success)
            {
                throw new NotSupportedException($"Unsupported condition: {condition}");
            }

            string varName, op, value, actualValue = "";

            if (!string.IsNullOrEmpty(eqMatch.Groups[1].Value))
            {
                // $(Variable) or "$(Variable)" <op> value
                varName = eqMatch.Groups[1].Value;
                op = eqMatch.Groups[2].Value;
                value = eqMatch.Groups[3].Success ? eqMatch.Groups[3].Value : eqMatch.Groups[4].Value;
            }
            else
            {
                // value <op> $(Variable) or "$(Variable)"
                value = eqMatch.Groups[5].Success ? eqMatch.Groups[5].Value : eqMatch.Groups[6].Value;
                op = eqMatch.Groups[7].Value;
                varName = eqMatch.Groups[9].Value;
            }

            // Trim quotes if present
            varName = varName.Trim('"');

            if (varName.StartsWith("var.", StringComparison.OrdinalIgnoreCase))
            {
                varName = varName.Substring(4);
                _defineVariablesDictionary.TryGetValue(varName, out actualValue);
            }
            else if (varName.StartsWith("sys.", StringComparison.OrdinalIgnoreCase))
            {
                varName = varName.Substring(4);
                _systemVariablesDictionary.TryGetValue(varName, out actualValue);
            }

            // Fallback to _defineConstantsDictionary if not found in variables
            if (actualValue == null || actualValue == "")
            {
                _defineConstantsDictionary.TryGetValue(varName, out actualValue);
            }

            switch (op)
            {
                case "=":
                    return string.Equals(actualValue, value, StringComparison.Ordinal);
                case "!=":
                    return !string.Equals(actualValue, value, StringComparison.Ordinal);
                case "~=":
                    return string.Equals(actualValue, value, StringComparison.OrdinalIgnoreCase);
                default:
                    throw new NotSupportedException($"Unsupported operator: {op}");
            }
        }

        private string CopySourceFile(string fileId, string source, string relativeRoot = "")
        {
            var destDir = Path.Combine(WixpackWorkingDir, fileId);
            Directory.CreateDirectory(destDir);

            source = GetAbsoluteSourcePath(source, relativeRoot);

            if (File.Exists(source))
            {
                var destPath = Path.Combine(destDir, Path.GetFileName(source));
                File.Copy(source, destPath, overwrite: true);
                return destPath;
            }
            else
            {
                throw new FileNotFoundException($"Source file not found: {source}");
            }
        }

        private void CopyExtensions()
        {
            for (int i = 0; i < Extensions.Length; i++)
            {
                var extensionPath = Extensions[i].ItemSpec;
                string filename = Path.GetFileName(extensionPath);
                CopySourceFile(filename, extensionPath);

                // Update the extension item spec to the new relative path
                Extensions[i] = new TaskItem(Path.Combine(filename, filename));
            }
        }

        private void CopyBindPathContents()
        {
            for (int i = 0; i < BindPaths?.Length; i++)
            {
                // Skip items where BindName metadata is not present
                string bindName = BindPaths[i].GetMetadata("BindName");
                if (!string.IsNullOrEmpty(bindName))
                {
                    string wixpackSubfolder = Path.GetRandomFileName();
                    string bindPath = BindPaths[i].ItemSpec;

                    foreach (string file in Directory.GetFiles(bindPath, "*", SearchOption.TopDirectoryOnly))
                    {
                        // Copy known usable files only
                        // .dll, .exe, .msi
                        if (file.EndsWith(".dll") ||
                            file.EndsWith(".exe") ||
                            file.EndsWith(".msi"))
                        {
                            CopySourceFile(wixpackSubfolder, file);
                        }
                    }

                    // Update the bind path item spec to the new relative folder
                    BindPaths[i].ItemSpec = wixpackSubfolder;
                    continue;
                }
            }
        }

        private void CopyLocalizationFiles()
        {
            for (int i = 0; i < LocalizationFiles?.Length; i++)
            {
                var localizationPath = LocalizationFiles[i].ItemSpec;
                string filename = Path.GetFileName(localizationPath);
                CopySourceFile(filename, localizationPath);

                // Update the localization item spec to the new relative path
                LocalizationFiles[i] = new TaskItem(Path.Combine(filename, filename));
            }
        }

        private string GetAbsoluteSourcePath(string source, string relativeRoot = "")
        {
            // If the source is relative, resolve it against the project directory
            if (!Path.IsPathRooted(source))
            {
                return string.IsNullOrEmpty(relativeRoot) ?
                    Path.Combine(_wixprojDir, source) :
                    Path.Combine(relativeRoot, source);
            }

            return source;
        }

        private static void CopyDirectoryRecursive(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), overwrite: true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                CopyDirectoryRecursive(dir, Path.Combine(destDir, Path.GetFileName(dir)));
            }
        }
    }
}
