// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    public class PackCollectorLogger : LoggerBase
    {
        private readonly ConcurrentQueue<ILogMessage> _errors;
        private ILogger _innerLogger;

        // Project level warnings properties
        public WarningProperties WarningProperties { get; set; }

        // Package specific warning property
        private PackCommand.PackageSpecificWarningProperties PackageSpecificWarningProperties { get; set; }

        public IEnumerable<ILogMessage> Errors => _errors.ToArray();

        public PackCollectorLogger(ILogger innerLogger, WarningProperties warningProperties)
            : this(innerLogger, warningProperties, null) { }

        public PackCollectorLogger(ILogger innerLogger, WarningProperties warningProperties, PackCommand.PackageSpecificWarningProperties packageSpecificWarningProperties)
        {
            _innerLogger = innerLogger;
            WarningProperties = warningProperties;
            PackageSpecificWarningProperties = packageSpecificWarningProperties;
            _errors = new ConcurrentQueue<ILogMessage>();
        }

        public override void Log(ILogMessage message)
        {
            // check if the message is a warning and it is suppressed 
            if (!IsWarningSuppressed(message))
            {
                // if the message is not suppressed then check if it needs to be upgraded to an error
                UpgradeWarningToErrorIfNeeded(message);

                if (CollectMessage(message.Level))
                {
                    _errors.Enqueue(message);
                }

                if (DisplayMessage(message))
                {
                    _innerLogger.Log(message);
                }
            }
        }

        public override Task LogAsync(ILogMessage message)
        {
            // check if the message is a warning and it is suppressed 
            if (!IsWarningSuppressed(message))
            {
                // if the message is not suppressed then check if it needs to be upgraded to an error
                UpgradeWarningToErrorIfNeeded(message);

                if (CollectMessage(message.Level))
                {
                    _errors.Enqueue(message);
                }

                if (DisplayMessage(message))
                {
                    return _innerLogger.LogAsync(message);
                }
            }

            return Task.FromResult(0);
        }

        private bool IsWarningSuppressed(ILogMessage message)
        {
            if (message.Level == LogLevel.Warning)
            {
                // If the WarningPropertiesCollection is present then test if the warning is suppressed in
                // project wide no warn
                if (WarningPropertiesCollection.ApplyProjectWideNoWarnProperties(message, warningProperties: WarningProperties))
                {
                    return true;
                }
                else
                {
                    // Use packagereference warning properties only if the project does not suppress the warning
                    // In packagereference warning properties look at only the package specific ones as all properties are per package reference.
                    IPackLogMessage packLogMessage = message as IPackLogMessage;

                    if (packLogMessage != null)
                    {
                        return PackageSpecificWarningProperties?.ApplyNoWarnProperties(packLogMessage) == true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This method upgrades the warning to an error if the project wide warning properties have set the code in WarningsAsErrors or
        /// set TreatWarningsAsErrors to true
        /// </summary>
        /// <param name="message">ILogMessage to be logged as an error or warning.</param>
        /// <returns>bool indicating if the message should be suppressed.</returns>
        private void UpgradeWarningToErrorIfNeeded(ILogMessage message)
        {
            WarningPropertiesCollection.ApplyProjectWideWarningsAsErrorProperties(message, WarningProperties);
        }

        /// <summary>
        /// Decides if the log should be passed to the inner logger.
        /// </summary>
        /// <param name="message">ILogMessage to be logged.</param>
        /// <returns>bool indicating if this message should be logged.</returns>
        private bool DisplayMessage(ILogMessage message)
        {
            return (message.Level >= VerbosityLevel);
        }
    }
}
