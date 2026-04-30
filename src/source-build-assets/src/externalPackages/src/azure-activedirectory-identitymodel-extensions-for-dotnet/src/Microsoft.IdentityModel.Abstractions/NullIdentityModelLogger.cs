// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Abstractions
{
    /// <summary>
    /// A minimalistic <see cref="IIdentityLogger"/> implementation that is disabled by default and doesn't log.
    /// </summary>
    public sealed class NullIdentityModelLogger : IIdentityLogger
    {
        /// <summary>
        /// Default instance of <see cref="NullIdentityModelLogger"/>.
        /// </summary>
        public static NullIdentityModelLogger Instance { get; } = new NullIdentityModelLogger();

        private NullIdentityModelLogger() { }

        /// <inheritdoc/>
        public bool IsEnabled(EventLogLevel eventLogLevel) => false;

        /// <inheritdoc/>
        public void Log(LogEntry entry)
        {
            // no-op
        }
    }
}
