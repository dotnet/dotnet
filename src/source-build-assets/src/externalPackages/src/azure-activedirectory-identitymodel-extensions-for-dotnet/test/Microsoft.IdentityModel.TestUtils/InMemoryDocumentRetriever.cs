// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Returns a string set in the constructor.
    /// Simplifies testing.
    /// </summary>
    public class InMemoryDocumentRetriever : IDocumentRetriever
    {
        private readonly Dictionary<string, string> _configurations;
        private ManualResetEvent _waitEvent;
        private ManualResetEvent _signalEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDocumentRetriever"/> class.
        /// </summary>
        public InMemoryDocumentRetriever(Dictionary<string, string> configuration)
        {
            _configurations = configuration;
        }

        public InMemoryDocumentRetriever(Dictionary<string, string> configuration, ManualResetEvent waitEvent, ManualResetEvent signalEvent)
        {
            _configurations = configuration;
            _waitEvent = waitEvent;
            _signalEvent = signalEvent;
        }

        /// <summary>
        /// Returns the document passed in constructor in dictionary./>
        /// </summary>
        /// <param name="address">Fully qualified path to a file. Ignored for now.</param>
        /// <param name="cancel"><see cref="CancellationToken"/> Ignored for now.</param>
        /// <returns>UTF8 decoding of bytes in the file.</returns>
        public async Task<string> GetDocumentAsync(string address, CancellationToken cancel)
        {
            // Some tests change the Metadata address on ConfigurationManger to test different scenarios.
            // This event is used to let the test know that the GetDocumentAsync method has been called, and the test can now change the Metadata address.
            if (_signalEvent != null)
                _signalEvent.Set();

            // This event lets the caller control when metadata can be returned.
            // Useful when testing delays.
            if (_waitEvent != null)
                _waitEvent.WaitOne();

            return await Task.FromResult(_configurations[address]).ConfigureAwait(false);
        }
    }
}
