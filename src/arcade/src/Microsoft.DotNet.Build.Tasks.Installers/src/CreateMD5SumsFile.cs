// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    /// <summary>
    /// Produce an md5sums file for a set of files.
    /// </summary>
    /// <remarks>
    /// Emits a file of the format specified by https://manpages.debian.org/bookworm/dpkg-dev/deb-md5sums.5.en.html
    /// </remarks>
    public sealed class CreateMD5SumsFile : BuildTask
    {
        [Required]
        public string RootDirectory { get; set; }
        [Required]
        public ITaskItem[] Files { get; set; }
        [Required]
        public string OutputFile { get; set; }

        [Output]
        public string InstalledSize { get; set; }

        public override bool Execute()
        {
            using FileStream outputFile = File.OpenWrite(OutputFile);
            using StreamWriter writer = new(outputFile, Encoding.ASCII);
            ulong installedSize = 0;
            foreach (ITaskItem file in Files)
            {
                using MD5 md5 = MD5.Create();
                using FileStream fileStream = File.OpenRead(file.ItemSpec);
                installedSize += (ulong)fileStream.Length;
                byte[] hash = md5.ComputeHash(fileStream);
                string relativePath = file.ItemSpec.Substring(RootDirectory.Length).TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/');
                // Always use Linux line-endings
#if NET
                writer.Write($"{Convert.ToHexString(hash)} {relativePath}\n");
#else
                writer.Write($"{BitConverter.ToString(hash).Replace("-", "")} {relativePath}\n");
#endif
            }

            InstalledSize = installedSize.ToString();

            return true;
        }
    }
}
