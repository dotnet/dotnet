// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.SignTool
{
    internal class Telemetry
    {
        private static readonly string s_sendEventName = "SignTool task completed";
        private Dictionary<string, double> _metrics;
        private static readonly Dictionary<string, string> s_properties = new Dictionary<string, string>()
        {
            { "BUILD_REPOSITORY_URI", Environment.GetEnvironmentVariable("BUILD_REPOSITORY_URI") },
            { "BUILD_SOURCEBRANCH", Environment.GetEnvironmentVariable("BUILD_SOURCEBRANCH") },
            { "BUILD_BUILDNUMBER", Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER") },
            { "BUILD_SOURCEVERSION", Environment.GetEnvironmentVariable("BUILD_SOURCEVERSION") }
        };

        private readonly bool _disableTelemetry ;

        public Telemetry()
        {
            _metrics = new Dictionary<string, double>();
            _disableTelemetry =
                (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SIGNTOOL_DISABLE_TELEMETRY")));
        }

        internal void AddMetric(string name, double value)
        {
            if (!_disableTelemetry)
            {
                _metrics.Add(name, value);
            }
        }

        internal void SendEvents()
        {
            if (!_disableTelemetry)
            {
                // set APPINSIGHTS_INSTRUMENTATIONKEY environment variable to begin tracking telemetry
                TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
                TelemetryClient telemetryClient = new TelemetryClient(configuration);

                telemetryClient.TrackEvent(s_sendEventName, properties: s_properties, metrics: _metrics);
                telemetryClient.Flush();
                _metrics = null;
            }
        }
    }
}
