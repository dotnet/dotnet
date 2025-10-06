// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.DotNet.Build.CloudTestTasks;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    /// <summary>
    /// Creates an Azure blob storage container with a unique name. If there is already a container named [ContainerName] will
    /// try creating a container [ContainerName]-1, [ContainerName]-2 and so on until the name is unique.
    /// The final name is saved in ContainerName.
    /// </summary>
    public class CreateNewAzureContainer : CreateAzureContainer
    {
        public override async Task<AzureStorageUtils> GetBlobStorageUtilsAsync()
        {
            int version = 0;
            string versionedContainerName = ContainerName;

            AzureStorageUtils blobUtils;
            bool needsUniqueName;
            do
            {
                blobUtils = AccountKey is null ?
                    new AzureStorageUtils(AccountName, new AzureCliCredential(), versionedContainerName) :
                    new AzureStorageUtils(AccountName, AccountKey, versionedContainerName);
                if (await blobUtils.CheckIfContainerExistsAsync())
                {
                    versionedContainerName = $"{ContainerName}-{++version}";
                    needsUniqueName = true;
                }
                else
                {
                    needsUniqueName = false;
                    ContainerName = versionedContainerName;
                }
            }
            while (needsUniqueName);

            return blobUtils;
        }
    }
}
