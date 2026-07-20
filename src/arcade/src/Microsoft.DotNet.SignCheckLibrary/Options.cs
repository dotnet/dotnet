// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck
{
    public class Options
    {
        public string ErrorLogFile { get; set; }

        public IEnumerable<string> FileStatus { get; set; }

        public string ExclusionsOutput { get; set; }

        public IEnumerable<string> InputFiles { get; set; }

        public bool EnableJarSignatureVerification { get; set; }

        public string LogFile { get; set; }

        public string ResultsXmlFile { get; set; }

        public bool EnableXmlSignatureVerification { get; set; }

        public bool SkipTimestamp { get; set; }

        public bool Recursive { get; set; }

        public bool VerifyStrongName { get; set; }

        public bool TraverseSubFolders { get; set; }

        public LogVerbosity Verbosity { get; set; } = LogVerbosity.Normal;

        public string ExclusionsFile { get; set; }

        /// <summary>
        /// When <see langword="true"/> (the default), SignCheck writes its per-file results to the console.
        /// Hosts that only consume the log and results files (e.g. the MSBuild task) can set this to
        /// <see langword="false"/> to avoid flooding the build output with every processed file.
        /// </summary>
        public bool ConsoleOutput { get; set; } = true;
    }
}
