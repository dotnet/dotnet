// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public static class RemoteErrorUtility
    {
        public static RemoteError ToRemoteError(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            ILogMessage logMessage;
            IReadOnlyList<ILogMessage>? logMessages = null;
            string typeName = exception.GetType().FullName;

            if (exception is SignatureException signatureException)
            {
                logMessage = signatureException.AsLogMessage();
                IReadOnlyList<PackageVerificationResult> results = signatureException.Results ?? Array.Empty<PackageVerificationResult>();
                logMessages = results.SelectMany(result => result.Issues).ToArray();

                return new RemoteError(typeName, logMessage, logMessages);
            }

            logMessage = new LogMessage(LogLevel.Error, ExceptionUtilities.DisplayMessage(exception, indent: false));
            string? projectContextLogMessage = null;
            string? activityLogMessage = null;

            if (exception is NuGetResolverConstraintException
                || exception is PackageAlreadyInstalledException
                || exception is MinClientVersionException
                || exception is FrameworkException
                || exception is NuGetProtocolException
                || exception is PackagingException
                || exception is InvalidOperationException
                || exception is PackageReferenceRollbackException)
            {
                projectContextLogMessage = ExceptionUtilities.DisplayMessage(exception, indent: true);
                activityLogMessage = exception.ToString();

                if (exception is PackageReferenceRollbackException rollbackException)
                {
                    logMessages = rollbackException.LogMessages
                        .Where(message => message.Level == LogLevel.Error || message.Level == LogLevel.Warning)
                        .ToArray();
                }
            }
            else
            {
                projectContextLogMessage = exception.ToString();
            }

            return new RemoteError(typeName, logMessage, logMessages, projectContextLogMessage, activityLogMessage);
        }
    }
}
