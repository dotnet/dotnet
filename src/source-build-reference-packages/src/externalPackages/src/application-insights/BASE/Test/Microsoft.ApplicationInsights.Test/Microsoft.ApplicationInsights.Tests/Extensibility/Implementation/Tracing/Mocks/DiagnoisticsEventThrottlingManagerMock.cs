// -----------------------------------------------------------------------
// <copyright file="DiagnoisticsEventThrottlingManagerMock.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
// <author>Sergei Nikitin: sergeyni@microsoft.com</author>
// <summary></summary>
// -----------------------------------------------------------------------

using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    internal class DiagnoisticsEventThrottlingManagerMock : IDiagnoisticsEventThrottlingManager
    {
        private readonly bool throttleAll;

        public DiagnoisticsEventThrottlingManagerMock(bool throttleAll)
        {
            this.throttleAll = throttleAll;
        }

        public bool ThrottleEvent(int eventId, long keywords)
        {
            return this.throttleAll;
        }
    }
}
