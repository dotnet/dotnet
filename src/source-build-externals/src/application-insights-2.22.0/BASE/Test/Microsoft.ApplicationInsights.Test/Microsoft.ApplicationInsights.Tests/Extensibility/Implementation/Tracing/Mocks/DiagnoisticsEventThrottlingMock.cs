// -----------------------------------------------------------------------
// <copyright file="DiagnoisticsEventThrottlingMock.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
// <author>Sergei Nikitin: sergeyni@microsoft.com</author>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;

    internal class DiagnoisticsEventThrottlingMock : IDiagnoisticsEventThrottling
    {
        private readonly bool throttleAll;
        private readonly bool signalJustExceeded;
        private readonly IDictionary<int, DiagnoisticsEventCounters> sampleCounters;

        public DiagnoisticsEventThrottlingMock(
            bool throttleAll, 
            bool signalJustExceeded,
            IDictionary<int, DiagnoisticsEventCounters> sampleCounters)
        {
            this.throttleAll = throttleAll;
            this.signalJustExceeded = signalJustExceeded;
            this.sampleCounters = sampleCounters;
        }

        public bool ThrottleEvent(int eventId, long keywords, out bool justExceededThreshold)
        {
            justExceededThreshold = this.signalJustExceeded;

            this.sampleCounters.Add(eventId, new DiagnoisticsEventCounters(1));

            return this.throttleAll;
        }

        public IDictionary<int, DiagnoisticsEventCounters> CollectSnapshot()
        {
            return this.sampleCounters;
        }
    }
}
