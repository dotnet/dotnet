// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Azure.Identity;
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

        public override async Task<AzureStorageUtils> GetBlobStorageUtilsAsync()
        {
            var blobUtils = AccountKey is null ?
                new AzureStorageUtils(AccountName, new AzureCliCredential(), ContainerName) :
                new AzureStorageUtils(AccountName, AccountKey, ContainerName);

            if (FailIfExists && await blobUtils.CheckIfContainerExistsAsync())
            {
                throw new System.InvalidOperationException($"Container {ContainerName} already exists in storage account {AccountName}.");
            }

            return blobUtils;
        }
    }
}
