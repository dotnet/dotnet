// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    /// <summary>
    /// Allows converting an Exception to an ILogMessage.
    /// </summary>
    public interface ILogMessageException
    {
        /// <summary>
        /// Retrieve the exception as a log message.
        /// </summary>
        ILogMessage AsLogMessage();
    }
}
