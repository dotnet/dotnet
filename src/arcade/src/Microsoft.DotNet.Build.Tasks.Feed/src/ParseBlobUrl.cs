// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MSBuild = Microsoft.Build.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public sealed class ParseBlobUrl : MSBuild.Task
    {
        [Required]
        public string BlobUrl { get; set; }

        [Output]
        public ITaskItem BlobElements { get; set; }

        public override bool Execute()
        {
            try
            {
                if (BlobUrl == null)
                {
                    Log.LogError($"No input blob url specified.");
                }
                else
                {
                    Log.LogMessage(MessageImportance.Low, "Parsing {0}", BlobUrl);

                    BlobUrlInfo info = new BlobUrlInfo(BlobUrl);

                    BlobElements = new TaskItem(BlobUrl);
                    BlobElements.SetMetadata("AccountName", info.AccountName);
                    BlobElements.SetMetadata("ContainerName", info.ContainerName);
                    BlobElements.SetMetadata("Endpoint", info.Endpoint);
                    BlobElements.SetMetadata("BlobPath", info.BlobPath);
                    return true;
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
