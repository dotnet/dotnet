// <copyright file="AdapterHelper.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Tracing.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    public class AdapterHelper : IDisposable
    {
        public string InstrumentationKey { get; }

#if NET452 || NET46
        private static readonly string ApplicationInsightsConfigFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config");
#else
        private static readonly string ApplicationInsightsConfigFilePath =
            Path.Combine(Path.GetDirectoryName(typeof(AdapterHelper).GetTypeInfo().Assembly.Location), "ApplicationInsights.config");
#endif

        public AdapterHelper(string instrumentationKey = "F8474271-D231-45B6-8DD4-D344C309AE69")
        {
            this.InstrumentationKey = instrumentationKey;

            string configuration = string.Format(InvariantCulture,
                                    @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                     <ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                                        <InstrumentationKey>{0}</InstrumentationKey>
                                     </ApplicationInsights>",
                                     instrumentationKey);

            File.WriteAllText(ApplicationInsightsConfigFilePath, configuration);
            this.Channel = new CustomTelemetryChannel();
        }

        internal CustomTelemetryChannel Channel { get; private set; }

        public static void ValidateChannel(AdapterHelper adapterHelper, string instrumentationKey, int expectedTraceCount)
        {
            // Validate that the channel received traces
            ITelemetry[] sentItems = null;
            int totalMillisecondsToWait = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            const int IterationMilliseconds = 250;

            while (totalMillisecondsToWait > 0)
            {
                sentItems = adapterHelper.Channel.SentItems;
                if (sentItems.Length > 0)
                {
                    ITelemetry telemetry = sentItems.FirstOrDefault();

                    Assert.AreEqual(expectedTraceCount, sentItems.Length, "All messages are received by the channel");
                    Assert.IsNotNull(telemetry, "telemetry collection is not null");
                    Assert.AreEqual(instrumentationKey, telemetry.Context.InstrumentationKey, "The correct instrumentation key was used");
                    break;
                }

                Thread.Sleep(IterationMilliseconds);
                totalMillisecondsToWait -= IterationMilliseconds;
            }

            Assert.IsNotNull(sentItems);
            Assert.IsTrue(sentItems.Length > 0);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Channel.Dispose();

                if (File.Exists(ApplicationInsightsConfigFilePath))
                {
                    File.Delete(ApplicationInsightsConfigFilePath);
                }
            }
        }
    }
}
