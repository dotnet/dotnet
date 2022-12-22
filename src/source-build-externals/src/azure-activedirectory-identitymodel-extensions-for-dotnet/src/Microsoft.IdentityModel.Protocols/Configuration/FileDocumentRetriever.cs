//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
