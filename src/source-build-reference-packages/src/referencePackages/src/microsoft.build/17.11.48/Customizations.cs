// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Manual additions to address missing interface members
// The generator doesn't properly emit all IBuildEventArgsReaderNotifications members
namespace Microsoft.Build.Logging
{
    public sealed partial class BinaryLogReplayEventSource
    {
        public event System.Action<ArchiveFileEventArgs>? ArchiveFileEncountered
        {
            add { }
            remove { }
        }

        public event System.Action<StringReadEventArgs>? StringReadDone
        {
            add { }
            remove { }
        }
    }
}
