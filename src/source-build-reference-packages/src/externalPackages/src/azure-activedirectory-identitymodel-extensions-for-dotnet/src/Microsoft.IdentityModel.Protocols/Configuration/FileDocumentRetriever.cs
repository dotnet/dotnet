// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Reads a local file from the disk.
    /// </summary>
    public class FileDocumentRetriever : IDocumentRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDocumentRetriever"/> class.
        /// </summary>
        public FileDocumentRetriever()
        { }

        /// <summary>
        /// Reads a document using <see cref="FileStream"/>.
        /// </summary>
        /// <param name="address">Fully qualified path to a file.</param>
        /// <param name="cancel"><see cref="CancellationToken"/> not used.</param>
        /// <returns>UTF8 decoding of bytes in the file.</returns>
        /// <exception cref="ArgumentNullException">If address is null or whitespace.</exception>
        /// <exception cref="IOException">with inner expection containing the original exception.</exception>
        public async Task<string> GetDocumentAsync(string address, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw LogHelper.LogArgumentNullException("address");

            try
            {
                using (var reader = File.OpenText(address))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new IOException(LogHelper.FormatInvariant(LogMessages.IDX20804, address), ex));
            }
        }
    }
}
