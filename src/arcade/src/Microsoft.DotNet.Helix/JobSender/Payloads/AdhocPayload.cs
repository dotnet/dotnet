// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Client
{
    internal class AdhocPayload : IPayload
    {
        public AdhocPayload(string[] files)
        {
            if (FindFileNameDuplicate(files, out var duplicateName))
            {
                throw new ArgumentException(
                    $"Names of files to upload have to be distinct. The following name repeats at least once: {Path.GetFileName(duplicateName)}",
                    nameof(files));
            }

            Files = files;
        }

        public string[] Files { get; }

        public async Task<string> UploadAsync(IBlobContainer payloadContainer, Action<string> log, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    foreach (string file in Files)
                    {
                        string name = Path.GetFileName(file);
                        using (Stream entryStream = zip.CreateEntry(name).Open())
                        using (FileStream inputStream = File.OpenRead(file))
                        {
                            await inputStream.CopyToAsync(entryStream, 81920, cancellationToken);
                        }
                    }
                }
                stream.Position = 0;
                Uri zipUri = await payloadContainer.UploadFileAsync(stream, $"{Guid.NewGuid()}.zip", log, cancellationToken);
                return zipUri.AbsoluteUri;
            }
        }

        private bool FindFileNameDuplicate(string[] files, out string duplicateName)
        {
            var filesSeen = new HashSet<string>();
            duplicateName = files.FirstOrDefault(file => !filesSeen.Add(Path.GetFileName(file).ToLowerInvariant()));
            return duplicateName != null;
        }
    }
}
