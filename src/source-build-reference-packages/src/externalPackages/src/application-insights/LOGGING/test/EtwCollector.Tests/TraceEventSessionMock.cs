//-----------------------------------------------------------------------
// <copyright file="TraceEventSessionMock.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System;
    using System.Collections.Generic;
    using Diagnostics.Tracing;
    using Diagnostics.Tracing.Session;
    using Microsoft.ApplicationInsights.EtwCollector;

    internal sealed class TraceEventSessionMock : ITraceEventSession
    {
        private bool isFakeAccessDenied;

        public List<string> EnabledProviderNames { get; private set; }

        public List<Guid> EnabledProviderGuids { get; private set; }


        public TraceEventSessionMock()
            : this(false)
        {
        }

        public TraceEventSessionMock(bool fakeAccessDeniedOnEnablingProvider)
        {
            this.EnabledProviderNames = new List<string>();
            this.EnabledProviderGuids = new List<Guid>();
            this.isFakeAccessDenied = fakeAccessDeniedOnEnablingProvider;
        }

        public TraceEventDispatcher Source { get; private set; }

        public void DisableProvider(Guid providerGuid)
        {
            if (this.isFakeAccessDenied)
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            EnabledProviderGuids.Remove(providerGuid);
        }

        public void DisableProvider(string providerName)
        {
            if (this.isFakeAccessDenied)
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            EnabledProviderNames.Remove(providerName);
        }

        public void Dispose()
        {
        }

        public bool EnableProvider(Guid providerGuid, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null)
        {
            if (this.isFakeAccessDenied)
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            this.EnabledProviderGuids.Add(providerGuid);
            return true;
        }

        public bool EnableProvider(string providerName, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null)
        {
            if (this.isFakeAccessDenied)
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            this.EnabledProviderNames.Add(providerName);
            return true;
        }

#pragma warning disable CA1801 // Review unused parameters
        public bool Stop(bool noThrow = false)
#pragma warning restore CA1801 // Review unused parameters
        {
            throw new NotImplementedException();
        }
    }
}
