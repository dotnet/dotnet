// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetVSExtension
{
    /// <summary>
    /// Result of attempting to create a Copilot tool session.
    /// On success, <see cref="Session"/> is non-null and the caller owns its disposal.
    /// On failure, <see cref="Error"/> indicates what went wrong (no resources to clean up).
    /// </summary>
    internal readonly struct CopilotToolSessionResult
    {
        private CopilotToolSessionResult(CopilotToolSession? session, CopilotToolSessionError error)
        {
            Session = session;
            Error = error;
        }

        public bool IsSuccess => Error == CopilotToolSessionError.None;

        public CopilotToolSessionError Error { get; }

        public CopilotToolSession? Session { get; }

        internal static CopilotToolSessionResult Success(CopilotToolSession session)
            => new(session, CopilotToolSessionError.None);

        internal static CopilotToolSessionResult Failure(CopilotToolSessionError error)
            => new(null, error);
    }
}
