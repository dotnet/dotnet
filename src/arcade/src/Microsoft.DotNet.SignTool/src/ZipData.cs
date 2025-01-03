// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Data;
using System.Diagnostics;
using Microsoft.DotNet.Build.Tasks.Installers;

#if NET472
using System.IO.Packaging;
#else
using System.Formats.Tar;
#endif

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// Data for a zip container. Can refer to any zip format such as VSIX, NuPkg, or a raw zip archive.
    /// </summary>
    internal sealed class ZipData
    {
        /// <summary>
        /// Signing information.
        /// </summary>
        internal FileSignInfo FileSignInfo { get; }

        /// <summary>
        /// The parts inside this container which need to be signed.
        /// </summary>
        internal ImmutableDictionary<string, ZipPart> NestedParts { get; }

        internal ZipData(FileSignInfo fileSignInfo, ImmutableDictionary<string, ZipPart> nestedBinaryParts)
        {
            FileSignInfo = fileSignInfo;
            NestedParts = nestedBinaryParts;
        }

        internal ZipPart? FindNestedPart(string relativeName)
        {
            if (NestedParts.TryGetValue(relativeName, out ZipPart part))
            {
                return part;
            }

            return null;
        }

        public static IEnumerable<(string relativePath, Stream content, long contentSize)> ReadEntries(string archivePath, string tempDir, string tarToolPath, bool ignoreContent = false)
        {
            if (FileSignInfo.IsTarGZip(archivePath))
            {
                // Tar APIs not available on .NET FX. We need sign tool to run on desktop msbuild because building VSIX packages requires desktop.
#if NET472
                return ReadTarGZipEntries(archivePath, tempDir, tarToolPath, ignoreContent);
#else
                return ReadTarGZipEntries(archivePath)
                    .Select(entry => (entry.Name, entry.DataStream, entry.Length));
#endif
            }
            else if (FileSignInfo.IsDeb(archivePath))
            {
#if NET472
                throw new NotImplementedException("Debian signing is not supported on .NET Framework");
#else
                return ReadDebContainerEntries(archivePath, "data.tar");
#endif
            }

            return ReadZipEntries(archivePath);
        }

        /// <summary>
        /// Repack the zip container with the signed files.
        /// </summary>
        public void Repack(TaskLoggingHelper log, string tempDir, string wixToolsPath, string tarToolPath)
        {
#if NET472
            if (FileSignInfo.IsVsix())
            {
                RepackPackage(log);
            }
            else
#endif
            if (FileSignInfo.IsTarGZip())
            {
                RepackTarGZip(log, tempDir, tarToolPath);
            }
            else if (FileSignInfo.IsWixContainer())
            {
                RepackWixPack(log, tempDir, wixToolsPath);
            }
            else if (FileSignInfo.IsDeb())
            {
#if NET472
                throw new NotImplementedException("Debian signing is not supported on .NET Framework");
#else
                RepackDebContainer(log, tempDir);
#endif
            }
            else 
            {
                RepackRawZip(log);
            }
        }

#if NET472
        /// <summary>
        /// Repack a zip container with a package structure.
        /// </summary>
        private void RepackPackage(TaskLoggingHelper log)
        {
            string getPartRelativeFileName(PackagePart part)
            {
                var path = part.Uri.OriginalString;
                if (!string.IsNullOrEmpty(path) && path[0] == '/')
                {
                    path = path.Substring(1);
                }

                return path;
            }
            
            using (var package = Package.Open(FileSignInfo.FullPath, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (var part in package.GetParts())
                {
                    var relativeName = getPartRelativeFileName(part);
                    var signedPart = FindNestedPart(relativeName);
                    if (!signedPart.HasValue)
                    {
                        log.LogMessage(MessageImportance.Low, $"Didn't find signed part for nested file: {FileSignInfo.FullPath} -> {relativeName}");
                        continue;
                    }

                    using (var signedStream = File.OpenRead(signedPart.Value.FileSignInfo.FullPath))
                    using (var partStream = part.GetStream(FileMode.Open, FileAccess.ReadWrite))
                    {
                        log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {FileSignInfo.FullPath} -> {relativeName}.");

                        signedStream.CopyTo(partStream);
                        partStream.SetLength(signedStream.Length);
                    }
                }
            }
        }
#endif

        private static IEnumerable<(string relativePath, Stream content, long contentSize)> ReadZipEntries(string archivePath)
        {
            using var archive = new ZipArchive(File.OpenRead(archivePath), ZipArchiveMode.Read, leaveOpen: false);

            foreach (var entry in archive.Entries)
            {
                string relativePath = entry.FullName; // lgtm [cs/zipslip] Archive from trusted source

                // `entry` might be just a pointer to a folder. We skip those.
                if (relativePath.EndsWith("/") && entry.Name == "")
                {
                    yield return (relativePath, null, 0);
                }
                else
                {
                    var contentStream = entry.Open();
                    yield return (relativePath, contentStream, entry.Length);
                    contentStream.Close();
                }
            }
        }

        /// <summary>
        /// Repack raw zip container.
        /// </summary>
        private void RepackRawZip(TaskLoggingHelper log)
        {
            using (var zipStream = File.Open(FileSignInfo.FullPath, FileMode.Open))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string relativeName = entry.FullName;
                    var signedPart = FindNestedPart(relativeName);
                    if (!signedPart.HasValue)
                    {
                        log.LogMessage(MessageImportance.Low, $"Didn't find signed part for nested file: {FileSignInfo.FullPath} -> {relativeName}");
                        continue;
                    }

                    using (var signedStream = File.OpenRead(signedPart.Value.FileSignInfo.FullPath))
                    using (var entryStream = entry.Open())
                    {
                        log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {FileSignInfo.FullPath} -> {relativeName}.");

                        signedStream.CopyTo(entryStream);
                        entryStream.SetLength(signedStream.Length);
                    }
                }
            }
        }

        private void RepackWixPack(TaskLoggingHelper log, string tempDir, string wixToolsPath)
        {
            // The wixpacks can have rather long paths when fully extracted.
            // To avoid issues, use the first element of the GUID (up to first -).
            // This does leave the very remote possibility of the dir already existing. In this case, the
            // create.cmd file will always end up being extracted twice, and ExtractToDirectory
            // will fail. Because of the very very remote possibility of this happening, no
            // attempt to workaround this possibility is made.
            var workingDirGuidSegment = Guid.NewGuid().ToString().Split('-')[0];
            var outputDirGuidSegment = Guid.NewGuid().ToString().Split('-')[0];

            string workingDir = Path.Combine(tempDir, "extract", workingDirGuidSegment);
            string outputDir = Path.Combine(tempDir, "output", outputDirGuidSegment);
            string createFileName = Path.Combine(workingDir, "create.cmd");
            string outputFileName = Path.Combine(outputDir, FileSignInfo.FileName);

            try
            {
                Directory.CreateDirectory(outputDir);
                ZipFile.ExtractToDirectory(FileSignInfo.WixContentFilePath, workingDir);

                var fileList = Directory.GetFiles(workingDir, "*", SearchOption.AllDirectories);
                foreach (var file in fileList)
                {
                    var relativeName = file.Substring($"{workingDir}\\".Length).Replace('\\', '/');
                    var signedPart = FindNestedPart(relativeName);
                    if (!signedPart.HasValue)
                    {
                        log.LogMessage(MessageImportance.Low, $"Didn't find signed part for nested file: {FileSignInfo.FullPath} -> {relativeName}");
                        continue;
                    }
                    log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {file}.");
                    File.Copy(signedPart.Value.FileSignInfo.FullPath, file, true);
                }

                if (!BatchSignUtil.RunWixTool(createFileName, outputDir, workingDir, wixToolsPath, log))
                {
                    log.LogError($"Packaging of wix file '{FileSignInfo.FullPath}' failed");
                    return;
                }

                if (!File.Exists(outputFileName))
                {
                    log.LogError($"Wix tool execution passed, but output file '{outputFileName}' was not found.");
                    return;
                }

                log.LogMessage($"Created wix file {outputFileName}, replacing '{FileSignInfo.FullPath}' with '{outputFileName}'");
                File.Copy(outputFileName, FileSignInfo.FullPath, true);
            }
            finally
            {
                // Delete the intermediates
                Directory.Delete(workingDir, true);
                Directory.Delete(outputDir, true);
            }
        }

#if NETFRAMEWORK
        private static bool RunTarProcess(string srcPath, string dstPath, string tarToolPath)
        {
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $@"exec ""{tarToolPath}"" ""{srcPath}"" ""{dstPath}""",
                UseShellExecute = false
            });

            process.WaitForExit();
            return process.ExitCode == 0;
        }

        private static IEnumerable<(string relativePath, Stream content, long contentSize)> ReadTarGZipEntries(string archivePath, string tempDir, string tarToolPath, bool ignoreContent)
        {
            var extractDir = Path.Combine(tempDir, Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(extractDir);

                if (!RunTarProcess(archivePath, extractDir, tarToolPath))
                {
                    yield break;
                }

                foreach (var path in Directory.EnumerateFiles(extractDir, "*.*", SearchOption.AllDirectories))
                {
                    var relativePath = path.Substring(extractDir.Length + 1).Replace(Path.DirectorySeparatorChar, '/');
                    using var stream = ignoreContent  ? null : (Stream)File.Open(path, FileMode.Open);
                    yield return (relativePath, stream, stream?.Length ?? 0);
                }
            }
            finally
            {
                Directory.Delete(extractDir, recursive: true);
            }
        }

        private void RepackTarGZip(TaskLoggingHelper log, string tempDir, string tarToolPath)
        {
            var extractDir = Path.Combine(tempDir, Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(extractDir);

                if (!RunTarProcess(srcPath: FileSignInfo.FullPath, dstPath: extractDir, tarToolPath))
                {
                    log.LogMessage(MessageImportance.Low, $"Failed to unpack tar archive: dotnet {tarToolPath} {FileSignInfo.FullPath}");
                    return;
                }

                foreach (var path in Directory.EnumerateFiles(extractDir, "*.*", SearchOption.AllDirectories))
                {
                    var relativePath = path.Substring(extractDir.Length + 1).Replace(Path.DirectorySeparatorChar, '/');

                    var signedPart = FindNestedPart(relativePath);
                    if (!signedPart.HasValue)
                    {
                        log.LogMessage(MessageImportance.Low, $"Didn't find signed part for nested file: {FileSignInfo.FullPath} -> {relativePath}");
                        continue;
                    }

                    log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {FileSignInfo.FullPath} -> {relativePath}.");
                    File.Copy(signedPart.Value.FileSignInfo.FullPath, path, overwrite: true);
                }

                if (!RunTarProcess(srcPath: extractDir, dstPath: FileSignInfo.FullPath, tarToolPath))
                {
                    log.LogMessage(MessageImportance.Low, $"Failed to pack tar archive: dotnet {tarToolPath} {FileSignInfo.FullPath}");
                    return;
                }
            }
            finally
            {
                Directory.Delete(extractDir, recursive: true);
            }
        }
#else
        private void RepackTarGZip(TaskLoggingHelper log, string tempDir, string tarToolPath)
        {
            using MemoryStream streamToCompress = new();
            using (TarWriter writer = new(streamToCompress, leaveOpen: true))
            {
                foreach (TarEntry entry in ReadTarGZipEntries(FileSignInfo.FullPath))
                {
                    if (entry.DataStream != null)
                    {
                        string relativeName = entry.Name;
                        ZipPart? signedPart = FindNestedPart(relativeName);

                        if (signedPart.HasValue)
                        {
                            using FileStream signedStream = File.OpenRead(signedPart.Value.FileSignInfo.FullPath);
                            entry.DataStream = signedStream;
                            entry.DataStream.Position = 0;
                            writer.WriteEntry(entry);

                            log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {FileSignInfo.FullPath} -> {relativeName}.");
                            continue;
                        }

                        log.LogMessage(MessageImportance.Low, $"Didn't find signed part for nested file: {FileSignInfo.FullPath} -> {relativeName}");
                    }

                    writer.WriteEntry(entry);
                }
            }

            streamToCompress.Position = 0;
            using (FileStream outputStream = File.Open(FileSignInfo.FullPath, FileMode.Truncate, FileAccess.Write))
            {
                using GZipStream compressor = new(outputStream, CompressionMode.Compress);
                streamToCompress.CopyTo(compressor);
            }
        }

        private static IEnumerable<TarEntry> ReadTarGZipEntries(string path)
        {
            using FileStream streamToDecompress = File.OpenRead(path);
            using GZipStream decompressor = new(streamToDecompress, CompressionMode.Decompress);
            using TarReader tarReader = new(decompressor);
            while (tarReader.GetNextEntry() is TarEntry entry)
            {
                yield return entry;
            }
        }

        /// <summary>
        /// Repack Deb container.
        /// </summary>
        private void RepackDebContainer(TaskLoggingHelper log, string tempDir)
        {
            // Data archive is the only expected nested part
            string dataArchive = NestedParts.Values.Single().FileSignInfo.FullPath;

            string controlArchive;
            try
            {
                controlArchive = GetUpdatedControlArchive(FileSignInfo.FullPath, dataArchive, tempDir);
            }
            catch(Exception e)
            {
                log.LogError(e.Message);
                return;
            }

            CreateDebPackage createDebPackageTask = new()
            {
                OutputDebPackagePath = FileSignInfo.FullPath,
                ControlFile = new TaskItem(controlArchive),
                DataFile = new TaskItem(dataArchive)
            };

            if (!createDebPackageTask.Execute())
            {
                log.LogError($"Failed to create new DEB package: {FileSignInfo.FileName}");
            }
        }

        /// <summary>
        /// Updates checksums of updated data file contents and updates the
        /// control file with the new checksums and new install size.
        /// </summary>
        /// <param name="debianPackage"></param>
        /// <param name="dataArchive"></param>
        /// <param name="tempDir"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetUpdatedControlArchive(string debianPackage, string dataArchive, string tempDir)
        {
            var workingDirGuidSegment = Guid.NewGuid().ToString().Split('-')[0];

            string workingDir = Path.Combine(tempDir, "work", workingDirGuidSegment);
            string controlLayout = Path.Combine(workingDir, "control");
            string dataLayout = Path.Combine(workingDir, "data");

            Directory.CreateDirectory(controlLayout);
            Directory.CreateDirectory(dataLayout);

            // Get the original control archive - to reuse package metadata and scripts
            var (relativePath, content, contentSize) = ReadDebContainerEntries(debianPackage, "control.tar").Single();
            string controlArchive = Path.Combine(workingDir, relativePath);
            File.WriteAllBytes(controlArchive, ((MemoryStream)content).ToArray());

            ExtractTarballContents(dataArchive, dataLayout);
            ExtractTarballContents(controlArchive, controlLayout);

            string sumsFile = Path.Combine(workingDir, "md5sums");
            CreateMD5SumsFile createMD5SumsFileTask = new()
            {
                OutputFile = sumsFile,
                RootDirectory = dataLayout,
                Files = Directory.GetFiles(dataLayout, "*", SearchOption.AllDirectories)
                            .Select(f => new TaskItem(f)).ToArray()
            };

            if (!createMD5SumsFileTask.Execute())
            {
                throw new Exception($"Failed to create MD5 checksums file for: {FileSignInfo.FileName}");
            }

            File.Copy(sumsFile, Path.Combine(controlLayout, "md5sums"), overwrite: true);

            // Update the optional Installed-Size field in control file
            string controlFile = Path.Combine(controlLayout, "control");
            string fileContents = File.ReadAllText(controlFile);
            string stringToFind = "Installed-Size: ";
            int index = fileContents.IndexOf(stringToFind);
            if (index != -1)
            {
                int end = fileContents.IndexOf('\n', index);
                fileContents = fileContents.Replace(fileContents.Substring(index, end - index), stringToFind + createMD5SumsFileTask.InstalledSize);
                File.WriteAllText(controlFile, fileContents);
            }

            // Repack the control tarball
            using (var dstStream = File.Open(controlArchive, FileMode.Create))
            {
                using var gzip = new GZipStream(dstStream, CompressionMode.Compress);
                TarFile.CreateFromDirectory(controlLayout, gzip, includeBaseDirectory: false);
            }

            return controlArchive;
        }

        internal static void ExtractTarballContents(string file, string destination, bool skipSymlinks = true)
        {
            foreach (TarEntry tar in ReadTarGZipEntries(file))
            {
                if (tar.EntryType == TarEntryType.Directory ||
                    (skipSymlinks && tar.EntryType == TarEntryType.SymbolicLink))
                {
                    continue;
                }

                string outputPath = Path.Join(destination, tar.Name);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

                using FileStream outputFileStream = File.Create(outputPath);
                tar.DataStream?.CopyTo(outputFileStream);
            }
        }

        internal static IEnumerable<(string relativePath, Stream content, long contentSize)> ReadDebContainerEntries(string archivePath, string match = null)
        {
            using var archive = new ArReader(File.OpenRead(archivePath), leaveOpen: false);

            while (archive.GetNextEntry() is ArEntry entry)
            {
                string relativePath = entry.Name; // lgtm [cs/zipslip] Archive from trusted source

                if (match == null || relativePath.StartsWith(match))
                {
                    yield return (relativePath, entry.DataStream, entry.DataStream.Length);
                }
            }
        }
#endif
    }
}
