// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// The signing implementation which actually signs binaries.
    /// </summary>
    internal sealed class RealSignTool : SignTool
    {
        private readonly string _msbuildPath;
        private readonly string _dotnetPath;
        private readonly string _logDir;
        private readonly string _snPath;

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
            _msbuildPath = args.MSBuildPath;
            _dotnetPath = args.DotNetPath;
            _snPath = args.SNBinaryPath;
            _logDir = args.LogDir;
        }

        public override bool RunMSBuild(IBuildEngine buildEngine, string projectFilePath, string binLogPath)
        {
            if (_msbuildPath == null && _dotnetPath == null)
            {
                return buildEngine.BuildProjectFile(projectFilePath, null, null, null);
            }

            Directory.CreateDirectory(_logDir);

            string processFileName = _dotnetPath;
            string processArguments = $@"build ""{projectFilePath}"" -bl:""{binLogPath}""";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                processFileName = _msbuildPath;
                processArguments = $@"""{projectFilePath}"" /bl:""{binLogPath}""";
            }

            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = processFileName,
                Arguments = processArguments,
                UseShellExecute = false,
                WorkingDirectory = TempDir,
            });

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                _log.LogError($"Failed to execute MSBuild on the project file {projectFilePath}");
                return false;
            }

            return true;
        }

        public override void RemoveStrongNameSign(string assemblyPath)
        {
            StrongName.ClearStrongNameSignedBit(assemblyPath);
        }

        public override bool VerifySignedPEFile(Stream assemblyStream)
        {
            // The assembly won't verify by design when doing test signing.
            if (TestSign)
            {
                return true;
            }

            return ContentUtil.IsAuthenticodeSigned(assemblyStream);
        }
        public override bool VerifyStrongNameSign(string fileFullPath)
        {
            // The assembly won't verify by design when doing test signing.
            if (TestSign)
            {
                return true;
            }

            return StrongName.IsSigned(fileFullPath, snPath:_snPath, log: _log);
        }

        public override bool VerifySignedDeb(TaskLoggingHelper log, string filePath)
        {
            return VerifySignatures.VerifySignedDeb(log, filePath);
        }

        public override bool VerifySignedPowerShellFile(string filePath)
        {
            return VerifySignatures.VerifySignedPowerShellFile(filePath);
        }

        public override bool VerifySignedNugetFileMarker(string filePath)
        {
            return VerifySignatures.VerifySignedNupkgByFileMarker(filePath);
        }

        public override bool VerifySignedVSIXFileMarker(string filePath)
        {
            return VerifySignatures.VerifySignedVSIXByFileMarker(filePath);
        }
        
        public override bool LocalStrongNameSign(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> files)
        {
            var filesToLocallyStrongNameSign = files.Where(f => f.SignInfo.ShouldLocallyStrongNameSign);

            _log.LogMessage($"Locally strong naming {filesToLocallyStrongNameSign.Count()} files.");

            foreach (var file in filesToLocallyStrongNameSign)
            {
                if (!LocalStrongNameSign(file))
                {
                    _log.LogMessage(MessageImportance.High, $"Failed to locally strong name sign '{file.FileName}'");
                    return false;
                }
            }

            return true;
        }
    }
}
