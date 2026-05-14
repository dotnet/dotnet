// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using System.Collections.Generic;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using Microsoft.DotNet.Build.Tasks.Feed;

namespace Microsoft.DotNet.Build.CloudTestTasks
{
    public class UploadToAzure : AzureConnectionStringBuildTask, ICancelableTask
    {
        private static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();
        private static readonly CancellationToken CancellationToken = TokenSource.Token;

        /// <summary>
        /// The name of the container to access.  The specified name must be in the correct format, see the
        /// following page for more info.  https://msdn.microsoft.com/en-us/library/azure/dd135715.aspx
        /// </summary>
        [Required]
        public string ContainerName { get; set; }

        /// <summary>
        /// An item group of files to upload.  Each item must have metadata RelativeBlobPath
        /// that specifies the path relative to ContainerName where the item will be uploaded.
        /// </summary>
        [Required]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Indicates if the destination blob should be overwritten if it already exists.  The default if false.
        /// </summary>
        public bool Overwrite { get; set; } = false;

        /// <summary>
        /// Enables idempotency when Overwrite is false.
        /// 
        /// false: (default) Attempting to upload an item that already exists fails.
        /// 
        /// true: When an item already exists, download the existing blob to check if it's
        /// byte-for-byte identical to the one being uploaded. If so, pass. If not, fail.
        /// </summary>
        public bool PassIfExistingItemIdentical { get; set; }

        /// <summary>
        /// Specifies the maximum number of clients to concurrently upload blobs to azure
        /// </summary>
        [Obsolete]
        public int MaxClients { get; set; } = 8;

        public int UploadTimeoutInMinutes { get; set; } = 5;

        public void Cancel()
        {
            TokenSource.Cancel();
        }

        public override bool Execute()
        {
            return ExecuteAsync(CancellationToken).GetAwaiter().GetResult();
        }

        public async Task<bool> ExecuteAsync(CancellationToken ct)
        {
            if (Items.Length == 0)
            {
                Log.LogError("No items were provided for upload.");
                return false;
            }

            Log.LogMessage("Begin uploading blobs to Azure account {0} in container {1}.",
                AccountName,
                ContainerName);

            try
            {
                AzureStorageUtils blobUtils = new AzureStorageUtils(AccountName, AccountKey, ContainerName);

                List<Task> uploadTasks = new List<Task>();

                foreach (var item in Items)
                {
                    uploadTasks.Add(Task.Run(async () =>
                    {
                        string relativeBlobPath = item.GetMetadata("RelativeBlobPath");

                        if (string.IsNullOrEmpty(relativeBlobPath))
                        {
                            throw new Exception(string.Format("Metadata 'RelativeBlobPath' is missing for item '{0}'.", item.ItemSpec));
                        }

                        if (!File.Exists(item.ItemSpec))
                        {
                            throw new Exception(string.Format("The file '{0}' does not exist.", item.ItemSpec));
                        }

                        BlobClient blobReference = blobUtils.GetBlob(relativeBlobPath);

                        if (!Overwrite && await blobReference.ExistsAsync())
                        {
                            if (PassIfExistingItemIdentical)
                            {
                                if (await blobReference.IsFileIdenticalToBlobAsync(item.ItemSpec))
                                {
                                    return;
                                }
                            }

                            throw new Exception(string.Format("The blob '{0}' already exists.", relativeBlobPath));
                        }

                        CancellationTokenSource timeoutTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(UploadTimeoutInMinutes));

                        using (Stream localFileStream = File.OpenRead(item.ItemSpec))
                        {
                            await blobReference.UploadAsync(localFileStream, timeoutTokenSource.Token);
                        }
                    }));
                }

                await Task.WhenAll(uploadTasks);

                Log.LogMessage("Upload to Azure is complete, a total of {0} items were uploaded.", Items.Length);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
