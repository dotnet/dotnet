// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// The <see cref="SignTool"/> implementation used for test / validation runs.  Does not actually 
    /// change the sign state of the binaries.
    /// </summary>
    internal sealed class ValidationOnlySignTool : SignTool
    {
        internal bool TestSign { get; }

        internal ValidationOnlySignTool(SignToolArgs args, TaskLoggingHelper log)
            : base(args, log)
        {
            TestSign = args.TestSign;
        }

        public override bool LocalStrongNameSign(IBuildEngine buildEngine, int round, IEnumerable<FileSignInfo> files)
        {
            foreach (var file in files)
            {
                if (file.SignInfo.ShouldLocallyStrongNameSign)
                {
                    if (!LocalStrongNameSign(file))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override void RemoveStrongNameSign(string assemblyPath)
        {
        }

        public override SigningStatus VerifySignedDeb(TaskLoggingHelper log, string filePath)
            => SigningStatus.Signed;

        public override SigningStatus VerifySignedRpm(TaskLoggingHelper log, string filePath)
            => SigningStatus.Signed;

        public override SigningStatus VerifySignedPEFile(Stream assemblyStream)
            => SigningStatus.Signed;

        public override SigningStatus VerifyStrongNameSign(string fileFullPath)
            => SigningStatus.Signed;

        public override bool RunMSBuild(IBuildEngine buildEngine, string projectFilePath, string binLogPath, string logPath, string errorLogPath)
        {
            if (TestSign)
            {
                return buildEngine.BuildProjectFile(projectFilePath, null, null, null);
            }
            else
            {
                return true;
            }
        }

        public override SigningStatus VerifySignedPowerShellFile(string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedNuGet(string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedVSIX(string filePath) => SigningStatus.Signed;

        public override SigningStatus VerifySignedPkgOrAppBundle(TaskLoggingHelper log, string filePath, string pkgToolPath) => SigningStatus.Signed;
    }
}
