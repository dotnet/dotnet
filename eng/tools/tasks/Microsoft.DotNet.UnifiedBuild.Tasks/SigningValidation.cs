// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

public class SigningValidation : Microsoft.Build.Utilities.Task
{
    /// <summary>
    /// Directory where the blobs and packages were downloaded to
    /// </summary>
    [Required]
    public required string ArtifactDownloadDirectory { get; init; }

    /// <summary>
    /// Branch that the task is running on
    /// </summary>
    [Required]
    public required string SourceBranch { get; init; }

    /// <summary>
    /// Path to the dotnet root directory
    /// </summary>
    [Required]
    public required string DotNetRootDirectory { get; init; }

    /// <summary>
    /// Path to the output logs directory
    /// </summary>
    [Required]
    public required string OutputLogsDirectory { get; init; }

    /// <summary>
    /// Files that are signed during DAC signing.
    /// These files are excluded from SignCheck on non-release branches.
    /// </summary>
    private static readonly string[] _dacSignedFiles = new[]
    {
        "mscordaccore.dll",
        "mscordbi.dll"
    };

    /// <summary>
    /// Directory where the sign check files are copied to
    /// </summary>
    private static readonly string _signCheckFilesDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "SignCheckFiles");

    private const string _signCheckExclusionsFileName = "SignCheckExclusionsFile.txt";
    private const string _signCheckStdoutLogFileName = "signcheck.log";
    private const string _signCheckStderrLogFileName = "signcheck.error.log";
    private const string _signCheckResultsXmlFileName = "signcheck.xml";
    private const int _signCheckTimeout = 60 * 60 * 1000 * 2; // 2 hours

    public override bool Execute()
    {
        try
        {
            ForceDirectory(OutputLogsDirectory);

            PrepareFilesToSignCheck();

            RunSignCheck();

            ProcessSignCheckResults();

            Log.LogMessage(MessageImportance.High, "Signing validation completed.");
        }
        catch (Exception ex)
        {
            Log.LogError($"Signing validation failed: {ex.Message}");
        }
        finally
        {
            if (Directory.Exists(_signCheckFilesDirectory))
            {
                Directory.Delete(_signCheckFilesDirectory, true);
            }
        }

        return !Log.HasLoggedErrors;
    }

    /// <summary>
    /// Gets the list of files to sign check from the merged manifest
    /// and copies them to the sign check directory.
    /// </summary>
    private void PrepareFilesToSignCheck()
    {
        Log.LogMessage(MessageImportance.High, "Preparing files to sign check...");

        List<(string artifactName, string fileName)> filesToSignCheck = [];

        foreach (string artifactDirectory in Directory.EnumerateDirectories(ArtifactDownloadDirectory))
        {
            var manifestsDir = Path.Combine(artifactDirectory, "manifests");
            if (!Directory.Exists(manifestsDir))
            {
                continue;
            }
            foreach (string manifest in Directory.EnumerateFiles(manifestsDir, "*.xml", SearchOption.AllDirectories))
            {
                using (Stream xmlStream = File.OpenRead(manifest))
                {
                    XDocument doc = XDocument.Load(xmlStream);

                    // Extract blobs
                    filesToSignCheck.AddRange(doc.Descendants("Blob")
                        .Where(blob => IsReleaseShipping(blob) && IsExternallyVisible(blob))
                        .Select(blob => (artifactDirectory, ExtractAttribute(blob, "PipelineArtifactPath"))));

                    // Extract packages
                    filesToSignCheck.AddRange(doc.Descendants("Package")
                        .Where(pkg => IsReleaseShipping(pkg) && IsExternallyVisible(pkg))
                        .Select(pkg => (artifactDirectory, ExtractAttribute(pkg, "PipelineArtifactPath"))));
                }
            }
        }

        ForceDirectory(_signCheckFilesDirectory);
        int count = 0;

        // Copy the shipping blobs and packages from the download directory to the signcheck directory
        foreach ((string artifactDirectory, string file) in filesToSignCheck)
        {
            // Ignore files we don't care about
            if (Path.GetExtension(file) == ".txt" || Path.GetExtension(file) == ".sha512")
            {
                continue;
            }

            string sourcePath = Path.Combine(artifactDirectory, file);
            string destinationPath = Path.Combine(_signCheckFilesDirectory, file);
            string? destinationDirectory = Path.GetDirectoryName(destinationPath);

            if (string.IsNullOrEmpty(destinationPath) || string.IsNullOrEmpty(destinationDirectory))
            {
                Log.LogWarning($"Invalid destination path.");
                continue;
            }

            if (!File.Exists(sourcePath))
            {
                Log.LogMessage($"File {file} missing from {artifactDirectory}, skipping.");
                continue;
            }

            ForceDirectory(destinationDirectory);

            if (File.Exists(destinationPath))
            {
                Log.LogWarning($"File {file} already exists in {_signCheckFilesDirectory}, skipping.");
            }

            File.Move(sourcePath, destinationPath, true);
            count++;
        }

        Log.LogMessage(MessageImportance.High, $"Prepared {count} files to sign check...");
    }

    /// <summary>
    /// Runs the signcheck task on the specified package base path
    /// </summary>
    private void RunSignCheck()
    {
        using (var process = new Process())
        {
            (string command, string arguments) = GetSignCheckCommandAndArguments();

            process.StartInfo = new ProcessStartInfo()
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            // SignCheck writes console output to log files and the output stream.
            // To avoid cluttering the console, redirect the output to empty handlers.
            process.OutputDataReceived += (sender, args) => { };
            process.ErrorDataReceived += (sender, args) => { };

            Log.LogMessage(MessageImportance.High, $"Running SignCheck...");

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            bool hasExited = process.WaitForExit(_signCheckTimeout);
            if (!hasExited)
            {
                throw new TimeoutException($"SignCheck timed out after {_signCheckTimeout / 1000} seconds.");
            }

            string errorLog = GetLogPath(_signCheckStderrLogFileName);
            string errorLogContent = File.Exists(errorLog) ? File.ReadAllText(errorLog).Trim() : string.Empty;
            if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(errorLogContent))
            {
                // We don't want to throw here because SignCheck will fail for unsigned files
                Log.LogError($"SignCheck failed with exit code {process.ExitCode}: {errorLogContent}");
            }

            string stdoutLog = GetLogPath(_signCheckStdoutLogFileName);
            string stdoutLogContent = File.Exists(stdoutLog) ? File.ReadAllText(stdoutLog).Trim() : string.Empty;
            if (!string.IsNullOrWhiteSpace(stdoutLogContent) && stdoutLogContent.Contains("No files were processed"))
            {
                Log.LogError("SignCheck did not process any files.");
            }

            Log.LogMessage(MessageImportance.High, $"SignCheck completed.");
        }
    }

    private void ProcessSignCheckResults()
    {
        string resultsXml = GetLogPath(_signCheckResultsXmlFileName);
        if (!File.Exists(resultsXml))
        {
            throw new FileNotFoundException($"SignCheck results XML file not found: {resultsXml}");
        }

        var results = XDocument.Load(resultsXml).Descendants("File");

        var unsignedResults = ExtractResults(results, "Outcome", "Unsigned");
        var doNotSignResults = ExtractResults(results, "Error", "matches a DO-NOT-SIGN exclusion and is signed");

        bool hasUnsignedFiles = LogAndCheckResults(unsignedResults, "unsigned files");
        bool signedDoNotSignFiles = LogAndCheckResults(doNotSignResults, "DO-NOT-SIGN violations");

        if (hasUnsignedFiles || signedDoNotSignFiles)
        {
            throw new Exception("SignCheck detected signing issues. See logs for details.");
        }
    }

    /// <summary>
    /// Extracts the results from the SignCheck XML file based on the specified attribute and match value.
    /// </summary>
    /// <param name="results">The results from the SignCheck XML file.</param>
    /// <param name="attributeName">The attribute name to match.</param>
    /// <param name="matchValue">The value to match against the attribute.</param>
    private IEnumerable<string> ExtractResults(IEnumerable<XElement> results, string attributeName, string matchValue)
    {
        return results
            .Where(result => ExtractAttribute(result, attributeName).Contains(matchValue))
            .Select(result =>
            {
                string fileName = ExtractAttribute(result, "Name");
                if (string.IsNullOrEmpty(fileName))
                {
                    return string.Empty;
                }

                string otherAttributes = string.Join(" ", result.Attributes()
                    .Where(a => a.Name != "Name")
                    .Select(a => $"{a.Name}=\"{a.Value}\""));
                return $"{fileName}: {otherAttributes}";
            })
            .Where(result => !string.IsNullOrEmpty(result));
    }

    /// <summary>
    /// Logs the results and sets the error flag if there are any issues.
    /// </summary>
    /// <param name="results">The results to log.</param>
    /// <param name="issueType">The type of issue (e.g., "unsigned files").</param>
    /// <returns>True if there are issues, otherwise false.</returns>
    private bool LogAndCheckResults(IEnumerable<string> results, string issueType)
    {
        if (results.Any())
        {
            Log.LogWarning($"There are {results.Count()} {issueType}.");
            foreach (string result in results)
            {
                Log.LogMessage(MessageImportance.High, $"   {result}");
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the command and arguments to run signcheck
    /// </summary>
    private (string command, string arguments) GetSignCheckCommandAndArguments()
    {
        string sdkTaskScript = Path.Combine(DotNetRootDirectory, "eng", "common", "sdk-task");

        string argumentsTemplate =
            $"'{sdkTaskScript}.$scriptExtension$' " +
            $"$argumentPrefix$task SigningValidation " +
            $"$argumentPrefix$restore " +
            $"/p:PackageBasePath='{_signCheckFilesDirectory}' " +
            $"/p:EnableStrongNameCheck=true " +
            $"/p:EnableJarSigningCheck=true " +
            $"/p:SignCheckLog='{GetLogPath(_signCheckStdoutLogFileName)}' " +
            $"/p:SignCheckErrorLog='{GetLogPath(_signCheckStderrLogFileName)}' " +
            $"/p:SignCheckResultsXmlFile='{GetLogPath(_signCheckResultsXmlFileName)}' " +
            $"/p:SignCheckExclusionsFile='{GetSignCheckExclusionsFile()}' " +
            $"$additionalArgs$";

        string command = string.Empty;
        string arguments = string.Empty;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            command = "powershell.exe";
            string formattedArguments = argumentsTemplate
                .Replace("$scriptExtension$", "ps1")
                .Replace("$argumentPrefix$", "-")
                .Replace("$additionalArgs$", "-msbuildEngine vs");
            arguments = $"& \"{formattedArguments}\"";
        }
        else
        {
            command = "/bin/bash";
            string formattedArguments = argumentsTemplate
                .Replace("$scriptExtension$", "sh")
                .Replace("$argumentPrefix$", "--")
                .Replace("$additionalArgs$", string.Empty);
            arguments = $"-c \"{formattedArguments}\"";
        }

        return (command, arguments);
    }

    /// <summary>
    /// Extracts the value of the specified attribute from the element and logs an error if it's missing or empty.
    /// </summary>
    private string ExtractAttribute(XElement element, string attributeName)
        =>  element.Attribute(attributeName)?.Value ?? string.Empty;

    /// <summary>
    /// Gets the path to the log file in the output logs directory.
    /// </summary>
    private string GetLogPath(string fileName)
        => Path.Combine(OutputLogsDirectory, fileName);

    /// <summary>
    /// Gets the SignCheckExclusionsFile path.
    /// Adds exclusions to the file and writes it to a temporary location.
    /// </summary>
    private string GetSignCheckExclusionsFile()
    {
        string exclusionsFile = Path.Combine(DotNetRootDirectory, "eng", _signCheckExclusionsFileName);
        
        var releaseBranchRegex = new Regex(@"^refs/heads/(internal/)?release/.*$");
        if (!releaseBranchRegex.IsMatch(SourceBranch))
        {
            // We need to exclude DAC signed files from SignCheck on non-release branches
            // because DAC signing is done only on release branches.

            // Write the updated exclusions file to the log directory for debugging purposes.
            string updatedExclusionsFile = GetLogPath(_signCheckExclusionsFileName);
            File.Copy(exclusionsFile, updatedExclusionsFile, true);

            string dacExclusionTemplate = "{0};;DAC_SIGNED_FILE, DAC signing is done only on release branches";
            foreach (string file in _dacSignedFiles)
            {
                string exclusion = string.Format(dacExclusionTemplate, file);
                File.AppendAllText(updatedExclusionsFile, exclusion + Environment.NewLine);
            }
            return updatedExclusionsFile;
        }

        return exclusionsFile;
    }

    /// <summary>
    /// Checks if the element has the "DotNetReleaseShipping" attribute set to "true".
    /// </summary>
    private static bool IsReleaseShipping(XElement element)
        => element.Attribute("DotNetReleaseShipping")?.Value == "true";

    /// <summary>
    /// Checks if the element has external visibility.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private static bool IsExternallyVisible(XElement element)
        => element.Attribute("Visibility")?.Value == "External";

    /// <summary>
    /// Creates the directory if it does not exist
    /// </summary>
    /// <param name="directory">The directory to create</param>
    private static void ForceDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
