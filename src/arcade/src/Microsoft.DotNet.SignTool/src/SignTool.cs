// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Packaging;
using Microsoft.DotNet.StrongName;

namespace Microsoft.DotNet.SignTool
{
    internal abstract class SignTool
    {
        private readonly SignToolArgs _args;
        internal readonly TaskLoggingHelper _log;
        internal string TempDir => _args.TempDir;
        internal string MicroBuildCorePath => _args.MicroBuildCorePath;

        internal string WixToolsPath => _args.WixToolsPath;
        internal string TarToolPath => _args.TarToolPath;
        internal string PkgToolPath => _args.PkgToolPath;

        internal SignTool(SignToolArgs args, TaskLoggingHelper log)
        {
            _args = args;
            _log = log;
        }

        public abstract void RemoveStrongNameSign(string assemblyPath);

        public abstract bool LocalStrongNameSign(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> files);

        public abstract SigningStatus VerifySignedDeb(TaskLoggingHelper log, string filePath);
        public abstract SigningStatus VerifySignedRpm(TaskLoggingHelper log, string filePath);
        public abstract SigningStatus VerifySignedPEFile(Stream stream);
        public abstract SigningStatus VerifySignedPowerShellFile(string filePath);
        public abstract SigningStatus VerifySignedNuGet(string filePath);
        public abstract SigningStatus VerifySignedVSIX(string filePath);
        public abstract SigningStatus VerifySignedPkgOrAppBundle(TaskLoggingHelper log, string filePath, string pkgToolPath);

        public abstract SigningStatus VerifyStrongNameSign(string fileFullPath);

        public abstract bool RunMSBuild(IBuildEngine buildEngine, string projectFilePath, string binLogPath, string logPath, string errorLogPath);

        public bool Sign(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> files)
        {
            return LocalStrongNameSign(buildEngine, round, files)
                && AuthenticodeSignAndNotarize(buildEngine, round, files);
        }

        /// <summary>
        /// Zip up the mac files. Note that the Microbuild task can automatically zip files, but only does so on Mac,
        /// so may as well make this generic.
        /// </summary>
        /// <param name="filesToSign">Files to sign</param>
        /// <returns>Dictionary of any files in filesToSign that were zipped</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private Dictionary<string, string> ZipMacFiles(IEnumerable<FileSignInfo> filesToSign)
        {
            var zipPaths = new Dictionary<string, string>();
            var osxFilesToZip = filesToSign.Where(fsi => SignToolConstants.MacSigningOperationsRequiringZipping.Contains(fsi.SignInfo.Certificate));

            foreach (var file in osxFilesToZip)
            {
                string zipFilePath = GetZipFilePath(file.FullPath);
                zipPaths.Add(file.FullPath, zipFilePath);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var process = Process.Start(new ProcessStartInfo()
                    {
                        FileName = "ditto",
                        Arguments = $"-V -ck --sequesterRsrc \"{file.FullPath}\" \"{zipFilePath}\"",
                        UseShellExecute = false,
                        WorkingDirectory = TempDir,
                    });

                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        _log.LogError($"Failed to zip file {file.FullPath} to {zipFilePath}");
                        throw new InvalidOperationException($"Failed to zip file {file.FullPath} to {zipFilePath}");
                    }
                }
                else
                {
                    using (var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(file.FullPath, Path.GetFileName(file.FullPath));
                    }
                }
            }

            return zipPaths;
        }

        private void UnzipMacFiles(Dictionary<string, string> zippedOSXFiles)
        {
            foreach (var item in zippedOSXFiles)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var process = Process.Start(new ProcessStartInfo()
                    {
                        FileName = "ditto",
                        Arguments = $"-V -xk \"{item.Value}\" \"{Path.GetDirectoryName(item.Key)}\"",
                        UseShellExecute = false,
                        WorkingDirectory = TempDir,
                    });

                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        _log.LogError($"Failed to unzip file {item.Value} to {item.Key}");
                        throw new InvalidOperationException($"Failed to unzip file {item.Value} to {item.Key}");
                    }
                }
                else
                {
                    // Delete the file first so that we can overwrite it. ExtractToDirectory's overwrite is not
                    // available on framework.
#if NETFRAMEWORK
                    File.Delete(item.Key);
                    ZipFile.ExtractToDirectory(item.Value, Path.GetDirectoryName(item.Key));
#else
                    ZipFile.ExtractToDirectory(item.Value, Path.GetDirectoryName(item.Key), true);
#endif
                }

                File.Delete(item.Value);
            }
        }

        private bool AuthenticodeSignAndNotarize(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> filesToSign)
        {
            var dir = Path.Combine(_args.TempDir, "Signing");
            bool status = true;

            Directory.CreateDirectory(dir);
            
            var zippedPaths = ZipMacFiles(filesToSign);

            // First the signing pass
            var signProjectPath = Path.Combine(dir, $"Round{round}-Sign.proj");
            File.WriteAllText(signProjectPath, GenerateBuildFileContent(filesToSign, zippedPaths, false));
            string signingLogName = $"SigningRound{round}";
            status = RunMSBuild(buildEngine, signProjectPath, Path.Combine(_args.LogDir, $"{signingLogName}.binlog"), Path.Combine(_args.LogDir, $"{signingLogName}.log"), Path.Combine(_args.LogDir, $"{signingLogName}.error.log"));

            if (!status)
            {
                return false;
            }

            // Now unzip. Notarization does not expect zipped packages.
            UnzipMacFiles(zippedPaths);

            // Then an additional notarization pass.
            var filesToNotarize = filesToSign.Where(f => !string.IsNullOrEmpty(f.SignInfo.NotarizationAppName));
            if (filesToNotarize.Any())
            {
                var notarizeProjectPath = Path.Combine(dir, $"Round{round}-Notarize.proj");
                File.WriteAllText(notarizeProjectPath, GenerateBuildFileContent(filesToNotarize, null, true));
                string notarizeLogName = $"NotarizationRound{round}";
                status = RunMSBuild(buildEngine, notarizeProjectPath, Path.Combine(_args.LogDir, $"{notarizeLogName}.binlog"), Path.Combine(_args.LogDir, $"{notarizeLogName}.log"), Path.Combine(_args.LogDir, $"{notarizeLogName}.error.log"));
            }

            return status;
        }

        private string GenerateBuildFileContent(IEnumerable<FileSignInfo> filesToSign, Dictionary<string, string> zippedPaths, bool notarize)
        {
            var builder = new StringBuilder();
            AppendLine(builder, depth: 0, text: @"<?xml version=""1.0"" encoding=""utf-8""?>");
            AppendLine(builder, depth: 0, text: @"<Project DefaultTargets=""AfterBuild"">");

            // Setup the code to get the NuGet package root.
            var signKind = _args.TestSign ? "test" : "real";
            AppendLine(builder, depth: 1, text: @"<PropertyGroup>");
            AppendLine(builder, depth: 2, text: $@"<OutDir>{_args.EnclosingDir}</OutDir>");
            AppendLine(builder, depth: 2, text: $@"<IntermediateOutputPath>{_args.TempDir}</IntermediateOutputPath>");
            AppendLine(builder, depth: 2, text: $@"<SignType>{signKind}</SignType>");
            AppendLine(builder, depth: 1, text: @"</PropertyGroup>");

            AppendLine(builder, depth: 1, text: $@"<Import Project=""{Path.Combine(MicroBuildCorePath, "build", "MicroBuild.Core.props")}"" />");
            AppendLine(builder, depth: 1, text: $@"<ItemGroup>");

            foreach (var fileToSign in filesToSign)
            {
                if (zippedPaths == null || !zippedPaths.TryGetValue(fileToSign.FullPath, out string filePath))
                {
                    filePath = fileToSign.FullPath;
                }
                AppendLine(builder, depth: 2, text: $@"<FilesToSign Include=""{Uri.EscapeDataString(filePath)}"">");
                AppendLine(builder, depth: 3, text: $@"<Authenticode>{(notarize ? SignToolConstants.MacNotarizationOperation : fileToSign.SignInfo.Certificate)}</Authenticode>");
                if (notarize)
                {
                    AppendLine(builder, depth: 3, text: $@"<MacAppName>{fileToSign.SignInfo.NotarizationAppName}</MacAppName>");
                }
                if (fileToSign.SignInfo.ShouldStrongName && !fileToSign.SignInfo.ShouldLocallyStrongNameSign)
                {
                    AppendLine(builder, depth: 3, text: $@"<StrongName>{fileToSign.SignInfo.StrongName}</StrongName>");
                }
                AppendLine(builder, depth: 2, text: @"</FilesToSign>");
            }

            AppendLine(builder, depth: 1, text: $@"</ItemGroup>");

            // The MicroBuild targets hook AfterBuild to do the signing hence we just make it our no-op default target
            AppendLine(builder, depth: 1, text: @"<Target Name=""AfterBuild"">");
            AppendLine(builder, depth: 2, text: @"<Message Text=""Running signing process."" />");
            AppendLine(builder, depth: 1, text: @"</Target>");

            AppendLine(builder, depth: 1, text: $@"<Import Project=""{Path.Combine(MicroBuildCorePath, "build", "MicroBuild.Core.targets")}"" />");
            AppendLine(builder, depth: 0, text: @"</Project>");

            return builder.ToString();
        }

        protected virtual string GetZipFilePath(string fullPath)
        {
            var zipFilePath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileName(fullPath) + ".zip");
            // If the file already exists, it means that the user asked for another file to be signed with a colliding name.
            // This is very unlikely. Throw in this case.
            if (File.Exists(zipFilePath))
            {
                throw new NotImplementedException($"The zip file path '{zipFilePath}' already exists.");
            }
            return zipFilePath;
        }

        private static void AppendLine(StringBuilder builder, int depth, string text)
        {
            for (int i = 0; i < depth; i++)
            {
                builder.Append("    ");
            }

            builder.AppendLine(text);
        }

        protected bool LocalStrongNameSign(FileSignInfo file)
        {
            _log.LogMessage($"Strong-name signing '{file.FullPath}' locally with key '{file.SignInfo.StrongName}'.");

            return StrongNameHelper.Sign(file.FullPath, file.SignInfo.StrongName, _args.SNBinaryPath);
        }
    }
}
