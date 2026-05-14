// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public class GenerateChecksums : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// An item collection of files for which to generate checksums.  Each item must have metadata
        /// 'DestinationPath' that specifies the path of the checksum file to create.
        /// </summary>
        [Required]
        public ITaskItem[] Items { get; set; }

        public override bool Execute()
        {
            foreach (ITaskItem item in Items)
            {
                try
                {
                    string destinationPath = item.GetMetadata("DestinationPath");
                    if (string.IsNullOrEmpty(destinationPath))
                    {
                        Log.LogError($"Metadata 'DestinationPath' is missing for item '{item.ItemSpec}'.");
                        return !Log.HasLoggedErrors;
                    }

                    if (!File.Exists(item.ItemSpec))
                    {
                        Log.LogError($"The file '{item.ItemSpec}' does not exist.");
                        return !Log.HasLoggedErrors;
                    }

                    Log.LogMessage(MessageImportance.High, $"Generating checksum for '{item.ItemSpec}' into '{destinationPath}'...");

                    using (FileStream stream = File.OpenRead(item.ItemSpec))
                    {
                        using(HashAlgorithm hashAlgorithm = SHA512.Create())
                        {
                            byte[] hash = hashAlgorithm.ComputeHash(stream);
                            string checksum = BitConverter.ToString(hash).Replace("-", string.Empty);
                            File.WriteAllText(destinationPath, checksum);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.LogErrorFromException(e);
                    return !Log.HasLoggedErrors;
                }
            }

            return !Log.HasLoggedErrors;
        }
    }
}
