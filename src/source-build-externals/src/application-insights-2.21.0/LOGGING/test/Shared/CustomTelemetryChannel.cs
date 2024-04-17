//-----------------------------------------------------------------------------------
// <copyright file='CustomTelemetryChannel.cs' company='Microsoft Corporation'>
//     Copyright (c) Microsoft Corporation. All Rights Reserved.
//     Information Contained Herein is Proprietary and Confidential.
// </copyright>
//-----------------------------------------------------------------------------------

namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    internal sealed class CustomTelemetryChannel : ITelemetryChannel
    {
        private EventWaitHandle waitHandle;

        public CustomTelemetryChannel()
        {
            this.waitHandle = new AutoResetEvent(false);
#if NET452
            this.SentItems = new ITelemetry[0];
#else
            this.SentItems = Array.Empty<ITelemetry>();
#endif
        }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

        public ITelemetry[] SentItems { get; private set; }

        public void Send(ITelemetry item)
        {
            lock (this)
            {
                ITelemetry[] current = this.SentItems;
                List<ITelemetry> temp = new List<ITelemetry>(current);
                temp.Add(item);
                this.SentItems = temp.ToArray();
                this.waitHandle.Set();
            }
        }

        public Task<int?> WaitForItemsCaptured(TimeSpan timeout)
        {
            // Pattern for Wait Handles from: https://msdn.microsoft.com/en-us/library/hh873178%28v=vs.110%29.aspx#WaitHandles
            var tcs = new TaskCompletionSource<int?>();

            var rwh = ThreadPool.RegisterWaitForSingleObject(
                this.waitHandle, 
                (state, timedOut) =>
                {
                    if (timedOut)
                    {
                        tcs.SetResult(null);
                    }
                    else
                    {
                        lock (this)
                        {
                            tcs.SetResult(this.SentItems.Length);
                        }
                    }
                }, 
                state: null, 
                millisecondsTimeOutInterval: Convert.ToUInt32(timeout.TotalMilliseconds), 
                executeOnlyOnce: true);

            var t = tcs.Task;
            t.ContinueWith((previousTask) => rwh.Unregister(null));
            return t;
        }

        public void Flush()
        {
            throw new Exception("Flush called");
        }

        public void Dispose()
        {
        }

        public CustomTelemetryChannel Reset()
        {
            lock (this)
            {
#if NET452
                this.SentItems = new ITelemetry[0];
#else
                this.SentItems = Array.Empty<ITelemetry>();
#endif
            }

            return this;
        }
    }
}
