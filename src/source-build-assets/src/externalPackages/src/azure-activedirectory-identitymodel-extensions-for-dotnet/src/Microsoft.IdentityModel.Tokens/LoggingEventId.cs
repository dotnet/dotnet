// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    internal static class LoggingEventId
    {
        // TokenValidation EventIds 100+
        internal static readonly EventId TokenValidationFailed = new(100, "TokenValidationFailed");
        internal static readonly EventId TokenValidationSucceeded = new(101, "TokenValidationSucceeded");
    }
}
