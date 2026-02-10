// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NuGet.Packaging;
using Microsoft.DotNet.Build.Tasks.Installers;
using System.Formats.Tar;

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

        public static IEnumerable<ZipDataEntry> ReadEntries(string archivePath, string tempDir, string pkgToolPath, bool ignoreContent = false)
        {
            if (FileSignInfo.IsTarGZip(archivePath))
            {
                return ReadTarGZipEntries(archivePath)
                    .Select(static entry => new ZipDataEntry(entry.Name, entry.DataStream, entry.Length)
                    {
                        UnixFileMode = (uint)entry.Mode,
                    });
            }
            else if (FileSignInfo.IsPkg(archivePath) || FileSignInfo.IsAppBundle(archivePath))
            {
                return ReadPkgOrAppBundleEntries(archivePath, tempDir, pkgToolPath, ignoreContent);
            }
            else if (FileSignInfo.IsDeb(archivePath))
            {
                return ReadDebContainerEntries(archivePath, "data.tar");
            }
            else if (FileSignInfo.IsRpm(archivePath))
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    throw new NotImplementedException("RPM signing is only supported on Linux platform");
                }

                return ReadRpmContainerEntries(archivePath);
            }
            else
            {
                return ReadZipEntries(archivePath);
            }
        }

        /// <summary>
        /// Repack the zip container with the signed files.
        /// </summary>
        public void Repack(TaskLoggingHelper log, string tempDir, string wix3ToolsPath, string wixToolsPath, string pkgToolPath)
        {
            if (FileSignInfo.IsVsix())
            {
                RepackPackage(log);
            }
            else if (FileSignInfo.IsTarGZip())
            {
                RepackTarGZip(log, tempDir);
            }
            else if (FileSignInfo.IsUnpackableWixContainer())
            {
                RepackWixPack(log, tempDir, wix3ToolsPath, wixToolsPath);
            }
            else if (FileSignInfo.IsPkg() || FileSignInfo.IsAppBundle())
            {
                RepackPkgOrAppBundles(log, tempDir, pkgToolPath);
            }
            else if (FileSignInfo.IsDeb())
            {
                RepackDebContainer(log, tempDir);
            }
            else if (FileSignInfo.IsRpm())
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    throw new NotImplementedException("RPM signing is only supported on Linux platform");
                }

                RepackRpmContainer(log, tempDir);
            }
            else 
            {
                RepackRawZip(log);
            }
        }

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

        private static IEnumerable<ZipDataEntry> ReadZipEntries(string archivePath)
        {
            using (var archive = new ZipArchive(File.OpenRead(archivePath), ZipArchiveMode.Read, leaveOpen: false))
            {
                foreach (var entry in archive.Entries)
                {
                    yield return new ZipDataEntry(entry);
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

        private void RepackWixPack(TaskLoggingHelper log, string tempDir, string wix3ToolsPath, string wixToolsPath)
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

                string wixPath = File.ReadAllText(createFileName).Contains("light.exe")
                                 ? wix3ToolsPath
                                 : wixToolsPath;

                if (!BatchSignUtil.RunWixTool(createFileName, outputDir, workingDir, wixPath, log))
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

        internal static bool RunPkgProcess(string srcPath, string dstPath, string action, string pkgToolPath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new Exception($"Pkg tooling is only supported on MacOS.");
            }

            string args = $@"{action} ""{srcPath}""";
            
            if (action != "verify")
            {
                args += $@" ""{dstPath}""";
            }

            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $@"exec ""{pkgToolPath}"" {args}",
                UseShellExecute = false,
                RedirectStandardError = true
            });

            process.WaitForExit();
            return process.ExitCode == 0;
        }

        private static IEnumerable<ZipDataEntry> ReadPkgOrAppBundleEntries(string archivePath, string tempDir, string pkgToolPath, bool ignoreContent)
        {
            string extractDir = Path.Combine(tempDir, Guid.NewGuid().ToString());
            try
            {
                if (!RunPkgProcess(archivePath, extractDir, "unpack", pkgToolPath))
                {
                    throw new Exception($"Failed to unpack pkg {archivePath}");
                }

                foreach (var path in Directory.EnumerateFiles(extractDir, "*.*", SearchOption.AllDirectories))
                {
                    var relativePath = path.Substring(extractDir.Length + 1).Replace(Path.DirectorySeparatorChar, '/');
                    using var stream = ignoreContent ? null : (Stream)File.Open(path, FileMode.Open);
                    yield return new ZipDataEntry(relativePath, stream)
                    {
                        UnixFileMode = GetUnixFileMode(path),
                    };
                }
            }
            finally
            {
                if (Directory.Exists(extractDir))
                {
                    Directory.Delete(extractDir, recursive: true);
                }
            }
        }

        private void RepackPkgOrAppBundles(TaskLoggingHelper log, string tempDir, string pkgToolPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                log.LogError("Pkg/AppBundle repackaging is not supported on Windows.");
                return;
            }

            string extractDir = Path.Combine(tempDir, Guid.NewGuid().ToString());
            try
            {
                if (!RunPkgProcess(srcPath: FileSignInfo.FullPath, dstPath: extractDir, "unpack", pkgToolPath))
                {
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

                    // Preserve the original file mode from the PKG/App. The sign cache might bring if from an entry in an archive with different perms.
                    UnixFileMode extractedFileMode = File.GetUnixFileMode(path);

                    log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {FileSignInfo.FullPath} -> {relativePath} (perms: {Convert.ToString((uint)extractedFileMode, 8)}).");
                    File.Copy(signedPart.Value.FileSignInfo.FullPath, path, overwrite: true);
                    File.SetUnixFileMode(path, extractedFileMode);
                }

                if (!RunPkgProcess(srcPath: extractDir, dstPath: FileSignInfo.FullPath, "pack", pkgToolPath))
                {
                    return;
                }
            }
            finally
            {
                if (Directory.Exists(extractDir))
                {
                    Directory.Delete(extractDir, recursive: true);
                }
            }
        }

        private void RepackTarGZip(TaskLoggingHelper log, string tempDir)
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
                            log.LogMessage(MessageImportance.Low, $"Copying signed stream from {signedPart.Value.FileSignInfo.FullPath} to {FileSignInfo.FullPath} -> {relativeName} (perms: {Convert.ToString((uint)entry.Mode, 8)}).");
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
                controlArchive = GetUpdatedControlArchive(log, FileSignInfo.FullPath, dataArchive, tempDir);
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
        private string GetUpdatedControlArchive(TaskLoggingHelper log, string debianPackage, string dataArchive, string tempDir)
        {
            var workingDirGuidSegment = Guid.NewGuid().ToString().Split('-')[0];

            string workingDir = Path.Combine(tempDir, "work", workingDirGuidSegment);
            string controlLayout = Path.Combine(workingDir, "control");
            string dataLayout = Path.Combine(workingDir, "data");

            Directory.CreateDirectory(controlLayout);
            Directory.CreateDirectory(dataLayout);

            // Get the original control archive - to reuse package metadata and scripts
            var controlEntry = ReadDebContainerEntries(debianPackage, "control.tar").Single();
            string controlArchive = Path.Combine(workingDir, controlEntry.RelativePath);
            controlEntry.WriteToFile(controlArchive);

            ExtractTarballContents(log, dataArchive, dataLayout);
            ExtractTarballContents(log, controlArchive, controlLayout);

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

            // Update the control tarball contents. We update the contents of the control entry streams
            // rather than recreating from the unpacked directory layout to ensure that
            // the original entry field metadata and tar format is preserved.
            using MemoryStream streamToCompress = new();
            using (TarWriter writer = new(streamToCompress, leaveOpen: true))
            {
                foreach (TarEntry entry in ReadTarGZipEntries(controlArchive))
                {
                    string relativeName = entry.Name;
                    if (relativeName is "./control" or "./md5sums")
                    {
                        using FileStream fileStream = File.OpenRead(Path.Combine(controlLayout, relativeName));
                        entry.DataStream = fileStream;
                        entry.DataStream.Position = 0;
                        writer.WriteEntry(entry);
                        continue;
                    }

                    writer.WriteEntry(entry);
                }
            }

            streamToCompress.Position = 0;
            using (FileStream outputStream = File.Open(controlArchive, FileMode.Create))
            {
                using GZipStream compressor = new(outputStream, CompressionMode.Compress);
                streamToCompress.CopyTo(compressor);
            }

            return controlArchive;
        }

        internal static void ExtractTarballContents(TaskLoggingHelper log, string file, string destination, bool skipSymlinks = true)
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

                using (FileStream outputFileStream = File.Create(outputPath))
                {
                    tar.DataStream?.CopyTo(outputFileStream);
                }
                SetUnixFileMode(log, (uint)tar.Mode, outputPath);
            }
        }

        internal static IEnumerable<ZipDataEntry> ReadDebContainerEntries(string archivePath, string match = null)
        {
            using var archive = new ArReader(File.OpenRead(archivePath), leaveOpen: false);

            while (archive.GetNextEntry() is ArEntry entry)
            {
                string relativePath = entry.Name; // lgtm [cs/zipslip] Archive from trusted source

                // The relative path occasionally ends with a '/', which is not a valid path given that the path is a file.
                // Remove the following workaround once https://github.com/dotnet/arcade/issues/15384 is resolved.
                if (relativePath.EndsWith("/"))
                {
                    relativePath = relativePath.TrimEnd('/');
                }

                if (match == null || relativePath.StartsWith(match))
                {
                    yield return new ZipDataEntry(relativePath, entry.DataStream)
                    {
                        UnixFileMode = entry.Mode & ArEntry.FilePermissionMask,
                    };
                }
            }
        }

        private static IEnumerable<ZipDataEntry> ReadRpmContainerEntries(string archivePath)
        {
            using var stream = File.Open(archivePath, FileMode.Open);
            using RpmPackage rpmPackage = RpmPackage.Read(stream);
            using var archive = new CpioReader(rpmPackage.ArchiveStream, leaveOpen: false);

            while (archive.GetNextEntry() is CpioEntry entry)
            {
                yield return new ZipDataEntry(entry.Name, entry.DataStream)
                {
                    UnixFileMode = entry.Mode & CpioEntry.FilePermissionMask,
                };
            }
        }

        private void RepackRpmContainer(TaskLoggingHelper log, string tempDir)
        {
            // Unpack original package - create the layout
            string workingDir = Path.Combine(tempDir, Guid.NewGuid().ToString().Split('-')[0]);
            Directory.CreateDirectory(workingDir);
            string layout = Path.Combine(workingDir, "layout");
            Directory.CreateDirectory(layout);
            ExtractRpmPayloadContents(log, FileSignInfo.FullPath, layout);

            // Update signed files in layout
            foreach (var signedPart in NestedParts.Values)
            {
                File.Copy(signedPart.FileSignInfo.FullPath, Path.Combine(layout, signedPart.RelativeName), overwrite: true);
            }

            // Create payload.cpio
            string payload = Path.Combine(workingDir, "payload.cpio");

            RunExternalProcess(log, "bash", $"-c \"find . -depth ! -wholename '.' -print  | cpio -H newc -o --quiet > '{payload}'\"", out string _, layout);

            // Collect file types for all files in layout
            RunExternalProcess(log, "bash", $"-c \"find . -depth ! -wholename '.'  -exec file {{}} \\;\"", out string output, layout);
            ITaskItem[] rawPayloadFileKinds =
                output.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                      .Select(t => new TaskItem(t))
                      .ToArray();

            IReadOnlyList<RpmHeader<RpmHeaderTag>.Entry> headerEntries = GetRpmHeaderEntries(FileSignInfo.FullPath);
            string[] requireNames = (string[])headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.RequireName).Value;
            string[] requireVersions = (string[])headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.RequireVersion).Value;
            string[] changelogLines = (string[])headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.ChangelogText).Value;
            string[] conflictNames = (string[])headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.ConflictName).Value;

            List<ITaskItem> scripts = [];
            foreach (var scriptTag in new[] { RpmHeaderTag.Prein, RpmHeaderTag.Preun, RpmHeaderTag.Postin, RpmHeaderTag.Postun })
            {
                string contents = (string)headerEntries.FirstOrDefault(e => e.Tag == scriptTag).Value;
                if (contents != null)
                {
                    string kind = Enum.GetName(scriptTag);
                    string file = Path.Combine(workingDir, kind);
                    File.WriteAllText(file, contents);
                    scripts.Add(new TaskItem(file, new Dictionary<string, string> { { "Kind", kind } }));
                }
            }

            // Create RPM package
            CreateRpmPackage createRpmPackageTask = new()
            {
                OutputRpmPackagePath = FileSignInfo.FullPath,
                Vendor = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.Vendor).Value.ToString(),
                Packager = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.Packager).Value.ToString(),
                PackageName = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.PackageName).Value.ToString(),
                PackageVersion = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.PackageVersion).Value.ToString(),
                PackageRelease = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.PackageRelease).Value.ToString(),
                PackageOS = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.OperatingSystem).Value.ToString(),
                PackageArchitecture = RpmBuilder.GetDotNetArchitectureFromRpmHeaderArchitecture(headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.Architecture).Value.ToString()),
                Payload = payload,
                RawPayloadFileKinds = rawPayloadFileKinds,
                Requires = requireNames != null ? requireNames.Zip(requireVersions, (name, version) => new TaskItem($"{name}", new Dictionary<string, string> { { "Version", version } })).Where(t => !t.ItemSpec.StartsWith("rpmlib")).ToArray() : [],
                Conflicts = conflictNames != null ? conflictNames.Select(c => new TaskItem(c)).ToArray() : [],
                OwnedDirectories = ((string[])headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.DirectoryNames).Value).Select(d => new TaskItem(d)).ToArray(),
                ChangelogLines = changelogLines != null ? changelogLines.Select(c => new TaskItem(c)).ToArray() : [],
                License = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.License).Value.ToString(),
                Summary = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.Summary).Value.ToString(),
                Description = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.Description).Value.ToString(),
                PackageUrl = headerEntries.FirstOrDefault(e => e.Tag == RpmHeaderTag.Url).Value.ToString(),
                Scripts = scripts.ToArray(),
            };

            if (!createRpmPackageTask.Execute())
            {
                throw new Exception($"Failed to create RPM package: {FileSignInfo.FileName}");
            }
        }

        internal static IReadOnlyList<RpmHeader<RpmHeaderTag>.Entry> GetRpmHeaderEntries(string rpmPackage)
        {
            using var stream = File.Open(rpmPackage, FileMode.Open);
            return RpmPackage.Read(stream).Header.Entries;
        }

        internal static void ExtractRpmPayloadContents(TaskLoggingHelper log, string rpmPackage, string layout)
        {
            foreach (var entry in ReadRpmContainerEntries(rpmPackage))
            {
                string outputPath = Path.Combine(layout, entry.RelativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

                if (entry != null)
                {
                    entry.WriteToFile(outputPath);
                    SetUnixFileMode(log, entry.UnixFileMode, outputPath);
                }
            }
        }

        private static bool RunExternalProcess(TaskLoggingHelper log, string cmd, string args, out string output, string workingDir = null)
        {
            log.LogMessage(MessageImportance.Low, $"Running command: '{cmd}' {args}");

            ProcessStartInfo psi = new()
            {
                FileName = cmd,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDir
            };

            using Process process = Process.Start(psi);
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string stderr = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(stderr))
            {
                log.LogMessage(MessageImportance.Low, $"  Stderr: {stderr}");
            }

            if (process.ExitCode != 0)
            {
                log.LogMessage(MessageImportance.Low, $"  Exit code: {process.ExitCode}");
            }

            return process.ExitCode == 0;
        }

        internal static void SetUnixFileMode(TaskLoggingHelper log, uint? unixFileMode, string outputPath)
        {
            // Set file mode if not the default.
            if (!OperatingSystem.IsWindows() && unixFileMode is { } mode and not /* 0644 */ 420)
            {
                log.LogMessage(MessageImportance.Low, $"Setting file mode {Convert.ToString(mode, 8)} on: {outputPath}");
                File.SetUnixFileMode(outputPath, (UnixFileMode)mode);
            }
        }

        private static uint? GetUnixFileMode(string filePath)
        {
            return OperatingSystem.IsWindows() ? null : (uint)File.GetUnixFileMode(filePath);
        }
    }
}
