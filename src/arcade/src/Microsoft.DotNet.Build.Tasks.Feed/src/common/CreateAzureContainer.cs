// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.Threading.Tasks;
using System;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Models;
using Microsoft.DotNet.Build.CloudTestTasks;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public abstract class CreateAzureContainer : AzureConnectionStringBuildTask
    {
        /// <summary>
        /// The name of the container to create.  The specified name must be in the correct format, see the
        /// following page for more info.  https://msdn.microsoft.com/en-us/library/azure/dd135715.aspx
        /// </summary>
        [Required]
        public string ContainerName { get; set; }

        /// <summary>
        /// The URI of the created container.
        /// </summary>
        [Output]
        public string StorageUri { get; set; }

        /// <summary>
        /// Whether the Container to be created is public or private
        /// </summary>
        public bool IsPublic { get; set; } = false;

        public abstract Task<AzureStorageUtils> GetBlobStorageUtilsAsync();

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }

        public async Task<bool> ExecuteAsync()
        {
            try
            {
                AzureStorageUtils blobUtils = await GetBlobStorageUtilsAsync();

                PublicAccessType permissions = IsPublic ? PublicAccessType.Blob : PublicAccessType.None;

                StorageUri = await blobUtils.CreateContainerAsync(permissions);

                Log.LogMessage(MessageImportance.High, $"Created blob storage container {StorageUri}");
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
