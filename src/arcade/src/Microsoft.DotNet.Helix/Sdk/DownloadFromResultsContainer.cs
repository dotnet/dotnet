using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Helix.Sdk
{
    public class DownloadFromResultsContainer : BaseTask, ICancelableTask
    {
        [Required]
        public ITaskItem[] WorkItems { get; set; }

        [Required]
        public string ResultsContainer { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        [Required]
        public string JobId { get; set; }

        [Required]
        public ITaskItem[] MetadataToWrite { get; set; }

        public string ResultsContainerReadSAS { get; set; }

        private const string MetadataFile = "metadata.txt";

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        public void Cancel() => _cancellationSource.Cancel();

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(ResultsContainer))
            {
                LogRequiredParameterError(nameof(ResultsContainer));
            }

            if (string.IsNullOrEmpty(OutputDirectory))
            {
                LogRequiredParameterError(nameof(OutputDirectory));
            }

            if (string.IsNullOrEmpty(JobId))
            {
                LogRequiredParameterError(nameof(JobId));
            }

            if (!Log.HasLoggedErrors)
            {
                Log.LogMessage(MessageImportance.High, $"Downloading result files for job {JobId}");
                ExecuteCore().GetAwaiter().GetResult();
            }

            return !Log.HasLoggedErrors;
        }

        private async Task ExecuteCore()
        {
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(OutputDirectory, JobId));
            using (FileStream stream = File.Open(Path.Combine(directory.FullName, MetadataFile), FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                foreach (ITaskItem metadata in MetadataToWrite)
                {
                    await writer.WriteLineAsync(metadata.GetMetadata("Identity"));
                }
            }

            ResultsContainer = ResultsContainer.EndsWith("/") ? ResultsContainer : ResultsContainer + "/";
            await Task.WhenAll(WorkItems.Select(wi => DownloadFilesForWorkItem(wi, directory.FullName, _cancellationSource.Token)));
            return;
        }

        private async Task DownloadFilesForWorkItem(ITaskItem workItem, string directoryPath, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (workItem.TryGetMetadata("DownloadFilesFromResults", out string files))
            {
                string workItemName = workItem.GetMetadata("Identity");
                string[] filesToDownload = files.Split(';');

                DirectoryInfo destinationDir = Directory.CreateDirectory(Path.Combine(directoryPath, workItemName));
                foreach (var file in filesToDownload)
                {
                    try
                    {
                        string destinationFile = Path.Combine(destinationDir.FullName, file);
                        Log.LogMessage(MessageImportance.Normal, $"Downloading {file} => {destinationFile}...");

                        // Currently the blob storage includes the retry iteration, however there is no good way
                        // to get the "best" iteration number to download the files from. For now, use always iteration
                        // 1 until helix provides an API to get result files from the "good" iteration run.
                        // https://github.com/dotnet/core-eng/issues/13983
                        var uri = new Uri($"{ResultsContainer}{workItemName}/1/{file}");
                        BlobClient blob = string.IsNullOrEmpty(ResultsContainerReadSAS) ? new BlobClient(uri) : new BlobClient(uri, new AzureSasCredential(ResultsContainerReadSAS));
                        await blob.DownloadToAsync(destinationFile);
                    }
                    catch (RequestFailedException rfe)
                    {
                        Log.LogWarning($"Failed to download {workItemName}/1/{file} blob from results container: {rfe.Message}");
                    }
                }
            };
            return;
        }

        private void LogRequiredParameterError(string parameter)
        {
            Log.LogError(FailureCategory.Build, $"Required parameter {parameter} string was null or empty");
        }
    }
}
