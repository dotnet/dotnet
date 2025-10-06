//-----------------------------------------------------------------------
// <copyright file="AITraceEventSession.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwCollector
{
    using System;
    using Microsoft.Diagnostics.Tracing;
    using Microsoft.Diagnostics.Tracing.Session;

    /// <summary>
    /// A wrapper of <see cref="Microsoft.Diagnostics.Tracing.Session.TraceEventSession"/>.
    /// </summary>
    internal sealed class AITraceEventSession : ITraceEventSession, IDisposable
    {
        private TraceEventSession session;

        public AITraceEventSession(TraceEventSession traceEventSession)
        {
            if (traceEventSession == null)
            {
                throw new ArgumentNullException(nameof(traceEventSession));
            }

            this.session = traceEventSession;
        }

        public TraceEventDispatcher Source
        {
            get
            {
                return this.session.Source;
            }
        }

        public static bool? IsElevated()
        {
            return TraceEventSession.IsElevated();
        }

        public void DisableProvider(Guid providerGuid)
        {
            this.session.DisableProvider(providerGuid);
        }

        public void DisableProvider(string providerName)
        {
            this.session.DisableProvider(providerName);
        }

        public void Dispose()
        {
            this.session.Dispose();
        }

        public bool EnableProvider(Guid providerGuid, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null)
        {
            return this.session.EnableProvider(providerGuid, providerLevel, matchAnyKeywords, options);
        }

        public bool EnableProvider(string providerName, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null)
        {
            return this.session.EnableProvider(providerName, providerLevel, matchAnyKeywords, options);
        }

        public bool Stop(bool noThrow = false)
        {
            return this.session.Stop(noThrow);
        }
    }
}
