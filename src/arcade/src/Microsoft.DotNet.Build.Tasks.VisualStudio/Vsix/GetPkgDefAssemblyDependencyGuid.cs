// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    /// <summary>
    /// Calculates Guid used in .pkgdef files for codeBase and bindingRedirect entries.
    /// The implementation matches Microsoft.VisualStudio.Shell.ProvideDependentAssemblyAttribute.
    /// </summary>
    public sealed class GetPkgDefAssemblyDependencyGuid : Microsoft.Build.Utilities.Task
    {
        [Required]
        public ITaskItem[] Items { get; set; }

        public string InputMetadata { get; set; }

        [Required]
        public string OutputMetadata { get; set; }

        [Output]
        public ITaskItem[] OutputItems { get; set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            OutputItems = Items;
           
            foreach (var item in Items)
            {
                var value = string.IsNullOrEmpty(InputMetadata) ? item.ItemSpec : item.GetMetadata(InputMetadata);
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                byte[] fullHash;
                using (var sha2 = SHA256.Create())
                {
                    fullHash = sha2.ComputeHash(Encoding.UTF8.GetBytes(value));
                }

                int targetBlockSize = Marshal.SizeOf(typeof(Guid));

                // SHA256 will produce a 32 byte hash, but GUIDs are only 16 bytes in size, so we simply partition the hash array into
                // two 16 byte arrays (via Take and Skip) and then Zip them together into a resultant 16 byte array by XORing the 
                // corresponding byte from each array and storing the result.
                byte[] reducedHash = fullHash.Take(targetBlockSize).Zip(fullHash.Skip(targetBlockSize), (b1, b2) => (byte)(b1 ^ b2)).ToArray();
                Debug.Assert(reducedHash.Length == targetBlockSize);

                item.SetMetadata(OutputMetadata, new Guid(reducedHash).ToString("B").ToUpperInvariant());
            }
        }
    }
}
