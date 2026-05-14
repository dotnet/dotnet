// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Common
{
    public interface ICollectorLogger : ILogger
    {
        /// <summary>
        /// Fetch all of the errors logged so far. This method is useful when error log messages
        /// should be redisplayed after the initial log message is emitted.
        /// </summary>
        IEnumerable<IRestoreLogMessage> Errors { get; }
    }
}
