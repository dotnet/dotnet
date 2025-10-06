// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Events
{
    /// <summary>
    /// Represents a diagnostic event for tracking protocol service index entries, specifically identifying if an HTTPS source contains HTTP resources.
    /// </summary>
    public sealed class ProtocolDiagnosticServiceIndexEntryEvent
    {
        /// <summary>
        /// Gets the source URL of the service index entry.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets a value indicating whether an HTTPS source has an HTTP resource.
        /// </summary>
        public bool HttpsSourceHasHttpResource { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolDiagnosticServiceIndexEntryEvent"/> class.
        /// </summary>
        /// <param name="source">The source URL of the service index entry.</param>
        /// <param name="httpsSourceHasHttpResource">Indicates if the HTTPS source has an HTTP resource.</param>
        public ProtocolDiagnosticServiceIndexEntryEvent(string source, bool httpsSourceHasHttpResource)
        {
            Source = source;
            HttpsSourceHasHttpResource = httpsSourceHasHttpResource;
        }
    }
}
