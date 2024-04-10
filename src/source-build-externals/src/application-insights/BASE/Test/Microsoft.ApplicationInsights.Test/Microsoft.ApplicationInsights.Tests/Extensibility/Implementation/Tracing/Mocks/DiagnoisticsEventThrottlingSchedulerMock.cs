// -----------------------------------------------------------------------
// <copyright file="DiagnoisticsEventThrottlingSchedulerMock.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
// <author>Sergei Nikitin: sergeyni@microsoft.com</author>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;

    internal class DiagnoisticsEventThrottlingSchedulerMock : IDiagnoisticsEventThrottlingScheduler
    {
        private readonly IList<ScheduleItem> items = new List<ScheduleItem>();

        public IList<ScheduleItem> Items
        {
            get { return this.items; }
        }

        public object ScheduleToRunEveryTimeIntervalInMilliseconds(
            int interval, 
            Action actionToExecute)
        {
            var item = new ScheduleItem
            {
                Action = actionToExecute,
                Interval = interval
            };

            this.Items.Add(item);

            return item;
        }

        public void RemoveScheduledRoutine(object token)
        {
            this.Items.Remove((ScheduleItem)token);
        }

        internal class ScheduleItem
        {
            internal Action Action { get; set; }

            internal int Interval { get; set; }
        }
    }
}
