// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Represents a single file associated with a release component such as an SDK or runtime.
    /// </summary>
    public class ReleaseFile : IEquatable<ReleaseFile>
    {
        private static readonly SHA512 s_defaultHashAlgorithm = SHA512.Create();

        /// <summary>
        /// The URL from where to download the file.
        /// </summary>
        public Uri Address
        {
            get;
            private set;
        }

        /// <summary>
        /// The filename and extension of this <see cref="ReleaseFile"/>.
        /// </summary>
        public string FileName => Path.GetFileName(Address.LocalPath);

        /// <summary>
        /// The <see cref="SHA512"/> hash of the file.
        /// </summary>
        public string Hash
        {
            get;
            private set;
        }

        /// <summary>
        /// The version agnostic name and extension of the file.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The runtime identifier associated with the file.
        /// </summary>
        public string Rid
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new <see cref="ReleaseFile"/> instance from a <see cref="JsonElement"/>.
        /// </summary>
        /// <param name="fileElement">The <see cref="JsonElement"/> to deserialize.</param>
        internal ReleaseFile(JsonElement fileElement)
        {
            Address = fileElement.GetUriOrDefault("url");
            Hash = fileElement.GetStringOrDefault("hash");
            Name = fileElement.GetStringOrDefault("name");
            Rid = fileElement.GetStringOrDefault("rid");
        }

        /// <summary>
        /// Creates a new <see cref="ReleaseFile"/> instance.
        /// </summary>
        /// <param name="address">The URL of the file.</param>
        /// <param name="hash">A string containing the SHA512 hash of the file.</param>
        /// <param name="name">The name and extension of the file.</param>
        /// <param name="rid">The RID associated with the file.</param>
        internal ReleaseFile(Uri address, string hash, string name, string rid)
        {
            Address = address;
            Hash = hash;
            Name = name;
            Rid = rid;
        }

        /// <summary>
        /// Download this file to the specified local file and verify the file hash. If the destination file exists, the new copy is
        /// downloaded to a temporary file before verifying its hash. If the hash check fails, the temporary file is deleted. Otherwise,
        /// the temporary file is copied to the destination path. If the destination file does not exist, the file is downloaded and
        /// the hash is verified. If the hash check fails, the destination file is deleted.
        /// </summary>
        /// <param name="destinationPath">The path, including the filename of the local file. The file will be
        /// overwritten if it already exists if the hash check passed.</param>
        /// <exception cref="InvalidDataException">Thrown if the downloaded file's hash does to match the 
        /// expected hash.</exception>
        public async Task DownloadAsync(string destinationPath)
        {
            if (destinationPath is null)
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (destinationPath == string.Empty)
            {
                throw new ArgumentException(ReleasesResources.ValueCannotBeEmpty, nameof(destinationPath));
            }

            // If the destination file doesn't exist we can skip using an actual temporary file.
            string tempPath = !File.Exists(destinationPath) ? destinationPath : Path.GetTempFileName();
            await Utils.DownloadFileAsync(Address, tempPath).ConfigureAwait(false);

            // Most of the files are large since they represent full installations of .NET/.NET Core. They can
            // easily be 100MB+ so we won't verify the hash in memory.
            string actualHash = Utils.GetFileHash(tempPath, s_defaultHashAlgorithm);

            if (!string.Equals(Hash, actualHash, StringComparison.OrdinalIgnoreCase))
            {
                File.Delete(tempPath);
                throw new InvalidDataException(string.Format(ReleasesResources.HashMismatch, Hash, actualHash, destinationPath));
            }

            // Replace the destination file if the hash verified successfully and we used an actual temporary file.
            if (!string.Equals(destinationPath, tempPath))
            {
                File.Delete(destinationPath);
                File.Move(tempPath, destinationPath);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare to the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; <see langword="false"/> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals((ReleaseFile)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ReleaseFile"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="ReleaseFile"/> to compare to this instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="ReleaseFile"/> is equal to this instance; <see langword="false"/> otherwise.</returns>
        public bool Equals(ReleaseFile other)
        {
            return ReferenceEquals(this, other) ||
                Name == other.Name &&
                Rid == other.Rid &&
                Hash == other.Hash &&
                Address == other.Address;
        }

        /// <summary>
        /// Returns the hash code for this release file.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() =>
            Hash.GetHashCode() ^ Name.GetHashCode() ^ Rid.GetHashCode() ^ Address.GetHashCode();

        internal static ReleaseFile Create(string hash, string name, string rid, string address) =>
            new ReleaseFile(new Uri(address), hash, name, rid);
    }
}
