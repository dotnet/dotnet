// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Events
{
    public sealed class ProtocolDiagnosticHttpEvent : ProtocolDiagnosticHttpEventBase
    {
        public DateTime Timestamp { get; }
        public TimeSpan EventDuration { get; }
        public long Bytes { get; }
        public bool IsSuccess { get; }

        internal ProtocolDiagnosticHttpEvent(
            DateTime timestamp,
            string source,
            Uri url,
            TimeSpan? headerDuration,
            TimeSpan eventDuration,
            long bytes,
            int? httpStatusCode,
            bool isSuccess,
            bool isRetry,
            bool isCancelled,
            bool isLastAttempt)
            : this(
                  timestamp,
                  eventDuration,
                  bytes,
                  isSuccess,
                  new ProtocolDiagnosticInProgressHttpEvent(
                      source,
                      url,
                      headerDuration,
                       httpStatusCode,
                       isRetry,
                       isCancelled,
                       isLastAttempt))
        {
        }

        internal ProtocolDiagnosticHttpEvent(
            DateTime timestamp,
            TimeSpan eventDuration,
            long bytes,
            bool isSuccess,
            ProtocolDiagnosticHttpEventBase eventBase)
            : base(eventBase)
        {
            Timestamp = timestamp;
            EventDuration = eventDuration;
            Bytes = bytes;
            IsSuccess = isSuccess;
        }
    }
}
