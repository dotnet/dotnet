// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TaskLoggingHelper = Microsoft.Build.Utilities.TaskLoggingHelper;

namespace Microsoft.DotNet.SignTool
{
    internal sealed class BatchSignUtil
    {
        private readonly TaskLoggingHelper _log;
        private readonly IBuildEngine _buildEngine;
        private readonly BatchSignInput _batchData;
        private readonly SignTool _signTool;
        private readonly string[] _itemsToSkipStrongNameCheck;
        private readonly Dictionary<SignedFileContentKey, string> _hashToCollisionIdMap;
        private Telemetry _telemetry;
        private readonly int _repackParallelism;
        private readonly long _maximumParallelFileSizeInBytes;

        internal BatchSignUtil(IBuildEngine buildEngine,
            TaskLoggingHelper log,
            SignTool signTool,
            BatchSignInput batchData,
            string[] itemsToSkipStrongNameCheck,
            Dictionary<SignedFileContentKey, string> hashToCollisionIdMap,
            int repackParallelism = 0,
            long maximumParallelFileSizeInBytes = 0,
            Telemetry telemetry = null)
        {
            _signTool = signTool;
            _batchData = batchData;
            _log = log;
            _buildEngine = buildEngine;
            _itemsToSkipStrongNameCheck = itemsToSkipStrongNameCheck ?? Array.Empty<string>();
            _telemetry = telemetry;
            _hashToCollisionIdMap = hashToCollisionIdMap;
            _repackParallelism = repackParallelism != 0 ? repackParallelism : Environment.ProcessorCount;
            _maximumParallelFileSizeInBytes = maximumParallelFileSizeInBytes != 0 ?
                maximumParallelFileSizeInBytes : 2048 / _repackParallelism * 1024 * 1024;
        }

        internal void Go(bool doStrongNameCheck)
        {
            VerifyCertificates(_log);

            if (_log.HasLoggedErrors)
            {
                return;
            }

            // Remove strong name signing, as sn.exe would choke on already signed binaries.
            // Our new signing infra is more resilient, but sn.exe may be used as a backup
            // or when not signing locally.
            RemoveStrongNameSigning();

            // Next sign all of the files
            if (!SignFiles())
            {
                _log.LogError("Error during execution of signing process.");
                return;
            }

            if (!CopyFiles())
            {
                return;
            }

            // Check that all files have a strong name signature
            if (doStrongNameCheck)
            {
                VerifyStrongNameSigning();
            }

            // Validate the signing worked and produced actual signed binaries in all locations.
            // This is a recursive process since we process nested containers.
            foreach (var file in _batchData.FilesToSign)
            {
                VerifyAfterSign(_log, file);
            }

            if (_log.HasLoggedErrors)
            {
                return;
            }

            _log.LogMessage(MessageImportance.High, "Build artifacts signed and validated.");
        }

        private void RemoveStrongNameSigning()
        {
            foreach (var fileSignInfo in _batchData.FilesToSign.Where(x => x.IsPEFile()))
            {
                if (fileSignInfo.SignInfo.ShouldStrongName)
                {
                    _log.LogMessage($"Removing strong name signing from: '{fileSignInfo.FileName}'");
                    _signTool.RemoveStrongNameSign(fileSignInfo.FullPath);
                }
            }
        }

        /// <summary>
        /// Actually sign all of the described files.
        /// </summary>
        private bool SignFiles()
        {
            // Generate the list of signed files in a deterministic order. Makes it easier to track down
            // bugs if repeated runs use the same ordering.
            var toProcessList = _batchData.FilesToSign.ToList();
            var toRepackSet = _batchData.FilesToSign.Where(x => x.ShouldRepack)?.Select(x => x.FullPath)?.ToHashSet();
            var round = 0;
            var trackedSet = new HashSet<SignedFileContentKey>();

            // Given a list of files that need signing, sign them in a batch.
            bool signGroup(IEnumerable<FileSignInfo> files, out int signedCount)
            {
                var filesToSign = files.Where(fileInfo => fileInfo.SignInfo.ShouldSign).ToArray();
                var filesToNotarize = files.Where(fileInfo => fileInfo.SignInfo.ShouldNotarize).ToArray();
                signedCount = filesToSign.Length;
                if (filesToSign.Length == 0) return true;

                _log.LogMessage(MessageImportance.High, $"Round {round}: Signing {filesToSign.Length} files" +
                    $"{(filesToNotarize.Length > 0? $", Notarizing {filesToNotarize.Length} files" : "")}");

                foreach (var file in filesToSign)
                {
                    string collisionIdInfo = string.Empty;
                    if(_hashToCollisionIdMap != null)
                    {
                        if(_hashToCollisionIdMap.TryGetValue(file.FileContentKey, out string collisionPriorityId) &&
                            !string.IsNullOrEmpty(collisionPriorityId))
                        {
                            collisionIdInfo = $"Collision Id='{collisionPriorityId}'";
                        }
                        
                    }
                    _log.LogMessage(MessageImportance.Low, $"{file} {collisionIdInfo}");
                }

                return _signTool.Sign(_buildEngine, round, filesToSign);
            }

            // Given a list of files that need signing, sign the installer engines
            // of those that are wix containers.
            bool signEngines(IEnumerable<FileSignInfo> files, out int signedCount)
            {
                var enginesToSign = files.Where(fileInfo => fileInfo.SignInfo.ShouldSign && 
                                                fileInfo.IsUnpackableWixContainer() &&
                                                Path.GetExtension(fileInfo.FullPath) == ".exe").ToArray();
                signedCount = enginesToSign.Length;
                if (enginesToSign.Length == 0)
                {
                    return true;
                }

                _log.LogMessage(MessageImportance.High, $"Round {round}: Signing {enginesToSign.Length} engines.");

                Dictionary<SignedFileContentKey, FileSignInfo> engines = new Dictionary<SignedFileContentKey, FileSignInfo>();
                var workingDirectory = Path.Combine(_signTool.TempDir, "engines");
                int engineContainer = 0;
                // extract engines
                foreach (var file in enginesToSign)
                {
                    string engineFileName = $"{Path.Combine(workingDirectory, $"{engineContainer}", file.FileName)}{SignToolConstants.MsiEngineExtension}";
                    _log.LogMessage(MessageImportance.Normal, $"Extracting engine from {file.FullPath}");
                    if (!RunWixTool("insignia.exe", $"-ib {file.FullPath} -o {engineFileName}",
                        workingDirectory, _signTool.WixToolsPath, _log))
                    {
                        _log.LogError($"Failed to extract engine from {file.FullPath}");
                        return false;
                    }

                    var fileUniqueKey = new SignedFileContentKey(file.ContentHash, engineFileName);

                    engines.Add(fileUniqueKey, file);
                    engineContainer++;
                }

                // sign engines
                bool signResult = _signTool.Sign(_buildEngine, round, engines.Select(engine =>
                    new FileSignInfo(new PathWithHash(engine.Key.FileName, engine.Value.ContentHash), engine.Value.SignInfo)));
                if(!signResult)
                {
                    _log.LogError($"Failed to sign engines");
                    return signResult;
                }

                // attach engines
                foreach (var engine in engines)
                {
                    _log.LogMessage(MessageImportance.Normal, $"Attaching engine {engine.Key.FileName} to {engine.Value.FullPath}");

                    try
                    {
                        if (!RunWixTool("insignia.exe",
                            $"-ab {engine.Key.FileName} {engine.Value.FullPath} -o {engine.Value.FullPath}", workingDirectory,
                            _signTool.WixToolsPath, _log))
                        {
                            _log.LogError($"Failed to attach engine to {engine.Value.FullPath}");
                            return false;
                        }
                    }
                    finally
                    {
                        // cleanup engines (they fail signing verification if they stay in the drop
                        File.Delete(engine.Key.FileName);
                    }
                }
                return true;
            }

            // Given a group of file that are ready for processing,
            // repack those files that are containers.
            void repackGroup(IEnumerable<FileSignInfo> files, out int repackCount)
            {
                var repackList = files.Where(w => toRepackSet.Contains(w.FullPath)).ToList();

                repackCount = repackList.Count();

                if (repackCount == 0)
                {
                    return;
                }
                _log.LogMessage(MessageImportance.High, $"Repacking {repackCount} containers.");

                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = _repackParallelism;

                // It's possible that there are large containers within this set that, if
                // repacked in parallel, could cause OOMs. To avoid this, we set a limit on the size of containers
                // that we will repack in parallel based on the parallelism degree and a 2GB limit.
                // Repack these in serial later.
                var largeRepackList = new List<FileSignInfo>();
                var smallRepackList = new List<FileSignInfo>();

                foreach (var file in repackList)
                {
                    FileInfo fileInfo = new FileInfo(file.FullPath);
                    if (fileInfo.Length > _maximumParallelFileSizeInBytes)
                    {
                        largeRepackList.Add(file);
                    }
                    else
                    {
                        smallRepackList.Add(file);
                    }
                }

                _log.LogMessage(MessageImportance.High, $"Repacking {smallRepackList.Count} containers in parallel.");

                Parallel.ForEach(smallRepackList, parallelOptions, file =>
                {
                    repackContainer(file);
                    toRepackSet.Remove(file.FullPath);
                });

                if (largeRepackList.Count == 0)
                {
                    return;
                }

                _log.LogMessage(MessageImportance.High, $"Repacking {largeRepackList.Count} large containers in serial.");

                foreach (var file in largeRepackList)
                {
                    repackContainer(file);
                    toRepackSet.Remove(file.FullPath);
                }
            }

            void repackContainer(FileSignInfo file)
            {
                if (file.IsUnpackableContainer())
                {
                    _log.LogMessage($"Repacking container: '{file.FileName}'");
                    _batchData.ZipDataMap[file.FileContentKey].Repack(_log, _signTool.TempDir, _signTool.WixToolsPath, _signTool.TarToolPath, _signTool.PkgToolPath);
                }
                else
                {
                    _log.LogError($"Don't know how to repack file '{file.FullPath}'");
                }
            }

            // Is this file ready to be signed or repackaged? That is are all of the items that it depends on already
            // signed, don't need signing, and are repacked.
            bool isReady(FileSignInfo file)
            {
                if (file.IsUnpackableContainer())
                {
                    var zipData = _batchData.ZipDataMap[file.FileContentKey];
                    return zipData.NestedParts.Values.All(x => (!x.FileSignInfo.SignInfo.ShouldSign ||
                        trackedSet.Contains(x.FileSignInfo.FileContentKey)) && !toRepackSet.Contains(x.FileSignInfo.FullPath)
                        );
                }
                return true;
            }

            // Identify the next set of files that should be signed or repacked.
            // This is the set of files for which all of the dependencies have been signed,
            // are already signed, are repacked, etc.
            List<FileSignInfo> identifyNextGroup()
            {
                var list = new List<FileSignInfo>();
                var i = 0;
                while (i < toProcessList.Count)
                {
                    var current = toProcessList[i];
                    if (isReady(current))
                    {
                        list.Add(current);
                        toProcessList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                return list;
            }

            // Telemetry data
            double telemetryTotalFilesSigned = 0;
            double telemetryTotalFilesRepacked = 0;
            Stopwatch telemetrySignedTime = new Stopwatch();
            Stopwatch telemetryRepackedTime = new Stopwatch();

            try
            {
                // Core algorithm of batch signing.
                // While there are files left to process,
                //  Identify which files are ready for processing (ready to repack or sign)
                //  Repack those of that set that are containers
                //  Sign any of those files that need signing, along with their engines.
                while (toProcessList.Count > 0)
                {
                    var trackList = identifyNextGroup();
                    if (trackList.Count == 0)
                    {
                        throw new InvalidOperationException("No progress made on signing which indicates a bug");
                    }

                    int fileModifiedCount;
                    telemetryRepackedTime.Start();
                    repackGroup(trackList, out fileModifiedCount);
                    telemetryRepackedTime.Stop();
                    telemetryTotalFilesRepacked += fileModifiedCount;

                    try
                    {
                        telemetrySignedTime.Start();
                        if (!signEngines(trackList, out fileModifiedCount))
                        {
                            return false;
                        }
                        if (fileModifiedCount > 0)
                        {
                            round++;
                            telemetryTotalFilesSigned += fileModifiedCount;
                        }

                        if (!signGroup(trackList, out fileModifiedCount))
                        {
                            return false;
                        }
                        if (fileModifiedCount > 0)
                        {
                            round++;
                            telemetryTotalFilesSigned += fileModifiedCount;
                        }
                    }
                    finally
                    {
                        telemetrySignedTime.Stop();
                    }

                    trackList.ForEach(x => trackedSet.Add(x.FileContentKey));
                }
            }
            finally
            {
                if (_telemetry != null)
                {
                    _telemetry.AddMetric("Signed file count", telemetryTotalFilesSigned);
                    _telemetry.AddMetric("Repacked file count", telemetryTotalFilesRepacked);
                    _telemetry.AddMetric("Signing duration (s)", telemetrySignedTime.ElapsedMilliseconds / 1000);
                    _telemetry.AddMetric("Repacking duration (s)", telemetryRepackedTime.ElapsedMilliseconds / 1000);
                }
            }

            return true;
        }

        internal static bool RunWixTool(string toolName, string arguments, string workingDirectory, string wixToolsPath, TaskLoggingHelper log)
        {
            if (wixToolsPath == null)
            {
                log.LogError("WixToolsPath must be defined to run WiX tooling. Wixpacks are used to produce signed msi's during post-build signing. If this repository is using in-build signing, remove '*.wixpack.zip' from ItemsToSign.");
                return false;
            }

            if (!Directory.Exists(wixToolsPath))
            {
                log.LogError($"WixToolsPath '{wixToolsPath}' not found.");
                return false;
            }

            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                Arguments = $"/c {toolName} {arguments}",
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            string path = processStartInfo.EnvironmentVariables["PATH"];
            path = $"{wixToolsPath};{path}";
            processStartInfo.EnvironmentVariables.Remove("PATH");
            processStartInfo.EnvironmentVariables.Add("PATH", path);

            var process = Process.Start(processStartInfo);
            process.WaitForExit();
            return process.ExitCode == 0;
        }

        private bool CopyFiles()
        {
            bool success = true;
            foreach (var entry in _batchData.FilesToCopy)
            {
                var src = entry.Key;
                var dst = entry.Value;

                try
                {
                    _log.LogMessage($"Updating '{dst}' with signed content");
                    File.Copy(src, dst, overwrite: true);
                }
                catch (Exception e)
                {
                    _log.LogError($"Updating '{dst}' with signed content failed: '{e.Message}'");
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Sanity check the certificates that are attached to the various items. Ensure we aren't using, say, a VSIX
        /// certificate on a DLL for example.
        /// </summary>
        private void VerifyCertificates(TaskLoggingHelper log)
        {
            foreach (var fileName in _batchData.FilesToSign.OrderBy(x => x.FullPath))
            {
                bool isVsixCert = (!string.IsNullOrEmpty(fileName.SignInfo.Certificate) && IsVsixCertificate(fileName.SignInfo.Certificate)) ||
                                    fileName.SignInfo.IsAlreadySigned && fileName.HasSignableParts;

                bool isInvalidEmptyCertificate = fileName.SignInfo.Certificate == null && !fileName.HasSignableParts && !fileName.SignInfo.IsAlreadySigned;

                if (fileName.IsPEFile())
                {
                    if (isVsixCert)
                    {
                        log.LogError($"Assembly {fileName} cannot be signed with a VSIX certificate");
                    }
                }
                else if (fileName.IsVsix())
                {
                    if (!isVsixCert)
                    {
                        log.LogError($"VSIX {fileName} must be signed with a VSIX certificate");
                    }

                    if (fileName.SignInfo.StrongName != null)
                    {
                        log.LogError($"VSIX {fileName} cannot be strong name signed.");
                    }
                }
                else if (fileName.IsDeb())
                {
                    if (isInvalidEmptyCertificate)
                    {
                        log.LogError($"Deb package {fileName} should have a certificate name.");
                    }
                    if (!IsLinuxSignCertificate(fileName.SignInfo.Certificate))
                    {
                        log.LogError($"Deb package {fileName} must be signed with a LinuxSign certificate.");
                    }
                }
                else if (fileName.IsRpm())
                {
                    if (isInvalidEmptyCertificate)
                    {
                        log.LogError($"Rpm package {fileName} should have a certificate name.");
                    }
                    if (!IsLinuxSignCertificate(fileName.SignInfo.Certificate))
                    {
                        log.LogError($"Rpm package {fileName} must be signed with a LinuxSign certificate.");
                    }
                }
                else if (fileName.IsNupkg())
                {
                    if(isInvalidEmptyCertificate)
                    {
                        log.LogError($"Nupkg {fileName} should have a certificate name.");
                    }

                    if (fileName.SignInfo.StrongName != null)
                    {
                        log.LogError($"Nupkg {fileName} cannot be strong name signed.");
                    }
                }
                else if (fileName.IsPkg())
                {
                    if(isInvalidEmptyCertificate)
                    {
                        log.LogError($"Pkg {fileName} should have a certificate name.");
                    }

                    if (fileName.SignInfo.StrongName != null)
                    {
                        log.LogError($"Pkg {fileName} cannot be strong name signed.");
                    }
                }
                else if (fileName.IsAppBundle())
                {
                    if (isInvalidEmptyCertificate)
                    {
                        log.LogError($"AppBundle {fileName} should have a certificate name.");
                    }

                    if (fileName.SignInfo.StrongName != null)
                    {
                        log.LogError($"AppBundle {fileName} cannot be strong name signed.");
                    }
                }
                else if (fileName.IsZip())
                {
                    if (fileName.SignInfo.Certificate != null)
                    {
                        log.LogError($"Zip {fileName} should not be signed with this certificate: {fileName.SignInfo.Certificate}");
                    }

                    if (fileName.SignInfo.StrongName != null)
                    {
                        log.LogError($"Zip {fileName} cannot be strong name signed.");
                    }
                }
                if (fileName.IsExecutableWixContainer())
                {
                    if (isInvalidEmptyCertificate)
                    {
                        log.LogError($"Wix file {fileName} should have a certificate name.");
                    }

                    if (fileName.SignInfo.StrongName != null)
                    {
                        log.LogError($"Wix file {fileName} cannot be strong name signed.");
                    }
                }
            }
        }

        /// <summary>
        /// Recursively verify that files are signed properly.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="file"></param>
        private void VerifyAfterSign(TaskLoggingHelper log, FileSignInfo file)
        {
            // No need to check if the file should not have been signed.
            if (file.SignInfo.ShouldSign)
            {
                if (file.IsPEFile())
                {
                    using (var stream = File.OpenRead(file.FullPath))
                    {
                        var status = _signTool.VerifySignedPEFile(stream);
                        LogSigningStatus(file, status, "PE file");
                    }
                }
                else if (file.IsDeb())
                {
                    var status = _signTool.VerifySignedDeb(log, file.FullPath);
                    LogSigningStatus(file, status, "Debian package");
                }
                else if (file.IsRpm())
                {
                    var status = _signTool.VerifySignedRpm(log, file.FullPath);
                    LogSigningStatus(file, status, "RPM package");
                }
                else if (file.IsPowerShellScript())
                {
                    var status = _signTool.VerifySignedPowerShellFile(file.FullPath);
                    LogSigningStatus(file, status, "Powershell file");
                }
                else if (file.IsPkg() || file.IsAppBundle())
                {
                    var status = _signTool.VerifySignedPkgOrAppBundle(_log, file.FullPath, _signTool.PkgToolPath);
                    LogSigningStatus(file, status, "Pkg or app");
                }
                else if (file.IsNupkg())
                {
                    var status = _signTool.VerifySignedNuGet(file.FullPath);
                    LogSigningStatus(file, status, "Nuget package");
                } 
                else if (file.IsVsix())
                {
                    var status = _signTool.VerifySignedVSIX(file.FullPath);
                    LogSigningStatus(file, status, "VSIX package");
                }
            }

            if (file.IsUnpackableContainer())
            {
                var zipData = _batchData.ZipDataMap[file.FileContentKey];

                foreach (var nestedPart in zipData.NestedParts.Values)
                {
                    VerifyAfterSign(log, nestedPart.FileSignInfo);
                }
            }

            void LogSigningStatus(FileSignInfo file, SigningStatus status, string fileType)
            {
                if (status == SigningStatus.NotSigned)
                {
                    _log.LogError($"{fileType} {file.FullPath} is not signed properly.");
                }
                else if (status == SigningStatus.Unknown)
                {
                    _log.LogMessage(MessageImportance.Low, $"Signing status of {file.FullPath} could not be determined.");
                }
                else
                {
                    _log.LogMessage(MessageImportance.Low, $"{fileType} {file.FullPath} is signed properly");
                }
            }
        }

        private void VerifyStrongNameSigning()
        {
            foreach (var file in _batchData.FilesToSign)
            {
                if (_itemsToSkipStrongNameCheck.Contains(file.FileName))
                {
                    _log.LogMessage($"Skipping strong-name validation for {file.FullPath}.");
                    continue;
                }

                if (file.IsManaged() && !file.IsCrossgened())
                {
                    if (_signTool.VerifyStrongNameSign(file.FullPath) != SigningStatus.Signed)
                    {
                        _log.LogError($"Assembly {file.FullPath} is not strong-name signed correctly.");
                    }
                    else
                    {
                        _log.LogMessage(MessageImportance.Low, $"Assembly {file.FullPath} strong-name signature is valid.");
                    }
                }
            }
        }

        private static bool IsVsixCertificate(string certificate) => certificate.StartsWith("Vsix", StringComparison.OrdinalIgnoreCase);

        private static bool IsLinuxSignCertificate(string certificate) => certificate.StartsWith("LinuxSign", StringComparison.OrdinalIgnoreCase);
    }
}
