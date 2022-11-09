// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using NuGet.Common;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Ensures a given telemetry event is emitted once.
    /// </summary>
    internal class TelemetryOnceEmitter
    {
        private int _emittedFlag = 0;

        internal TelemetryOnceEmitter(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(eventName));
            }

            EventName = eventName;
        }

        internal string EventName { get; }

        /// <summary>
        /// Emits telemetry event once
        /// </summary>
        public void EmitIfNeeded()
        {
            if (Interlocked.CompareExchange(ref _emittedFlag, 1, 0) == 0)
            {
                TelemetryActivity.EmitTelemetryEvent(new TelemetryEvent(EventName));
            }
        }

        /// <summary>
        /// For testing purposes only
        /// </summary>
        internal void Reset() => Interlocked.Exchange(ref _emittedFlag, 0);
    }
}
