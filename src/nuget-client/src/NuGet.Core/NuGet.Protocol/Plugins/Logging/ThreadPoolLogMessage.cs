// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal sealed class ThreadPoolLogMessage : PluginLogMessage
    {
        private readonly int _maxCompletionPortThreads;
        private readonly int _maxWorkerThreads;
        private readonly int _minCompletionPortThreads;
        private readonly int _minWorkerThreads;

        internal ThreadPoolLogMessage(DateTimeOffset now)
            : base(now)
        {
            ThreadPool.GetMinThreads(out _minWorkerThreads, out _minCompletionPortThreads);
            ThreadPool.GetMaxThreads(out _maxWorkerThreads, out _maxCompletionPortThreads);
        }

        public override string ToString()
        {
            var message = new JObject(
                new JProperty("worker thread minimum count", _minWorkerThreads),
                new JProperty("worker thread maximum count", _maxWorkerThreads),
                new JProperty("completion port thread minimum count", _minCompletionPortThreads),
                new JProperty("completion port thread maximum count", _maxCompletionPortThreads));

            return ToString("thread pool", message);
        }
    }
}
