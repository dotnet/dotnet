// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelemetryExtensions.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FunctionalTests.Utils
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;

    using AI;

    public static class TelemetryExtensions
    {
        public static Envelope[] ReceiveItems(
            this TelemetryHttpListenerObservable listener,
            int timeOut)
        {
            if (null == listener)
            {
                throw new ArgumentNullException("listener");
            }

            var result = listener
                .TakeUntil(DateTimeOffset.UtcNow.AddMilliseconds(timeOut))
                .ToEnumerable()
                .ToArray();

            return result;
        }

        public static Envelope[] ReceiveItems(
            this TelemetryHttpListenerObservable listener,
            int count,
            int timeOut)
        {
            if (null == listener)
            {
                throw new ArgumentNullException("listener");
            }

            var result = listener
                .TakeUntil(DateTimeOffset.UtcNow.AddMilliseconds(timeOut))
                .Take(count)
                .ToEnumerable()
                .ToArray();

            if (result.Length != count)
            {
              throw new InvalidDataException("Incorrect number of items. Expected: " + count + " Received: " + result.Length);
            }

            return result;
        }

        public static Envelope[] ReceiveItemsOfType<T>(
            this TelemetryHttpListenerObservable listener,
            int timeOut)
        {
            var result = listener
                .Where(item => (item is T))
                .TakeUntil(DateTimeOffset.UtcNow.AddMilliseconds(timeOut))
                .ToEnumerable()
                .ToArray();
           
            return result;
        }

        public static Envelope[] ReceiveItemsOfType<T>(
            this TelemetryHttpListenerObservable listener,
            int count,
            int timeOut)
        {
            var result = listener
                .Where(item => (item is T))
                .TakeUntil(DateTimeOffset.UtcNow.AddMilliseconds(timeOut))
                .Take(count)
                .ToEnumerable()
                .ToArray();

            if (result.Length != count)
            {
                throw new InvalidDataException("Incorrect number of items. Expected: " + count + " Received: " + result.Length);
            }

            return result;
        }
    }
}
