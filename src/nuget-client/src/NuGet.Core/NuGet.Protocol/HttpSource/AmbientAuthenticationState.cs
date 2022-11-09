// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol
{
    /// <summary>
    /// Represents source authentication status per active operation
    /// </summary>
    public class AmbientAuthenticationState
    {
        internal const int MaxAuthRetries = 4;

        public bool IsBlocked { get; private set; } = false;
        public int AuthenticationRetriesCount { get; private set; } = 0;

        public void Block()
        {
            IsBlocked = true;
        }

        public void Increment()
        {
            AuthenticationRetriesCount++;

            if (AuthenticationRetriesCount >= MaxAuthRetries)
            {
                // Block future attempts.
                Block();
            }
        }
    }
}
