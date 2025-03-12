// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Collections.Generic;
using Microsoft.DotNet.StrongName;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// The signing implementation which actually signs binaries.
    /// </summary>
    internal sealed class RealSignTool : SignTool
    {
        private readonly string _dotnetPath;
        private readonly string _logDir;
        private readonly string _msbuildVerbosity;
        private readonly string _snPath;
        private readonly int _dotnetTimeout;

        /// <summary>
        /// The number of bytes from the start of the <see cref="CorHeader"/> to its <see cref="CorFlags"/>.
        /// </summary>
        internal const int OffsetFromStartOfCorHeaderToFlags =
               sizeof(Int32)  // byte count
             + sizeof(Int16)  // major version
             + sizeof(Int16)  // minor version
             + sizeof(Int64); // metadata directory

        internal bool TestSign { get; }

        internal RealSignTool(SignToolArgs args, TaskLoggingHelper log) : base(args, log)
        {
            TestSign = args.TestSign;
            _dotnetPath = args.DotNetPath;
            _msbuildVerbosity = args.MSBuildVerbosity;
            _snPath = args.SNBinaryPath;
            _logDir = args.LogDir;
            _dotnetTimeout = args.DotNetTimeout;
        }

        public override bool RunMSBuild(IBuildEngine buildEngine, string projectFilePath, string binLogPath, string logPath, string errorLogPath)
        {
            if (_dotnetPath == null)
            {
                return buildEngine.BuildProjectFile(projectFilePath, null, null, null);
            }

            Directory.CreateDirectory(_logDir);

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = _dotnetPath,
                    Arguments = $@"build ""{projectFilePath}"" -v:""{_msbuildVerbosity}"" -bl:""{binLogPath}""",
                    UseShellExecute = false,
                    WorkingDirectory = TempDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                var output = new StringBuilder();
                var error = new StringBuilder();

                process.OutputDataReceived += (sender, e) => output.AppendLine(e.Data);
                process.ErrorDataReceived += (sender, e) => error.AppendLine(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                bool success = true;
                if (!process.WaitForExit(_dotnetTimeout))
                {
                    _log.LogError($"MSBuild process did not exit within '{_dotnetTimeout}' ms.");
                    process.Kill();
                    process.WaitForExit();
                    success = false;
                }

                if (process.ExitCode != 0)
                {
                    _log.LogError($"Failed to execute MSBuild on the project file '{projectFilePath}'" +
                    $" with exit code '{process.ExitCode}'.");
                    success = false;
                }

                string outputStr = output.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(outputStr))
                {
                    File.WriteAllText(logPath, outputStr);
                }
                
                string errorStr = error.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(errorStr))
                {
                    File.WriteAllText(errorLogPath, errorStr);
                }

                return success;
            }
        }

        public override void RemoveStrongNameSign(string assemblyPath)
        {
            StrongNameHelper.ClearStrongNameSignedBit(assemblyPath);
        }

        public override SigningStatus VerifySignedPEFile(Stream assemblyStream)
        {
            // The assembly won't verify by design when doing test signing, but pretend it is.
            if (TestSign)
            {
                return SigningStatus.Signed;
            }

            return VerifySignatures.IsSignedPE(assemblyStream);
        }
        public override SigningStatus VerifyStrongNameSign(string fileFullPath)
        {
            // The assembly won't verify by design when doing test signing.
            if (TestSign)
            {
                return SigningStatus.Signed;
            }

            return StrongNameHelper.IsSigned(fileFullPath, snPath:_snPath) ? SigningStatus.Signed : SigningStatus.NotSigned;
        }

        public override SigningStatus VerifySignedDeb(TaskLoggingHelper log, string filePath)
        {
            return VerifySignatures.IsSignedDeb(log, filePath);
        }

        public override SigningStatus VerifySignedRpm(TaskLoggingHelper log, string filePath)
        {
            return VerifySignatures.IsSignedRpm(log, filePath);
        }

        public override SigningStatus VerifySignedPowerShellFile(string filePath)
        {
            return VerifySignatures.IsSignedPowershellFile(filePath);
        }

        public override SigningStatus VerifySignedNuGet(string filePath)
        {
            return VerifySignatures.IsSignedNupkg(filePath);
        }

        public override SigningStatus VerifySignedVSIX(string filePath)
        {
            // Open the VSIX and check for the digital signature file.
            return VerifySignatures.IsSignedVSIXByFileMarker(filePath);
        }

        public override SigningStatus VerifySignedPkgOrAppBundle(TaskLoggingHelper log, string fullPath, string pkgToolPath)
        {
            return VerifySignatures.IsSignedPkgOrAppBundle(log, fullPath, pkgToolPath);
        }

        public override bool LocalStrongNameSign(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> files)
        {
            var filesToLocallyStrongNameSign = files.Where(f => f.SignInfo.ShouldLocallyStrongNameSign);

            if (filesToLocallyStrongNameSign.Any())
            {
                _log.LogMessage($"Locally strong naming {filesToLocallyStrongNameSign.Count()} files.");

                foreach (var file in filesToLocallyStrongNameSign)
                {
                    if (!LocalStrongNameSign(file))
                    {
                        _log.LogMessage(MessageImportance.High, $"Failed to locally strong name sign '{file.FileName}'");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
