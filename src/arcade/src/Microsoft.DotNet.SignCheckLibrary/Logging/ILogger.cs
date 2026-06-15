// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.SignCheck.Verification;

namespace Microsoft.SignCheck.Logging
{
    public interface ILogger
    {
        bool HasLoggedErrors
        {
            get;
        }

        void Close();

        void WriteMessage(string message);

        void WriteMessage(LogVerbosity verbosity, string message);

        void WriteMessage(LogVerbosity verbosity, string message, params object[] values);

        void WriteLine();

        void WriteError(string message);

        void WriteError(LogVerbosity verbosity, string message);

        void WriteError(LogVerbosity verbosity, string message, params object[] values);

        void WriteStartResult(SignatureVerificationResult result, string outcome);

        void WriteEndResult();
    }
}
