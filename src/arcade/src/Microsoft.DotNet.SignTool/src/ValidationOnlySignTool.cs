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
            // On non-Windows, we skip strong name signing because sn.exe is not available.
            // We could skip it always in the validation sign tool, but it is useful to
            // get some level of validation.

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }

            foreach (var file in files)
            {
                if (file.SignInfo.ShouldLocallyStrongNameSign)
                {
                    return LocalStrongNameSign(file);
                }
            }

            return true;
        }

        public override void RemovePublicSign(string assemblyPath)
        {
        }

        public override bool VerifySignedPEFile(Stream assemblyStream)
            => true;

        public override bool VerifyStrongNameSign(string fileFullPath)
            => true;

        public override bool RunMSBuild(IBuildEngine buildEngine, string projectFilePath, string binLogPath)
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

        public override bool VerifySignedPowerShellFile(string filePath)
        {
            return true;
        }

        public override bool VerifySignedNugetFileMarker(string filePath)
        {
            return true;
        }

        public override bool VerifySignedVSIXFileMarker(string filePath)
        {
            return true;
        }
    }
}
