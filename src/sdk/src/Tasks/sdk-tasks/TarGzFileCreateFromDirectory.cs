// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Runtime.InteropServices;

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
        /// Optional path to the tar executable. If not specified, uses 'tar' from PATH.
        /// </summary>
        public string TarToolPath { get; set; }

        /// <summary>
        /// If zipping an entire folder without exclusion patterns, whether to include the folder in the archive.
        /// </summary>
        public bool IncludeBaseDirectory { get; set; }

        /// <summary>
        /// An item group of regular expressions for content to exclude from the archive.
        /// </summary>
        public ITaskItem[] ExcludePatterns { get; set; }

        public bool IgnoreExitCode { get; set; }

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            int returnCode = base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);

            if (IgnoreExitCode)
            {
                returnCode = 0;
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

            // Log diagnostic information about tar and OS
            if (retVal)
            {
                LogEnvironmentInfo();
            }

            return retVal;
        }

        private void LogEnvironmentInfo()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "=== TAR ENVIRONMENT INFO ===");

                // OS Information
                Log.LogMessage(MessageImportance.High, $"OS: {RuntimeInformation.OSDescription}");
                Log.LogMessage(MessageImportance.High, $"OS Platform: {(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS" : "Unknown")}");
                Log.LogMessage(MessageImportance.High, $"OS Architecture: {RuntimeInformation.OSArchitecture}");
                Log.LogMessage(MessageImportance.High, $"OS Version: {Environment.OSVersion}");

                // Windows-specific information
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    LogWindowsInfo();
                }

                // Try to get tar version
                LogTarVersion();

                Log.LogMessage(MessageImportance.High, "=== END TAR ENVIRONMENT INFO ===");
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.Normal, $"Failed to log environment info: {ex.Message}");
            }
        }

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        private void LogWindowsInfo()
        {
            try
            {
                // Get Windows edition/SKU from registry
                var productName = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "Unknown");
                var editionId = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Unknown");
                var currentBuild = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild", "Unknown");
                var releaseId = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "Unknown");
                var displayVersion = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "Unknown");

                Log.LogMessage(MessageImportance.High, $"Windows Product: {productName}");
                Log.LogMessage(MessageImportance.High, $"Windows Edition: {editionId}");
                Log.LogMessage(MessageImportance.High, $"Windows Build: {currentBuild}");
                Log.LogMessage(MessageImportance.High, $"Windows Release: {releaseId}");
                Log.LogMessage(MessageImportance.High, $"Windows Display Version: {displayVersion}");
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.Normal, $"Failed to get Windows info: {ex.Message}");
            }
        }

        private void LogTarVersion()
        {
            try
            {
                string tarPath = !string.IsNullOrEmpty(TarToolPath) ? TarToolPath : "tar";

                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tarPath,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        var output = process.StandardOutput.ReadToEnd();
                        var error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                        {
                            // Only log the first line which typically contains the version
                            var firstLine = output.Split('\n')[0].Trim();
                            Log.LogMessage(MessageImportance.High, $"Tar Path: {tarPath}");
                            Log.LogMessage(MessageImportance.High, $"Tar Version: {firstLine}");
                        }
                        else
                        {
                            Log.LogMessage(MessageImportance.Normal, $"Could not determine tar version. Exit code: {process.ExitCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.Normal, $"Failed to get tar version: {ex.Message}");
            }
        }

        public override bool Execute() => base.Execute();

        protected override string ToolName => "tar";

        protected override MessageImportance StandardOutputLoggingImportance => MessageImportance.High;

        protected override string GenerateFullPathToTool()
        {
            // Use custom tar path if provided, otherwise fall back to 'tar' from PATH
            if (!string.IsNullOrEmpty(TarToolPath))
            {
                return TarToolPath;
            }
            return "tar";
        }

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
