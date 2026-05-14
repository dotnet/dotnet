// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CommandLine;
using Microsoft.SignCheck.Logging;
using System.Collections.Generic;

namespace SignCheckTask
{
    public class Options
    {
        [Option('e', "error-log-file",
            HelpText = "Log errors to a separate file. If the file already exists it will be overwritten.")]
        public string ErrorLogFile
        {
            get;
            set;
        }

        [Option('f', "file-status",
            Separator = ',',
            HelpText = "Report the status of a specific set of files. Any combination of the following values are allowed. Values are separated by a ','. 'UnsignedFiles', 'SignedFiles', 'SkippedFiles', 'ExcludedFiles', 'AllFiles'. Default is 'UnsignedFiles'")]
        public IEnumerable<string> FileStatus
        {
            get;
            set;
        }

        [Option('g', "generate-exclusions-file",
            HelpText = "Name of the exclusions file to generate. The entries in the file are generated using reported unsigned files. If the file already exists it will be overwritten.")]
        public string ExclusionsOutput
        {
            get;
            set;
        }

        [Option('i', "input-files",
            HelpText = "A list of files to scan. Wildcards (* and ?) are supported. You can specify groups of files, e.g. C:\\Dir1\\Dir*\\File?.EXE or a URL (http or https).")]
        public IEnumerable<string> InputFiles
        {
            get;
            set;
        }

        [Option('j', "verify-jar",
            HelpText = "Enable JAR signature verification. By default, .jar files are no verified.")]
        public bool EnableJarSignatureVerification
        {
            get;
            set;
        }

        [Option('l', "log-file",
            HelpText = "Output results to the specified log file. If the file already exists it will be overwritten.")]
        public string LogFile
        {
            get;
            set;
        }

        [Option("results-xml-file",
            HelpText = "Output signing results to the specified XML log file. If the file already exists it will be overwritten.")]
        public string ResultsXmlFile
        {
            get;
            set;
        }

        [Option('m', "verify-xml",
            HelpText = "Enable XML signature verification. By default, .xml files are not verified.")]
        public bool EnableXmlSignatureVerification
        {
            get;
            set;
        }

        [Option('p', "skip-timestamp",
            HelpText = "Ignore timestamp checks for AuthentiCode signatures.")]
        public bool SkipTimestamp
        {
            get;
            set;
        }

        [Option('r', "recursive",
            HelpText = "Traverse subdirectories or container files such as .zip, .nupkg, .cab, and .msi")]
        public bool Recursive
        {
            get;
            set;
        }

        [Option('s', "verify-strongname",
            HelpText = "Enable strongname checks for managed code files (.exe and .dll)")]
        public bool VerifyStrongName
        {
            get;
            set;
        }

        [Option('t', "traverse-subfolders",
            HelpText = "Traverse subfolders to find files matching wildcard patterns used by the --input-files option.")]
        public bool TraverseSubFolders
        {
            get;
            set;
        }

        [Option('v', "verbosity",
            HelpText = "Set the verbosity level: Minimum, Normal, Detailed, Diagnostic.")]
        public LogVerbosity Verbosity
        {
            get;
            set;
        } = LogVerbosity.Normal;

        [Option('x', "exclusions-file",
            HelpText = "Path to a file containing a list of files to ignore when verification fails. Exclusions are not reported as errors.")]
        public string ExclusionsFile
        {
            get;
            set;
        }
    }
}
