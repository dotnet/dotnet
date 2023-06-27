// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.DotNet.Build.CloudTestTasks;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class CreateAzureContainerIfNotExists : CreateAzureContainer
    {
        /// <summary>
        /// When false, if the specified container already exists get a reference to it.
        /// When true, if the specified container already exists, fail the task.
        /// </summary>
        public bool FailIfExists { get; set; }

        public override async Task<AzureStorageUtils> GetBlobStorageUtilsAsync(string accountName, string accountKey, string containerName)
        {
            AzureStorageUtils blobUtils = new(accountName, accountKey, containerName);

            if (FailIfExists && await blobUtils.CheckIfContainerExistsAsync())
            {
                throw new System.InvalidOperationException($"Container {containerName} already exists in storage account {accountName}.");
            }

            return blobUtils;
        }
    }
}
