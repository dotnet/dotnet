// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#if IS_CORECLR
using System.Threading;
#else
using System.Runtime.Remoting.Messaging;
#endif

namespace NuGet.Common
{
    /// <summary>
    /// Ambient correlation ID used to associate information pertaining to a current activity. A single activity
    /// engages multiple method calls at different layers. Sometimes it's necessary to identify separate calls
    /// belonging to the same activity if shared state is needed.
    /// </summary>
    public static class ActivityCorrelationId
    {
#if IS_CORECLR
        private static readonly AsyncLocal<string?> _correlationId = new AsyncLocal<string?>();
#else
        private const string CorrelationIdSlot = "NuGet.Common.ActivityCorrelationId";
#endif

        private static readonly string DefaultCorrelationId = Guid.Empty.ToString();

        /// <summary>
        /// Returns current activity correlation ID or a default if not set previously.
        /// </summary>
        public static string Current
        {
            get
            {
#if IS_CORECLR
                var correlationId = _correlationId.Value;
#else
                var correlationId = CallContext.LogicalGetData(CorrelationIdSlot) as string;
#endif

                return correlationId ?? DefaultCorrelationId;
            }
        }

        /// <summary>
        /// Starts a new activity activity correlation ID by updating ambient context value.
        /// </summary>
        public static void StartNew()
        {
            var correlationId = Guid.NewGuid().ToString();

#if IS_CORECLR
            _correlationId.Value = correlationId;
#else
            CallContext.LogicalSetData(CorrelationIdSlot, correlationId);
#endif
        }

        public static void Clear()
        {
#if IS_CORECLR
            _correlationId.Value = null;
#else
            CallContext.FreeNamedDataSlot(CorrelationIdSlot);
#endif
        }
    }
}
