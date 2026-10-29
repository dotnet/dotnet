// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Helix.Sdk;

[MSBuildMultiThreadableTask]
public class DownloadFromResultsContainer : HelixTask, ICancelableTask, IMultiThreadableTask
{
    /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
    public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

    [Required]
    public ITaskItem[] WorkItems { get; set; }

    [Required]
    public string OutputDirectory { get; set; }

    [Required]
    public string JobId { get; set; }

    [Required]
    public ITaskItem[] MetadataToWrite { get; set; }

    private const string MetadataFile = "metadata.txt";

    private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

    protected override async Task ExecuteCore(CancellationToken cancellationToken) 
    {
        if (string.IsNullOrEmpty(OutputDirectory))
        {
            LogRequiredParameterError(nameof(OutputDirectory));
        }

        if (string.IsNullOrEmpty(JobId))
        {
            LogRequiredParameterError(nameof(JobId));
        }

        if (Log.HasLoggedErrors)
        {
            return;                
        }

        Log.LogMessage(MessageImportance.High, $"Downloading result files for job {JobId}");

        DirectoryInfo directory = Directory.CreateDirectory(TaskEnvironment.GetAbsolutePath(Path.Combine(OutputDirectory, JobId)));
        if (MetadataToWrite != null)
        {
            using (FileStream stream = File.Open(Path.Combine(directory.FullName, MetadataFile), FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                foreach (ITaskItem metadata in MetadataToWrite)
                {
                    await writer.WriteLineAsync(metadata.GetMetadata("Identity"));
                }
            }
        }

        if (WorkItems != null)
        {
            await Task.WhenAll(WorkItems.Select(wi => DownloadFilesForWorkItem(wi, directory.FullName, _cancellationSource.Token)));
        }
    }

    private async Task DownloadFilesForWorkItem(ITaskItem workItem, string directoryPath, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (workItem.TryGetMetadata("DownloadFilesFromResults", out string files))
        {
            string workItemName = workItem.GetMetadata("Identity");
            string[] filesToDownload = files.Split(';');

            // Use the Helix API to get the last possible iteration of the work item's execution 
            var allAvailableFiles = await HelixApi.WorkItem.ListFilesAsync(workItemName, JobId, true, ct);
            var resultsUri = await HelixApi.Job.ResultsAsync(JobId, ct);

            DirectoryInfo destinationDir = Directory.CreateDirectory(TaskEnvironment.GetAbsolutePath(Path.Combine(directoryPath, workItemName)));
            foreach (string file in filesToDownload)
            {
                try
                {
                    string destinationFile = Path.Combine(destinationDir.FullName, file);
                    Log.LogMessage(MessageImportance.Normal, $"Downloading {file} => {destinationFile} ...");

                    // Ensure directory exists - A noop if it already does
                    Directory.CreateDirectory(Path.Combine(destinationDir.FullName, Path.GetDirectoryName(file)));

                    // Helix clients currently provide file paths in the format of the executing OS;
                    // the Arcade feature historically only worked with / so only check one direction of conversion.
                    var fileAvailableForDownload = allAvailableFiles.Where(f => f.Name == file || f.Name.Replace('\\', '/') == file).FirstOrDefault();

                    if (fileAvailableForDownload == null) 
                    {
                        Log.LogWarning($"Work item {workItemName} in Job {JobId} did not upload a result file with path '{file}' ");
                        continue;
                    }

                    // Default timeout for blob operations is 100 seconds, this might not be enough for large result files or low end machines.
                    var blobClientOptions = new BlobClientOptions
                    {
                        Retry =
                        {
                            NetworkTimeout = TimeSpan.FromMinutes(5)
                        }
                    };

                    BlobClient blob;
                    var (blobUri, sasToken) = GetBlobUriAndSas(fileAvailableForDownload.Link, resultsUri?.ResultsUriRSAS);
                    if (string.IsNullOrEmpty(sasToken)) 
                    {
                        blob = new BlobClient(blobUri, blobClientOptions);
                    }
                    else 
                    {
                        blob = new BlobClient(blobUri, new AzureSasCredential(sasToken), blobClientOptions);
                    }
                    await blob.DownloadToAsync(destinationFile);
                }
                catch (RequestFailedException rfe)
                {
                    Log.LogWarning($"Failed to download file '{file}' from results container for work item '{workItemName}': {rfe.Message}");
                }
            }
        };
        return;
    }

    internal static (Uri BlobUri, string SasToken) GetBlobUriAndSas(string link, string resultsUriRSAS)
    {
        if (string.IsNullOrEmpty(resultsUriRSAS))
        {
            return (new Uri(link), null);
        }

        int queryIndex = link.IndexOf('?');
        string strippedUrl = queryIndex >= 0 ? link.Substring(0, queryIndex) : link;
        string sasToken = resultsUriRSAS.StartsWith("?", StringComparison.Ordinal)
            ? resultsUriRSAS.Substring(1)
            : resultsUriRSAS;

        return (new Uri(strippedUrl), sasToken);
    }

    private void LogRequiredParameterError(string parameter)
    {
        Log.LogError(FailureCategory.Build, $"Required parameter {parameter} string was null or empty");
    }
}
