// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Formats.Tar;

namespace Microsoft.DotNet.Build.Tasks
{
    public sealed class TarGzFileCreateFromDirectory : Task
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

        private bool ValidateParameters()
        {
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

            // Diagnostic: Log filesystem state before creating tarball (after parameters are validated)
            if (retVal)
            {
                LogFilesystemState();
            }

            return retVal;
        }

        public override bool Execute()
        {
            if (!ValidateParameters())
            {
                return false;
            }

            // Use .NET's System.Formats.Tar API on all platforms for consistency and to avoid tar bugs
            return ExecuteWithDotNetTarApi();
        }

        private bool ExecuteWithDotNetTarApi()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, $"Creating tar.gz archive using .NET API: {DestinationArchive}");

                using (var fileStream = new FileStream(DestinationArchive, FileMode.Create, FileAccess.Write))
                using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionLevel.Optimal))
                {
                    System.Formats.Tar.TarFile.CreateFromDirectory(
                        SourceDirectory,
                        gzipStream,
                        includeBaseDirectory: false);
                }

                Log.LogMessage(MessageImportance.High, $"Successfully created archive: {DestinationArchive}");
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError($"Failed to create tar.gz archive: {ex.Message}");
                return false;
            }
        }

        private void LogFilesystemState()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "=== PRE-TARBALL FILESYSTEM STATE ===");
                Log.LogMessage(MessageImportance.High, $"SourceDirectory: {SourceDirectory}");

                // Log top-level directories
                var topLevelDirs = Directory.GetDirectories(SourceDirectory)
                    .Select(d => Path.GetFileName(d))
                    .OrderBy(d => d)
                    .ToList();

                Log.LogMessage(MessageImportance.High, "Top-level directories:");
                foreach (var dir in topLevelDirs)
                {
                    Log.LogMessage(MessageImportance.High, $"  - {dir}");
                }

                // Log hard links on Windows
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    LogHardLinks();
                }

                Log.LogMessage(MessageImportance.High, "=== END PRE-TARBALL STATE ===");
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.Normal, $"Failed to log filesystem state: {ex.Message}");
            }
        }

        private void LogHardLinks()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Hard link analysis:");

                // Group files by their file index (inode equivalent on Windows)
                var filesByIndex = new Dictionary<(uint volumeSerial, ulong fileIndex), List<string>>();

                foreach (var file in Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories))
                {
                    var info = GetFileIdentity(file);
                    if (info.HasValue)
                    {
                        var key = (info.Value.volumeSerial, info.Value.fileIndex);
                        if (!filesByIndex.ContainsKey(key))
                        {
                            filesByIndex[key] = new List<string>();
                        }
                        filesByIndex[key].Add(file);
                    }
                }

                // Log hard link groups (files with same index = hard linked together)
                var hardLinkGroups = filesByIndex.Where(kvp => kvp.Value.Count > 1).Take(10).ToList();

                Log.LogMessage(MessageImportance.High, $"Found {hardLinkGroups.Count} hard link groups (showing first 10):");

                foreach (var group in hardLinkGroups)
                {
                    Log.LogMessage(MessageImportance.High, $"  Hard link group (file index: {group.Key.fileIndex}):");
                    foreach (var file in group.Value)
                    {
                        var relativePath = file.Substring(SourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        Log.LogMessage(MessageImportance.High, $"    - {relativePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.Normal, $"Failed to log hard links: {ex.Message}");
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileInformationByHandle(
            IntPtr hFile,
            out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
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

        private (uint volumeSerial, ulong fileIndex)? GetFileIdentity(string filePath)
        {
            const uint GENERIC_READ = 0x80000000;
            const uint FILE_SHARE_READ = 0x00000001;
            const uint OPEN_EXISTING = 3;
            IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

            IntPtr handle = CreateFile(filePath, GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            if (handle == INVALID_HANDLE_VALUE)
            {
                return null;
            }

            try
            {
                if (GetFileInformationByHandle(handle, out BY_HANDLE_FILE_INFORMATION fileInfo))
                {
                    ulong fileIndex = ((ulong)fileInfo.FileIndexHigh << 32) | fileInfo.FileIndexLow;
                    return (fileInfo.VolumeSerialNumber, fileIndex);
                }
                return null;
            }
            finally
            {
                CloseHandle(handle);
            }
        }
    }
}
