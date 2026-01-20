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

            return retVal;
        }

        public override bool Execute()
        {
            // Validate parameters first
            if (!ValidateParameters())
            {
                return false;
            }

            // On Windows, use Docker to run tar in a container to work around old tar versions
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return ExecuteInDockerContainer();
            }

            return base.Execute();
        }

        private bool ExecuteInDockerContainer()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Running tar in Windows Server Core container to work around tar version issues");

                // Ensure destination directory exists
                var destDir = Path.GetDirectoryName(Path.GetFullPath(DestinationArchive));
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                // Normalize paths
                var fullDestPath = Path.GetFullPath(DestinationArchive);
                var destDirectory = Path.GetDirectoryName(fullDestPath);
                var destFileName = Path.GetFileName(fullDestPath);

                // Container paths
                var containerSourcePath = "C:\\source";
                var containerOutputPath = "C:\\output";
                var containerDestFile = $"{containerOutputPath}\\{destFileName}";

                // Determine source path and tar command based on IncludeBaseDirectory
                string mountSourcePath;
                string tarArgs;

                if (IncludeBaseDirectory)
                {
                    // Mount the parent directory so we can include the base directory name
                    var parentDirectory = Directory.GetParent(SourceDirectory).Parent.FullName;
                    var sourceDirectoryName = Path.GetFileName(Path.GetDirectoryName(SourceDirectory));
                    mountSourcePath = parentDirectory;
                    tarArgs = $"-czf {containerDestFile} --directory {containerSourcePath} {sourceDirectoryName} {GetExcludes()}";

                    Log.LogMessage(MessageImportance.High, $"Including base directory '{sourceDirectoryName}' in archive");
                    Log.LogMessage(MessageImportance.High, $"Mounting parent directory: {mountSourcePath}");
                }
                else
                {
                    // Mount the source directory directly
                    mountSourcePath = Path.GetFullPath(SourceDirectory);
                    tarArgs = $"-czf {containerDestFile} --directory {containerSourcePath} {GetExcludes()} .";

                    Log.LogMessage(MessageImportance.High, $"Archiving contents of: {mountSourcePath}");
                }

                // Pull the Docker image first
                var imageName = "mcr.microsoft.com/windows/servercore:ltsc2022";
                Log.LogMessage(MessageImportance.High, $"Pulling Docker image: {imageName}");

                var pullStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"pull {imageName}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var pullProcess = System.Diagnostics.Process.Start(pullStartInfo))
                {
                    if (pullProcess == null)
                    {
                        Log.LogError("Failed to start docker pull process");
                        return false;
                    }

                    var pullOutput = pullProcess.StandardOutput.ReadToEnd();
                    var pullError = pullProcess.StandardError.ReadToEnd();

                    pullProcess.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(pullOutput))
                    {
                        Log.LogMessage(MessageImportance.High, $"Docker pull output: {pullOutput}");
                    }

                    if (!string.IsNullOrWhiteSpace(pullError))
                    {
                        Log.LogMessage(MessageImportance.High, $"Docker pull stderr: {pullError}");
                    }

                    if (pullProcess.ExitCode != 0)
                    {
                        Log.LogError($"Docker pull failed with exit code {pullProcess.ExitCode}");
                        return false;
                    }
                }

                // Build docker command
                var dockerArgs = $"run --rm " +
                    $"-v \"{mountSourcePath}:{containerSourcePath}\" " +
                    $"-v \"{destDirectory}:{containerOutputPath}\" " +
                    $"{imageName} " +
                    $"tar {tarArgs}";

                Log.LogMessage(MessageImportance.High, $"Docker command: docker {dockerArgs}");

                // Execute docker
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = dockerArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        Log.LogError("Failed to start docker process");
                        return false;
                    }

                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        Log.LogMessage(MessageImportance.High, $"Docker output: {output}");
                    }

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Log.LogMessage(MessageImportance.High, $"Docker stderr: {error}");
                    }

                    if (process.ExitCode != 0 && !IgnoreExitCode)
                    {
                        Log.LogError($"Docker command failed with exit code {process.ExitCode}");
                        return false;
                    }

                    Log.LogMessage(MessageImportance.High, $"Successfully created archive: {fullDestPath}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Failed to execute tar in Docker container: {ex.Message}");
                Log.LogMessage(MessageImportance.High, $"Exception details: {ex}");
                return false;
            }
        }

        protected override string ToolName => "tar";

        protected override MessageImportance StandardOutputLoggingImportance => MessageImportance.High;

        protected override string GenerateFullPathToTool()
        {
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
