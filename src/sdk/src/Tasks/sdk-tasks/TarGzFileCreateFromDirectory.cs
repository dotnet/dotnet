// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.DotNet.Build.Tasks
{
    public sealed class TarGzFileCreateFromDirectory : ToolTask
    {
        /// <summary>
        /// The path to the directory to be archived.
        /// </summary>
        [Required]
        public string SourceDirectory { get; set; }

        /// <summary>
        /// The path of the archive to be created.
        /// </summary>
        [Required]
        public string DestinationArchive { get; set; }

        /// <summary>
        /// Indicates if the destination archive should be overwritten if it already exists.
        /// </summary>
        public bool OverwriteDestination { get; set; }

        /// <summary>
        /// If zipping an entire folder without exclusion patterns, whether to include the folder in the archive.
        /// </summary>
        public bool IncludeBaseDirectory { get; set; }

        /// <summary>
        /// An item group of regular expressions for content to exclude from the archive.
        /// </summary>
        public ITaskItem[] ExcludePatterns { get; set; }

        public bool IgnoreExitCode { get; set; }

        // P/Invoke for hardlink detection on Windows
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetFileInformationByHandle(
            SafeFileHandle hFile,
            out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [StructLayout(LayoutKind.Sequential)]
        private struct BY_HANDLE_FILE_INFORMATION
        {
            public uint FileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }

        private (uint, uint, uint) GetFileIdentity(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var handle = fileStream.SafeFileHandle)
                {
                    if (GetFileInformationByHandle(handle, out BY_HANDLE_FILE_INFORMATION fileInfo))
                    {
                        return (fileInfo.NumberOfLinks, fileInfo.FileIndexHigh, fileInfo.FileIndexLow);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.Low, $"Could not get file identity for {filePath}: {ex.Message}");
            }
            return (0, 0, 0);
        }

        private void LogDirectoryStructure(string baseDirectory, string description)
        {
            Log.LogMessage(MessageImportance.High, $"==================== {description} ====================");
            Log.LogMessage(MessageImportance.High, $"Base Directory: {baseDirectory}");

            var hardlinkGroups = new Dictionary<(uint, uint), List<string>>();

            try
            {
                var allFiles = Directory.GetFiles(baseDirectory, "*", SearchOption.AllDirectories);
                var allDirectories = Directory.GetDirectories(baseDirectory, "*", SearchOption.AllDirectories);

                Log.LogMessage(MessageImportance.High, $"Total Directories: {allDirectories.Length}");
                Log.LogMessage(MessageImportance.High, $"Total Files: {allFiles.Length}");
                Log.LogMessage(MessageImportance.High, "");

                // Log directory structure
                Log.LogMessage(MessageImportance.High, "Directory Structure:");
                foreach (var dir in allDirectories.OrderBy(d => d))
                {
                    var relativePath = Path.GetRelativePath(baseDirectory, dir);
                    Log.LogMessage(MessageImportance.High, $"  [DIR]  {relativePath}");
                }
                Log.LogMessage(MessageImportance.High, "");

                // Log files and collect hardlink information
                Log.LogMessage(MessageImportance.High, "Files:");
                foreach (var file in allFiles.OrderBy(f => f))
                {
                    var relativePath = Path.GetRelativePath(baseDirectory, file);
                    var fileInfo = new FileInfo(file);
                    var (linkCount, indexHigh, indexLow) = GetFileIdentity(file);

                    var linkInfo = linkCount > 1 ? $" [HARDLINK: {linkCount} links]" : "";
                    Log.LogMessage(MessageImportance.High, $"  [FILE] {relativePath} ({fileInfo.Length} bytes){linkInfo}");

                    if (linkCount > 1)
                    {
                        var key = (indexHigh, indexLow);
                        if (!hardlinkGroups.ContainsKey(key))
                        {
                            hardlinkGroups[key] = new List<string>();
                        }
                        hardlinkGroups[key].Add(relativePath);
                    }
                }

                // Log hardlink groups
                if (hardlinkGroups.Count > 0)
                {
                    Log.LogMessage(MessageImportance.High, "");
                    Log.LogMessage(MessageImportance.High, "Hardlink Groups:");
                    int groupNum = 1;
                    foreach (var group in hardlinkGroups.Values)
                    {
                        Log.LogMessage(MessageImportance.High, $"  Group {groupNum} ({group.Count} files):");
                        foreach (var file in group)
                        {
                            Log.LogMessage(MessageImportance.High, $"    - {file}");
                        }
                        groupNum++;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error logging directory structure: {ex.Message}");
                Log.LogError($"Stack trace: {ex.StackTrace}");
            }

            Log.LogMessage(MessageImportance.High, $"==================== END {description} ====================");
            Log.LogMessage(MessageImportance.High, "");
        }

        private void LogTarballContents(string tarballPath)
        {
            Log.LogMessage(MessageImportance.High, $"==================== TARBALL CONTENTS: {tarballPath} ====================");

            try
            {
                var fileInfo = new FileInfo(tarballPath);
                Log.LogMessage(MessageImportance.High, $"Tarball Size: {fileInfo.Length} bytes");
                Log.LogMessage(MessageImportance.High, "");

                // Use tar to list contents
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "tar",
                    Arguments = $"-tzf \"{tarballPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    var output = process.StandardOutput.ReadToEnd();
                    var errors = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Log.LogMessage(MessageImportance.High, "Tarball Contents:");
                        var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        Log.LogMessage(MessageImportance.High, $"Total Entries: {lines.Length}");
                        foreach (var line in lines)
                        {
                            Log.LogMessage(MessageImportance.High, $"  {line}");
                        }

                        // Now get verbose listing with hardlink information
                        startInfo.Arguments = $"-tvzf \"{tarballPath}\"";
                        using (var verboseProcess = System.Diagnostics.Process.Start(startInfo))
                        {
                            var verboseOutput = verboseProcess.StandardOutput.ReadToEnd();
                            verboseProcess.WaitForExit();

                            if (verboseProcess.ExitCode == 0)
                            {
                                Log.LogMessage(MessageImportance.High, "");
                                Log.LogMessage(MessageImportance.High, "Detailed Listing (with hardlinks):");
                                var verboseLines = verboseOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var line in verboseLines)
                                {
                                    Log.LogMessage(MessageImportance.High, $"  {line}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Log.LogError($"Failed to list tarball contents. Exit code: {process.ExitCode}");
                        if (!string.IsNullOrEmpty(errors))
                        {
                            Log.LogError($"Errors: {errors}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Error listing tarball contents: {ex.Message}");
                Log.LogError($"Stack trace: {ex.StackTrace}");
            }

            Log.LogMessage(MessageImportance.High, $"==================== END TARBALL CONTENTS ====================");
            Log.LogMessage(MessageImportance.High, "");
        }

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            int returnCode = base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);

            if (IgnoreExitCode)
            {
                returnCode = 0;
            }

            // Immediately after tar completes, capture its hash to detect later modification
            if (returnCode == 0 && File.Exists(DestinationArchive))
            {
                try
                {
                    using (var stream = File.OpenRead(DestinationArchive))
                    using (var sha = System.Security.Cryptography.SHA256.Create())
                    {
                        var hash = sha.ComputeHash(stream);
                        var hashString = BitConverter.ToString(hash).Replace("-", "");
                        Log.LogMessage(MessageImportance.High, "");
                        Log.LogMessage(MessageImportance.High, $"TARBALL CREATED - IMMEDIATE POST-TAR STATE:");
                        Log.LogMessage(MessageImportance.High, $"  Path: {DestinationArchive}");
                        Log.LogMessage(MessageImportance.High, $"  Size: {new FileInfo(DestinationArchive).Length} bytes");
                        Log.LogMessage(MessageImportance.High, $"  SHA256: {hashString}");
                        Log.LogMessage(MessageImportance.High, $"  Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                        Log.LogMessage(MessageImportance.High, "");
                    }
                }
                catch (Exception ex)
                {
                    Log.LogWarning($"Could not compute tarball hash: {ex.Message}");
                }
            }

            return returnCode;
        }

        protected override bool ValidateParameters()
        {
            base.ValidateParameters();

            var retVal = true;

            if (File.Exists(DestinationArchive))
            {
                if (OverwriteDestination == true)
                {
                    Log.LogMessage(MessageImportance.Low, $"{DestinationArchive} will be overwritten");
                }
                else
                {
                    Log.LogError($"'{DestinationArchive}' already exists. Did you forget to set '{nameof(OverwriteDestination)}' to true?");

                    retVal = false;
                }
            }

            SourceDirectory = Path.GetFullPath(SourceDirectory);

            SourceDirectory = SourceDirectory.EndsWith(Path.DirectorySeparatorChar.ToString())
                ? SourceDirectory
                : SourceDirectory + Path.DirectorySeparatorChar;

            if (!Directory.Exists(SourceDirectory))
            {
                Log.LogError($"SourceDirectory '{SourceDirectory} does not exist.");

                retVal = false;
            }

            return retVal;
        }

        public override bool Execute()
        {
            // Log source directory structure before creating tarball
            // This happens AFTER zip creation on Windows, so it will show if zip had side effects
            Log.LogMessage(MessageImportance.High, "");
            Log.LogMessage(MessageImportance.High, "##############################################################");
            Log.LogMessage(MessageImportance.High, "## DIAGNOSTIC LOGGING: PRE-TAR CREATION");
            Log.LogMessage(MessageImportance.High, $"## NOTE: On Windows, zip creation has already completed");
            Log.LogMessage(MessageImportance.High, $"## This logging occurs immediately before tar execution");
            Log.LogMessage(MessageImportance.High, "##############################################################");
            LogDirectoryStructure(SourceDirectory, "SOURCE DIRECTORY STRUCTURE (IMMEDIATE PRE-TAR)");

            // Execute the tar command
            bool result = base.Execute();

            // Log tarball contents after creation
            if (result && File.Exists(DestinationArchive))
            {
                Log.LogMessage(MessageImportance.High, "");
                Log.LogMessage(MessageImportance.High, "##############################################################");
                Log.LogMessage(MessageImportance.High, "## DIAGNOSTIC LOGGING: POST-TAR CREATION");
                Log.LogMessage(MessageImportance.High, "##############################################################");
                LogTarballContents(DestinationArchive);

                // Log source directory AGAIN after tar to see if tar had side effects
                Log.LogMessage(MessageImportance.High, "");
                Log.LogMessage(MessageImportance.High, "##############################################################");
                Log.LogMessage(MessageImportance.High, "## DIAGNOSTIC LOGGING: POST-TAR SOURCE VERIFICATION");
                Log.LogMessage(MessageImportance.High, "##############################################################");
                LogDirectoryStructure(SourceDirectory, "SOURCE DIRECTORY STRUCTURE (POST-TAR VERIFICATION)");

                // Final hash capture before returning - to detect any modifications after logging
                try
                {
                    using (var stream = File.OpenRead(DestinationArchive))
                    using (var sha = System.Security.Cryptography.SHA256.Create())
                    {
                        var hash = sha.ComputeHash(stream);
                        var hashString = BitConverter.ToString(hash).Replace("-", "");
                        Log.LogMessage(MessageImportance.High, "");
                        Log.LogMessage(MessageImportance.High, "##############################################################");
                        Log.LogMessage(MessageImportance.High, "## FINAL TARBALL STATE - END OF TASK EXECUTION");
                        Log.LogMessage(MessageImportance.High, "##############################################################");
                        Log.LogMessage(MessageImportance.High, $"  Path: {DestinationArchive}");
                        Log.LogMessage(MessageImportance.High, $"  Size: {new FileInfo(DestinationArchive).Length} bytes");
                        Log.LogMessage(MessageImportance.High, $"  SHA256: {hashString}");
                        Log.LogMessage(MessageImportance.High, $"  Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                        Log.LogMessage(MessageImportance.High, "");
                        Log.LogMessage(MessageImportance.High, "NOTE: If this hash differs from the 'IMMEDIATE POST-TAR STATE' hash above,");
                        Log.LogMessage(MessageImportance.High, "      something modified the tarball during the logging operations.");
                        Log.LogMessage(MessageImportance.High, "NOTE: If a later build step shows a different hash, the tarball was modified");
                        Log.LogMessage(MessageImportance.High, "      AFTER this task completed (e.g., by signing infrastructure).");
                        Log.LogMessage(MessageImportance.High, "##############################################################");
                    }
                }
                catch (Exception ex)
                {
                    Log.LogWarning($"Could not compute final tarball hash: {ex.Message}");
                }
            }

            return result;
        }

        protected override string ToolName => "tar";

        protected override MessageImportance StandardOutputLoggingImportance => MessageImportance.High;

        protected override string GenerateFullPathToTool() => "tar";

        protected override string GenerateCommandLineCommands() => $"{GetDestinationArchive()} {GetSourceSpecification()}";

        private string GetSourceSpecification()
        {
            if (IncludeBaseDirectory)
            {
                var parentDirectory = Directory.GetParent(SourceDirectory).Parent.FullName;
                var sourceDirectoryName = Path.GetFileName(Path.GetDirectoryName(SourceDirectory));
                return $"--directory {parentDirectory} {sourceDirectoryName}  {GetExcludes()}";
            }
            else
            {
                return $"--directory {SourceDirectory}  {GetExcludes()} \".\"";
            }
        }

        private string GetDestinationArchive() => $"-czf {DestinationArchive}";

        private string GetExcludes()
        {
            var excludes = string.Empty;

            if (ExcludePatterns != null)
            {
                foreach (var excludeTaskItem in ExcludePatterns)
                {
                    excludes += $" --exclude {excludeTaskItem.ItemSpec}";
                }
            }
            
            return excludes;
        }

        protected override void LogToolCommand(string message) => base.LogToolCommand($"{GetWorkingDirectory()}> {message}");
    }
}
