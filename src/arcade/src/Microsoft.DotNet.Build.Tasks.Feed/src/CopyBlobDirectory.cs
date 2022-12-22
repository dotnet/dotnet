// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MSBuild = Microsoft.Build.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public sealed class CopyBlobDirectory : MSBuild.Task
    {
        [Required]
        public string SourceBlobDirectory { get; set; }

        [Required]
        public string TargetBlobDirectory { get; set; }

        [Required]
        public string AccountKey { get; set; }

        public bool Overwrite { get; set; }

        public bool SkipCreateContainer { get; set; } = false;

        public bool SkipIfMissing { get; set; } = false;

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
            
        }

        private static string GetCanonicalStorageUri(string uri, string subPath = null)
        {
            string newUri = uri.TrimEnd('/');
            if (!string.IsNullOrEmpty(subPath))
            {
                newUri = $"{newUri}/{subPath.Trim('/')}";
            }
            return newUri;
        }

        public async Task<bool> ExecuteAsync()
        {
            try
            {
                Log.LogMessage("Performing blob merge...");

                if (string.IsNullOrEmpty(SourceBlobDirectory) || string.IsNullOrEmpty(TargetBlobDirectory))
                {
                    Log.LogError($"Please specify a source blob directory and a target blob directory");
                }
                else
                {
                    // Canonicalize the target uri
                    string targetUri = GetCanonicalStorageUri(TargetBlobDirectory);
                    // Invoke the blob URI parser on the target URI and deal with any container creation that needs to happen
                    BlobUrlInfo targetUrlInfo = new BlobUrlInfo(targetUri);
                    CloudStorageAccount storageAccount = new CloudStorageAccount(new WindowsAzure.Storage.Auth.StorageCredentials(targetUrlInfo.AccountName, AccountKey), true);
                    CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer targetContainer = client.GetContainerReference(targetUrlInfo.ContainerName);

                    if (!SkipCreateContainer)
                    {
                        Log.LogMessage($"Creating container {targetUrlInfo.ContainerName} if it doesn't exist.");
                        await targetContainer.CreateIfNotExistsAsync();
                    }

                    string sourceUri = GetCanonicalStorageUri(SourceBlobDirectory);
                    // Grab the source blob path from the source info and combine with the target blob path.
                    BlobUrlInfo sourceBlobInfo = new BlobUrlInfo(sourceUri);

                    // For now the source and target storage accounts should be the same, so the same account key is used for each.
                    if (sourceBlobInfo.AccountName != targetUrlInfo.AccountName)
                    {
                        Log.LogError($"Source and target storage accounts should be identical");
                    }
                    else
                    {
                        CloudBlobContainer sourceContainer = client.GetContainerReference(sourceBlobInfo.ContainerName);

                        Log.LogMessage($"Listing blobs in {sourceUri}");

                        // Get all source URI's with the blob prefix
                        BlobContinuationToken token = null;
                        List<IListBlobItem> sourceBlobs = new List<IListBlobItem>();
                        do
                        {
                            BlobResultSegment segment = await sourceContainer.ListBlobsSegmentedAsync(sourceBlobInfo.BlobPath, true,
                                BlobListingDetails.None, null, token, new BlobRequestOptions(), null);
                            token = segment.ContinuationToken;
                            sourceBlobs.AddRange(segment.Results);
                        }
                        while (token != null);

                        // Ensure the source exists
                        if (!SkipIfMissing && sourceBlobs.Count == 0)
                        {
                            Log.LogError($"No blobs found in {sourceUri}");
                        }

                        await Task.WhenAll(sourceBlobs.Select(async blob =>
                        {
                            // Determine the relative URI for the target.  This works properly when the
                            // trailing slash is left off of the source and target URIs.
                            string relativeBlobPath = blob.Uri.ToString().Substring(sourceUri.Length);
                            string specificTargetUri = GetCanonicalStorageUri(targetUri, relativeBlobPath);
                            BlobUrlInfo specificTargetBlobUrlInfo = new BlobUrlInfo(specificTargetUri);
                            CloudBlob targetBlob = targetContainer.GetBlobReference(specificTargetBlobUrlInfo.BlobPath);

                            Log.LogMessage($"Merging {blob.Uri.ToString()} into {targetBlob.Uri.ToString()}");

                            if (!Overwrite && await targetBlob.ExistsAsync())
                            {
                                Log.LogError($"Target blob {targetBlob.Uri.ToString()} already exists.");
                            }
                            
                            await targetBlob.StartCopyAsync(blob.Uri);
                        }));
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
