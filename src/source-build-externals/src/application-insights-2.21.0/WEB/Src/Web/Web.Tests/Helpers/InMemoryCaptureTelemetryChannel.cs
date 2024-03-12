// <copyright file="InMemoryCaptureTelemetryChannel.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;

    internal class InMemoryCaptureTelemetryChannel : ITelemetryChannel
    {
        public InMemoryCaptureTelemetryChannel()
        {
            this.SentItems = new List<ITelemetry>();
        }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

        public List<ITelemetry> SentItems { get; private set; }

        public void Send(ITelemetry item)
        {
            this.SentItems.Add(item);
        }

        public void Dispose()
        {
        }

        public void Flush()
        {   
        }
    }
}
