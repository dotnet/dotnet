// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.SignCheck;
using Microsoft.SignCheck.Logging;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace SignCheckTask
{
    public class SignCheckTask : BuildTask
    {
        public bool EnableJarSignatureVerification { get; set; }

        public bool EnableXmlSignatureVerification { get; set; }

        public string FileStatus { get; set; }

        public string ExclusionsOutput { get; set; }

        public bool SkipTimestamp { get; set; }

        public bool Recursive { get; set; }

        public bool VerifyStrongName { get; set; }

        public string ExclusionsFile { get; set; }

        public ITaskItem[] InputFiles { get; set; }

        [Required]
        public string LogFile { get; set; }

        [Required]
        public string ErrorLogFile { get; set; }

        public string ResultsXmlFile { get; set; }

        public string Verbosity { get; set; }

        public string ArtifactFolder { get; set; }

        public override bool Execute()
        {
            bool succeeded = ExecuteImpl();
            return succeeded && !Log.HasLoggedErrors;
        }

        private bool ExecuteImpl()
        {
            Options options = new Options
            {
                EnableJarSignatureVerification = EnableJarSignatureVerification,
                EnableXmlSignatureVerification = EnableXmlSignatureVerification,
                ExclusionsFile = ExclusionsFile,
                ExclusionsOutput = ExclusionsOutput,
                FileStatus = FileStatus?.Split(';', ',') ?? Array.Empty<string>(),
                Recursive = Recursive,
                TraverseSubFolders = Recursive,
                SkipTimestamp = SkipTimestamp,
                VerifyStrongName = VerifyStrongName,
                LogFile = LogFile,
                ErrorLogFile = ErrorLogFile,
                ResultsXmlFile = ResultsXmlFile,
                // The task consumes the log and results files directly, so suppress the per-file
                // console output that would otherwise flood the build log with every processed file.
                ConsoleOutput = false,
            };

            List<string> inputFiles = new List<string>();
            if (InputFiles != null)
            {
                ArtifactFolder = ArtifactFolder ?? Environment.CurrentDirectory;
                SearchOption fileSearchOptions = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                foreach (var checkFile in InputFiles.Select(s => s.ItemSpec).ToArray())
                {
                    if (Path.IsPathRooted(checkFile))
                    {
                        inputFiles.Add(checkFile);
                    }
                    else
                    {
                        var matchedFiles = Directory.GetFiles(ArtifactFolder, checkFile, fileSearchOptions);

                        if (matchedFiles.Length == 1)
                        {
                            inputFiles.Add(matchedFiles[0]);
                        }
                        else if (matchedFiles.Length == 0)
                        {
                            Log.LogError($"Unable to find file '{checkFile}' in folder '{ArtifactFolder}'.  Try specifying 'Recursive=true' to include subfolders");
                        }
                        else if (matchedFiles.Length > 1)
                        {
                            Log.LogError($"found multiple files matching pattern '{checkFile}'");
                            foreach (var file in matchedFiles)
                            {
                                Log.LogError($" - {file}");
                            }
                        }
                    }
                }
            }
            options.InputFiles = inputFiles.ToArray();

            if (Enum.TryParse<LogVerbosity>(Verbosity, out LogVerbosity verbosity))
            {
                options.Verbosity = verbosity;
            }

            var sc = new SignCheckRunner(options);
            int result = sc.Run();

            // SignCheck signals signing issues (and internal failures) through a non-zero exit
            // code without logging an MSBuild error. Surface a real error here so the build fails
            // meaningfully instead of the generic MSB4181. The detailed per-file report is produced
            // by the calling project from the results XML.
            if (result != 0)
            {
                Log.LogError("SignCheck reported signing issues. See the SignCheck logs for details.");
            }

            return result == 0;
        }
    }
}
