// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;

namespace Microsoft.DotNet.SignTool
{
    internal sealed class FakeSignTool : SignTool
    {
        private static readonly byte[] _stamp = Guid.NewGuid().ToByteArray();

        internal FakeSignTool(SignToolArgs args, TaskLoggingHelper log)
            : base(args, log)
        {
        }

        public override void RemoveStrongNameSign(string assemblyPath)
        {
        }

        public override bool LocalStrongNameSign(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> files) => true;

        public override SigningStatus VerifySignedPEFile(Stream stream)
        {
            var buffer = new byte[_stamp.Length];
            return stream.TryReadAll(buffer, 0, buffer.Length) == buffer.Length && 
                   ByteSequenceComparer.Equals(buffer, _stamp) ? SigningStatus.Signed : SigningStatus.NotSigned;
        }

        public override SigningStatus VerifyStrongNameSign(string fileFullPath) => SigningStatus.Signed;

        public override bool RunMSBuild(IBuildEngine buildEngine, string projectFilePath, string binLogPath, string logPath, string errorLogPath)
            => buildEngine.BuildProjectFile(projectFilePath, null, null, null);

        internal static void SignFile(string path)
        {
            // Cannot fake sign the engine as that invalidates the exe and makes it non-executable
            // which will cause insignia to be unable to reattach it to the installer exe.
            if (FileSignInfo.IsPEFile(path) &&
                !path.EndsWith(SignToolConstants.MsiEngineExtension))
            {
                SignPEFile(path);
            }
        }

        private static void SignPEFile(string path)
        {
            using (var stream = File.OpenWrite(Uri.UnescapeDataString(path)))
            {
                stream.Write(_stamp, 0, _stamp.Length);
            }
        }

        public override SigningStatus VerifySignedDeb(TaskLoggingHelper log, string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedRpm(TaskLoggingHelper log, string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedPowerShellFile(string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedNuGet(string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedVSIX(string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedPkgOrAppBundle(TaskLoggingHelper log, string filePath, string pkgToolPath) => SigningStatus.Signed;
    }
}
