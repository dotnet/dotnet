// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Events
{
    public sealed class ProtocolDiagnosticNupkgCopiedEvent
    {
        public string Source { get; }
        public long FileSize { get; }

        public ProtocolDiagnosticNupkgCopiedEvent(
            string source,
            long fileSize)
        {
            Source = source;
            FileSize = fileSize;
        }
    }
}
