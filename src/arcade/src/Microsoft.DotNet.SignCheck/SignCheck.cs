// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.CommandLine;
using System.Linq;
using Microsoft.SignCheck;
using Microsoft.SignCheck.Logging;

namespace SignCheck
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            Option<string> errorLogFileOption = new("--error-log-file", "-e")
            {
                Description = "Log errors to a separate file. If the file already exists it will be overwritten."
            };

            Option<string[]> fileStatusOption = new("--file-status", "-f")
            {
                Description = "Report the status of a specific set of files. Any combination of the following values are allowed. Values are separated by a ','. 'UnsignedFiles', 'SignedFiles', 'SkippedFiles', 'ExcludedFiles', 'AllFiles'. Default is 'UnsignedFiles'",
                AllowMultipleArgumentsPerToken = true,
                CustomParser = result => result.Tokens.SelectMany(t => t.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToArray()
            };

            Option<string> exclusionsOutputOption = new("--generate-exclusions-file", "-g")
            {
                Description = "Name of the exclusions file to generate. The entries in the file are generated using reported unsigned files. If the file already exists it will be overwritten."
            };

            Option<string[]> inputFilesOption = new("--input-files", "-i")
            {
                Description = "A list of files to scan. Wildcards (* and ?) are supported. You can specify groups of files, e.g. C:\\Dir1\\Dir*\\File?.EXE or a URL (http or https).",
                AllowMultipleArgumentsPerToken = true
            };

            Option<bool> verifyJarOption = new("--verify-jar", "-j")
            {
                Description = "Enable JAR signature verification. By default, .jar files are not verified."
            };

            Option<string> logFileOption = new("--log-file", "-l")
            {
                Description = "Output results to the specified log file. If the file already exists it will be overwritten."
            };

            Option<string> resultsXmlFileOption = new("--results-xml-file")
            {
                Description = "Output signing results to the specified XML log file. If the file already exists it will be overwritten."
            };

            Option<bool> verifyXmlOption = new("--verify-xml", "-m")
            {
                Description = "Enable XML signature verification. By default, .xml files are not verified."
            };

            Option<bool> skipTimestampOption = new("--skip-timestamp", "-p")
            {
                Description = "Ignore timestamp checks for AuthentiCode signatures."
            };

            Option<bool> recursiveOption = new("--recursive", "-r")
            {
                Description = "Traverse subdirectories or container files such as .zip, .nupkg, .cab, and .msi"
            };

            Option<bool> verifyStrongNameOption = new("--verify-strongname", "-s")
            {
                Description = "Enable strongname checks for managed code files (.exe and .dll)"
            };

            Option<bool> traverseSubFoldersOption = new("--traverse-subfolders", "-t")
            {
                Description = "Traverse subfolders to find files matching wildcard patterns used by the --input-files option."
            };

            Option<LogVerbosity> verbosityOption = new("--verbosity", "-v")
            {
                Description = "Set the verbosity level: Minimum, Normal, Detailed, Diagnostic.",
                DefaultValueFactory = _ => LogVerbosity.Normal
            };

            Option<string> exclusionsFileOption = new("--exclusions-file", "-x")
            {
                Description = "Path to a file containing a list of files to ignore when verification fails. Exclusions are not reported as errors."
            };

            RootCommand rootCommand = new("Build artifact signing validation tool")
            {
                TreatUnmatchedTokensAsErrors = true
            };

            rootCommand.Options.Add(errorLogFileOption);
            rootCommand.Options.Add(fileStatusOption);
            rootCommand.Options.Add(exclusionsOutputOption);
            rootCommand.Options.Add(inputFilesOption);
            rootCommand.Options.Add(verifyJarOption);
            rootCommand.Options.Add(logFileOption);
            rootCommand.Options.Add(resultsXmlFileOption);
            rootCommand.Options.Add(verifyXmlOption);
            rootCommand.Options.Add(skipTimestampOption);
            rootCommand.Options.Add(recursiveOption);
            rootCommand.Options.Add(verifyStrongNameOption);
            rootCommand.Options.Add(traverseSubFoldersOption);
            rootCommand.Options.Add(verbosityOption);
            rootCommand.Options.Add(exclusionsFileOption);

            rootCommand.SetAction(parseResult =>
            {
                Options options = new()
                {
                    ErrorLogFile = parseResult.GetValue(errorLogFileOption),
                    FileStatus = parseResult.GetValue(fileStatusOption) ?? Array.Empty<string>(),
                    ExclusionsOutput = parseResult.GetValue(exclusionsOutputOption),
                    InputFiles = parseResult.GetValue(inputFilesOption) ?? Array.Empty<string>(),
                    EnableJarSignatureVerification = parseResult.GetValue(verifyJarOption),
                    LogFile = parseResult.GetValue(logFileOption),
                    ResultsXmlFile = parseResult.GetValue(resultsXmlFileOption),
                    EnableXmlSignatureVerification = parseResult.GetValue(verifyXmlOption),
                    SkipTimestamp = parseResult.GetValue(skipTimestampOption),
                    Recursive = parseResult.GetValue(recursiveOption),
                    VerifyStrongName = parseResult.GetValue(verifyStrongNameOption),
                    TraverseSubFolders = parseResult.GetValue(traverseSubFoldersOption),
                    Verbosity = parseResult.GetValue(verbosityOption),
                    ExclusionsFile = parseResult.GetValue(exclusionsFileOption),
                };

                return new SignCheckRunner(options).Run();
            });

            return rootCommand.Parse(args).Invoke();
        }
    }
}
