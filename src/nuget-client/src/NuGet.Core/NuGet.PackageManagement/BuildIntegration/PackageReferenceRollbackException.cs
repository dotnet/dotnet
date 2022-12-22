// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// An exception containing a generic rollback message for the user
    /// and additional log messages with specific information on
    /// what caused the rollback.
    /// </summary>
    public class PackageReferenceRollbackException : InvalidOperationException
    {
        /// <summary>
        /// Additional log messages for the error list.
        /// </summary>
        public IReadOnlyList<ILogMessage> LogMessages { get; }

        /// <summary>
        /// Create a PackageReferenceRollbackException
        /// </summary>
        /// <param name="message">High level exception message.</param>
        /// <param name="logMessages">Log messages to be shown in the error list.</param>
        public PackageReferenceRollbackException(string message, IEnumerable<ILogMessage> logMessages)
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (logMessages == null)
            {
                throw new ArgumentNullException(nameof(logMessages));
            }

            LogMessages = logMessages.ToList();
        }
    }
}
