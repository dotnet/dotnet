// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.SignCheck.Verification;

namespace Microsoft.SignCheck.Logging
{
    public class ConsoleLogger : LoggerBase, ILogger
    {
        public ConsoleLogger(LogVerbosity verbosity) : base(verbosity)
        {

        }

        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteMessage(LogVerbosity verbosity, string message)
        {
            if (verbosity <= Verbosity)
            {
                WriteMessage(message);
            }
        }

        public void WriteMessage(LogVerbosity verbosity, string message, params object[] values)
        {
            if (verbosity <= Verbosity)
            {
                WriteMessage(Verbosity, String.Format(message, values));
            }
        }

        public void WriteError(string message)
        {
            HasLoggedErrors = true;
            var fgColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = fgColor;
        }

        public void WriteError(LogVerbosity verbosity, string message)
        {
            if (verbosity <= Verbosity)
            {
                WriteError(message);
            }
        }

        public void WriteError(LogVerbosity verbosity, string message, params object[] values)
        {
            if (verbosity <= Verbosity)
            {
                WriteError(Verbosity, String.Format(message, values));
            }
        }

        public void WriteStartResult(SignatureVerificationResult result, string outcome)
        {
            throw new NotImplementedException("ConsoleLogger does not support WriteStartResult.");
        }

        public void WriteEndResult()
        {
            throw new NotImplementedException("ConsoleLogger does not support WriteEndResult.");
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
