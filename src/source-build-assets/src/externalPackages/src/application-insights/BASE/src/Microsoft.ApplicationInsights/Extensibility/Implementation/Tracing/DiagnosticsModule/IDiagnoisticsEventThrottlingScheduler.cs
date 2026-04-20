// -----------------------------------------------------------------------
// <copyright file="IDiagnoisticsEventThrottlingScheduler.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
// <author>Sergei Nikitin: sergeyni@microsoft.com</author>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;

    internal interface IDiagnoisticsEventThrottlingScheduler
    {
        object ScheduleToRunEveryTimeIntervalInMilliseconds(
            int interval, 
            Action actionToExecute);

        void RemoveScheduledRoutine(object token);
    }
}