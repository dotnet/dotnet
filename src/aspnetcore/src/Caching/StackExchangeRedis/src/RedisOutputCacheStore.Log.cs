// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET7_0_OR_GREATER // IOutputCacheStore only exists from net7

using System;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Microsoft.Extensions.Caching.StackExchangeRedis;

internal partial class RedisOutputCacheStore
{
    [LoggerMessage(1, LogLevel.Warning, "Transient error occurred executing redis output-cache GC loop.", EventName = "RedisOutputCacheGCTransientError")]
    internal static partial void RedisOutputCacheGCTransientFault(ILogger logger, Exception exception);

    [LoggerMessage(2, LogLevel.Error, "Fatal error occurred executing redis output-cache GC loop.", EventName = "RedisOutputCacheGCFatalError")]
    internal static partial void RedisOutputCacheGCFatalError(ILogger logger, Exception exception);
}
#endif
