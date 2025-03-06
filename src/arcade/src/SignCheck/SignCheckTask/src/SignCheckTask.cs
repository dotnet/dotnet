// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
#if NET472
using AppDomainIsolatedTask = Microsoft.Build.Utilities.AppDomainIsolatedTask;
#else
using BuildTask = Microsoft.Build.Utilities.Task;
#endif
using Microsoft.SignCheck.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SignCheckTask
{
#if NETFRAMEWORK
    [LoadInSeparateAppDomain]
    [RunInSTA]
    public class SignCheckTask : AppDomainIsolatedTask
    {
        static SignCheckTask() => Microsoft.DotNet.AssemblyResolution.Initialize();
#else
    public class SignCheckTask : BuildTask
    {
#endif
        public bool EnableJarSignatureVerification
        {
            get;
            set;
        }
        public bool EnableXmlSignatureVerification
        {
            get;
            set;
        }
        public string FileStatus
        {
            get;
            set;
        }

        public string ExclusionsOutput
        {
            get;
            set;
        }

        public bool SkipTimestamp
        {
            get;
            set;
        }
        public bool Recursive
        {
            get;
            set;
        }
        
        public bool VerifyStrongName
        {
            get;
            set;
        }
        public string ExclusionsFile
        {
            get;
            set;
        }
        public ITaskItem[] InputFiles
        {
            get;
            set;
        }
        [Required]
        public string LogFile
        {
            get;
            set;
        }
        [Required]
        public string ErrorLogFile
        {
            get;
            set;
        }
        public string ResultsXmlFile
        {
            get;
            set;
        }
        public string Verbosity
        {
            get;
            set;
        }
        public string ArtifactFolder
        {
            get;
            set;
        }

        public override bool Execute()
        {
#if NETFRAMEWORK
            Microsoft.DotNet.AssemblyResolution.Log = Log;
#endif
            try
            {
                bool succeeded = ExecuteImpl();
                return succeeded && !Log.HasLoggedErrors;
            }
            finally
            {
#if NETFRAMEWORK
            Microsoft.DotNet.AssemblyResolution.Log = null;
#endif
            }
        }

        private bool ExecuteImpl()
        {
            Options options = new Options();
            options.EnableJarSignatureVerification = EnableJarSignatureVerification;
            options.EnableXmlSignatureVerification = EnableXmlSignatureVerification;
            options.ExclusionsFile = ExclusionsFile;
            options.ExclusionsOutput = ExclusionsOutput;
            string[] filestatuses = FileStatus.Split(';', ',');
            options.FileStatus = filestatuses;
            options.Recursive = Recursive;
            options.TraverseSubFolders = Recursive;
            options.SkipTimestamp = SkipTimestamp;
            options.VerifyStrongName = VerifyStrongName;
            options.LogFile = LogFile;
            options.ErrorLogFile = ErrorLogFile;
            options.ResultsXmlFile = ResultsXmlFile;

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

                        if(matchedFiles.Length == 1)
                        {
                            inputFiles.Add(matchedFiles[0]);
                        }
                        else if(matchedFiles.Length == 0)
                        {
                            Log.LogError($"Unable to find file '{checkFile}' in folder '{ArtifactFolder}'.  Try specifying 'Recursive=true` to include subfolders");
                        }
                        else if (matchedFiles.Length > 1)
                        {
                            Log.LogError($"found multiple files matching pattern '{checkFile}'");
                            foreach(var file in matchedFiles)
                            {
                                Log.LogError($" - {file}");
                            }
                        }
                    }
                }
            }
            options.InputFiles = inputFiles.ToArray();

            if(Enum.TryParse<LogVerbosity>(Verbosity, out LogVerbosity verbosity))
            {
                options.Verbosity = verbosity;
            }
            
            var sc = new SignCheck(options);
            int result = sc.Run();
            return result == 0;
        }
    }
}
