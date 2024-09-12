// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_DESKTOP
using System.Security;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace NuGet.Commands.SignCommand
{
    public interface IPasswordProvider
    {
        // Currently there is no cross platform interactive scenario
#if IS_DESKTOP
        /// <summary>
        /// Requests user to input password and returns it as a SecureString.
        /// </summary>
        /// <param name="filePath">Path to the file that needs a password to open.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>SecureString containing the user input password. The SecureString should be disposed after use.</returns>
        Task<SecureString> GetPassword(string filePath, CancellationToken token);
#endif
    }
}
