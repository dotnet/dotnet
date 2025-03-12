// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Linq;
using CommandLine;
using System.Threading.Tasks;
using Microsoft.SignCheck.Logging;
using Microsoft.SignCheck.Verification;

namespace SignCheckTask
{
    public class SignCheck
    {
        private static readonly char[] _wildcards = new char[] { '*', '?' };

        // Location where files can be downloaded
        private static readonly string _appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SignCheck");

        internal List<string> _inputFiles;

        internal Exclusions Exclusions
        {
            get;
            set;
        }

        internal bool LoggedResults
        {
            get;
            set;
        }

        internal string[] ResultDetails
        {
            get;
            set;
        }

        internal IEnumerable<string> InputFiles
        {
            get
            {
                if (_inputFiles == null)
                {
                    _inputFiles = GetInputFilesFromOptions();
                }
                return _inputFiles;
            }
        }

        public FileStatus FileStatus
        {
            get;
            set;
        }

        public bool HasArgErrors
        {
            get;
            set;
        }

        public Options Options
        {
            get;
            set;
        }

        public bool NoSignIssues
        {
            get;
            set;
        }

        public Log Log
        {
            get;
            set;
        }

        public int TotalFiles
        {
            get;
            set;
        }

        public int TotalUnsignedFiles
        {
            get;
            set;
        }

        public int TotalSignedFiles
        {
            get;
            set;
        }

        public int TotalSkippedFiles
        {
            get;
            set;
        }

        public int TotalExcludedFiles
        {
            get;
            set;
        }

        public int TotalSkippedExcludedFiles
        {
            get;
            set;
        }

        public SignCheck(string[] args)
        {
            Options = new Options();
            ParserResult<Options> parseResult = Parser.Default.ParseArguments<Options>(args).
                WithParsed(options => HandleOptions(options)).
                WithNotParsed<Options>(errors => HandleErrors(errors));
        }

        public SignCheck(Options options)
        {
            HandleOptions(options ?? new Options());
        }

        private void HandleOptions(Options options)
        {
            Options = options;

            Log = new Log(options.LogFile, options.ErrorLogFile, options.ResultsXmlFile, options.Verbosity);

            if (Options.FileStatus.Count() > 0)
            {
                FileStatus = FileStatus.NoFiles;
                foreach (string value in Options.FileStatus)
                {
                    FileStatus result;
                    if (Enum.TryParse<FileStatus>(value, out result))
                    {
                        FileStatus |= result;
                    }
                    else
                    {
                        Log.WriteError(LogVerbosity.Minimum, SignCheckResources.scErrorUnknownFileStatus, value);
                    }
                }

                if (FileStatus == FileStatus.NoFiles)
                {
                }
            }
            else
            {
                FileStatus = FileStatus.UnsignedFiles;
            }

            if (!String.IsNullOrEmpty(Options.ExclusionsFile))
            {
                ProcessExclusions(Options.ExclusionsFile);
            }
            else
            {
                Exclusions = new Exclusions();
            }
            // Add some well-known exclusions for WiX
            Exclusions.Add(new Exclusion("*netfxca*;*.msi;Wix custom action (NGEN"));
            Exclusions.Add(new Exclusion("*wixdepca*;*.msi;WiX custom action"));
            Exclusions.Add(new Exclusion("*wixuiwixca*;*.msi;WiX custom action"));
            Exclusions.Add(new Exclusion("*wixca*;*.msi;Wix custom action"));
            Exclusions.Add(new Exclusion("*wixstdba.dll*;*.exe;WiX standard bundle application"));

            if (!Directory.Exists(_appData))
            {
                Directory.CreateDirectory(_appData);
            }
        }

        private void HandleErrors(IEnumerable<Error> errors)
        {
            HasArgErrors = true;
        }

        private List<string> GetInputFilesFromOptions()
        {
            var inputFiles = new List<string>();
            var downloadFiles = new List<Uri>();
            if (Options.InputFiles == null)
            {
                return inputFiles;
            }
            foreach (string inputFile in Options.InputFiles)
            {
                Uri uriResult;

                if ((Uri.TryCreate(inputFile, UriKind.Absolute, out uriResult)) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    string downloadPath = Path.Combine(_appData, Path.GetFileName(uriResult.LocalPath));
                    inputFiles.Add(downloadPath);
                    downloadFiles.Add(uriResult);
                }
                else if (inputFile.IndexOfAny(_wildcards) > -1)
                {
                    SearchOption fileSearchOptions = Options.TraverseSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                    string fileSearchPath = Path.GetDirectoryName(inputFile);
                    string fileSearchPattern = Path.GetFileName(inputFile);
                    string[] matchedFiles = null;

                    if (String.IsNullOrEmpty(fileSearchPath))
                    {
                        // CASE 1: No path, pattern in filename, e.g. "--input-File *.txt" or "-i Foo?Bar.txt"
                        fileSearchPath = Directory.GetCurrentDirectory();
                        matchedFiles = Directory.GetFiles(fileSearchPath, fileSearchPattern, fileSearchOptions);
                    }
                    else
                    {
                        if (fileSearchPath.IndexOfAny(_wildcards) > -1)
                        {
                            // CASE 2: Path contains wildcards, e.g. "-i C:\Foo*\Bar.txt" or "-i C:\Foo*\Bar*.txt"
                            string[] wildcardDirectories = Utils.GetDirectories(fileSearchPath, null, fileSearchOptions);

                            var _matchedFiles = new List<string>();

                            foreach (string dir in wildcardDirectories)
                            {
                                _matchedFiles.AddRange(Directory.GetFiles(dir, fileSearchPattern, fileSearchOptions));
                            }

                            matchedFiles = _matchedFiles.ToArray();
                        }
                        else
                        {
                            // CASE 3: Path contains no search patterns, e.g. "-i C:\Foo\Bar\*.txt"
                            if (Directory.Exists(fileSearchPath))
                            {
                                matchedFiles = Directory.GetFiles(fileSearchPath, fileSearchPattern, fileSearchOptions);
                            }
                            else
                            {
                                Log.WriteError(String.Format(SignCheckResources.scDirDoesNotExist, fileSearchPath));
                            }
                        }
                    }

                    if (matchedFiles != null)
                    {
                        foreach (string file in matchedFiles)
                        {
                            inputFiles.Add(file);
                        }
                    }
                }
                else
                {
                    if (Directory.Exists(inputFile))
                    {
                        SearchOption searchOption = Options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                        foreach (string dirFile in Directory.GetFiles(inputFile, "*.*", searchOption))
                        {
                            inputFiles.Add(dirFile);
                        }
                    }
                    else if (File.Exists(Path.GetFullPath(inputFile)))
                    {
                        inputFiles.Add(inputFile);
                    }
                    else
                    {
                        Log.WriteError(String.Format(SignCheckResources.scInputFileDoesNotExist, inputFile));
                    }
                }
            }

            if (downloadFiles.Count > 0)
            {
                DownloadFilesAsync(downloadFiles).Wait();
            }

            // Exclude log files in case they are created in the folder being scanned.
            if (!String.IsNullOrEmpty(Options.ErrorLogFile))
            {
                inputFiles.Remove(Path.GetFullPath(Options.ErrorLogFile));
            }

            if (!String.IsNullOrEmpty(Options.LogFile))
            {
                inputFiles.Remove(Path.GetFullPath(Options.LogFile));
            }
            return inputFiles;
        }

        private void ProcessExclusions(string exclusionsFile)
        {
            Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.scProcessExclusions);
            Exclusions = new Exclusions(exclusionsFile);
        }

        private void ProcessResults(IEnumerable<SignatureVerificationResult> results, int indent)
        {
            foreach (SignatureVerificationResult result in results)
            {
                TotalFiles++;
                string outcome = "Unknown";

                if (result.IsSigned && !result.IsExcluded)
                {
                    TotalSignedFiles++;
                    outcome = "Signed";
                }
                else if (!(result.IsExcluded || result.IsSkipped) && (!result.IsSigned && !result.IsDoNotSign))
                {
                    TotalUnsignedFiles++;
                    outcome = "Unsigned";
                }

                if (result.IsExcluded || (!result.IsSigned && result.IsDoNotSign))
                {
                    TotalExcludedFiles++;
                    outcome = "Excluded";
                }

                if (result.IsSkipped)
                {
                    TotalSkippedFiles++;
                    outcome = "Skipped";
                }

                if (result.IsSkipped && result.IsExcluded)
                {
                    TotalSkippedExcludedFiles++;
                    outcome = "SkippedExcluded";
                }

                // Regardless of the file status reporting settings, a container file like an MSI or NuGet package
                // is always reported to keep the file hierarchy in the log readable.
                if (((result.IsSkipped) && ((FileStatus & FileStatus.SkippedFiles) != 0)) ||
                    ((result.IsSigned) && ((FileStatus & FileStatus.SignedFiles) != 0)) ||
                    ((result.IsExcluded) && ((FileStatus & FileStatus.ExcludedFiles) != 0)) ||
                    ((result.IsSigned) && (result.IsDoNotSign)) ||
                    ((result.NestedResults.Count() > 0) && (Options.Recursive)) ||
                    ((FileStatus & FileStatus.AllFiles) == FileStatus.AllFiles) ||
                    ((!result.IsSigned && !result.IsDoNotSign) && (!result.IsSkipped) && (!result.IsExcluded) && ((FileStatus & FileStatus.UnsignedFiles) != 0)))
                {
                    LoggedResults = true;
                    Log.WriteMessage(LogVerbosity.Minimum, String.Empty.PadLeft(indent) + result.ToString(result.IsExcluded ? DetailKeys.ResultKeysExcluded : ResultDetails));
                }

                if (((!result.IsSigned) && (!(result.IsSkipped || result.IsExcluded || result.IsDoNotSign))) || (result.IsSigned && result.IsDoNotSign))
                {
                    NoSignIssues = false;
                }

                Log.WriteStartResult(result, outcome);
                if (result.NestedResults.Count > 0)
                {
                    ProcessResults(result.NestedResults, indent + 2);
                }
                Log.WriteEndResult();
            }
        }

        public void GenerateExclusionsFile(StreamWriter writer, IEnumerable<SignatureVerificationResult> results)
        {
            foreach (SignatureVerificationResult result in results)
            {
                if ((!result.IsSigned) && (!result.IsSkipped))
                {
                    writer.WriteLine(result.ExclusionEntry);
                }

                if (result.NestedResults.Count > 0)
                {
                    GenerateExclusionsFile(writer, result.NestedResults);
                }
            }
        }

        public int Run()
        {
            try
            {
                Log.WriteMessage("Starting execution of SignCheck.");

                SignatureVerificationOptions options = SignatureVerificationOptions.None;
                options |= Options.Recursive ? SignatureVerificationOptions.VerifyRecursive : SignatureVerificationOptions.None;
                options |= Options.EnableXmlSignatureVerification ? SignatureVerificationOptions.VerifyXmlSignatures : SignatureVerificationOptions.None;
                options |= Options.SkipTimestamp ? SignatureVerificationOptions.None : SignatureVerificationOptions.VerifyAuthentiCodeTimestamps;
                options |= Options.VerifyStrongName ? SignatureVerificationOptions.VerifyStrongNameSignature : SignatureVerificationOptions.None;
                options |= Options.EnableJarSignatureVerification ? SignatureVerificationOptions.VerifyJarSignatures : SignatureVerificationOptions.None;
                options |= !String.IsNullOrEmpty(Options.ExclusionsOutput) ? SignatureVerificationOptions.GenerateExclusion : SignatureVerificationOptions.None;

                var signatureVerificationManager = new SignatureVerificationManager(Exclusions, Log, options);

                ResultDetails = Options.Verbosity > LogVerbosity.Normal ? DetailKeys.ResultKeysVerbose : DetailKeys.ResultKeysNormal;

                if (InputFiles != null && InputFiles.Count() > 0)
                {
                    DateTime startTime = DateTime.Now;
                    IEnumerable<SignatureVerificationResult> results = signatureVerificationManager.VerifyFiles(InputFiles);
                    DateTime endTime = DateTime.Now;

                    NoSignIssues = true;
                    Log.WriteLine();
                    Log.WriteMessage(LogVerbosity.Minimum, SignCheckResources.scResults);
                    Log.WriteLine();
                    ProcessResults(results, 0);

                    // Generate an exclusions file for any unsigned files that were reported.
                    if (!String.IsNullOrEmpty(Options.ExclusionsOutput))
                    {
                        if (!Directory.Exists(Options.ExclusionsOutput))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(Options.ExclusionsOutput)));
                        }
                        using (var exclusionsWriter = new StreamWriter(Options.ExclusionsOutput, append: false))
                        {
                            GenerateExclusionsFile(exclusionsWriter, results);
                        }
                    }

                    if (LoggedResults)
                    {
                        Log.WriteLine();
                    }

                    if (NoSignIssues)
                    {
                        Log.WriteMessage(LogVerbosity.Minimum, SignCheckResources.scNoSignIssues);
                    }
                    else
                    {
                        Log.WriteError(LogVerbosity.Minimum, SignCheckResources.scSignIssuesFound);
                    }

                    TimeSpan totalTime = endTime - startTime;
                    Log.WriteMessage(LogVerbosity.Minimum, String.Format(SignCheckResources.scTime, totalTime));
                    Log.WriteMessage(LogVerbosity.Minimum, String.Format(SignCheckResources.scStats,
                        TotalFiles, TotalSignedFiles, TotalUnsignedFiles, TotalSkippedFiles, TotalExcludedFiles, TotalSkippedExcludedFiles));
                }
                else
                {
                    Log.WriteMessage(LogVerbosity.Minimum, SignCheckResources.scNoFilesProcessed);
                }
            }

            catch (Exception e)
            {
                Log.WriteError(e.ToString());
            }
            finally
            {
                if (Log != null)
                {
                    Log.Close();
                }
            }

            return Log.HasLoggedErrors ? -1 : 0;
        }

        private async Task DownloadFileAsync(Uri uri)
        {
            try
            {
                using (var httpClient = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true }))
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(10000); // 10 seconds

                    string downloadPath = Path.Combine(_appData, Path.GetFileName(uri.LocalPath));

                    if (File.Exists(downloadPath))
                    {
                        Log.WriteMessage(LogVerbosity.Detailed, SignCheckResources.scDeleteExistingFile, downloadPath);
                        File.Delete(downloadPath);
                    }

                    Log.WriteMessage(LogVerbosity.Detailed, SignCheckResources.scDownloading, uri.AbsoluteUri, downloadPath);

                    using (var stream = await httpClient.GetStreamAsync(uri))
                    {
                        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.WriteError(e.Message);
            }
        }

        private async Task DownloadFilesAsync(IEnumerable<Uri> uris)
        {
            await Task.WhenAll(uris.Select(u => DownloadFileAsync(u)));
        }
    }
}
