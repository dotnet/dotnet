// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Shared;

namespace NuGet.Commands
{
    public class RestoreCollectorLogger : LoggerBase, ICollectorLogger
    {
        private readonly ILogger _innerLogger;
        private readonly ConcurrentQueue<IRestoreLogMessage> _errors;
        private readonly bool _hideWarningsAndErrors;
        private IEnumerable<RestoreTargetGraph> _restoreTargetGraphs;
        private PackageSpec _projectSpec;
        private WarningPropertiesCollection _transitiveWarningPropertiesCollection;

        public string ProjectPath => _projectSpec?.RestoreMetadata?.ProjectPath;

        public IEnumerable<IRestoreLogMessage> Errors => _errors.ToArray();

        public WarningPropertiesCollection ProjectWarningPropertiesCollection { get; set; }

        public WarningPropertiesCollection TransitiveWarningPropertiesCollection
        {
            get
            {
                if (_transitiveWarningPropertiesCollection == null)
                {
                    // Populate TransitiveWarningPropertiesCollection only if it is null and we have RestoreTargetGraphs.
                    // This will happen at most once and only if we have the project spec with restore metadata.
                    if (_restoreTargetGraphs != null &&
                        _restoreTargetGraphs.Any() &&
                        _projectSpec != null &&
                        _projectSpec.RestoreMetadata != null)
                    {
                        TransitiveWarningPropertiesCollection = TransitiveNoWarnUtils.CreateTransitiveWarningPropertiesCollection(
                            _restoreTargetGraphs,
                            _projectSpec);
                    }
                }

                return _transitiveWarningPropertiesCollection;
            }

            set => _transitiveWarningPropertiesCollection = value;
        }

        /// <summary>
        /// Stores a reference to PackageSpec for the project from the restore request.
        /// This are used to generate the warning properties for the project.
        /// </summary>
        /// <param name="projectSpec">PackageSpec to be stored for reference.</param>
        public void ApplyRestoreInputs(PackageSpec projectSpec)
        {
            _projectSpec = projectSpec;

            ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                projectSpec.RestoreMetadata?.ProjectWideWarningProperties,
                PackageSpecificWarningProperties.CreatePackageSpecificWarningProperties(projectSpec),
                projectSpec.TargetFrameworks.Select(f => f.FrameworkName).AsList().AsReadOnly()
                );
        }

        /// <summary>
        /// Stores a reference to RestoreTargetGraphs from the restore output.
        /// These graphs are used to generate the transitive warning properties.
        /// </summary>
        /// <param name="restoreTargetGraphs">RestoreTargetGraphs to be stored for reference.</param>
        public void ApplyRestoreOutput(IEnumerable<RestoreTargetGraph> restoreTargetGraphs)
        {
            _restoreTargetGraphs = restoreTargetGraphs;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RestoreCollectorLogger"/>, while still
        /// delegating all log messages to the inner logger.
        /// </summary>
        /// <param name="innerLogger">The inner logger used to delegate the logging.</param>
        /// <param name="verbosity">Minimum verbosity below which no logs will be passed to the inner logger.</param>
        /// <param name="hideWarningsAndErrors">If this is true, then errors and warnings will not be passed to inner logger.</param>
        public RestoreCollectorLogger(ILogger innerLogger, LogLevel verbosity, bool hideWarningsAndErrors)
            : base(verbosity)
        {
            _innerLogger = innerLogger;
            _errors = new ConcurrentQueue<IRestoreLogMessage>();
            _hideWarningsAndErrors = hideWarningsAndErrors;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RestoreCollectorLogger"/>, while still
        /// delegating all log messages to the inner logger.
        /// </summary>
        /// <param name="innerLogger">The inner logger used to delegate the logging.</param>
        /// <param name="hideWarningsAndErrors">If this is false, then errors and warnings will not be passed to inner logger.</param>
        public RestoreCollectorLogger(ILogger innerLogger, bool hideWarningsAndErrors)
            : this(innerLogger, LogLevel.Debug, hideWarningsAndErrors)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RestoreCollectorLogger"/>, while still
        /// delegating all log messages to the inner logger.
        /// </summary>
        /// <param name="innerLogger">The inner logger used to delegate the logging.</param>
        /// <param name="verbosity">Minimum verbosity below which no logs will be passed to the inner logger.</param>
        public RestoreCollectorLogger(ILogger innerLogger, LogLevel verbosity)
            : this(innerLogger, verbosity, hideWarningsAndErrors: false)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RestoreCollectorLogger"/>, while still
        /// delegating all log messages to the inner logger.
        /// </summary>
        /// <param name="innerLogger">The inner logger used to delegate the logging.</param>
        public RestoreCollectorLogger(ILogger innerLogger)
            : this(innerLogger, LogLevel.Debug, hideWarningsAndErrors: false)
        {
        }

        public void Log(IRestoreLogMessage message)
        {
            // check if the message is a warning and it is suppressed 
            if (!IsWarningSuppressed(message))
            {
                // if the message is not suppressed then check if it needs to be upgraded to an error
                UpgradeWarningToErrorIfNeeded(message);

                if (string.IsNullOrEmpty(message.FilePath))
                {
                    message.FilePath = message.ProjectPath ?? ProjectPath;
                }

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

        public Task LogAsync(IRestoreLogMessage message)
        {
            // check if the message is a warning and it is suppressed 
            if (!IsWarningSuppressed(message))
            {
                // if the message is not suppressed then check if it needs to be upgraded to an error
                UpgradeWarningToErrorIfNeeded(message);

                if (string.IsNullOrEmpty(message.FilePath))
                {
                    message.FilePath = message.ProjectPath ?? ProjectPath;
                }

                if (CollectMessage(message.Level))
                {
                    _errors.Enqueue(message);
                }

                if (DisplayMessage(message))
                {
                    return _innerLogger.LogAsync(message);
                }
            }

            return Task.CompletedTask;
        }

        public override void Log(ILogMessage message)
        {
            Log(ToRestoreLogMessage(message));
        }

        public override Task LogAsync(ILogMessage message)
        {
            return LogAsync(ToRestoreLogMessage(message));
        }

        /// <summary>
        /// Decides if the log should be passed to the inner logger.
        /// </summary>
        /// <param name="message">IRestoreLogMessage to be logged.</param>
        /// <returns>bool indicating if this message should be logged.</returns>
        protected bool DisplayMessage(IRestoreLogMessage message)
        {
            if (message.Level == LogLevel.Error || message.Level == LogLevel.Warning)
            {
                return ((!_hideWarningsAndErrors || message.ShouldDisplay) && message.Level >= VerbosityLevel);
            }
            else
            {
                return (message.Level >= VerbosityLevel);
            }
        }

        /// <summary>
        /// This method checks if at least one of the warning properties collections is not null and it suppresses the warning.
        /// </summary>
        /// <param name="message">IRestoreLogMessage to be logged.</param>
        /// <returns>bool indicating if the message should be suppressed.</returns>
        private bool IsWarningSuppressed(IRestoreLogMessage message)
        {
            if (message.Level == LogLevel.Warning)
            {
                // If the ProjectWarningPropertiesCollection is present then test if the warning is suppressed in
                // project wide no warn or package specific no warn
                if (ProjectWarningPropertiesCollection?.ApplyNoWarnProperties(message) == true)
                {
                    return true;
                }
                else
                {
                    // Use transitive warning properties only if the project does not suppress the warning
                    // In transitive warning properties look at only the package specific ones as all properties are per package reference.
                    return TransitiveWarningPropertiesCollection?.ApplyNoWarnProperties(message) == true;
                }
            }

            return false;
        }

        /// <summary>
        /// This method upgrades the warning to an error if the project wide warning properties have set the code in WarningsAsErrors or
        /// set TreatWarningsAsErrors to true
        /// </summary>
        /// <param name="message">IRestoreLogMessage to be logged as an error or warning.</param>
        /// <returns>bool indicating if the message should be suppressed.</returns>
        private void UpgradeWarningToErrorIfNeeded(IRestoreLogMessage message)
        {
            ProjectWarningPropertiesCollection?.ApplyWarningAsErrorProperties(message);
        }

        private static IRestoreLogMessage ToRestoreLogMessage(ILogMessage message)
        {
            if (message is IRestoreLogMessage restoreLogMessage)
            {
                return restoreLogMessage;
            }

            if (message is SignatureLog signatureLog)
            {
                return signatureLog.AsRestoreLogMessage();
            }

            return new RestoreLogMessage(message.Level, message.Code, message.Message)
            {
                Time = message.Time,
                WarningLevel = message.WarningLevel,
                ProjectPath = message.ProjectPath
            };
        }
    }
}
