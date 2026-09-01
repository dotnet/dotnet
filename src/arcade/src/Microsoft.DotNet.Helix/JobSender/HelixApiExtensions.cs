// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Client
{
    public static class HelixApiExtensions
    {
        public static IJobDefinitionWithType Define(this IJob jobApi)
        {
            return new JobDefinition(jobApi);
        }

        public static IJobDefinition WithQueueStats(this IJobDefinition jobDefinition)
        {
            return jobDefinition is JobDefinition implementation
                ? implementation.WithQueueStats()
                : jobDefinition;
        }

        public static Task<ISentJob> SendAsync(
            this IJobDefinition jobDefinition,
            Action<string> log,
            Action<string> queueStatsLog,
            CancellationToken cancellationToken = default)
        {
            return jobDefinition is JobDefinition implementation
                ? implementation.SendAsync(log, queueStatsLog, cancellationToken)
                : jobDefinition.SendAsync(log, cancellationToken);
        }
    }
}
