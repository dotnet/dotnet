// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.SignCheck.Verification;

namespace Microsoft.SignCheck.Logging
{
    public class Log : ILogger
    {
        private List<ILogger> _loggers;

        public bool HasLoggedErrors
        {
            get
            {
                return _loggers.Any(l => l.HasLoggedErrors);
            }
        }

        public LogVerbosity Verbosity
        {
            get;
            private set;
        }

        public Log(string logFile, string errorFile, string resultsFile, LogVerbosity verbosity)
        {
            _loggers = new List<ILogger>();
            Add(new FileLogger(verbosity, logFile, errorFile, resultsFile));
            Add(new ConsoleLogger(verbosity));
            Verbosity = verbosity;
        }

        public void Add(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public void WriteMessage(string message)
        {
            _loggers.ForEach(p => p.WriteMessage(message));
        }

        public void WriteMessage(LogVerbosity verbosity, string message)
        {
            _loggers.ForEach(p => p.WriteMessage(verbosity, message));
        }

        public void WriteMessage(LogVerbosity verbosity, string message, params object[] values)
        {
            _loggers.ForEach(p => p.WriteMessage(verbosity, message, values));
        }

        public void WriteError(string message)
        {
            _loggers.ForEach(p => p.WriteError(message));
        }

        public void WriteError(LogVerbosity verbosity, string message)
        {
            _loggers.ForEach(p => p.WriteError(verbosity, message));
        }

        public void WriteError(LogVerbosity verbosity, string message, params object[] values)
        {
            _loggers.ForEach(p => p.WriteError(verbosity, message, values));
        }

        public void WriteLine()
        {
            _loggers.ForEach(p => p.WriteLine());
        }

        public void WriteStartResult(SignatureVerificationResult result, string outcome)
        {
            _loggers.OfType<FileLogger>().FirstOrDefault().WriteStartResult(result, outcome);
        }

        public void WriteEndResult()
        {
            _loggers.OfType<FileLogger>().FirstOrDefault().WriteEndResult();
        }

        public void Close()
        {
            _loggers.ForEach(p => p.Close());
        }
    }
}
